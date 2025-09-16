using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public enum InteractionType { Damage, Heal }
    public InteractionType interactionType;
    public float percentageEffect;

    [Header("Medkit Effects")]
    public GameObject pickupEffect; // Particle system prefab
    public AudioClip pickupSound;   // Sound effect

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthSystem health = collision.GetComponent<HealthSystem>();
            if (health != null)
            {
                if (interactionType == InteractionType.Damage)
                {
                    health.TakePercentageDamage(percentageEffect);
                }
                else if (interactionType == InteractionType.Heal)
                {
                    // Only heal if the player is not at full health
                    if (health.CanHeal())
                    {
                        health.HealPercentage(percentageEffect);

                        // Play effects only for healing items
                        PlayPickupEffects();

                        // Disable and hide the object first
                        GetComponent<Collider2D>().enabled = false;
                        GetComponent<SpriteRenderer>().enabled = false;

                        // Destroy after a delay (to allow effects to play)
                        Destroy(gameObject, 1f);
                    }
                }
            }
        }
    }

    void PlayPickupEffects()
    {
        // Play sound if available
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        // Create particle effect if available
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }
    }
}
