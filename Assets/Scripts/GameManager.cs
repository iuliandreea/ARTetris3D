using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    int score;
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

    public bool ReadGameOver()
    {
        return gameOver;
    }

    public void setGameOver()
    {
        gameOver = true;
        SceneManager.LoadScene("GameOverScene");
        // UIHandler.instance.SetGameOverWindow();
    }
}