using UnityEngine;
using DG.Tweening; // DOTweenを使用するために必要

public class Coin : MonoBehaviour
{
    public bool AlreadyPassed;
    private PlayerStatus playerStatus;
    
    [Header("コイン設定")]
    [SerializeField] 
    [Tooltip("取得時に加算されるコインの枚数")]
    private int GetCoins = 1;

    [Header("取得時アニメーション設定")]
    [SerializeField] 
    [Tooltip("コインが上に移動する距離")]
    private float _moveUpDistance = 1f;

    [SerializeField] 
    [Tooltip("アニメーションにかかる時間（秒）")]
    private float _animationDuration = 0.5f;

    [SerializeField] 
    [Tooltip("アニメーションのイージング")]
    private Ease _animationEase = Ease.OutQuad;

    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;

    void Awake()
    {
        // 自身のコンポーネントを予め取得しておく
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
    }
    
    void Start()
    {
        // PlayerStatusはFindでも良いが、シーンに常に一つしかないならシングルトン化も検討
        playerStatus = GameObject.FindWithTag("Player").GetComponent<PlayerStatus>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // プレイヤーに触れて、かつまだ取得されていない場合
        if (other.CompareTag("Player") && !AlreadyPassed)
        {
            // 多重取得を防ぐ
            AlreadyPassed = true;
            // アニメーション中に再度触れないようにColliderを無効化
            _collider.enabled = false;
            
            // プレイヤーにコインを加算
            playerStatus.CoinSet(GetCoins);
            
            // --- ここからDOTweenによるアニメーション処理 ---

            // 1. 現在位置から上に移動するアニメーション
            transform.DOMoveY(transform.position.y + _moveUpDistance, _animationDuration)
                .SetEase(_animationEase);

            // 2. SpriteRendererのアルファ値を0に（フェードアウト）するアニメーション
            //    上の移動と同時に実行される
            _spriteRenderer.DOFade(0f, _animationDuration)
                .SetEase(_animationEase)
                .OnComplete(() =>
                {
                    // 3. アニメーションが完了したらGameObjectを破棄する
                    Destroy(this.gameObject);
                });
        }
    }
}