
using System.Collections;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    private SimpleMazeGenerator m_Generator;
    public float rotationSpeed = 90f; // Degrees per second

    void Start()
    {
        m_Generator = FindObjectOfType<SimpleMazeGenerator>();
        m_Generator.UfoMove(this.gameObject);
        // Start a continuous rotation
        StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        while (true)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);
            yield return null;
        }
    }
}
