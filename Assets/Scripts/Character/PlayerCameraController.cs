using UnityEngine;


[RequireComponent((typeof(Camera)))]
public class PlayerCameraController : MonoBehaviour {

    const float MIN_CLAMP_ANGLE = -60f, MAX_CLAMP_ANGLE = 70.0f;
    const float SMOOTH_SPEED_IN = .999f, SMOOTH_SPEED_OUT = 1.02f;

    public Transform LookAtTransform;
    public float DefaultDistance = 2.5f;

    Camera PlayerCam;
    Transform PlayerCamTransform;
    float distance;
    float currentY;
    float sensitivity = .5f;
    Vector3 offset;
    

    void Start() {
        PlayerCam = GetComponent<Camera>();
        PlayerCamTransform = PlayerCam.transform;
        distance = DefaultDistance;
        offset = new Vector3(0, 0, -DefaultDistance); 
    }

    void Update() {
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
        PlayerCamTransform.position = LookAtTransform.position + LookAtTransform.rotation * offset;
        PlayerCamTransform.LookAt(LookAtTransform.position);
 
        Quaternion upAndDown = Quaternion.Euler(-currentY * sensitivity, 0, 0);
        PlayerCamTransform.localRotation *= upAndDown;
    }


    /*Manual lerping of camera if camera collides with an object*/
    void CameraCollision() {
        RaycastHit hit;
        if (Physics.Linecast(LookAtTransform.position, PlayerCamTransform.position, out hit))
        {
            distance = Mathf.Clamp(hit.distance * SMOOTH_SPEED_IN, 1, DefaultDistance);
        }
        else
        {
            float isNextDistanceACollisionDistance = Mathf.Clamp(distance * SMOOTH_SPEED_OUT, 1, DefaultDistance);
            Vector3 testOffset = new Vector3(offset.x, offset.y, -isNextDistanceACollisionDistance);

            //NOTE - Code is copied from UpdateCamera()
            Transform nextCamTransform = PlayerCamTransform;
            nextCamTransform.position = LookAtTransform.position + LookAtTransform.rotation * testOffset;

            //Pushes camera back if it wont cause a new collision - caused jittery cam
            if (!Physics.Linecast(LookAtTransform.position, nextCamTransform.position, out hit))
            {
                distance = Mathf.Clamp(distance * SMOOTH_SPEED_OUT, 1, DefaultDistance);
            }
        }
    }



    public Vector3 GetHitmarkerPointInWorld(SpellStats spell, out RaycastHit camHit, out bool camFoundHit)
    {
        Ray camRay = PlayerCam.ViewportPointToRay(Vector3.one * 0.5f);

        LayerMask ignorePlayerMask = ~(1 << 8);
        float rangeToUse = spell.maxRange + 3.0f;

        Vector3 hitPoint;

        if (Physics.Raycast(camRay, out camHit, rangeToUse, ignorePlayerMask))
        {
            camFoundHit = true;
            hitPoint = camHit.point;
        }
        else
        {
            camFoundHit = false;
            hitPoint = PlayerCam.transform.position + PlayerCam.transform.forward * rangeToUse + new Vector3(0, .4f, 0);
        }
        return hitPoint;
    }



}
