using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Previewer : MonoBehaviour
{
    public static Previewer instance;
    public GameObject[] previewBlocks;
    GameObject currentActive;

    int detectedBlockIndex; 

    private void Awake()
    {
        instance = this;
    }

    public void ShowPreview(int index)
    {
        Destroy(currentActive);
        currentActive = Instantiate(previewBlocks[index], transform.position, Quaternion.identity) as GameObject;
        currentActive.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void ShowPreview()
    {
        GameObject detectedBlock = null; 

        foreach (GameObject block in previewBlocks) 
        {
            if (block.name == PrefabScanner.instance.GetScannedPrefabName())
            {
                detectedBlock = block;
                if (block.name == "T Preview")
                    detectedBlockIndex = 0;
                else if (block.name == "I Preview")
                    detectedBlockIndex = 1;
                else if (block.name == "O Preview")
                    detectedBlockIndex = 3;
                else if (block.name == "S Preview")
                    detectedBlockIndex = 4;
            }
        }

        if (detectedBlock != null) 
        {
            Destroy(currentActive);
            currentActive = Instantiate(detectedBlock, transform.position, Quaternion.identity) as GameObject;
            currentActive.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    public int GetDetectedBlockIndex()
    {
        return detectedBlockIndex;
    }
}
