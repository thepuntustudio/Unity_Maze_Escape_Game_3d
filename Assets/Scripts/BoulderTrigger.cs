// BoulderTrigger.cs  —  Point 3
// Attach to the trigger zone at the corridor entrance.
// The boulder Sphere with Rigidbody is assigned in Inspector.

using UnityEngine;
using System.Collections;

public class BoulderTrigger : MonoBehaviour
{
    public Rigidbody boulder;
    public float resetDelay = 5f;

    private Vector3 boulderStart;
    private bool rolling = false;

    void Start()
    {
        boulderStart = boulder.transform.position;
        boulder.isKinematic = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (rolling) return;
        if (!other.CompareTag("Player")) return;
        rolling = true;
        boulder.isKinematic = false;    // boulder begins rolling
        HintUI.instance?.ShowHint("Run! A boulder is coming!", 2f);
        StartCoroutine(ResetBoulder());
    }

    IEnumerator ResetBoulder()
    {
        yield return new WaitForSeconds(resetDelay);
        boulder.isKinematic = true;
        boulder.linearVelocity = Vector3.zero;
        boulder.angularVelocity = Vector3.zero;
        boulder.transform.position = boulderStart;
        yield return new WaitForSeconds(1f);
        rolling = false;
    }
}