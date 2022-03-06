using UnityEngine;
using System.Collections;

public class MissileBarageTrigger : MonoBehaviour
{

	void OnTriggerEnter(Collider col)
	{
		//trigger missile barage
		if (col.tag == "Player")
		{
			M3IRVLevelOneOutdated.instance.hasTriggeredMissileBarage = true;
			//destroy trigger to prevent reactivating QTE
			Destroy(gameObject);
		}
	}
}
