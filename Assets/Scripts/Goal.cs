using UnityEngine;

public class Goal : MonoBehaviour
{
    private StageController stageController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stageController = GameObject.FindWithTag("StageController").GetComponent<StageController>();
    }

    // Update is called once per frame
    // void Update()
    // {

    // }
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("other.gameObject.tag=" + other.gameObject);
        if (other.gameObject.tag == "Player")
        {
            stageController.Goal();
        }
    }
}
