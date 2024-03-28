using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixLevels : MonoBehaviour
{

    [SerializeField] AudioMixer masterMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] float _multiplier = 30f;
    [SerializeField] string _musicVol = "MusicVol";
    [SerializeField] string _sfxVol = "SfxVol";
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;
    private bool _disableToggleEvent;
    private bool _disableToggleEvent2;

    private void Awake()
    {
        musicSlider.onValueChanged.AddListener(SetMusicLvl);
        sfxSlider.onValueChanged.AddListener(SetSfxLvl);
        musicToggle.onValueChanged.AddListener(SetMusic);
        sfxToggle.onValueChanged.AddListener(SetSfx);
    }
    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat(_musicVol, musicSlider.value);
        sfxSlider.value = PlayerPrefs.GetFloat(_sfxVol, sfxSlider.value);
    }
    public void SetSfxLvl(float sfxLvl)
    {
        masterMixer.SetFloat(_sfxVol, Mathf.Log10(sfxLvl) * _multiplier);
        _disableToggleEvent = true;
        sfxToggle.isOn = sfxSlider.value > sfxSlider.minValue;
        _disableToggleEvent = false;
    }

    public void SetMusicLvl(float musicLvl)
    {
        masterMixer.SetFloat(_musicVol, Mathf.Log10(musicLvl) * _multiplier);
        _disableToggleEvent2 = true;
        musicToggle.isOn = musicSlider.value > musicSlider.minValue;
        _disableToggleEvent2 = false;
    }
    public void SetSfx(bool sfx)
    {
        if (_disableToggleEvent)
            return;
        if (sfx)

            sfxSlider.value = sfxSlider.maxValue;

        else

            sfxSlider.value = sfxSlider.minValue;

    }

    public void SetMusic(bool music)
    {
        if (_disableToggleEvent2)
            return;
        if (music)

            musicSlider.value = musicSlider.maxValue;

        else

            musicSlider.value = musicSlider.minValue;

    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(_musicVol, musicSlider.value);
        PlayerPrefs.SetFloat(_sfxVol, sfxSlider.value);
    }
}
