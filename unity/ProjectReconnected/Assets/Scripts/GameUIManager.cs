using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;

    [Header("복원율 UI")]
    public TMP_Text restorationText;
    public Slider restorationBar;

    [Header("단서 UI")]
    public TMP_Text clueText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateRestorationUI(float value)
    {
        restorationText.text = $"Restoration: {value}%";
        restorationBar.value = value / 100f;
    }

    public void UpdateClueUI(int count)
    {
        clueText.text = $"Clues: {count}";
    }
}
