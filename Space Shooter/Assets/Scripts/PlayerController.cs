using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
	public float speed;
	public Boundary boundary;
	public float tilt;

	public Transform shotSpawn;
	public GameObject shot;

	private Rigidbody rb;
	private AudioSource aus;

	public float fireRate;
	private float nextFire;

	private Quaternion calibrationQuaternion;

	public SimpleTouchPad touchPad;
	public SimpleTouchAreaButton areaButton;

	void Update () {
		if (areaButton.CanFire () && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			aus.Play ();
		}
	}

	void Start () {
		rb = GetComponent <Rigidbody> ();
		aus = GetComponent <AudioSource> ();
	}

//	void FixedUpdate() {
//		float moveHorizontal = Input.GetAxis ("Horizontal");
//		float moveVertical = Input.GetAxis ("Vertical");
//
//		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
//		rb.velocity = movement * speed;
//
//		rb.position = new Vector3 (
//			Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax), 
//			0.0f, 
//			Mathf.Clamp (rb.position.z, boundary.zMin, boundary.zMax)
//		);
//
//		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
//	}

	void FixedUpdate() {
//		float moveHorizontal = Input.GetAxis ("Horizontal");
//		float moveVertical = Input.GetAxis ("Vertical");

		/* Vector3 acceleration = Input.acceleration; // Looking at the accelerometer
		 * x, 0, y because of the difference of device's orientation
		 */
//		   Vector3 accelerationRaw = Input.acceleration;
//		   Vector3 acceleration = FixedAcceleration (accelerationRaw);
//		   Vector3 movement = new Vector3 (acceleration.x, 0.0f, acceleration.y); 
		Vector2 direction = touchPad.GetDirection ();
		Vector3 movement = new Vector3 (direction.x, 0.0f, direction.y); 

		rb.velocity = movement * speed;
		rb.position = new Vector3 (
			Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (rb.position.z, boundary.zMin, boundary.zMax)
		);
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}

	void CalibrateAccelerometer () {
		/* Takes a Vector3 snapshot of the input acceleration */
		Vector3 accelerationSnapshot = Input.acceleration;
		/* Create a Quaternion from that acceleration snapshot */
		Quaternion rotateQuaternion = Quaternion.FromToRotation (new Vector3 (0.0f, 0.0f, -1.0f), accelerationSnapshot);
		/* Invert it so that we have a calibration quaternion to take that rotation from */
		calibrationQuaternion = Quaternion.Inverse (rotateQuaternion);
	}

	Vector3 FixedAcceleration (Vector3 acceleration) {
		Vector3 fixedAcceleration = calibrationQuaternion * acceleration;

		return fixedAcceleration;
	}
}