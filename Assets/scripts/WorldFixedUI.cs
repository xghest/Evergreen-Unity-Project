using UnityEngine;

public class WorldFixedUI : MonoBehaviour
{
    private Vector3 fixedPosition;

    void Start()
    {
        // Save the world position the moment this UI is created
        fixedPosition = transform.position;
    }

    void LateUpdate()
    {
        // Force it to stay at that position no matter what
        transform.position = fixedPosition;
    }
}
