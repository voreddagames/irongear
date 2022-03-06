using UnityEngine;
using System.Collections;

//script placed on all throwable objects
public class ThrowableObject : MonoBehaviour
{
	public static ThrowableObject instance;

	public bool hasThrownObject;

	public Transform pickupTarget;

	void Awake()
	{
		instance = this;
	}

	public void ThrowMe()
	{
		//move to m3irv's hands
		transform.position = pickupTarget.position;
		//set to be the current object m3irv is holding
		M3IRVAnimation.instance.currentObjectHolding = transform;
	}
}
