using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using System.Linq;

public class LocalPlayerController : NetworkBehaviour {

    public Transform playerPrefab;
    public Transform player;

	void Start () {
        if (isLocalPlayer)
        {
            CmdSpawnPlayer();

            EnableComponents();
        }
	}

    private void EnableComponents()
    {
        player.GetComponent<Movement>().enabled = true;
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<Transform>().position = new Vector3(-3, 5, 0);
        GameObject.FindObjectOfType<Camera>().GetComponent<FollowTarget>().target = player.transform;
        GameObject.FindObjectOfType<Camera>().GetComponent<LookatTarget>().SetTarget(player.transform);
    }

    [Command]
    private void CmdSpawnPlayer()
    {
        Transform t = Instantiate<Transform>(playerPrefab);
        t.GetComponent<PlayerController>().localPlayerController = this.gameObject.GetComponent<LocalPlayerController>();
        this.gameObject.GetComponent<LocalPlayerController>().player = t.transform;
        NetworkServer.Spawn(t.gameObject);
    }
}
