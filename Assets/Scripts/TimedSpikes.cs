// TimedSpikes.cs  —  Point 7
// Attach to the PARENT of all spike objects (SpikesGroup).
// Spikes rise and fall on a repeating cycle.

using UnityEngine;

public class TimedSpikes : MonoBehaviour
{
    public float upY = 0.8f;         // how far spikes rise above floor
    public float downY = -0.2f;      // retracted position (slightly below floor)
    public float upDuration = 2f;
    public float downDuration = 2f;
    public float lerpSpeed = 4f;

    private float timer = 0f;
    private bool spikesUp = true;
    private Vector3 targetPos;

    void Start()
    {
        // Start with spikes up so player can see the pattern first
        targetPos = new Vector3(transform.position.x, upY, transform.position.z);
    }

    void Update()
    {
        timer += Time.deltaTime;
        float cycleDuration = spikesUp ? upDuration : downDuration;

        if (timer >= cycleDuration)
        {
            timer = 0f;
            spikesUp = !spikesUp;
            float newY = spikesUp ? upY : downY;
            targetPos = new Vector3(transform.position.x, newY, transform.position.z);
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
    }
}