using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

	public float smoothing;

	private Vector2 origin;
	private Vector2 direction;
	private Vector2 smoothDirection;
	private bool touched;
	private int pointerID;

	void Awake () {
		direction = Vector2.zero;
		touched = false;
	}

	public void OnPointerDown (PointerEventData data) {
		// Set our start point
		if (!touched) {
			touched = true;
			pointerID = data.pointerId;
			origin = data.position; // Origin is set as point where touchpad originally touched
		}
	}

	public void OnDrag (PointerEventData data) {
		// Compare the difference between our start point and our current pointer position
		if (data.pointerId == pointerID) {
			Vector2 currentPosition = data.position;
			Vector2 directionRaw = currentPosition - origin;
			direction = directionRaw.normalized;
		}
//		Debug.Log (direction);

	}

	public void OnPointerUp (PointerEventData data) {
		// Reset everything
		if (data.pointerId == pointerID) {
			direction = Vector2.zero;
			touched = false;
		}
	}

	public Vector2 GetDirection () {
		// Allows us to grab the direction from the player
		smoothDirection = Vector2.MoveTowards (smoothDirection, direction, smoothing);
		return smoothDirection;
	}
}
