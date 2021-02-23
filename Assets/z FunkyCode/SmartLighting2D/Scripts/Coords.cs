using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coords : MonoBehaviour {
   	static public void Calculate() {
		Sprite fillSprite = LightingManager2D.Get().materials.GetAtlasWhiteMaskSprite();

		if (fillSprite != null) {
			Rect uvRect = new Rect((float)fillSprite.rect.x / fillSprite.texture.width, (float)fillSprite.rect.y / fillSprite.texture.height, (float)fillSprite.rect.width / fillSprite.texture.width , (float)fillSprite.rect.height / fillSprite.texture.height);
			uvRect.x += uvRect.width / 2;
			uvRect.y += uvRect.height / 2;
			
			Max2DMatrix.c_x = uvRect.x;
			Max2DMatrix.c_y = uvRect.y;
		}
	}
}
