using UnityEngine;

public class EndGame : MonoBehaviour
{
    public GameObject popupUI;   // Assign your existing UI object
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            if (popupUI != null)
            {
                popupUI.SetActive(true); // just turn it on
            }
        }
    }
}
