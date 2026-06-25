// AxeDamage.cs  —  Point 2
// Attach to the AXE BLADE child cube (not the pivot).
// Kills/respawns player on contact.

using UnityEngine;

public class AxeDamage : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HintUI.instance?.ShowHint("The axe got you! Try again.", 2f);
            GameManager.instance?.RespawnPlayer();
        }
    }
}