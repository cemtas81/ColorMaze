using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    private SimpleMazeGenerator m_Generator;
    public float rotationSpeed = 90f,upAbove; // Degrees per second
    public GameObject spinner;
    public float moveDuration = 1f; // Duration of UFO movement
    private CountDownTimer timer;
    public ParticleSystem particle;
  
    void Start()
    {
        m_Generator = FindObjectOfType<SimpleMazeGenerator>();
        m_Generator.UfoMove(this.gameObject);
        // Start a continuous rotation
        StartCoroutine(Spin());
        timer=FindObjectOfType<CountDownTimer>(); 
     
    }

    private IEnumerator Spin()
    {
        while (true)
        {
            spinner.transform.Rotate(rotationSpeed * Time.deltaTime * Vector3.up);
            yield return null;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerMovement plyr))
        {
            plyr.enabled = false;
            plyr.GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(MovePlayerUpAndMoveUfo(plyr));
        }
    }

    private IEnumerator MovePlayerUpAndMoveUfo(PlayerMovement player)
    {
        float elapsedTime = 0f;
        Vector3 startPos = player.transform.position;
        Vector3 targetPos = startPos + Vector3.up * upAbove; // Move player 2 units up

        // Move the player up
        while (elapsedTime < moveDuration)
        {
            player.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = targetPos; // Ensure final position accuracy
        player.transform.parent=transform;
        // Move UFO to a random position outside the screen
        MoveUfoToRandomPosition();
    }

    private void MoveUfoToRandomPosition()
    {
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float randomX = Random.Range(-screenBounds.x, screenBounds.x);
        float currentY = transform.position.y;
        Vector3 targetPosition = new Vector3(randomX, currentY, 0);

        transform.DOMove(targetPosition, moveDuration).SetEase(Ease.Linear);
        StartCoroutine(timer.GameOver(2));
    }
}
