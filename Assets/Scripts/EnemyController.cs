using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EnemyStatus enemy_status;
    GameObject Player;
    [HideInInspector]
    public float EnemyToPlayer;
    [HideInInspector]
    public int EnemyToPlayerDirection;
    int MyDirection; //自分が向いている方向
    Vector3 DefaultLocalScale;
    private float EnemyToPlayerDistanceMeasure;
    // Start is called before the first frame update
    void Start()
    {
        enemy_status = this.gameObject.GetComponent<EnemyStatus>();
        Player = GameObject.FindWithTag("Player");
        DefaultLocalScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //敵からPlayerまでの距離(左側にいるのか 右側にいるのか)
        EnemyToPlayer = Player.transform.position.x - this.transform.position.x;

        //自分が向いている方向を-1か1で取得
        MyDirection = (int)Mathf.Floor(Mathf.Clamp(this.transform.localScale.x, -1, 1));

        //自分からプレイヤーまでの距離
        EnemyToPlayerDistanceMeasure = Vector2.Distance(this.transform.position, Player.transform.position);
        
        if(EnemyToPlayer > 0){
            EnemyToPlayerDirection = 1;
        }else if(EnemyToPlayer < 0){
            EnemyToPlayerDirection = -1;
        }
    }
}
