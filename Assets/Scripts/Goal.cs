using UnityEngine;

public class Goal : MonoBehaviour
{
    //ゴールに触れたときの処理
    private StageController stageController;
    void Start()
    {
        stageController = GameObject.FindWithTag("StageController").GetComponent<StageController>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //StageControllerのGoal関数を実行。 
            stageController.Goal();
        }
    }
}
