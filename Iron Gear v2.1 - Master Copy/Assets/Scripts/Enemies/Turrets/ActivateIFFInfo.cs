using UnityEngine;
using System.Collections;

public class ActivateIFFInfo : MonoBehaviour
{
	public GameObject gameMaster;

	public EnemyHealth enemyHealth;
	private TutorialSystem tutorialSystem;

	void Awake()
	{
		enemyHealth = GetComponent<EnemyHealth>();
		tutorialSystem = gameMaster.GetComponent<TutorialSystem>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		ActivateIFFInfoOnDeath();
	}

	void ActivateIFFInfoOnDeath()
	{
		//display the IFF system after the enemy died the first time
		if (enemyHealth.health <= 0)
		{
			tutorialSystem.canDisplayIFFInfo = true;
		}
	}
}
