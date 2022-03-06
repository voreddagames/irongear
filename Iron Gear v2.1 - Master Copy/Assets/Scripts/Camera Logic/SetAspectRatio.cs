using UnityEngine;
using System.Collections;

public class SetAspectRatio : MonoBehaviour
{
	public GameObject aspectCam;

	private CalculateAspectRatio calcAspectRatio;

	// Use this for initialization
	void Start ()
	{
		calcAspectRatio = aspectCam.GetComponent<CalculateAspectRatio>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		SetRatio();
	}

	void SetRatio()
	{
		Camera.main.rect = new Rect(0, 0, Mathf.Abs(1 - calcAspectRatio.screenAspect), 1);
	}
}
