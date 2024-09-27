using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SaveScriptableObjectAsJson : MonoBehaviour
{
    [SerializeField]
    string jsonFileName = "StageData.json";  // JSON으로 저장할 파일 이름

    // ScriptableObject를 JSON으로 직렬화하고 파일로 저장
    public void SaveToJsonFile(StageData stageData)
    {
        string jsonData = JsonConvert.SerializeObject(stageData, Formatting.Indented);

        // 파일 경로 설정 (persistentDataPath는 런타임에서 사용 가능)
        string filePath = Path.Combine(Application.persistentDataPath, jsonFileName);

        // JSON 데이터를 파일로 저장
        File.WriteAllText(filePath, jsonData);
        Debug.Log("ScriptableObject saved as JSON to: " + filePath);
    }
}
