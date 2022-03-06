using UnityEngine;
using System.Collections;

public class DetectPlayerLevel : MonoBehaviour
{
	public bool isOnSameLevel;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			isOnSameLevel = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
		{
			isOnSameLevel = false;
		}
	}
}
