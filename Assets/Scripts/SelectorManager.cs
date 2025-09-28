using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SelectorManager : MonoBehaviour
{
    public List<GameObject> SelectObjectsHistory;
    public GameObject lastSelectedObject;
    public GameObject DefaultSelectObject;
    Button select_button;
    Selectable select_selectable;


    void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
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
