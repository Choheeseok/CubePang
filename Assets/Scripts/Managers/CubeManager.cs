using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public static CubeManager instance = null;

    public GameObject cubePrefab;
    public Tile tilePrefab;
    public GameObject mouseMoveParticle;
    private GameObject playParticle;
    //큐브, 타일 몬스터 프리팹

    public LayerMask TileLayer;

    private List<GameObject> cubeList;
    //큐브와 타일 인스턴스들을 저장하기 위한 리스트

    private Transform cubeHolder;
    //생성된 큐브들을 같은 항목에 넣어준다.
    private Transform backgroundCubeHolder;

    private int cubeCount;
    private float radius;
    //타일 하나의 반지름

    private float rotatedAngle;
    //얼만큼 돌았는지 누적해서 저장.
    //마우스 좌 버튼을 땠을 때 이 값을 빼줘서 원점으로 돌린 후,
    //0,90,180,270 도에 대해 공전
    private bool isSelect;
    //회전해야 할 블럭이 선택되었는가
    private bool isDirConfirmed;
    //방향이 결정되었나
    private int[] idx;
    //9개의 블럭을 인덱스로 기억하기 위한 변수
    //27개의 블럭을 돌면서 찾는 것은 비효율 적이기 때문에
    private Vector3 axis;
    //어느 축으로 돌 지

    private Vector3 oldWorldPos;
    //마우스가 피킹한 지점의 월드 좌표
    //마우스에서 발생한 ray가 오브젝트에 충돌한 지점
    private Vector3 oldScreenPos;
    //클릭했을 때 마우스의 스크린 좌표
    //충분히 드래그를 했을 때 회전이 시작되도록
    //최초 클릭 좌표와 이동된 좌표의 거리를 비교하여
    //일정 크기 이상이 되었을 때 회전이 시작되도록 함
    private Tile colObject;
    private MeshRenderer colObjectMeshRenderer;
    //충돌된 타일
    //충돌된 타일의 up벡터와 좌표에 따라
    //회전하는 면(9개)이 결정됨
    private Color prevColor;
    private Color clickedColor;

    //타일의 원래 색을 저장한다
    //클릭된 타일은 검정색으로 표시한다.
    //마우스를 땠을 때 다시 원래 색으로 돌리기 위해

    private int reverseRotation;

    private Quaternion RotationToAxisX;

    private float adjustAngle;
    private bool isAdjusting;

    private float clickTime;
    private Vector3 PrevTilePosition;

    private float holdingTime;
    public bool isWorldShaking { get; set; }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        clickTime = Time.time;
        PrevTilePosition = Vector3.zero;
        clickedColor = Color.white;

        cubeList = new List<GameObject>();

        cubeHolder = new GameObject("CubeHolder").transform;
        axis = new Vector3(0, 0, 0);
        isSelect = false;
        isWorldShaking = false;
        rotatedAngle = 0;

        playParticle = Instantiate(mouseMoveParticle);
    }

    private void Start()
    {
        GameManager.instance.ConfirmBingo();

        cubeCount = GameManager.instance.CubeCount;
        radius = GameManager.instance.radius;
        idx = new int[cubeCount * cubeCount];

        float margin = 0.0f;

        for (int i = 0; i < cubeCount; ++i)
            for (int j = 0; j < cubeCount; ++j)
                for (int k = 0; k < cubeCount; ++k)
                {
                    cubeList.Add(Instantiate(cubePrefab, new Vector3(radius * (2 * i - (cubeCount - 1)), radius * (2 * j - (cubeCount - 1)), radius * (2 * k - (cubeCount - 1))), Quaternion.identity));
                    cubeList[cubeList.Count - 1].transform.SetParent(cubeHolder);
                }

        for (int i = 0; i < cubeList.Count; ++i)
        {
            cubeList[i].GetComponent<BoxCollider>().enabled = false;

            if (!Physics.Linecast(cubeList[i].transform.position, cubeList[i].transform.position + cubeList[i].transform.up))
            {
                Tile tile;
                tile = Instantiate(tilePrefab, cubeList[i].transform.position + cubeList[i].transform.up * (radius + margin), Quaternion.identity, cubeList[i].transform);
                GameManager.instance.TileList.Add(tile);
            }

            if (!Physics.Linecast(cubeList[i].transform.position, cubeList[i].transform.position - cubeList[i].transform.up))
            {
                Tile tile;
                tile = Instantiate(tilePrefab, cubeList[i].transform.position - cubeList[i].transform.up * (radius + margin), Quaternion.Euler(0, 0, 180), cubeList[i].transform);
                GameManager.instance.TileList.Add(tile);
            }

            if (!Physics.Linecast(cubeList[i].transform.position, cubeList[i].transform.position + cubeList[i].transform.right))
            {
                Tile tile;
                tile = Instantiate(tilePrefab, cubeList[i].transform.position + cubeList[i].transform.right * (radius + margin), Quaternion.Euler(0, 0, -90), cubeList[i].transform);
                GameManager.instance.TileList.Add(tile);
            }

            if (!Physics.Linecast(cubeList[i].transform.position, cubeList[i].transform.position - cubeList[i].transform.right))
            {
                Tile tile;
                tile = Instantiate(tilePrefab, cubeList[i].transform.position - cubeList[i].transform.right * (radius + margin), Quaternion.Euler(0, 0, 90), cubeList[i].transform);
                GameManager.instance.TileList.Add(tile);
            }

            if (!Physics.Linecast(cubeList[i].transform.position, cubeList[i].transform.position + cubeList[i].transform.forward))
            {
                Tile tile;
                tile = Instantiate(tilePrefab, cubeList[i].transform.position + cubeList[i].transform.forward * (radius + margin), Quaternion.Euler(90, 0, 0), cubeList[i].transform);
                GameManager.instance.TileList.Add(tile);
            }

            if (!Physics.Linecast(cubeList[i].transform.position, cubeList[i].transform.position - cubeList[i].transform.forward))
            {
                Tile tile;
                tile = Instantiate(tilePrefab, cubeList[i].transform.position - cubeList[i].transform.forward * (radius + margin), Quaternion.Euler(-90, 0, 0), cubeList[i].transform);
                GameManager.instance.TileList.Add(tile);
            }
            cubeList[i].GetComponent<BoxCollider>().enabled = true;
        }

        foreach (var cube in cubeList)
        {
            cube.GetComponent<BoxCollider>().enabled = false;
        }

        foreach (var tile in GameManager.instance.TileList)
        {
            KeyValuePair<CustomVariables.COLOR, Color> pair= TileColors.RandomColor(GameManager.instance.Level);
            GameManager.instance.TileCountDictionary[CustomVariables.COLOR.RED]++;
            tile.SetColor(pair);
            tile.type = CustomVariables.TILE.EMPTY;
        }
    }

    private void AttemptToSelectTile()
    {
        oldScreenPos = Input.mousePosition;
        GameManager.instance.IsCubeSelected = true;
    }

    private IEnumerator AdjustRotation(float destAngle)
    {
        isAdjusting = true;
        float stackedAdjustAngle = 0;

        while (true)
        {
            float curTime = Time.deltaTime;
            stackedAdjustAngle += adjustAngle * curTime * 2;
            for (int i = 0; i < cubeCount * cubeCount; i++)
                cubeList[idx[i]].transform.RotateAround(new Vector3(0, 0, 0), axis, -adjustAngle * curTime * 2);

            if (Mathf.Abs(stackedAdjustAngle) >= Mathf.Abs(adjustAngle))
                break;

            yield return null;
        }

        for (int i = 0; i < cubeCount * cubeCount; i++)
            cubeList[idx[i]].transform.RotateAround(new Vector3(0, 0, 0), axis, stackedAdjustAngle - adjustAngle);

        if (destAngle != 0)
        {
            GameManager.instance.PlayerTurnEnd = true;
            GameManager.instance.ConfirmBingo();
        }
        ResetInform();
    }

    private void FinishRotation()
    {
        int sign = rotatedAngle < 0 ? -1 : 1;
        rotatedAngle *= sign;
        rotatedAngle %= 360;
        rotatedAngle *= sign;

        if (-45 < rotatedAngle && rotatedAngle <= 45)
        {
            adjustAngle = rotatedAngle - 0;
            StartCoroutine(AdjustRotation(0));
        }
        else if (-135 < rotatedAngle && rotatedAngle <= 135)
        {
            adjustAngle = rotatedAngle - sign * 90;
            StartCoroutine(AdjustRotation(sign * 90));
        }
        else if (-225 < rotatedAngle && rotatedAngle <= 225)
        {
            adjustAngle = rotatedAngle - sign * 180;
            StartCoroutine(AdjustRotation(sign * 180));
        }
        else if (-315 < rotatedAngle && rotatedAngle <= 315)
        {
            adjustAngle = rotatedAngle - sign * 270;
            StartCoroutine(AdjustRotation(sign * 270));
        }
    }

    private void ResetInform()
    {
        rotatedAngle = 0;
        isDirConfirmed = false;
        idx = new int[cubeCount * cubeCount];
        axis = new Vector3(0, 0, 0);
        isSelect = false;
        GameManager.instance.IsCubeSelected = false;
        isAdjusting = false;
        colObjectMeshRenderer.material.color = prevColor;
    }

    private void RotateCube()
    {
        if (!isDirConfirmed)
        {
            if (Vector3.Distance(Input.mousePosition, oldScreenPos) > 10)
                ConfirmDirection();
        }
        else
        {
            float tmpAngle = 0;
            tmpAngle = ((RotationToAxisX * Input.mousePosition).x - oldScreenPos.x) * reverseRotation * 0.3f;

            oldScreenPos = RotationToAxisX * Input.mousePosition;

            rotatedAngle += tmpAngle;
            for (int i = 0; i < idx.Length; i++)
                cubeList[idx[i]].transform.RotateAround(new Vector3(0, 0, 0), axis, tmpAngle);
        }
    }

    public void ProcessMouseInput()
    {
        if (isAdjusting)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            oldWorldPos = GetTileCollisionPos();

            if (isSelect)
            {
                playParticle.GetComponent<ParticleSystem>().Play();

                clickedColor = colObject.transform.GetComponent<MeshRenderer>().material.color;
                clickedColor *= 0.5f;
                colObject.transform.GetComponent<MeshRenderer>().material.color = clickedColor;
                AttemptToSelectTile();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isSelect)
            {
                FinishRotation();
                GameManager.instance.IsCubeSelected = false;

                holdingTime = 0.0f;

                playParticle.GetComponent<ParticleSystem>().Stop();
            }
        }
        if (Input.GetMouseButton(0))
        {
            if (isSelect)
            {
                RotateCube();
                playParticle.transform.position = colObject.transform.position + colObject.transform.up*0.8f;
            }
        }
    }

    //충돌한 타일중 가장 가까운 타일의 충돌 지점의 월드 좌표를 반환한다.
    private Vector3 GetTileCollisionPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out RaycastHit hit, 100, TileLayer);

        if (hit.transform != null)
        {
            if (Vector3.Dot(ray.direction, hit.transform.up) < 0)
            {
                if (!isSelect)
                {
                    isSelect = true;
                    colObject = GameManager.instance.GetTile(hit.transform);
                    colObjectMeshRenderer = colObject.transform.GetComponent<MeshRenderer>();
                    if (colObjectMeshRenderer.material.color != clickedColor && prevColor != colObjectMeshRenderer.material.color)
                        prevColor = colObjectMeshRenderer.material.color;
                }
                return hit.point;
            }
        }
        return new Vector3(0, 0, 0);
    }

    //회전 방향을 정하고 돌릴 블럭들과 회전축을 정한다.
    private void ConfirmDirection()
    {
        Vector3 upVector = colObject.transform.up;
        Vector3 colPos = GetTileCollisionPos();
        Vector3 deltaVector = colPos - oldWorldPos;
        Vector3 colCubePos = colObject.transform.parent.transform.position;
        Vector3 addVector = new Vector3(0, 0, 0);
        int j = 0;

        if (upVector.x > 0.9 || upVector.x < -0.9)
        {
            if (Mathf.Abs(deltaVector.y) > Mathf.Abs(deltaVector.z))
            {
                for (int i = 0; i < cubeList.Count; i++)
                    if (colCubePos.z - radius < cubeList[i].transform.position.z && cubeList[i].transform.position.z < colCubePos.z + radius)
                        idx[j++] = i;

                axis = new Vector3(0, 0, 1);
                addVector.y += radius;
            }
            else
            {
                for (int i = 0; i < cubeList.Count; i++)
                    if (colCubePos.y - radius < cubeList[i].transform.position.y && cubeList[i].transform.position.y < colCubePos.y + radius)
                        idx[j++] = i;

                axis = new Vector3(0, -1, 0);
                addVector.z += radius;
            }
        }
        else if (upVector.y > 0.9 || upVector.y < -0.9)
        {
            if (Mathf.Abs(deltaVector.x) > Mathf.Abs(deltaVector.z))
            {
                for (int i = 0; i < cubeList.Count; i++)
                    if (colCubePos.z - radius < cubeList[i].transform.position.z && cubeList[i].transform.position.z < colCubePos.z + radius)
                        idx[j++] = i;

                axis = new Vector3(0, 0, -1);
                addVector.x += radius;
            }
            else
            {
                for (int i = 0; i < cubeList.Count; i++)
                    if (colCubePos.x - radius < cubeList[i].transform.position.x && cubeList[i].transform.position.x < colCubePos.x + radius)
                        idx[j++] = i;

                axis = new Vector3(1, 0, 0);
                addVector.z += radius;
            }
        }
        else if (upVector.z > 0.9 || upVector.z < -0.9)
        {
            if (Mathf.Abs(deltaVector.x) > Mathf.Abs(deltaVector.y))
            {
                for (int i = 0; i < cubeList.Count; i++)
                    if (colCubePos.y - radius < cubeList[i].transform.position.y && cubeList[i].transform.position.y < colCubePos.y + radius)
                        idx[j++] = i;

                axis = new Vector3(0, 1, 0);
                addVector.x += radius;
            }
            else
            {
                for (int i = 0; i < cubeList.Count; i++)
                    if (colCubePos.x - radius < cubeList[i].transform.position.x && cubeList[i].transform.position.x < colCubePos.x + radius)
                        idx[j++] = i;
                axis = new Vector3(-1, 0, 0);
                addVector.y += radius;
            }
        }

        if (upVector.x + upVector.y + upVector.z > 0)
            reverseRotation = 1;
        else
            reverseRotation = -1;

        Vector3 screenLine = Camera.main.WorldToScreenPoint(colCubePos + addVector) - Camera.main.WorldToScreenPoint(colCubePos);
        screenLine.z = 0;

        float degree = Vector3.SignedAngle(screenLine, new Vector3(1, 0, 0), new Vector3(0, 0, 1));
        RotationToAxisX = Quaternion.Euler(0.0f, 0.0f, degree);
        oldScreenPos = RotationToAxisX * Input.mousePosition;

        isDirConfirmed = true;
    }
}