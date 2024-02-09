
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject settings;
    public AudioClip clip1;
    public AudioSource effectSounds;
    public void StartLevel()
    {
        SceneManager.LoadScene(2);
        effectSounds.PlayOneShot(clip1);
    }
    public void Shop()
    {
        SceneManager.LoadScene(1);
        effectSounds.PlayOneShot(clip1);
    }
    public void MainScene()
    {
        SceneManager.LoadScene(0);
        effectSounds.PlayOneShot(clip1);
    }
    public void SettingsMenu()
    {
        settings.SetActive(!settings.activeSelf);
        effectSounds.PlayOneShot(clip1);
    }
}
