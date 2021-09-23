using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagaer : MonoBehaviour
{
    [Tooltip("The distance in the local x-z plane to the target")]
    [SerializeField]
    private float distance = 7.0f;

    [Tooltip("The height we want the camera to be above the target")]
    [SerializeField]
    private float height = 3.0f;

    [Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
    [SerializeField]
    private Vector3 centerOffset = Vector3.zero;

    [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
    [SerializeField]
    private bool followOnStart = false;

    [Tooltip("The Smoothing for the camera to follow the target")]
    [SerializeField]
    private float smoothSpeed = 0.125f;

    // cached transform of the target
    Transform cameraTransform;

    // maintain a flag internally to reconnect if target is lost or camera is switched
    bool isFollowing;

    // Cache for camera offset
    Vector3 cameraOffset = Vector3.zero;


    // horizontal rotation speed
    public float horizontalSpeed = 1f;
    // vertical rotation speed
    public float verticalSpeed = 1f;
    private float xRotation = 0.0f;
    private float yRotation = 0.0f;





    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        // Start following the target if wanted.
        if (followOnStart)
        {
            OnStartFollowing();
        }
    }


    void LateUpdate()
    {
        // The transform target may not destroy on level load, 
        // so we need to cover corner cases where the Main Camera is different everytime we load a new scene, and reconnect when that happens
        if (cameraTransform == null && isFollowing)
        {
            OnStartFollowing();
        }

        // only follow is explicitly declared
        if (isFollowing)
        {
            Follow();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isFollowing = false;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            PhotonManager.instance.LeaveRoom();           
        }
    }


    public void OnStartFollowing()
    {
        cameraTransform = Camera.main.transform;
        isFollowing = true;
        // we don't smooth anything, we go straight to the right camera shot
        Cut();

    }

    void Follow()
    {
        float mouseX = Input.GetAxis("Mouse X") * horizontalSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * verticalSpeed;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        cameraTransform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);

        cameraOffset.z = -distance;
        cameraOffset.y = height;

        cameraTransform.position = Vector3.Lerp(cameraTransform.position, this.transform.position + this.transform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);

        //cameraTransform.LookAt(this.transform.position + centerOffset);

    }

    void Cut()
    {
        cameraOffset.z = -distance;
        cameraOffset.y = height;

        cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);

        cameraTransform.LookAt(this.transform.position + centerOffset);
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
}
