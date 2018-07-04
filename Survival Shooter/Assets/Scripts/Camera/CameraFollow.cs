﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public Transform target; // Camera will follow this position
	public float smoothing = 5f; // Bit of lag to make it easier to see

	private Vector3 offset; // Initial offset from the target

	void Start () {
		// Camera - player position (calculating initial offset)
		offset = transform.position - target.position;
	}

	void FixedUpdate () {
		// Create a position the camera is aiming for based on the offset from the target
		Vector3 targetCamPos = target.position + offset;
		// Smoothly interpolate between camera's current position and it's target position
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}