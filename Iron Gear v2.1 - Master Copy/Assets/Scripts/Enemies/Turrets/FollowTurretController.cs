using UnityEngine;
using System.Collections;

public class FollowTurretController : MonoBehaviour
{
	public Transform turretController;

	// Update is called once per frame
	void Update ()
	{
		FollowController();
	}

	void FollowController()
	{
		//make sure the model is following the turret controller object
		transform.position = turretController.position;
	}
}
