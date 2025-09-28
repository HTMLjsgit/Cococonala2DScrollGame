using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class StageController : MonoBehaviour
{
    // public staticでインスタンスを保持する変数
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
    [SerializeField]
    private TextMeshProUGUI KillEnemiesCountTextUI;
    private GameControllerScript gameControllerScript;
    private PlayerStatus playerStatus;
    private PlayerMoveScript playerMoveScript;
    [SerializeField]
    private AudioSource GoalAudioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
        playerMoveScript = playerStatus.gameObject.GetComponent<PlayerMoveScript>();
        gameControllerScript = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
        BackToStageSelectorButtonUI.onClick.AddListener(() =>
        {
            //ゴールした後次のステージに進むボタンをクリックしたら　次のステージに進む
            DOVirtual.DelayedCall(1, () =>
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("StageSelector");
            });
        });
        GameObject Player = GameObject.FindWithTag("Player");
    }
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
    
    // Update is called once per frame
    void Update()
    {

    }
    public void Goal()
    {
        GoalCanvasObject.gameObject.SetActive(true);
        goalHaveCoinTextUI.SetText($"{playerStatus.HaveCoins}");
        StageCountTextUI.SetText($"{gameControllerScript.StageCountHistory[0]}");
        playerMoveScript.PermitMove = false;
        GoalAudioSource.Play();
        KillEnemiesCountTextUI.SetText(playerStatus.KillEnemyCount.ToString());
        Time.timeScale = 0;
    }
}
