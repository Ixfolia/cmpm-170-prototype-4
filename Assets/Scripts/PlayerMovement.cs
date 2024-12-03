using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody rb;
    private Vector3 movement;
    public bool hiding = false;

    public GameObject playerModel;
    public GameObject hidingModel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from the horizontal and vertical axes (WASD or arrow keys)
        if (!hiding) {
            movement.x = Input.GetAxis("Horizontal");
            movement.z = Input.GetAxis("Vertical");
        }

        // Check if the player pressed the space bar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hiding = !hiding;
        }

        // Change model based on hiding state
        playerModel.SetActive(!hiding);
        hidingModel.SetActive(hiding);
    }

    void FixedUpdate()
    {
        // Check if any keys are pressed
        if (movement != Vector3.zero && !hiding)
        {
            // Move the player
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime * -1);
        }
        else {
            // Stop the player when no keys are pressed
            rb.linearVelocity = Vector3.zero;
        }
    }
}
