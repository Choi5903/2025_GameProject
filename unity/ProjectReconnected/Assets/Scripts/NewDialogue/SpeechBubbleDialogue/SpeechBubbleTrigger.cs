using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleTrigger : MonoBehaviour
{
    public SpeechBubbleData speechData;
    public Transform targetCharacter;
    public GameObject eventReceiver;

    public void TriggerSpeechBubble()
    {
        SpeechBubbleManager.Instance.StartDialogue(speechData, targetCharacter, OnDialogueEnd);
    }

    private void OnDialogueEnd()
    {
        if (eventReceiver != null)
        {
            var handler = eventReceiver.GetComponent<IDialogueEventHandler>();
            if (handler != null)
                handler.OnDialogueEvent();
        }
    }
}
