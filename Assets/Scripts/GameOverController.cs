using UnityEngine;
using UnityEngine.UI; // Buttonを扱うために必要
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームオーバー画面を管理するコントローラー (AddListenerを使用)
/// </summary>
public class GameOverController : MonoBehaviour
{
    // インスペクターから設定するボタン
    public Button moveToStageSelectorButton;

    void Start()
    {
        // MoveToStageSelectorメソッドをボタンのクリックイベントに登録
        moveToStageSelectorButton.onClick.AddListener(MoveToStageSelector);

    }

    /// <summary>
    /// StageSelectorシーンに移動する
    /// </summary>
    private void MoveToStageSelector()
    {
        // "StageSelector" という名前のシーンをロードする
        SceneManager.LoadScene("StageSelector");
    }

    // オブジェクトが破棄される際に、登録したイベントを解除（推奨）
    void OnDestroy()
    {
        moveToStageSelectorButton.onClick.RemoveListener(MoveToStageSelector);
    }
}