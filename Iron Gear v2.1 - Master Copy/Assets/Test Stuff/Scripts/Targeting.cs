using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Targeting : MonoBehaviour
{
	//make a list command calling all the targets as transforms.
	public List<Transform> targets;
	public Transform selectTarget;
	
	private Transform myTransfom;
	
	void Start()
	{
		//initialize targets as a new list of transforms.
		targets = new List<Transform>();
		//selectTarget is initialized as null because none are selected.
		selectTarget = null;
		myTransfom = transform;
		//call this function.
		AddAllEnemies();
	}
	
	public void AddAllEnemies()
	{
		//make an Array of game objects reefenced as go.
		//find all game objects with the Enemy tag.
		GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
		//cycle through the each game object named enemy in the go variable array.
		foreach (GameObject enemy in go)
			//for each enemy.transform (the current enemy targeted) selected call the AddTarget function.
			AddTarget(enemy.transform);
	}
	
	//make enemy of Transform type
	public void AddTarget(Transform enemy)
	{
		//add the currently targeted enemy game object to the end of the Array.
		targets.Add(enemy);
	}
	
	private void SortTargetsByDist()
	{
		//sort the targets in order depending upon the distance between the player and the closest enemy object.
		//delegate is refering functions as a parameter.
		//This Sort call is being given the two references to compare them.
		targets.Sort(delegate(Transform t1, Transform t2)
		             {
			//return the comparison distance values between the position of t1 and the player compared to the distance of the position of t2 and the player.
			return Vector3.Distance(t1.position, myTransfom.position).CompareTo(Vector3.Distance(t2.position, myTransfom.position));
		});
	}
	
	private void TargetEnemy()
	{
		//if the selected target is null meaning none is seleceted.
		if (selectTarget == null)
		{
			//call the SortTargetsByDist function.
			SortTargetsByDist();
			//make the closest target be the selected target. 
			selectTarget = targets[0];
		}
		
		else
		{
			//set the index variable.  IndexOf means
			//targets will be the index of the selected target
			//in relation to where it is in the list.
			int index = targets.IndexOf(selectTarget);
			//if the index is less than the count of the targets (the count being how many game objects in the list) minus one
			//minus one because the count needs to be one less than the total list count.
			//basically if index is less than two.
			if (index < targets.Count - 1)
			{
				//increase the index by one to cycle through
				index++;
			}
			
			//use this else statement to go back to the first game object in the list.
			else
			{
				index = 0;
			}
			//call the deselect function.
			DeselectTarget();
			//refer the currently selected target as the selected target in the list.
			selectTarget = targets[index];
		}
		//call this function when TargetEnemy is called.
		SelectTarget();
	}
	
	private void SelectTarget()
	{

	}
	
	private void DeselectTarget()
	{
		//make the deselcted target null.
		selectTarget = null;
	}
	
	void Update()
	{
		//when the Tab key is pressed
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			//call this function.
			TargetEnemy();
		}
	}
}
