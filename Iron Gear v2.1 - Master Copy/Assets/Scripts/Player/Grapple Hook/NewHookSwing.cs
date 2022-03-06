using UnityEngine;
using System.Collections;

public class NewHookSwing : MonoBehaviour
{
    public float rotationSpeed = 10.0f,
                targetAngle = 0.0f,
                input = 0.0f;

    public Quaternion rot;

    public ConfigurableJoint configJoint;

	// Use this for initialization
	void Start ()
    {
		configJoint = GetComponent<ConfigurableJoint>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Vector3 pivotPoint = hook.transform.position - transform.position;

        //if (!Hook.hasLatched)
        //{
        //    dist = Vector3.Distance(GrappleHook.instance.currentHookSpot.transform.position, transform.position);
        //}

        //float counterAngle = 0.0f;

        //Vector3 objPos = Camera.main.WorldToScreenPoint(transform.position);
        
        //float angle = Mathf.Atan2(hook.transform.position.y - objPos.y, hook.transform.position.x - objPos.x) * Mathf.Rad2Deg;

        //Quaternion rot = Quaternion.Euler(new Vector3(angle, 0, 0));

        //Vector3 pos = rot * new Vector3(0, -zipLineLength, 0) + hook.transform.position;

        ////transform.rotation = rot;
        ////transform.position = pos;

        //if (Hook.hasLatched)
        //{
        //    zipLineLength = Mathf.Clamp(zipLineLength, 3, dist);

        //    //transform.position = pos;
            
        //}

        //transform.up = pivotPoint;

        //transform.LookAt(hook.transform.position);

        input = Input.GetAxis("Horizontal");
        //transform.Rotate(Vector3.right * maxTargetVelocity * input);
        //configJoint.targetRotation = new Quaternion(-maxTargetVelocity * input, 0, 0, 1);

//        float configRotX = configJoint.targetRotation.z;

        targetAngle += input * rotationSpeed * Time.deltaTime;
        targetAngle = Mathf.Clamp(targetAngle, -30, 30);
        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle, transform.forward);
        configJoint.targetRotation = targetRotation;

        //if (input != 0)
        //{
        //    counterAngle += input * maxTargetVelocity * Time.deltaTime;
        //    transform.position += Vector3.forward * maxTargetVelocity * input * Time.deltaTime;
        //}
	}
}
