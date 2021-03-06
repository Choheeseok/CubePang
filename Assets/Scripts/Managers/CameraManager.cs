using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    public static CameraManager instance = null;

    private GameObject endSpot;
    private bool isCameraSelect;
    private Vector3 oldPos;

    public bool rotateActive { get; set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        rotateActive = true;
    }

    void Start()
    {
        CameraInit();
    }

    public void CameraReset()
    {
        endSpot = new GameObject();

        float minAngle = float.MaxValue;

        for (int i = 0; i < 4; i++)
        {
            Quaternion destRotation = Quaternion.identity;
            destRotation *= Quaternion.AngleAxis(45 + i * 90, new Vector3(0, 1, 0));
            destRotation *= Quaternion.AngleAxis(-45, new Vector3(1, 0, 0));
            float angle = Quaternion.Angle(destRotation, transform.rotation);

            if (angle < minAngle)
            {
                minAngle = angle;
                endSpot.transform.rotation = destRotation;
            }
        }

        StartCoroutine(SmoothCameraMove(endSpot));
    }

    public void CameraInit()
    {
        endSpot = new GameObject();
        endSpot.transform.rotation.SetLookRotation(new Vector3(0, 0, 1));
        endSpot.transform.Rotate(new Vector3(0, 1, 0), 45);
        endSpot.transform.Rotate(new Vector3(1, 0, 0), -45);
        StartCoroutine(SmoothCameraMove(endSpot));
    }

    private IEnumerator SmoothCameraMove(GameObject endSpot)
    {
        float t = 0;
        while (true)
        {
            if (t > 1)
                break;
            Vector3 localPosition = transform.GetChild(0).transform.localPosition;
            transform.rotation = Quaternion.Lerp(transform.rotation, endSpot.transform.rotation, t);
            transform.GetChild(0).transform.localPosition = Vector3.Lerp(localPosition, new Vector3(localPosition.x, localPosition.y, CustomVariables.CAMERA_ZOOMDEFAULT), t);
            t += 2f * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(endSpot);
    }

    public void ProcessInput()
    {
        //확대 축소
        if (Input.GetKey(KeyCode.W))
        {
            transform.GetChild(0).position += transform.GetChild(0).forward * Time.deltaTime * 10;
            if (transform.GetChild(0).localPosition.z < CustomVariables.CAMERA_ZOOMIN)
                transform.GetChild(0).localPosition = new Vector3(0, 0, CustomVariables.CAMERA_ZOOMIN);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.GetChild(0).position -= transform.GetChild(0).forward * Time.deltaTime * 10;
            if (transform.GetChild(0).localPosition.z > CustomVariables.CAMERA_ZOOMOUT)
                transform.GetChild(0).localPosition = new Vector3(0, 0, CustomVariables.CAMERA_ZOOMOUT);
        }

        //리셋
        if (Input.GetKeyDown(KeyCode.R))
            CameraReset();
    }

    public void RotateInput()
    {
        if (false == rotateActive)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            oldPos = Input.mousePosition;
            isCameraSelect = true;
        }

        if (true == isCameraSelect)
        {
            if (Input.GetMouseButton(0))
            {
                if ((oldPos - Input.mousePosition).sqrMagnitude > 200000)    // 부자연스러운 이동시 움직이지 않습니다.
                    return;

                Vector3 pos = Input.mousePosition - oldPos;

                transform.RotateAround(new Vector3(0, 0, 0), transform.up, pos.x * 0.15f);
                transform.RotateAround(new Vector3(0, 0, 0), transform.right, pos.y * 0.15f);

                oldPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
                isCameraSelect = false;
        }
    }
}
