// Checkpoint.cs
// Attach to any invisible trigger cube placed in the maze corridors.
// When player walks through, their respawn position is updated.

using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool reached = false;

    void OnTriggerEnter(Collider other)
    {
        if (reached) return;
        if (!other.CompareTag("Player")) return;
        reached = true;
        GameManager.instance.lastCheckpointPos = transform.position;
        GameManager.instance.hasCheckpoint = true;
        HintUI.instance?.ShowHint("Checkpoint!", 1.5f);
        // Optional: change material to show it's been activated
    }
}