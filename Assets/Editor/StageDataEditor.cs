using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageData))]
public class StageDataEditor : Editor
{
    private SerializedProperty tileNum;
    private SerializedProperty tileRotate;
    private int width = 6;  // 2���� �迭�� ���� ũ��
    private int height = 6; // 2���� �迭�� ���� ũ��

    private void OnEnable()
    {
        // 1���� �迭�� ��ȯ�� tiles �迭�� ����
        tileNum = serializedObject.FindProperty("tileNumbers");
        tileRotate = serializedObject.FindProperty("tileRotated");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // ������ ������Ʈ

        // 6x6 Ÿ�� ���� UI
        for (int x = 0; x < width; x++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int y = 0; y < height; y++)
            {
                // 2���� �迭�� �ε����� 1���� �迭�� ��ȯ
                int index = x + (y * width);

                // 1���� �迭�� ��ҿ� ����
                SerializedProperty Num = tileNum.GetArrayElementAtIndex(index);
                SerializedProperty Rotate = tileRotate.GetArrayElementAtIndex(index);

                // �ν����Ϳ� Ÿ�� ��ȣ�� ȸ�� ���θ� ǥ���ϰ� ���� �����ϰ� ��
                Num.intValue = EditorGUILayout.IntField(Num.intValue, GUILayout.Width(40));
                Rotate.boolValue = EditorGUILayout.Toggle(Rotate.boolValue, GUILayout.Width(40));
            }
            EditorGUILayout.EndHorizontal();
        }

        // ���� ���� ����
        serializedObject.ApplyModifiedProperties();
    }
}
