// BigButton.cs  —  Point 8
// Attach to the large trigger cube/sphere at the dead-end corridor.

using UnityEngine;

public class BigButton : MonoBehaviour
{
    private bool pressed = false;

    void OnTriggerEnter(Collider other)
    {
        if (pressed) return;
        if (!other.CompareTag("Player")) return;
        pressed = true;

        // Visual feedback — squish the button down slightly
        transform.localScale = new Vector3(
            transform.localScale.x,
            transform.localScale.y * 0.3f,
            transform.localScale.z
        );

        GameManager.instance?.UnlockGate9();
        HintUI.instance?.ShowHint("*CLUNK* Something opened in the distance...\nThere is no way forward here. Turn back.", 5f);
    }
}