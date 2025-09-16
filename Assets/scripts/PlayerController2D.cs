using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    [SerializeField] private int maxJumps = 2;

    [Header("Animations")]
    public AnimationClip idleAnimation;
    public AnimationClip moveAnimation;

    [Header("Stealth Settings")]
    public float hideSpotDetectionRadius = 0.5f; // kept for Gizmos
    public bool IsHiding { get; private set; }
    public KeyCode hideKey = KeyCode.LeftShift;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isFacingRight = true;
    private int jumpsRemaining;

    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        jumpsRemaining = maxJumps;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        UpdateAnimations();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0 && !isFacingRight) Flip();
        else if (moveInput < 0 && isFacingRight) Flip();
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpsRemaining--;
        }

        if (rb.velocity.y == 0)
        {
            jumpsRemaining = maxJumps;
        }
    }

    void UpdateAnimations()
    {
        if (Mathf.Abs(rb.velocity.x) > 0.1f) PlayAnimation(moveAnimation);
        else PlayAnimation(idleAnimation);
    }

    void PlayAnimation(AnimationClip clip)
    {
        if (animator != null && clip != null)
            animator.Play(clip.name);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // --- MINIMAL CHANGE: Hiding handled by trigger ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HideSpot"))
        {
            IsHiding = true;
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            // playerCollider.enabled = false; ❌ remove this
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("HideSpot"))
        {
            IsHiding = false;
            spriteRenderer.color = Color.white;
            // playerCollider.enabled = true; ❌ remove this
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hideSpotDetectionRadius);
    }
}
