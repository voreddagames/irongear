using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ExplodedLenoxMine : MonoBehaviour
{
	public Transform mainLenoxMine;
	public Transform piecesParent;
	
	public float fadeTime = 1.0f;
	public float waitToFade = 3.0f;
	
	public List<GameObject> allPieces;
	private Renderer[] allPiecesRenderer;
	
	public bool hasExploded = false;
	public int numOfChildren = 0;

	// Use this for initialization
	void Start ()
	{
		//add the damaged model into a list so we may randomly scatter them in an explosion force
		foreach(Transform piece in piecesParent)
		{
			if (piece.tag == "Raycast Ignore")
			{
				allPieces.Add(piece.gameObject);
				
			}
		}
		
		allPiecesRenderer = GetComponentsInChildren<Renderer>();
		//do not render the pieces yet
		foreach (Renderer piece in allPiecesRenderer)
		{
			piece.enabled = false;
		}
		
		transform.position = mainLenoxMine.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Follow();
		Fade();
	}

	void Fade()
	{
		//if the mine has exploded destroy that enemy
		//######we may need to change this as the enemies are not respawning#########
		if (hasExploded)
		{
			Destroy(gameObject, waitToFade);
		}
	}
	
	void Follow()
	{
		//follow the position of the mine at all times so we may explode at the correct spot
		//#####WILL CHANGE FOR RESOURCE PURPOSES.####
		if (mainLenoxMine)
		{
			transform.position = mainLenoxMine.position;
		}
	}

	public void Explode()
	{
		//Now that the mine has exploded, render the damaged pieces
		foreach (Renderer piece in allPiecesRenderer)
		{
			piece.enabled = true;
		}
		hasExploded = true;
	}
}
