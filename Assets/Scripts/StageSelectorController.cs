using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

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
    [Tooltip("ステージの順番")]
    public int stageNumber = 0;
}

public class StageSelectorController : MonoBehaviour
{
    [Header("ステージ選択の設定")]
    [SerializeField]
    [Tooltip("UIボタンと読み込むシーン名のペアをここに登録します。")]
    private List<StageButtonMapping> _stageButtonMappings = new List<StageButtonMapping>();
    private GameControllerScript gameControllerScript;
    private void Start()
    {
        // リストに登録された全てのボタンにイベントを登録する
        foreach (var mapping in _stageButtonMappings)
        {
            string sceneToLoad = mapping.sceneName;
            mapping.button.onClick.AddListener(() => LoadScene(sceneToLoad));
        }
        gameControllerScript = GameControllerScript.instance;
        // まずは全てのボタンを非インタラクティブにする
        foreach (var mapping in _stageButtonMappings)
        {
            mapping.button.interactable = false;
        }
        List<int> stageNumbers = _stageButtonMappings.Select(mapping => mapping.stageNumber).ToList();
        
        // 2. GameControllerのTotalStageリストを更新する
        gameControllerScript.UpdateTotalStages(stageNumbers);
        // 登録されているボタンをステージ番号順にソートする（Inspectorでの順番に依存しないように）
        _stageButtonMappings.Sort((a, b) => a.stageNumber.CompareTo(b.stageNumber));

        // ステージをチェックし、条件に応じてボタンを有効化する
        int highestClearedStage = 0;
        foreach (var clearedStatus in gameControllerScript.stageClearHistory)
        {
            if (clearedStatus.isCleared)
            {
                // クリア済みのステージマッピングを探す
                var mapping = _stageButtonMappings.Find(x => x.sceneName == clearedStatus.stageName);
                if (mapping != null)
                {
                    // クリア済みのステージ番号のうち、最も大きいものを記録
                    if (mapping.stageNumber > highestClearedStage)
                    {
                        highestClearedStage = mapping.stageNumber;
                    }
                }
            }
        }
        // クリア済みステージと、その次のステージを解放する
        foreach (var mapping in _stageButtonMappings)
        {
            // ステージ1、またはクリア済みのステージ、または最高クリアステージの次のステージならボタンを有効化
            if (mapping.stageNumber <= highestClearedStage + 1)
            {
                mapping.button.interactable = true;
            }

            // クリックイベントの登録 (重複登録を避けるため一度削除してから登録)
            string sceneToLoad = mapping.sceneName;
            mapping.button.onClick.RemoveAllListeners();
            mapping.button.onClick.AddListener(() => LoadScene(sceneToLoad));
        }
        
    }

    /// <summary>
    /// 指定された名前のシーンを読み込みます。
    /// </summary>
    /// <param name="name">読み込むシーンの名前</param>
    private void LoadScene(string name)
    {
        SceneLoadManager.instance.LoadScene(name);
    }
}