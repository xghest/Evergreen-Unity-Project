using UnityEngine;

public class HideHealth : MonoBehaviour
{
    [Header("References")]
    public GameObject healthUIContainer; // drag your health bar parent here

    void OnEnable()
    {
        if (healthUIContainer) healthUIContainer.SetActive(false);
    }

    void OnDisable()
    {
        if (healthUIContainer) healthUIContainer.SetActive(true);
    }
}
