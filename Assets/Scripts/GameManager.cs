using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    var go = new GameObject("===GameManager===");
                    instance = go.AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }

    public int score = 0;
    public int killAmount = 0;
    public int winScore = 100;
    public int winKillAmount = 30;
    public int remainTotalTime = 300;
    public float remainTime;

    public GameState gameState;

    public Entity Player;

    public delegate void GameStateChanged(GameState state);
    public event GameStateChanged gameStateChanged;

    private void Start()
    {
        StartNewGame();
    }

    public void StartNewGame()
    {
        score = 0;
        killAmount = 0;
        remainTime = remainTotalTime;
        gameState = GameState.Playing;
    }

    public void AddScore(int score)
    {
        this.score += score;
        CheckWinCondition();
    }

    public void AddKillAmount(int amount)
    {
        killAmount += amount;
        CheckWinCondition();
    }

    public void AnyEntityDeath(Entity entity)
    {
        EntityType type = entity.entityType;
        switch (type)
        {
            case EntityType.Player:
                DoGameFail();
                break;
            case EntityType.Monster:
                AddScore(entity.dropCoin);
                AddKillAmount(1);
                break;
            case EntityType.Other:
                AddScore(entity.dropCoin);
                break;
            default:
                break;
        }
    }

    public void CheckWinCondition()
    {
        if (killAmount >= winKillAmount && score >= winScore)
        {
            DoGameWin();
        }
    }

    public void DoGameFail()
    {
        if (gameState != GameState.Playing)
            return;

        ChangeGameState(GameState.Fail);
        InternalGameFail();
    }

    private void InternalGameFail()
    {
        Debug.Log("你输了");
    }

    public void DoGameWin()
    {
        if (gameState != GameState.Playing)
            return;

        ChangeGameState(GameState.Win);
        InternalGameWin();
    }

    private void InternalGameWin()
    {
        Debug.Log("你赢了");
        var array = FindObjectsOfType<Entity>();
        for (int i = 0; i < array.Length; i++)
        {
            var obj = array[i];
            Destroy(obj.gameObject);
        }
    }

    public void ChangeGameState(GameState state)
    {
        gameState = state;
        gameStateChanged?.Invoke(state);
    }

    private void Update()
    {
        if (gameState == GameState.Playing)
        {
            remainTime -= Time.deltaTime;
            if (remainTime <= 0)
            {
                remainTime = 0;
                DoGameWin();
            }
        }
    }
}

public enum EntityType
{
    Player,
    Monster,
    Other
}

public enum GameState
{
    Playing,
    Win,
    Fail
}