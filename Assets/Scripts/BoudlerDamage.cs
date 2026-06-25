// BoulderDamage.cs  —  Point 3
// Attach to the Boulder Sphere itself.

using UnityEngine;

public class BoulderDamage : MonoBehaviour
{
    private Rigidbody rb;

    void Start() => rb = GetComponent<Rigidbody>();

    void OnCollisionEnter(Collision col)
    {
        if (!col.gameObject.CompareTag("Player")) return;
        if (rb.isKinematic) return;   // only hurts when actually rolling
        HintUI.instance?.ShowHint("Crushed! Try timing your run.", 2f);
        GameManager.instance?.RespawnPlayer();
    }
}