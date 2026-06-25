// Gate9.cs  —  Point 9 hidden door
// Attach to the hidden door cube that blocks the path to the EXIT.
// Starts locked (collider on). Unlocks when GameManager.UnlockGate9() is called.

using UnityEngine;
using System.Collections;

public class Gate9 : MonoBehaviour
{
    public static Gate9 instance;

    public float slideDistance = 4f;   // how far the door slides into the wall
    public float slideSpeed = 2f;

    private Collider col;
    private bool unlocked = false;
    private Vector3 closedPos;

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }

    void Start()
    {
        col = GetComponent<Collider>();
        closedPos = transform.position;
        col.enabled = true;   // starts locked/solid
    }

    public void Unlock()
    {
        if (unlocked) return;
        unlocked = true;
        col.enabled = false;  // player can now pass through
        StartCoroutine(SlideOpen());
    }

    IEnumerator SlideOpen()
    {
        Vector3 openPos = closedPos + Vector3.up * slideDistance;
        while (Vector3.Distance(transform.position, openPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPos, slideSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = openPos;
    }
}