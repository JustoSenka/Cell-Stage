using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FollowTarget : MonoBehaviour {

    public enum FollowType { Simple, Lerp, SmoothDamp }
    public enum UpdateType { Update, FixedUpdate, LateUpdate, ManualUpdate }

    private Camera cam;
    private Vector3 velocity;

    public UpdateType updateType = UpdateType.FixedUpdate;
    
    public Transform target;
    public FollowType followType = FollowType.SmoothDamp;
    public float lerpSpeed = 0.7f;
    public float aboveTarget = 1.5f;

    public bool lookAtTarget = true;

    public Vector2 rotationAngles = new Vector2(15f, 50f);
    public float rotationSpeed = 0.7f;

    private Vector3 followAngles;
    private Quaternion originalRotation;
    private Vector3 followVelocity;
    private float time = 0;

    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        originalRotation = cam.transform.localRotation;
    }

    private void Update()
    {
        if (updateType == UpdateType.Update)
        {
            FollowPosition();
            LookAtTarget(Time.deltaTime);
        }

        if (updateType == UpdateType.ManualUpdate)
        {
            if (Time.time > time)
            {
                time += 0.033f;
                ManualUpdate();
            }
        }
    }

    private void FixedUpdate()
    {
        if (updateType == UpdateType.FixedUpdate)
        {
            FollowPosition();
            LookAtTarget(Time.deltaTime);
        }
    }


    private void LateUpdate()
    {
        if (updateType == UpdateType.LateUpdate)
        {
            FollowPosition();
            LookAtTarget(Time.deltaTime);
        }
    }


    public void ManualUpdate()
    {
        if (updateType == UpdateType.ManualUpdate)
        {
            FollowPosition();
            LookAtTarget(Time.deltaTime);
        }
    }

    private void FollowPosition()
    {
        if (target != null)
        {
            Vector3 targetPos = new Vector3(target.position.x, target.position.y + aboveTarget, cam.transform.position.z);
            if (followType == FollowType.SmoothDamp) cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPos, ref velocity, lerpSpeed);
            else if (followType == FollowType.Lerp) cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, lerpSpeed / 10);
            else cam.transform.position = targetPos;
        }
    }

    private void LookAtTarget(float deltaTime)
    {
        if (target != null && lookAtTarget)
        {
            // we make initial calculations from the original local rotation
            transform.localRotation = originalRotation;

            // tackle rotation around Y first
            Vector3 localTarget = transform.InverseTransformPoint(target.position);
            float yAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

            yAngle = Mathf.Clamp(yAngle, -rotationAngles.y * 0.5f, rotationAngles.y * 0.5f);
            transform.localRotation = originalRotation * Quaternion.Euler(0, yAngle, 0);

            // then recalculate new local target position for rotation around X
            localTarget = transform.InverseTransformPoint(target.position);
            float xAngle = Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;
            xAngle = Mathf.Clamp(xAngle, -rotationAngles.x * 0.5f, rotationAngles.x * 0.5f);
            var targetAngles = new Vector3(followAngles.x + Mathf.DeltaAngle(followAngles.x, xAngle),
                                            followAngles.y + Mathf.DeltaAngle(followAngles.y, yAngle));

            // smoothly interpolate the current angles to the target angles
            followAngles = Vector3.SmoothDamp(followAngles, targetAngles, ref followVelocity, rotationSpeed);


            // and update the gameobject itself
            transform.localRotation = originalRotation * Quaternion.Euler(-followAngles.x, followAngles.y, 0);
        }
    }
}
