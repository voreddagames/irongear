using UnityEngine;
using System.Collections;

//placed on any object we need to respawn to the correct locations
public class ResetLevel : MonoBehaviour
{
	public Vector3 origPos;
	public Rigidbody rb;

	void Awake ()
	{
		origPos = transform.position;
	}

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update ()
	{
		ResetPosition();
	}

	void ResetPosition()
	{
		if (Checkpoints.isRespawning)
		{
			transform.position = origPos;
			rb.constraints = RigidbodyConstraints.FreezeAll;
		}
		else
		{
//			rb.constraints = RigidbodyConstraints.None;
//			rb.freezeRotation = true;
		}
	}
}
