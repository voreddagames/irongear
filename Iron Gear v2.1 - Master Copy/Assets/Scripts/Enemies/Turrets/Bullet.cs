using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	public float timeOut = 5.0f;
	public float bulletSpeed = 0.0f;

	public GameObject explosion;

	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

    void FixedUpdate()
    {
		RocketPhysics();
    }

	void RocketPhysics()
	{
		//start timer
		timeOut -= Time.deltaTime;

		//time out so explode
		if (timeOut <= 0)
		{
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy(gameObject);
		}
		//move rocket
		rb.AddForce(transform.right * bulletSpeed * Time.deltaTime, ForceMode.VelocityChange);
	}

    void OnTriggerEnter(Collider col)
    {
		//explode as long as we aren't colliding with certain objects
        if (col.tag != "Raycast Ignore" && col.tag != "Barrier")
        {
			Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}