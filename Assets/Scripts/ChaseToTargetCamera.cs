using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseToTargetCamera : MonoBehaviour
{
    public GameObject Target;
    public Vector2 MaxPosition;
    public Vector2 MinPosition;
    float x;
    float y;
    public float smooth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        x = Mathf.Clamp(Target.transform.position.x, MinPosition.x, MaxPosition.x);
        y = Mathf.Clamp(Target.transform.position.y, MinPosition.y, MaxPosition.y);
        this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(x, y, -10), smooth);
    }
}
