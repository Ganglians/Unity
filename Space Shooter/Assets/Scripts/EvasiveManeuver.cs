using UnityEngine;
using System.Collections;

public class EvasiveManeuver : MonoBehaviour {
	public float dodge;
	public float smoothing;
	public float tilt;
	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;
	public Boundary boundary;

	private float currentSpeed;
	private float targetManeuver;
	private Rigidbody rb;

	void Start () {
		rb = GetComponent <Rigidbody> ();
		currentSpeed = rb.velocity.z; // Keep z velocity constant
		StartCoroutine (Evade ());
	}

	IEnumerator Evade () { // IEnum because coroutine
		// No evading right away, waiting coroutine
		yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));

		while (true) { //zigzag motion after waiting for maneuverTime seconds
			targetManeuver = Random.Range (1, dodge) * -Mathf.Sign (transform.position.x);
			yield return new WaitForSeconds (Random.Range (maneuverTime.x, maneuverTime.y));
			targetManeuver = 0;
			yield return new WaitForSeconds (Random.Range (maneuverWait.x, maneuverWait.y));
		}
	}

	void FixedUpdate () { // Where enemy ship is actually moved
		/* 
		 * Moving rb's x velocity towards targetManeuver  and last var says how fast we're going to do this 
		 * Time.deltatime is time it takes frame to render so multiply by smoothing to have a little control over
		 * how long it takes
		 */
		float newManeuver = Mathf.MoveTowards (rb.velocity.x, targetManeuver, Time.deltaTime * smoothing); 
		rb.velocity = new Vector3 (newManeuver, 0.0f, currentSpeed);
		// Make sure still in center for y
		rb.position = new Vector3 (
			Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp (rb.position.z, boundary.zMin, boundary.zMax)
		);
		 
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}
