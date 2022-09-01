using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif
public enum TabType
{
    Color, Sprite
}
[System.Serializable]
public class TabData
{
    [SerializeField]
    public string Title;
    [SerializeField]
    public int Index;
    [SerializeField]
    public GameObject ConnectedPage;
}
[System.Serializable]
[RequireComponent (typeof(TabGroup))]
public class MenuSettings : MonoBehaviour
{
    [HideInInspector]
    public TabType tabType;
    [HideInInspector] public TabGroup tabGroup;

    [HideInInspector] public List<TabButton> Tabs = new List<TabButton>();

    [HideInInspector] public float PageLoadInTime;
}
#if UNITY_EDITOR
[CustomEditor(typeof(MenuSettings))]
public class MenuSettingsEditor : Editor
{
    public static bool showTabSettings = true;
    public static List<bool> tabsShown = new List<bool>();
    public static bool showTabs = true;
    public static bool showPageSettings = true;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
        myFoldoutStyle.fontStyle = FontStyle.Bold;
        myFoldoutStyle.fontSize = 12;
        Color myStyleColor = Color.white;
        myFoldoutStyle.normal.textColor = myStyleColor;
        myFoldoutStyle.onNormal.textColor = myStyleColor;
        myFoldoutStyle.hover.textColor = myStyleColor;
        myFoldoutStyle.onHover.textColor = myStyleColor;
        myFoldoutStyle.focused.textColor = myStyleColor;
        myFoldoutStyle.onFocused.textColor = myStyleColor;
        myFoldoutStyle.active.textColor = myStyleColor;
        myFoldoutStyle.onActive.textColor = myStyleColor;

        MenuSettings script = (MenuSettings)target;
        SerializedObject serializedObject = new SerializedObject(target);

        if (script.tabGroup == null)
        {
            script.tabGroup = script.gameObject.GetComponent<TabGroup>();
        }

        showTabSettings = EditorGUILayout.Foldout(showTabSettings, "Tab Button Settings", myFoldoutStyle);
        if (showTabSettings)
        {
            ++EditorGUI.indentLevel;
            script.tabGroup.TabPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", script.tabGroup.TabPrefab, typeof(GameObject), true);
            script.tabGroup.TabHolder = (Transform)EditorGUILayout.ObjectField(new GUIContent("Holder", "Where the tabs will be spawned"), script.tabGroup.TabHolder, typeof(Transform), true);
            script.tabType = (TabType)EditorGUILayout.EnumPopup("Tab Type", script.tabType);
            
            //GUILayout.FlexibleSpace();
            switch (script.tabType)
            {
                case TabType.Color:
                    EditorGUILayout.LabelField("Colors:");
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Idle: ", GUILayout.MinWidth(40));
                    script.tabGroup.TabIdle = EditorGUILayout.ColorField(script.tabGroup.TabIdle, GUILayout.MinWidth(70));
                    EditorGUILayout.LabelField("Hover: ", GUILayout.MinWidth(50));
                    script.tabGroup.TabHover = EditorGUILayout.ColorField(script.tabGroup.TabHover, GUILayout.MinWidth(70));
                    EditorGUILayout.LabelField("Active: ", GUILayout.MinWidth(60));
                    script.tabGroup.TabActive = EditorGUILayout.ColorField(script.tabGroup.TabActive, GUILayout.MinWidth(70));
                    break;
                case TabType.Sprite:
                    EditorGUILayout.LabelField("Images:");
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Idle: ", GUILayout.MinWidth(40));
                    script.tabGroup.tabIdle = (Sprite)EditorGUILayout.ObjectField(script.tabGroup.tabIdle, typeof(Sprite), true, GUILayout.MinWidth(85));
                    EditorGUILayout.LabelField("Hover: ", GUILayout.MinWidth(50));
                    script.tabGroup.tabHover = (Sprite)EditorGUILayout.ObjectField(script.tabGroup.tabHover, typeof(Sprite), true, GUILayout.MinWidth(85));
                    EditorGUILayout.LabelField("Active: ", GUILayout.MinWidth(60));
                    script.tabGroup.tabActive = (Sprite)EditorGUILayout.ObjectField(script.tabGroup.tabActive, typeof(Sprite), true, GUILayout.MinWidth(85));
                    break;
            }
            EditorGUILayout.EndHorizontal();
            showTabs = EditorGUILayout.Foldout(showTabs, $"Tabs");
            if (showTabs)
            {
                ++EditorGUI.indentLevel;
                for (int i = 0; i < script.Tabs.Count; ++i)
                {
                    if (tabsShown.Count <= i)
                    {
                        tabsShown.Add(true);
                    }
                    tabsShown[i] = EditorGUILayout.Foldout(tabsShown[i], $"Element {i}");
                    if (tabsShown[i])
                    {
                        ++EditorGUI.indentLevel;
                        var tab = script.Tabs[i];
                        tab.Title = EditorGUILayout.TextField("Title", tab.Title);
                        //tab.Index = EditorGUILayout.IntField("Index", tab.Index);
                        tab.ConnectedPage = (GameObject)EditorGUILayout.ObjectField("ConnectedPage", tab.ConnectedPage, typeof(GameObject), true);
                        --EditorGUI.indentLevel;
                    }
                }
                EditorGUILayout.BeginHorizontal();
                if (!Application.isPlaying && GUILayout.Button("+", GUILayout.Width(20)))
                {
                    /*script.Tabs.Add(new TabButton());
                    script.tabGroup.CreateTab(script.Tabs[^1]);*/
                    script.tabGroup.CreateTab();
                    //script.tabGroup.tabButtons = script.Tabs;
                }
                if (!Application.isPlaying && script.Tabs.Count > 0 && GUILayout.Button("-", GUILayout.Width(20)))
                {
                    DestroyImmediate(script.Tabs[^1].GameObject);
                    script.Tabs.RemoveAt(script.Tabs.Count - 1);
                    //script.tabGroup.tabButtons = script.Tabs;
                }
                EditorGUILayout.EndHorizontal();
                --EditorGUI.indentLevel;
            }
            --EditorGUI.indentLevel;
        }
        if (!Application.isPlaying)
        {
            for (int i = script.Tabs.Count - 1; i >= 0; --i)
            {
                if (script.tabGroup.TabHolder.childCount < script.Tabs.Count)
                {
                    script.tabGroup.CreateTab(script.Tabs[i]);
                }
                else if (script.tabGroup.TabHolder.childCount > script.Tabs.Count)
                {
                    script.tabGroup.DeleteTab(script.Tabs[i]);
                }
                else
                {
                    script.tabGroup.EditTab(script.Tabs[i]);
                }
            }
        }
        //script.tabGroup.tabButtons = script.Tabs;
        showPageSettings = EditorGUILayout.Foldout(showPageSettings, "Page Settings", myFoldoutStyle);
        if (showPageSettings)
        {
            ++EditorGUI.indentLevel;
            script.PageLoadInTime = EditorGUILayout.Slider(new GUIContent("Load in animation speed: ", "Seconds it takes for all the cards to load in"), script.PageLoadInTime, 0, 0.1f);
            PageLoadIn.LoadTime = script.PageLoadInTime;
            --EditorGUI.indentLevel;
        }
    }
}
#endif