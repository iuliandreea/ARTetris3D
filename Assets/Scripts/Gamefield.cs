using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamefield : MonoBehaviour
{
    public Camera aRCamera;

    private void Update()
    {
        if (aRCamera != null)
        {
            transform.position = aRCamera.transform.position;
            transform.rotation = aRCamera.transform.rotation;
        }
    }
}
