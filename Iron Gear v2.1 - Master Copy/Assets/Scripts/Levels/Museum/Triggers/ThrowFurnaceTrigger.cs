using UnityEngine;
using System.Collections;

public class ThrowFurnaceTrigger : MonoBehaviour
{
	void OnTriggerEnter(Collider col)
	{
		//trigger QTE
		if (col.tag == "Player")
		{
			//activate the bool in m3irv's script
			M3IRVLevelOne.instance.isQTEActive = true;
			//destroy trigger to prevent reactivating QTE
			Destroy(gameObject);
		}
	}
}
