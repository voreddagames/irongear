using UnityEngine;
using System.Collections;

public class CogTrigger : MonoBehaviour
{
	private GameObject cam;
	private CamPositioning camPositioning;

	void Start()
	{
		cam = Camera.main.gameObject;
		camPositioning = cam.GetComponent<CamPositioning>();
	}

	void OnTriggerEnter(Collider col)
	{
		//trigger QTE
		if (col.tag == "Player")
		{
			M3IRVLevelOneOutdated.instance.hasTriggeredCog = true;
			camPositioning.triggerCog = true;
			//destroy the trigger to prevent reactivating QTE
			Destroy(gameObject);
		}
	}
}
