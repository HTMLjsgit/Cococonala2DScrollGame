using UnityEngine.UI;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
public class GameStartController : MonoBehaviour
{
    [SerializeField] private Button gameStartButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameStartButton.onClick.AddListener(() =>
        {
            SceneLoadManager.instance.LoadScene("StageSelector");
        });
    }


}
