using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public Transform[] patrolPoints;

    [Header("Detection")]
    public float detectionRange = 5f;
    public LayerMask obstacleLayer;

    private Transform player;
    private Rigidbody2D rb;
    private int currentPatrolIndex;
    private bool isFacingRight = true;
    private bool isChasing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            isChasing = true;
            ChasePlayer();
        }
        else
        {
            isChasing = false;
            Patrol();
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        // Distance check
        if (Vector2.Distance(transform.position, player.position) > detectionRange)
            return false;

        // Hide spot check - NEW IMPLEMENTATION
        Collider2D hideSpot = Physics2D.OverlapCircle(player.position, 0.5f);
        if (hideSpot != null && hideSpot.CompareTag("HideSpot"))
            return false;

        // Direction check
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        if (Vector2.Dot(directionToPlayer, isFacingRight ? Vector2.right : Vector2.left) < 0)
            return false;

        // Line of sight check
        RaycastHit2D hit = Physics2D.Linecast(
            transform.position,
            player.position,
            obstacleLayer
        );
        return hit.collider == null;
    }

    void ChasePlayer()
    {
        Vector2 moveDirection = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            chaseSpeed * Time.deltaTime
        );

        if (moveDirection.x > 0 && !isFacingRight) Flip();
        else if (moveDirection.x < 0 && isFacingRight) Flip();
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Vector2 moveDirection = (patrolPoints[currentPatrolIndex].position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(
            transform.position,
            patrolPoints[currentPatrolIndex].position,
            patrolSpeed * Time.deltaTime
        );

        if (moveDirection.x > 0 && !isFacingRight) Flip();
        else if (moveDirection.x < 0 && isFacingRight) Flip();

        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isChasing)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }
            rb.velocity = Vector2.zero;
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Visualize hide spot detection range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (player != null)
            Gizmos.DrawWireSphere(player.position, 0.5f);
    }
}