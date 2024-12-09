using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody rb;
    private Transform currentPoint;
    public float speed;
    private float coneAngle = 22f;
    public float detectionRange = 10f;
    public Transform player;
    public GameObject playerObj;

    private bool isHiding;

    public GameObject key;
    public GameObject door;
    public GameObject doorCollider;

    private CollectKey collectKeyScript;
    private UnlockDoor unlockDoorScript;
    private OpenDoor openDoorScript;

    private bool keyObj;
    private bool doorObj;
    private bool doorColliderObj;

    [SerializeField] private AudioClip deathSound;

    private float delayTimer = 0f; // Timer to handle delay
    public float changeDirectionDelay = 1f; // Delay in seconds
    private bool isTurning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentPoint = pointB.transform;

        collectKeyScript = key.GetComponent<CollectKey>();
        unlockDoorScript = doorCollider.GetComponent<UnlockDoor>();
        openDoorScript = door.GetComponent<OpenDoor>();
    }

    void Update()
    {
        doorObj = openDoorScript.opened;
        doorColliderObj = unlockDoorScript.doorUnlocked;
        keyObj = collectKeyScript.keyCollected;

        isHiding = playerObj.GetComponent<PlayerMovement>().hiding;

        // Delay handling
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
            return; // Wait for the delay to finish
        }

        // Rotation handling for smooth turning
        if (isTurning)
        {
            RotateTowardsTarget();
            return;
        }

        // Move enemy if no delay is active
        Vector3 movementDirection = (currentPoint.position - transform.position).normalized;

        rb.linearVelocity = movementDirection * speed;
        transform.forward = movementDirection;

        // Check for arrival at the current point
        if (Vector3.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            // Initiate turn and set delay
            StartTurning();
        }

        // if (IsPlayerInCone() && !isHiding)
        // {
        //     Debug.Log("Player detected within the cone!");

        //     collectKeyScript.keyCollected = false;
        //     unlockDoorScript.doorUnlocked = false;
        //     openDoorScript.opened = false;
        //     key.SetActive(true);
        //     doorCollider.SetActive(true);

        //     SFXManager.instance.playSFXClip(deathSound, transform, 1.0f);

        //     SceneManager.LoadScene("MenuScene");
        // }
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the player entered the cone
        if (other.CompareTag("Player") && !isHiding)
        {
            Debug.Log("Player detected by cone!");

            // Reset key and door states
            collectKeyScript.keyCollected = false;
            unlockDoorScript.doorUnlocked = false;
            openDoorScript.opened = false;

            key.SetActive(true);
            doorCollider.SetActive(true);

            // Play the death sound and reload the scene
            SFXManager.instance.playSFXClip(deathSound, transform, 1.0f);
            SceneManager.LoadScene("MenuScene");
        }
    }

    private void StartTurning()
    {
        isTurning = true;
        rb.linearVelocity = Vector3.zero; // Stop movement
        delayTimer = changeDirectionDelay; // Set delay after turning
    }

    private void RotateTowardsTarget()
    {
        Vector3 targetDirection = (currentPoint == pointB.transform ? pointA.transform.position : pointB.transform.position) - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection.normalized);

        // Smoothly rotate towards the target direction
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 180f * Time.deltaTime);

        // Check if the turn is complete
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            currentPoint = (currentPoint == pointB.transform) ? pointA.transform : pointB.transform; // Switch target point
            isTurning = false;
        }
    }

    // bool IsPlayerInCone()
    // {
    //     Vector3 enemyToPlayer = player.position - transform.position;

    //     if (enemyToPlayer.magnitude <= detectionRange)
    //     {
    //         float angle = Vector3.Angle(transform.forward, enemyToPlayer);
    //         if (angle <= coneAngle / 2f)
    //         {
    //             return true;
    //         }
    //     }

    //     return false;
    // }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * detectionRange);

        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -coneAngle / 2f) * transform.forward * detectionRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, coneAngle / 2f) * transform.forward * detectionRange);
    }
}
