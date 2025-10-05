using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーの移動を制御するスクリプト
public class PlayerMoveScript : MonoBehaviour
{
    Rigidbody2D rigid;
    [SerializeField] private float Jumpflap;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float MoveSpeedUp;
    [SerializeField] private KeyCode JumpKey;
    [SerializeField] private KeyCode SpeedUpKey;
    [SerializeField] private float x = 0;
    private bool SpeedUpMode;
    [SerializeField] private GroundCheckScript ground_check_script; 
    public bool JumpKeyPush;
    public bool Move = true;
    public bool IsGround;
    public bool PermitMove = true;
    [SerializeField] private PhysicsMaterial2D jump_material;
    [SerializeField] private AudioSource JumpAudioSource;
    Vector2 DefaultLocalScale;
    [HideInInspector]
    [SerializeField] private int Direction;
    [SerializeField] private int DirectionOfLocalScaleX;
    [SerializeField] private bool JumpMove;
    private float InitSpeed;

    void Start()
    {
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
        
        // オブジェクトの初期スケールを保存
        DefaultLocalScale = this.gameObject.transform.localScale;
        
        // 通常の移動速度を初期速度として保存
        InitSpeed = MoveSpeed;
    }

    void Update()
    {
        // 操作が許可されていない場合は、以降の処理を中断
        if (!PermitMove) return;

        x = Input.GetAxisRaw("Horizontal");
        JumpKeyPush = Input.GetKeyDown(JumpKey);
        // 指定したスピードアップキーが押され続けているかどうかを判定
        SpeedUpMode = Input.GetKey(SpeedUpKey);
        

        // --- オブジェクトの向きを判定・更新 ---
        if (this.transform.localScale.x < 0)
        {
            DirectionOfLocalScaleX = -1;
        }
        else if (this.transform.localScale.x > 0)
        {
            DirectionOfLocalScaleX = 1;
        }

        if(x > 0){
            Direction = 1;
        }else if(x < 0){
            Direction = -1;
        }

        // 入力があった場合、移動方向に応じてオブジェクトの向きを変える
        if(x != 0){
            this.transform.localScale = new Vector2(Direction * DefaultLocalScale.x, DefaultLocalScale.y);
        }

        // --- 状態の更新 ---
        // GroundCheckScriptから接地状態を取得
        IsGround = ground_check_script.IsGround;

        // --- ジャンプ処理 ---
        // ジャンプキーが押されて、かつ接地している場合
        if(JumpKeyPush && IsGround){
            JumpAudioSource.PlayOneShot(JumpAudioSource.clip); // ジャンプ音を再生
            JumpMove = true; // FixedUpdateでジャンプを実行するためのフラグを立てる
        }
        
        // --- 物理マテリアルの切り替え ---
        // 接地している場合は、物理マテリアルを解除（摩擦を元に戻す）
        if(IsGround){
            rigid.sharedMaterial = null;
        }else{
            // 空中にいる場合は、設定した物理マテリアルを適用（壁への張り付き防止など）
            rigid.sharedMaterial = jump_material;
        }

        // --- スピードアップ処理 ---
        // スピードアップキーが押されている場合
        if(SpeedUpMode){
            MoveSpeed = MoveSpeedUp; // 移動速度を上げる
        }else{
            MoveSpeed = InitSpeed; // 押されていない場合は、元の速度に戻す
        }
    }

    // 固定フレームレートで呼ばれる処理（物理演算はこちらに書くのが一般的）
    void FixedUpdate()
    {
        // 操作が許可されていない場合は、以降の処理を中断
        if (!PermitMove) return;

        // Updateで立てられたジャンプフラグを確認
        if(JumpMove){
            // Y方向の速度を上書きしてジャンプさせる（X方向の速度は維持）
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, Jumpflap);
            JumpMove = false; // フラグをリセットして、連続でジャンプしないようにする
        }

        // 水平移動が許可されている場合
        if(Move){
            // X方向の速度を入力と移動速度に応じて変更する（Y方向の速度は維持）
            rigid.linearVelocity = new Vector2(x * MoveSpeed, rigid.linearVelocity.y);
        }else{
            // 水平移動が許可されていない場合は、X方向の速度を0にする
            rigid.linearVelocity = new Vector2(0, rigid.linearVelocity.y);
        }
    }
}