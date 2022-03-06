using UnityEngine;
using System.Collections;

public class HookSystem : MonoBehaviour
{
	public static HookSystem instance;

	public Transform hookSpot;
	public Transform player;

	public float hookSpeed = 10.0f;

	public bool isLatched = false;
	public bool hasShot = false;

	public bool isPlayerUnder = false;
	private FixedJoint fixedJoint;

	private LineRenderer lineRenderer;

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
		FireHook();

		#region Trash
//		if (Input.GetMouseButtonDown(0))
//		{
//			hasShot = true;
//		}


//		Vector3 targetDir = player.position - transform.position;
//		float angle = Vector3.Angle(targetDir, -transform.up);
//
//		if (angle < 5)
//		{
//			isPlayerUnder = true;
//		}
		#endregion
	}

	void FireHook()
	{
		if (!hookSpot)
		{
			return;
		}

		if (hasShot)
		{
			transform.position = Vector3.MoveTowards(transform.position, hookSpot.position, hookSpeed * Time.deltaTime);

			lineRenderer = GetComponent<LineRenderer>();
			
			lineRenderer.SetPosition(0, player.position);
			lineRenderer.SetPosition(1, transform.position);
		}
		if (transform.position == hookSpot.position)
		{
			isLatched = true;
		}
	}
}
