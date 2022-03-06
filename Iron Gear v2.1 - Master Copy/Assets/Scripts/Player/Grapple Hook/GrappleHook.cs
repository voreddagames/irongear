using UnityEngine;
using System.Collections.Generic;

public class GrappleHook : MonoBehaviour
{
    public static GrappleHook instance;

    public float rangeToHookSpot = 30.0f,
                distToHookSpot = 0.0f,
                curDist = 0.0f;

    public GameObject[] allHookSpots;

    public bool hasShotHook = false;

    public GameObject currentHookSpot,
                    hook,
                    swing;
    
    void Awake()
    {
        instance = this;

        allHookSpots = GameObject.FindGameObjectsWithTag("Hook Spot");
    }

	// Use this for initialization
	void Start ()
    {

	}

	// Update is called once per frame
	void Update ()
    {
        FindCurrentHookSpot();

        ShootHookShot();
	}

    void FindCurrentHookSpot()
    {
        curDist = rangeToHookSpot;

        currentHookSpot = null;

        foreach (GameObject hookSpot in allHookSpots)
        {
            distToHookSpot = hookSpot.transform.position.z - transform.position.z;

            if (distToHookSpot < curDist && distToHookSpot > -curDist)
            {
                curDist = distToHookSpot;

                currentHookSpot = hookSpot;
            }
        }
    }

    void ShootHookShot()
    {
        if (Input.GetMouseButtonDown(0) && currentHookSpot)
        {
            Instantiate(hook, transform.position, transform.rotation);
        }
    }
}
