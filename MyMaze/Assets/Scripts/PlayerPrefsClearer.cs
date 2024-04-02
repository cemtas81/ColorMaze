using UnityEngine;

public class PlayerPrefsClearer : MonoBehaviour
{

    [ContextMenu("Clear PlayerPrefs")]
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs cleared.");
    }
    private void Start()
    {
        if (PlayerPrefs.GetInt("FirstGame")!=1)
        {

            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("FirstGame", 1);
            PlayerPrefs.Save();
        }
    }
}


