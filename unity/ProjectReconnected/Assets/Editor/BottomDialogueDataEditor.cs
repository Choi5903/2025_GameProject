using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BottomDialogueData))]
public class BottomDialogueDataEditor : Editor
{
    SerializedProperty dialogueLines;

    private void OnEnable()
    {
        dialogueLines = serializedObject.FindProperty("dialogueLines");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("💬 Bottom Dialogue Lines", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        for (int i = 0; i < dialogueLines.arraySize; i++)
        {
            SerializedProperty line = dialogueLines.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"▶ 대사 {i + 1}", EditorStyles.miniBoldLabel);

            SerializedProperty speaker = line.FindPropertyRelative("speakerName");
            SerializedProperty sentence = line.FindPropertyRelative("sentence");
            SerializedProperty bgImage = line.FindPropertyRelative("backgroundImage");

            EditorGUILayout.PropertyField(speaker, new GUIContent("화자 이름"));
            EditorGUILayout.PropertyField(sentence, new GUIContent("대사 내용"));
            EditorGUILayout.PropertyField(bgImage, new GUIContent("배경 이미지 (선택)"));

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("↑", GUILayout.Width(30)) && i > 0)
                dialogueLines.MoveArrayElement(i, i - 1);
            if (GUILayout.Button("↓", GUILayout.Width(30)) && i < dialogueLines.arraySize - 1)
                dialogueLines.MoveArrayElement(i, i + 1);
            if (GUILayout.Button("삭제"))
                dialogueLines.DeleteArrayElementAtIndex(i);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(5);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("+ 대사 추가"))
        {
            dialogueLines.InsertArrayElementAtIndex(dialogueLines.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
