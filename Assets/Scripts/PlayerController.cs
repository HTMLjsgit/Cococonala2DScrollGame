using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    PlayerStatus player_status;
    private PlayerMoveScript player_move_script;
    // AttackColliderScript attack_collider_script;

    // Start is called before the first frame update
    void Start()
    {
        player_status = this.gameObject.GetComponent<PlayerStatus>();
        player_move_script = this.gameObject.GetComponent<PlayerMoveScript>();
    }

}
