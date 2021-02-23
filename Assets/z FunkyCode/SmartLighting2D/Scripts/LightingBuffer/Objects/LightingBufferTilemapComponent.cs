using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_2018_1_OR_NEWER

public class LightingBufferTilemapComponent {
	static Vector2D zero = Vector2D.Zero();
	static Vector2D vA = Vector2D.Zero(), vB = Vector2D.Zero(), vC = Vector2D.Zero(), vD = Vector2D.Zero();
	static Vector2D pA = Vector2D.Zero(), pB = Vector2D.Zero();

	public static Vector2D offset = Vector2D.Zero();
	public static Vector2D polyOffset = Vector2D.Zero();
	public static Vector2D tilemapOffset = Vector2D.Zero();
	public static Vector2D inverseOffset = Vector2D.Zero();

	static LightingTile tile;
	static Polygon2D poly = null;
	static Pair2D p;
	public static List<Pair2D> pairList;

	static Vector2 tileSize = Vector2.zero;
	static Vector2 scale = Vector2.zero;
	static Vector2 polyOffset2 = Vector2.zero;

	static Sprite penumbraSprite;
	static Rect uvRect = new Rect();
	public static Vector2Int newPositionInt = new Vector2Int();
	static VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();
	static LightingManager2D manager;
	static Vector2D tileSize2 = new Vector2D(1, 1);

	static int sizeInt;

	const float uv0 = 0f;
	const float uv1 = 1f;

	static float uvRectX;
	static float uvRectY;
	static float uvRectWidth;
	static float uvRectHeight;

	static float angleA, angleB;
	static double rot;

	static public void Shadow(LightingBuffer2D buffer, LightingTilemapCollider2D id, float lightSizeSquared, float z) {
		if (id.colliderType != LightingTilemapCollider2D.ColliderType.Collider) {
			return;
		}

		manager = LightingManager2D.Get();
		
		CalculatePenumbra();

		polyOffset.x = -buffer.lightSource.transform.position.x;
		polyOffset.y = -buffer.lightSource.transform.position.y;

		ShadowEdge(buffer, id, lightSizeSquared, z);
		ShadowPolygon(buffer, id, lightSizeSquared, z);
	}

	static public void ShadowPolygon(LightingBuffer2D buffer, LightingTilemapCollider2D id, float lightSizeSquared, float z) {
		for(int i = 0; i < id.polygonColliders.Count; i++) {
			poly = id.polygonColliders[i];

			pairList = Pair2D.GetList(poly.pointsList);

			GL.Color(Color.black);

			for(int x = 0; x < pairList.Count; x++) {
				p = pairList[x];				

				vA.x = p.A.x + polyOffset.x;
				vA.y = p.A.y + polyOffset.y;

				vB.x = p.B.x + polyOffset.x;
				vB.y = p.B.y + polyOffset.y;

				vC.x = p.A.x + polyOffset.x;
				vC.y = p.A.y + polyOffset.y;

				vD.x = p.B.x + polyOffset.x;
				vD.y = p.B.y + polyOffset.y;
				
				rot = System.Math.Atan2 (vA.y - zero.y, vA.x - zero.x);
				vA.x += System.Math.Cos(rot) * lightSizeSquared;
				vA.y += System.Math.Sin(rot) * lightSizeSquared;

				rot = System.Math.Atan2 (vB.y - zero.y, vB.x - zero.x);
				vB.x += System.Math.Cos(rot) * lightSizeSquared;
				vB.y += System.Math.Sin(rot) * lightSizeSquared;

				GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
				GL.Vertex3((float)vC.x, (float)vC.y, z);

				GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
				GL.Vertex3((float)vD.x, (float)vD.y, z);

				GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
				GL.Vertex3((float)vA.x, (float)vA.y, z);


				GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
				GL.Vertex3((float)vA.x, (float)vA.y, z);

				GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
				GL.Vertex3((float)vB.x, (float)vB.y, z);

				GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
				GL.Vertex3((float)vD.x, (float)vD.y, z);
			}

			GL.Color(Color.white);

			for(int x = 0; x < pairList.Count; x++) {
				p = pairList[x];	

				vA.x = p.A.x + polyOffset.x;
				vA.y = p.A.y + polyOffset.y;

				pA.x = p.A.x + polyOffset.x;
				pA.y = p.A.y + polyOffset.y;

				pB.x = p.B.x + polyOffset.x;
				pB.y = p.B.y + polyOffset.y;

				vB.x = p.B.x + polyOffset.x;
				vB.y = p.B.y + polyOffset.y;

				vC.x = p.A.x + polyOffset.x;
				vC.y = p.A.y + polyOffset.y;

				vD.x = p.B.x + polyOffset.x;
				vD.y = p.B.y + polyOffset.y;

				angleA = (float)System.Math.Atan2 (vA.y - zero.y, vA.x - zero.x);
				angleB = (float)System.Math.Atan2 (vB.y - zero.y, vB.x - zero.x);

				vA.x += System.Math.Cos(angleA) * lightSizeSquared;
				vA.y += System.Math.Sin(angleA) * lightSizeSquared;

				vB.x += System.Math.Cos(angleB) * lightSizeSquared;
				vB.y += System.Math.Sin(angleB) * lightSizeSquared;

				rot = angleA - Mathf.Deg2Rad * buffer.lightSource.occlusionSize;
				pA.x += System.Math.Cos(rot) * lightSizeSquared;
				pA.y += System.Math.Sin(rot) * lightSizeSquared;

				rot = angleB + Mathf.Deg2Rad * buffer.lightSource.occlusionSize;
				pB.x += System.Math.Cos(rot) * lightSizeSquared;
				pB.y += System.Math.Sin(rot) * lightSizeSquared;

				GL.TexCoord3(uvRectX, uvRectY, 0);
				GL.Vertex3((float)vC.x,(float)vC.y, z);

				GL.TexCoord3(uvRectWidth, uvRectHeight, 0);
				GL.Vertex3((float)vA.x, (float)vA.y, z);
				
				GL.TexCoord3((float)uvRectX, uvRectHeight, 0);
				GL.Vertex3((float)pA.x,(float)pA.y, z);


				GL.TexCoord3(uvRectX, uvRectY, 0);
				GL.Vertex3((float)vD.x,(float)vD.y, z);

				GL.TexCoord3(uvRectWidth, uvRectY, 0);
				GL.Vertex3((float)vB.x, (float)vB.y, z);
				
				GL.TexCoord3(uvRectX, uvRectHeight, 0);
				GL.Vertex3((float)pB.x, (float)pB.y, z);
			}
		}
	}

