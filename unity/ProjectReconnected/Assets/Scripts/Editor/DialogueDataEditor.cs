using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueData))]
public class DialogueDataEditor : Editor
{
    SerializedProperty dialogueLines;

    private void OnEnable()
    {
        dialogueLines = serializedObject.FindProperty("dialogueLines");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Dialogue Lines", EditorStyles.boldLabel);

        for (int i = 0; i < dialogueLines.arraySize; i++)
        {
            SerializedProperty line = dialogueLines.GetArrayElementAtIndex(i);
            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.LabelField($"Line {i + 1}", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(line.FindPropertyRelative("speakerName"), new GUIContent("이름"));
            EditorGUILayout.PropertyField(line.FindPropertyRelative("sentence"), new GUIContent("대사"));

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("이미지 설정", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(line.FindPropertyRelative("characterCGLeft"), new GUIContent("캐릭터 CG 왼쪽"));
            EditorGUILayout.PropertyField(line.FindPropertyRelative("characterCGRight"), new GUIContent("캐릭터 CG 오른쪽"));
            EditorGUILayout.PropertyField(line.FindPropertyRelative("extraImage"), new GUIContent("추가 이미지 (이펙트 등)"));

            EditorGUILayout.Space(5);
            if (GUILayout.Button("🗑 Remove Line"))
            {
                dialogueLines.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("➕ Add New Line"))
        {
            dialogueLines.InsertArrayElementAtIndex(dialogueLines.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
