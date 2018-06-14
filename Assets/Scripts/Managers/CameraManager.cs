using UnityEngine;


public class CameraManager : MonoBehaviour {

    public static CameraManager Instance { get; private set; }

    void Start()
    {
        EnsureSingleton();
    }

    void EnsureSingleton()
    {
        if (Instance == null)
            Instance = this;
         else 
            Destroy(gameObject);
    }


    public static void SetCursorToLockAndInvisible()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void SetCursorToFreeAndVisible()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
