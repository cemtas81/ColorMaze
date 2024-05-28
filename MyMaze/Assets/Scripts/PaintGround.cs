
using UnityEngine;
using DG.Tweening;


public class PaintGround : MonoBehaviour
{

    public Material currentcolor;
    public Material nextcolor;
    private SimpleMazeGenerator m_generator;
    public ParticleSystem part;
    //private CountDownTimer m_countdownTimer;
    private Collider coll;
    // Start is called before the first frame update
    void Start()
    {
        //m_countdownTimer = FindAnyObjectByType<CountDownTimer>();
        currentcolor = GetComponent<MeshRenderer>().material;
        part = GetComponentInChildren<ParticleSystem>();
        m_generator = FindAnyObjectByType<SimpleMazeGenerator>();
        coll = GetComponent<Collider>();
        m_generator.currentCell++;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (currentcolor != nextcolor)
            {
                //currentcolor.DOColor(nextcolor.color, 0.1f);
                currentcolor = nextcolor; // because of painted doesn't increase again
                                          //m_countdownTimer.AddSecond();
                                          // Create the sequence
                Sequence rotationSequence = DOTween.Sequence();

                // Disable the collider before starting the rotation
                rotationSequence.AppendCallback(() => coll.enabled = false);

                // Perform the rotation
                rotationSequence.Append(transform.DOLocalRotate(new Vector3(-90, -90, 0), 0.3f));

                // Enable the collider after the rotation is complete
                rotationSequence.AppendCallback(() => coll.enabled = true);
                if (other.gameObject.TryGetComponent(out PlayerMovement plyr))
                {
                    plyr.isBonus = true;
                }

                part.Play();

                m_generator.currentCell--;
                if (m_generator.currentCell < 1)
                {
                    m_generator.FinishCurrentLevel();
                }
            }
            else
            {
                //m_countdownTimer.ExtractOneSecond();
                if (other.gameObject.TryGetComponent(out PlayerMovement plyr))
                {
                    plyr.isDeploy = true;
                }
            }
        }

    }

}

