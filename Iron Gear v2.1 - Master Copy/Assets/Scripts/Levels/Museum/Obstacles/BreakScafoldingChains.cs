using UnityEngine;
using System.Collections;

public class BreakScafoldingChains : MonoBehaviour
{
	public static BreakScafoldingChains instance;

	public bool isBroken = false;

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
		BreakChains();
	}

	void BreakChains()
	{
		FixedJoint[] joints = GetComponents<FixedJoint>();

		//break the support chains when the final rocket hits this scaffolding
		if (isBroken)
		{
			for (int i = 0; i < joints.Length - 2; i++)
			{
				Destroy(GetComponent<FixedJoint>());
			}
		}
	}
}
