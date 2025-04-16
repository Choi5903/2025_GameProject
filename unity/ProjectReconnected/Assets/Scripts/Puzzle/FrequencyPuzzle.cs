using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FrequencyPuzzle : MonoBehaviour
{
    [System.Serializable]
    public class FrequencySlider
    {
        public Slider slider;
        public Image feedbackImage;
        public float targetValue;     // 정답 값 (0~1)
        public float tolerance = 0.05f; // 허용 오차

        public bool IsCorrect()
        {
            return Mathf.Abs(slider.value - targetValue) <= tolerance;
        }

        public void UpdateFeedback()
        {
            float distance = Mathf.Abs(slider.value - targetValue);

            if (distance <= tolerance)
                feedbackImage.color = Color.green;
            else if (distance <= tolerance * 3f)
                feedbackImage.color = Color.yellow;
            else
                feedbackImage.color = Color.gray;
        }
    }

    public List<FrequencySlider> sliders;
    public GameObject clearPanel;

    private bool isCleared = false;

    void Start()
    {
        foreach (var s in sliders)
        {
            s.slider.onValueChanged.AddListener((_) => OnSliderChanged());
            s.UpdateFeedback();
        }

        clearPanel.SetActive(false);
    }

    void OnSliderChanged()
    {
        if (isCleared) return;

        foreach (var s in sliders)
            s.UpdateFeedback();

        if (CheckAllCorrect())
        {
            isCleared = true;
            clearPanel.SetActive(true);
            Debug.Log("주파수 퍼즐 클리어!");

            // 미니게임 매니저 호출
            MiniGameManager mgr = FindObjectOfType<MiniGameManager>();
            if (mgr != null)
                mgr.OnMiniGameClear();
        }
    }

    bool CheckAllCorrect()
    {
        foreach (var s in sliders)
        {
            if (!s.IsCorrect())
                return false;
        }
        return true;
    }

    public void ResetGame()
    {
        isCleared = false;
        clearPanel?.SetActive(false);

        foreach (var s in sliders)
        {
            s.slider.value = 0.5f;
            s.UpdateFeedback();
        }
    }

}
