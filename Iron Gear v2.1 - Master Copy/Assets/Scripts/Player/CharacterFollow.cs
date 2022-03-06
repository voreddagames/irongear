using UnityEngine;
using System.Collections;

public class CharacterFollow : MonoBehaviour
{
    private GameObject playerControllerTarget;

    void Awake()
    {
        playerControllerTarget = GameObject.FindWithTag("Player");
    }

	void LateUpdate ()
    {
		FollowController();
	}

	void FollowController()
	{
		//follow the player controller.
		transform.position = playerControllerTarget.transform.position;
	}
}