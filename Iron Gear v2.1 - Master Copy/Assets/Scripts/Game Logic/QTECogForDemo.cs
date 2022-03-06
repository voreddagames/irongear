using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QTECogForDemo : MonoBehaviour
{
	public bool endGameDemo = false;

	public AudioSource BGMSource;

	public Image endScreenBG;
	public Image likeAndShare;
	public Image thanks;
	public Image continueButton;

//	public float timeToEndDemo = 2.0f;
	public float timeTillEndLevel = 2.0f;
	public float timeToDisplayThanks = 2.0f;
	public float timeToDisplayLike = 4.0f;

	private GameObject player;
	private PlayerMotor playerMotor;

	private Rigidbody rb;

	private float curTimeTillEndLevel;
//	private float curTimeToEndDemo;
	private float curTimeToDisplayThanks;
	private float curTimeToDisplayLike;

	private GameObject loadingScreenGUI;
	private GameObject cam;

	private LoadingScreen loadingScreen;
	private CamPositioning camPositioning;

	// Use this for initialization
	void Awake ()
	{
		endScreenBG.gameObject.SetActive(false);
		thanks.gameObject.SetActive(false);
		likeAndShare.gameObject.SetActive(false);
		continueButton.gameObject.SetActive(false);

		curTimeToDisplayThanks = timeToDisplayThanks;
		curTimeToDisplayLike = timeToDisplayLike;
		curTimeTillEndLevel = timeTillEndLevel;
//		curTimeToEndDemo = timeToEndDemo;

		cam = Camera.main.gameObject;
		camPositioning = cam.GetComponent<CamPositioning>();

		player = GameObject.FindWithTag("Player");
		rb = player.GetComponent<Rigidbody>();
		playerMotor = player.GetComponent<PlayerMotor>();
	}

	void Start()
	{
		loadingScreenGUI = GameObject.FindWithTag("Loading Screen");
		loadingScreen = loadingScreenGUI.GetComponent<LoadingScreen>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (camPositioning.triggerCog)
		{
//			if (player)
//			{
//				playerMotor.RemoveControl();
//				rb.constraints = RigidbodyConstraints.FreezePositionX |
//					RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
//			}
			if (curTimeTillEndLevel > 0)
			{
				curTimeTillEndLevel -= Time.deltaTime;
			}
			else
			{
//				Time.timeScale = 0;

				endGameDemo = true;
			}
		}
		if (endGameDemo)
		{


			BGMSource.Stop();
			//display thank you
			if (curTimeToDisplayThanks > 0)
			{
				thanks.gameObject.SetActive(true);
				endScreenBG.gameObject.SetActive(true);

				curTimeToDisplayThanks -= Time.deltaTime;
			}
			//display like and share
			else
			{

				thanks.gameObject.SetActive(false);
				if (curTimeToDisplayLike > 0)
				{
					likeAndShare.gameObject.SetActive(true);
					curTimeToDisplayLike -= Time.deltaTime;
				}
				//restart game
				else
				{
					likeAndShare.gameObject.SetActive(false);
					continueButton.gameObject.SetActive(true);

					Time.timeScale = 1;
//					loadingScreen.lastLevelLoaded = 0;
					loadingScreen.isLoadingNextLevel = true;
				}
			}
		}
	}
}
