using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.IO;

public class SteamChangeAbility : MonoBehaviour
{
    public static SteamChangeAbility instance;

	public GameObject extraUpperSteams;
	public GameObject extraLowerSteams;

    public ParticleSystem[] upperSteams;
	public ParticleSystem[] lowerSteams;

	public bool hasSwitchedColor = false;

	public float maxEmitRate = 10.0f;
	public float minEmitRate = 2.0f;

	public int currentColor = 0;
	public int maxColors = 4;

	public bool hasPressedColorChange = false;

	private bool didChangeColorPC = false;

    private string currentColorType;

    public string CurrentColorType() { return currentColorType; }

	public bool DidChangeColor() { return didChangeColorPC; }

    void Awake()
    {
        instance = this;
    }

	void Start()
	{
		//upperSteams = extraUpperSteams.GetComponentsInChildren<ParticleSystem>();
		//lowerSteams = extraLowerSteams.GetComponentsInChildren<ParticleSystem>();
	}

	public void UpdateSteamChangeAbility ()
    {
		SteamColorChangePower(Input.GetAxis("RightAnalogHorizontal"), Input.GetAxis("RightAnalogVertical"));
		//EmitColorChange();
	}

    void SteamColorChangePower(float inputHoriz, float inputVert)
    {
		/*
		//blue
		if (inputHoriz < -0.25f)
		{
			currentColor = 1;
//			if (currentColor == 1)
//			{
//				currentColor = 0;
//			}
//			else
//			{
//				currentColor = 1;
//			}
		}
		//yellow
		if (inputVert > 0.25f)
		{
			currentColor = 2;
//			if (currentColor == 2)
//			{
//				currentColor = 0;
//			}
//			else
//			{
//				currentColor = 2;
//			}
		}
		//red
		if (inputHoriz > 0.25f)
		{
			currentColor = 3;
//			if (currentColor == 3)
//			{
//				currentColor = 0;
//			}
//			else
//			{
//				currentColor = 3;
//			}
		}
		//normal color
		if (inputVert < -0.25f)
		{
			currentColor = 0;
		}
		*/
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        //cycle through color
        //cycle up
        if (mouseWheel > 0 || Input.GetKeyDown(KeyCode.W))
        {
            if ((currentColor + 1) < maxColors)
            {
                currentColor++;
            }
            else
            {
                currentColor = 0;
            }
			didChangeColorPC = true;
        }
        //cycle down
        if (mouseWheel < 0 || Input.GetKeyDown(KeyCode.Q))
        {
            if ((currentColor - 1) < maxColors)
            {
                currentColor--;
            }
            if (currentColor < 0)
            {
                currentColor = maxColors - 1;
            }
			didChangeColorPC = true;
        }
        //we are no longer pushing color change buttons
		if (mouseWheel == 0 && !Input.GetKeyDown(KeyCode.Q) && !Input.GetKeyDown(KeyCode.W))
		{
			didChangeColorPC = false;
			hasPressedColorChange = false;
		}

//        //apply colors
        switch (currentColor)
        {
        //blue
        case 1:
            UpdateColorType("Blue");
            break;
        //yellow
        case 2:
            UpdateColorType("Yellow");
            break;
        //red
        case 3:
            UpdateColorType("Red");
            break;
        //normal
        default:
            UpdateColorType("White");
            break;
        }

		#region PC input
		if (!hasPressedColorChange && didChangeColorPC)
		{
			//blue
			if (currentColor == 1)
			{
				foreach(ParticleSystem steam in upperSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.blue);
				}
				foreach(ParticleSystem steam in lowerSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.blue);
				}
				hasPressedColorChange = true;
			}
			//yellow
			if (currentColor == 2)
			{
				foreach(ParticleSystem steam in upperSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.yellow);
				}
				foreach(ParticleSystem steam in lowerSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.yellow);
				}
				hasPressedColorChange = true;
			}
			if (currentColor == 3)
			{
				//red
				foreach(ParticleSystem steam in upperSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.red);
				}
				foreach(ParticleSystem steam in lowerSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.red);
				}
				hasPressedColorChange = true;
			}
			//normal
			if (currentColor == 0)
			{
				foreach(ParticleSystem steam in upperSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.white);
				}
				foreach(ParticleSystem steam in lowerSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.white);
				}
				hasPressedColorChange = true;
			}
		}
		#endregion

		#region PS4 controller input
		/*
		//in deadzones
		if (inputVert < 0.2f && inputVert > -0.2f &&
		    inputHoriz < 0.2f && inputHoriz > -0.2f)
		{
			hasPressedColorChange = false;
		}
		//emit steam each time color is changed
		if (!hasPressedColorChange)
		{
			//blue
			if (inputHoriz < -0.25f)
			{
				foreach(ParticleSystem steam in upperSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.blue);
				}
				foreach(ParticleSystem steam in lowerSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.blue);
				}
				hasPressedColorChange = true;
			}
			//yellow
			if (inputVert > 0.25f)
			{
				foreach(ParticleSystem steam in upperSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.yellow);
				}
				foreach(ParticleSystem steam in lowerSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.yellow);
				}
				hasPressedColorChange = true;
			}
			//red
			if (inputHoriz > 0.25f)
			{
				foreach(ParticleSystem steam in upperSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.red);
				}
				foreach(ParticleSystem steam in lowerSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.red);
				}
				hasPressedColorChange = true;
			}
			//normal
			if (inputVert < -0.25f)
			{
				foreach(ParticleSystem steam in upperSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.white);
				}
				foreach(ParticleSystem steam in lowerSteams)
				{
					steam.Emit(steam.transform.position, new Vector3(0, 0, -3), 1, 0.5f, Color.white);
				}
				hasPressedColorChange = true;
			}
		}
*/
		#endregion
    }

    void UpdateColorType(string colorType)
    {
        currentColorType = colorType;
    }
}
