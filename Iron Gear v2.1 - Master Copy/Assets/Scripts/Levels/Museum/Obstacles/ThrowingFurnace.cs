using UnityEngine;
using System.Collections;

public class ThrowingFurnace : MonoBehaviour
{
	public static ThrowingFurnace instance;

	public float speed = 10.0f;

	public Transform target;
//	public Transform pickupTarget;

	public GameObject brokenScafolding;
	public GameObject scaffoldingToDestroy;

	private GameObject cam;
	private CamPositioning camPositioning;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		transform.LookAt(target);

		cam = Camera.main.gameObject;
		camPositioning = cam.GetComponent<CamPositioning>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		ThrowFurnace();
		DestroyScaffolding();
	}

	void ThrowFurnace()
	{
		if (!target)
		{
			return;
		}

		//###MAY BE ABLE TO TAKE THIS OUT AND PUT INTO START METHOD###
		ThrowableObject throwableObject = GetComponent<ThrowableObject>();

		//call this method so we know what m3irv is currently throwing
		if (M3IRVLevelOneOutdated.instance.hasTriggeredFurnace)
		{
			throwableObject.ThrowMe();
		}
		if (throwableObject.hasThrownObject)
		{
			//m3irv threw object towards the scaffolding target
			transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
			//no longer holding object
			M3IRVAnimation.instance.currentObjectHolding = null;
		}
	}

	void DestroyScaffolding()
	{
		ThrowableObject throwableObject = GetComponent<ThrowableObject>();

		if (!target)
		{
			return;
		}

		if (transform.position == target.position)
		{
			//replace scaffolding with the broken one
			Instantiate (brokenScafolding, target.position, target.rotation);

			M3IRVLevelOneOutdated.instance.isQTEComplete = true;
			throwableObject.hasThrownObject = false;
			camPositioning.triggerCog = false;

			Destroy(scaffoldingToDestroy);
		}
	}
}
