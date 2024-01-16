using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameOverHandler : MonoBehaviour
{
    public Camera aRCamera;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        scoreText.text = "Score: " + GameManager.instance.GetScore().ToString("D5");
    }

    private void Update()
    {
        if (aRCamera != null)
        {
            transform.position = aRCamera.transform.position;
            transform.rotation = aRCamera.transform.rotation;
        }
    }

    public void LoadScene(string sceneName)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.UnloadSceneAsync(currentSceneName);
        SceneManager.LoadScene(sceneName);
    }
}
