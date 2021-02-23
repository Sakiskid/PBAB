using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeshVertices {
	public List<MeshVertice> list = new List<MeshVertice>();
}

[System.Serializable]
public class MeshVertice {
	public Vector2D a;
	public Vector2D b;
	public Vector2D c;
}

[System.Serializable]
public class LightingShape {
	public List<List<Pair2D>> polygons_world_pair = null;
	public List<List<Pair2D>> polygons_world_pair_cache = null;
	
	public List<Polygon2D> polygons_world = null;
	public List<Polygon2D> polygons_world_cache = null;

	public List<Polygon2D> polygons_local = null;
	public float meshDistance = -1f;
	public Mesh mesh = null;
	public MeshVertices mesh_vertices = null;

	public void ResetLocal() {
		polygons_local = null;
		meshDistance = -1f;
		mesh = null;
	}

	public void ResetWorld() {
		polygons_world_pair = null;
		polygons_world = null;
		mesh_vertices = null;
	}
}

[System.Serializable]
public class LightingColliderShape {
	public LightingCollider2D.ColliderType colliderType = LightingCollider2D.ColliderType.Collider;
	public LightingCollider2D.MaskType maskType = LightingCollider2D.MaskType.Sprite;

	private LightingShape colliderShape = new LightingShape();
	private LightingShape spriteShape = new LightingShape();

	private CustomPhysicsShape customPhysicsShape = null;

	public CustomPhysicsShape GetPhysicsShape() {
		if (customPhysicsShape == null) {
			customPhysicsShape = SpriteAtlasManager.RequesCustomShape(originalSprite);
		}
		return(customPhysicsShape);
	}

	private Sprite originalSprite;
	private Sprite atlasSprite;

	public Sprite GetOriginalSprite() {
		return(originalSprite);
	}

	public Sprite GetAtlasSprite() {
		return(atlasSprite);
	}

	public void SetAtlasSprite(Sprite sprite) {
		atlasSprite = sprite;
	}

	public void SetOriginalSprite(Sprite sprite) {
		originalSprite = sprite;
	}

	public void ResetLocal() {
		colliderShape.ResetLocal();

		spriteShape.ResetLocal();

		customPhysicsShape = null;

		ResetWorld();
	}

	public void ResetWorld() {
		colliderShape.ResetWorld();
		spriteShape.ResetWorld();
	}

	//////////////////////////////////////////////////////////////////////////////
	
	public float GetFrustumDistance(Transform transform) {
		switch(colliderType) {
			case LightingCollider2D.ColliderType.Collider:
				return(GetFrustumDistance_Collider(transform));
			case LightingCollider2D.ColliderType.SpriteCustomPhysicsShape:
				return(GetFrustumDistance_Shape());
		}
		return(1000f);
	}
	
	public float GetFrustumDistance_Collider(Transform transform) {
		if (colliderShape.meshDistance < 0) {
			colliderShape.meshDistance = 0;
			if (GetPolygons_Collider_Local(transform).Count > 0) {
				foreach (Vector2D id in GetPolygons_Collider_Local(transform)[0].pointsList) {
					colliderShape.meshDistance = Mathf.Max(colliderShape.meshDistance, Vector2.Distance(id.ToVector2(), Vector2.zero));
				}
			}
		}
		return(colliderShape.meshDistance);
	}

	public float GetFrustumDistance_Shape() {
		if (spriteShape.meshDistance < 0) {
			spriteShape.meshDistance = 0;
			if (GetPolygons_Shape_Local().Count > 0) {
				foreach (Vector2D id in GetPolygons_Shape_Local()[0].pointsList) {
					spriteShape.meshDistance = Mathf.Max(spriteShape.meshDistance, Vector2.Distance(id.ToVector2(), Vector2.zero));
				}
			}
		}
		return(spriteShape.meshDistance);
	}

	//////////////////////////////////////////////////////////////////////////////// Pair List 
	public List<List<Pair2D>> GetPolygons_Pair_World_ColliderType(Transform transform, VirtualSpriteRenderer spriteRenderer) {
		switch(colliderType) {
			case LightingCollider2D.ColliderType.Collider:
				return(GetPolygons_Pair_Collider_World(transform));

			case LightingCollider2D.ColliderType.SpriteCustomPhysicsShape:
				return(GetPolygons_Pair_Shape_World(transform, spriteRenderer));
		}
		return(null);
	}

