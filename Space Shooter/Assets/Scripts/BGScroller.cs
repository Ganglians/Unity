using UnityEngine;
using System.Collections;

public class BGScroller : MonoBehaviour {
	public float scrollSpeed;
	public float tileSizeZ;

	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position; 
	}
	
	// Update is called once per frame
	void Update () {
		float newPosition = Mathf.Repeat (Time.time * scrollSpeed, tileSizeZ);
		//Vector3.forward shorthand for <0x, 0y, 1z> 
		transform.position = startPosition + Vector3.forward * newPosition; 
	}
}
