using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    [Header("복원율 UI")]
    public TextMeshProUGUI restorationText;
    public Image restorationCircleFill;

    [Header("단서 UI")]
    public TextMeshProUGUI clueText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateRestorationUI(float value)
    {
        restorationText.text = $"Restoration\n{Mathf.RoundToInt(value)}%";

        // 0 ~ 100 값을 0 ~ 1로 변환하여 원형 UI 반영
        if (restorationCircleFill != null)
        {
            restorationCircleFill.fillAmount = Mathf.Clamp01(value / 100f);
        }
    }

    public void UpdateClueUI(int clues)
    {
        clueText.text = $"Clues\n{clues}";
    }
}
