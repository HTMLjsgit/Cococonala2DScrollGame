using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyMove : MonoBehaviour
{
    [Header("移動ロジック")]
    public string[] TargetTurningPointTag; // ぶつかったら迂回するtagたち
    public float speed;
    public float DirectionX = 1;
    Rigidbody2D rigid;

    [SerializeField]
    private enum MoveVector
    {
        X,
        Y
    };
    [SerializeField] private MoveVector moveVector;

    Vector3 DefaultLocalScale;
    public bool Move = true;
    public bool PermitMove = true;
    public bool LookAtPlayer = false;
    EnemyController enemy_controller;
    GameObject Player;

    void Start()
    {
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
        DefaultLocalScale = this.transform.localScale;
        enemy_controller = this.gameObject.GetComponent<EnemyController>();
        Player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        // ▼▼▼ この中のロジックは一切変更していません ▼▼▼
        if (!LookAtPlayer)
        {
            this.transform.localScale = new Vector2(DefaultLocalScale.x * DirectionX, DefaultLocalScale.y);
        }
        else if (LookAtPlayer)
        {
            this.transform.localScale = new Vector3(DefaultLocalScale.x * enemy_controller.EnemyToPlayerDirection, DefaultLocalScale.y);
        }
        if (Move && PermitMove)
        {
            if (moveVector == MoveVector.X)
            {
                rigid.linearVelocity = new Vector2(DirectionX * speed, rigid.linearVelocity.y);
            }
            else if (moveVector == MoveVector.Y)
            {
                rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, DirectionX * speed);
            }
        }
        else
        {
            rigid.linearVelocity = Vector2.zero;
        }
        // ▲▲▲ この中のロジックは一切変更していません ▲▲▲
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (TargetTurningPointTag.Contains(other.gameObject.tag))
        {
            DirectionX *= -1;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (TargetTurningPointTag.Contains(collision.gameObject.tag))
        {
            DirectionX *= -1;
        }
    }


}