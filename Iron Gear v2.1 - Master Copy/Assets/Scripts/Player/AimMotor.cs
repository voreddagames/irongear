using UnityEngine;
using System.Collections;

public class AimMotor : MonoBehaviour
{
	public float rotationSpeed = 10.0f;
	
	public int faceDir = 0;

    public bool isGunEquiped = false;

	[HideInInspector]
    public bool hasControl;

	public bool isUpPressed;

	private PlayerPhysics playerPhysics;
	private GameObject playerControllerTarget;

    public void GiveAimControl() { hasControl = true; }
    public void RemoveAimControl() { hasControl = false; }
    public bool HasAimControl() { return hasControl; }
    
	void Awake()
	{
		playerControllerTarget = GameObject.FindWithTag("Player");
		playerPhysics = playerControllerTarget.GetComponent<PlayerPhysics>();
	}

	void Start()
	{
		faceDir = playerPhysics.faceDir;
	}

    void Update()
    {
		AimControls();
		AimInput();
    }

	void AimInput()
	{
//		float dPadUp = Input.GetAxisRaw("D-pad Up");

		//equip gun via keyboard
		if (Input.GetKeyDown(KeyCode.G))
		{
			isGunEquiped = !isGunEquiped;
		}
		//equip gun via controller
//		if (dPadUp > 0)
//		{
//			if (!isUpPressed)
//			{
//				isGunEquiped = !isGunEquiped;
//			}
//			isUpPressed = true;
//		}
//		if (dPadUp == 0)
//		{
//			isUpPressed = false;
//		}

		if (isGunEquiped)
		{
			GiveAimControl();
		}
		else
		{
			RemoveAimControl();
		}
	}

	#region THIS METHOD OF AIMMING MAY BE CHANGING!!!!!!
	void AimControls()
	{
		Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
		
		if (!hasControl)
		{
			faceDir = playerPhysics.faceDir;
		}
		else
		{
			if (screenPos.x > Input.mousePosition.x)
			{
				faceDir = -1;
			}
			else
			{
				faceDir = 1;
			}
			playerPhysics.faceDir = faceDir;
		}
		Quaternion targetRot = Quaternion.LookRotation(Vector3.right * faceDir);
		Quaternion rot = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

		transform.rotation = rot;
	}
	#endregion
}
