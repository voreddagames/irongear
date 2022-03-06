using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour
{
	public AudioMixerSnapshot paused;
	public AudioMixerSnapshot unpaused;

	private Canvas canvas;

	void Start()
	{
		canvas = GetComponent<Canvas>();
		
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
		//pause
		if (Time.timeScale == 1)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

//			BGMSource.Pause();
			Time.timeScale = 0;
		}
		//unpause
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

//			BGMSource.UnPause();
			Time.timeScale = 1;
		}
		LowPass();
	}

	void LowPass()
	{
		if (Time.timeScale == 0)
		{
			paused.TransitionTo(0.01f);
		}
		else
		{
			unpaused.TransitionTo(0.01f);
		}
	}
}
