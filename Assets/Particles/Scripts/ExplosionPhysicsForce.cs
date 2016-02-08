using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Effects
{
    public class ExplosionPhysicsForce : MonoBehaviour
    {
        public float explosionForce = 4;
        public float explosionDamage = 4;
        public float range = 10;

        void Start(){

        }

        // Message receiver from bomb
        public void Play()
        {
            var systems = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem system in systems)
            {
                system.Play();
            }

            StartCoroutine(MimicExplode());
        }

        private IEnumerator MimicExplode()
        {
            // wait one frame because some explosions instantiate debris which should then
            // be pushed by physics force
            yield return null;

            float multiplier = GetComponent<ParticleSystemMultiplier>().multiplier;

            float r = range * multiplier;
            var cols = Physics.OverlapSphere(transform.position, r);
            var rigidbodies = new List<Rigidbody>();
            foreach (var col in cols)
            {
                if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
                {
                    rigidbodies.Add(col.attachedRigidbody);
                }
            }
            foreach (var rb in rigidbodies)
            {
                rb.AddExplosionForce(explosionForce*multiplier, transform.position, r, 1*multiplier, ForceMode.Impulse);
                AlterHealth(rb.gameObject);
            }
        }

        private void AlterHealth(GameObject go)
        {
            float multiplier = GetComponent<ParticleSystemMultiplier>().multiplier;
            short damage = (short)((range * multiplier - Vector3.Distance(transform.position, go.transform.position)) * explosionDamage);
            damage = (short) -damage;
            go.SendMessage("CmdAlterHealth", damage, SendMessageOptions.DontRequireReceiver);
        }
    }
}
