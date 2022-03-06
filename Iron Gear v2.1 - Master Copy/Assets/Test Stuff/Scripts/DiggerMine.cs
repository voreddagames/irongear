using UnityEngine;
using System.Collections;

public class DiggerMine : MonoBehaviour
{
	public Transform target;

	public float speed = 20.0f;

	// Update is called once per frame
	void Update ()
	{
		FollowTarget();
	}

	void FollowTarget()
	{
		transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
		transform.LookAt(target);
	}
}
