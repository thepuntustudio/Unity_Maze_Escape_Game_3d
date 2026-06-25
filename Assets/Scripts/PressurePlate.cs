// PressurePlate.cs  —  Point 4
// Attach to the floor trigger plate cube.
// Ceiling block GameObjects are assigned as an array in Inspector.

using UnityEngine;
using System.Collections;

public class PressurePlate : MonoBehaviour
{
    public Rigidbody[] ceilingBlocks;
    public float blockResetDelay = 4f;

    private Vector3[] blockStartPositions;
    private bool triggered = false;

    void Start()
    {
        blockStartPositions = new Vector3[ceilingBlocks.Length];
        for (int i = 0; i < ceilingBlocks.Length; i++)
        {
            blockStartPositions[i] = ceilingBlocks[i].transform.position;
            ceilingBlocks[i].isKinematic = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;
        triggered = true;
        HintUI.instance?.ShowHint("The ceiling is falling! Run!", 2f);
        foreach (var rb in ceilingBlocks) rb.isKinematic = false;
        StartCoroutine(ResetBlocks());
    }

    IEnumerator ResetBlocks()
    {
        yield return new WaitForSeconds(blockResetDelay);
        for (int i = 0; i < ceilingBlocks.Length; i++)
        {
            ceilingBlocks[i].isKinematic = true;
            ceilingBlocks[i].linearVelocity = Vector3.zero;
            ceilingBlocks[i].transform.position = blockStartPositions[i];
        }
        yield return new WaitForSeconds(1f);
        triggered = false;   // can trigger again — player must re-time the run
    }
}