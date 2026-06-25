// HiddenDoor.cs  —  Point 6 (always accessible)
// Attach to the wall cube used as the always-open hidden door.
// On Start, collider is disabled so player walks right through.
// Optionally give it a transparent/matching material in the Editor.

using UnityEngine;

public class HiddenDoor : MonoBehaviour
{
    void Start()
    {
        GetComponent<Collider>().enabled = false;
    }
}