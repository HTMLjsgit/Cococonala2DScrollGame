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
            // settingsオブジェクトをシリアライズ化し、UndoやリッチなUI表示に対応
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

        // SerializedObjectの変更を監視
        serializedSettings.Update();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        // "prefabMappings" という名前のプロパティを取得し、リスト全体をUIに描画
        // 'true' を指定することで、リストの子要素（parentNameやprefabsリスト）も再帰的に描画される
        EditorGUILayout.PropertyField(serializedSettings.FindProperty("prefabMappings"), true);
        
        EditorGUILayout.EndScrollView();
        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Batch Operation", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Organize All Prefabs in Scene"))
        {
            OrganizeAllPrefabsInScene();
        }

        // GUIで行われた変更を実際のオブジェクトに適用
        serializedSettings.ApplyModifiedProperties();
    }

    private void OrganizeAllPrefabsInScene()
    {
        if (settings == null || settings.prefabMappings.Count == 0)
        {
            EditorUtility.DisplayDialog("Info", "No settings found.", "OK");
            return;
        }

        int organizedCount = 0;
        
        // 設定を検索しやすいように、Prefabをキー、親の名前をバリューにしたDictionaryを作成
        var prefabToParentMap = new Dictionary<GameObject, string>();
        foreach (var mapping in settings.prefabMappings)
        {
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
                if (string.IsNullOrEmpty(parentName)) continue; // 親の名前が空ならスキップ

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