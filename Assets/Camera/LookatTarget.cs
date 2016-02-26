using System;
using UnityEngine;

namespace UnityStandardAssets.Cameras
{
    public class LookatTarget : AbstractTargetFollower
    {
        [SerializeField] private Vector2 m_RotationRange;
        [SerializeField] private float m_FollowSpeed = 1;

        private Vector3 m_FollowAngles;
        private Quaternion m_OriginalRotation;

        protected Vector3 m_FollowVelocity;


        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            m_OriginalRotation = transform.localRotation;
        }


        protected override void FollowTarget(float deltaTime)
        {
            if (targetRigidbody != null)
            {

                // we make initial calculations from the original local rotation
                transform.localRotation = m_OriginalRotation;

                // tackle rotation around Y first
                Vector3 localTarget = transform.InverseTransformPoint(m_Target.position);
                float yAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

                yAngle = Mathf.Clamp(yAngle, -m_RotationRange.y * 0.5f, m_RotationRange.y * 0.5f);
                transform.localRotation = m_OriginalRotation * Quaternion.Euler(0, yAngle, 0);

                // then recalculate new local target position for rotation around X
                localTarget = transform.InverseTransformPoint(m_Target.position);
                float xAngle = Mathf.Atan2(localTarget.y, localTarget.z) * Mathf.Rad2Deg;
                xAngle = Mathf.Clamp(xAngle, -m_RotationRange.x * 0.5f, m_RotationRange.x * 0.5f);
                var targetAngles = new Vector3(m_FollowAngles.x + Mathf.DeltaAngle(m_FollowAngles.x, xAngle),
                                               m_FollowAngles.y + Mathf.DeltaAngle(m_FollowAngles.y, yAngle));

                // smoothly interpolate the current angles to the target angles
                m_FollowAngles = Vector3.SmoothDamp(m_FollowAngles, targetAngles, ref m_FollowVelocity, m_FollowSpeed);


                // and update the gameobject itself
                transform.localRotation = m_OriginalRotation * Quaternion.Euler(-m_FollowAngles.x, m_FollowAngles.y, 0);
            }
        }
    }
}
