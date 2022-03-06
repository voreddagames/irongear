using UnityEngine;
using System.Collections;

public class LightAnimationSpeed : MonoBehaviour
{
	public float spinSpeed = 0.5f;

	private Animation anim;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animation>();
	}

	void Update()
	{
		//play the animation at a specific speed
		anim["spin"].speed = spinSpeed;
	}
}
