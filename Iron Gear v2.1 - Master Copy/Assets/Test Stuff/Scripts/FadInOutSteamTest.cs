using UnityEngine;
using System.Collections;

public class FadInOutSteamTest : MonoBehaviour
{
	public Transform origSteamTarget;

	public float fadeRate = 0.5f;

	public float minAlpha = 0.2f;
	public float maxAlpha = 0.6f;

	public float speed = 5.0f;

	private float rate;

	private Color matColor;

	// Use this for initialization
	void Start ()
	{
		matColor = GetComponent<Renderer>().material.color;
		matColor.a = maxAlpha;

		GetComponent<Renderer>().material.color = matColor;

		rate = fadeRate;
	}

	// Update is called once per frame
	void Update ()
	{
		matColor.a -= rate * Time.deltaTime;
		GetComponent<Renderer>().material.color = matColor;

		if (matColor.a <= minAlpha)
		{
//			transform.position = origSteamTarget.position;
//			transform.rotation = origSteamTarget.rotation;
			matColor.a = maxAlpha;
			GetComponent<Renderer>().material.color = matColor;
		}
//		transform.position += transform.up * speed * Time.deltaTime;
	}
}
