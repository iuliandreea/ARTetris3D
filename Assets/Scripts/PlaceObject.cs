using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class PlaceObject : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private bool isCreated;

    public Camera aRCamera;

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0)
            return;

        if (aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits,
                                     TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;
            GameObject obj = Instantiate(prefab, pose.position, pose.rotation);

            Canvas canvas = obj.transform.Find("Canvas").transform.GetComponent<Canvas>();
            canvas.worldCamera = aRCamera;
            this.enabled = false;

            Transform holder = obj.transform.Find("InputHolder").transform;
            holder.Find("MoveCanvas").GetComponent<Canvas>().worldCamera = aRCamera;
            holder.Find("RotateCanvasX").GetComponent<Canvas>().worldCamera = aRCamera;
            holder.Find("RotateCanvasY").GetComponent<Canvas>().worldCamera = aRCamera;
            holder.Find("RotateCanvasZ").GetComponent<Canvas>().worldCamera = aRCamera;

            if (aRPlaneManager.GetPlane(hits[0].trackableId).alignment == PlaneAlignment.HorizontalUp)
            {
                Vector3 position = obj.transform.position;
                position.y = 0f;
                Vector3 cameraPosition = Camera.main.transform.position;
                cameraPosition.y = 0f;
                Vector3 direction = cameraPosition - position;
                Vector3 targetRotationEuler = Quaternion.LookRotation(direction).eulerAngles;
                Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, obj.transform.up.normalized); // (0, 1, 0)
                Quaternion targetRotation = Quaternion.Euler(scaledEuler);
                obj.transform.rotation = obj.transform.rotation * targetRotation;

            }

            //this.enabled = false;
        }
    }
}
