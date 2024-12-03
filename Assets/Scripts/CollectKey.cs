using UnityEngine;

public class CollectKey : MonoBehaviour
{
    public bool keyCollected = false; // Boolean to track if collision happened with tile

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if it's a tile
        {
            // Set the bool to true when the tile is collided with
            keyCollected = true;

            // Disable the object
            gameObject.SetActive(false);
        }
    }

}