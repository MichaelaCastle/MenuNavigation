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
        }

        showTabSettings = EG.Foldout(showTabSettings, "Tab Button Settings", myFoldoutStyle);
        if (showTabSettings)
        {
            ++EditorGUI.indentLevel;
            showHolderAndFrame = EG.Foldout(showHolderAndFrame, "Holder and Frame Settings", myFoldoutStyle2);
            if (showHolderAndFrame)
            {
                ++EditorGUI.indentLevel;
                EG.LabelField("Tab Holder:");
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
                    holderImage.color = EG.ColorField(holderImage.color, GUILayout.MinWidth(70));
                    EG.EndHorizontal();
                }
                --EditorGUI.indentLevel;
                EG.LabelField("");
                EG.LabelField("Tab Frame:");
                ++EditorGUI.indentLevel;
                script.TabFrameImage = script.TabFrameImage.Field<Image>("Frame", "Border around the holder");
                EG.BeginHorizontal();
                "Sprite: ".Label(40);
                script.TabFrameImage.sprite = script.TabFrameImage.sprite.Field<Sprite>(85);
                "Color: ".Label(40);
                script.TabFrameImage.color = EG.ColorField(script.TabFrameImage.color, GUILayout.MinWidth(70));
                EG.EndHorizontal();
                --EditorGUI.indentLevel;
                EG.LabelField("");
                --EditorGUI.indentLevel;
            }
            script.tabGroup.TabPrefab = (GameObject)EG.ObjectField("Prefab", script.tabGroup.TabPrefab, typeof(GameObject), true);
            script.tabType = (TabType)EG.EnumPopup("Tab Type", script.tabType);
            
            //GUILayout.FlexibleSpace();
            switch (script.tabType)
            {
                case TabType.Color:
                    EG.LabelField("Colors:");
                    EG.BeginHorizontal();
                    EG.LabelField("Idle: ", GUILayout.MinWidth(40));
                    script.tabGroup.TabIdle = EG.ColorField(script.tabGroup.TabIdle, GUILayout.MinWidth(70));
                    EG.LabelField("Hover: ", GUILayout.MinWidth(50));
                    script.tabGroup.TabHover = EG.ColorField(script.tabGroup.TabHover, GUILayout.MinWidth(70));
                    EG.LabelField("Active: ", GUILayout.MinWidth(60));
                    script.tabGroup.TabActive = EG.ColorField(script.tabGroup.TabActive, GUILayout.MinWidth(70));
                    break;
                case TabType.Sprite:
                    EG.LabelField("Images:");
                    EG.BeginHorizontal();
                    EG.LabelField("Idle: ", GUILayout.MinWidth(40));
                    script.tabGroup.tabIdle = (Sprite)EG.ObjectField(script.tabGroup.tabIdle, typeof(Sprite), true, GUILayout.MinWidth(85));
                    EG.LabelField("Hover: ", GUILayout.MinWidth(50));
                    script.tabGroup.tabHover = (Sprite)EG.ObjectField(script.tabGroup.tabHover, typeof(Sprite), true, GUILayout.MinWidth(85));
                    EG.LabelField("Active: ", GUILayout.MinWidth(60));
                    script.tabGroup.tabActive = (Sprite)EG.ObjectField(script.tabGroup.tabActive, typeof(Sprite), true, GUILayout.MinWidth(85));
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
                EG.LabelField("Prefab Setup: ", GUILayout.MinWidth(40));
                EG.BeginHorizontal();
                EG.LabelField("Text: ", GUILayout.MinWidth(40));
                script.tabPrefabData.Text = (TextMeshProUGUI)EG.ObjectField(script.tabPrefabData.Text, typeof(TextMeshProUGUI), true, GUILayout.MinWidth(85));
                EG.LabelField("Image: ", GUILayout.MinWidth(40));
                script.tabPrefabData.Image = (Image)EG.ObjectField(script.tabPrefabData.Image, typeof(Image), true, GUILayout.MinWidth(85));
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
                EG.BeginHorizontal();
                EG.LabelField("Font: ", GUILayout.MinWidth(30));
                script.tabPrefabData.Text.font = (TMP_FontAsset)EG.ObjectField(script.tabPrefabData.Text.font, typeof(TMP_FontAsset), true, GUILayout.MinWidth(85));
                EG.LabelField("Text Color: ", GUILayout.MinWidth(60));
                script.tabPrefabData.Text.color = EG.ColorField(script.tabPrefabData.Text.color, GUILayout.MinWidth(70));
                EG.EndHorizontal();
                EG.BeginHorizontal();
                EG.LabelField("Image Sprite", GUILayout.MinWidth(80));
                script.tabPrefabData.Image.sprite = (Sprite)EG.ObjectField(script.tabPrefabData.Image.sprite, typeof(Sprite), true, GUILayout.MinWidth(70));
                EG.LabelField("Pixels / Unit", GUILayout.MinWidth(80));
                script.tabPrefabData.Image.pixelsPerUnitMultiplier = EG.FloatField(script.tabPrefabData.Image.pixelsPerUnitMultiplier, GUILayout.MinWidth(70));
                EG.EndHorizontal();
                --EditorGUI.indentLevel;
            }
            showTabs = EG.Foldout(showTabs, $"Tabs", myFoldoutStyle2);
            if (showTabs)
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
                        //tab.Index = EG.IntField("Index", tab.Index);
                        tab.ConnectedPage = (GameObject)EG.ObjectField("ConnectedPage", tab.ConnectedPage, typeof(GameObject), true);
                        --EditorGUI.indentLevel;
                    }
                }
                EG.BeginHorizontal();
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
        //script.tabGroup.tabButtons = script.Tabs;
        showPageSettings = EG.Foldout(showPageSettings, "Page Settings", myFoldoutStyle);
        if (showPageSettings)
        {
            ++EditorGUI.indentLevel;
            script.PageLoadInTime = EG.Slider(new GUIContent("Load in animation speed: ", "Seconds it takes for all the cards to load in"), script.PageLoadInTime, 0, 0.1f);
            PageLoadIn.LoadTime = script.PageLoadInTime;
            --EditorGUI.indentLevel;
        }
    }
}
#endif