// Attach to a Canvas Text. Set the Text component reference in Inspector.
// This is a simple on-screen message system all other scripts will use.

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HintUI : MonoBehaviour
{
    public static HintUI instance;
    public Text hintText;          // Assign in Inspector (legacy UI Text)
    public GameObject panel;       // The background panel, assign in Inspector

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        panel.SetActive(false);
    }

    public void ShowHint(string message, float duration)
    {
        StopAllCoroutines();
        hintText.text = message;
        panel.SetActive(true);
        StartCoroutine(HideAfter(duration));
    }

    IEnumerator HideAfter(float t)
    {
        yield return new WaitForSeconds(t);
        panel.SetActive(false);
    }
}