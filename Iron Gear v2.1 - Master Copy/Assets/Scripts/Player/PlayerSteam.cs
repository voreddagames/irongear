using UnityEngine;
using System.Collections;
using System.Linq;

//[RequireComponent(typeof(ParticleSystem))]
public class PlayerSteam : MonoBehaviour
{
	public float angle = 45.0f;

	public int steamCnt = 1;

//	[HideInInspector]
//	public bool isDone = false;

//	public GameObject steam;

	public ParticleSystem[] steamSystems;
	private ParticleSystem.Particle[] steamParticles;

	public bool isEmittingAllFlaps = false;
	private bool canEmit = true;

	private GameObject player;

	private SteamChangeAbility steamChangeAbility;
	private PlayerPhysics playerPhysics;
	private AnimateSteamFlaps animateSteamFlaps;
	private PlayerMotor playerMotor;

	void Start()
	{
		steamSystems = GetComponentsInChildren<ParticleSystem>();

		foreach (ParticleSystem steam in steamSystems)
		{
			var steamEmissionRot = steam.main.startRotation;
			steamEmissionRot = angle;
		}
//		steamSystem.startRotation = angle;

		player = GameObject.FindWithTag("Player");
		playerPhysics = player.GetComponent<PlayerPhysics>();
		steamChangeAbility = player.GetComponent<SteamChangeAbility>();
		animateSteamFlaps = player.GetComponent<AnimateSteamFlaps>();
		playerMotor = player.GetComponent<PlayerMotor>();
	}

	void Update()
	{
//		SteamRotationCorrection();
		//LowerFlapEmission();
		UpperFlapsEmission();
		SwitchColor();
		EmitAllFlaps();
	}

	/*void LowerFlapEmission()
	{
		if (steamCnt == 3)
		{
			//emitting for double jumping
			if (playerMotor.isDoubleJumping)
			{
				//blue
				if (steamChangeAbility.CurrentColorType() == "Blue")
				{
					foreach (ParticleSystem steam in steamSystems)
					{
						var emitParams = new ParticleSystem.EmitParams();
						emitParams.position = new Vector3(steam.transform.position);
						emitParams.velocity = new Vector3(0, 0, -3);
						emitParams.startSize = 1;
						emitParams.startColor = Color.blue;
						steam.Emit(emitParams, 0.5f);
					}
				}
				//red
				if (steamChangeAbility.CurrentColorType() == "Red")
				{
					foreach (ParticleSystem steam in steamSystems)
					{
						steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.red);
					}
				}
				//yellow
				if (steamChangeAbility.CurrentColorType() == "Yellow")
				{
					foreach (ParticleSystem steam in steamSystems)
					{
						steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.yellow);
					}
				}
				//white
				if (steamChangeAbility.CurrentColorType() == "White")
				{
					foreach (ParticleSystem steam in steamSystems)
					{
						steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.white);
					}
				}
			}
		}
	}*/

	void UpperFlapsEmission()
	{
		if (steamCnt == 1 || steamCnt == 2)
		{
			//emitting for sliding
			if (playerPhysics.canSlideFromCrouch)
			{
				//blue
				if (steamChangeAbility.CurrentColorType() == "Blue")
				{
					foreach (ParticleSystem steam in steamSystems)
					{
						steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.blue);
					}
				}
				//red
				if (steamChangeAbility.CurrentColorType() == "Red")
				{
					foreach (ParticleSystem steam in steamSystems)
					{
						steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.red);
					}
				}
				//yellow
				if (steamChangeAbility.CurrentColorType() == "Yellow")
				{
					foreach (ParticleSystem steam in steamSystems)
					{
						steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.yellow);
					}
				}
				//white
				if (steamChangeAbility.CurrentColorType() == "White")
				{
					foreach (ParticleSystem steam in steamSystems)
					{
						steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.white);
					}
				}
			}
		}
	}


	//emit steam from all the flaps in their respective colors
	void EmitAllFlaps()
	{
		if (playerPhysics.IsAirDashing())
		{
			isEmittingAllFlaps = true;
		}
		else
		{
			isEmittingAllFlaps = false;
		}

		if (isEmittingAllFlaps)
		{
			//blue
			if (steamChangeAbility.CurrentColorType() == "Blue")
			{
				foreach (ParticleSystem steam in steamSystems)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.blue);
				}
//				isEmittingAllFlaps = false;
			}
			//red
			if (steamChangeAbility.CurrentColorType() == "Red")
			{
				foreach (ParticleSystem steam in steamSystems)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.red);
				}
//				isEmittingAllFlaps = false;
			}
			//yellow
			if (steamChangeAbility.CurrentColorType() == "Yellow")
			{
				foreach (ParticleSystem steam in steamSystems)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.yellow);
				}
//				isEmittingAllFlaps = false;
			}
			//white
			if (steamChangeAbility.CurrentColorType() == "White")
			{
				foreach (ParticleSystem steam in steamSystems)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.white);
				}
//				isEmittingAllFlaps = false;
			}
		}
	}

	void SwitchColor()
	{
		if (steamCnt == animateSteamFlaps.curFlapIndex && canEmit)
		{
			//switch color to blue
			if (steamChangeAbility.CurrentColorType() == "Blue")
			{
				foreach (ParticleSystem steam in steamSystems)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.blue);
//					int numOfParticles = steam.GetParticles(steamParticles);
//					
//					for (int i = 0; i < numOfParticles; i++)
//					{
//						steamParticles[i].rotation = -angle * playerPhysics.faceDir;
//					}
//					steam.SetParticles(steamParticles, numOfParticles);
//					steam.startRotation = -angle * playerPhysics.faceDir;
	//				steam.startColor = Color.blue;
				}
			}
			//switch to red
			if (steamChangeAbility.CurrentColorType() == "Red")
			{
				foreach (ParticleSystem steam in steamSystems)
				{
					steam.Emit(steam.transform.position, new Vector3(-3, 3, 0), 1, 0.5f, Color.red);
				}
			}
			//switch to yellow
			if (steamChangeAbility.CurrentColorType() == "Yellow")
			{
				foreach (ParticleSystem steam in steamSystems)
				{
					steam.Emit(steam.transform.position, new Vector3(-3, 3, 0), 1, 0.5f, Color.yellow);
				}
			}
			//switch to normal
			if (steamChangeAbility.CurrentColorType() == "White")
			{
				foreach (ParticleSystem steam in steamSystems)
				{
					steam.Emit(steam.transform.position, new Vector3(-3, 3, 0), 1, 0.5f, Color.white);
				}
			}
			canEmit = false;
		}
		if (steamCnt != animateSteamFlaps.curFlapIndex)
		{
			canEmit = true;
		}
	}

		void InitializeIfNeeded()
		{
//		int numOfParticles = steamSystems.GetParticles(steamParticles);
//		
//		for (int i = 0; i < numOfParticles; i++)
//		{
//			steamParticles[i].rotation = -angle * playerPhysics.faceDir;
//		}
//		steamSystems.SetParticles(steamParticles, numOfParticles);
//
//			if (!steamSystems)
//			{
//				steamSystems = GetComponent<ParticleSystem>();
//			}
//	
//			if (steamParticles == null || steamParticles.Length < steamSystems.maxParticles)
//			{
//				steamParticles = new ParticleSystem.Particle[steamSystems.maxParticles];
//			}
		}
	

	void SteamRotationCorrection()
	{
		foreach(ParticleSystem steam in steamSystems)
		{
			steam.startRotation = -angle * playerPhysics.faceDir;
		}
	}
}
