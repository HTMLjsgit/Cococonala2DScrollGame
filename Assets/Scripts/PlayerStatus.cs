using System.Collections;
using System.Collections.Generic;
using DG.Tweening; // DOFadeを使用するために必要
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    public float MaxHP = 100;
    public float HP = 100;
    public int HaveCoins = 0;
    public float AttackPower;
    public bool Death;
    public AttackColliderScript[] AttackColliders;

    [SerializeField] private AudioSource DeathSoundSource;

    [Header("残機設定")]
    public int lives = 3;
    public TextMeshProUGUI livesTextUI;

    [Header("復活・無敵設定")]
    [Tooltip("復活後の無敵時間（秒）")]
    public float invincibilityDuration = 2f;
    private bool isInvincible = false; // 無敵状態かどうかを管理
    private SpriteRenderer spriteRenderer; // 点滅させるためのレンダラー

    [Header("イベント系")]
    public UnityEvent AttackedEvent;
    Animator anim;
    private PlayerMoveScript playerMoveScript;
    public Slider HPSlider;
    [SerializeField] private float displayedCoins;
    [SerializeField] private TextMeshProUGUI HaveCoinsTextUI;
    [Header("コインアニメーション設定")]
    [SerializeField] private float coinAnimationDuration = 0.5f;
    [SerializeField] private Ease coinAnimationEase = Ease.OutQuad;
    [SerializeField] private bool useCountUpEffect = true;

    [Header("コイン音響設定")]
    [SerializeField] private AudioSource coinAudioSource;
    [SerializeField] private AudioClip coinGetSE;
    [SerializeField] private bool playCountUpSound = true;
    [SerializeField] private float countSoundInterval = 0.05f;
    [SerializeField] private float countSoundPitchMin = 0.8f;
    [SerializeField] private float countSoundPitchMax = 1.2f;
    public int KillEnemyCount = 0;
    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>(); // SpriteRendererを取得
        playerMoveScript = this.gameObject.GetComponent<PlayerMoveScript>();
        foreach (AttackColliderScript attack_col_script in AttackColliders)
        {
            attack_col_script.AttackPower = this.AttackPower;
        }

        if (coinAudioSource == null)
        {
            coinAudioSource = this.gameObject.GetComponent<AudioSource>();
            if (coinAudioSource == null)
            {
                coinAudioSource = this.gameObject.AddComponent<AudioSource>();
            }
        }

        displayedCoins = HaveCoins;
        UpdateCoinDisplay();
        UpdateLivesDisplay();
    }

    void Update()
    {
        if (this.transform.position.y < -20)
        {
            HP = 0;

        }

        HPSlider.value = HP / MaxHP;

        if (HP <= 0 && !Death)
        {
            Death = true;
            lives--;
            UpdateLivesDisplay();
            playerMoveScript.enabled = false;
            DeathEvent();
        }
    }

    public void DeathEvent()
    {
        DeathSoundSource.Play();
        if (lives >= 0)
        {
            Respawn();
        }
        else
        {
            // StageSelectorではなくGameOverシーンに遷移するように変更
            SceneLoadManager.instance.LoadScene("GameOver");
        }
    }

    private void UpdateLivesDisplay()
    {
        if (livesTextUI != null && lives >= 0)
        {
            livesTextUI.text = lives.ToString();
        }
    }

    public void HPSet(float IncreaseHP)
    {
        HP = Mathf.Clamp(HP + IncreaseHP, 0, MaxHP);
    }

    // コイン関連の関数は変更なし
    #region Coin Functions
    public void CoinSet(int IncreaseCoin)
    {
        HaveCoins += IncreaseCoin;
        if (IncreaseCoin > 0)
        {
            PlayCoinGetSound();
        }
        displayedCoins = HaveCoins;
        UpdateCoinDisplay();
    }

    public void CoinSetSilent(int IncreaseCoin)
    {
        HaveCoins += IncreaseCoin;
        AnimateCoinIncreaseWithoutSound();
    }

    public void SetCoinsDirectly(int newCoinAmount)
    {
        HaveCoins = newCoinAmount;
        displayedCoins = newCoinAmount;
        UpdateCoinDisplay();
    }



    private void AnimateCoinIncreaseWithoutSound()
    {
        DOTween.Kill("CoinCountTween");
        if (useCountUpEffect)
        {
            DOTween.To(() => displayedCoins, x =>
            {
                displayedCoins = x;
                UpdateCoinDisplay();
            }, HaveCoins, coinAnimationDuration)
            .SetEase(coinAnimationEase)
            .SetId("CoinCountTween")
            .OnComplete(() =>
            {
                displayedCoins = HaveCoins;
                UpdateCoinDisplay();
            });
        }
        else
        {
            displayedCoins = HaveCoins;
            UpdateCoinDisplay();
        }
    }

    private void UpdateCoinDisplay()
    {
        if (HaveCoinsTextUI != null)
        {
            HaveCoinsTextUI.text = Mathf.RoundToInt(displayedCoins).ToString();
        }
    }


    private void PlayCoinGetSound()
    {
        if (coinAudioSource != null && coinGetSE != null)
        {
            coinAudioSource.pitch = 1f;
            coinAudioSource.PlayOneShot(coinGetSE);
        }
    }


    #endregion
    
    public void Attacked(float _attacked)
    {
        if (!Death && !isInvincible)
        {
            HP -= _attacked;
            AttackedEvent.Invoke();
        }
    }

    private void Respawn()
    {
        HP = MaxHP;
        Death = false;

        if (StageController.instance != null)
        {
            // StageControllerのplayerStartPositionがTransform型の場合を想定
            this.transform.position = StageController.instance.playerStartPosition.position;
        }
        else
        {
            Debug.LogError("StageControllerまたはplayerStartPositionが見つかりません。");
            this.transform.position = Vector3.zero;
        }
        playerMoveScript.enabled = true;
        StartCoroutine(HandleInvincibility());
    }

    private IEnumerator HandleInvincibility()
    {
        isInvincible = true;

        spriteRenderer.DOKill();

        // 点滅のアニメーション設定
        float flashCycleDuration = 0.2f; // 1回の点滅にかかる時間（半透明->不透明）
        float targetAlpha = 0.1f;        // 点滅時のアルファ値
        
        // 無敵時間に合わせてループ回数を計算
        int loops = Mathf.FloorToInt(invincibilityDuration / flashCycleDuration);

        // DOTweenのSequenceを作成して点滅を表現
        Sequence flashSequence = DOTween.Sequence();
        flashSequence.Append(spriteRenderer.DOFade(targetAlpha, flashCycleDuration / 2)); // 半透明へ
        flashSequence.Append(spriteRenderer.DOFade(1f, flashCycleDuration / 2));      // 不透明へ
        flashSequence.SetLoops(loops); // 計算した回数だけループ

        // Sequenceが終わるまで待機
        yield return flashSequence.WaitForCompletion();

        isInvincible = false;

        // 念のため、最後にアルファ値を1に戻す
        if (spriteRenderer != null)
        {
            Color finalColor = spriteRenderer.color;
            finalColor.a = 1f;
            spriteRenderer.color = finalColor;
        }
    }
}