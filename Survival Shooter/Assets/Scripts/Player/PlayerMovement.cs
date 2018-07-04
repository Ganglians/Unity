using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;

	Vector3 movement; // Store movement we want to apply to player
	Animator anim; // Reference to the animator component
	Rigidbody playerRigidbody;
	int floorMask; // using int as a layer mask to tell raycast to only hit floor
	float camRayLength = 100f;

	// Awake and FixedUpdate are automatically called by unity (monobehavior functions)

	// Is called regardless of enabled/disabled script (Good for setting up references)
	void Awake () {
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
	}

	// Activates every physics update (since moving the character using a rigidbody)
	void FixedUpdate () {
		// Normal axes would have value between -1 and 1
		// Raw will only have values [ -1, 0, 1 ]
		// Character moves immediately, does not accelerate slowly (more responsive feel)
		float h = Input.GetAxisRaw ("Horizontal"); // "Horizontal" is a default value mapped in unity
		float v = Input.GetAxisRaw ("Vertical"); // Same with "Vertical"

		Move (h, v);
		Turning ();
		Animating (h, v);
	}

	void Move (float h, float v) {
		movement.Set(h, 0.0f, v);
		// Since h and v are both 1, moving diagonally would make movement vector be 1.4 but want it to be 1
		movement = movement.normalized; 
		// * Time.deltaTime else it would move at 6 units per fixed update (6 units per 1/50th of a second)
		// Multiplying by Time.deltaTime changes movement to per second
		movement = movement * speed * Time.deltaTime; 
		// Apply movement to the player (transform.position = current relative position)
		playerRigidbody.MovePosition (transform.position + movement);
	}

	// Position will be based on mouse input
	void Turning () {
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit floorHit;

		// Raycast returns true if it hits something
		// out floorHit: going to get info out of function and store info in floorHit
		// camRayLength: how far we're going to cast ray for
		// Finally, make sure ray cast is only hitting things on the floor layer
		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			// Create a vector from player (transform.position) to where mouse has hit (floorHit.point)
			Vector3 playerToMouse = floorHit.point - transform.position;
			// Want character to turn but not lean back/forward (disorienting)
			playerToMouse.y = 0f;
			// A quaternion is a way to store rotation
			// Most characters/cameras in most 3D modeling use Z axis as their forward vector (playerTomouse vector is 
			// assigned role of forward vector in this case)
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			// Giving a completely new rotation (i.e. no transform.rotation + newRotation stuff)
			playerRigidbody.MoveRotation (newRotation);
		}
	}

	void Animating (float h, float v) {
		// If either axis has input then character walks
		bool walking = h != 0f || v != 0f; 
		// Parse this to our animator component
		anim.SetBool ("IsWalking", walking);
	}
}
