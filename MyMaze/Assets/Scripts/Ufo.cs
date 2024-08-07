using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    private SimpleMazeGenerator m_Generator;
    public float rotationSpeed = 90f, upAbove; // Degrees per second
    public GameObject spinner;
    public float moveDuration = 1f; // Duration of UFO movement
    private CountDownTimer timer;
    public ParticleSystem particle;
    private bool leaving;
    void Start()
    {
        m_Generator = FindObjectOfType<SimpleMazeGenerator>();
        m_Generator.UfoMove(this.gameObject);
        // Start a continuous rotation
        StartCoroutine(Spin());
        timer =CountDownTimer.instance;
        StartCoroutine(GoAway());

    }
    IEnumerator GoAway()
    {
        yield return new WaitForSeconds(10);
        leaving = true;
        particle.Stop();
        GetComponent<BoxCollider>().enabled = false;
        MoveUfoToRandomPosition(true);
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
        if (other.TryGetComponent(out PlayerMovement plyr) && !leaving)
        {

            plyr.enabled = false;
            plyr.GetComponent<Rigidbody>().isKinematic = true;
            plyr.transform.parent = transform;
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

        // Move UFO to a random position outside the screen
        MoveUfoToRandomPosition(false);
    }

    private void MoveUfoToRandomPosition(bool empty)
    {

        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float randomX = Random.Range(-screenBounds.x, screenBounds.x);
        float currentY = transform.position.y;
        Vector3 targetPosition = new(randomX, currentY, 0);

        transform.DOMove(targetPosition, moveDuration).SetEase(Ease.Linear);
        if (!empty)
        {
            StartCoroutine(timer.GameOver(2));
        }

    }
}
