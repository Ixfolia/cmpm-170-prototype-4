using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public bool doorUnlocked = false;  // Boolean to track if the door is unlocked
    public GameObject key;             // Reference to the key object
    private bool unlockable = false;   // Boolean to track if the door can be unlocked

    [SerializeField] private AudioClip doorUnlockSound; // Sound to play when door is unlocked
    void Update() {
        // Access the keyCollected status from the key object
        unlockable = key.GetComponent<CollectKey>().keyCollected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && unlockable) // Check if the player triggers and key is collected
        {
            // Unlock the door
            doorUnlocked = true;

            // Play the sound
            SFXManager.instance.playSFXClip(doorUnlockSound, transform, 1.0f);

            // Disable the object
            gameObject.SetActive(false);
        }
    }
}