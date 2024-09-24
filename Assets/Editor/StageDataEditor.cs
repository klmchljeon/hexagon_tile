using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageData))]
public class StageDataEditor : Editor
{
    private SerializedProperty stageNum;
    private SerializedProperty tileNum;
    private SerializedProperty tileRotate;
    private SerializedProperty cantRotate;

    private SerializedProperty player;
    private int width = 6;  // 2차원 배열의 가로 크기
    private int height = 6; // 2차원 배열의 세로 크기

    private void OnEnable()
    {
        stageNum = serializedObject.FindProperty("stageNumber");

        // 1차원 배열로 변환된 tiles 배열에 접근
        tileNum = serializedObject.FindProperty("tileNumbers");
        tileRotate = serializedObject.FindProperty("tileRotated");
        cantRotate = serializedObject.FindProperty("cantRotate");

        player = serializedObject.FindProperty("playerPosition");
    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update(); // 데이터 업데이트

        base.OnInspectorGUI();

        //EditorGUILayout.BeginHorizontal();
        //stageNum.intValue = EditorGUILayout.IntField(stageNum.intValue, GUILayout.Width(40));
        //EditorGUILayout.EndHorizontal();

        // 6x6 타일 편집 UI
        for (int y = height-1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            if (y % 2 == 1)
            {
                GUILayout.Space(40);
            }
            for (int x = 0; x < width; x++)
            {


                // 2차원 배열의 인덱스를 1차원 배열로 변환
                int index = x + (y * width);

                // 1차원 배열의 요소에 접근
                SerializedProperty Num = tileNum.GetArrayElementAtIndex(index);
                SerializedProperty Rotate = tileRotate.GetArrayElementAtIndex(index);
                SerializedProperty CantRotate = cantRotate.GetArrayElementAtIndex(index);

                // 인스펙터에 타일 번호와 회전 여부를 표시하고 수정 가능하게 함
                Num.intValue = EditorGUILayout.IntField(Num.intValue, GUILayout.Width(40));
                Rotate.boolValue = EditorGUILayout.Toggle(Rotate.boolValue, GUILayout.Width(15));
                CantRotate.boolValue = EditorGUILayout.Toggle(CantRotate.boolValue, GUILayout.Width(25));
            }
            EditorGUILayout.EndHorizontal();
        }

        // 변경 사항 적용
        serializedObject.ApplyModifiedProperties();
    }
}
