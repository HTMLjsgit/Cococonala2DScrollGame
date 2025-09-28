using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Unityのインスペクターに表示するための、Buttonとシーン名をペアで管理するクラス。
/// [System.Serializable] をつけるのがポイント。
/// </summary>
[System.Serializable]
public class StageButtonMapping
{
    public Button button;
    [Tooltip("読み込むシーンの名前を正確に入力してください")]
    public string sceneName;
}

public class StageSelectorController : MonoBehaviour
{
    [Header("ステージ選択の設定")]
    [SerializeField]
    [Tooltip("UIボタンと読み込むシーン名のペアをここに登録します。")]
    private List<StageButtonMapping> _stageButtonMappings = new List<StageButtonMapping>();

    private void Start()
    {
        // リストに登録された全てのボタンにイベントを登録する
        foreach (var mapping in _stageButtonMappings)
        {
            // ボタンやシーン名が未設定の場合はスキップ
            if (mapping.button == null || string.IsNullOrEmpty(mapping.sceneName))
            {
                Debug.LogWarning("Button or Scene Name is not set in a mapping.");
                continue;
            }

            // --- 重要 ---
            // foreachループ内でAddListenerのラムダ式を使うためのクロージャ問題対策
            string sceneToLoad = mapping.sceneName;
            
            mapping.button.onClick.AddListener(() => LoadScene(sceneToLoad));
        }
    }

    /// <summary>
    /// 指定された名前のシーンを読み込みます。
    /// </summary>
    /// <param name="name">読み込むシーンの名前</param>
    private void LoadScene(string name)
    {
        Debug.Log($"Loading scene: {name}...");
        SceneManager.LoadScene(name);
    }
}