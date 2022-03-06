using UnityEngine;
using System.Collections;

public class ActivateUI : MonoBehaviour
{
	public Transform uiObj;

	void Awake()
	{
		uiObj.gameObject.SetActive(true);
	}
}