	static public void ShadowEdge(LightingBuffer2D buffer, LightingTilemapCollider2D id, float lightSizeSquared, float z) {
		for(int i = 0; i < id.edgeColliders.Count; i++) {
			poly = id.edgeColliders[i];

			pairList = Pair2D.GetList(poly.pointsList, false);
			
			GL.Color(Color.black);

			for(int x = 0; x < pairList.Count; x++) {
				p = pairList[x];				

				vA.x = p.A.x + polyOffset.x;
				vA.y = p.A.y + polyOffset.y;

				vB.x = p.B.x + polyOffset.x;
				vB.y = p.B.y + polyOffset.y;

				vC.x = p.A.x + polyOffset.x;
				vC.y = p.A.y + polyOffset.y;

				vD.x = p.B.x + polyOffset.x;
				vD.y = p.B.y + polyOffset.y;
				
				rot = System.Math.Atan2 (vA.y - zero.y, vA.x - zero.x);
				vA.x += System.Math.Cos(rot) * lightSizeSquared;
				vA.y += System.Math.Sin(rot) * lightSizeSquared;

				rot = System.Math.Atan2 (vB.y - zero.y, vB.x - zero.x);
				vB.x += System.Math.Cos(rot) * lightSizeSquared;
				vB.y += System.Math.Sin(rot) * lightSizeSquared;

				GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
				GL.Vertex3((float)vC.x, (float)vC.y, z);

				GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
				GL.Vertex3((float)vD.x, (float)vD.y, z);

				GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
				GL.Vertex3((float)vA.x, (float)vA.y, z);


				GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
				GL.Vertex3((float)vA.x, (float)vA.y, z);

				GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
				GL.Vertex3((float)vB.x, (float)vB.y, z);

				GL.TexCoord3(Max2DMatrix.c_x, Max2DMatrix.c_y, 0);
				GL.Vertex3((float)vD.x, (float)vD.y, z);
			}

			GL.Color(Color.white);

			for(int x = 0; x < pairList.Count; x++) {
				p = pairList[x];	

				vA.x = p.A.x + polyOffset.x;
				vA.y = p.A.y + polyOffset.y;

				pA.x = p.A.x + polyOffset.x;
				pA.y = p.A.y + polyOffset.y;

				pB.x = p.B.x + polyOffset.x;
				pB.y = p.B.y + polyOffset.y;

				vB.x = p.B.x + polyOffset.x;
				vB.y = p.B.y + polyOffset.y;

				vC.x = p.A.x + polyOffset.x;
				vC.y = p.A.y + polyOffset.y;

				vD.x = p.B.x + polyOffset.x;
				vD.y = p.B.y + polyOffset.y;

				angleA = (float)System.Math.Atan2 (vA.y - zero.y, vA.x - zero.x);
				angleB = (float)System.Math.Atan2 (vB.y - zero.y, vB.x - zero.x);

				vA.x += System.Math.Cos(angleA) * lightSizeSquared;
				vA.y += System.Math.Sin(angleA) * lightSizeSquared;

				vB.x += System.Math.Cos(angleB) * lightSizeSquared;
				vB.y += System.Math.Sin(angleB) * lightSizeSquared;

				rot = angleA - Mathf.Deg2Rad * buffer.lightSource.occlusionSize;
				pA.x += System.Math.Cos(rot) * lightSizeSquared;
				pA.y += System.Math.Sin(rot) * lightSizeSquared;

				rot = angleB + Mathf.Deg2Rad * buffer.lightSource.occlusionSize;
				pB.x += System.Math.Cos(rot) * lightSizeSquared;
				pB.y += System.Math.Sin(rot) * lightSizeSquared;

				GL.TexCoord3(uvRectX, uvRectY, 0);
				GL.Vertex3((float)vC.x,(float)vC.y, z);

				GL.TexCoord3(uvRectWidth, uvRectHeight, 0);
				GL.Vertex3((float)vA.x, (float)vA.y, z);
				
				GL.TexCoord3((float)uvRectX, uvRectHeight, 0);
				GL.Vertex3((float)pA.x,(float)pA.y, z);


				GL.TexCoord3(uvRectX, uvRectY, 0);
				GL.Vertex3((float)vD.x,(float)vD.y, z);

				GL.TexCoord3(uvRectWidth, uvRectY, 0);
				GL.Vertex3((float)vB.x, (float)vB.y, z);
				
				GL.TexCoord3(uvRectX, uvRectHeight, 0);
				GL.Vertex3((float)pB.x, (float)pB.y, z);
			}
		}
	}
   
