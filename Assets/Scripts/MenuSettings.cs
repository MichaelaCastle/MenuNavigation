using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
using EG = UnityEditor.EditorGUILayout;
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
public class TabPrefabData
{
    public TextMeshProUGUI Text;
    public Image Image;
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
    [HideInInspector] public Image TabFrameImage;
    [HideInInspector] public bool UsingCustomTab;
    [HideInInspector] public TabPrefabData tabPrefabData;
}
#if UNITY_EDITOR
[CustomEditor(typeof(MenuSettings))]
public class MenuSettingsEditor : Editor
{
    public static bool showTabSettings = true;
    public static List<bool> tabsShown = new List<bool>();
    public static bool showTabs = true;
    public static bool showPageSettings = true;
    public static bool showTabTextInfo = true;
    public static bool showHolderAndFrame = true;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        #region styles
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
        GUIStyle myFoldoutStyle2 = new GUIStyle(EditorStyles.foldout);
        myFoldoutStyle2.fontStyle = FontStyle.Italic;
        myFoldoutStyle2.fontSize = 12;
        myFoldoutStyle2.normal.textColor = myStyleColor;
        myFoldoutStyle2.onNormal.textColor = myStyleColor;
        myFoldoutStyle2.hover.textColor = myStyleColor;
        myFoldoutStyle2.onHover.textColor = myStyleColor;
        myFoldoutStyle2.focused.textColor = myStyleColor;
        myFoldoutStyle2.onFocused.textColor = myStyleColor;
        myFoldoutStyle2.active.textColor = myStyleColor;
        myFoldoutStyle2.onActive.textColor = myStyleColor;
        #endregion

        MenuSettings script = (MenuSettings)target;
        SerializedObject serializedObject = new SerializedObject(target);

        if (script.tabGroup == null)
        {
            script.tabGroup = script.gameObject.GetComponent<TabGroup>();
            script.tabGroup.menuSettings = script;
        }

