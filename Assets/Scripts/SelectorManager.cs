using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SelectorManager : MonoBehaviour
{
    //ボタン選択管理
    [SerializeField] private List<GameObject> SelectObjectsHistory;
    [SerializeField] private GameObject lastSelectedObject;
    [SerializeField] private GameObject DefaultSelectObject;
    private Button select_button;
    private Selectable select_selectable;

    // Start is called before the first frame update
    void Start()
    {
        //デフォルトで選んでおきたいbuttonがあれば選択
        if (DefaultSelectObject != null)
        {
            DefaultSelectObject.GetComponent<Selectable>().Select();

        }
    }
    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null){
            if(select_button != null || select_selectable !=  null){
                EventSystem.current.SetSelectedGameObject(lastSelectedObject);
            }
        }else{
            if(EventSystem.current.currentSelectedGameObject != null){
                if(lastSelectedObject != EventSystem.current.currentSelectedGameObject){
                    SelectObjectsHistory.Add(EventSystem.current.currentSelectedGameObject);
                }
            }
            lastSelectedObject = EventSystem.current.currentSelectedGameObject;
            if(lastSelectedObject != null){
                

                select_button = lastSelectedObject.GetComponent<Button>();
                select_selectable = lastSelectedObject.GetComponent<Selectable>();
            }

        }
    }
}
