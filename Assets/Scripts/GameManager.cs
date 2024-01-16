
using UnityEngine;using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int score;
    bool gameOver;


    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetScore(score);
    }

    public void SetScore(int amount)
    {
        score += (100 * amount);
        UIHandler.instance.UpdateUI(score);
    }

    public int GetScore()
    {
        return score;
    }

    public bool ReadGameOver()
    {
        return gameOver;
    }

    public void setGameOver()
    {
        gameOver = true;
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.UnloadSceneAsync(currentSceneName);
        SceneManager.LoadScene("GameOverScene");
        // UIHandler.instance.SetGameOverWindow();
    }
}