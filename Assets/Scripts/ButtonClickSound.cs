using UnityEngine;
using UnityEngine.UI;
public class ButtonClickSound : MonoBehaviour
{
    private AudioSource buttonAudioSource;
    private Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = this.gameObject.GetComponent<Button>();
        buttonAudioSource = this.gameObject.GetComponent<AudioSource>();
        button.onClick.AddListener(buttonAudioSource.Play);
    }
}
