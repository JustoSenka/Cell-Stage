using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour {

    public Transform bombPrefab;
    public float throwStrength = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateAndThrowBomb();
        }
	}

    private void CreateAndThrowBomb()
    {
        GameObject bomb = InstantiateTransform(bombPrefab);

        bomb.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity + new Vector3(0, throwStrength, 0);
        bomb.transform.position = transform.position + new Vector3(0, transform.lossyScale.y / 2, 0);
        bomb.transform.SetParent(GameObject.FindGameObjectWithTag("Generated").transform);
    }

    private GameObject InstantiateTransform(Transform obj)
    {
        Debug.Log("Connections: " + NetworkManager.singleton.numPlayers);
        GameObject go;
        if (NetworkManager.singleton.numPlayers > 1)
        {
            go = (GameObject)Network.Instantiate(
                obj,
                new Vector3(),
                new Quaternion(),
                0);
        }
        else
        {
            go = Instantiate<Transform>(obj.transform).gameObject;
        }
        return go;
    }
}
