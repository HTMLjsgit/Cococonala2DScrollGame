// ファイル名: PrefabAutoParenting.cs
// 配置場所: Assets/Editor/

using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PrefabAutoParenting
{
    private static PrefabParentSettings settings;

    static PrefabAutoParenting()
    {
        LoadSettings();
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private static void LoadSettings()
    {
        string[] guids = AssetDatabase.FindAssets("t:PrefabParentSettings");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            settings = AssetDatabase.LoadAssetAtPath<PrefabParentSettings>(path);
        }
    }
    
    private static void OnHierarchyChanged()
    {
        if (settings == null || settings.prefabMappings.Count == 0) return;
        
        GameObject activeObject = Selection.activeGameObject;
        if(activeObject == null || activeObject.transform.parent != null) return;
        
        GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(activeObject);
        if (sourcePrefab == null) return;
        
        // 新しいデータ構造を検索
        foreach (var mapping in settings.prefabMappings)
        {
            // mapping.prefabs リストに sourcePrefab が含まれているかチェック
            if (mapping.prefabs.Contains(sourcePrefab))
            {
                string parentName = mapping.parentName;
                if (string.IsNullOrEmpty(parentName)) continue; // 親の名前が空ならスキップ

                GameObject parentObject = GameObject.Find(parentName);
                
                if (parentObject == null)
                {
                    parentObject = new GameObject(parentName);
                    Undo.RegisterCreatedObjectUndo(parentObject, "Create Parent Object");
                }
                
                Undo.SetTransformParent(activeObject.transform, parentObject.transform, "Auto Parent Prefab");
                return; // 処理が完了したらループを抜ける
            }
        }
    }
}