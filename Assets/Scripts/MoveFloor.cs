using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    public enum MovementType
    {
        Horizontal,
        Vertical,
        Circular,
        OrbitalPatrol
    }

    [Header("移動タイプ選択")]
    [SerializeField] private MovementType movementType = MovementType.Horizontal;

    [Header("共通設定")]
    [Tooltip("移動速度（中心の移動や円運動の速さ）")]
    [SerializeField] private float speed = 2.0f;

    // --- 各移動タイプごとの設定 ---

    [Header("横・縦移動")]
    [SerializeField] private int directionX = 1;
    [SerializeField] private int directionY = 1;

    [Header("その場での円運動")]
    [SerializeField] private Transform centerPoint;

    [Header("円運動・往復円運動")]
    [Tooltip("円運動の半径")]
    [SerializeField] private float radius = 2.0f;
    [Tooltip("往復円運動時の、円運動（公転）の速度")]
    [SerializeField] private float orbitalSpeed = 3.0f;

    [Header("往復移動")]
    [Tooltip("往復移動の始点")]
    [SerializeField] private Transform pointA;
    [Tooltip("往復移動の終点")]
    [SerializeField] private Transform pointB;

    // --- 内部計算用 ---
    private float angle = 0f;
    private Transform currentTarget;
    private Vector2 currentCenterPosition;

    void Start()
    {
        if (movementType == MovementType.OrbitalPatrol)
        {
            if (pointA != null && pointB != null)
            {
                currentCenterPosition = pointA.position;
                currentTarget = pointB;
            }
            else
            {
                Debug.LogError("OrbitalPatrolにはPointAとPointBの設定が必要です！", this.gameObject);
                enabled = false;
            }
        }
    }

    void Update()
    {
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
            case MovementType.OrbitalPatrol:
                MoveOrbitalPatrol();
                break;
        }
    }

    private void MoveHorizontally()
    {
        transform.Translate(Vector2.right * directionX * speed * Time.deltaTime);
    }

    private void MoveVertically()
    {
        transform.Translate(Vector2.up * directionY * speed * Time.deltaTime);
    }

    private void MoveCircularly()
    {
        if (centerPoint == null) return;
        angle += speed * Time.deltaTime; // 修正箇所
        float x = centerPoint.position.x + Mathf.Cos(angle) * radius;
        float y = centerPoint.position.y + Mathf.Sin(angle) * radius;
        transform.position = new Vector2(x, y);
    }

    private void MoveOrbitalPatrol()
    {
        currentCenterPosition = Vector2.MoveTowards(currentCenterPosition, currentTarget.position, speed * Time.deltaTime);

        if (Vector2.Distance(currentCenterPosition, currentTarget.position) < 0.01f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
        }
        
        angle += orbitalSpeed * Time.deltaTime;
        float x = currentCenterPosition.x + Mathf.Cos(angle) * radius;
        float y = currentCenterPosition.y + Mathf.Sin(angle) * radius;
        
        transform.position = new Vector2(x, y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Turn"))
        {
            if (movementType == MovementType.Horizontal) directionX *= -1;
            else if (movementType == MovementType.Vertical) directionY *= -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y < -0.5f)
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
    //移動の軌道を表示
    private void OnDrawGizmos()
    {
        if (movementType == MovementType.Circular && centerPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(centerPoint.position, radius);
        }
        else if (movementType == MovementType.OrbitalPatrol && pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(pointA.position, 0.2f);
            Gizmos.DrawWireSphere(pointB.position, 0.2f);

            if (Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(currentCenterPosition, 0.3f);
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(currentCenterPosition, radius);
            }
        }
    }
}