
using UnityEngine;

public class WarpHole : MonoBehaviour
{
    private SimpleMazeGenerator m_Generator;
    private void Start()
    {
        m_Generator = FindObjectOfType<SimpleMazeGenerator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_Generator.Warp(other.gameObject);
        }
    }
}
