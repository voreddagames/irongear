using UnityEngine;
using System.Collections;

//used when enemy explodes
public class RandomShake : MonoBehaviour
{
	public Transform player;

	public float duration = 0.5f;
	public float magnitude = 0.1f;
	
	public bool test = false;
	
	// -------------------------------------------------------------------------
	public void PlayShake()
	{
		StopAllCoroutines();
		StartCoroutine("Shake");
	}
	
	// -------------------------------------------------------------------------
	IEnumerator Shake()
	{
		float elapsed = 0.0f;
		
		Vector3 originalCamPos = Camera.main.transform.position;
		
		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;			
			
			float percentComplete = elapsed / duration;			
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);
			
			// map noise to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= magnitude * damper;
			y *= magnitude * damper;

			Camera.main.transform.position = new Vector3(x + player.position.x, y + player.position.y + GetComponent<CamPositioning>().camOffsetY, originalCamPos.z);
//			Camera.main.transform.position = new Vector3(x + originalCamPos.x, y + originalCamPos.y, originalCamPos.z);
				
			yield return null;
		}
		
//		Camera.main.transform.position = originalCamPos;
	}
}
