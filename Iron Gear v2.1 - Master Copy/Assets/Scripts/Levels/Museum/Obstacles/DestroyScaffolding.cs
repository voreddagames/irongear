using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScaffolding : MonoBehaviour
{
	public void BreakScaffolding()
	{
		//later add in the destroyed model before deleting this object
		Destroy(gameObject);
	}
}
