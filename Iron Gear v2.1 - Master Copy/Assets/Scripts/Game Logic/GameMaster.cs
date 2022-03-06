using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameMaster : MonoBehaviour
{
	private static GameMaster _instance;

	public AudioMixerSnapshot paused;
	public AudioMixerSnapshot unpaused;

	public AudioSource BGMSource;

	public Image pauseScreen;

	public Vector3 startPos;
	private GameObject player;

	private Canvas canvas;

	public static GameMaster instance
	{
		get
		{
			if (!_instance)
			{
				_instance = GameObject.FindObjectOfType<GameMaster>();

				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}

	void Awake()
	{
		if (!_instance)
		{
			_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			if (this != _instance)
			{
				Destroy(this.gameObject);
			}
		}

//		player = GameObject.FindWithTag("Player");
//		startPos = player.transform.position;
	}

	/*void Start()
	{
		canvas = GetComponent<Canvas>();
		pauseScreen.gameObject.SetActive(false);

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			canvas.enabled = !canvas.enabled;
			PauseGame();
		}
	}

	void PauseGame()
	{
//		if (Input.GetKeyDown(KeyCode.Escape))
//		{
			//pause
			if (Time.timeScale == 1)
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;

				pauseScreen.gameObject.SetActive(true);
				BGMSource.Pause();
				Time.timeScale = 0;
			}
			//unpause
			else
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;

				pauseScreen.gameObject.SetActive(false);
				BGMSource.UnPause();
				Time.timeScale = 1;
			}
//		}
	}*/
}
