using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBufferSorted {
    static Vector2D offset = Vector2D.Zero();

	static List<LightingCollider2D> colliderList;
	#if UNITY_2018_1_OR_NEWER
        static List<LightingTilemapCollider2D> tilemapList;
    #endif

    static LightingManager2D manager;

    static bool drawMask = false;
	static bool drawShadows = false;

    static int layerID = 0;

	static LightingSource2D source2D;
	static float lightSizeSquared;
	static float z;
	static bool maskEffect;

    static Material materialWhite;
	static Material materialBlack;

    static ColliderDepthList list = new ColliderDepthList();
    static ColliderDepth depth;
    static LightingCollider2D collider;    

    static Material material;

    static PartiallyBatched_Collider batch_collider;              
    static PartiallyBatched_Tilemap batch_tilemap;

    #if UNITY_2018_1_OR_NEWER
        static LightingTilemapCollider2D tilemap;
    #endif

    static public void Draw(LightingBuffer2D buffer, LayerSetting layer) {
        // Layer ID
        layerID = layer.GetLayerID();
        if (layerID < 0) {
            return;
        }

        // Calculation Setup
        source2D = buffer.lightSource;
        z = buffer.transform.position.z;
        lightSizeSquared = Mathf.Sqrt(source2D.lightSize * source2D.lightSize + source2D.lightSize * source2D.lightSize);
       
        offset.x = -source2D.transform.position.x;
        offset.y = -source2D.transform.position.y;

        manager = LightingManager2D.Get();
        colliderList = LightingCollider2D.GetList();

		#if UNITY_2018_1_OR_NEWER
			tilemapList = LightingTilemapCollider2D.GetList();
		#endif

        maskEffect = (layer.effect == LightingLayerEffect.InvisibleBellow);

        // Texture Coords
        Coords.Calculate();

        // Materials
        materialWhite = manager.materials.GetWhiteSprite();
        materialBlack = manager.materials.GetBlackSprite();

        // Draw Mask & Shadows?
        drawMask = (layer.type != LightingLayerType.ShadowOnly);
        drawShadows = (layer.type != LightingLayerType.MaskOnly);

        SortObjects(buffer, layer);

        GL.PushMatrix();

        if (manager.lightingSpriteAtlas && SpriteAtlasManager.Get().atlasTexture != null) {
            WithAtlas.Draw(buffer, layer);
        } else {
            WithoutAtlas.Draw(buffer, layer);
        }

        GL.PopMatrix();
    }

    class WithAtlas {

        public static void Draw(LightingBuffer2D buffer, LayerSetting layer) {
            manager.materials.GetAtlasMaterial().SetPass(0);

            GL.Begin(GL.TRIANGLES);
           
            for(int i = 0; i < list.count; i ++) {
                depth = list.list[i];

                switch (depth.type) {
                    case ColliderDepth.Type.Collider:
                        if ((int)depth.collider.lightingCollisionLayer == layerID && drawShadows) {	
                            LightingBufferColliderShape.Shadow(buffer, depth.collider, lightSizeSquared, z, offset);
                        }

                        if ((int)depth.collider.lightingMaskLayer == layerID && drawMask) {
                            LightingBufferColliderSprite.WithAtlas.Mask(buffer, depth.collider, offset, z);

                            LightingBufferColliderShape.Mask(buffer, depth.collider, layer, offset, z);  

                            // Partialy Batched (Depth Edition)
                            if (buffer.partiallyBatchedList_Collider.Count > 0) {
                                GL.End();
                            
                                for(int s = 0; s < buffer.partiallyBatchedList_Collider.Count; s++) {
                                    batch_collider = buffer.partiallyBatchedList_Collider[s];

                                    material = materialWhite;

                                    if (batch_collider.collider2D.maskMode == LightingMaskMode.Invisible || (maskEffect && source2D.transform.position.y > batch_collider.collider2D.transform.position.y)) {
                                        material = materialBlack;
                                    }

                                    LightingBufferColliderSprite.WithoutAtlas.Mask(buffer, batch_collider.collider2D, material, offset, z);
                            
                                }
                                buffer.partiallyBatchedList_Collider.Clear();

                                manager.materials.GetAtlasMaterial().SetPass(0);
                                GL.Begin(GL.TRIANGLES);
                            }
                        }

                    break;

                    #if UNITY_2018_1_OR_NEWER

                        case ColliderDepth.Type.Tile:
                            if ((int)depth.tilemap.lightingCollisionLayer == layerID && drawShadows) {	
                                GL.Color(Color.black);
                                LightingBufferTile.DrawShadow(buffer, depth.tile, depth.polyOffset, depth.tilemap, lightSizeSquared, z);
                            }

                            if ((int)depth.tilemap.lightingMaskLayer == layerID && drawMask) {
                                LightingBufferTile.MaskSpriteDepthWithAtlas(buffer, depth.tile, layer, depth.tilemap, depth.polyOffset, z);

                                // GL.Color(Color.white);
                                // depth.tile.MaskShapeDepthWithAtlas(buffer, order, depth.tilemap, depth.polyOffset, z);

                                // Partialy Batched (Depth Edition)
                                if (buffer.partiallyBatchedList_Tilemap.Count > 0) {
                                    GL.End();

                                    for(int s = 0; s < buffer.partiallyBatchedList_Tilemap.Count; s++) {
                                        batch_tilemap = buffer.partiallyBatchedList_Tilemap[s];
                                        LightingBufferTile.DrawMask(buffer, depth.tile, layer.effect, materialWhite, materialBlack, new Vector2D(batch_tilemap.polyOffset), batch_tilemap.tilemap, lightSizeSquared, z);
                                    }
                                    buffer.partiallyBatchedList_Tilemap.Clear();

                                    manager.materials.GetAtlasMaterial().SetPass(0);
                                    GL.Begin(GL.TRIANGLES);
                                }
                            }   
                        
                        break;

                    #endif

                }
            }
            
            GL.End();
        }
    }

    class WithoutAtlas {
        
        public static void Draw(LightingBuffer2D buffer, LayerSetting layer) {
            for(int i = 0; i < list.count; i ++) {
                depth = list.list[i];

                switch (depth.type) {
                    case ColliderDepth.Type.Collider:
                        if ((int)depth.collider.lightingCollisionLayer == layerID && drawShadows) {	
                            manager.materials.GetAtlasMaterial().SetPass(0);
                            
                            GL.Begin(GL.TRIANGLES);

                                LightingBufferColliderShape.Shadow(buffer, depth.collider, lightSizeSquared, z, offset);

                            GL.End();
                        }

                    if ((int)depth.collider.lightingMaskLayer == layerID && drawMask) {
                            // Masking
                            material = materialWhite;

                            if (depth.collider.maskMode == LightingMaskMode.Invisible || (maskEffect && source2D.transform.position.y > depth.collider.transform.position.y)) {
                                material = materialBlack;
                            }
                            
                            LightingBufferColliderSprite.WithoutAtlas.Mask(buffer, depth.collider, material, offset, z);
                        
                            materialWhite.SetPass(0);

                            GL.Begin(GL.TRIANGLES);

                                GL.Color(Color.white);
                                LightingBufferColliderShape.Mask(buffer, depth.collider, layer, offset, z);
                                
                            GL.End();
                        }

                    break;

                    #if UNITY_2018_1_OR_NEWER

                        case ColliderDepth.Type.Tile:
                            manager.materials.GetAtlasMaterial().SetPass(0);
                                
                            GL.Begin(GL.TRIANGLES);

                                if ((int)depth.tilemap.lightingCollisionLayer == layerID && drawShadows) {	
                                    LightingBufferTile.DrawShadow(buffer, depth.tile, depth.polyOffset, depth.tilemap, lightSizeSquared, z);
                                }

                            GL.End();  

                            if ((int)depth.tilemap.lightingMaskLayer == layerID && drawMask) {
                                // Sprite, But What About Shape?
                                LightingBufferTile.DrawMask(buffer, depth.tile, layer.effect, materialWhite, materialBlack, depth.polyOffset, depth.tilemap, lightSizeSquared, z);
                            } 
                        
                        break;

                    #endif
                }
            }
        }

    }
   
    static public void SortObjects(LightingBuffer2D buffer, LayerSetting layer) {
        list.Reset();

        for(int id = 0; id < colliderList.Count; id++) {
            // Check If It's In Light Area?
            collider = colliderList[id];

            if ((int)colliderList[id].lightingCollisionLayer != layerID && (int)colliderList[id].lightingMaskLayer != layerID) {
				continue;
			}

            if (layer.renderingOrder == LightRenderingOrder.YAxis) {
                list.Add(collider, -collider.transform.position.y);
            } else {
                list.Add(collider, -Vector2.Distance(collider.transform.position, source2D.transform.position));
            }
        }

        #if UNITY_2018_1_OR_NEWER

            for(int id = 0; id < tilemapList.Count; id++) {
                SortTilemap(buffer, tilemapList[id], layer);
            }

        #endif

        list.Sort();
    }

    #if UNITY_2018_1_OR_NEWER

        static public void SortTilemap(LightingBuffer2D buffer, LightingTilemapCollider2D id, LayerSetting layer) {
            //if (id.maskType != LightingTilemapCollider2D.MaskType.Sprite) {
            //	return;
            //}
            
            if (id.map == null) {
                return;
            }

            Vector3 rot = Math2D.GetPitchYawRollRad(id.transform.rotation);

            float rotationYScale = Mathf.Sin(rot.x + Mathf.PI / 2);
            float rotationXScale = Mathf.Sin(rot.y + Mathf.PI / 2);

            float scaleX = id.transform.lossyScale.x * rotationXScale * id.cellSize.x;
            float scaleY = id.transform.lossyScale.y * rotationYScale * id.cellSize.y;

            int sizeInt = LightingBufferTilemapRectangle.LightTilemapSize(id, buffer);

            LightingBufferTilemapRectangle.LightTilemapOffset(id, new Vector2(scaleX, scaleY), buffer);

            Vector2Int newPositionInt = LightingBufferTilemapRectangle.newPositionInt;

            Vector2D tilemapOffset = LightingBufferTilemapRectangle.tilemapOffset;
            Vector2D polyOffset = LightingBufferTilemapRectangle.polyOffset;
            Vector2D inverseOffset = LightingBufferTilemapRectangle.inverseOffset;

            tilemapOffset.x = id.area.position.x + id.cellAnchor.x + id.transform.position.x;
            tilemapOffset.y = id.area.position.y + id.cellAnchor.y + id.transform.position.y;

            VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();

            Vector2 tileSize = new Vector2(scaleX / id.cellSize.x, scaleY / id.cellSize.y);

            LightingTile tile;

            for(int x = newPositionInt.x - sizeInt; x < newPositionInt.x + sizeInt; x++) {
                for(int y = newPositionInt.y - sizeInt; y < newPositionInt.y + sizeInt; y++) {
                    if (x < 0 || y < 0) {
                        continue;
                    }

                    if (x >= id.area.size.x || y >= id.area.size.y) {
                        continue;
                    }

                    tile = id.map[x, y];
                    if (tile == null) {
                        continue;
                    }

                    if (tile.GetOriginalSprite() == null) {
                        return;
                    }

                    polyOffset.x = x + tilemapOffset.x;
                    polyOffset.y = y + tilemapOffset.y;

                    polyOffset.x *= scaleX;
                    polyOffset.y *= scaleY;

                    if (LightingManager2D.culling && Vector2.Distance(polyOffset.ToVector2(), source2D.transform.position) > 2 + source2D.lightSize) {
                        LightingDebug.culled ++;
                        continue;
                    }
                    
                    polyOffset.x += offset.x;
                    polyOffset.y += offset.y;
                
                    //inverseOffset.x = -polyOffset.x;
                    //inverseOffset.y = -polyOffset.y;

                    //if (poly.PointInPoly (inverseOffset)) {
                    //	continue;
                    //}

                    if (layer.renderingOrder == LightRenderingOrder.YAxis) {
                        list.Add(id, tile, -(float)polyOffset.y - (float)offset.y, polyOffset);
                    } else {
                        list.Add(id, tile,  -Vector2.Distance(polyOffset.ToVector2() - offset.ToVector2() , source2D.transform.position), polyOffset);
                    }
                }	
            }
        }
    
    #endif
}