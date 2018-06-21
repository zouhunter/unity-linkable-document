using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace LinkAbleDocument
{
    public class GUIUtil
    {
        public const float padding = 7;
        public static Rect DrawBoxRect(Rect orignalRect, string index)
        {
            var idRect = new Rect(orignalRect.x - padding, orignalRect.y + padding, 20, 20);
            EditorGUI.LabelField(idRect, index.ToString());
            var boxRect = PaddingRect(orignalRect, padding * 0.5f);
            GUI.Box(boxRect, "");
            var rect = PaddingRect(orignalRect);
            return rect;
        }
        public static Rect PaddingRect(Rect orignalRect, float padding = padding)
        {
            var rect = new Rect(orignalRect.x + padding, orignalRect.y + padding, orignalRect.width - padding * 2, orignalRect.height - padding * 2);
            return rect;
        }
        public static void CreateNewScriptObjectToProp<T>(UnityAction<T> onCreate) where T : ScriptableObject
        {
            var instence = ScriptableObject.CreateInstance<T>();
            ProjectWindowUtil.CreateAsset(instence, typeof(T).ToString() + ".asset");
            EditorApplication.update = () =>
            {
                if (instence == null)
                {
                    EditorApplication.update = null;
                }
                else
                {
                    var path = AssetDatabase.GetAssetPath(instence);
                    if (!string.IsNullOrEmpty(path))
                    {
                        var obj = AssetDatabase.LoadAssetAtPath<T>(path);
                        if(onCreate != null){
                            onCreate(obj);
                        }
                        EditorApplication.update = null;
                    }
                }

            };
        }
    }
}