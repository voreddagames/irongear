using UnityEngine;
using System.Collections;

public class LookAtHookSpot : MonoBehaviour
{
    public GameObject hookSpot,
                    grapple,
                    rope;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.up = hookSpot.transform.position - transform.position;

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(grapple, transform.position, transform.rotation);
            Instantiate(rope, transform.position, transform.rotation);
        }
	}
}
