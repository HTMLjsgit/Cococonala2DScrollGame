using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;

public class StageController : MonoBehaviour
{
    public static StageController instance;

    // プレイヤーの初期位置
    public Transform playerStartPosition;

    [SerializeField]
    private GameObject GoalCanvasObject;

    [SerializeField]
    private TextMeshProUGUI goalHaveCoinTextUI;
    [SerializeField]
    private TextMeshProUGUI StageCountTextUI;
    [SerializeField]
    private Button BackToStageSelectorButtonUI;
    private PlayerStatus playerStatus;
    private PlayerMoveScript playerMoveScript;
    private GameControllerScript gameControllerScript;
    [SerializeField]
    private AudioSource GoalAudioSource;

    void Awake()
    {
        // シーンにインスタンスが存在しない場合、このインスタンスを登録
        if (instance == null)
        {
            instance = this;
        }
        // すでにインスタンスが存在する場合は、このオブジェクトを破棄
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        gameControllerScript = GameControllerScript.instance;
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        playerMoveScript = playerStatus.gameObject.GetComponent<PlayerMoveScript>();
        
    }

    public void Goal()
    {
        // --- 既存のゴール処理 ---
        GoalCanvasObject.gameObject.SetActive(true);
        goalHaveCoinTextUI.SetText($"{playerStatus.HaveCoins}");
        gameControllerScript.MarkStageAsCleared(SceneManager.GetActiveScene().name);
        playerMoveScript.PermitMove = false;
        GoalAudioSource.Play();
        Time.timeScale = 0;

        // 1. 現在のステージ番号を取得
        string currentSceneName = SceneManager.GetActiveScene().name;
        string numberPart = currentSceneName.Replace("Stage", "");
        int.TryParse(numberPart, out int currentStageNumber);
        
        // 現在のステージ数をUIに書き込む
        StageCountTextUI.SetText(currentStageNumber.ToString());

        // 2. 最終ステージの番号を取得
        // TotalStageが空でないことを確認してからLast()を呼ぶ
        int finalStageNumber = 0;
        if (gameControllerScript.TotalStage != null && gameControllerScript.TotalStage.Count > 0)
        {
            finalStageNumber = gameControllerScript.TotalStage.Last();
        }

        // 3. 現在が最終ステージかどうかを判定
        if (currentStageNumber > 0 && currentStageNumber >= finalStageNumber)
        {
            // --- ゲームクリアの場合 ---
            Debug.Log("最終ステージクリア！ゲームクリア画面へ");
            // ボタンのイベントをクリアし、GameClearシーンをロードするイベントを登録
            BackToStageSelectorButtonUI.onClick.RemoveAllListeners();
            BackToStageSelectorButtonUI.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                SceneLoadManager.instance.LoadScene("GameClear");
            });
        }
        else
        {
            // --- 通常のステージクリアの場合 ---
            Debug.Log("ステージクリア！ステージ選択画面へ");
            // ボタンのイベントをクリアし、StageSelectorシーンをロードするイベントを登録
            BackToStageSelectorButtonUI.onClick.RemoveAllListeners();
            BackToStageSelectorButtonUI.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                SceneLoadManager.instance.LoadScene("StageSelector");
            });
        }
    }
}