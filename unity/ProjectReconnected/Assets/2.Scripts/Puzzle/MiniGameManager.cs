using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniGameManager : MonoBehaviour
{
    [Header("미니게임 UI 컨테이너들 (1~5)")]
    public List<GameObject> miniGameUIs;
    [Header("미니게임 UI 스크립트 (1~5)")]
    public List<MonoBehaviour> miniGameScripts;
    //[Header("클리어 패널")]
    //public GameObject clearPanel;

    private GameObject currentActiveGame = null;
    [HideInInspector] public MonoBehaviour currentGameScript = null;

    void Start()
    {
        foreach (var game in miniGameUIs)
            game.SetActive(false);

        //clearPanel.SetActive(false);
    }

    public void StartMiniGame(int index)
    {
        if (currentActiveGame != null)
            currentActiveGame.SetActive(false);

        if (index >= 0 && index < miniGameUIs.Count)
        {
            currentActiveGame = miniGameUIs[index];
            currentGameScript = miniGameScripts[index];

            currentActiveGame.SetActive(true);
            //clearPanel.SetActive(false);

            GameManager.Instance.SetMiniGamePlaying(true);

            var method = currentGameScript?.GetType().GetMethod("ResetGame");
            if (method != null)
                method.Invoke(currentGameScript, null);

            Debug.Log($"미니게임 {index + 1} 시작!");
        }
    }

    public void OnMiniGameClear(MiniGameBase miniGame)
    {
        StartCoroutine(CloseMiniGameAfterDelay(miniGame));
    }

    private IEnumerator CloseMiniGameAfterDelay(MiniGameBase miniGame)
    {
        //clearPanel.SetActive(true);

        yield return new WaitForSeconds(2f);

        //clearPanel.SetActive(false);
        currentActiveGame?.SetActive(false);
        GameManager.Instance.SetMiniGamePlaying(false);

        if (miniGame.autoTriggerTarget != null)
        {
            var interactable = miniGame.autoTriggerTarget.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                Debug.Log($"🟢 클리어 후 자동 상호작용 실행: {miniGame.autoTriggerTarget.name}");
            }
        }
    }
}