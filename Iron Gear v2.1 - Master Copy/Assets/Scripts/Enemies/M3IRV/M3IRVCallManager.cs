using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M3IRVCallManager : MonoBehaviour
{
	public enum CurrentLevelName
	{
		LevelOne,
		LevelTwo
	}

	public CurrentLevelName currentLevel;

	//public string CurrentLevel() { return ("M3IRV" + currentLevel.ToString()); }

	// Use this for initialization
	void Start ()
	{
		UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(gameObject, "Assets/Scripts/Enemies/M3IRV/M3IRVCallManager.cs (22,3)", "M3IRV" + currentLevel.ToString());
	}
}
