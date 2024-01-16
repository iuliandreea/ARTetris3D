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
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            string prefab = trackedImage.referenceImage.name;
            if (prefab.Contains("TBlock"))
            {
                scannedPrefabName = "T Preview";
                SceneManager.LoadScene("MenuScene");
            }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            string prefab = trackedImage.referenceImage.name;
            if (prefab.Contains("TBlock"))
            {
                scannedPrefabName = "T Preview";
                SceneManager.LoadScene("MenuScene");
            }
            else if (prefab.Contains("IBlock"))
            {
                scannedPrefabName = "I Preview";
                SceneManager.LoadScene("MenuScene");
            }
            else if (prefab.Contains("OBlock"))
            {
                scannedPrefabName = "O Preview";
                SceneManager.LoadScene("MenuScene");
            }
            else if (prefab.Contains("SBlock"))
            {
                scannedPrefabName = "S Preview";
                SceneManager.LoadScene("MenuScene");
            }

        }
    }

    public string GetScannedPrefabName()
    { 
        return scannedPrefabName; 
    }   
}
