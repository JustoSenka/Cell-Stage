using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using CellStage;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]
public class PlayerController : NetworkBehaviour {

    [SyncVar]
    public short health = 100;
    public Transform bombPrefab;
    public float throwStrength = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdCreateAndThrowBomb();
        }
	}

    [Command]
    private void CmdCreateAndThrowBomb()
    {
        GameObject go = Instantiate<Transform>(bombPrefab.transform).gameObject;
        go.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity + new Vector3(0, throwStrength, 0);
        go.transform.position = transform.position + new Vector3(0, transform.lossyScale.y / 2, 0);
        go.transform.SetParent(GameObject.FindGameObjectWithTag("Generated").transform);

        if (NetworkManager.singleton.numPlayers > 1)
        {
            NetworkServer.Spawn(go, go.GetComponent<NetworkIdentity>().assetId);
        }
    }

    [Command]
    public void CmdAlterHealth(short value)
    {
        health += value;

        if (health < 0) 
        { 
            ExplodePlayer();
        }
    }

    private void ExplodePlayer()
    {
        gameObject.GetComponentInChildren<ParticleSystem>().Play();
        SetEnabledForGameObject(gameObject, false);

        this.DoAfter(3, () => Respawn());
    }

    private void Respawn()
    {
        gameObject.transform.position = GetStartPosition();
        SetEnabledForGameObject(gameObject, true);
        health = 100;
        CmdAlterHealth(0);
    }

    private void SetEnabledForGameObject(GameObject go, bool enabled)
    {
        // Do not enable moveent and player controller for not local player, never.
        if (isLocalPlayer && enabled)
        {
            go.GetComponent<Movement>().enabled = enabled;
            go.GetComponent<PlayerController>().enabled = enabled;
        }
        go.GetComponentsInChildren<MeshRenderer>().SetEnabled(enabled);
        go.GetComponentsInChildren<Collider>().SetEnabled(enabled);
    }

    public Vector3 GetStartPosition()
    {
        return new Vector3(-3, 5, 0);
    }
}
