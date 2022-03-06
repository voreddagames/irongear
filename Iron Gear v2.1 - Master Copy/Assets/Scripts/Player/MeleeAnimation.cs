using UnityEngine;
using System.Collections;

public class MeleeAnimation : MonoBehaviour
{
	public Transform animatedModel;
	public Transform rightShoulder;
	public Transform leftShoulder;

	public float animSpeed = 1.3f;

	private MeleeSystem meleeSystem;

	private Animation anim;

	void Awake()
	{
		meleeSystem = GetComponent<MeleeSystem>();
	}

	void Start()
	{
		anim = animatedModel.GetComponent<Animation>();
	}

	#region Tier One Attack animations
	public void T1LeftCross()
	{
		anim["T1 left straight punch"].speed = animSpeed;

		anim.Play("T1 left straight punch");

		//		if (anim.IsPlaying("T1 left straight punch"))
		//		{
		//			meleeSystem.canAttack = false;
		//		}

		float animLength = anim["T1 left straight punch"].length;
		float animTime = anim["T1 left straight punch"].time;

		if (animTime >= (animLength - 0.1f))
		{
			Reset();
		}
		if (animTime > (animLength * 0.75f))
		{
			meleeSystem.hasAttacked = false;
		}
	}

	public void T1RightUppercut()
	{
		anim["T1 right uppercut"].speed = animSpeed;

		//play upper cut animation
		anim.Play("T1 right uppercut");

		//		if (anim.IsPlaying("T1 right uppercut"))
		//		{
		//			meleeSystem.canAttack = false;
		//		}

		//get length and time of the animation
		float animLength = anim["T1 right uppercut"].length;
		float animTime = anim["T1 right uppercut"].time;

		//wait for the animation to finish then reset
		if (animTime >= (animLength - 0.1f))
		{
			Reset();
		}
		if (animTime > (animLength * 0.7f))
		{
			meleeSystem.hasAttacked = false;
		}
	}

	public void T1RightStomp()
	{
		anim["T1 right leg stomp"].speed = animSpeed;

		anim.Play("T1 right leg stomp");

		//		if (anim.IsPlaying("T1 right leg stomp"))
		//		{
		//			meleeSystem.canAttack = false;
		//		}

		float animLength = anim["T1 right leg stomp"].length;
		float animTime = anim["T1 right leg stomp"].time;

		if (animTime >= (animLength - 0.1f))
		{
			Reset();
		}

		if (animTime > (animLength * 0.6f))
		{
			meleeSystem.hasAttacked = false;
		}
	}

	public void T1RightRoundKick()
	{
		anim["T1 right round kick"].speed = animSpeed;

		anim.Play("T1 right round kick");

		//		if (anim.IsPlaying("T1 right round kick"))
		//		{
		//			meleeSystem.canAttack = false;
		//		}

		float animLength = anim["T1 right round kick"].length;
		float animTime = anim["T1 right round kick"].time;

		if (animTime >= (animLength - 0.1f))
		{
			Reset();
		}

		if (animTime > (animLength * 0.6f))
		{
			meleeSystem.hasAttacked = false;
		}
	}

	public void T1LeftSpinKick()
	{
		anim["T1 forward left spin kick"].speed = animSpeed;

		anim.Play("T1 forward left spin kick");

		if (anim.IsPlaying("T1 forward left spin kick"))
		{
			meleeSystem.canAttack = false;
		}

		float animLength = anim["T1 forward left spin kick"].length;
		float animTime = anim["T1 forward left spin kick"].time;

		if (animTime >= (animLength - 0.08f))
		{
			Reset();
		}
	}

	public void T1RightCross()
	{		
		anim["T1 right straight punch"].speed = animSpeed;

		anim.Play("T1 right straight punch");

		if (anim.IsPlaying("T1 right straight punch"))
		{
			meleeSystem.canAttack = false;
		}

		float animLength = anim["T1 right straight punch"].length;
		float animTime = anim["T1 right straight punch"].time;

		if (animTime >= (animLength - 0.08f))
		{
			Reset();
		}
	}

	public void T1CrouchLeftPunch()
	{
		anim["T1 crouch left punch"].speed = animSpeed;
		anim.Play("T1 crouch left punch");

		if (anim.IsPlaying("T1 crouch left punch"))
		{
			meleeSystem.canAttack = false;
		}

		float animLength = anim["T1 crouch left punch"].length;
		float animTime = anim["T1 crouch left punch"].time;

		if (animTime >= (animLength - 0.08f))
		{
			Reset();
		}
	}

	public void T1CrouchRightKick()
	{
		anim["T1 crouch right kick"].speed = animSpeed;
		anim.Play("T1 crouch right kick");

		if (anim.IsPlaying("T1 crouch right kick"))
		{
			meleeSystem.canAttack = false;
		}

		float animLength = anim["T1 crouch right kick"].length;
		float animTime = anim["T1 crouch right kick"].time;

		if (animTime >= (animLength - 0.08f))
		{
			Reset();
		}
	}

	public void T1CrouchRightPunch()
	{
		anim["T1 crouch right punch"].speed = animSpeed;
		anim.Play("T1 crouch right punch");

		if (anim.IsPlaying("T1 crouch right punch"))
		{
			meleeSystem.canAttack = false;
		}

		float animLength = anim["T1 crouch right punch"].length;
		float animTime = anim["T1 crouch right punch"].time;

		if (animTime >= (animLength - 0.08f))
		{
			Reset();
		}
	}

	public void T1AirLeftJab()
	{
		anim["T1 inair left jab"].speed = animSpeed;
		anim.Play("T1 inair left jab");

		if (anim.IsPlaying("T1 inair left jab"))
		{
			meleeSystem.canAttack = false;
		}

		float animLength = anim["T1 inair left jab"].length;
		float animTime = anim["T1 inair left jab"].time;

		if (animTime >= (animLength - 0.08f))
		{
			Reset();
		}
	}

	public void T1AirRightKick()
	{
		anim["T1 inair right kick"].speed = animSpeed;
		anim.Play("T1 inair right kick");

		if (anim.IsPlaying("T1 inair right kick"))
		{
			meleeSystem.canAttack = false;
		}

		float animLength = anim["T1 inair right kick"].length;
		float animTime = anim["T1 inair right kick"].time;

		if (animTime >= (animLength - 0.08f))
		{
			Reset();
		}
	}

	public void T1AirRightHook()
	{
		anim["T1 inair right hook"].speed = animSpeed;
		anim.Play("T1 inair right hook");

		if (anim.IsPlaying("T1 inair right hook"))
		{
			meleeSystem.canAttack = false;
		}

		float animLength = anim["T1 inair right hook"].length;
		float animTime = anim["T1 inair right hook"].time;

		if (animTime >= (animLength - 0.08f))
		{
			Reset();
		}
	}
	#endregion

	void Reset()
	{
		//allowAttack
		meleeSystem.canAttack = true;
	}
}
