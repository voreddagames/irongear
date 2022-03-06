using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
	public int levelToLoad;
//	public int curLevel;
//	[HideInInspector]
//	public int lastLevelLoaded;

	public bool isLoadingNextLevel;

	public GameObject hudToHide;

	public Image backGround;
	public Image loadingText;
	public Image headOfCl8ton;
	public RectTransform progressGear;

	public float progressGearRotSpeed = 10.0f;

	private int loadProgress = 0;

	// Use this for initialization
	void Awake ()
	{

	}

	void Start()
	{
		loadingText.gameObject.SetActive(false);
		progressGear.gameObject.SetActive(false);
		backGround.gameObject.SetActive(false);
		headOfCl8ton.gameObject.SetActive(false);

//		if (curLevel == 0)
//		{
//			isLoadingNextLevel = true;
//		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		LoadingLevel();
	}

	//void OnLevelWasLoaded(int lvl)
	//{
//		levelToLoad++;
//		lastLevelLoaded = curLevel;
//		if (lvl != 0)
//		{
//			isLoadingNextLevel = false;
//		}
	//}

	void LoadingLevel()
	{
		//will be replaced with win condition
		if (Input.GetKeyDown(KeyCode.N))
		{
			StartCoroutine(DisplayLoadingScreen());
		}
	}

	IEnumerator DisplayLoadingScreen()
	{
		hudToHide.SetActive(false);
		backGround.gameObject.SetActive(true);
		loadingText.gameObject.SetActive(true);
		progressGear.gameObject.SetActive(true);
		headOfCl8ton.gameObject.SetActive(true);

		AsyncOperation aSync = SceneManager.LoadSceneAsync(levelToLoad);

		while (!aSync.isDone)
		{
//			loadProgress = (int)(aSync.progress * 100);

			//rotate image to indicate loading
			progressGear.Rotate(progressGear.forward * progressGearRotSpeed);

			yield return null;
		}
	}
}
