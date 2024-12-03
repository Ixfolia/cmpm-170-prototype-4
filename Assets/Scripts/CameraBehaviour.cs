using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    // create a flag boolean
    public bool flag = false;
    // Create a reference to the player
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        // interpolate the camera's position to follow the player
        if (flag)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, player.position.y + 15, player.position.z), 0.01f);
        }
    }
    
    // After 3 seconds, set the flag to true
    void Start()
    {
        Invoke("SetFlag", 3);
    }

    void SetFlag()
    {
        flag = true;
    }
}
