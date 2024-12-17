using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // Game Stage Manager for controlling game Stage
    public static StageManager Instance { get; private set; }
    public GameStage Stage;
    public static event Action<GameStage> OnStageChange;
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void UpdateGameStage(GameStage gameStage)
    {
        this.Stage = gameStage;
        
        switch (gameStage)
        {
            case GameStage.GameStart:
                break;
            case GameStage.Prepare:
                break;
            case GameStage.GameOver:
                Time.timeScale = 0;
                break;
            case GameStage.GenerateNextMap:
                break;
            default:
                break;
        }
        if (OnStageChange != null)
        {
            OnStageChange.Invoke(gameStage);
        }
        else
        {
            Debug.LogWarning("OnStageChange event has no subscribers.");
        }
    }
}
public enum GameStage
{
    Prepare,
    GameStart,
    GameOver,
    GenerateNextMap
}