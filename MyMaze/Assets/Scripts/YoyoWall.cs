using UnityEngine;
using DG.Tweening;

public class YoyoWall : MonoBehaviour
{
    public float maxY;
    public GameObject crushEffect;

    void Start()
    {
        int lastLevel = PlayerPrefs.GetInt("lastLevel");
        if (lastLevel > 0 && lastLevel % 10 == 0) // Trigger yoyo effect every 5 levels and multiples of 5
        {
            float duration = Random.Range(3f, maxY);
            transform.DOMoveY(maxY, duration)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}
