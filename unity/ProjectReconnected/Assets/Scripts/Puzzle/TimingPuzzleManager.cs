using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingPuzzleManager : MonoBehaviour
{
    public List<TimingSlider> sliders;   // 5개 슬라이더 (좌 → 우 순서)
    public Button stopButton;
    public GameObject clearPanel;

    public float targetMin = 0.45f;
    public float targetMax = 0.55f;

    private int currentIndex = 0;
    private bool puzzleCleared = false;

    void Start()
    {
        clearPanel.SetActive(false);
        stopButton.onClick.AddListener(OnStopButtonPressed);
        ResetPuzzle();
    }

    public void ResetPuzzle()
    {
        StopAllCoroutines();
        foreach (var s in sliders)
            s.ResetSlider();

        currentIndex = 0;
        puzzleCleared = false;
        clearPanel.SetActive(false);
        sliders[currentIndex].StartSlider();
    }

    void OnStopButtonPressed()
    {
        if (puzzleCleared || currentIndex >= sliders.Count) return;

        float val = sliders[currentIndex].GetCurrentValue();
        if (val >= targetMin && val <= targetMax)
        {
            // 성공
            sliders[currentIndex].StopSlider();
            currentIndex++;

            if (currentIndex >= sliders.Count)
            {
                PuzzleClear();
            }
            else
            {
                sliders[currentIndex].StartSlider();
            }
        }
        else
        {
            // 실패
            Debug.Log("타이밍 실패!");
            ResetPuzzle();
        }
    }

    void PuzzleClear()
    {
        puzzleCleared = true;
        clearPanel.SetActive(true);
        Debug.Log("타이밍 퍼즐 클리어!");
        FindObjectOfType<MiniGameManager>()?.OnMiniGameClear();
    }
    public void ResetGame()
    {
        ResetPuzzle();
    }
}
