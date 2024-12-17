using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOver : MonoBehaviour
{
    // Control UI Gameover anytime Gamge Stage change to GameOver

    void Start()
    {
        StageManager.OnStageChange += OnGameOver;
        this.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        StageManager.OnStageChange -= OnGameOver;
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnGameOver(GameStage stage)
    {
        if ((int)stage != (int)GameStage.GameOver)
        {
            return;
        }
        this.gameObject.SetActive(true);
    }

    public void OnPressRestart()
    {
        var current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
        Time.timeScale = 1;
    }
    public void OnPressMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
