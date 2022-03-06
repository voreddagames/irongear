using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTECamPosition : MonoBehaviour
{
	public float rotationSpeed = 20.0f;
	public float tiltAngle = -40.0f;
	
	// Update is called once per frame
	void Update ()
	{
		QTEPosition();
	}

	void QTEPosition()
	{
		if (M3IRVLevelOne.instance.isQTEActive)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(tiltAngle, 0, 0)), rotationSpeed * Time.deltaTime);
		}
		else if (transform.rotation.x != 0)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, 0, 0)), rotationSpeed * Time.deltaTime);
		}
	}
}
