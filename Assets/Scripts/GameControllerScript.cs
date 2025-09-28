using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameControllerScript : MonoBehaviour
{
    GameObject Player;
    public List<string> HistroySceneName;
    public List<int> StageCountHistory;
    public int BeforeSceneGotCoinCount;
    public string NowSceneName;
    public string BeforeSceneName;

    public Vector2 PlayerPosition;

    public float PlayerHP;
    public float PlayerMaxHP;
    PlayerStatus player_status;
    // Start is called before the first frame update
    void Awake()
    {

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
        if (scene.name.Contains("Stage"))
        {
            string stageCount = scene.name.Replace("Stage", "");
            StageCountHistory.Add(int.Parse(stageCount));
        }
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
