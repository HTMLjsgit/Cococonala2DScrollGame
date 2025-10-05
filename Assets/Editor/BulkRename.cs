using UnityEngine;
using UnityEditor;

public class BulkRename : EditorWindow
{
    private string nameToFind = "MediumAmountCoin"; // 初期値を設定
    private string newName = "HimawariSeed";       // 初期値を設定

    // "Tools"メニューに "Bulk Rename" という項目を追加
    [MenuItem("Tools/Bulk Rename")]
    public static void ShowWindow()
    {
        // 既存のウィンドウがあればそれを表示し、なければ新しく作成
        GetWindow<BulkRename>("Bulk Rename");
    }

    // エディタウィンドウのUIを描画
    void OnGUI()
    {
        GUILayout.Label("Rename GameObjects in Scene", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();

        // ユーザーが自由に入力できるテキストフィールドを追加
        nameToFind = EditorGUILayout.TextField("Name to find (contains)", nameToFind);
        newName = EditorGUILayout.TextField("New name", newName);
        
        EditorGUILayout.Space();

        if (GUILayout.Button("Rename Objects"))
        {
            RenameObjects();
        }
    }

    private void RenameObjects()
    {
        // 入力が空の場合は処理をしない
        if (string.IsNullOrEmpty(nameToFind))
        {
            Debug.LogError("'Name to find' cannot be empty.");
            return;
        }

        int renamedCount = 0;
        // シーン内のすべてのGameObjectを取得
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // オブジェクト名に指定した文字列が含まれているかチェック
            if (obj.name.Contains(nameToFind))
            {
                // Undo（元に戻す）操作を登録
                Undo.RecordObject(obj, "Rename GameObject");
                // 指定された新しい名前に変更
                obj.name = newName;
                renamedCount++;
            }
        }
        
        // 結果をログに表示
        if (renamedCount > 0)
        {
            Debug.Log($"✅ Renamed {renamedCount} objects containing '{nameToFind}' to '{newName}'.");
        }
        else
        {
            Debug.Log($"ℹ️ No objects found with the name containing '{nameToFind}'.");
        }
    }
}