using UnityEngine;
using System.Collections;

public class BoilingPointsSystem : MonoBehaviour
{
	public bool hasChangedTierLevel;

	public int tierLevel = 1;
	public int lastTier = 0;
	public int numOfAccessedTiers = 1;

	public int startBoilTemperature = 400;
	public int minBoilTemperature = 200;
	public int maxBoilTemperature = 2000;

	public float temperatureModifier = 10.0f;
	public float lowSteamDamageRate = 3.0f;

	public float boilTemp = 0.0f;

	public GameObject tutSystemObj;

	private TutorialSystem tutSystem;
	private PlayerHealth playerHealth;

	private GameObject cam;
	private CamPositioning camPositioning;

	// Use this for initialization
	void Start ()
	{
		cam = Camera.main.gameObject;
		camPositioning = cam.GetComponent<CamPositioning>();

//		tutSystem = tutSystemObj.GetComponent<TutorialSystem>();
		playerHealth = GetComponent<PlayerHealth>();
		boilTemp = startBoilTemperature;
	}
	
	// Update is called once per frame
	public void UpdateBoilingPointsSystem ()
	{
		CalculateBoilTemp();
		CalculateTierLevel();
	}

	void CalculateTierLevel()
	{
		//set tier level 1
		if (boilTemp <= 630 && numOfAccessedTiers > 0)
		{
			tierLevel = 1;
		}
		//set tier 2
		if (boilTemp > 630 && boilTemp <= 1000 && numOfAccessedTiers > 1)
		{
			tierLevel = 2;
		}
		//set tier level 3
		if (boilTemp > 1000 && boilTemp <= 1500 && numOfAccessedTiers > 2)
		{
			tierLevel = 3;
		}
		//set the final tier level 4
		if (boilTemp > 1500 && numOfAccessedTiers > 3)
		{
			tierLevel = 4;
		}
		if (lastTier != tierLevel)
		{
			//tier level increased
			if (tierLevel > lastTier)
			{
				//call methods for increased tier
			}
			//tier level decreased
			if (tierLevel < lastTier)
			{
				//call methods for decreased tier
			}
			hasChangedTierLevel = true; 
			lastTier = tierLevel;
		}
	}

	void CalculateBoilTemp()
	{
		//###PUT THIS COG FUNCTION IN ITS OWN SCRIPT###
		if (!camPositioning.triggerCog)
		{
			//deplete steam over time
			if (boilTemp > minBoilTemperature)
			{
//				if (tutSystem.canDisplayIFFInfo)
//				{
					boilTemp -= (int)Mathf.Abs(temperatureModifier) * Time.deltaTime;
//				}
			}
			//no more steam.  deplete health
			else
			{
				playerHealth.health -= lowSteamDamageRate * Time.deltaTime;
			}
		}
		//cap the steam value
		if (boilTemp > startBoilTemperature)
		{
			boilTemp = startBoilTemperature;
		}
	}

	public void AdjustBoilTemp(int adj)
	{
		//apply current steam values
		boilTemp += adj;
	}
}
