
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
            EditorGUILayout.PropertyField(line.FindPropertyRelative("speakerName"));
            EditorGUILayout.PropertyField(line.FindPropertyRelative("sentence"));
            EditorGUILayout.PropertyField(line.FindPropertyRelative("portrait"));
            if (GUILayout.Button("Remove Line"))
            {
                dialogueLines.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add New Line"))
        {
            dialogueLines.InsertArrayElementAtIndex(dialogueLines.arraySize);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
