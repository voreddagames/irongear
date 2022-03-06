using UnityEngine;
using System.Collections;

//used to better organize scripts
public class M3IRVManager : MonoBehaviour
{
	private M3IRVPhysics m3irvPhysics;
	private M3IRVAnimation m3irvAnimation;
	private M3IRVLevelOne m3irvLevelOne;

	void Awake()
	{
		m3irvPhysics = GetComponent<M3IRVPhysics>();
		m3irvAnimation = GetComponent<M3IRVAnimation>();
		m3irvLevelOne = GetComponent<M3IRVLevelOne>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		m3irvPhysics.UpdateM3IRVPhysics();
		m3irvAnimation.UpdateM3IRVAnimation();
		//m3irvLevelOne.UpdateM3IRVLevelOne();
	}
}
