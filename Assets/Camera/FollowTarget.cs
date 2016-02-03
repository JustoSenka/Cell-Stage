using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FollowTarget : MonoBehaviour {

    private Camera cam;
    private Vector3 velocity;
    
    public Transform target;
    public float lerpSpeed = 1.0f;
    public float aboveTarget = 2.0f;

    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

	void Update () 
    {
        Vector3 targetPos = new Vector3(target.position.x, target.position.y + aboveTarget, cam.transform.position.z);
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPos, ref velocity, lerpSpeed);
	}
}
