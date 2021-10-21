using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagaer : MonoBehaviour
{
    [Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
    [SerializeField]
    private Vector3 centerOffset = Vector3.zero;

    [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
    [SerializeField]
    private bool followOnStart = false;

    [Tooltip("The Smoothing for the camera to follow the target")]
    [SerializeField]
    //private float smoothSpeed = 0f;

    // cached transform of the target
    public Transform cameraTransform;
    
    // maintain a flag internally to reconnect if target is lost or camera is switched
    public bool isFollowing;

    // Cache for camera offset
    Vector3 cameraOffset = Vector3.zero;


    // horizontal rotation speed
    public float horizontalSpeed = 1f;
    // vertical rotation speed
    public float verticalSpeed = 1f;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    public static CameraManagaer instance;
    void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        if (followOnStart)
        {
            OnStartFollowing();
        }
    }


    void LateUpdate()
    {
        if (cameraTransform == null && isFollowing)
        {
            OnStartFollowing();
        }

        if (isFollowing)
        {
            Follow();
        }
    }


    public void OnStartFollowing()
    {
        cameraTransform = Camera.main.transform;
        isFollowing = true;
        Cut();

    }

    void Follow()
    {
        if (PlayerGUI.instance.playerMouseInput == true)
        {
            float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90, 90);
        }       

        cameraTransform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);

        cameraOffset.x = centerOffset.x;
        cameraOffset.y = centerOffset.y;
        cameraOffset.z = centerOffset.z;

        cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);  
    }

    void Cut()
    {
        /*cameraOffset.z = -distance;
        cameraOffset.y = height;*/

        //cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);

        //cameraTransform.LookAt(this.transform.position + centerOffset);
    }

    public Vector3 GetCameraDirection()
    {
        if (cameraTransform)
        {
            return cameraTransform.eulerAngles;   
        }
        else
        {
            return Vector3.zero;
        }       
    }

    public Vector3 GetCameraFacing()
    {
        Vector3 cameraFacing = Camera.main.transform.forward;
        return cameraFacing;
    }
}
