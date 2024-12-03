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
    public Transform player;  // Reference to the player
    public GameObject playerObj; // Reference to the player object
    private Vector3 movementDirection;

     public GameObject key;
    public GameObject door;
    public GameObject doorCollider;
    
    // References to other scripts
    private CollectKey collectKeyScript;
    private UnlockDoor unlockDoorScript;
    private OpenDoor openDoorScript;

    private bool keyObj;
    private bool doorObj;
    private bool doorColliderObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentPoint = pointB.transform;

        
        // Initialize references to other scripts
        collectKeyScript = key.GetComponent<CollectKey>();
        unlockDoorScript = doorCollider.GetComponent<UnlockDoor>();
        openDoorScript = door.GetComponent<OpenDoor>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update doorUnlocked status every frame
        doorObj = openDoorScript.opened;
        doorColliderObj = unlockDoorScript.doorUnlocked;
        keyObj = collectKeyScript.keyCollected;

         // Access the hiding bool from PlayerScript
        bool isHiding = playerObj.GetComponent<PlayerMovement>().hiding;

        // Calculate the direction vector towards the current target point
        movementDirection = (currentPoint.position - transform.position).normalized;

        // Set the velocity to move in the direction towards the current target point
        rb.linearVelocity = movementDirection * speed;

        // Rotate the enemy to face the direction of movement
        transform.forward = movementDirection;

        // Check if the enemy is close enough to the current target point to switch targets
        if (Vector3.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            currentPoint = (currentPoint == pointB.transform) ? pointA.transform : pointB.transform;
        }

        // Detect if the player is within the cone
        if (IsPlayerInCone() && !isHiding)
        {
            Debug.Log("Player detected within the cone!");

            // Change states in the other scripts
            collectKeyScript.keyCollected = false;   // Key collected
            unlockDoorScript.doorUnlocked = false;  // Door still locked at this point
            openDoorScript.opened = false;          // Door is closed
            key.SetActive(true);
            doorCollider.SetActive(true);

            // Load a different scene
            SceneManager.LoadScene("MenuScene");
        }
    }

    bool IsPlayerInCone()
    {
        // Calculate direction from the enemy to the player
        Vector3 enemyToPlayer = player.position - transform.position;
        
        // Check if the player is within the detection range
        if (enemyToPlayer.magnitude <= detectionRange)
        {
            // Calculate the angle between the enemy's forward direction and the player
            float angle = Vector3.Angle(movementDirection, enemyToPlayer);  // Use movement direction instead of transform.up
            
            // Check if the angle is within the cone's angle range
            if (angle <= coneAngle / 2f)
            {
                return true;  // Player is within the cone
            }
        }
        
        return false;  // Player is not within the cone
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);

        // Draw the direction the enemy is facing (movement direction)
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, movementDirection * detectionRange);

        // Draw the cone visualization
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);  // Semi-transparent yellow
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -coneAngle / 2f) * movementDirection * detectionRange);  // Left edge
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, coneAngle / 2f) * movementDirection * detectionRange);  // Right edge
    }
}
