using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimStateMachineTest : MonoBehaviour
{
	public int curAnimationState;

	private Animator anim;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
