using UnityEngine;
using System.Collections;

public class ImpetusPhysics : MonoBehaviour
{
	public Transform target;

	public Light impetusLight;

	public float lightRange = 10.0f;

	public float extendRate = 10.0f;
	public float retractRate = 20.0f;

	private float origRange;

	private Color origLightColor;
	public Color lastColor;

	public bool hasChangedColor = false;
	public bool canChangeLight = false;
	public bool isPerformingLightChange = false;

	private GameObject player;
	private SteamChangeAbility steamChangeAbility;

	public Color curColor;

	void Awake ()
	{
		player = GameObject.FindWithTag("Player");
		steamChangeAbility = player.GetComponent<SteamChangeAbility>();
	}

	void Start()
	{
		origRange = impetusLight.range;
		origLightColor = impetusLight.color;
	}

	public void UpdateImpetusPhysics ()
	{
		FollowPlayer();
		IndicateColorChange();
	}

	void FollowPlayer()
	{
		//keep with player's location
		transform.position = target.position;
	}

	void IndicateColorChange()
	{
		curColor = impetusLight.color;

		//player just changed color
		if (steamChangeAbility.hasPressedColorChange && !hasChangedColor && impetusLight.color != lastColor)
		{
			canChangeLight = true;
			hasChangedColor = true;
		}
		if (canChangeLight)
		{
			//change light color to the selected steam color
			switch (steamChangeAbility.currentColor)
			{
				//blue
			case 1:
				impetusLight.color = Color.blue;
				lastColor = Color.blue;
				break;
				//yellow
			case 2:
				impetusLight.color = Color.yellow;
				lastColor = Color.yellow;
				break;
				//red
			case 3:
				impetusLight.color = Color.red;
				lastColor = Color.red;
				break;
				//normal color
			default:
//				impetusLight.color = Color.white;
				impetusLight.color = origLightColor;
				lastColor = origLightColor;
				break;
			}

			//extend the impetus light
			if (impetusLight.range < lightRange)
			{
				impetusLight.range += extendRate * Time.deltaTime;
			}
			else
			{
				canChangeLight = false;
			}
		}
		//at its peak stop extending
		else
		{
			//retract the light
			if (impetusLight.range > origRange)
			{
				impetusLight.range -= retractRate * Time.deltaTime;
			}
			//reset the color and the range to the original values
			else
			{
				impetusLight.range = origRange;
//				impetusLight.color = origLightColor;
				
				if (!steamChangeAbility.hasPressedColorChange)
				{
					hasChangedColor = false;
				}
			}
		}
		//if player changes while still performing the color changing snap to restart
		if (impetusLight.color == lastColor && steamChangeAbility.hasPressedColorChange && isPerformingLightChange)
		{
			impetusLight.range = origRange;
			//only allow this action once
			canChangeLight = true;
			isPerformingLightChange = false;

		}
		//reset when letting go of the button
		if (!isPerformingLightChange && !steamChangeAbility.hasPressedColorChange)
		{
			hasChangedColor = false;
			isPerformingLightChange = true;
		}
	}
}
