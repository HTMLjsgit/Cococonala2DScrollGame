using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameControllerScript : MonoBehaviour
{
    private GameObject Player;
    public List<string> HistroySceneName;
    public List<int> StageCountHistory;
    public List<int> TotalStage;
    public int BeforeSceneGotCoinCount;
    public string NowSceneName;
    [System.Serializable]
    public class StageClearStatus
    {
        public string stageName;
        public bool isCleared;
    }
    public List<StageClearStatus> stageClearHistory = new List<StageClearStatus>();

    [SerializeField] private string BeforeSceneName;

    [SerializeField] private Vector2 PlayerPosition;

    [SerializeField] private float PlayerHP;
    [SerializeField] private float PlayerMaxHP;
    PlayerStatus player_status;

   public static GameControllerScript instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnUnLoaded;
        Player = GameObject.FindWithTag("Player");
        if (Player != null)
        {
            player_status = Player.GetComponent<PlayerStatus>();
        }

        NowSceneName = SceneManager.GetActiveScene().name;
        if (NowSceneName.Contains("Stage"))
        {
            string stageCount = NowSceneName.Replace("Stage", "");
            StageCountHistory.Add(int.Parse(stageCount));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            PlayerPosition = Player.transform.position;
            PlayerHP = player_status.HP;
            PlayerMaxHP = player_status.MaxHP;
            BeforeSceneGotCoinCount = player_status.HaveCoins;
        }  
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        NowSceneName = SceneManager.GetActiveScene().name;
        string stageCountStr = NowSceneName.Replace("Stage", "");
        // int.TryParseで、stageCountStrを整数に変換できるか試す
        if (int.TryParse(stageCountStr, out int stageCount))
        {
            // 変換に成功した場合のみ、リストに追加する
            StageCountHistory.Add(stageCount);
        }
    }
    public void MarkStageAsCleared(string name)
    {
        // リストの中から該当するステージ名を探す
        var stageStatus = stageClearHistory.FirstOrDefault(s => s.stageName == name);

        if (stageStatus != null)
        {
            // すでにリストに存在する場合
            stageStatus.isCleared = true;
            Debug.Log($"ステージ '{name}' の状態をクリア済に更新しました。");
        }
        else
        {
            // リストにない場合は新しく追加する
            stageClearHistory.Add(new StageClearStatus { stageName = name, isCleared = true });
            Debug.Log($"ステージ '{name}' をクリア済みとして新しく記録しました。");
        }
    }
    /// <summary>
    /// 指定されたステージがクリア済みかどうかを確認します。
    /// </summary>
    /// <param name="name">確認したいステージのシーン名</param>
    /// <returns>クリア済みならtrue、そうでなければfalse</returns>
    
    public bool IsStageCleared(string name)
    {
        var stageStatus = stageClearHistory.FirstOrDefault(s => s.stageName == name);
        if (stageStatus != null)
        {
            return stageStatus.isCleared;
        }
        // 記録がない場合は未クリアとみなす
        return false;
    }

    public void UpdateTotalStages(List<int> stageList)
    {
        TotalStage = new List<int>(stageList); // リストをコピーして設定
        TotalStage.Sort(); // 念のためソート
        Debug.Log($"TotalStageリストが更新されました。合計ステージ数: {TotalStage.Count}");
    }
    void OnUnLoaded(Scene scene)
    {
        BeforeSceneName = scene.name;
        HistroySceneName.Add(scene.name);
        if (scene.name.Contains("Stage"))
        {
            GameObject UnLoadPlayer = GameObject.FindWithTag("Player");
            if (UnLoadPlayer != null)
            {
                PlayerStatus playerStatus = UnLoadPlayer.GetComponent<PlayerStatus>();
            }
        }
    }
}
