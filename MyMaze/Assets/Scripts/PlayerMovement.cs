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
            _ = StartCoroutine(nameof(TimeDelay));

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
        yield return new WaitForSeconds(20);
        isMoving=false;
    }
    void Update()
    {
        speedy = rb.velocity.magnitude;


        if (Input.GetMouseButton(0) && speedy < 1)
        {
            swipePosFirst = new Vector2(Input.mousePosition.x, Input.mousePosition.y);


            if (swipePosSecond != Vector2.zero)
            {

                currentSwipe = swipePosFirst - swipePosSecond;
                if (currentSwipe.sqrMagnitude < minSwipeRange)
                {
                    return;
                }

                //Forward and Back
                if (currentSwipe.x > -10f && currentSwipe.x < 10f)
                {

                    if (currentSwipe.y > 4f)
                    {
                        SetDirection(Vector3.forward);
                        if (forsound == false)
                        {
                            forsound = true;
                            _ = StartCoroutine(nameof(SoundManager));
                        }


                    }
                    if (currentSwipe.y < -4f)
                    {
                        SetDirection(Vector3.back);
                        _ = StartCoroutine(nameof(TimeDelay));
                        if (forsound == false)
                        {
                            forsound = true;
                            _ = StartCoroutine(nameof(SoundManager));
                        }
                    }
                }

                // Right and Left
                else if (currentSwipe.y > -10f && currentSwipe.y < 10f)
                {
                    if (currentSwipe.x > 4f)
                    {
                        SetDirection(Vector3.right);
                        _ = StartCoroutine(nameof(TimeDelay));
                        if (forsound == false)
                        {
                            forsound = true;
                            _ = StartCoroutine(nameof(SoundManager));
                        }
                    }
                    if (currentSwipe.x < -4f)
                    {
                        SetDirection(Vector3.left);
                        _ = StartCoroutine(nameof(TimeDelay));
                        if (forsound == false)
                        {
                            forsound = true;
                            _ = StartCoroutine(nameof(SoundManager));
                        }
                    }
                }
            }
            swipePosSecond = swipePosFirst;

        }

        if (Input.GetMouseButtonUp(0))
        {
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

    public IEnumerator TimeDelay()
    {
        yield return new WaitForSeconds(5f);
    }

    public IEnumerator SoundManager()
    {

        watersound.Play();

        yield return new WaitForSeconds(0.2f); // watersound doesn't work all the time

        forsound = false;
    }

}


