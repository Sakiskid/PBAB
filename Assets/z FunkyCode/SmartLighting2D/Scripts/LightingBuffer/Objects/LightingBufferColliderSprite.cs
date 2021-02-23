using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingBufferColliderSprite {

	static VirtualSpriteRenderer spriteRenderer = new VirtualSpriteRenderer();
	static Vector2 p = Vector2.zero;
	static Vector2 scale = Vector2.zero;

	public class WithAtlas {

		public static void Mask(LightingBuffer2D buffer, LightingCollider2D id, Vector2D offset, float z) {
			if (id.shape.maskType != LightingCollider2D.MaskType.Sprite) {
				return;
			}

			if (id.isVisibleForLight(buffer) == false) {
				return;
			}

			if (id.shape.GetOriginalSprite() == null || id.spriteRenderer == null) {
				return;
			}

			Sprite sprite = id.shape.GetAtlasSprite();
			if (sprite == null) {
				Sprite reqSprite = SpriteAtlasManager.RequestSprite(id.shape.GetOriginalSprite(), SpriteRequest.Type.WhiteMask);
				if (reqSprite == null) {
					PartiallyBatched_Collider batched = new PartiallyBatched_Collider();

					batched.collider2D = id;

					buffer.partiallyBatchedList_Collider.Add(batched);
					return;
				} else {
					id.shape.SetAtlasSprite(reqSprite);
					sprite = reqSprite;
				}
			}
			
			p.x = id.transform.position.x;
			p.y = id.transform.position.y;

			p.x += (float)offset.x;
			p.y += (float)offset.y;

			scale.x = id.transform.lossyScale.x;
			scale.y = id.transform.lossyScale.y;

			spriteRenderer.sprite = sprite;

			Max2D.DrawSpriteBatched_Tris(spriteRenderer, buffer.lightSource.layerSetting[0], id.maskMode, p, scale, id.transform.rotation.eulerAngles.z, z);
			
			LightingDebug.maskGenerations ++;		
		}
	}

	public class WithoutAtlas {
			
		public static void Mask(LightingBuffer2D buffer, LightingCollider2D id, Material material, Vector2D offset, float z) {
			if (id.shape.maskType != LightingCollider2D.MaskType.Sprite) {
				return;
			}
			
			if (id.isVisibleForLight(buffer) == false) {
				return;
			}

			Sprite sprite = id.shape.GetOriginalSprite();
			if (sprite == null || id.spriteRenderer == null) {
				return;
			}

			p.x = id.transform.position.x;
			p.y = id.transform.position.y;

			p.x += (float)offset.x;
			p.y += (float)offset.y;

			scale.x = id.transform.lossyScale.x;
			scale.y = id.transform.lossyScale.y;

			material.mainTexture = sprite.texture;

			Max2D.DrawSprite(material, id.spriteRenderer, p, scale, id.transform.rotation.eulerAngles.z, z);

			material.mainTexture = null;
			
			LightingDebug.maskGenerations ++;		
		}
	}
}