using UnityEngine;
using System.Collections;

public class UIPopupTrigger : MonoBehaviour
{
    public GameObject popupUI;       // Assign your existing UI object
    public float displayTime = 3f;   // How long it stays fully visible
    public float fadeDuration = 0.5f; // Time it takes to fade in/out

    private bool hasTriggered = false;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (popupUI != null)
        {
            canvasGroup = popupUI.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = popupUI.AddComponent<CanvasGroup>();
            }
            popupUI.SetActive(false); // start hidden
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(ShowPopup());
        }
    }

    private IEnumerator ShowPopup()
    {
        if (popupUI == null || canvasGroup == null) yield break;

        popupUI.SetActive(true);

        // Fade in
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;

        // Wait while visible
        yield return new WaitForSeconds(displayTime);

        // Fade out
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;

        popupUI.SetActive(false);
    }
}
