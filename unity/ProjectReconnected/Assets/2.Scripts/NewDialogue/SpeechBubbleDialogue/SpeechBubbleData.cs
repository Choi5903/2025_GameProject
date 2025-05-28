using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSpeechBubbleData", menuName = "Dialogue/Speech Bubble Dialogue Data")]
public class SpeechBubbleData : ScriptableObject
{
    public List<SpeechBubbleLine> bubbleLines;
}
