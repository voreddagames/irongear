using UnityEngine;
using System.Collections;

public class CalculateAspectRatio : MonoBehaviour
{
	public Camera cam;

	public float xScreen;
	public float yScreen;
	public float screenAspect;

	// Use this for initialization
	void Awake ()
	{
		cam = GetComponent<Camera>();

		yScreen = Screen.height;
		xScreen = Screen.width;

		SetAspectRatio();
	}
	
	// Update is called once per frame
	void Update ()
	{
//		SetAspectRatio();
	}

	void SetAspectRatio()
	{
//		screenAspect = cam.aspect;
//		cam.rect = new Rect(screenAspect, 0, screenAspect, 1);
		screenAspect = xScreen / yScreen;
	}
}
