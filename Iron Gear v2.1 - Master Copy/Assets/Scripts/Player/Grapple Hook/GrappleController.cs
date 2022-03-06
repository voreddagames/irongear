using UnityEngine;
using System.Collections;

public class GrappleController : MonoBehaviour
{
    public float zipSpeed = 50.0f,
                zipLineLength = 50.0f;

    public float dist = 0.0f;

    public GameObject hook;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        hook = GameObject.FindWithTag("Hook");

        if (!hook)
        {
            return;
        }
        else
        {
//            if (!Hook.hasLatched)
//            {
                dist = Vector3.Distance(hook.transform.position, transform.position);
//            }
//            else
//            {
                zipLineLength = Mathf.Clamp(zipLineLength, 3, dist);

                if (zipLineLength > 3)
                {
                    if (Input.GetMouseButton(0))
                    {
                        zipLineLength -= zipSpeed * Time.deltaTime;
                        GetComponent<Rigidbody>().position = Vector3.MoveTowards(transform.position, hook.transform.position - Vector3.up * zipLineLength, zipSpeed * Time.deltaTime);
                    }
                    if (Input.GetMouseButtonUp(0))
                    {
//                        Hook.hasLatched = false;
                    }
                }
                else
                {
                    zipLineLength = 3;
                }
//            }
        }

        //if (Hook.hasLatched)
        //{
        //    zipLineLength = Mathf.Clamp(zipLineLength, 3, dist);

        //    rigidbody.position = Vector3.MoveTowards(transform.position, hook.position - Vector3.up * zipLineLength, zipSpeed * Time.deltaTime);

        //    if (zipLineLength > 3)
        //    {
        //        if (Input.GetMouseButton(0))
        //        {
        //            zipLineLength -= zipSpeed * Time.deltaTime;
        //        }
        //        if (Input.GetMouseButtonUp(0))
        //        {
        //            Hook.hasLatched = false;
        //        }
        //    }
        //    else
        //    {
        //        zipLineLength = 3;
        //    }
        //    //if (Input.GetMouseButton(1) && zipLineLength < dist)
        //    //{
        //    //    zipLineLength += climbSpeed * Time.deltaTime;
        //    //}
        //}
	}
}