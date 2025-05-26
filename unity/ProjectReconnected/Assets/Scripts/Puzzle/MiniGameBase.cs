using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MiniGameBase : MonoBehaviour
{
    protected MiniGameManager miniGameManager;

    [Header("클리어 후 자동 실행 오브젝트")]
    public GameObject autoTriggerTarget;

    protected virtual void Awake()
    {
        miniGameManager = FindObjectOfType<MiniGameManager>();
    }
    protected virtual void OnEnable()
    {
        ResetGame(); // 활성화될 때 자동 초기화
    }

    protected void NotifyClear()
    {
        if (miniGameManager != null)
            miniGameManager.OnMiniGameClear(this);
    }

    public abstract void ResetGame();
}