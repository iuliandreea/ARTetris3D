using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public Camera aRCamera;

    void Start()
    {
        Previewer.instance.ShowPreview();
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
        SceneManager.LoadScene(sceneName);
    }
}