// ファイル名: PrefabParentEditorWindow.cs
// 配置場所: Assets/Editor/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PrefabParentEditorWindow : EditorWindow
{
    private PrefabParentSettings settings;
    private SerializedObject serializedSettings;
    private Vector2 scrollPosition;

    [MenuItem("Tools/Prefab Parent Settings")]
    public static void ShowWindow()
    {
        GetWindow<PrefabParentEditorWindow>("Prefab Parent Settings");
    }

    private void OnEnable()
    {
        string[] guids = AssetDatabase.FindAssets("t:PrefabParentSettings");
        if (guids.Length > 0)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            settings = AssetDatabase.LoadAssetAtPath<PrefabParentSettings>(path);
            serializedSettings = new SerializedObject(settings);
        }
        else
        {
            Debug.LogError("PrefabParentSettings.asset not found. Please create it via Assets > Create > Editor > Prefab Parent Settings.");
        }
    }

    private void OnGUI()
    {
        if (settings == null || serializedSettings == null)
        {
            EditorGUILayout.HelpBox("Settings asset could not be loaded.", MessageType.Error);
            return;
        }

        serializedSettings.Update();

        // グローバル設定のチェックボックス
        EditorGUILayout.PropertyField(serializedSettings.FindProperty("isGloballyEnabled"), new GUIContent("Enable Auto-Parenting Feature"));
        EditorGUILayout.Space();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // ★変更: シンプルなリスト表示に戻す
        EditorGUILayout.PropertyField(serializedSettings.FindProperty("prefabMappings"), true);
        
        EditorGUILayout.EndScrollView();
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Batch Operation", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Organize All Prefabs in Scene"))
        {
            OrganizeAllPrefabsInScene();
        }

        serializedSettings.ApplyModifiedProperties();
    }

    private void OrganizeAllPrefabsInScene()
    {
        // グローバル設定が無効な場合はダイアログを表示して中断
        if (settings == null || !settings.isGloballyEnabled || settings.prefabMappings.Count == 0)
        {
            EditorUtility.DisplayDialog("Info", "Auto-Parenting is disabled or no settings found.", "OK");
            return;
        }

        int organizedCount = 0;
        
        var prefabToParentMap = new Dictionary<GameObject, string>();
        foreach (var mapping in settings.prefabMappings)
        {
            // ★変更: 個別の有効チェックを削除
            if (string.IsNullOrEmpty(mapping.parentName)) continue;

            foreach (var prefab in mapping.prefabs)
            {
                if (prefab != null && !prefabToParentMap.ContainsKey(prefab))
                {
                    prefabToParentMap.Add(prefab, mapping.parentName);
                }
            }
        }

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.parent != null) continue;

            GameObject sourcePrefab = PrefabUtility.GetCorrespondingObjectFromSource(obj);
            if (sourcePrefab == null) continue;

            if (prefabToParentMap.TryGetValue(sourcePrefab, out string parentName))
            {
                GameObject parentObject = GameObject.Find(parentName);
                if (parentObject == null)
                {
                    parentObject = new GameObject(parentName);
                    Undo.RegisterCreatedObjectUndo(parentObject, "Create Parent Object");
                }
                
                Undo.SetTransformParent(obj.transform, parentObject.transform, "Organize Prefab");
                organizedCount++;
            }
        }
        
        EditorUtility.DisplayDialog("Organize Complete", $"Organized {organizedCount} prefab instance(s).", "OK");
    }
}