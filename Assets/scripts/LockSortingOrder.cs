using UnityEngine;

public class LockSortingOrder : MonoBehaviour
{
    public int lockedOrder = 5; // You can change this in Inspector

    void Start()
    {
        // Set the initial sorting order when game starts
        GetComponent<SpriteRenderer>().sortingOrder = lockedOrder;
    }

    void Update()
    {
        // Continuously check and enforce the sorting order
        if (GetComponent<SpriteRenderer>().sortingOrder != lockedOrder)
        {
            GetComponent<SpriteRenderer>().sortingOrder = lockedOrder;
        }
    }
}