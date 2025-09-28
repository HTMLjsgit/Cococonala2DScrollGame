using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{

    Rigidbody2D rigid;
    public float Jumpflap;
    public float MoveSpeed;
    public float MoveSpeedUp;
    public KeyCode JumpKey;
    public KeyCode SpeedUpKey;
    public float x = 0;
    private bool SpeedUpMode;
    public bool IsGround;
    public GroundCheckScript ground_check_script; 
    public bool JumpKeyPush;
    public bool Move = true;
    public PhysicsMaterial2D jump_material;
    public AudioSource JumpAudioSource;
    Vector2 DefaultLocalScale;
    [HideInInspector]
    public int Direction;
    public int DirectionOfLocalScaleX;
    private float InitSpeed;
    public bool JumpMove;
    public bool PermitMove = true;
    // Start is called before the first frame update
    void Start()
    {
        rigid = this.gameObject.GetComponent<Rigidbody2D>();
        //
        DefaultLocalScale = this.gameObject.transform.localScale;
        InitSpeed = MoveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PermitMove) return;
        x = Input.GetAxisRaw("Horizontal");
        JumpKeyPush = Input.GetKeyDown(JumpKey);
        SpeedUpMode = Input.GetKey(SpeedUpKey);
        
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
        IsGround = ground_check_script.IsGround;
        // if(y > 0){
        //     Debug.Log("JumpPushKey--------------------------");
        //     JumpKeyPush = true;
        // }
        // if(y == 0 || y < 0){
        //     JumpKeyPush = false;
        // }
        if(x != 0){
            this.transform.localScale = new Vector2(Direction * DefaultLocalScale.x, DefaultLocalScale.y);
        }
        if(JumpKeyPush){
            if(IsGround){
                JumpAudioSource.PlayOneShot(JumpAudioSource.clip);
                JumpMove = true;
            }
        }
        if(IsGround){
            rigid.sharedMaterial = null;
        }else{
            rigid.sharedMaterial = jump_material;
        }
        if(SpeedUpMode){
            MoveSpeed = MoveSpeedUp;   
        }else{
            MoveSpeed = InitSpeed;
        }

        
    }

    void FixedUpdate()
    {
        if (!PermitMove) return;
        if(JumpMove){
            rigid.linearVelocity = new Vector2(rigid.linearVelocity.x, Jumpflap);
            JumpMove = false;
        }
        if(Move){
            rigid.linearVelocity = new Vector2(x * MoveSpeed, rigid.linearVelocity.y);
        }else{
            rigid.linearVelocity = new Vector2(0, rigid.linearVelocity.y);
        }


    }
}
