using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCamera : MonoBehaviour {

    [SerializeField] float rotationRadius = 24.0f;
    [SerializeField] float rotationRate = 3.0f;

    static Camera sceneCam;
    static bool sceneCanRotate;
    static float sceneCamRotation;


    void Start()
    {
        sceneCam = GameObject.Find("Cameras/SkyCam").GetComponent<Camera>();
        SetSceneCamActive(true);
    }

    void Update()
    {
        RotateSceneCam();
    }

    public static void SetSceneCamActive(bool isActive)
    {
        sceneCanRotate = isActive;
        sceneCam.enabled = isActive;

        if (isActive == true)
            CameraManager.SetCursorToFreeAndVisible();
        else CameraManager.SetCursorToLockAndInvisible();
    }

    void RotateSceneCam()
    {
        if (!sceneCanRotate)
            return;

        sceneCamRotation += rotationRate * Time.deltaTime;
        if (sceneCamRotation >= 360)
            sceneCamRotation = -360;

        sceneCam.transform.position = Vector3.zero;
        sceneCam.transform.rotation = Quaternion.Euler(0, sceneCamRotation, 0);
        sceneCam.transform.Translate(0, rotationRadius, -rotationRadius);
        sceneCam.transform.LookAt(Vector3.zero);
    }
}
