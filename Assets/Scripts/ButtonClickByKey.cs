using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonClickByKey : MonoBehaviour
{
    public KeyCode PushKey;

    public bool SelectNow;

    Selectable selectable;

    Button button;
    // Start is called before the first frame update
    void Start()
    {
        selectable = this.gameObject.GetComponent<Selectable>();
        button = this.gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if(SelectNow){
            if(Input.GetKeyDown(PushKey)){
                button.onClick.Invoke();
            }
        }
    }

    public void OnSelect(){
        SelectNow = true;

    }
    public void DeSelect(){
        SelectNow = false;
    }
}
