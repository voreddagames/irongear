using UnityEngine;
using System.Collections;

public class Rope : MonoBehaviour
{
    public JointSpring hingeSpring;
    public HingeJoint hinge;

    public Rigidbody pivot;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
//        if (GrappleJoint.hasLatched)
//        {
            if (!hinge)
            {
                hinge = gameObject.AddComponent<HingeJoint>();
            }
            else
            {
                GetComponent<HingeJoint>().connectedBody = pivot;

                hingeSpring = GetComponent<HingeJoint>().spring;

                hinge.useSpring = true;

                hingeSpring.damper = 10;
                hingeSpring.spring = 2;

                GetComponent<HingeJoint>().anchor = new Vector3(0, 1, 0);

                GetComponent<HingeJoint>().spring = hingeSpring;

                GetComponent<Rigidbody>().isKinematic = false;
            }

            transform.parent = null;
//        }
	}
}
