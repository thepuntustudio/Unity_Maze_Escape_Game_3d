// Attach to an empty GameObject named "GameManager" in the scene.

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool gate9Unlocked = false;

    // Checkpoint position — updated by Checkpoint.cs when player walks through
    [HideInInspector] public Vector3 lastCheckpointPos;
    [HideInInspector] public bool hasCheckpoint = false;

    private Transform playerTransform;

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        lastCheckpointPos = playerTransform.position;
    }

    public void RespawnPlayer()
    {
        if (playerTransform == null) return;
        // Disable rigidbody velocity before teleporting
        Rigidbody rb = playerTransform.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = Vector3.zero;
        playerTransform.position = hasCheckpoint ? lastCheckpointPos : lastCheckpointPos;
    }

    public void UnlockGate9()
    {
        gate9Unlocked = true;
        Gate9.instance?.Unlock();
        HintUI.instance?.ShowHint("A hidden gate has opened somewhere nearby...", 4f);
    }

    public void WinGame()
    {
        Debug.Log("PLAYER ESCAPED THE MAZE!");
        // Replace with SceneManager.LoadScene("WinScene") once you have one
        HintUI.instance?.ShowHint("You escaped the maze! Congratulations!", 99f);
    }
}