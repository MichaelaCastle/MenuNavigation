using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using EG = UnityEditor.EditorGUILayout;
#endif
public static class Extensions
{
#if UNITY_EDITOR
    public static T Field<T>(this Object data, string name, string tooltip, int width) where T : Object
    {
        return (T)EG.ObjectField(new GUIContent(name, tooltip), obj: (T)data, objType: typeof(T), allowSceneObjects: true, GUILayout.MinWidth(width));
    }
    public static T Field<T>(this Object data, string name, string tooltip) where T : Object
    {
        return (T)EG.ObjectField(new GUIContent(name, tooltip), obj: (T)data, objType: typeof(T), allowSceneObjects: true);
    }
    public static T Field<T>(this Object data, int width) where T : Object
    {
        return (T)EG.ObjectField(obj: (T)data, objType: typeof(T), allowSceneObjects: true, GUILayout.MinWidth(width));
    }
    public static void Label(this string label)
    {
        EG.LabelField(label);
    }
    public static void Label(this string label, int width)
    {
        EG.LabelField(label, GUILayout.MinWidth(width));
    }
#endif
}
