using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening; // DOTweenを使用するために必要

public class EnemyStatus : MonoBehaviour
{
    //ここは敵のステータスを管理する場所
    [SerializeField] private  float HP = 100; //HP
    [SerializeField] private  float AttackPower; //普通攻撃の攻撃力
    [SerializeField] private  AttackColliderScript[] AttackColliders;
    [Header("イベント系")]
    public UnityEvent DeathEvent;
    public UnityEvent AttackedEvent;
    public UnityEvent DeathAnimationEndEvent;
    [SerializeField] private bool Death;
    private Animator anim;
    private bool death_once;
    private EnemyMove enemyMove;
    // Start is called before the first frame update
    void Start()
    {
        foreach (AttackColliderScript attack_col_script in AttackColliders)
        {
            //AttackColliderScriptのAttackPowerに自分のステータスの攻撃力を代入してあげる。
            attack_col_script.AttackPower = this.AttackPower;
        }
        enemyMove = this.gameObject.GetComponent<EnemyMove>();
        anim = this.gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //もしHPが0以下になれば死ぬ(破壊する)
        if (this.HP <= 0)
        {
            // まだ死亡処理が呼ばれていなければ実行
            if (!death_once)
            {
                Die();
            }
        }
    }

    // 死亡処理をこの関数にまとめる
    private void Die()
    {
        death_once = true; // 処理を一度だけ実行するためのフラグ
        Death = true;
        DeathEvent.Invoke();
        enemyMove.PermitMove = false;
        // 親以下の全Transformを取ってきて…
        Transform[] allChildren = transform.GetComponentsInChildren<Transform>(true);

        foreach (var child in allChildren)
        {
            // 子についてるattackcolliderを無効にする。
            if (child.CompareTag("AttackCollider"))
            {
                GameObject attackColliderObj = child.gameObject;
                attackColliderObj.SetActive(false);
            }
            if (child.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                child.gameObject.SetActive(false);
            }
        }
    }


    /// <summary>
    /// 踏みつけられた時に呼ばれる関数
    /// </summary>
    public void Stamped()
    {
        // すでに死んでいる場合は何もしない
        if (Death) return;

        // HPを0にして即座に死亡状態にする
        HP = 0;

        // アニメーターがある場合は停止させる（任意）
        if(anim != null)
        {
            anim.enabled = false;
        }
        
        // --- ここからDOTweenによる潰れるアニメーション ---
        var sequence = DOTween.Sequence();

        // 0.2秒かけて、X(横)に1.5倍、Y(縦)に0.2倍にスケール変化させる
        sequence.Append(transform.DOScale(new Vector3(1.5f, 0.2f, 1f), 0.2f));
        // その後、0.1秒かけて完全に平ら（Yスケールが0）にする
        sequence.Append(transform.DOScaleY(0f, 0.1f));
        
        // アニメーションが完了したらDeathAnimationEnd()を呼ぶ
        sequence.OnComplete(() =>
        {
            Destroy(this.gameObject, 1);
        });
    }


}