using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VNSceneDialogueController : MonoBehaviour
{
    [Header("대화 데이터")]
    public DialogueData dialogueData;

    [Header("씬 이동 버튼")]
    public Button nextSceneButton;

    [Header("이동할 씬 이름")]
    public string nextSceneName;

    private void Start()
    {
        // 버튼 비활성화 상태로 시작
        if (nextSceneButton != null)
            nextSceneButton.gameObject.SetActive(false);

        // 대사 자동 재생
        DialogueManager.Instance.StartDialogue(dialogueData, OnDialogueComplete);
    }

    private void OnDialogueComplete()
    {
        // 대사 종료 후 버튼 표시
        if (nextSceneButton != null)
        {
            nextSceneButton.gameObject.SetActive(true);
            nextSceneButton.onClick.AddListener(GoToNextScene);
        }
    }

    private void GoToNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.LogWarning("⚠️ 이동할 씬 이름이 설정되지 않았습니다.");
    }
}
