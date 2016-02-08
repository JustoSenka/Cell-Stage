using UnityEngine;
using System;
using System.Linq;
using System.Collections;

namespace CellStage
{
    public static class Extention
    {

        public static void DisableMeshes(this MeshRenderer[] array)
        {
            foreach (var r in array)
            {
                r.enabled = false;
            }
        }

        public static void DisableColliders<T>(this T[] array) where T : Collider
        {
            foreach (var r in array)
            {
                r.enabled = false;
            }
        }

        public static void SetActiveChildrenIfContaints(this GameObject go, string name, bool active)
        {
            var array = go.GetComponentsInChildren<Transform>().Where((x) => x.name.Contains(name));
            foreach (var r in array)
            {
                r.gameObject.SetActive(active);
            }
        }

        public static void SetActiveChildrenIfEquals(this GameObject go, string name, bool active)
        {
            var array = go.GetComponentsInChildren<Transform>().Where((x) => x.name.Equals(name));
            foreach (var r in array)
            {
                r.gameObject.SetActive(active);
            }
        }

        public static void DoAfter<T>(this T mb, float time, Action action) where T : MonoBehaviour
        {
            mb.StartCoroutine(DoAfterEnumerator(time, action));
        }

        private static IEnumerator DoAfterEnumerator(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            if (action != null) action.Invoke();
        } 

    }
}
