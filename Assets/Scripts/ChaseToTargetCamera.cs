using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseToTargetCamera : MonoBehaviour
{
    [SerializeField] private GameObject Target;
    [SerializeField] private Vector2 MaxPosition;
    [SerializeField] private Vector2 MinPosition;
    private float x;
    private float y;
    [SerializeField] private float smooth;
    // Update is called once per frame
    void Update()
    {
        // ターゲットのX座標を、指定した最小値(MinPosition.x)と最大値(MaxPosition.x)の間に制限（クランプ）し、変数xに代入する
        x = Mathf.Clamp(Target.transform.position.x, MinPosition.x, MaxPosition.x);
        // ターゲットのY座標を、指定した最小値(MinPosition.y)と最大値(MaxPosition.y)の間に制限（クランプ）し、変数yに代入する
        y = Mathf.Clamp(Target.transform.position.y, MinPosition.y, MaxPosition.y);
        // Lerp（線形補間）を使って、カメラの現在位置から目標位置へスムーズに移動させる
        // this.transform.position: カメラの現在の位置
        // new Vector3(x, y, -10): 制限範囲内のターゲット座標。Z座標を-10にしているのは、2Dゲームでオブジェクトより手前にカメラを置くための一般的な設定
        // smooth: 移動の滑らかさの度合い
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(x, y, -10), smooth);
    }
}
