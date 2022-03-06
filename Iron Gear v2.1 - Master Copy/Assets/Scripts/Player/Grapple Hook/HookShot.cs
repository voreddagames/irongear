using UnityEngine;
using System.Collections;

public class HookShot : MonoBehaviour
{
    public static HookShot instance;

    public GameObject hookSpot;

    public float speed = 10.0f;

    public static bool hasLatchedOn = false;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
//		print(hasLatchedOn);
        transform.position = Vector3.MoveTowards(transform.position, hookSpot.transform.position, speed * Time.deltaTime);
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Hook Spot")
        {
            hasLatchedOn = true;
        }
    }
}
