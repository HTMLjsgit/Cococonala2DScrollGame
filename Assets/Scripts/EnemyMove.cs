using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyMove : MonoBehaviour
{
    [Header("移動ロジック")]
    [SerializeField] private string[] TargetTurningPointTag; // ぶつかったら迂回するtagたち
    [SerializeField] private float speed;
    [SerializeField] private float Direction = 1;
    [SerializeField] Rigidbody2D rigid;

    private enum MoveVector
    {
        X,
        Y
    };
    [SerializeField] private MoveVector moveVector;

    public bool Move = true;
    public bool PermitMove = true;
    private Vector3 DefaultLocalScale;
    void Start()
    {
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
        DefaultLocalScale = this.transform.localScale;
    }

    void Update()
    {
        this.transform.localScale = new Vector2(DefaultLocalScale.x * Direction, DefaultLocalScale.y);
        if (Move && PermitMove)
        {
            if (moveVector == MoveVector.X)
            {
                //横に移動するタイプ
                rigid.linearVelocity = new Vector2(Direction * speed, rigid.linearVelocity.y);
            }
            else if (moveVector == MoveVector.Y)
            {
                //縦に移動するタイプ
                rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, Direction * speed);
            }
        }
        else
        {
            //!Move or !PermitMoveなら 完全に停止
            rigid.linearVelocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (TargetTurningPointTag.Contains(other.gameObject.tag))
        {
            //TurnPointに触れたら(到着したら)、方向を変更する
            Direction *= -1;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (TargetTurningPointTag.Contains(collision.gameObject.tag))
        {
            Direction *= -1;
        }
    }


}