	public List<List<Pair2D>> GetPolygons_Pair_Collider_World(Transform transform) {
		if (colliderShape.polygons_world_pair == null) {
			if (colliderShape.polygons_world_pair_cache != null) {
				colliderShape.polygons_world_pair = colliderShape.polygons_world_pair_cache;
				
				// Recalculate Cache !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

			} else {
				LightingDebug.ShadowColliderTotalGenerationsWorld_collider_pair ++;

				colliderShape.polygons_world_pair = new List<List<Pair2D>>();
				foreach(Polygon2D poly in GetPolygons_Collider_World(transform)) {
					colliderShape.polygons_world_pair.Add(Pair2D.GetList(poly.pointsList));
				}

				colliderShape.polygons_world_pair_cache = colliderShape.polygons_world_pair;
			}
		}

		return(colliderShape.polygons_world_pair);
	}

	public List<List<Pair2D>> GetPolygons_Pair_Shape_World(Transform transform, VirtualSpriteRenderer spriteRenderer) {
		if (spriteShape.polygons_world_pair == null) {
			
			if (spriteShape.polygons_world_pair_cache != null) {
				GetPolygons_Shape_World(transform, spriteRenderer);					
			}

			if (spriteShape.polygons_world_pair_cache != null) {
				spriteShape.polygons_world_pair = spriteShape.polygons_world_pair_cache;
		
				
			} else {
				spriteShape.polygons_world_pair = new List<List<Pair2D>>();

				foreach(Polygon2D poly in GetPolygons_Shape_World(transform, spriteRenderer)) {
					spriteShape.polygons_world_pair.Add(Pair2D.GetList(poly.pointsList));
				}

				spriteShape.polygons_world_pair_cache = spriteShape.polygons_world_pair;
			}
		}

		return(spriteShape.polygons_world_pair);
	}

	/////////////////////////////////////////// Mesh Object
	public Mesh GetMesh_MaskType(Transform transform) {
		switch(maskType) {
			case LightingCollider2D.MaskType.Collider:
				return(GetMesh_Collider(transform));

			case LightingCollider2D.MaskType.SpriteCustomPhysicsShape:
				return(GetMesh_Shape());
		}
		return(null);
	}
	
	public Mesh GetMesh_Collider(Transform transform) {
       if (colliderShape.mesh == null) {
			if (GetPolygons_Collider_Local(transform).Count > 0) {
				if (GetPolygons_Collider_Local(transform)[0].pointsList.Count > 2) {
					// Triangulate Polygon List?
					colliderShape.mesh = PolygonTriangulator2D.Triangulate (GetPolygons_Collider_Local(transform)[0], Vector2.zero, Vector2.zero, PolygonTriangulator2D.Triangulation.Advanced);
				}
			}
		}
		return(colliderShape.mesh);
    }

	public Mesh GetMesh_Shape() {
        if (spriteShape.mesh == null) {
            if (GetPolygons_Shape_Local().Count > 0) {
                if (GetPolygons_Shape_Local()[0].pointsList.Count > 2) {
                    // Triangulate Polygon List?
                    spriteShape.mesh = GetPhysicsShape().GetShapeMesh();
                }
            }
           LightingDebug.ConvexHullGenerations ++;
        }
        return(spriteShape.mesh);
    }

	//////////////////////////////////////////// Mesh Vertices
	public MeshVertices GetMesh_Vertices_MaskType(Transform transform) {
		switch(maskType) {
			case LightingCollider2D.MaskType.Collider:
				return(GetMesh_Vertices_Collider(transform));

			case LightingCollider2D.MaskType.SpriteCustomPhysicsShape:
				return(GetMesh_Vertices_Shape(transform));
		}
		return(null);
	}
	
