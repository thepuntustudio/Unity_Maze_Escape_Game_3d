// LeverDoor.cs  —  Point 5
// Attach to the DOOR cube.
// Each lever cube has a Lever.cs script that calls NotifyLever() on this script.

using UnityEngine;
using System.Collections;

public class LeverDoor : MonoBehaviour
{
    public float openY = -3f;        // how far down the door slides when open
    public float slideSpeed = 2f;
    public float lever1ResetTime = 5f; // lever 1 resets after this many seconds

    [HideInInspector] public bool lever1On = false;
    [HideInInspector] public bool lever2On = false;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen = false;
    private Coroutine lever1ResetCoroutine;

    void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + Vector3.up * openY;  // slides into floor
    }

    void Update()
    {
        Vector3 target = (lever1On && lever2On) ? openPos : closedPos;
        transform.position = Vector3.MoveTowards(transform.position, target, slideSpeed * Time.deltaTime);

        bool nowOpen = (lever1On && lever2On);
        if (nowOpen && !isOpen) { isOpen = true; HintUI.instance?.ShowHint("Door opened!", 2f); }
        if (!nowOpen && isOpen) { isOpen = false; }
    }

    public void ActivateLever1()
    {
        lever1On = true;
        HintUI.instance?.ShowHint("Lever 1 activated! Hurry — it resets!", 2f);
        if (lever1ResetCoroutine != null) StopCoroutine(lever1ResetCoroutine);
        lever1ResetCoroutine = StartCoroutine(ResetLever1());
    }

    public void ActivateLever2()
    {
        lever2On = !lever2On;   // lever 2 is a simple toggle, stays on
        HintUI.instance?.ShowHint(lever2On ? "Lever 2 activated!" : "Lever 2 off.", 1.5f);
    }

    IEnumerator ResetLever1()
    {
        yield return new WaitForSeconds(lever1ResetTime);
        lever1On = false;
        HintUI.instance?.ShowHint("Lever 1 reset. Try again!", 2f);
    }
}