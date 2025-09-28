using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Destroy(float time){
        Destroy(this.gameObject, time);
    }
}
