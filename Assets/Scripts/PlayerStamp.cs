using UnityEngine;

// ファイル名も「PlayerStomp.cs」に変更してください
public class PlayerStomp : MonoBehaviour
{
    [Header("踏みつけ判定")]
    [SerializeField]
    [Tooltip("Raycastを発射する位置（プレイヤーの足元など）")]
    private Transform _raycastOrigin;

    [SerializeField]
    [Tooltip("Raycastの距離")]
    private float _raycastDistance = 0.5f;

    [SerializeField]
    [Tooltip("どのレイヤーを敵として検出するか")]
    private LayerMask _enemyLayerMask;

    [Header("踏みつけアクション")]
    [SerializeField]
    [Tooltip("敵を踏んだ時のジャンプ力")]
    private float _stompJumpForce = 10f;
    [SerializeField]
    private AudioSource StampedSound;
    private Rigidbody2D _rigid; // privateなフィールドは慣習的にアンダースコア(_)で始めます

    private void Awake()
    {
        // このオブジェクトにアタッチされているRigidbody2Dを取得
        _rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Physics2D.RaycastはRaycastHit2Dを戻り値として返す
        RaycastHit2D hit = Physics2D.Raycast(
            _raycastOrigin.position,
            Vector2.down,
            _raycastDistance,
            _enemyLayerMask
        );

        // Raycastが何かに当たったか (colliderがnullでないか) をチェック
        if (hit.collider != null)
        {
            Debug.DrawRay(hit.point, hit.normal * 10, Color.green);
            // タグが"Enemy"かどうかをチェック
            if (hit.collider.CompareTag("Enemy") && _rigid.linearVelocityY < 0)
            {
                EnemyStatus enemyStatus = hit.collider.gameObject.GetComponent<EnemyStatus>();
                enemyStatus.Stamped();
                // --- プレイヤーをジャンプさせる処理 ---
                // 現在の垂直方向の速度を0にリセットし、常に一定の高さでジャンプするようにする
                _rigid.linearVelocity = new Vector2(_rigid.linearVelocity.x, 0);
                // 上向きに瞬間的な力（Impulse）を加える
                _rigid.AddForce(Vector2.up * _stompJumpForce, ForceMode2D.Impulse);
                //音を鳴らす
                StampedSound.Play();
            }
        }
    }
}