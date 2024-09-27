#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;

public class JsonToAssetEditor : EditorWindow
{
    public string fileName = "MapMaker1";
    public string assetFilePath = "Assets/Stage/MapMaker1.asset";  // 에셋 파일 경로
    public string jsonFileName = "StageData.json";  // JSON 파일 이름 (런타임에서 저장된 파일)

    [MenuItem("Tools/Apply JSON to ScriptableObject Asset")]
    public static void ShowWindow()
    {
        GetWindow<JsonToAssetEditor>("JSON to Asset");
    }

    private void OnGUI()
    {
        GUILayout.Label("JSON to ScriptableObject Asset", EditorStyles.boldLabel);

        fileName = EditorGUILayout.TextField("fileName", fileName);
        //assetFilePath = EditorGUILayout.TextField("Asset File Path", assetFilePath);
        jsonFileName = EditorGUILayout.TextField("JSON File Name", jsonFileName);

        if (GUILayout.Button("Apply JSON to Asset"))
        {
            ApplyJsonToAsset();
        }
    }

    // JSON 파일을 읽어와 ScriptableObject에 덮어쓰고, 에셋 파일에 저장
    private void ApplyJsonToAsset()
    {
        // 파일 경로 설정 (persistentDataPath는 런타임에서 사용한 경로)
        string jsonFilePath = Path.Combine(Application.persistentDataPath, jsonFileName);

        // 파일이 존재하는지 확인
        if (File.Exists(jsonFilePath))
        {
            // JSON 데이터를 읽어옴
            string jsonData = File.ReadAllText(jsonFilePath);


            assetFilePath = $"Assets/Stage/{fileName}.asset";
            // 에셋 파일에서 ScriptableObject를 불러옴
            StageData stageData = AssetDatabase.LoadAssetAtPath<StageData>(assetFilePath);

            if (stageData != null)
            {
                // JSON 데이터를 ScriptableObject에 덮어씌움
                JsonConvert.PopulateObject(jsonData, stageData);

                // 에셋 파일에 변경된 ScriptableObject 저장
                EditorUtility.SetDirty(stageData);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                Debug.Log("ScriptableObject updated from JSON and saved to asset file: " + assetFilePath);
            }
            else
            {
                Debug.LogError("Failed to load asset at: " + assetFilePath);
            }
        }
        else
        {
            Debug.LogError("JSON file not found at: " + jsonFilePath);
        }
    }
}
#endif
