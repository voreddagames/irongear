using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretExplode : MonoBehaviour
{
	public Transform mainTurret;
	public Transform piecesParent;

	public float fadeTime = 1.0f;
	public float waitToFade = 3.0f;

	public List<GameObject> allPieces;
	private Renderer[] allPiecesRenderer;
	private MeshCollider[] allPiecesCollider;

	public bool hasExploded = false;
	public int numOfChildren = 0;

	void Start()
	{
		foreach(Transform piece in piecesParent)
		{
			if (piece.tag == "Raycast Ignore")
			{
				allPieces.Add(piece.gameObject);

			}
		}

		allPiecesRenderer = GetComponentsInChildren<Renderer>();
		allPiecesCollider = GetComponentsInChildren<MeshCollider>();

		foreach (Renderer piece in allPiecesRenderer)
		{
			piece.enabled = false;
		}
		foreach (MeshCollider colliderPiece in allPiecesCollider)
		{
			colliderPiece.enabled = false;
		}

		transform.position = mainTurret.position;
	}

	// Update is called once per frame
	void Update ()
	{
		Follow();
		Fade();
	}

	void Fade()
	{
		//destroy the object after the turret has died
		//###CHANGE THIS SO ENEMY CAN RESPAWN###
		if (hasExploded)
		{
			Destroy(gameObject, waitToFade);
		}
	}

	void Follow()
	{
		//follow the turrect controller
		if (mainTurret)
		{
			transform.position = mainTurret.position;
		}
	}

	public void Explode()
	{
		foreach (MeshCollider colliderPiece in allPiecesCollider)
		{
			colliderPiece.enabled = false;
		}
		//render the broken pieces
		foreach (Renderer piece in allPiecesRenderer)
		{
			piece.enabled = true;
		}
		hasExploded = true;
	}
}
