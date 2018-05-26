/*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {


    public float cameraMoveSpeed = 120.0f;
    public GameObject CameraFollowObject;
    Vector3 followPosition;
    public float clampAngle = 80.0f;
    public float inputSensitivity = 150.0f;
    public GameObject CameraObject;
    public GameObject PlayerObject;
    public float camDistanceToPlayerX, camDistanceToPlayerY, camDistanceToPlayerZ;
    public float mouseX, mouseY;
    public float smoothX, smoothY;
    private float rotX = 0.0f, rotY = 0.0f;

    // Use this for initialization
    void Start () {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotX = rot.x;
        rotY = rot.y;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMouseInput();
    }
    
    void UpdateMouseInput() {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        rotX = mouseX * inputSensitivity * Time.deltaTime;
        rotY = mouseY * inputSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

        //MyLog.log("Here");
        //MyLog.printToScreen();
    }

    void LateUpdate() {
        CameraUpdater();
    }

    void CameraUpdater() {
        //Set target object to follow
        Transform target = CameraFollowObject.transform;

        //Move towards game object that is the taret
        float step = cameraMoveSpeed * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}



























using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private const float CLAMP_ANGLE = 70.0f;
    private const float SMOOTH_SPEED_IN = .999f, SMOOTH_SPEED_OUT = 1.02f;

    public Transform lookAtTransform, camTransform;
    private Vector3 offsetFromLookAt = new Vector3(0, 2, -10);
    public float xTilt = 10.0f;


    Vector3 destination = Vector3.zero;
    public CharacterMovementController charController;
    float rotVel = 0.0f;

    public float maxDistance = 10.0f;
    private float distance;
    private float currentX = 0.0f, currentY = 0.0f;
    private float lookAtCurrentX = 0.0f, lookAtCurrentY = 0.0f;
    private float sensitivity = 1.0f;
    

    void Start() {
        camTransform = transform;
        distance = maxDistance;
    }

    public void setCameraTarget(Transform t)
    {
        if (t == null)
        {
            print("CamFollow - CameraNeedsATarget");
        } else {

            lookAtTransform = t;
            if (t.GetComponent<CharacterController>()) {
                charController = lookAtTransform.GetComponent<CharacterMovementController>();
            } else {
                print("CamFollow - Warning - No character controller on newly set camera target");
            }

        }

    }

    void Update() {
        currentX += Input.GetAxis("Mouse X");
        currentY += Input.GetAxis("Mouse Y");
        currentY = Mathf.Clamp(currentY, -CLAMP_ANGLE, CLAMP_ANGLE);
    }

    void LateUpdate() {
        UpdateCamera();
    }
    
void UpdateCamera()
{
    Vector3 offset = lookAtTransform.parent.rotation * new Vector3(0, 0, -distance);
    //Quaternion rotation = Quaternion.Euler(currentY * sensitivity, currentX  * sensitivity, 0.0f);
    //Quaternion rotation = Quaternion.Euler(lookAtCurrentY * 10 * sensitivity, lookAtCurrentX * 10 * sensitivity, 0.0f);

    // camTransform.position = lookAtTransform.position + rotation * offset;

    destination = charController.GetTargetRotation() * offsetFromLookAt;
    destination += lookAtTransform.position;
    transform.position = destination;


    //camTransform.position += lookAtTransform.position;
    print("X: " + lookAtTransform.rotation.x + "Z: " + lookAtTransform.rotation.z);
    //camTransform.LookAt(lookAtTransform.position);
    //CameraCollision();
}

void CameraCollision()
{
    RaycastHit hit;
    if (Physics.Linecast(lookAtTransform.position, camTransform.position, out hit))
    {
        distance = Mathf.Clamp(hit.distance * SMOOTH_SPEED_IN, 1, maxDistance);
    }
    else
    {
        float isNextDistanceACollisionDistance = Mathf.Clamp(distance * SMOOTH_SPEED_OUT, 1, maxDistance);
        Vector3 offset = new Vector3(0, 0, -isNextDistanceACollisionDistance);

        //NOTE - Code is copied from UpdateCamera()
        Transform nextCamTransform = camTransform;
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0.0f);
        nextCamTransform.position = lookAtTransform.position + rotation * offset;


        //Pushes camera back if it wont cause a new collision - caused jittery cam
        if (!Physics.Linecast(lookAtTransform.position, nextCamTransform.position, out hit))
        {
            distance = Mathf.Clamp(distance * SMOOTH_SPEED_OUT, 1, maxDistance);
        }

    }
}

}




*/