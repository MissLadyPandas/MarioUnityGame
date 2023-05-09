using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Pause pause;

    public int world {get; private set;}
    public int stage {get; private set;}
    public int lives {get; private set;}
    public int coins {get; private set;}
    
    [Header("UI")]
    public TextMeshProUGUI coinCounter;
    public TextMeshProUGUI livesCounter;

    public void UpdateUI()
    {
        if (coinCounter != null && livesCounter != null)
        {
            coinCounter.text = $"Coins: {coins}";
            livesCounter.text = $"Lives: {lives}";
        }
    }

    public void UpdateUIReferences()
{
    coinCounter = GameObject.Find("CoinCounter")?.GetComponent<TextMeshProUGUI>();
    livesCounter = GameObject.Find("LivesCounter")?.GetComponent<TextMeshProUGUI>();
}

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        coins = 0;

        UpdateUI();

        LoadLevel(1,1);
    }

    public void LoadLevel(int world, int stage)
    {
        this.world = world;
        this.stage = stage;
        string sceneName = $"{world}-{stage}";

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            UpdateUIReferences();
            UpdateUI();
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' not found. Make sure it's added to the build settings.");
        }
    }


    public void ResetLevel(float delay)
    {
        Invoke(nameof(ResetLevel), delay);
    }


    public void ResetLevel()
    {
        lives--;

        if(lives > 0) {
            LoadLevel(world, stage);
        } else {
            GameOver();
        }

        UpdateUI();
    }

    private void GameOver()
    {
        NewGame();
    }

    public void AddCoin()
    {
        coins++;

        if (coins == 100)
        {
            AddLife();
            coins = 0;
        }
            UpdateUI();
        
    }


    public void AddLife()
    {
        lives++;
        AudioManager.Instance.PlayOneUp();
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pause != null)
            {
                if (Pause.GameIsPaused)
                {
                    pause.Resume();
                    AudioManager.Instance.PlayBackgroundMusic();
                }
                else
                {
                    pause.PauseGame();
                    AudioManager.Instance.StopBackgroundMusic();
                }
            }
            else
            {
                Debug.LogWarning("Pause object is not assigned in the GameManager!");
            }
        }
    }



}
