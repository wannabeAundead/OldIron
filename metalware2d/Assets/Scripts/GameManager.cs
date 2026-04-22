using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float levelDuration = 30f;
    public int currentLevel = 1;
    public float levelTimeRemaining;

    public GameState State = GameState.MainMenu;
    public event Action<GameState> OnStateChanged;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        State = GameState.MainMenu;
        Time.timeScale = 0f;
    }

    void Start()
    {
        if (OnStateChanged != null) OnStateChanged(State);
    }

    void Update()
    {
        GameState next = State;
        bool startRequested = false;
        bool advanceRequested = false;

        switch (State)
        {
            case GameState.MainMenu:
                if (Input.GetKeyDown(KeyCode.Space)) { startRequested = true; next = GameState.Playing; }
                break;
            case GameState.Playing:
                levelTimeRemaining -= Time.deltaTime;
                if (levelTimeRemaining <= 0f) { levelTimeRemaining = 0f; next = GameState.LevelUp; }
                if (Input.GetKeyDown(KeyCode.Escape)) next = GameState.Paused;
                break;
            case GameState.Paused:
                if (Input.GetKeyDown(KeyCode.Escape)) next = GameState.Playing;
                break;
            case GameState.LevelUp:
                break;
            case GameState.GameOver:
                if (Input.GetKeyDown(KeyCode.R)) { startRequested = true; next = GameState.Playing; }
                break;
        }

        if (startRequested)
        {
            currentLevel = 1;
            levelTimeRemaining = levelDuration;
            GameHelpers.ClearEnemiesAndBullets();
            GameHelpers.ResetPlayerToOrigin();
        }
        if (advanceRequested)
        {
            currentLevel++;
            levelTimeRemaining = levelDuration;
            GameHelpers.ClearEnemiesAndBullets();
        }
        if (next != State) ApplyState(next);
    }

    public void RequestAdvanceLevel()
    {
        currentLevel++;
        levelTimeRemaining = levelDuration;
        GameHelpers.ClearEnemiesAndBullets();
        ApplyState(GameState.Playing);
    }

    public void RequestGameOver()
    {
        ApplyState(GameState.GameOver);
    }

    public void ApplyState(GameState next)
    {
        State = next;
        Time.timeScale = (next == GameState.Playing) ? 1f : 0f;
        if (OnStateChanged != null) OnStateChanged(next);
    }
}
