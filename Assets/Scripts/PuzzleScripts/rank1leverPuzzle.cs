using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rank1leverPuzzle : MonoBehaviour
{
    AudioManager audioManager;
    private float reachRange = 5.0f;
    public Camera mainCamera;
    Transform pivot;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Update()
    {
        Transform playerPrefab = GameObject.Find("player").transform;
        Transform cameraHolder = playerPrefab.Find("CameraHolder");
        Camera mainCamera = cameraHolder.GetComponentInChildren<Camera>();
        // Cast a ray from the mouse position into the scene
        
        
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, reachRange) && (hit.collider.CompareTag("LeverC") || hit.collider.CompareTag("Lever")))
            {
                // get the pivot object which is the child of the LeverC object
                if(hit.collider.CompareTag("LeverC"))
                {
                    pivot = hit.collider.transform.GetChild(0);
                }
                else if(hit.collider.CompareTag("Lever"))
                {
                    pivot = hit.collider.transform.parent;
                }
                
                if (pivot != null)
                {
                    // rotate the pivot object around the z-axis by -120 degrees
                    if (pivot.rotation.eulerAngles.z == 330)
                    {
                        Debug.Log("lever down");
                        pivot.Rotate(0, 0, -120);
                        audioManager.PlaySFX(audioManager.LeverOn);
                    }
                    else if(pivot.rotation.eulerAngles.z == 210)
                    {
                        Debug.Log("lever up");
                        pivot.Rotate(0, 0, 120);
                        audioManager.PlaySFX(audioManager.LeverOff);
                    }
                }
            }
        }
    }
}
