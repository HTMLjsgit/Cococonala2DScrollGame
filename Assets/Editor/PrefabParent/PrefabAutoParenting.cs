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
        EditorApplication.delayCall += () => {
            LoadSettings();
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        };
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
        // グローバル設定が無効、または設定ファイルがない場合は処理を中断
        if (settings == null || !settings.isGloballyEnabled || settings.prefabMappings.Count == 0) return;
        
        GameObject activeObject = Selection.activeGameObject;
        if(activeObject == null || activeObject.transform.parent != null) return;
        
        GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(activeObject);
        if (sourcePrefab == null) return;
        
        foreach (var mapping in settings.prefabMappings)
        {
            // ★変更: 個別の有効チェックを削除
            if (mapping.prefabs.Contains(sourcePrefab))
            {
                string parentName = mapping.parentName;
                if (string.IsNullOrEmpty(parentName)) continue;

                GameObject parentObject = GameObject.Find(parentName);
                
                if (parentObject == null)
                {
                    parentObject = new GameObject(parentName);
                    Undo.RegisterCreatedObjectUndo(parentObject, "Create Parent Object");
                }
                
                Undo.SetTransformParent(activeObject.transform, parentObject.transform, "Auto Parent Prefab");
                return;
            }
        }
    }
}