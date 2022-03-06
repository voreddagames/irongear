using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//parent script for TurretBarrel
public class MasterTurretBarrel : MonoBehaviour
{
    public Rigidbody bullet;

    public float distance = 10.0f,
                 rotationSpeed = 10.0f,
                 shootRate = 1.0f;

    public GameObject player;

    public TurretPhysics turretPhysics;

    public SteamChangeAbility steamChangeAbility;
    
//    private float shootCoolDown = 0.0f;
    
	public float shootCoolDown = 0.0f;

    public void SetUp()
    {
        player = GameObject.FindWithTag("Player");

        steamChangeAbility = player.GetComponent<SteamChangeAbility>();
    }

	// Use this for initialization
	void Start ()
    {
//        ShootCoolDown = shootRate;
	}
}
