using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	private Rigidbody rb;

	public float amount = 50f;

	void Start () {
		rb = gameObject.GetComponent<Rigidbody> ();
	}

	void FixedUpdate ()
	{
		float h = Input.GetAxis("Horizontal") * amount * Time.deltaTime;
		float v = Input.GetAxis("Vertical") * amount * Time.deltaTime;

		Debug.Log (h + " " + v);

		rb.AddForce(transform.up * h);
		rb.AddForce(transform.right * v);
	}
}