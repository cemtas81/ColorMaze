using DG.Tweening;

using UnityEngine;

public class YoyoWall : MonoBehaviour
{
    public float maxY, duration;
    void Start()
    {
        duration=Random.Range(.3f,maxY);
        transform.DOMoveY(maxY, duration).SetLoops(-1, LoopType.Yoyo);
    }

  
}
