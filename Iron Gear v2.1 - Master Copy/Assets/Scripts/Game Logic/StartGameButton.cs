using UnityEngine;
using System.Collections;

public class StartGameButton : MonoBehaviour
{
	private GameObject loadingScreenGUI;

	private LoadingScreen loadingScreen;

	// Use this for initialization
	void Start ()
	{
		loadingScreenGUI = GameObject.FindWithTag("Loading Screen");
		loadingScreen = loadingScreenGUI.GetComponent<LoadingScreen>();
	}
	
	// Update is called once per frame
	void Update ()
	{
//		StartGame();
	}

	void StartGame()
	{
//		if (Input.GetKeyDown(KeyCode.Space) && !loadingScreen.isLoadingNextLevel)
//		{
//			loadingScreen.lastLevelLoaded = loadingScreen.levelToLoad;
//			loadingScreen.isLoadingNextLevel = true;
//		}
	}
}
