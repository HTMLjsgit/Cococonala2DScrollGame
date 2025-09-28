using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckScript : MonoBehaviour
{
    public bool IsGround;

    //3つもGroundを用意している理由は安定化させるため。(坂道があっても有効)
    public bool Ground_1;
    public bool Ground_2;
    public bool Ground_3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Ground_1 || Ground_2 || Ground_3){
            //３つのうちどれか一つがGroundと接地していたらIsGroundをTrueにする
            IsGround = true;
        }else if(!Ground_1 && !Ground_2 && !Ground_3){
            IsGround = false;
        }
    }
}
