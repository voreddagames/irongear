using UnityEngine;
using System.Collections;

public class RocketThrust : MonoBehaviour
{
	public int xTile = 3;
	public int yTile = 1;

	public float cycleSpeed = 3.0f;

	private Vector2 tileSize;
	private int lastIndex = -1;
	private Renderer thisRenderer;
	// Use this for initialization
	void Start ()
	{
		tileSize = new Vector2(1.0f / xTile, 1.0f / yTile);
		thisRenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		ThrustAnimation();
	}

	void ThrustAnimation()
	{
		int i = (int)(Time.time * cycleSpeed) % (xTile * yTile);

		if (i != lastIndex)
		{
			int hIndex = i % xTile;
			int vIndex = i / yTile;

			Vector2 offset = new Vector2(hIndex * tileSize.x, 1.0f - tileSize.y - vIndex * tileSize.y);

			thisRenderer.material.mainTextureOffset = offset;
			thisRenderer.material.mainTextureScale = tileSize;

			lastIndex = i;
		}
	}
}
