using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public GameObject gameOverPanel;
    public Button restartButton;

    [Header("Game Over Settings")]
    public float gameOverDistance = 1.5f;
    public float checkRate = 0.2f;

    [Header("Fall Detection Settings")]
    public float fallYThreshold = -10f;

    private Transform player;
    private PlayerController2D playerController;
    private GameObject[] enemies;
    private float nextCheckTime;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // --- Reset everything on fresh load ---
        Time.timeScale = 1f;
        isGameOver = false;

        gameOverPanel.SetActive(false);
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(RestartGame);

        // --- Refresh references ---
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
            playerController = player.GetComponent<PlayerController2D>();

        UpdateEnemyList();
    }

    void Update()
    {
        if (isGameOver || playerController == null) return;

        if (Time.time >= nextCheckTime)
        {
            if (!playerController.IsHiding)
            {
                CheckForGameOver();
            }
            nextCheckTime = Time.time + checkRate;
        }

        if (player != null && player.position.y < fallYThreshold)
        {
            GameOver();
        }
    }

    void CheckForGameOver()
    {
        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;

            float dist = Vector3.Distance(player.position, enemy.transform.position);
            if (dist <= gameOverDistance)
            {
                GameOver();
                return;
            }
        }
    }

    public void UpdateEnemyList()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