	public MeshVertices GetMesh_Vertices_Collider(Transform transform) {
       if (colliderShape.mesh_vertices == null) {
			Mesh mesh = GetMesh_Collider(transform);

			colliderShape.mesh_vertices = new MeshVertices();

			if (mesh != null) {
				MeshVertice vertice;

				int triangleCount = mesh.triangles.GetLength (0);

				for (int i = 0; i < triangleCount; i = i + 3) {
					vertice = new MeshVertice();
					vertice.a = new Vector2D(transform.TransformPoint(mesh.vertices [mesh.triangles [i]]));
					vertice.b = new Vector2D(transform.TransformPoint(mesh.vertices [mesh.triangles [i + 1]]));
					vertice.c = new Vector2D(transform.TransformPoint(mesh.vertices [mesh.triangles [i + 2]]));

					colliderShape.mesh_vertices.list.Add(vertice);
				}
			}	
		}
		return(colliderShape.mesh_vertices);
    }

	public MeshVertices GetMesh_Vertices_Shape(Transform transform) {
        if (spriteShape.mesh_vertices == null) {
          	Mesh mesh = GetMesh_Shape();

			spriteShape.mesh_vertices = new MeshVertices();

			if (mesh != null) {
				MeshVertice vertice;

				int triangleCount = mesh.triangles.GetLength (0);

				for (int i = 0; i < triangleCount; i = i + 3) {
					vertice = new MeshVertice();
					vertice.a = new Vector2D(transform.TransformPoint(mesh.vertices [mesh.triangles [i]]));
					vertice.b = new Vector2D(transform.TransformPoint(mesh.vertices [mesh.triangles [i + 1]]));
					vertice.c = new Vector2D(transform.TransformPoint(mesh.vertices [mesh.triangles [i + 2]]));

					spriteShape.mesh_vertices.list.Add(vertice);
				}
			}
        }
        return(spriteShape.mesh_vertices);
    }

	//////////////////////////////////////////// Polygon Objects
	public List<Polygon2D> GetPolygons_World_ColliderType(Transform transform, VirtualSpriteRenderer virtualSpriteRenderer) {
		switch(colliderType) {
			case LightingCollider2D.ColliderType.Collider:
				return(GetPolygons_Collider_World(transform));

			case LightingCollider2D.ColliderType.SpriteCustomPhysicsShape:
				return(GetPolygons_Shape_World(transform, virtualSpriteRenderer));
		}
		return(null);
	}

	public List<Polygon2D> GetPolygons_Local_ColliderType(Transform transform) {
		switch(colliderType) {
			case LightingCollider2D.ColliderType.Collider:
				return(GetPolygons_Collider_Local(transform));

			case LightingCollider2D.ColliderType.SpriteCustomPhysicsShape:
				return(GetPolygons_Shape_Local());
		}
		return(null);
	}

	public List<Polygon2D> GetPolygons_Collider_World(Transform transform) {
		if (colliderShape.polygons_world == null) {
			if (colliderShape.polygons_world_cache != null) {
				LightingDebug.ShadowColliderTotalGenerationsWorld_collider_re ++;
				
				colliderShape.polygons_world = colliderShape.polygons_world_cache;

				Polygon2D poly;
				Vector2D point;
				List<Polygon2D> list = GetPolygons_Collider_Local(transform);
				for(int i = 0; i < list.Count; i++) {
					poly = list[i];
					for(int p = 0; p < poly.pointsList.Count; p++) {
						point = poly.pointsList[p];
						
						colliderShape.polygons_world[i].pointsList[p].x = point.x;
						colliderShape.polygons_world[i].pointsList[p].y = point.y;
					}
					colliderShape.polygons_world[i].ToWorldSpaceItself(transform);
				}

			} else {
				LightingDebug.ShadowColliderTotalGenerationsWorld_collider ++;

				colliderShape.polygons_world = new List<Polygon2D>();
				
				foreach(Polygon2D poly in GetPolygons_Collider_Local(transform)) {
					colliderShape.polygons_world.Add(poly.ToWorldSpace(transform));
				}

				 colliderShape.polygons_world_cache = colliderShape.polygons_world;
			}
		}

		return(colliderShape.polygons_world);
	}

