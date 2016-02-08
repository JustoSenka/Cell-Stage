using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using UnityEngine.Networking;

namespace CellStage
{
    public static class Extention
    {

        public static void SetEnabled(this MeshRenderer[] array, bool enabled)
        {
            foreach (var r in array)
            {
                r.enabled = enabled;
            }
        }

        public static void SetEnabled<T>(this T[] array, bool enabled) where T : Collider
        {
            foreach (var r in array)
            {
                r.enabled = enabled;
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

        public static void DoAfter(this MonoBehaviour mb, float time, Action action)
        {
            mb.StartCoroutine(DoAfterEnumerator(time, action));
        }

        public static void DoAfter(this NetworkBehaviour mb, float time, Action action)
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
