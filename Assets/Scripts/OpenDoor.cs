using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour
{
    public GameObject door; 
    public GameObject player; 
    public bool opened;
    public GameObject key;
    public GameObject doorCollider;

    // References to other scripts
    private CollectKey collectKeyScript;
    private UnlockDoor unlockDoorScript;

    private bool keyObj;
    private bool doorColliderObj;

    void Start() {
        // Initialize references to other scripts
        collectKeyScript = key.GetComponent<CollectKey>();
        unlockDoorScript = doorCollider.GetComponent<UnlockDoor>();
    }

    void Update() {
        // Access the keyCollected status from the key object
        opened = doorCollider.GetComponent<UnlockDoor>().doorUnlocked;

        // Update doorUnlocked status every frame
        doorColliderObj = unlockDoorScript.doorUnlocked;
        keyObj = collectKeyScript.keyCollected;

        if (opened) {
            // // Check if tilemap and newTile are assigned
            // if (tilemap != null && newTile != null)
            // {
            //     // Change the tile at the specified position to the new tile
            //     tilemap.SetTile(tilePosition, newTile);
            // }
        }
        else {
            // if (tilemap != null && originalTile != null)
            // {
            //     // Change the tile at the specified position to the new tile
            //     tilemap.SetTile(tilePosition, originalTile);
            // }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if it's a tile
        {
            if (!unlockDoorScript.doorUnlocked)
            {
                // Load a different scene if the door is not unlocked
                // SceneManager.LoadScene("prototype1_death"); // Actually, don't do this
            }
            else
            {
                collectKeyScript.keyCollected = false;   // Key collected
                unlockDoorScript.doorUnlocked = false;  // Door still locked at this point
                opened = false;          // Door is closed
                key.SetActive(true);
                doorCollider.SetActive(true);
                SceneManager.LoadScene("MenuScene");
            }
        }
    }
}