using UnityEngine;
using System.Collections;

public class MissileDodgeTrigger : MonoBehaviour
{

	void OnTriggerEnter(Collider col)
	{
		//trigger missile dodge QTE
		if (col.tag == "Player")
		{
			M3IRVLevelOneOutdated.instance.hasTriggeredDodgeMissiles = true;
			//destroy trigger to prevent reactivating QTE
			Destroy(gameObject);
		}
	}
}
