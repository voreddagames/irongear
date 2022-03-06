using UnityEngine;
using System.Collections;

public class DestroyStuffForDemo : MonoBehaviour
{
	private float timeTillDeath = 1.0f;
	
	private GameObject endDemoManager;
	
	private QTECogForDemo demoQTE;
	
	void Start()
	{
		endDemoManager = GameObject.FindWithTag("GameController");
		demoQTE = endDemoManager.GetComponent<QTECogForDemo>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		KillObject();
	}
	
	void KillObject()
	{
		if (demoQTE.endGameDemo)
		{
			Destroy(gameObject, timeTillDeath);
		}
	}
}
