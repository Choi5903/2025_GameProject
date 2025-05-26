using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SynopsisSceneManager : MonoBehaviour
{
    [Header("UI 연결")]
    public GameObject fullscreenBackgroundImage;
    public GameObject mainUI;
    public Button chatRoomButton;           // ✅ 메신저 앱 내 채팅방 진입 버튼
    public GameObject chatPanel;            // 채팅창 패널
    public List<GameObject> chatObjects;    // Chat_1 ~ Chat_n 오브젝트
    public GameObject sceneChangeButton;    // 씬 전환 버튼

    [Header("대사 데이터")]
    public BottomDialogueData openingDialogue;
    public BottomDialogueData postChatDialogue;

    private int chatIndex = 0;
    private bool isChatActive = false;

    private void Start()
    {
        InitializeScene();
    }

    private void Update()
    {
        if (isChatActive && Input.GetKeyDown(KeyCode.F))
        {
            AdvanceChat();
        }
    }

    void InitializeScene()
    {
        fullscreenBackgroundImage.SetActive(true);
        mainUI.SetActive(false);
        chatPanel.SetActive(false);
        sceneChangeButton.SetActive(false);

        DialogueManagerV2.Instance.StartDialogue(openingDialogue, OnOpeningDialogueEnd);
    }

    void OnOpeningDialogueEnd()
    {
        fullscreenBackgroundImage.SetActive(false);
        mainUI.SetActive(true);

        // ✅ 대사 종료 후 채팅방 버튼 활성화
        if (chatRoomButton != null)
        {
            chatRoomButton.gameObject.SetActive(true);
            chatRoomButton.interactable = true;
        }
    }

    public void OnChatRoomClicked()
    {
        // mainUI는 그대로 유지 → 채팅창이 위에 떠야 하므로 꺼지지 않게 함
        chatPanel.SetActive(true);
        chatIndex = 0;
        isChatActive = true;
        UpdateChatDisplay();
    }

    void AdvanceChat()
    {
        chatIndex++;
        if (chatIndex >= chatObjects.Count)
        {
            chatPanel.SetActive(false);
            isChatActive = false;
            DialogueManagerV2.Instance.StartDialogue(postChatDialogue, OnPostChatDialogueEnd);
        }
        else
        {
            UpdateChatDisplay();
        }
    }

    void UpdateChatDisplay()
    {
        for (int i = 0; i < chatObjects.Count; i++)
        {
            chatObjects[i].SetActive(i <= chatIndex);
        }
    }

    void OnPostChatDialogueEnd()
    {
        sceneChangeButton.SetActive(true);
    }

    public void OnSceneChangeClicked()
    {
        SceneManager.LoadScene("Demo_03_C1_Puzzle"); // 전환할 씬 이름 여기에 설정
    }
}
