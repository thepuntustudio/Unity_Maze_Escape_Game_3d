// Attach to the EXIT zone trigger cube at the end of the maze.

using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance?.WinGame();
        }
    }
}