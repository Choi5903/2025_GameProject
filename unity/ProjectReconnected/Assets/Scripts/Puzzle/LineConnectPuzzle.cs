using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LineConnectPuzzle : MonoBehaviour
{
    [System.Serializable]
    public class IconSlot
    {
        public RectTransform iconTransform;
        public Image iconImage;
        public int colorIndex; // 0 ~ 4
    }

    public List<IconSlot> leftIcons;  // 고정된 색상 아이콘들
    public List<IconSlot> rightIcons; // 점멸 후 랜덤 배치
    public GameObject clearPanel;
    public GameObject linePrefab;     // 라인 프리팹
    public Canvas canvas;

    private Dictionary<int, int> connectionMap = new Dictionary<int, int>(); // leftIndex → rightIndex
    private Color[] colorSet = new Color[5] { Color.red, Color.blue, Color.green, Color.black, Color.white };

    private bool isDragging = false;
    private int currentStartIndex = -1;
    private LineRenderer currentLine;
    private bool isInteractable = false;


    private List<GameObject> drawnLines = new List<GameObject>();

    void Start()
    {
        clearPanel.SetActive(false);
        StartCoroutine(ShowPattern());
    }

    void Update()
    {
        if (isDragging && currentLine != null)
        {
            Vector3 mouseScreen = Input.mousePosition;
            mouseScreen.z = 0f;

            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            mouseWorld.z = 0f;

            currentLine.SetPosition(1, mouseWorld);
        }
    }
    IEnumerator ShowPattern()
    {
        isInteractable = false; // 🔒 드래그 금지

        List<int> shuffled = new List<int>() { 0, 1, 2, 3, 4 };
        ShuffleList(shuffled);

        for (int i = 0; i < rightIcons.Count; i++)
        {
            rightIcons[i].colorIndex = shuffled[i];
            rightIcons[i].iconImage.color = Color.gray;
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < rightIcons.Count; i++)
        {
            int colorIndex = rightIcons[i].colorIndex;
            rightIcons[i].iconImage.color = colorSet[colorIndex];
        }

        yield return new WaitForSeconds(2f);

        foreach (var icon in rightIcons)
            icon.iconImage.color = Color.gray;

        EnableDragging(true);
        isInteractable = true; // 🔓 드래그 가능
    }

    void EnableDragging(bool enable)
    {
        for (int i = 0; i < leftIcons.Count; i++)
        {
            Button btn = leftIcons[i].iconTransform.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();

            if (enable)
            {
                int idx = i;
                btn.onClick.AddListener(() => OnStartDrag(idx));
            }
        }

        for (int i = 0; i < rightIcons.Count; i++)
        {
            Button btn = rightIcons[i].iconTransform.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();

            if (enable)
            {
                int idx = i;
                btn.onClick.AddListener(() => OnEndDrag(idx));
            }
        }
    }

    void OnStartDrag(int leftIndex)
    {
        if (!isInteractable || isDragging) return;

        // 이미 연결된 왼쪽이면 다시 연결 못하게 막고 싶으면 아래 주석 해제
        // if (connectionMap.ContainsKey(leftIndex)) return;

        isDragging = true;
        currentStartIndex = leftIndex;

        currentLine = Instantiate(linePrefab, transform).GetComponent<LineRenderer>();
        currentLine.positionCount = 2;

        Vector3 worldPos = leftIcons[leftIndex].iconTransform.position;
        Debug.Log($"[Line Start] Left Icon {leftIndex} World Pos: {worldPos}");

        currentLine.SetPosition(0, worldPos);
        currentLine.SetPosition(1, worldPos); // 초기 끝점도 동일하게

        drawnLines.Add(currentLine.gameObject);
    }

    void OnEndDrag(int rightIndex)
    {
        if (!isDragging) return;
        isDragging = false;

        // 이미 연결된 오른쪽이면 취소
        if (connectionMap.ContainsValue(rightIndex))
        {
            Destroy(currentLine.gameObject);
            currentLine = null;
            Debug.Log("이미 연결된 오른쪽 슬롯입니다.");
            return;
        }

        currentLine.SetPosition(1, rightIcons[rightIndex].iconTransform.position);
        connectionMap[currentStartIndex] = rightIndex;

        currentLine = null;
        currentStartIndex = -1;

        CheckClear();
    }

    void CheckClear()
    {
        if (connectionMap.Count < 5) return;

        bool success = true;
        foreach (var kvp in connectionMap)
        {
            int left = kvp.Key;
            int right = kvp.Value;

            if (leftIcons[left].colorIndex != rightIcons[right].colorIndex)
            {
                success = false;
                break;
            }
        }

        if (success)
        {
            clearPanel.SetActive(true);
            Debug.Log("연결 퍼즐 클리어!");
            FindObjectOfType<MiniGameManager>()?.OnMiniGameClear();
        }
        else
        {
            Debug.Log("틀렸습니다. 다시 시도하세요.");
            StartCoroutine(ResetAfterDelay());
        }
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        ResetGame(); // 🔄 패턴 재시작 포함
    }


    void ResetConnections()
    {
        foreach (GameObject line in drawnLines)
        {
            Destroy(line);
        }
        drawnLines.Clear();
        connectionMap.Clear();
        currentLine = null;
        currentStartIndex = -1;

        EnableDragging(true);
    }

    List<int> ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
        return list;
    }

    public void ResetGame()
    {
        StopAllCoroutines();
        clearPanel?.SetActive(false); // 안전하게 클리어 패널 닫기

        foreach (GameObject line in drawnLines)
            Destroy(line);
        drawnLines.Clear();
        connectionMap.Clear();
        currentLine = null;
        currentStartIndex = -1;

        EnableDragging(true);
        StartCoroutine(ShowPattern()); // 패턴 다시 보여주기
    }

}
