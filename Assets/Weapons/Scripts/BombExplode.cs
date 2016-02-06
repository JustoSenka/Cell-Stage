using System;
using UnityEngine;
using System.Collections;
using System.Linq;

public class BombExplode : MonoBehaviour {

    public float lifetime = 3f;

    public void Start()
    {
        StartCoroutine(ExplodeAfter(lifetime));
    }

    public IEnumerator ExplodeAfter(float time)
    {
        yield return new WaitForSeconds(time);

        gameObject.BroadcastMessage("Play");
        DisableBombMesh();
        DisableBombCollider();
        DisableFlare("Scale");

        StartCoroutine(DoAfter(3, () => Destroy(gameObject)));
    }

    private void DisableBombMesh()
    {
        var array = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (var r in array)
        {
            r.enabled = false;
        }
    }

    private void DisableBombCollider()
    {
        var array = gameObject.GetComponentsInChildren<Collider>();
        foreach (var r in array)
        {
            r.enabled = false;
        }
    }

    private void DisableFlare(string str)
    {
        var d = GetComponentsInChildren<Transform>().Where((t) => t.name.Contains(str));
        d.First<Transform>().gameObject.SetActive(false);
    }

    private IEnumerator DoAfter(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        if (action != null) { action.Invoke(); Debug.Log("Invoke"); }
    }
}
