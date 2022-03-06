using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD instance;

	public Image continueButtonWhenDead;
	public Slider healthBar;
	public Slider steamBar;

	public RectTransform healthBarGear;

	public RectTransform selectedColorIndicator;
	public RectTransform smallGearGroup;
	public RectTransform smallGearL;
	public RectTransform smallGearR;

	public float healthBarGearSpinSpeed = 10.0f;
	public float curAngle = 0.0f;

//    public float xColorPowerBarPos = Screen.width * (3.4f / 6.55f),
//                 yColorPowerBarPos = Screen.height * (1.1f / 6.3f),
//                 powerMeterHeight = 0.0f;
//	public float powerMeterWidth = 100.0f;

//    public Texture powerBarTexture;

	private GameObject player;

	private PlayerPhysics playerPhysics;
	private PlayerHealth playerHealth;
	private BoilingPointsSystem boilingPointsSystem;

    void Awake()
    {
        instance = this;

		player = GameObject.FindWithTag("Player");
		boilingPointsSystem = player.GetComponent<BoilingPointsSystem>();
		playerHealth = player.GetComponent<PlayerHealth>();
		playerPhysics = player.GetComponent<PlayerPhysics>();
    }

	void Start()
	{
		//steamBar.maxValue = boilingPointsSystem.startBoilTemperature;
		//steamBar.minValue = boilingPointsSystem.minBoilTemperature;

		//continueButtonWhenDead.gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update ()
    {
		//AdjustHealthBar();
		IndicateColorSelection();
		//AdjustSteamBar();
		//DisplayResetLevel();
        //restrain from going outside certain values
        //powerMeterHeight = Mathf.Clamp(powerMeterHeight, 1, Screen.height / 2);
	}

	void AdjustSteamBar()
	{
		//size steam bar in accordance with available boiling points
		steamBar.value = boilingPointsSystem.boilTemp;
	}

	void AdjustHealthBar()
	{
		//adjust health bar size
		if (playerHealth.isHit)
		{
			healthBarGear.Rotate(Vector3.forward * healthBarGearSpinSpeed);
		}
		healthBar.value = playerHealth.health * 4;
	}

	void DisplayResetLevel()
	{
		//display the continue button
		if (playerPhysics.IsDead)
		{
			continueButtonWhenDead.gameObject.SetActive(true);
		}
		else
		{
			continueButtonWhenDead.gameObject.SetActive(false);
		}
	}

	void IndicateColorSelection()
	{
		switch (SteamChangeAbility.instance.currentColor)
		{
			//blue
		case 1:
			curAngle = 90.0f;
			break;
			//yellow
		case 2:
			curAngle = 0.0f;
			break;
			//red
		case 3:
			curAngle = -90.0f;
			break;
		default:
			curAngle = 180.0f;
			break;
		}
		//rotate the image
		smallGearL.localRotation = Quaternion.RotateTowards(smallGearL.localRotation, Quaternion.AngleAxis(curAngle, Vector3.forward), 200.0f * Time.deltaTime);
		smallGearR.localRotation = Quaternion.RotateTowards(smallGearR.localRotation, Quaternion.AngleAxis(curAngle, Vector3.forward), 200.0f * Time.deltaTime);
		selectedColorIndicator.rotation = Quaternion.RotateTowards(selectedColorIndicator.rotation, Quaternion.AngleAxis(curAngle, Vector3.forward), 200.0f * Time.deltaTime);
		smallGearGroup.rotation = Quaternion.RotateTowards(smallGearGroup.rotation, Quaternion.AngleAxis(curAngle, Vector3.forward), 200.0f * Time.deltaTime);
	}

//    void OnGUI()
//    {
//        //create the gui power bar
////        GUI.Box(new Rect(xColorPowerBarPos, yColorPowerBarPos, powerMeterWidth, powerMeterHeight), powerBarTexture);
//
//        //temporary reset button
//        if (GUI.Button(new Rect(Screen.width * 0.5f, Screen.height - 50, 50, 50), "Reset"))
//        {
//            Application.LoadLevel(1);
//        }
//    }
}
