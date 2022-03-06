using UnityEngine;
using System.Collections;

public class ParticleSystemLayerOrder : MonoBehaviour
{
	public float layerHeight = 0.0f;

	private Vector3 pos;

	public GameObject cam;

	void Start()
	{
		pos = transform.localPosition;
		cam = Camera.main.gameObject;
	}

	void Update()
	{
		//set the explosion to be properly viewed
		Vector3 camDiff = cam.transform.position - transform.position;

		if (camDiff.magnitude > layerHeight || layerHeight < 0.0f)
		{
			transform.position = transform.parent.TransformPoint(pos) + camDiff.normalized * layerHeight;
		}
	}
}
