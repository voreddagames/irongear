using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
	
	// Update is called once per frame
	void Update ()
	{
		StartGame();
	}

	void StartGame()
	{
		//load the first level
		//###CHANGE TO USE THIS FOR BOTH NEW GAME AND CONTINUE GAME###
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene(1);
		}
	}
}
