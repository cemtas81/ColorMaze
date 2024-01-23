
using UnityEngine;
using DG.Tweening;


public class PaintGround : MonoBehaviour
{

    public Material currentcolor;
    public Material nextcolor;
    private SimpleMazeGenerator m_generator;
    public ParticleSystem part;

    // Start is called before the first frame update
    void Start()
    {
     
        currentcolor = GetComponent<MeshRenderer>().material;
        part = GetComponentInChildren<ParticleSystem>();
        m_generator=FindAnyObjectByType<SimpleMazeGenerator>();
        m_generator.currentCell++;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && currentcolor != nextcolor)
        {
            currentcolor.DOColor(nextcolor.color, 0.1f);
            currentcolor = nextcolor; // because of painted doesn't increase again
           
            part.Play();
            m_generator.currentCell--;
            if (m_generator.currentCell<1)
            {
                m_generator.FinishCurrentLevel();
            }
        }
        

    }

}

