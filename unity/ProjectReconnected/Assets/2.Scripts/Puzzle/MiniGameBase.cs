using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    protected MiniGameManager miniGameManager;

    [Header("Ŭ���� �� �ڵ� ���� ������Ʈ")]
    public GameObject autoTriggerTarget;

    protected virtual void Awake()
    {
        miniGameManager = FindObjectOfType<MiniGameManager>();
    }
    protected virtual void OnEnable()
    {
        ResetGame(); // Ȱ��ȭ�� �� �ڵ� �ʱ�ȭ
    }

    protected void NotifyClear()
    {
        if (miniGameManager != null)
            miniGameManager.OnMiniGameClear(this);
    }

    public abstract void ResetGame();
}