	public List<Polygon2D> GetPolygons_Shape_World(Transform transform, VirtualSpriteRenderer virtualSpriteRenderer) {
		if (spriteShape.polygons_world == null) {

			Vector2 scale = new Vector2();
			List<Polygon2D> list = GetPolygons_Shape_Local();

			if (spriteShape.polygons_world_cache != null) {				
				if (list.Count != spriteShape.polygons_world_cache.Count) {
					spriteShape.polygons_world_cache = null;
					spriteShape.polygons_world_pair_cache = null;

				} else {
					for(int i = 0; i < list.Count; i++) {
						if (list[i].pointsList.Count != spriteShape.polygons_world_cache[i].pointsList.Count) {
							spriteShape.polygons_world_cache = null;
							spriteShape.polygons_world_pair_cache = null;

							break;
						}
					}
				}
			}
		
			if (spriteShape.polygons_world_cache != null) {
				LightingDebug.ShadowColliderTotalGenerationsWorld_shape_re ++;

				spriteShape.polygons_world = spriteShape.polygons_world_cache;

				Polygon2D poly;
				Vector2D point;

				for(int i = 0; i < list.Count; i++) {
					poly = list[i];
					for(int p = 0; p < poly.pointsList.Count; p++) {
						point = poly.pointsList[p];
						
						spriteShape.polygons_world[i].pointsList[p].x = point.x;
						spriteShape.polygons_world[i].pointsList[p].y = point.y;
					}

					if (virtualSpriteRenderer != null) {
						scale.x = 1;
						scale.y = 1;

						if (virtualSpriteRenderer.flipX == true) {
							scale.x = -1;
						}

						if (virtualSpriteRenderer.flipY == true) {
							scale.y = -1;
						}
						
						if (virtualSpriteRenderer.flipX != false || virtualSpriteRenderer.flipY != false) {
							spriteShape.polygons_world[i].ToScaleItself(scale);
						}
					}

					spriteShape.polygons_world[i].ToWorldSpaceItself(transform);
				}
			} else {
				LightingDebug.ShadowColliderTotalGenerationsWorld_shape ++;

				Polygon2D polygon;

				spriteShape.polygons_world = new List<Polygon2D>();

				foreach(Polygon2D poly in list) {
					polygon = poly.Copy();

					if (virtualSpriteRenderer != null) {
						scale.x = 1;
						scale.y = 1;

						if (virtualSpriteRenderer.flipX == true) {
							scale.x = -1;
						}

						if (virtualSpriteRenderer.flipY == true) {
							scale.y = -1;
						}
						
						if (virtualSpriteRenderer.flipX != false || virtualSpriteRenderer.flipY != false) {
							polygon.ToScaleItself(scale);
						}
					}
					
					polygon.ToWorldSpaceItself(transform);

					spriteShape.polygons_world.Add(polygon);

					spriteShape.polygons_world_cache = spriteShape.polygons_world;
				}
			}
		}

		return(spriteShape.polygons_world);
	}

	public List<Polygon2D> GetPolygons_Collider_Local(Transform transform) {
		if (colliderShape.polygons_local == null) {
			LightingDebug.ShadowColliderTotalGenerationsLocal_collider ++;
			
			colliderShape.polygons_local = Polygon2DList.CreateFromGameObject (transform.gameObject);
			if (colliderShape.polygons_local.Count > 0) {

			} else {
				Debug.LogWarning("SmartLighting2D: LightingCollider2D object is missing Collider2D Component", transform.gameObject);
			}
		}
		return(colliderShape.polygons_local);
	}

    public List<Polygon2D> GetPolygons_Shape_Local() {
		if (spriteShape.polygons_local == null) {
			LightingDebug.ShadowColliderTotalGenerationsLocal_shape ++;

			spriteShape.polygons_local = new List<Polygon2D>();

			#if UNITY_2018_1_OR_NEWER

				if (customPhysicsShape == null) {
					if (originalSprite == null) {
						return(spriteShape.polygons_local);
					}

					customPhysicsShape = GetPhysicsShape();

					spriteShape.polygons_local = customPhysicsShape.GetShape();
				}

			#endif
	
		}
		return(spriteShape.polygons_local);
	}
}