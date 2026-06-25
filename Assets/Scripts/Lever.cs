// Lever.cs  —  Point 5
// Attach to each Lever cube. Set leverNumber to 1 or 2 in Inspector.
// Assign the LeverDoor reference in Inspector.

using UnityEngine;

public class Lever : MonoBehaviour
{
    public int leverNumber = 1;       // 1 or 2
    public LeverDoor door;
    public float rotateAngle = 45f;   // visual tilt when pulled

    private bool pulled = false;
    private Quaternion defaultRot;
    private Quaternion pulledRot;

    void Start()
    {
        defaultRot = transform.localRotation;
        pulledRot = defaultRot * Quaternion.Euler(rotateAngle, 0f, 0f);
    }

    // Called when player looks at it and presses E (via Raycast in player script)
    void OnInteract()
    {
        if (leverNumber == 1)
        {
            pulled = true;
            transform.localRotation = pulledRot;
            door.ActivateLever1();
        }
        else
        {
            pulled = !pulled;
            transform.localRotation = pulled ? pulledRot : defaultRot;
            door.ActivateLever2();
        }
    }
}