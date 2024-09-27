using UnityEngine;
using UnityEditor;
using System.IO;

public class ScriptableObjectUtility
{
    // 지정된 경로에 스크립터블 오브젝트를 저장하는 함수
    public static T CreateOrLoadScriptableObject<T>(string path) where T : ScriptableObject
    {
        // 경로가 없으면 해당 폴더 생성
        string folderPath = Path.GetDirectoryName(path);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // 지정된 경로에 이미 있는지 확인
        T obj = AssetDatabase.LoadAssetAtPath<T>(path);

        // 없으면 새로 생성
        if (obj == null)
        {
            obj = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(obj, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"새로운 ScriptableObject를 생성하여 {path}에 저장했습니다.");
        }
        else
        {
            Debug.Log($"이미 존재하는 ScriptableObject를 불러왔습니다: {path}");
        }

        return obj;
    }
}
