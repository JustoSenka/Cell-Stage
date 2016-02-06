using UnityEngine;
using System.Collections;

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
        Transform bomb = Instantiate<Transform>(bombPrefab);
        bomb.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity + new Vector3(0, throwStrength, 0);
        bomb.position = transform.position + new Vector3(0, transform.lossyScale.y / 2, 0);
        bomb.SetParent(GameObject.FindGameObjectWithTag("Generated").transform);
    }
}
