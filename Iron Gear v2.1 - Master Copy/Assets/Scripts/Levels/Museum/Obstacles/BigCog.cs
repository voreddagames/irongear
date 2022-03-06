using UnityEngine;
using System.Collections;

public class BigCog : MonoBehaviour
{
	public static BigCog instance;

	public float force = -80.0f;

	private GlassShatter glassShatter;

	// Use this for initialization
	void Awake ()
	{
		instance = this;

		glassShatter = GetComponent<GlassShatter>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		glassShatter.UpdateGlassShatter();
		ThrowCog();
	}

	void ThrowCog()
	{
		//###MAY BE ABLE TO TAKE THIS OUT AND PUT INTO START METHOD###
		ThrowableObject throwableObject = GetComponent<ThrowableObject>();

		//call this method so we know what m3irv is currently throwing
		if (M3IRVLevelOneOutdated.instance.hasTriggeredCog)
		{
			throwableObject.ThrowMe();
		}
		//m3irv threw the cog
		if (throwableObject.hasThrownObject)
		{
			GetComponent<ConstantForce>().force = new Vector3(0, force, 0);
			M3IRVAnimation.instance.currentObjectHolding = null;
		}
	}
}
