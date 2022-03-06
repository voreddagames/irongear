//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class PlayerMeleeAnimManager : MonoBehaviour
//{
//	public static PlayerMeleeAnimManager instance;
//
//	public GameObject playerAnim;
//
//	private GameObject player;
//
//	public Animator anim;
//
//	private MeleeSystem meleeSystem;
//
//	void Awake()
//	{
//		instance = this;
//	}
//
//	// Use this for initialization
//	void Start ()
//	{
//		player = GameObject.FindWithTag("Player");
//		anim = playerAnim.GetComponent<Animator>();
//		meleeSystem = player.GetComponent<MeleeSystem>();
//	}
//	
//	// Update is called once per frame
//	void Update ()
//	{
//		//DebugGame();
//
//		OverrideAnimation();
//		ResetAnimation();
//	}
//
//	void OverrideAnimation()
//	{
//		if (meleeSystem.isAttacking)
//		{
//			anim.SetLayerWeight(0, 0);
//			anim.SetLayerWeight(1, 1);
//		}
//		else
//		{
//			anim.SetLayerWeight(0, 1);
//			anim.SetLayerWeight(1, 0);
//		}
//
//	}
//
//	public void CalculateStandingAttack()
//	{
//		Debug.Log("What the fuck");
//		if (meleeSystem.isAttacking) //or meleeSystem.canAttack is false
//		{
//			anim.SetInteger("standingAttackIndex", meleeSystem.curStandingAttackIndex);
//			anim.SetBool("isAttacking", true);
//			anim.SetTrigger("isAttackButton");
//		}
//	}
//
//	public void CalculateCrouchingAttack()
//	{
//
//	}
//
//	public void CalculateAirAttack()
//	{
//
//	}
//
//	void ResetAnimation()
//	{
//		if (anim.GetCurrentAnimatorStateInfo(0).length < anim.GetCurrentAnimatorStateInfo(0).normalizedTime && anim.IsInTransition(0))
//		{
//			meleeSystem.isAttacking = false;
//		}
//		if (!meleeSystem.isAttacking)
//		{
//			anim.SetBool("isAttacking", false);
//		}
//	}
//
//	void DebugGame()
//	{
//		Debug.Log("Layer 1 is " + anim.GetLayerWeight(0));
//		Debug.Log("Layer 2 is " + anim.GetLayerWeight(1));
//	}
//}
