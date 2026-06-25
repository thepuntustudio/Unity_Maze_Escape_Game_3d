// // InteractPrompt.cs
// // Attach to any lever, button, or interactable object.
// // Uses your existing PlayerInputActions + "Interact" action.

// using UnityEngine;

// public class InteractPrompt : MonoBehaviour
// {
//     [Tooltip("Max distance for player to interact")]
//     public float interactRange = 2.5f;

//     [Tooltip("What to show the player when in range")]
//     public string promptMessage = "Press E to interact";

//     private Transform playerCam;
//     private bool playerInRange = false;

//     // Implement this in each interactable child class
//     public virtual void OnInteract() { }

//     void Start()
//     {
//         // Find the camera (your existing FPS setup)
//         playerCam = Camera.main?.transform;
//     }

//     void Update()
//     {
//         if (playerCam == null) return;

//         float dist = Vector3.Distance(transform.position, playerCam.position);
//         bool inRange = dist <= interactRange;

//         if (inRange && !playerInRange)
//         {
//             playerInRange = true;
//             HintUI.instance?.ShowHint(promptMessage, 99f); // stays until out of range
//         }
//         else if (!inRange && playerInRange)
//         {
//             playerInRange = false;
//             HintUI.instance?.ShowHint("", 0f);
//         }

//         // Read interact input via your existing New Input System action
//         if (playerInRange)
//         {
//             // Reads the "Interact" action you already have in PlayerInputActions
//             // Assumes you have OnInteract() sent via SendMessage mode
//         }
//     }

//     // Called by your PlayerInput component via SendMessage
//     void OnInteract()
//     {
//         if (playerInRange) OnInteract();
//     }
// }