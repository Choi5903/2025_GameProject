using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    [Header("버튼 연결")]
    public Button startButton;
    public Button loadButton;
    public Button settingsButton;

    [Header("씬 이름")]
    public string nextSceneName = "VNScene_Prologue"; // 다음 씬 이름을 설정하세요

    void Start()
    {
        // 버튼 리스너 연결
        startButton.onClick.AddListener(OnStartClicked);
        loadButton.onClick.AddListener(OnLoadClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    void OnStartClicked()
    {
        Debug.Log("게임 시작 버튼 클릭됨");
        SceneManager.LoadScene(nextSceneName);
    }

    void OnLoadClicked()
    {
        Debug.Log("로드 버튼 클릭됨 - 기능 미구현");
        // TODO: 추후 로드 기능 구현
    }

    void OnSettingsClicked()
    {
        Debug.Log("설정 버튼 클릭됨 - 기능 미구현");
        // TODO: 추후 설정 메뉴 오픈
    }
}
