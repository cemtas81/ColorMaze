using System.Collections;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
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
    private bool targetDecided;
    Vector2 swipePosFirst;
    Vector2 swipePosSecond;
    Vector2 currentSwipe;
    public CinemachineVirtualCamera virtualCam;
    public CinemachineFreeLook cam2;
    void Start()
    {
        watersound = GetComponent<AudioSource>();
        rb = gameObject.GetComponent<Rigidbody>();
        isMoving = true;
        _ = StartCoroutine(nameof(StartDelay));
        targetDecided = false;
        cam2=FindAnyObjectByType<CinemachineFreeLook>();
        virtualCam=FindAnyObjectByType<CinemachineVirtualCamera>();
    
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
        yield return new WaitForSeconds(.3f);
        isMoving=false;
    }
    void Update()
    {
        speedy = rb.velocity.magnitude;
        // Keyboard input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if ((horizontalInput != 0 || verticalInput != 0) &&  speedy < 1 &&!isMoving)
        {
            // Check if the movement is horizontal or vertical
            if (horizontalInput != 0 )
            {
                SetDirection(new Vector3(horizontalInput, 0f, 0f));
            }
            else if (verticalInput != 0 )
            {
                SetDirection(new Vector3(0f, 0f, verticalInput));
            }

            if (!forsound)
            {
                forsound = true;
                StartCoroutine(nameof(SoundManager));
            }
        }
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
        if (!targetDecided)
        {
            virtualCam.Follow = this.transform;
            //cam2.Follow = this.transform;   
            targetDecided = true;

        }
        isMoving = false;
    }

    public IEnumerator SoundManager()
    {

        watersound.Play();

        yield return new WaitForSeconds(0.2f); // watersound doesn't work all the time

        forsound = false;
  
    }

}


