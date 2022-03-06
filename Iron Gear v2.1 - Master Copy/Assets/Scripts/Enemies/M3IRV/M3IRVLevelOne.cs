using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class M3IRVLevelOne : MonoBehaviour
{
	public static M3IRVLevelOne instance;

	public GameObject pickupTarget;

	public List<GameObject> objectsToThrow;
	public List<GameObject> waypoints;
	public List<GameObject> targetsForThrownObjects;
	public List<GameObject> scaffoldingObjectsToDestroy;

	public GameObject currentObjectToThrow;
	public GameObject targetWaypoint;
	public GameObject currentTargetForThrownObject;
	public GameObject currentScaffoldingToDestroy;

	public bool isQTEActive;
	public bool tempCheckForAnimation;

	public float speed = 40.0f;
	public float thrownObjectSpeed = 30.0f;

	public float QTEFireRocketsDuration = 20.0f;

	public int QTEIndex = 0;
	public int tempQTEIndex;

	private GameObject player;
	private PlayerMotor playerMotor;

	// Use this for initialization
	void Start ()
	{
		instance = this;

		player = GameObject.FindWithTag("Player");
		pickupTarget = GameObject.Find("Target for Pickup");

		playerMotor = player.GetComponent<PlayerMotor>();

		//create the Lists
		objectsToThrow = new List<GameObject>();
		waypoints = new List<GameObject>();
		targetsForThrownObjects = new List<GameObject>();
		scaffoldingObjectsToDestroy = new List<GameObject>();

		//add the objects to the coresponding lists by their tags
		foreach(GameObject thrownObjectTag in GameObject.FindGameObjectsWithTag("Throwable Object"))
		{
			objectsToThrow.Add(thrownObjectTag);
		}

		foreach(GameObject waypointTag in GameObject.FindGameObjectsWithTag("Waypoint"))
		{
			waypoints.Add(waypointTag);
		}

		foreach(GameObject thrownObjectTargetTag in GameObject.FindGameObjectsWithTag("Thrown Object Target"))
		{
			targetsForThrownObjects.Add(thrownObjectTargetTag);
		}

		foreach(GameObject scaffoldingObject in GameObject.FindGameObjectsWithTag("Scaffolding to Destroy"))
		{
			scaffoldingObjectsToDestroy.Add(scaffoldingObject);
		}

		//sort the lists by name to make sure the QTEs execute in the proper order
		objectsToThrow = objectsToThrow.OrderBy(thrownObject => thrownObject.name).ToList();
		waypoints = waypoints.OrderBy(waypoint => waypoint.name).ToList();
		targetsForThrownObjects = targetsForThrownObjects.OrderBy(thrownObjectTarget => thrownObjectTarget.name).ToList();
		scaffoldingObjectsToDestroy = scaffoldingObjectsToDestroy.OrderBy(scaffolding => scaffolding.name).ToList();
		//set the first waypoint for m3irv to head towards
		targetWaypoint = waypoints[QTEIndex];
	}
	
	// Update is called once per frame
	void Update ()
	{
		Movement();
		ResetPlayerControl();
	}

	void Movement()
	{
		if (!isQTEActive)
		{
			//if the QTE is not activated move towards the waypoint
			transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.transform.position, speed * Time.deltaTime);
		}
		//QTE is activated by the player when they collide with the trigger
		else
		{
			//don't do anything unless m3irv is at the waypoint
			//and check the tempQTEindex to prevent the QTEindex from endlessly counting up
			if (transform.position == targetWaypoint.transform.position &&
				tempQTEIndex == QTEIndex)
			{
				QTEIndex++;
			}
			switch(QTEIndex)
			{
			//execute the furnace throw
			case 1:
				QTEThrowObject();
				break;
			//execute the cog throw
			case 2:
				QTEThrowObject();
				break;
			case 3:
				QTEFireRockets();
				break;
			//we will have a separate method executing the other QTEs.
			}
		}
	}

	void QTEThrowObject()
	{
		if (!currentObjectToThrow)
		{
			//if the object to throw is not set we will set the objects from the lists
			currentTargetForThrownObject = targetsForThrownObjects[QTEIndex - 1];
			currentObjectToThrow = objectsToThrow[QTEIndex - 1];
		}
		else
		{
			//check for completion of m3irv's throw animation.
			if (!tempCheckForAnimation)
			{
				//keep this object at his hands
				currentObjectToThrow.transform.position = pickupTarget.transform.position;
			}
			else
			{
				//move the object towards the target
				currentObjectToThrow.transform.position = Vector3.MoveTowards(currentObjectToThrow.transform.position,
					currentTargetForThrownObject.transform.position, thrownObjectSpeed * Time.deltaTime);


				//#####PLACED BELOW FOR TESTING######
				currentScaffoldingToDestroy = scaffoldingObjectsToDestroy[QTEIndex - 1];
				//blow up the current scaffolding
				//DestroyScaffolding destroyScaffolding = currentScaffoldingToDestroy.GetComponent<DestroyScaffolding>();
				//destroyScaffolding.BreakScaffolding();

				//we make the object null to stop calling the movement code
				currentObjectToThrow = null;
				//we check to make sure we still have waypoints in the list for m3irv to move towards
				if (QTEIndex < waypoints.Count)
				{
					//then we set the waypoint from the list
					targetWaypoint = waypoints[QTEIndex];
				}
				//we set the temp index to the current index to allow us to perform the next actions
				tempQTEIndex = QTEIndex;
				//we are done executing the QTE
				isQTEActive = false;

				//######PLACED ABOVE FOR TESTING######
			}
		}
		//when it reaches the target...
		if (currentObjectToThrow.transform.position == currentTargetForThrownObject.transform.position)
		{
//			currentScaffoldingToDestroy = scaffoldingObjectsToDestroy[QTEIndex - 1];
//			//blow up the current scaffolding
//			DestroyScaffolding destroyScaffolding = currentScaffoldingToDestroy.GetComponent<DestroyScaffolding>();
//			destroyScaffolding.BreakScaffolding();
//
//			//we make the object null to stop calling the movement code
//			currentObjectToThrow = null;
//			//we check to make sure we still have waypoints in the list for m3irv to move towards
//			if (QTEIndex < waypoints.Count)
//			{
//				//then we set the waypoint from the list
//				targetWaypoint = waypoints[QTEIndex];
//			}
//			//we set the temp index to the current index to allow us to perform the next actions
//			tempQTEIndex = QTEIndex;
//			//we are done executing the QTE
//			isQTEActive = false;
		}
	}

	void QTEFireRockets()
	{
		QTEFireRocketsDuration -= Time.deltaTime;

		if (QTEFireRocketsDuration > 0)
		{
			FireM3IRVRockets.instance.FireRockets();
		}
		else
		{
			//we check to make sure we still have waypoints in the list for m3irv to move towards
			if (QTEIndex < waypoints.Count)
			{
				//then we set the waypoint from the list
				targetWaypoint = waypoints[QTEIndex];
			}
			//we set the temp index to the current index to allow us to perform the next actions
			tempQTEIndex = QTEIndex;
			//we are done executing the QTE
			isQTEActive = false;
		}
	}

	void ResetPlayerControl()
	{
		if (!isQTEActive)
		{
			//playerMotor.GiveControl();
		}
	}
}
