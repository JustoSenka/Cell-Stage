using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;

public class SetupLocalPlayer : NetworkBehaviour {

	void Start () {
        if (isLocalPlayer)
        {
            GetComponent<Movement>().enabled = true;
            GetComponent<PlayerController>().enabled = true;
            GetComponent<Transform>().position = GetStartPosition();

            GameObject.FindObjectOfType<Camera>().GetComponent<FollowTarget>().target = GetComponent<Transform>();
            GameObject.FindObjectOfType<Camera>().GetComponent<LookatTarget>().SetTarget(GetComponent<Transform>());
        }
	}

    public Vector3 GetStartPosition()
    {
        return new Vector3(-3, 5, 0);
    }
}
