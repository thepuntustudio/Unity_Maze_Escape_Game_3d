// SpikeDamage.cs  —  Point 7
// Attach to any individual spike collider (or the group if using one collider).

using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    private TimedSpikes spikesController;

    void Start()
    {
        spikesController = GetComponentInParent<TimedSpikes>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        // Only hurt when spikes are in "up" state (not retracting)
        HintUI.instance?.ShowHint("The spikes got you! Watch the timing.", 2f);
        GameManager.instance?.RespawnPlayer();
    }
}