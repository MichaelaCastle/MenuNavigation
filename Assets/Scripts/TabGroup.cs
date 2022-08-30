using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class TabGroup : MonoBehaviour
{
    /*[HideInInspector]*/ public List<TabButton> tabButtons = new List<TabButton>();
    [HideInInspector] public Sprite tabIdle = null;
    [HideInInspector] public Sprite tabHover = null;
    [HideInInspector] public Sprite tabActive = null;
    [HideInInspector] public Color TabIdle = Color.white;
    [HideInInspector] public Color TabHover = Color.white;
    [HideInInspector] public Color TabActive = Color.white;
    [HideInInspector] public TabButton selectedTab;
    [HideInInspector] public List<GameObject> objectsToSwap = new List<GameObject>();
    [HideInInspector] public Transform TabHolder;
    [HideInInspector] public GameObject TabPrefab;
    public void CreateTab(TabData data)
    {
        GameObject t = Instantiate(TabPrefab, TabHolder);
        t.name = data.Title;
        //t.transform.SetSiblingIndex(data.Index);
        t.GetComponentInChildren<TextMeshProUGUI>().text = data.Title;
        TabButton currentTab = t.GetComponent<TabButton>();
        currentTab.tabGroup = this;
        currentTab.data = data;
        tabButtons.Add(currentTab);
    }
    public void EditTab(TabData data)
    {
        TabButton b = tabButtons.Find(x => x.data == data);
        GameObject t = b.gameObject;
        t.name = data.Title;
        //t.transform.SetSiblingIndex(data.Index);
        t.GetComponentInChildren<TextMeshProUGUI>().text = data.Title;
    }
    public void DeleteTab(TabData data)
    {
        TabButton b = tabButtons.Find(x => x.data == data);
        tabButtons.Remove(b);
        DestroyImmediate(b.gameObject);
    }
    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
        ResetTabs();
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            //button.background.sprite = tabHover;
            button.background.color = TabHover;
        }
    }
    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }
    public void OnTabSelected(TabButton button)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }
        selectedTab = button;
        selectedTab.Select();
        ResetTabs();
        //button.background.sprite = tabActive;
        button.background.color = TabActive;
        int index = button.transform.GetSiblingIndex() - 1;
        for (int i = 0; i < objectsToSwap.Count; ++i)
        {
            objectsToSwap[i].SetActive(i == index);
        }
    }
    public void ResetTabs()
    {
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            //button.background.sprite = tabIdle;
            button.background.color = TabIdle;
        }
    }
}