
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject settings;
    public AudioClip clip1;
    public AudioSource effectSounds;
    public AudioSource music;
    private Animator ani;
    public Toggle musicT;
    public Toggle sfxT;
    private void Start()
    {
        if (settings != null)
        {
            ani = settings.GetComponent<Animator>();
        }
        Time.timeScale = 1;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& settings != null)
        {
            SettingsMenu();
        }
    }
    public void MuteVolume()
    {
        musicT.isOn = !musicT.isOn;
    }
    public void MuteSfx()
    {
        sfxT.isOn = !sfxT.isOn;
    }
    
    public void StartLevel()
    {
        StartCoroutine(GameOn());
        effectSounds.PlayOneShot(clip1);
    }
    public void Shop()
    {
        StartCoroutine(Shopping());
        effectSounds.PlayOneShot(clip1);
    }
    public void Options()
    {
        StartCoroutine(Opts());
        effectSounds.PlayOneShot(clip1);
    }
    public void LeaderBoard()
    {
        StartCoroutine(Leader());
        effectSounds.PlayOneShot(clip1);
    }
    public void MainScene()
    {
        StartCoroutine(Main());
        effectSounds.PlayOneShot(clip1);
    }
    public void SettingsMenu()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        if (Time.timeScale==1)
        {
            ani.SetBool("UnSlide",true);
            ani.SetBool("Slide", false);
        }
        else if (Time.timeScale==0)
        {
            ani.SetBool("UnSlide", false);
            ani.SetBool("Slide", true);
        }
        effectSounds.PlayOneShot(clip1);
    }
    public void ExitGame()
    {
        effectSounds.PlayOneShot(clip1);
        StartCoroutine(Quit());
    }
    IEnumerator Leader()
    {
        yield return new WaitForSeconds(.2f);
        SceneManager.LoadScene(4);
    } 
    IEnumerator GameOn()
    {
        yield return new WaitForSecondsRealtime(.2f);
        SceneManager.LoadScene(2);
    }
    IEnumerator Main()
    {
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(.2f);
        SceneManager.LoadScene(0);
    }
    IEnumerator Shopping()
    {
        yield return new WaitForSeconds(.2f);
        SceneManager.LoadScene(1);
    }
    IEnumerator Opts()
    {
        yield return new WaitForSeconds(.2f);
        SceneManager.LoadScene(3);
    }
    IEnumerator Quit()
    {
        yield return new WaitForSeconds(.2f);
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
