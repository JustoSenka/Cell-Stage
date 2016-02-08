using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using CellStage;

public class BombExplode : MonoBehaviour {

    public float lifetime = 3f;

    public void Start()
    {
        this.DoAfter(lifetime, () =>
        {
            gameObject.GetComponentsInChildren<MeshRenderer>().DisableMeshes();
            gameObject.GetComponentsInChildren<Collider>().DisableColliders();
            gameObject.SetActiveChildrenIfContaints("Scale", false);

            gameObject.BroadcastMessage("Play");

            this.DoAfter(3, () => Destroy(gameObject));
        });
    }
}
