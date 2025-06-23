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
    public void OnMiniGameClear(MiniGameBase miniGame)
    {
        StartCoroutine(CloseMiniGameAfterDelay(miniGame));
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
    private IEnumerator CloseMiniGameAfterDelay(MiniGameBase miniGame)
    {
        yield return new WaitForSeconds(2f);

        currentActiveGame?.SetActive(false);
        GameManager.Instance.SetMiniGamePlaying(false);

        if (miniGame == null)
        {
            Debug.LogWarning("❌ miniGame is null. 클리어 처리 실패.");
            yield break;
        }

        if (miniGame.autoTriggerTarget != null)
        {
            var interactable = miniGame.autoTriggerTarget.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                Debug.Log($"🟢 클리어 후 자동 상호작용 실행: {miniGame.autoTriggerTarget.name}");
            }
            else
            {
                Debug.LogWarning($"⚠️ autoTriggerTarget에 IInteractable이 없음: {miniGame.autoTriggerTarget.name}");
            }
        }
        else
        {
            Debug.Log("ℹ️ 클리어 후 실행할 오브젝트 없음.");
        }
    }
}