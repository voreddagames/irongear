using UnityEngine;
using System.Collections;

//used to better organize scripts
public class PlayerManager : MonoBehaviour
{
	private PlayerMotor playerMotor;
	private PlayerPhysics playerPhysics;
	private MeleeSystem meleeSystem;
	private PlayerAnimation playerAnimation;
	private SteamChangeAbility steamChangeAbility;
	private PlayerHealth playerHealth;
	private BoilingPointsSystem boilingPointsSystem;
	private AnimateSteamFlaps animateSteamFlaps;
	private PlayerAudio playerAudio;

	void Awake()
	{
		//playerAudio = GetComponent<PlayerAudio>();
		//animateSteamFlaps = GetComponent<AnimateSteamFlaps>();
		playerMotor = GetComponent<PlayerMotor>();
		//playerAnimation = GetComponent<PlayerAnimation>();
		meleeSystem = GetComponent<MeleeSystem>();
		playerPhysics = GetComponent<PlayerPhysics>();
		steamChangeAbility = GetComponent<SteamChangeAbility>();
		playerHealth = GetComponent<PlayerHealth>();
		boilingPointsSystem = GetComponent<BoilingPointsSystem>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//playerAudio.UpdatePlayerAudio();
		/*meleeSystem.UpdateMeleeSystem();
		playerPhysics.UpdatePlayerPhysics();
		playerMotor.UpdateMotor();*/
		steamChangeAbility.UpdateSteamChangeAbility();
		playerHealth.UpdatePlayerHealth();
		boilingPointsSystem.UpdateBoilingPointsSystem();
		//animateSteamFlaps.UpdateAnimateSteamFlaps();
	}
	
	/*void FixedUpdate()
	{
		playerMotor.FixedUpdateMotor();
		playerPhysics.FixedUpdatePlayerPhysics();
		meleeSystem.FixedUpdateMeleeSystem();
		//playerAnimation.FixedUpdatePlayerAnimation();
	}*/

	void LateUpdate()
	{
		//playerPhysics.LateUpdatePlayerPhysics();
	}
}
