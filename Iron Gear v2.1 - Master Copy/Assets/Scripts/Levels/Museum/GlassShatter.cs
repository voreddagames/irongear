using UnityEngine;
using System.Collections;

public class GlassShatter : MonoBehaviour
{
	public GameObject brokenSkylight;
	public ParticleSystem[] shatteredGlass;

	private bool canEmit = false;

	// Use this for initialization
	void Start ()
	{
	
	}

	// Update is called once per frame
	public void UpdateGlassShatter()
	{
		EmitGlass();
	}

	void EmitGlass()
	{
		if (canEmit)
		{
			//emit the glass shatters
			for (int i = 0; i < shatteredGlass.Length; i++)
			{
				shatteredGlass[i].Play();
				canEmit = false;
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.name == "Glass_2_2")
		{
			//replace skylight with the broken one
			//make sure to place the broken skylight where the whole one was since this script is on the cog
			Instantiate(brokenSkylight, col.transform.position, Quaternion.Euler(new Vector3(0, 90, 180)));
			Destroy(col.gameObject);

			canEmit = true;
		}
	}
}
