using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;
using System.Runtime.CompilerServices;

public class PrefabScanner : MonoBehaviour
{
    public static PrefabScanner instance;

    public ARTrackedImageManager trackedImageManager;

    public string scannedPrefabName;

    private void Awake()
    {
        instance = this;
        /*trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach(GameObject prefab in placeablePrefabs)
        {
            GameObject newPrefab = (Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            spawnedPrefabs.Add(prefab.name, newPrefab);
        }*/
    }

/*    // Start is called before the first frame update
    void Start()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }
*/

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            string prefab = trackedImage.referenceImage.name;
            scannedPrefabName = null;
            if (prefab.Contains("TBlock"))
                scannedPrefabName = "T Preview";
            else if (prefab.Contains("IBlock"))
                scannedPrefabName = "I Preview";
            else if (prefab.Contains("OBlock"))
                scannedPrefabName = "O Preview";
            else if (prefab.Contains("SBlock"))
                scannedPrefabName = "S Preview";

            if (scannedPrefabName != null)
            {
                this.enabled = false;
                string currentSceneName = SceneManager.GetActiveScene().name;
                SceneManager.UnloadSceneAsync(currentSceneName);
                SceneManager.LoadScene("MenuScene");
                break;
            }
        }
    }

    public string GetScannedPrefabName()
    { 
        return scannedPrefabName; 
    }   
}
