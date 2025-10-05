using UnityEngine.UI;
using UnityEngine;

public class GameClearController : MonoBehaviour
{
    [SerializeField] private Button backToSelectorButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backToSelectorButton.onClick.AddListener(() =>
        {
            SceneLoadManager.instance.LoadScene("StageSelector");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
