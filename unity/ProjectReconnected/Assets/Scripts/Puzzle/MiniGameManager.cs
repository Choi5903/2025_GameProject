using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniGameManager : MonoBehaviour
{
    [Header("미니게임 UI 컨테이너들 (1~5)")]
    public List<GameObject> miniGameUIs; // MiniGame_1 ~ MiniGame_5
    [Header("미니게임 UI 스크립트 (1~5)")]
    public List<MonoBehaviour> miniGameScripts; // 각 미니게임에 붙은 스크립트 직접 연결
    [Header("클리어 패널")]
    public GameObject clearPanel;

    private GameObject currentActiveGame = null;

    void Start()
    {
        // 모든 미니게임 UI 비활성화
        foreach (var game in miniGameUIs)
            game.SetActive(false);

        clearPanel.SetActive(false);
    }
    public void StartMiniGame(int index)
    {
        if (currentActiveGame != null)
            currentActiveGame.SetActive(false);

        if (index >= 0 && index < miniGameUIs.Count)
        {
            currentActiveGame = miniGameUIs[index];
            currentActiveGame.SetActive(true);

            clearPanel.SetActive(false);

            // 🔁 명확한 스크립트 참조로 ResetGame 호출
            var script = miniGameScripts[index];
            var method = script?.GetType().GetMethod("ResetGame");
            if (method != null)
                method.Invoke(script, null);

            Debug.Log($"미니게임 {index + 1} 시작!");
        }
    }
    public void OnMiniGameClear()
    {
        clearPanel.SetActive(true);
        Debug.Log("미니게임 클리어!");
    }

    public void CloseClearPanel()
    {
        clearPanel.SetActive(false);
    }

}
