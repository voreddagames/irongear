using UnityEngine;
using System.Collections;

//used for moving platforms
public class StickPlayer : MonoBehaviour
{
    void OnTriggerStay(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            col.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Player")
        {
//			PlatformMove.instance.isOnPlatform = false;
			transform.DetachChildren();
        }
    }
}