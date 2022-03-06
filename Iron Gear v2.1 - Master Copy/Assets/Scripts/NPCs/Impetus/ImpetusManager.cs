using UnityEngine;
using System.Collections;

//used to better organize scripts
public class ImpetusManager : MonoBehaviour
{
	private ImpetusPhysics impetusPhysics;

	void Awake()
	{
		impetusPhysics = GetComponent<ImpetusPhysics>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		impetusPhysics.UpdateImpetusPhysics();
	}
}
