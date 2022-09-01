using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
[System.Serializable]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    
    [HideInInspector] public TabGroup tabGroup;
    /*[HideInInspector]*/ public Image background;
    [HideInInspector] public UnityEvent onTabSelected;
    [HideInInspector] public UnityEvent onTabDeselected;
    public string Title;
    public int Index;
    public GameObject ConnectedPage;
    public GameObject GameObject;
    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    private void Start()
    {
        //background = GetComponent<Image>();
        if(tabGroup == null)
        {
            tabGroup = GameObject.Find("Canvas").GetComponent<TabGroup>();
        }
        tabGroup.Subscribe(this);
    }

    public void Select()
    {
        if (onTabSelected != null)
        {
            onTabSelected.Invoke();
        }
    }
    public void Deselect()
    {
        if (onTabDeselected != null)
        {
            onTabDeselected.Invoke();
        }
    }
}
