using UnityEngine;
using System.Collections;

public class PlayerAudio : MonoBehaviour
{
	public GameObject playerModel;

	public AudioClip doubleJump;
	public AudioClip steps;
	public AudioClip hit;

	public AudioSource audioInstance;
	public AudioSource audioComp;

	public bool canPlayDoubleJump = false;

	private PlayerPhysics playerPhysics;
	private PlayerMotor playerMotor;
	private FisticuffCollisionDetection fisticuffCollisionDetection;

	void Awake()
	{
		playerPhysics = GetComponent<PlayerPhysics>();
		playerMotor = GetComponent<PlayerMotor>();
		fisticuffCollisionDetection = GetComponent<FisticuffCollisionDetection>();
	}

	// Use this for initialization
	void Start ()
	{
		audioComp = playerModel.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	public void UpdatePlayerAudio ()
	{
		HitEnemy();
		Walk ();
		DoubleJump();
	}

	void DoubleJump()
	{
		if (canPlayDoubleJump)
		{
			audioInstance.PlayOneShot(doubleJump);
			canPlayDoubleJump = false;
		}
	}

	void Walk()
	{
		if (playerPhysics.moveSpeed != 0 &&
		    playerPhysics.IsGrounded() &&
		    !playerPhysics.canTacticalSlide &&
		    !playerPhysics.isCrouching &&
		    !playerPhysics.IsOnWall())
		{
			if (!audioComp.isPlaying)
			{
				audioComp.pitch = 1.4f;
				audioComp.clip = steps;
				audioComp.Play();
			}
		}
		else
		{
			audioComp.pitch = 1;
			audioComp.Stop();
		}
	}

	void HitEnemy()
	{
		if (FisticuffCollisionDetection.DidHit)
		{
			if (!audioComp.isPlaying)
			{
				audioComp.clip = hit;
				audioComp.Play();
			}
		}
	}
}
