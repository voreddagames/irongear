using UnityEngine;
using System.Collections;

public class M3IRVPhysics : MonoBehaviour
{
	public static M3IRVPhysics instance;

	public float flightSpeed = 40.0f;

	public GameObject player;

	public float moveSpeed = 0.0f;

	public Vector3 moveVector;

	void Awake()
	{
		instance = this;

		moveSpeed = flightSpeed;
		player = GameObject.FindWithTag("Player");
	}

	public void UpdateM3IRVPhysics()
	{
		CalculateMovement();
	}

	void CalculateMovement()
	{
		//move M3IRV
		moveVector = Vector3.right * moveSpeed;
		GetComponent<Rigidbody>().velocity = moveVector;
	}
}
