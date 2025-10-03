using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    // 移動タイプをインスペクターで選べるようにする
    public enum MovementType
    {
        Horizontal, // 横移動
        Vertical,   // 縦移動
        Circular    // 円運動
    }

    [Header("移動タイプ選択")]
    [SerializeField] private MovementType movementType = MovementType.Horizontal;

    [Header("共通設定")]
    [SerializeField] private float speed = 2.0f;

    [Header("横・縦移動の設定")]
    [Tooltip("横移動の現在の向き (1が右, -1が左)")]
    [SerializeField] private int directionX = 1;
    [Tooltip("縦移動の現在の向き (1が上, -1が下)")]
    [SerializeField] private int directionY = 1;

    [Header("円運動の設定")]
    [Tooltip("回転の中心となるオブジェクト")]
    [SerializeField] private Transform centerPoint;
    [SerializeField] private float radius = 3.0f;

    // 円運動で使う角度の変数
    private float angle = 0f;

    void Update()
    {
        // 選択された移動タイプに応じて、処理を切り替える
        switch (movementType)
        {
            case MovementType.Horizontal:
                MoveHorizontally();
                break;

            case MovementType.Vertical:
                MoveVertically();
                break;

            case MovementType.Circular:
                MoveCircularly();
                break;
        }
    }

    // 横移動の処理
    private void MoveHorizontally()
    {
        transform.Translate(Vector2.right * directionX * speed * Time.deltaTime);
    }

    // 縦移動の処理
    private void MoveVertically()
    {
        transform.Translate(Vector2.up * directionY * speed * Time.deltaTime);
    }

    // 円運動の処理
    private void MoveCircularly()
    {
        if (centerPoint == null) return; // 中心が設定されていなければ何もしない

        angle += speed * Time.deltaTime;
        float x = centerPoint.position.x + Mathf.Cos(angle) * radius;
        float y = centerPoint.position.y + Mathf.Sin(angle) * radius;
        transform.position = new Vector2(x, y);
    }

    // 反転ポイントに接触した時の処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // "Turn"タグのオブジェクトに当たったら
        if (collision.gameObject.CompareTag("Turn"))
        {
            // 移動タイプに応じて、向きを反転させる
            if (movementType == MovementType.Horizontal)
            {
                directionX *= -1;
            }
            else if (movementType == MovementType.Vertical)
            {
                directionY *= -1;
            }
        }
    }

    // プレイヤーが床に乗った/降りた時の処理 (変更なし)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 衝突情報の中から、最初の接点の法線ベクトルを取得
            Vector2 normal = collision.contacts[0].normal;

            // 法線ベクトルのY成分が-0.5より小さい場合（＝ほぼ真上を向いている場合）
            // つまり、プレイヤーが床の上に乗ったと判断
            if (normal.y < -0.5f)
            {
                // プレイヤーを床の子供にして、一緒に動くようにする
                collision.transform.SetParent(this.transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    // Gizmoの表示処理
    private void OnDrawGizmos()
    {
        // 円運動の時だけGizmoを表示する
        if (movementType == MovementType.Circular)
        {
            if (centerPoint != null)
            {
                Gizmos.color = Color.cyan;
                // 中心点
                Gizmos.DrawWireSphere(centerPoint.position, 0.2f);
                // 回転軌道
                Gizmos.DrawWireSphere(centerPoint.position, radius);
            }
        }
    }
}