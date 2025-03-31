using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    private const float ROTATION_SPEED = 0.15f;
    private const float MOVEMENT_SPEED = 10f;
    private const float SMOOTH_MOVE_SPEED = 2f;
    private const float MAX_MOUSE_MOVEMENT = 200000f;
    private const float INITIAL_Y_ROTATION = 45f;
    private const float INITIAL_X_ROTATION = -45f;
    private const float ROTATION_STEP = 90f;

    public static CameraManager instance { get; private set; }

    private GameObject endSpot;
    private bool isCameraSelect;
    private Vector3 oldPos;
    private Transform cameraTransform;

    public bool rotateActive { get; set; }

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        cameraTransform = transform.GetChild(0);
        CameraInit();
    }

    private void InitializeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        rotateActive = true;
    }

    public void CameraReset()
    {
        endSpot = new GameObject();
        Quaternion bestRotation = FindBestRotation();
        endSpot.transform.rotation = bestRotation;
        StartCoroutine(SmoothCameraMove(endSpot));
    }

    private Quaternion FindBestRotation()
    {
        float minAngle = float.MaxValue;
        Quaternion bestRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            Quaternion destRotation = Quaternion.identity;
            destRotation *= Quaternion.AngleAxis(INITIAL_Y_ROTATION + i * ROTATION_STEP, Vector3.up);
            destRotation *= Quaternion.AngleAxis(INITIAL_X_ROTATION, Vector3.right);
            
            float angle = Quaternion.Angle(destRotation, transform.rotation);
            if (angle < minAngle)
            {
                minAngle = angle;
                bestRotation = destRotation;
            }
        }

        return bestRotation;
    }

    public void CameraInit()
    {
        endSpot = new GameObject();
        endSpot.transform.rotation.SetLookRotation(Vector3.forward);
        endSpot.transform.Rotate(Vector3.up, INITIAL_Y_ROTATION);
        endSpot.transform.Rotate(Vector3.right, INITIAL_X_ROTATION);
        StartCoroutine(SmoothCameraMove(endSpot));
    }

    private IEnumerator SmoothCameraMove(GameObject endSpot)
    {
        float t = 0;
        Vector3 localPosition = cameraTransform.localPosition;

        while (t <= 1)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, endSpot.transform.rotation, t);
            cameraTransform.localPosition = Vector3.Lerp(localPosition, 
                new Vector3(localPosition.x, localPosition.y, GameConstants.CAMERA_ZOOMDEFAULT), t);
            
            t += SMOOTH_MOVE_SPEED * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(endSpot);
    }

    public void ProcessInput()
    {
        HandleZoomInput();
        HandleResetInput();
    }

    private void HandleZoomInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            cameraTransform.position += cameraTransform.forward * Time.deltaTime * MOVEMENT_SPEED;
            ClampCameraZoom(true);
        }
        if (Input.GetKey(KeyCode.S))
        {
            cameraTransform.position -= cameraTransform.forward * Time.deltaTime * MOVEMENT_SPEED;
            ClampCameraZoom(false);
        }
    }

    private void ClampCameraZoom(bool isZoomingIn)
    {
        float targetZ = isZoomingIn ? GameConstants.CAMERA_ZOOMIN : GameConstants.CAMERA_ZOOMOUT;
        cameraTransform.localPosition = new Vector3(0, 0, targetZ);
    }

    private void HandleResetInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CameraReset();
        }
    }

    public void RotateInput()
    {
        if (!rotateActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            oldPos = Input.mousePosition;
            isCameraSelect = true;
        }

        if (isCameraSelect)
        {
            if (Input.GetMouseButton(0))
            {
                HandleCameraRotation();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isCameraSelect = false;
            }
        }
    }

    private void HandleCameraRotation()
    {
        Vector3 mouseDelta = Input.mousePosition - oldPos;
        if (mouseDelta.sqrMagnitude > MAX_MOUSE_MOVEMENT) return;

        transform.RotateAround(Vector3.zero, transform.up, mouseDelta.x * ROTATION_SPEED);
        transform.RotateAround(Vector3.zero, transform.right, mouseDelta.y * ROTATION_SPEED);

        oldPos = Input.mousePosition;
    }
}
