using UnityEngine;
using System.Collections;

public class GrappleSystem : MonoBehaviour
{
	public static GrappleSystem instance;

	public float max = 60.0f;
	public float min = -60.0f;

	public float swingSpeed = 10.0f;

	private float clampAngle;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
//		float horiz = Input.GetAxis("Horizontal");
		clampAngle += swingSpeed * Time.deltaTime;
		clampAngle = Mathf.Clamp(clampAngle, min, max);

//		Quaternion rotLimit = Quaternion.Euler(clampAngle * horiz, 0, 0);

		if (HookSystem.instance.isLatched)
		{
//			transform.rotation = rotLimit;
//			transform.rotation = Quaternion.AngleAxis(maxSwingLimit * input, transform.right);
//			transform.rotation = Quaternion.Slerp(transform.rotation, to.rotation, swingSpeed * input * Time.deltaTime);
		}

		if (HookSystem.instance.isPlayerUnder)
		{

		}
	}
}
