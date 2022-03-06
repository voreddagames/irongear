using UnityEngine;
using System.Collections;

//might be able to place this on the parent of the tread objects.
//try that during the optimization process
public class TreadPhysics : MonoBehaviour
{
	public float speed = 10.0f;
	public float additionalSpeed = 3.0f;

//	public float height = 0.1f;

//	public Transform target;

	public GameObject turret;

//	public CapsuleCollider treadBase;

//	private float refVel = 0.0f;

//	private Vector3 faceNormal, surfaceAlign, hitPoint;
//	private Quaternion targetRot;

	public TurretPhysics turretPhysics;

	private Animation anim;

//	public Vector3 vel;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animation>();
//		vel = GetComponent<Rigidbody>().velocity;
		turretPhysics = turret.GetComponent<TurretPhysics>();
	}

	void Update ()
	{
		ProcessMovement();
//		ProcessNormalSnapping();
	}

	void ProcessMovement()
	{
		anim.Play("Spin");
		//move the tread according the the direction the turret is moving
		speed = Mathf.SmoothStep(speed, turretPhysics.curDirVel.x * 0.05f, 1);
		//play the animation at the speed and direction of the turret
		anim["Spin"].speed = speed;
	}

//	void ProcessNormalSnapping()
//	{
//		RaycastHit hitInfo;
//		//create the Ray
//		Ray ray = new Ray(transform.position, -transform.up);
//
//		//cast the ray to the tread base's collider only
//		if (target.GetComponent<Collider>().Raycast(ray, out hitInfo, 1))
//		{
//			//get the normal and hit point
//			faceNormal = hitInfo.normal;
//			hitPoint = hitInfo.point;
//		}
//
//		//position the tread
//		Vector3 targetPos = (hitPoint + transform.up * height);
//		transform.position = targetPos;
//		//calculate the surface normal
//		surfaceAlign = (transform.position + faceNormal) - transform.position;
//
//		//rotate the tread
//		targetRot = Quaternion.FromToRotation(transform.up, surfaceAlign) * transform.rotation;
//		transform.rotation = targetRot;
//
//		//move the tread according the the direction the turret is moving
//		speed = Mathf.SmoothStep(speed, turretScript.curDirVel.x * 0.5f, 1);
////		transform.InverseTransformDirection(vel);
//		vel = transform.right * speed;
//		GetComponent<Rigidbody>().velocity = vel;
//	}
}
