using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField] float followSpeed;
    [SerializeField] float zoomInSpeed;
    [SerializeField] float zoomOutSpeed;
    [SerializeField] float minZoom = 8;
    [SerializeField] float maxZoom = 16;
    [SerializeField] float zoomRange = 8;
    PlayerController player;
    Camera camera;

    float originalZoom;
       	
	void Start () {
        player = FindObjectOfType<PlayerController>();
        camera = GetComponent<Camera>();
        originalZoom = camera.orthographicSize;
	}
	
	void Update () {
        FollowPlayer();
        HandleZoom();
	}

    private void HandleZoom()
    {
        if (GetPlayerDistance() > zoomRange)
        {
            // Zoom out
            float step = Time.deltaTime * zoomOutSpeed;
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, maxZoom, step);
        }
        else
        {
            // Zoom in
            float step = Time.deltaTime * zoomInSpeed;
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, minZoom, step);
        }
    }

    private void FollowPlayer()
    {
        Vector3 playerPos = player.transform.position;

        float step = followSpeed * Time.deltaTime * GetPlayerDistance();
        Vector2 newPos = Vector2.Lerp(transform.position, playerPos, step);
        transform.position = new Vector3 (newPos.x, newPos.y, transform.position.z);
    }

    private float GetPlayerDistance()
    {
        float xDistanceToPlayer = (player.transform.position.x - transform.position.x);
        float yDistanceToPlayer = (player.transform.position.y - transform.position.y);
        float distanceToPlayer = new Vector2(xDistanceToPlayer, yDistanceToPlayer).magnitude;
        return distanceToPlayer;
    }
}
