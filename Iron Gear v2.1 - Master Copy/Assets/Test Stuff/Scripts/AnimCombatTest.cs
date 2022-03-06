using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCombatTest : MonoBehaviour
{
	public int attackIndex;

	private Animator anim;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		ActivateAttack();
	}

	void ActivateAttack()
	{
		if (Input.GetMouseButtonDown(0))
		{
			anim.SetTrigger("isAttackButton");
			Attack();
		}
		if (anim.GetCurrentAnimatorStateInfo(0).IsName("Left Straight Punch") &&
			anim.GetBool("isAttacking"))
		{
			ComboReset();
		}
	}

	public void Attack()
	{
		if (attackIndex == 0)
		{
			AttackPossible();
			attackIndex = 1;
		}

		if (attackIndex != 0)
		{
			Combo();
			attackIndex++;
//			if (anim.GetBool("isAttacking"))
//			{
//				anim.SetBool("isAttacking", false);
//			}
		}
	}

	public void AttackPossible()
	{
		anim.SetBool("isAttacking", true);
		anim.SetLayerWeight(1, 1.0f);
	}

	public void Combo()
	{
		//Use Switch Case
		if (attackIndex == 1)
		{
			anim.Play("Left Straight Punch");
		}
		if (attackIndex == 2)
		{
			anim.Play("Right Straight Punch");
		}
		if (attackIndex == 3)
		{
			anim.Play("Right Uppercut");
		}
		if (attackIndex == 4)
		{
			anim.Play("Right Round Kick");
			ComboReset();
		}
	}

	public void ComboReset()
	{
		anim.SetBool("isAttacking", false);
		anim.SetLayerWeight(1, 0.0f);
		attackIndex = 0;
	}
}