	public static void CalculatePenumbra() {
		penumbraSprite = manager.materials.GetAtlasPenumbraSprite();

		uvRect.x = penumbraSprite.rect.x / penumbraSprite.texture.width;
		uvRect.y = penumbraSprite.rect.y / penumbraSprite.texture.height;
		uvRect.width = penumbraSprite.rect.width / penumbraSprite.texture.width;
		uvRect.height = penumbraSprite.rect.height / penumbraSprite.texture.height;
		
		uvRect.width += uvRect.x;
		uvRect.height += uvRect.y;

		uvRect.x += 1f / 2048;
		uvRect.y += 1f / 2048;
		uvRect.width -= 1f / 2048;
		uvRect.height -= 1f / 2048;

		uvRectX = uvRect.x;
		uvRectY = uvRect.y;
		uvRectWidth = uvRect.width;
		uvRectHeight = uvRect.height;
	}
}

#endif




		/* 
		foreach(Polygon2D polygon in id.edgeColliders) {
			Vector2D polyOffset = new Vector2D (-buffer.lightSource.transform.position);
		
			poly = polygon.Copy();
			poly.ToOffsetItself(polyOffset);

			foreach (Pair2D p in Pair2D.GetList(poly.pointsList, false)) {
			
			}	
			
			LightingDebug.shadowGenerations ++;
		}

		foreach(Polygon2D polygon in id.polygonColliders) {
			Vector2D polyOffset = new Vector2D (-buffer.lightSource.transform.position);
		
			poly = polygon.Copy();
			poly.ToOffsetItself(polyOffset);

			foreach (Pair2D p in Pair2D.GetList(poly.pointsList)) {
				vA = p.A.Copy();
				vB = p.B.Copy();
				
				vA.Push (Vector2D.Atan2 (vA, zero), lightSizeSquared);
				vB.Push (Vector2D.Atan2 (vB, zero), lightSizeSquared);
				
				Max2DMatrix.DrawTriangle(p.A ,p.B, vA, zero, z);
				Max2DMatrix.DrawTriangle(vA, vB, p.B, zero, z);
			}	
			
			LightingDebug.shadowGenerations ++;
		} */




		

		//LightingDebug.penumbraGenerations ++;