using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour {

    const float MIN_CLAMP_ANGLE = -15.0f, MAX_CLAMP_ANGLE = 70.0f;
    const float SMOOTH_SPEED_IN = .999f, SMOOTH_SPEED_OUT = 1.02f;

    public Transform lookAtTransform, camTransform;
    public float maxDistance;

    float distance;
    float currentX = 0.0f, currentY = 0.0f;
    float sensitivity; //Get From Settings
    Vector3 offset;



    void Start() {
        camTransform = transform;
        maxDistance = 2.5f;
        distance = maxDistance;
        sensitivity = .5f;
        offset = new Vector3(0, 0, -maxDistance);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        currentX += Input.GetAxis("Mouse X");
        currentY += Input.GetAxis("Mouse Y");
        currentY = Mathf.Clamp(currentY, MIN_CLAMP_ANGLE, MAX_CLAMP_ANGLE);
    }

    void LateUpdate() {
        UpdateCamera();
        CameraCollision();
    }

    /*Updates camera based on character rotation*/
    void UpdateCamera()
    {
        offset = new Vector3(offset.x, offset.y, -distance);
        camTransform.position = lookAtTransform.position + lookAtTransform.rotation * offset;
        camTransform.LookAt(lookAtTransform.position);
 
        Quaternion upAndDown = Quaternion.Euler(-currentY * sensitivity, 0, 0);
        camTransform.localRotation *= upAndDown;
    }


    /*Manual lerping of camera if camera collides with an object*/
    void CameraCollision() {
        RaycastHit hit;
        if (Physics.Linecast(lookAtTransform.position, camTransform.position, out hit))
        {
            distance = Mathf.Clamp(hit.distance * SMOOTH_SPEED_IN, 1, maxDistance);
        }
        else
        {
            float isNextDistanceACollisionDistance = Mathf.Clamp(distance * SMOOTH_SPEED_OUT, 1, maxDistance);
            Vector3 testOffset = new Vector3(offset.x, offset.y, -isNextDistanceACollisionDistance);

            //NOTE - Code is copied from UpdateCamera()
            Transform nextCamTransform = camTransform;
            nextCamTransform.position = lookAtTransform.position + lookAtTransform.rotation * testOffset;

            //Pushes camera back if it wont cause a new collision - caused jittery cam
            if (!Physics.Linecast(lookAtTransform.position, nextCamTransform.position, out hit))
            {
                distance = Mathf.Clamp(distance * SMOOTH_SPEED_OUT, 1, maxDistance);
            }
        }
    }

}
