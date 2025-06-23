using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LineConnectPuzzle : MiniGameBase
{
    [System.Serializable]
    public class IconSlot
    {
        public RectTransform iconTransform;
        public Image iconImage;
        public int colorIndex;
    }

    public List<IconSlot> leftIcons;
    public List<IconSlot> rightIcons;
    // public GameObject clearPanel; // clearPanel 제거
    public GameObject linePrefab;
    public Canvas canvas;

    private Dictionary<int, int> connectionMap = new Dictionary<int, int>();
    private Color[] colorSet = new Color[5] { Color.red, Color.blue, Color.green, Color.black, Color.white };

    private bool isDragging = false;
    private int currentStartIndex = -1;
    private LineRenderer currentLine;
    private bool isInteractable = false;
    private List<GameObject> drawnLines = new List<GameObject>();

    void Start()
    {
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
        isInteractable = false;

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
        isInteractable = true;
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

        isDragging = true;
        currentStartIndex = leftIndex;

        currentLine = Instantiate(linePrefab, transform).GetComponent<LineRenderer>();
        currentLine.positionCount = 2;

        Vector3 worldPos = leftIcons[leftIndex].iconTransform.position;
        currentLine.SetPosition(0, worldPos);
        currentLine.SetPosition(1, worldPos);

        drawnLines.Add(currentLine.gameObject);
    }

    void OnEndDrag(int rightIndex)
    {
        if (!isDragging) return;
        isDragging = false;

        if (connectionMap.ContainsValue(rightIndex))
        {
            Destroy(currentLine.gameObject);
            currentLine = null;
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
            Debug.Log("연결 퍼즐 클리어!");
            NotifyClear(); // MiniGameBase에 의해 MiniGameManager로 전달됨
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
        ResetGame();
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

    public override void ResetGame()
    {
        StopAllCoroutines();

        foreach (GameObject line in drawnLines)
            Destroy(line);
        drawnLines.Clear();
        connectionMap.Clear();
        currentLine = null;
        currentStartIndex = -1;

        EnableDragging(true);

        // 🟢 퍼즐 초기화 시작 (재시작 시에도 작동하게)
        StartCoroutine(ShowPattern());
    }
}
