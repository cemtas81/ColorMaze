using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement: MonoBehaviour
{

    public bool isMoving;
    public bool isReachTheCorner;
    public float speed = 50f;
    public float speedy;
    public Rigidbody rb;
    public AudioSource watersound;
    public bool forsound;
    public int minSwipeRange = 500;
    Vector3 direction;
    Vector3 nextWallPos;

    Vector2 swipePosFirst;
    Vector2 swipePosSecond;
    Vector2 currentSwipe;

    void Start()
    {
        watersound = GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody>();
        isMoving = true;
        _ = StartCoroutine(nameof(StartDelay));
 
    }

    private void FixedUpdate()
    {
        if (!isMoving )
        {
            rb.velocity = speed * direction;
            //_ = StartCoroutine(nameof(TimeDelay));

        }
        if (nextWallPos != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextWallPos) < 0.5)
            {
                isMoving = false;
                direction = Vector3.zero;
                nextWallPos = Vector3.zero;
            }
        }
    }
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(20f);
        isMoving=false;
    }
    void Update()
    {
        speedy = rb.velocity.magnitude;

        if (Input.GetMouseButtonDown(0))
        {
            swipePosFirst = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButton(0))
        {
            swipePosSecond = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButtonUp(0) && speedy < 1)
        {
            // Calculate swipe direction
            currentSwipe = swipePosSecond - swipePosFirst;

            // Check if swipe distance is significant
            if (currentSwipe.sqrMagnitude >= minSwipeRange)
            {
                // Forward
                if (currentSwipe.y > Mathf.Abs(currentSwipe.x))
                {
                    SetDirection(Vector3.forward);
                    if (forsound == false)
                    {
                        forsound = true;
                        StartCoroutine(nameof(SoundManager));
                    }
                }
                // Backward
                else if (currentSwipe.y < -Mathf.Abs(currentSwipe.x))
                {
                    SetDirection(Vector3.back);
                    //StartCoroutine(nameof(TimeDelay));
                    if (forsound == false)
                    {
                        forsound = true;
                        StartCoroutine(nameof(SoundManager));
                    }
                }
                // Right
                else if (currentSwipe.x > Mathf.Abs(currentSwipe.y))
                {
                    SetDirection(Vector3.right);
                    //StartCoroutine(nameof(TimeDelay));
                    if (forsound == false)
                    {
                        forsound = true;
                        StartCoroutine(nameof(SoundManager));
                    }
                }
                // Left
                else if (currentSwipe.x < -Mathf.Abs(currentSwipe.y))
                {
                    SetDirection(Vector3.left);
                    //StartCoroutine(nameof(TimeDelay));
                    if (forsound == false)
                    {
                        forsound = true;
                        StartCoroutine(nameof(SoundManager));
                    }
                }
            }

            // Reset swipe positions
            swipePosFirst = Vector2.zero;
            swipePosSecond = Vector2.zero;
        }
    }

    public void SetDirection(Vector3 forSetDirection)
    {
        direction = forSetDirection.normalized;

        transform.rotation = Quaternion.LookRotation(direction);
        // Raycast to find the next wall position
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 100f))
        {
            nextWallPos = hit.point;
        }

        isMoving = false;
    }

    //public IEnumerator TimeDelay()
    //{
    //    yield return new WaitForSeconds(5f);
    //}

    public IEnumerator SoundManager()
    {

        watersound.Play();

        yield return new WaitForSeconds(0.2f); // watersound doesn't work all the time

        forsound = false;
    }

}


