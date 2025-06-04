using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomEditor(typeof(MessengerDialogueData))]
public class MessengerDialogueDataEditor : Editor
{
    private ReorderableList reorderableList;

    private void OnEnable()
    {
        reorderableList = new ReorderableList(serializedObject,
            serializedObject.FindProperty("chatLines"),
            true, true, true, true);

        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Messenger Chat Lines");
        };

        reorderableList.elementHeightCallback = (index) =>
        {
            return EditorGUIUtility.singleLineHeight * 4.5f;
        };

        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            // Message
            Rect messageRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2);
            EditorGUI.PropertyField(messageRect, element.FindPropertyRelative("message"), GUIContent.none);

            // Position
            Rect posRect = new Rect(rect.x, rect.y + 40, rect.width * 0.5f - 5, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(posRect, element.FindPropertyRelative("position"), new GUIContent("위치"));

            // Size
            Rect sizeRect = new Rect(rect.x + rect.width * 0.5f + 5, rect.y + 40, rect.width * 0.5f - 5, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(sizeRect, element.FindPropertyRelative("sizeLevel"), new GUIContent("크기"));
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        reorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
