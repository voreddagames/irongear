using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
	public int num;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			num = 0;
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			num = 1;
		}
//		if (Input.GetKeyDown(KeyCode.Alpha2))
//		{
//			num = 2;
//		}

		if (num == 0)
		{
			print("number zero");
		}
		else if (Input.GetMouseButton(0))
		{
			print ("success!");
		}
	}
}
