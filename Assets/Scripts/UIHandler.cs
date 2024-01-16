using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;

    public TextMeshProUGUI pauseText;
    public TextMeshProUGUI scoreText;

    bool isPaused;
    bool drop;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateUI(int score)
    {
        scoreText.text = "Score: " + score.ToString("D5");
    }

    public void Pause()
    {
        isPaused = !isPaused;
        pauseText.text = isPaused ? "Resume" : "Pause";
    }

    public void Drop(bool dropVal)
    {
        drop = dropVal;
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }

    public bool GetDrop()
    {
        return drop;
    }
}
