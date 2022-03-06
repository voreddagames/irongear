using UnityEngine;
using System.Collections;

public class DetectionLight : MonoBehaviour
{
	public bool hasDetected = false;

	public GameObject lenox;

	private GameObject player;

	private SteamChangeAbility steamChangeAbility;
	private LenoxMines lenoxMines;

	void Awake()
	{
		lenoxMines = lenox.GetComponent<LenoxMines>();
		player = GameObject.FindWithTag("Player");
		steamChangeAbility = player.GetComponent<SteamChangeAbility>();
	}

	void OnTriggerEnter(Collider col)
	{
		//if the player hits the cone trigger and is a different color then the player has been detected.
		if (col.tag == "Player" && lenoxMines.colorType.ToString() != steamChangeAbility.CurrentColorType() && !hasDetected)
		{
			hasDetected = true;
		}
	}
}
