using UnityEngine;
using System.Collections;

public class DestroyStuff : MonoBehaviour
{
	public float timeTillDeath = 0.5f;

	// Update is called once per frame
	void Update ()
	{
		KillObject();
	}

	void KillObject()
	{
		Destroy(gameObject, timeTillDeath);
	}
}
