using UnityEngine;

public class CollectKey : MonoBehaviour
{
    public bool keyCollected = false; // Boolean to track if collision happened with tile

    [SerializeField] private AudioClip keyCollectSound; // Sound to play when key is collected

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if it's a tile
        {
            // Set the bool to true when the tile is collided with
            keyCollected = true;

            // Play the sound
            SFXManager.instance.playSFXClip(keyCollectSound, transform, 1.0f);

            // Disable the object
            gameObject.SetActive(false);
        }
    }

}