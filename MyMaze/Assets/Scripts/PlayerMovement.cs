using System.Collections;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask layer;
    public float speed = 50f;
    private float speedy;
    private Rigidbody rb;
    public AudioSource watersound;
    public bool forsound, canCrush, isAvatar, isBonus, isDeploy;
    public int minSwipeRange = 500;
    Vector3 direction, nextWallPos;
    private bool targetDecided, isMoving;
    Vector2 swipePosFirst, swipePosSecond, currentSwipe;
    public CinemachineVirtualCamera virtualCam;
    public CinemachineFreeLook cam2;
    public GameObject wall, crushEffect;
    [SerializeField] private List<GameObject> skins;
    [SerializeField] private List<Material> mats;
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private SkinnedMeshRenderer skinnedMesh;
    private int SkinCount, matCount;
    private SimpleMazeGenerator mGenerator;
    private CountDownTimer m_countdownTimer;
    void Start()
    {
        mGenerator = FindObjectOfType<SimpleMazeGenerator>();
        SkinCount = PlayerPrefs.GetInt("Skin");
        skins[SkinCount].SetActive(true);
        matCount = PlayerPrefs.GetInt("Mat");
        m_countdownTimer = FindObjectOfType<CountDownTimer>();
        if (!isAvatar)
        {
            watersound = GetComponent<AudioSource>();
            rb = gameObject.GetComponent<Rigidbody>();
            isMoving = true;
            _ = StartCoroutine(nameof(StartDelay));
            targetDecided = false;
            cam2 = FindAnyObjectByType<CinemachineFreeLook>();
            virtualCam = FindAnyObjectByType<CinemachineVirtualCamera>();
            mesh.material = mats[matCount];
        }
        else
        {
            skinnedMesh.material = mats[matCount];
        }

    }

    private void FixedUpdate()
    {
        if (!isMoving && targetDecided && !isAvatar)
        {
            rb.linearVelocity = speed * direction;
        }
    }
    public void Stars()
    {
        mGenerator.Stars();
    }
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(.9f);
        isMoving = false;
        targetDecided = true;
    }
    void Update()
    {
        if (Time.timeScale != 1 || isAvatar)
            return;
        if (nextWallPos != Vector3.zero)
        {
            if (!canCrush)
            {
                if (Vector3.Distance(transform.position, nextWallPos) < 0.5)
                {

                    //isMoving = false;
                    direction = Vector3.zero;
                    nextWallPos = Vector3.zero;


                }
            }
            else if (speedy >= 1)
            {
                if (Vector3.Distance(transform.position, nextWallPos) < .55)
                {

                    Destroy(wall);
                    nextWallPos = Vector3.zero;
                    canCrush = false;
                    if (crushEffect != null)
                    {
                        Instantiate(crushEffect, wall.transform.position, wall.transform.rotation);
                    }

                }
            }
            if (isBonus)
            {
                m_countdownTimer.AddSecond();
                isBonus = false;
            }
            else if (isDeploy)
            {
                m_countdownTimer.ExtractOneSecond();
                isDeploy = false;
            }

        }
        speedy = rb.linearVelocity.magnitude;
        // Keyboard input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if ((horizontalInput != 0 || verticalInput != 0) && speedy < 1)
        {
            // Check if the movement is horizontal or vertical
            if (horizontalInput != 0)
            {
                SetDirection(new Vector3(horizontalInput, 0f, 0f));
            }
            else if (verticalInput != 0)
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            canCrush = !canCrush;
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
        if (transform.position.y < 5f)
        {
            transform.position = new Vector3(transform.position.x, .95f, transform.position.z);
        }
    }

    public void SetDirection(Vector3 forSetDirection)
    {

        direction = forSetDirection.normalized;

        transform.rotation = Quaternion.LookRotation(direction);
        transform.GetChild(0).DOScale(.35f, .1f);
        transform.GetChild(0).DOScale(.5f, .3f);
        // Raycast to find the next wall position
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 100f, layer))
        {
            nextWallPos = hit.point;
            wall = hit.collider.gameObject;
        }
        isMoving = false;
        MoveCam(direction);

    }
    void MoveCam(Vector3 forSetDirection)
    {

        Vector3 direction = new(forSetDirection.x, 0f, forSetDirection.z);
        Vector3 targetPosition = virtualCam.transform.position + direction;
        targetPosition.x = Mathf.Clamp(targetPosition.x, 10, 12); // clamp x position
        targetPosition.z = Mathf.Clamp(targetPosition.z, 9, 12); // clamp z position
        virtualCam.transform.DOMove(targetPosition, 1f);
    }
    public IEnumerator SoundManager()
    {

        watersound.Play();

        yield return new WaitForSeconds(0.2f); // watersound doesn't work all the time

        forsound = false;

    }

}


