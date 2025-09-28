using UnityEngine;

public class Turn : MonoBehaviour
{
    // 色と半径を設定
    public Color gizmoColor = Color.red;
    public float radius = 0.2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        // 塗りつぶされた球体を描画するように変更
        Gizmos.DrawSphere(transform.position, radius);
    }
}