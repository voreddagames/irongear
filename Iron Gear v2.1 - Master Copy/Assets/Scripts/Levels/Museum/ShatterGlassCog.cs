using UnityEngine;
using System.Collections;

public class ShatterGlassCog : MonoBehaviour
{
	public ParticleSystem glass;

	public bool isGlassParticleEnabled = false;

	// Use this for initialization
	void Start ()
	{
		glass = GetComponent<ParticleSystem>();
		var glassEmission = glass.emission;
		glassEmission.enabled = isGlassParticleEnabled;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//the cog QTE was triggered so we're going to render the glass particle system
		if (M3IRVLevelOneOutdated.instance.hasTriggeredCog)
		{
			isGlassParticleEnabled = true;

			var glassEmission = glass.emission;
			glassEmission.enabled = isGlassParticleEnabled;
		}
	}
}
