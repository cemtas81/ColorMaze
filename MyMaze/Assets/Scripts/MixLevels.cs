using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MixLevels : MonoBehaviour {

	public AudioMixer masterMixer;

	public void SetSfxLvl(float sfxLvl)
	{
		masterMixer.SetFloat("SfxVol", Mathf.Log10(sfxLvl));
	}

	public void SetMusicLvl (float musicLvl)
	{
		masterMixer.SetFloat ("MusicVol",Mathf.Log10( musicLvl));
	}
}
