using UnityEngine;
using System.Collections;

public class PlayerGrappleSystem : MonoBehaviour
{
	public Transform hook;

	public float rotSpeed = 10.0f;
	public float zipLineSpeed = 10.0f;

	// Use this for initialization
	void Start ()
	{
//		initialVector = transform.position - hook.position;
//
//		Vector3 angles = transform.eulerAngles;
//		angle = angles.x;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		LookAtMouse();
		SpawnGrapplePoint();
		ZipToHook();
		#region Trash
//		if (HookSystem.instance.isLatched)
//		{
//			Vector3 dist = hook.position - transform.position;
//
//			Vector3 curPos = transform.position;
//			float horiz = Input.GetAxis("Horizontal") * GrappleSystem.instance.swingSpeed * Time.deltaTime;
//			if (Input.GetButtonDown("Horizontal"))
//			{
////				rigidbody.AddForceAtPosition(dist * horiz, transform.position, ForceMode.Impulse);
////				
//			}
//			angle = horiz;
//			curPos.x = Mathf.Clamp(curPos.x, -10, 10);
//
////			float dist = Vector3.Distance(transform.position, hook.position);
//
////			transform.parent = HookSystem.instance.hookSpot;
//
//			Quaternion rot = Quaternion.Euler(angle, 0, 0);
//			Vector3 pos = rot * -dist + hook.position;
//
//			float rightAngle = Vector3.Angle(dist, hook.forward);
//			float leftAngle = Vector3.Angle(dist, -hook.forward);
//
//			transform.rotation = rot;
//
//			//right side
//			if (rightAngle > 10 && horiz > 0)
//			{
//				transform.position = pos;
//			}
//			if (leftAngle > 10 && horiz < 0)
//			{
//				transform.position = pos;
//			}
//			if (horiz == 0)
//			{
////				transform.position = rot * Vector3.Lerp(transform.position, , GrappleSystem.instance.swingSpeed * Time.deltaTime);
//			}

//			transform.position = curPos;
//			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, hook.position.z), GrappleSystem.instance.swingSpeed * Time.deltaTime);
//		}
		#endregion
	}

	void LookAtMouse()
	{
		//create plane
		Plane playerPlane = new Plane(Vector3.right, transform.position);
		//find mouse position
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		float hitDist = 0.0f;
		
		if (playerPlane.Raycast(ray, out hitDist))
		{
			Vector3 targetPoint = ray.GetPoint(hitDist);

			//look at the mouse cursor
			transform.LookAt(targetPoint);
		}
	}

	void SpawnGrapplePoint()
	{
		RaycastHit hitInfo;

		if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 10.0f))
		{
			if (hitInfo.collider)
			{
				Debug.DrawLine(transform.position, hitInfo.point, Color.red);

				if (Input.GetMouseButtonDown(0))
				{
					Transform clone = Instantiate(hook, hitInfo.point, Quaternion.identity) as Transform;

					HookSystem.instance.hookSpot = clone;
					HookSystem.instance.hasShot = true;
				}
			}
		}
		else
		{
			Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
		}
	}

	void ZipToHook()
	{
		if (HookSystem.instance.isLatched)
		{
			transform.position = Vector3.MoveTowards(transform.position, HookSystem.instance.hookSpot.position, zipLineSpeed * Time.deltaTime);
		}
	}

//	static float ClampAngle(float clampAngle, float min, float max)
//	{
//		if (clampAngle < -360)
//		{
//			clampAngle += 360;
//		}
//		if (clampAngle > 360)
//		{
//			clampAngle -= 360;
//		}
//		return Mathf.Clamp(clampAngle, min, max);
//	}
}
