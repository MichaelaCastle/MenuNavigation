using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class TabGroup : MonoBehaviour
{
    /*[HideInInspector]*/ //public List<TabButton> tabButtons = new List<TabButton>();
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
    [SerializeField] private PageLoadIn pageLoad;
    [SerializeField] private MenuSettings menuSettings;
    public void CreateTab(TabButton data)
    {
        if (Application.isPlaying)
        {
            return;
        }
        GameObject t = Instantiate(TabPrefab, TabHolder);
        t.name = data.Title;
        //t.transform.SetSiblingIndex(data.Index);
        t.GetComponentInChildren<TextMeshProUGUI>().text = data.Title;
        data.tabGroup = this;
        data.GameObject = t;
    }
    public void CreateTab()
    {
        if (Application.isPlaying)
        {
            return;
        }
        GameObject t = Instantiate(TabPrefab, TabHolder);
        TabButton data = t.GetComponent<TabButton>();
        t.name = data.Title;
        //t.transform.SetSiblingIndex(data.Index);
        t.GetComponentInChildren<TextMeshProUGUI>().text = data.Title;
        data.tabGroup = this;
        data.GameObject = t;
        menuSettings.Tabs.Add(data);
    }
    public void EditTab(TabButton data)
    {
        if (Application.isPlaying || data == null)
        {
            return;
        }
        GameObject t = data.GameObject;
        t.name = data.Title;
        //t.transform.SetSiblingIndex(data.Index);
        t.GetComponentInChildren<TextMeshProUGUI>().text = data.Title;
    }
    public void DeleteTab(TabButton data)
    {
        if (Application.isPlaying)
        {
            return;
        }
        menuSettings.Tabs.Remove(data);
        DestroyImmediate(data.gameObject);
    }
    public void Subscribe(TabButton button)
    {
        if (menuSettings.Tabs == null)
        {
            menuSettings.Tabs = new List<TabButton>();
        }
        menuSettings.Tabs.Add(button);
        ResetTabs();
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            if (menuSettings.tabType == TabType.Sprite)
            {
                button.background.sprite = tabHover;
            }
            else
            {
                button.background.color = TabHover;
            }
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
        if (menuSettings.tabType == TabType.Sprite)
        {
            button.background.sprite = tabActive;
        }
        else
        {
            button.background.color = TabActive;
        }
        int index = button.ConnectedPage.transform.GetSiblingIndex();
        for (int i = 0; i < menuSettings.Tabs.Count; ++i)
        {
            menuSettings.Tabs[i].ConnectedPage.SetActive(menuSettings.Tabs[i] == button);
        }
        pageLoad.Selected(button.ConnectedPage.transform);
    }
    public void ResetTabs()
    {
        foreach (TabButton button in menuSettings.Tabs)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            if (menuSettings.tabType == TabType.Sprite)
            {
                button.background.sprite = tabIdle;
            }
            else
            {
                button.background.color = TabIdle;
            }
        }
    }
}