using UnityEngine;
using System.Collections;

public class FinalMissileTrigger : MonoBehaviour
{
	public static FinalMissileTrigger instance;

	public bool hasTriggeredFinalMissile = false;

	void Awake ()
	{
		instance = this;
	}

	void OnTriggerEnter(Collider col)
	{
		//trigger final missile that will destroy scaffolding chains
		if (col.tag == "Player")
		{
			hasTriggeredFinalMissile = true;
		}
	}
}
