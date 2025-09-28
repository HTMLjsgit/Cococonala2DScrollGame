using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeRaycastToTarget : MonoBehaviour
{
    //Raycastで得た接地判定をGroundCheckScriptに渡してあげるスクリプト
    public GroundCheckScript ground_check_script;
    public bool OnGround;
    public GameObject TargetObject;
    RaycastHit2D physics;
    public LayerMask GroundLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        physics = Physics2D.Linecast(this.transform.position, TargetObject.transform.position, GroundLayerMask);
        if(physics.collider){
            OnGround = true;

            if(this.gameObject.name == "1"){
                ground_check_script.Ground_1 = true;
            }else if(this.gameObject.name == "2"){
                ground_check_script.Ground_2 = true;
            }else if(this.gameObject.name == "3"){
                ground_check_script.Ground_3 = true;
            }
        }else{
            OnGround = false;

            if(this.gameObject.name == "1"){
                ground_check_script.Ground_1 = false;
            }else if(this.gameObject.name == "2"){
                ground_check_script.Ground_2 = false;
            }else if(this.gameObject.name == "3"){
                ground_check_script.Ground_3 = false;
            }
        }
    }
}
