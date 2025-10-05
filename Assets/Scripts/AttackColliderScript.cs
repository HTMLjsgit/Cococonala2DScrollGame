using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AttackColliderScript : MonoBehaviour
{
    public enum MineStatus{
        Enemy,
        Player
    }
    // MineStatusでEnemyとPlayerを分けている理由は EnemyとEnemy同士の衝突をさけたり、使いまわしをするため。
    public MineStatus mine_status; //自分がPlayerかEnemyか
    public float AttackPower; //攻撃力(EnemyStatus PlayerStatus それぞれから代入してあげる。)

    GameObject Player;
    PlayerStatus player_status;
    PlayerMoveScript playerMoveScript;
    private float StayTimeNow;
    public UnityEvent HittingEvent; //ヒットしたときのイベント
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        player_status = Player.GetComponent<PlayerStatus>();
        playerMoveScript = Player.GetComponent<PlayerMoveScript>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (mine_status == MineStatus.Enemy)
        {
            //自分がEnemyならばPlayerに当たったとき
            if (other.gameObject.tag == "Player")
            {
                if (playerMoveScript.IsGround)
                {
                    //地面に触れてる間だけダメージを受ける
                    player_status.Attacked(AttackPower);
                    HittingEvent.Invoke();
                    StayTimeNow = 0;
                }

            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(mine_status == MineStatus.Enemy){
            //自分がEnemyならばPlayerに当たったとき
            if(other.gameObject.tag == "Player"){
                StayTimeNow += Time.deltaTime;
                if (StayTimeNow > 1)
                {
                    player_status.Attacked(AttackPower);
                    HittingEvent.Invoke();
                    StayTimeNow = 0;
                }

            }
        }


    }
}
