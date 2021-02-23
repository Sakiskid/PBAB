using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public class LightingBuffer2D : MonoBehaviour {
	public RenderTexture renderTexture;
	public int textureSize = 0;

	public LightingSource2D lightSource;
	public Camera bufferCamera;

	public List<PartiallyBatched_Collider> partiallyBatchedList_Collider = new List<PartiallyBatched_Collider>();
	public List<PartiallyBatched_Tilemap> partiallyBatchedList_Tilemap = new List<PartiallyBatched_Tilemap>();

	public bool free = true;

	public static List<LightingBuffer2D> list = new List<LightingBuffer2D>();

	private Material material;
	private Material additiveMaterial;

	public void OnEnable() {
		list.Add(this);
	}

	public void OnDisable() {
		list.Remove(this);
	}

	static public List<LightingBuffer2D> GetList() {
		return(list);
	}

	static public int GetCount() {
		return(list.Count);
	}

	public void Initiate (int textureSize) {
		SetUpRenderTexture (textureSize);
		SetUpCamera ();
	}

	void SetUpRenderTexture(int _textureSize) {
		textureSize = _textureSize;

		LightingDebug.NewRenderTextures ++;
		
		renderTexture = new RenderTexture(textureSize, textureSize, 16, LightingManager2D.Get().textureFormat);

		name = "Buffer " + GetCount() + " (size: " + textureSize + ")";
	}

	public Material GetMaterial() {
		if (material == null) {
			material = new Material (Shader.Find (Max2D.shaderPath + "Particles/Additive"));
			material.mainTexture = renderTexture;
		}
		return(material);
	}

	public Material GetAdditiveMaterial() {
		if (additiveMaterial == null) {
			additiveMaterial = new Material (Shader.Find (Max2D.shaderPath + "Particles/Additive"));
			additiveMaterial.mainTexture = renderTexture;
		}
		return(additiveMaterial);
	}

	void SetUpCamera() {
		bufferCamera = gameObject.AddComponent<Camera> ();
		bufferCamera.clearFlags = CameraClearFlags.Color;
		bufferCamera.backgroundColor = Color.white;
		bufferCamera.cameraType = CameraType.Game;
		bufferCamera.orthographic = true;
		bufferCamera.targetTexture = renderTexture;
		bufferCamera.farClipPlane = 0.5f;
		bufferCamera.nearClipPlane = 0f;
		bufferCamera.allowHDR = false;
		bufferCamera.allowMSAA = false;
		bufferCamera.enabled = false;
	}

	void LateUpdate() {
		float cameraZ = -1000f;

		Camera camera = LightingManager2D.Get().GetCamera();

		if (camera != null) {
			cameraZ = camera.transform.position.z - 10 - GetCount();
		}

		bufferCamera.transform.position = new Vector3(0, 0, cameraZ);

		transform.rotation = Quaternion.Euler(0, 0, 0);
	}
	
	public void OnRenderObject() {
		if(Camera.current != bufferCamera) {
			return;
		}

		LateUpdate ();

		for (int layerID = 0; layerID < lightSource.layerCount; layerID++) {
			if (lightSource.layerSetting == null || lightSource.layerSetting.Length <= layerID) {
				continue;
			}

			LayerSetting layerSetting = lightSource.layerSetting[layerID];

			if (layerSetting == null) {
				continue;
			}

			if (lightSource.enableCollisions) {	
				if (layerSetting.renderingOrder == LightRenderingOrder.Default) {
					Default.Draw(this, layerSetting);
				} else {
					Sorted.Draw(this, layerSetting);
				}
			}
		}
	
		LightingSourceTexture.Draw(this);

		LightingDebug.LightBufferUpdates ++;
		LightingDebug.totalLightUpdates ++;

		bufferCamera.enabled = false;
	}

	class Default {
		static public void Draw(LightingBuffer2D buffer, LayerSetting layer) {
			LightingBufferDefault.Draw(buffer, layer);
		}
	}

	class Sorted {
		static public void Draw(LightingBuffer2D buffer, LayerSetting layer) {
			LightingBufferSorted.Draw(buffer, layer);
		}
	}	
}