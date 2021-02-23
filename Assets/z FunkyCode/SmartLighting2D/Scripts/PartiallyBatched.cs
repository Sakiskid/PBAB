using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartiallyBatched_Tilemap {
	public VirtualSpriteRenderer virtualSpriteRenderer;
	public Vector2 polyOffset; 
	public Vector2 tileSize;

	#if UNITY_2018_1_OR_NEWER
		public LightingTilemapCollider2D tilemap;
	#endif
}

public class PartiallyBatched_Collider {
	public LightingCollider2D collider2D;
}
