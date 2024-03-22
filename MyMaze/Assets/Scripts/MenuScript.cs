
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject settings;
    public AudioClip clip1;
    public AudioSource effectSounds;
    public AudioSource music;
    public void MuteVolume()
    {
        if (music.isPlaying)
        {
            music.Stop();
        }
        else
        {
            music.Play();
        }
    }
    public void MuteSfx()
    {
        if (effectSounds.isPlaying)
        {
            effectSounds.Stop();
        }
        else
        {
            effectSounds.Play();
        }
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
        settings.SetActive(!settings.activeSelf);
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
        yield return new WaitForSeconds(.2f);
        SceneManager.LoadScene(2);
    }
    IEnumerator Main()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(.2f);
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
