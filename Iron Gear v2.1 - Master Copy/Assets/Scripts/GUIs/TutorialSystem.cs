using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialSystem : MonoBehaviour
{
	public float fadeRate = 10.0f;
	public float waitTimeToFade = 1.0f;
	public float rateToAdjustTime = 0.5f;
	public float timeForIFFInfoDisplayed = 2.0f;
	public float bpDisplayTime = 2.0f;
	public float steamBlinkDur = 2.0f;
	public float steamBlinkRate = 0.5f;

	public Image directionalMovementTut;
	public Image jumpTut;
	public Image attackTut;
	public Image IFFInputTut;
	public Image crouchTut;
	public Image IFFInfo;
	public Image BPInfo;
	public Image steamBar;

//	[HideInInspector]
	public bool didCompleteDirTut = false;
//	[HideInInspector]
	public bool didCompleteJumpTut = false;
//	[HideInInspector]
	public bool didCompleteCrouchTut = false;
//	[HideInInspector]
	public bool didCompleteIFFTut = false;
//	[HideInInspector]
	public bool didCompleteAttackTut = false;
//	[HideInInspector]
	public bool didDisplayIFFInfo = false;
//	[HideInInspector]
	public bool didDisplayBPInfo = false;
//	[HideInInspector]
	public bool canDisplayIFFInfo = false;

	public bool canFade = false;
	public bool canResumeTime = false;

	public bool flashSteamBar = false;

	private float waitToFadeDur;
	private float curIFFDisplayDur;
	public float curBPDisplayDur;
	private float curSteamBlinkRate;

	private GameObject player;

	private PlayerMotor playerMotor;
	private PlayerPhysics playerPhysics;
	private SteamChangeAbility steamChangeAbility;

	public bool CanStartIFFTut { get; set; }
	public bool CanStartJumpTut { get; set; }

	void Awake()
	{
		player = GameObject.FindWithTag("Player");
		playerMotor = player.GetComponent<PlayerMotor>();
		playerPhysics = player.GetComponent<PlayerPhysics>();
		steamChangeAbility = player.GetComponent<SteamChangeAbility>();
	}

	// Use this for initialization
	void Start ()
	{
		waitToFadeDur = waitTimeToFade;
		curIFFDisplayDur = timeForIFFInfoDisplayed;
		curBPDisplayDur = bpDisplayTime;
		curSteamBlinkRate = steamBlinkRate;

		//steamBar.gameObject.SetActive(false);

		Color dirMoveTutColor = directionalMovementTut.color;
		dirMoveTutColor.a = 0;
		directionalMovementTut.color = dirMoveTutColor;

		Color jumpTutColor = jumpTut.color;
		jumpTutColor.a = 0;
		jumpTut.color = jumpTutColor;

		Color crouchTutColor = crouchTut.color;
		crouchTutColor.a = 0;
		crouchTut.color = crouchTutColor;

		Color IFFTutColor = IFFInputTut.color;
		IFFTutColor.a = 0;
		IFFInputTut.color = IFFTutColor;

		Color attackTutColor = attackTut.color;
		attackTutColor.a = 0;
		attackTut.color = attackTutColor;

		Color IFFInfoColor = IFFInfo.color;
		IFFInfoColor.a = 0;
		IFFInfo.color = IFFInfoColor;

		Color BPInfoColor = BPInfo.color;
		BPInfoColor.a = 0;
		BPInfo.color = BPInfoColor;
	}
	
	// Update is called once per frame
	void Update ()
	{
//		DisplayDirectionalTut();
//		DisplayJumpTut();
		ActivateCrouchTut();
		DisplayIFFTut();
		DisplayAttackTut();
		DisplayIFFInfo();
		DisplayBPInfo();
	}

	void DisplayDirectionalTut()
	{
		if (!didCompleteDirTut)
		{
			//wait a bit before displaying image
			if (waitToFadeDur > 0)
			{
				waitToFadeDur -= Time.deltaTime;
			}
			//now display by fading in
			else if (!canFade)
			{
				if (Time.timeScale > 0.3f)
				{
					Time.timeScale -= rateToAdjustTime * Time.deltaTime; 
				}
				else
				{
					Time.timeScale = 0;

					Color dirMoveTutColor = directionalMovementTut.color;
//					dirMoveTutColor.a += fadeRate * Time.deltaTime;
					dirMoveTutColor.a = 1;
					directionalMovementTut.color = dirMoveTutColor;

					//after faded in prepare to fade out when player presses directional key
					if (Input.GetAxisRaw("Horizontal") != 0)
					{
						Time.timeScale = 1;
						canFade = true;
					}
				}
			}
		
			//fade out
			if (canFade)
			{
				//go back to normal time
//				if (Time.timeScale < 1)
//				{
//					Time.timeScale += rateToAdjustTime * Time.deltaTime;
//				}
//				else
//				{
//					Time.timeScale = 1;
//				}
				Color dirMoveTutColor = directionalMovementTut.color;
				dirMoveTutColor.a -= fadeRate * Time.deltaTime;
				directionalMovementTut.color = dirMoveTutColor;

				//faded out
				//tutorial complete
				if (directionalMovementTut.color.a <= 0)
				{
					waitToFadeDur = waitTimeToFade;
					didCompleteDirTut = true;
					canFade = false;
				}
			}
		}
	}

	void DisplayJumpTut()
	{
		if (CanStartJumpTut && didCompleteDirTut && !didCompleteJumpTut)
		{
			if (!canFade)
			{
				if (Time.timeScale > 0.3f)
				{
					Time.timeScale -= rateToAdjustTime * Time.deltaTime; 
				}
				else
				{
					Time.timeScale = 0;

					Color jumpTutColor = jumpTut.color;
					jumpTutColor.a = 1;
					jumpTut.color = jumpTutColor;

					if (Input.GetButtonDown("Jump"))
					{
						Time.timeScale = 1;
						canFade = true;
					}
				}
			}
			else
			{
				//go back to normal time
//				if (Time.timeScale < 1)
//				{
//					Time.timeScale += rateToAdjustTime * Time.deltaTime;
//				}
//				else
//				{
//					Time.timeScale = 1;
//				}
				Color jumpTutColor = jumpTut.color;
				jumpTutColor.a -= fadeRate * Time.deltaTime;
				jumpTut.color = jumpTutColor;

				if (jumpTut.color.a <= 0)
				{
					waitToFadeDur = waitTimeToFade;
					didCompleteJumpTut = true;
					canFade = false;
				}
			}
		}
	}

	void ActivateCrouchTut()
	{
		if (CanStartIFFTut && !didCompleteCrouchTut)
		{
			if (!didCompleteJumpTut)
			{
				Color jumpTutColor = jumpTut.color;
				jumpTutColor.a = 0;
				jumpTut.color = jumpTutColor;

				didCompleteJumpTut = true;
			}
			if (!canResumeTime)
			{
				//slow time down
				if (Time.timeScale > 0.3f)
				{
					Time.timeScale -= rateToAdjustTime * Time.deltaTime; 
				}
				else
				{
					Time.timeScale = 0;

					Color crouchTutColor = crouchTut.color;
					crouchTutColor.a = 1;
					crouchTut.color = crouchTutColor;

					if (Input.GetAxisRaw("Vertical") < -0.5f)
					{
						Time.timeScale = 1;
						canResumeTime = true;
					}
				}
			}

			if (canResumeTime)
			{
				//go back to normal time
//				if (Time.timeScale < 1)
//				{
//					Time.timeScale += rateToAdjustTime * Time.deltaTime;
//				}
//				else
//				{
//					Time.timeScale = 1;
//				}

				Color crouchTutColor = crouchTut.color;
				crouchTutColor.a -= fadeRate * Time.deltaTime;
				crouchTut.color = crouchTutColor;

				if (crouchTut.color.a <= 0)
				{
					didCompleteCrouchTut = true;
				}
			}
		}
	}

	void DisplayIFFTut()
	{
		if (didCompleteCrouchTut && !didCompleteIFFTut)
		{
			if (!canFade)
			{
				//slow time down
				if (Time.timeScale > 0.3f)
				{
					Time.timeScale -= rateToAdjustTime * Time.deltaTime; 
				}
				else
				{
					Time.timeScale = 0;

					Color IFFTutColor = IFFInputTut.color;
					IFFTutColor.a = 1;
					IFFInputTut.color = IFFTutColor;

					if (steamChangeAbility.CurrentColorType() == "Red")
					{
						Time.timeScale = 1;
						canFade = true;
					}
				}
			}
			else
			{
				//go back to normal time
//				if (Time.timeScale < 1)
//				{
//					Time.timeScale += rateToAdjustTime * Time.deltaTime;
//				}
//				else
//				{
//					Time.timeScale = 1;
//				}

				Color IFFTutColor = IFFInputTut.color;
				IFFTutColor.a -= fadeRate * Time.deltaTime;
				IFFInputTut.color = IFFTutColor;

				if (IFFInputTut.color.a <= 0)
				{
					didCompleteIFFTut = true;
					canFade = false;
				}
			}
		}
	}

	void DisplayAttackTut()
	{
		if (didCompleteIFFTut && !didCompleteAttackTut)
		{
			if (!canFade)
			{
				//slow time down
				if (Time.timeScale > 0.3f)
				{
					Time.timeScale -= rateToAdjustTime * Time.deltaTime; 
				}
				else
				{
					Time.timeScale = 0;

					Color attackTutColor = attackTut.color;
					attackTutColor.a = 1;
					attackTut.color = attackTutColor;

					if (Input.GetMouseButtonDown(0))
					{
						Time.timeScale = 1;
						canFade = true;
					}
				}
			}
			else
			{
				//go back to normal time
//				if (Time.timeScale < 1)
//				{
//					Time.timeScale += rateToAdjustTime * Time.deltaTime;
//				}
//				else
//				{
//					Time.timeScale = 1;
//				}

				Color attackTutColor = attackTut.color;
				attackTutColor.a -= fadeRate * Time.deltaTime;
				attackTut.color = attackTutColor;

				if (attackTut.color.a <= 0)
				{
					didCompleteAttackTut = true;
					canFade = false;
				}
			}
		}
	}

	void DisplayIFFInfo()
	{
		if (canDisplayIFFInfo && didCompleteAttackTut && !didDisplayIFFInfo)
		{
			steamBar.gameObject.SetActive(true);

			if (waitToFadeDur > 0)
			{
				waitToFadeDur -= Time.deltaTime;
			}
			if (waitToFadeDur <= 0 && IFFInfo.color.a < 1 && curIFFDisplayDur > 0)
			{
				Color IFFInfoColor = IFFInfo.color;
				IFFInfoColor.a += fadeRate * Time.deltaTime;
				IFFInfo.color = IFFInfoColor;
			}
			if (IFFInfo.color.a >= 1 && curIFFDisplayDur > 0)
			{
				curIFFDisplayDur -= Time.deltaTime;
			}
			if (curIFFDisplayDur <= 0)
			{
				Color IFFInfoColor = IFFInfo.color;
				IFFInfoColor.a -= fadeRate * Time.deltaTime;
				IFFInfo.color = IFFInfoColor;

				if (IFFInfo.color.a <= 0)
				{
					waitToFadeDur = waitTimeToFade;
					didDisplayIFFInfo = true;
				}
			}
		}
	}

	void DisplayBPInfo()
	{
		if (didDisplayIFFInfo && !didDisplayBPInfo)
		{
			if (waitToFadeDur > 0)
			{
				waitToFadeDur -= Time.deltaTime;
			}
			if (waitToFadeDur <= 0 && BPInfo.color.a < 1 && curBPDisplayDur > 0)
			{
				Color BPInfoColor = BPInfo.color;
				BPInfoColor.a += fadeRate * Time.deltaTime;
				BPInfo.color = BPInfoColor;
			}
			if (BPInfo.color.a >= 1 && curBPDisplayDur > 0)
			{
				curBPDisplayDur -= Time.deltaTime;
			}
			if (curBPDisplayDur <= 0)
			{
				Color BPInfoColor = BPInfo.color;
				BPInfoColor.a -= fadeRate * Time.deltaTime;
				BPInfo.color = BPInfoColor;
				
				if (BPInfo.color.a <= 0)
				{
					waitToFadeDur = waitTimeToFade;
					didDisplayBPInfo = true;
				}
			}
		}
	}

	IEnumerator DisplaySteamBar()
	{
//		steamBar.enabled = false;
//		steamBar.gameObject.SetActive(false);
//		steamBlinkDur -= Time.deltaTime;
		while (curBPDisplayDur > 0)
		{
			yield return new WaitForSeconds(steamBlinkRate);
			flashSteamBar = !flashSteamBar;
			if (flashSteamBar)
			{
				steamBar.gameObject.SetActive(true);
			}
			else
			{
				steamBar.gameObject.SetActive(false);
			}
//			yield return null;
		}
	
//		steamBar.enabled = true;
	}
}
