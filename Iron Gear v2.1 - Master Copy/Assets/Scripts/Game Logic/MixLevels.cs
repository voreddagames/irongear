using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MixLevels : MonoBehaviour
{
	public AudioMixer masterMixer;

	public void SetfxLvl(float sfxLvl)
	{
		masterMixer.SetFloat("sfxVol", sfxLvl);
	}

	public void SetMusicVol(float musicLvl)
	{
		masterMixer.SetFloat("musicVol", musicLvl);
	}
}
