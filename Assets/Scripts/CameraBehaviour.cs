using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    // Create a reference to the player
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        // Set the camera's position to the player's position and increase the camera's height by 15
        transform.position = new Vector3(player.position.x, player.position.y + 15, player.position.z);
    }
}
