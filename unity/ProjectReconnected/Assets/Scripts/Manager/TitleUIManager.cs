using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIManager : MonoBehaviour
{
    [Header("��ư ����")]
    public Button startButton;
    public Button loadButton;
    public Button settingsButton;

    [Header("�� �̸�")]
    public string nextSceneName = "VNScene_Prologue"; // ���� �� �̸��� �����ϼ���

    void Start()
    {
        // ��ư ������ ����
        startButton.onClick.AddListener(OnStartClicked);
        loadButton.onClick.AddListener(OnLoadClicked);
        settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    void OnStartClicked()
    {
        Debug.Log("���� ���� ��ư Ŭ����");
        SceneManager.LoadScene(nextSceneName);
    }

    void OnLoadClicked()
    {
        Debug.Log("�ε� ��ư Ŭ���� - ��� �̱���");
        // TODO: ���� �ε� ��� ����
    }

    void OnSettingsClicked()
    {
        Debug.Log("���� ��ư Ŭ���� - ��� �̱���");
        // TODO: ���� ���� �޴� ����
    }
}
