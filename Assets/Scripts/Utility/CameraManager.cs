using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    Camera playerCam;

    [SerializeField] Camera sceneCam;
    [SerializeField] float sceneRotationRadius = 24.0f;
    [SerializeField] float sceneCamRotationRate = 3.0f;
    bool sceneCanRotate;
    float sceneCamRotation;



    void Start()
    {
        sceneCam = GameObject.Find("Cameras/SkyCam").GetComponent<Camera>();
        SetSceneCamActive(true);
    }

    public void SetSceneCamActive(bool isActive)
    {
        sceneCam.enabled = isActive;
        sceneCanRotate = isActive;

        if (isActive == true)
            SetCursorToFreeAndVisible();
        else SetCursorToLockAndInvisible();

    }


    void Update()
    {
        rotateSceneCam();

    }
    
    void rotateSceneCam()
    {
        if (!sceneCanRotate)
            return;
        
        sceneCamRotation += sceneCamRotationRate * Time.deltaTime;
        if (sceneCamRotation >= 360)
            sceneCamRotation = -360;

        sceneCam.transform.position = Vector3.zero;
        sceneCam.transform.rotation = Quaternion.Euler(0, sceneCamRotation, 0);
        sceneCam.transform.Translate(0, sceneRotationRadius, -sceneRotationRadius);
        sceneCam.transform.LookAt(Vector3.zero);
    }




    public void SetCursorToLockAndInvisible()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SetCursorToFreeAndVisible()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
