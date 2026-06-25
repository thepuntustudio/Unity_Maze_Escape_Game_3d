// Attach to each tile cube. Tile falls when player steps on it, resets after delay.

using UnityEngine;
using System.Collections;

public class FallingTile : MonoBehaviour
{
    public float fallDelay = 0.8f;
    public float resetDelay = 3f;
    public float fallDistance = 10f;

    private Vector3 startPos;
    private Rigidbody rb;
    private bool triggered = false;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;
        triggered = true;
        StartCoroutine(FallAndReset());
    }

    IEnumerator FallAndReset()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.isKinematic = false;         // tile drops under gravity

        yield return new WaitForSeconds(resetDelay);
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        transform.position = startPos;  // snap back to original position
        triggered = false;
    }
}