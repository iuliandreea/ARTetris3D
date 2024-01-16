using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARCameraMovement : MonoBehaviour
{
    private ARSessionOrigin arSessionOrigin;

    void Start()
    {
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>();

        if (arSessionOrigin == null)
        {
            Debug.LogError("AR Session Origin not found.");
        }
    }

    void Update()
    {
        // Rotate the AR camera based on user input
        float rotationSpeed = 50f;
        float rotationInput = Input.GetAxis("Horizontal");
        arSessionOrigin.transform.Rotate(Vector3.up, rotationInput * rotationSpeed * Time.deltaTime);

        // Move the AR camera forward or backward based on user input
        float moveSpeed = 3f;
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = arSessionOrigin.transform.forward * verticalInput;
        Vector3 moveAmount = moveDirection * moveSpeed * Time.deltaTime;

        arSessionOrigin.transform.Translate(moveAmount);
    }
}