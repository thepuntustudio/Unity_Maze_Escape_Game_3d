// SwingAxe.cs  —  Point 2
// Attach to the PIVOT empty GameObject. The axe mesh is a child of the pivot.
// Pivot rotates back and forth on Z axis using sine wave.

using UnityEngine;

public class SwingAxe : MonoBehaviour
{
    public float swingAngle = 60f;   // degrees each direction
    public float swingSpeed = 1.5f;  // full cycles per second

    void Update()
    {
        float angle = Mathf.Sin(Time.time * swingSpeed * Mathf.PI * 2f) * swingAngle;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}