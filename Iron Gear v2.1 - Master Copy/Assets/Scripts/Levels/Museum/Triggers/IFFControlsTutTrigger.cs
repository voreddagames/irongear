using UnityEngine;
using System.Collections;

public class IFFControlsTutTrigger : MonoBehaviour
{
	public Transform tutSystemGO;

	private TutorialSystem tutorialSystem;

	void Awake()
	{
		tutorialSystem = tutSystemGO.GetComponent<TutorialSystem>();
	}

	void OnTriggerEnter(Collider col)
	{
		//trigger the IFF tutorial
		if (col.tag == "Player")
		{
			tutorialSystem.CanStartIFFTut = true;
			Destroy(gameObject);
		}
	}
}
