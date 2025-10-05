using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    //プレイヤーのアニメーションを管理するスクリプト
    private PlayerMoveScript playerMoveScript;
    [SerializeField]private GroundCheckScript groundCheckScript;
    private PlayerStatus playerStatus;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMoveScript = this.gameObject.GetComponent<PlayerMoveScript>();
        anim = this.gameObject.GetComponent<Animator>();
        playerStatus = this.gameObject.GetComponent<PlayerStatus>();
        playerStatus.AttackedEvent.AddListener(() =>
        {
            anim.SetTrigger("Hurt");
        });
    }

    // Update is called once per frame
    void Update()
    {
        //アニメーションをセット。
        if (playerMoveScript.JumpKeyPush)
        {
            anim.SetTrigger("Jump");
        }
        anim.SetBool("IsGrounded", groundCheckScript.IsGround);
    }
}
