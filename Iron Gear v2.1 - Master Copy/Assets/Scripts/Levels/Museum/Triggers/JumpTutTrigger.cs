using UnityEngine;
using System.Collections;

public class JumpTutTrigger : MonoBehaviour
{
	public Transform tutSystemGO;
	
	private TutorialSystem tutorialSystem;

	void Awake()
	{
		tutorialSystem = tutSystemGO.GetComponent<TutorialSystem>();
	}

	void OnTriggerEnter(Collider col)
	{
		//trigger the jump tutorial
		if (col.tag == "Player")
		{
			tutorialSystem.CanStartJumpTut = true;
			Destroy(gameObject);
		}
	}
}