        showTabSettings = EG.Foldout(showTabSettings, "Tab Button Settings", myFoldoutStyle);
        if (showTabSettings)
        {
            ++EditorGUI.indentLevel;
            showHolderAndFrame = EG.Foldout(showHolderAndFrame, "Holder and Frame Settings", myFoldoutStyle2);
            if (showHolderAndFrame)
            {
                ++EditorGUI.indentLevel;
                "Tab Holder:".Label();
                ++EditorGUI.indentLevel;
                script.tabGroup.TabHolder = script.tabGroup.TabHolder.Field<Transform>("Holder", "Where the tabs will be spawned");
                Mask holderMask = script.tabGroup.TabHolder.GetComponent<Mask>();
                holderMask.showMaskGraphic = EG.Toggle("Show Mask", holderMask.showMaskGraphic);
                if (holderMask.showMaskGraphic)
                {
                    EG.BeginHorizontal();
                    "Sprite: ".Label(40);
                    Image holderImage = script.tabGroup.TabHolder.GetComponent<Image>();
                    holderImage.sprite = holderImage.sprite.Field<Sprite>(85);
                    "Color: ".Label(40);
                    holderImage.color = holderImage.color.Field(70);
                    EG.EndHorizontal();
                }
                --EditorGUI.indentLevel;
                "".Label();
                "Tab Frame:".Label();
                ++EditorGUI.indentLevel;
                script.TabFrameImage = script.TabFrameImage.Field<Image>("Frame", "Border around the holder");
                EG.BeginHorizontal();
                "Sprite: ".Label(40);
                script.TabFrameImage.sprite = script.TabFrameImage.sprite.Field<Sprite>(85);
                "Color: ".Label(40);
                script.TabFrameImage.color = script.TabFrameImage.color.Field(70);
                EG.EndHorizontal();
                --EditorGUI.indentLevel;
                EG.LabelField("");
                --EditorGUI.indentLevel;
            }
            script.tabGroup.TabPrefab = script.tabGroup.TabPrefab.Field<GameObject>("Prefab", "What each tab will look like");
            script.tabType = (TabType)EG.EnumPopup("Tab Type", script.tabType);
            
            //GUILayout.FlexibleSpace();
            switch (script.tabType)
            {
                case TabType.Color:
                    "Colors:".Label();
                    EG.BeginHorizontal();
                    "Idle: ".Label(40);
                    script.tabGroup.TabIdle = script.tabGroup.TabIdle.Field(70);
                    "Hover: ".Label(50);
                    script.tabGroup.TabHover = script.tabGroup.TabHover.Field(70);
                    "Active: ".Label(60);
                    script.tabGroup.TabActive = script.tabGroup.TabActive.Field(70);
                    break;
                case TabType.Sprite:
                    "Images:".Label();
                    EG.BeginHorizontal();
                    "Idle: ".Label(40);
                    script.tabGroup.tabIdle = script.tabGroup.tabIdle.Field<Sprite>(85);
                    "Hover: ".Label(50);
                    script.tabGroup.tabHover = script.tabGroup.tabHover.Field<Sprite>(85);
                    "Active: ".Label(60);
                    script.tabGroup.tabActive = script.tabGroup.tabActive.Field<Sprite>(85);
                    break;
            }
            EG.EndHorizontal();

            if(script.tabPrefabData == null)
            {
                script.tabPrefabData = new TabPrefabData();
            }
            script.UsingCustomTab = EG.Toggle("Using Custsom Prefab", script.UsingCustomTab);
            if (script.UsingCustomTab)
            {
                ++EditorGUI.indentLevel;
                "Prefab Setup: ".Label(40);
                EG.BeginHorizontal();
                "Text: ".Label(40);
                script.tabPrefabData.Text = script.tabPrefabData.Text.Field<TextMeshProUGUI>(85);
                "Image: ".Label(40);
                script.tabPrefabData.Image = script.tabPrefabData.Image.Field<Image>(85);
                EG.EndHorizontal();
                --EditorGUI.indentLevel;
            }
            else
            {
                script.tabPrefabData.Text = script.tabGroup.TabPrefab.GetComponentInChildren<TextMeshProUGUI>();
                script.tabPrefabData.Image = script.tabGroup.TabPrefab.GetComponent<Image>();
            }
            showTabTextInfo = EG.Foldout(showTabTextInfo, $"Prefab Settings", myFoldoutStyle2);
            if (showTabTextInfo)
            {
                ++EditorGUI.indentLevel;
                "Changes will not take effect during runtime".Label();
                EG.BeginHorizontal();
                "Font: ".Label(30);
                script.tabPrefabData.Text.font = script.tabPrefabData.Text.font.Field<TMP_FontAsset>(85);
                "Text Color: ".Label(60);
                script.tabPrefabData.Text.color = script.tabPrefabData.Text.color.Field(70);
                EG.EndHorizontal();
                EG.BeginHorizontal();
                "Image Sprite".Label(100);
                script.tabPrefabData.Image.sprite = script.tabPrefabData.Image.sprite.Field<Sprite>(70);
                //"Pixels / Unit".Label(80);
                script.tabPrefabData.Image.pixelsPerUnitMultiplier = EG.FloatField("Pixels / Unit", script.tabPrefabData.Image.pixelsPerUnitMultiplier/*, GUILayout.MinWidth(150)*/);
                EG.EndHorizontal();
                --EditorGUI.indentLevel;
            }
            showTabs = EG.Foldout(showTabs, $"Tabs", myFoldoutStyle2);
            if (showTabs && !Application.isPlaying)
            {
                ++EditorGUI.indentLevel;
                for (int i = 0; i < script.Tabs.Count; ++i)
                {
                    if (tabsShown.Count <= i)
                    {
                        tabsShown.Add(true);
                    }
                    tabsShown[i] = EG.Foldout(tabsShown[i], $"Element {i}");
                    if (tabsShown[i])
                    {
                        ++EditorGUI.indentLevel;
                        var tab = script.Tabs[i];
                        tab.Title = EG.TextField("Title", tab.Title);
                        tab.ConnectedPage = tab.ConnectedPage.Field<GameObject>("ConnectedPage", "What page should be displayed when this tab is selected");
                        --EditorGUI.indentLevel;
                    }
                }
                EG.BeginHorizontal();
                if (!Application.isPlaying && GUILayout.Button("+", GUILayout.Width(20)))
                {
                    script.tabGroup.CreateTab();
                }
                if (!Application.isPlaying && script.Tabs.Count > 0 && GUILayout.Button("-", GUILayout.Width(20)))
                {
                    DestroyImmediate(script.Tabs[^1].GameObject);
                    script.Tabs.RemoveAt(script.Tabs.Count - 1);
                }
                EG.EndHorizontal();
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
        showPageSettings = EG.Foldout(showPageSettings, "Page Settings", myFoldoutStyle);
        if (showPageSettings)
        {
            ++EditorGUI.indentLevel;
            script.PageLoadInTime = EG.Slider(new GUIContent("Load in animation speed: ", "Seconds it takes for all the cards to load in"), script.PageLoadInTime, 0, 0.1f);
            PageLoadIn.LoadTime = script.PageLoadInTime;
            if (!Application.isPlaying)
            {
                PageLoadIn.LoadDirection = EG.Toggle(new GUIContent("Load right to left", "The first card to load in will be the bottom right card"), PageLoadIn.LoadDirection);
            }
            --EditorGUI.indentLevel;
        }
    }
}
#endif