using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : Singleton<CameraManager>
{
    Camera mainCamera;

    public enum CurrentCameraType
    {
        Basic,
        ISO,
        TypeEnd,
    }

    public struct CameraSetting
    {
        public Vector3 FollowOffset;
        public float RotX;
    }

    public CurrentCameraType currentCameraType { get; private set; } = CurrentCameraType.Basic;
    CameraSetting[] cameraSettings = new CameraSetting[(int)CurrentCameraType.TypeEnd];

    protected override void Awake()
    {
        base.Awake();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            SetCameraSetting();
        }
    }
    void SetCameraSetting()
    {
        cameraSettings[(int)CurrentCameraType.Basic].FollowOffset = mainCamera.GetComponent<CinemachineFollow>().FollowOffset;
        cameraSettings[(int)CurrentCameraType.Basic].RotX = mainCamera.transform.eulerAngles.x;

        cameraSettings[(int)CurrentCameraType.ISO].FollowOffset = new Vector3(0f, 15f, -7.6f);
        cameraSettings[(int)CurrentCameraType.ISO].RotX = 65f;
    }
    public void ChangeCameraSetting()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            SetCameraSetting();
            return;
        }
        if (currentCameraType == CurrentCameraType.Basic)
        {
            //mainCamera.GetComponent<CinemachineFollow>().FollowOffset = cameraSettings[(int)CurrentCameraType.ISO].FollowOffset;
            //mainCamera.transform.rotation = Quaternion.Euler(cameraSettings[(int)CurrentCameraType.ISO].RotX, 0f, 0f);
            mainCamera.GetComponent<Camera>().orthographic = true;
            mainCamera.GetComponent<Camera>().orthographicSize = 10f;
            currentCameraType = CurrentCameraType.ISO;
        }
        else if (currentCameraType == CurrentCameraType.ISO)
        {
            //mainCamera.GetComponent<CinemachineFollow>().FollowOffset = cameraSettings[(int)CurrentCameraType.Basic].FollowOffset;
            //mainCamera.transform.rotation = Quaternion.Euler(cameraSettings[(int)CurrentCameraType.Basic].RotX, 0f, 0f);
            mainCamera.GetComponent<Camera>().orthographic = false;
            currentCameraType = CurrentCameraType.Basic;
        }
    }

    public void InitCameraSetting()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            SetCameraSetting();
        }

        if (currentCameraType == CurrentCameraType.Basic)
        {
            mainCamera.GetComponent<Camera>().orthographic = false;
        }
        else if (currentCameraType == CurrentCameraType.ISO)
        {
            mainCamera.GetComponent<Camera>().orthographic = true;
            mainCamera.GetComponent<Camera>().orthographicSize = 10f;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SetCameraSetting(SceneManager.GetActiveScene());
    }

    public void SetCameraSetting(Scene scene)
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            SetCameraSetting();
            return;
        }

        //if (currentCameraType == CurrentCameraType.Basic)
        //{
        //    mainCamera.GetComponent<Camera>().orthographic = true;
        //    mainCamera.GetComponent<Camera>().orthographicSize = 10f;
        //
        //    currentCameraType = CurrentCameraType.ISO;
        //}
        //else if (currentCameraType == CurrentCameraType.ISO)
        //{
        //    mainCamera.GetComponent<Camera>().orthographic = false;
        //    currentCameraType = CurrentCameraType.Basic;
        //}
        ChangeCameraSetting();
    }

    public float GetRotX()
    {
        return currentCameraType == CurrentCameraType.Basic ? 0 : cameraSettings[(int)CurrentCameraType.ISO].RotX;
    }
}
