using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryPatternPuzzle : MonoBehaviour
{
    public List<Button> patternButtons; // 순서대로 배치된 버튼들
    public GameObject clearPanel;

    private List<int> pattern = new List<int>();     // 예: [2, 0, 4, 1, 3]
    private List<int> input = new List<int>();       // 플레이어의 입력 순서
    private bool inputEnabled = false;

    public float flashDuration = 0.4f;
    public float flashDelay = 0.3f;

    private Color baseColor = Color.white;
    private Color highlightColor = Color.yellow;
    private Color wrongColor = Color.red;

    void Start()
    {
        clearPanel.SetActive(false);

        for (int i = 0; i < patternButtons.Count; i++)
        {
            int index = i;
            patternButtons[i].onClick.AddListener(() => OnButtonClicked(index));
            SetButtonColor(i, baseColor);
        }

        StartCoroutine(PlayPattern());
    }

    IEnumerator PlayPattern()
    {
        inputEnabled = false;
        pattern = GeneratePattern(5); // 5개의 랜덤 순서 생성
        input.Clear();

        yield return new WaitForSeconds(1f);

        foreach (int index in pattern)
        {
            yield return FlashButton(index, highlightColor);
            yield return new WaitForSeconds(flashDelay);
        }

        inputEnabled = true;
    }

    List<int> GeneratePattern(int count)
    {
        List<int> newPattern = new List<int>();
        List<int> available = new List<int>() { 0, 1, 2, 3, 4 };

        while (newPattern.Count < count && available.Count > 0)
        {
            int pick = available[Random.Range(0, available.Count)];
            newPattern.Add(pick);
            available.Remove(pick); // 중복 방지
        }

        return newPattern;
    }

    void OnButtonClicked(int index)
    {
        if (!inputEnabled) return;

        StartCoroutine(FlashButton(index, highlightColor));
        input.Add(index);

        if (input.Count == pattern.Count)
        {
            inputEnabled = false;
            StartCoroutine(CheckResult());
        }
    }

    IEnumerator CheckResult()
    {
        yield return new WaitForSeconds(0.2f);

        bool isCorrect = true;
        for (int i = 0; i < pattern.Count; i++)
        {
            if (pattern[i] != input[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            clearPanel.SetActive(true);
            Debug.Log("패턴 퍼즐 클리어!");
            FindObjectOfType<MiniGameManager>()?.OnMiniGameClear();
        }
        else
        {
            Debug.Log("틀렸습니다. 다시 시도하세요.");
            yield return FlashAllButtons(wrongColor);
            StartCoroutine(PlayPattern());
        }
    }

    IEnumerator FlashButton(int index, Color color)
    {
        SetButtonColor(index, color);
        yield return new WaitForSeconds(flashDuration);
        SetButtonColor(index, baseColor);
    }

    IEnumerator FlashAllButtons(Color color)
    {
        foreach (var btn in patternButtons)
            btn.GetComponent<Image>().color = color;

        yield return new WaitForSeconds(flashDuration);

        foreach (var btn in patternButtons)
            btn.GetComponent<Image>().color = baseColor;
    }

    void SetButtonColor(int index, Color color)
    {
        patternButtons[index].GetComponent<Image>().color = color;
    }
    public void ResetGame()
    {
        StopAllCoroutines();
        clearPanel?.SetActive(false);

        for (int i = 0; i < patternButtons.Count; i++)
        {
            SetButtonColor(i, baseColor);
        }

        input.Clear();
        pattern.Clear();
        StartCoroutine(PlayPattern());
    }

}
