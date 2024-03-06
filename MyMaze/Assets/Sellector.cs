using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Sellector : MonoBehaviour
{
    public Vector3 boxSize = new Vector3(1f, 1f, 1f);
    public LayerMask layerMask;
    public float moveDuration = 1f;
    public float moveDistance = 1f;
    public float returnDuration = 1f;
    private Collider[] colliders = new Collider[10]; // Pre-allocated array to store colliders

    void Start()
    {
        // Get the position of the GameObject this script is attached to
        Vector3 center = transform.position;

        // Call Physics.OverlapBoxNonAlloc to find colliders within the box
        int numColliders = Physics.OverlapBoxNonAlloc(center, boxSize / 2, colliders, Quaternion.identity, layerMask);

        // If there are colliders found
        if (numColliders > 0)
        {
            // Choose a random index within the range of found colliders
            int randomIndex = Random.Range(0, numColliders);

            // Get the collider at the random index
            Collider randomCollider = colliders[randomIndex];

            // If the collider has a YoyoWall component, stop its yoyo movement
            if (randomCollider.TryGetComponent<YoyoWall>(out var yoyoWall))
            {
                yoyoWall.enabled = false; // Disable the YoyoWall component
            }

            // Move the collider down using DOTween
            randomCollider.transform.DOMoveY( - moveDistance, moveDuration).OnComplete(() =>
            {
                randomCollider.transform.position = new Vector3(randomCollider.transform.position.x, -moveDistance, randomCollider.transform.position.z);
                // Move the collider back to its original position
                randomCollider.transform.DOMoveY( + moveDistance, returnDuration);

                // If the collider has a YoyoWall component, enable it again
                if (randomCollider.TryGetComponent<YoyoWall>(out var yoyoWall))
                {
                    yoyoWall.enabled = true;
                }

            });
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a wireframe box in the scene view to visualize the area where overlap detection will occur
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }
}
