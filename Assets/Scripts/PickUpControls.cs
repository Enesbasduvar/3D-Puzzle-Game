using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;

public class PickUpControls : MonoBehaviour
{
    [Header("Pick Up Settings")]
    [SerializeField] public Transform holdArea;
    
    public GameObject heldObj;
    private Rigidbody heldObjRB;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 5.0f;
    private bool isRotating = false;

    [Header("Moving Closer or Farther")]
    //[SerializeField] private float moveSmoothness = 5.0f;
    [SerializeField] private float moveSpeed = 50.0f; // Adjust the move speed as needed
    //[SerializeField] private float minDistance = 1.0f; // Minimum distance between the player and the held object
    //[SerializeField] private float maxDistance = 10.0f; // Maximum distance between the player and the held object
    Renderer rend = null;
    Material originalMaterial, tempMaterial;
    private Color highlightColor;
    Vector3 holdAreaPos = new Vector3(0,0,3);
    void Start()
    {
        highlightColor = Color.green;
    }

    private void FixedUpdate(){
        if(heldObj == null)
        {
            highlightObject();
        }
        else
        {
            returnObject();
        }

    }

    private void Update()
    {
        MoveObjectCloserOrFarther();
        //debugging();
        if(Input.GetMouseButtonDown(0))
        {
            
            returnObject();
            if(heldObj == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    PickupObject(hit.transform.gameObject);
                }
            }
            else
            {
                DropObject();
            }
        }
        
        else if (Input.GetMouseButtonDown(1) && heldObj != null)
        {
            StartRotating();
        }
        
        else if (Input.GetMouseButtonUp(1) && heldObj != null)
        {
            StopRotating();
        }

        if (heldObj != null && isRotating)
        {
            RotateHeldObject();
        }
        
        if(heldObj != null)
        {
            MoveObject(); // Move the held object based on the previous logic            
            //MoveObjectCloserOrFarther(); // New function: Move the picked object closer or farther using the mouse wheel
        }
        
    }
    void MoveObject()
    {
        if(Vector3.Distance(heldObj.transform.position, holdArea.position)> 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }
    void RotateHeldObject()
    {
        if (heldObj != null && holdArea != null)
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

            // Calculate the local Y-axis of the HoldArea object
            Vector3 localYAxis = holdArea.up;

            // Rotate the held object around the local Y-axis of the HoldArea object
            heldObj.transform.RotateAround(holdArea.position, localYAxis, -rotationX);
            heldObj.transform.RotateAround(holdArea.position, holdArea.right, rotationY);
        }
    }
    void StartRotating()
    {
        isRotating = true;
        // Disable the script controlling camera rotation (replace "YourCameraRotationScript" with the actual script name)
        PlayerCam cameraRotationScript = GetComponent<PlayerCam>();
        if (cameraRotationScript != null)
        {
            cameraRotationScript.enabled = false;
        }
    }
    void StopRotating()
    {
        isRotating = false;
        // Enable the script controlling camera rotation (replace "YourCameraRotationScript" with the actual script name)
        PlayerCam cameraRotationScript = GetComponent<PlayerCam>();
        if (cameraRotationScript != null)
        {
            cameraRotationScript.enabled = true;
        }
    }
    void PickupObject(GameObject pickObj)
    {
        if(pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
        }
    }
    void DropObject()
    {
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;
        heldObjRB.transform.parent = null;
        heldObj = null;
        Vector3 offset = transform.forward * 3.0f;
        holdArea.position = transform.position + offset;
        
    }
    void debugging()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        Debug.Log(scrollWheel);

        if(heldObj != null)
        {
            float distance = Vector3.Distance(heldObj.transform.position, gameObject.transform.position);
            Debug.Log("Distance: " + distance.ToString("F2"));
        }
        
    }
    void MoveObjectCloserOrFarther()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        
        if (Mathf.Abs(scrollWheel) > 0 && heldObj != null)
        {
            float distance = Vector3.Distance(heldObj.transform.position, gameObject.transform.position);
            float newDistance = Vector3.Distance(heldObj.transform.position, holdArea.position) * scrollWheel * moveSpeed;
            //Debug.Log("New Distance: " + newDistance.ToString("F2"));
            //newDistance = Mathf.Clamp(newDistance, minDistance, maxDistance);
            //Debug.Log("scrollWheel: " + scrollWheel.ToString("F2"));
            //Debug.Log("mult: " + scrollWheel*moveSpeed);

            Vector3 targetPosition;
            targetPosition = holdArea.position;
            RaycastHit hit;
            if (scrollWheel > 0 && distance < 5.0f)
            {   // Scrolling up, move the object farther
                targetPosition = holdArea.position + holdArea.forward * newDistance;
            }
            if (scrollWheel < 0 && distance > 1.5f)
            {   // Scrolling down, move the object closer
                targetPosition = holdArea.position + holdArea.forward * newDistance;
            }
            if (Physics.Raycast(holdArea.position, targetPosition - holdArea.position, out hit, newDistance))
            {   // Adjust the new position to avoid collisions
                targetPosition = hit.point;
            }
            holdArea.position = targetPosition;
        }
        
        
    }

    private void highlightObject()
    {
        //enable emission on the material of the object that the player is looking at
        RaycastHit hitObj;
        Renderer currRend;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitObj, pickupRange) && hitObj.collider.gameObject.GetComponent<Rigidbody>() != null)
        {
            currRend = hitObj.collider.gameObject.GetComponent<Renderer>();
            if (currRend == rend)
                return;
            if (currRend && currRend != rend)
            {
                if (rend)
                {
                    rend.sharedMaterial = originalMaterial;
                }

            }
            if (currRend)
                rend = currRend;
            else
                return;
            originalMaterial = rend.sharedMaterial;
            originalMaterial.EnableKeyword("_EMISSION");
            tempMaterial = new Material(originalMaterial);
            rend.material = tempMaterial;
            //rend.material.color = highlightColor;
        }
        else
        {
            if (rend)
            {
                
                rend.sharedMaterial = originalMaterial;
                rend = null;
                originalMaterial.DisableKeyword("_EMISSION");
                
            }
        }
    }

    private void returnObject()
    {
        //enable emission on the material of the object that the player is looking at
        RaycastHit hitObj;
        Renderer currRend;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitObj, pickupRange) && hitObj.collider.gameObject.GetComponent<Rigidbody>() != null)
        {
            originalMaterial.DisableKeyword("_EMISSION");
            currRend = hitObj.collider.gameObject.GetComponent<Renderer>();
            if (currRend == rend)
                return;
            if (currRend && currRend != rend)
            {
                if (rend)
                {
                    rend.sharedMaterial = originalMaterial;
                }

            }
            if (currRend)
                rend = currRend;
            else
                return;
            originalMaterial = rend.sharedMaterial;
            
            tempMaterial = new Material(originalMaterial);
            rend.material = tempMaterial;
            //rend.material.color = highlightColor;
        }
        else
        {
            if (rend)
            {
                
                rend.sharedMaterial = originalMaterial;
                rend = null;
                originalMaterial.DisableKeyword("_EMISSION");
                
            }
        }
    }
    
    
}
