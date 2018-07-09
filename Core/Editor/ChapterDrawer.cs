using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using System;

namespace LinkAbleDocument
{
    [CustomEditor(typeof(Chapter))]
    public class ChapterDrawer : Editor
    {
        private const float padding = 7;
        private SerializedProperty prop_title;
        private SerializedProperty prop_paragraphs;
        private ReorderableList list_paragraphs;

        private void OnEnable()
        {
            if (target == null)
            {
                DestroyImmediate(this);
            }
            else
            {
                InitProps();
                InitLists();
            }
        }

        private void OnSceneGUI()
        {
            Debug.Log("OnSceneGUI");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            OnInspectorGUI_Interal();
            serializedObject.ApplyModifiedProperties();
        }

        private void OnInspectorGUI_Interal()
        {
            EditorGUILayout.PropertyField(prop_title);
            target.name = prop_title.stringValue;
            list_paragraphs.DoLayoutList();
        }

        private void InitProps()
        {
            prop_title = serializedObject.FindProperty("title");
            prop_paragraphs = serializedObject.FindProperty("paragraphs");
        }

        private void InitLists()
        {
            list_paragraphs = new ReorderableList(serializedObject, prop_paragraphs);
            list_paragraphs.drawHeaderCallback = (rect =>
            {
                EditorGUI.LabelField(rect, "段落列表");
            });
            list_paragraphs.elementHeightCallback = CalcuteElementHeight;
            list_paragraphs.drawElementCallback = DrawElement;
        }

        private float CalcuteElementHeight(int index)
        {
            var prop = prop_paragraphs.GetArrayElementAtIndex(index);
            var prop_type = prop.FindPropertyRelative("type");
            if (prop_type.enumValueIndex == (int)ParagraphType.Text)
            {
                return 4 * EditorGUIUtility.singleLineHeight + 2 * padding;
            }
            else if (prop_type.enumValueIndex == (int)ParagraphType.Sprite)
            {
                return 4 * EditorGUIUtility.singleLineHeight + 2 * padding;
            }
            return 0;
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = GUIUtil. DrawBoxRect(rect, index.ToString());
            var prop = prop_paragraphs.GetArrayElementAtIndex(index);
            var prop_type = prop.FindPropertyRelative("type");
            var typeRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
            prop_type.enumValueIndex = EditorGUI.Popup(typeRect, prop_type.enumValueIndex, prop_type.enumNames);
            if (prop_type.enumValueIndex == (int)ParagraphType.Text)
            {
                var prop_textContent = prop.FindPropertyRelative("text");
                DrawTextContent(rect, prop_textContent);
            }
            else if (prop_type.enumValueIndex == (int)ParagraphType.Sprite)
            {
                var prop_spriteContent = prop.FindPropertyRelative("sprite");
                DrawSpriteContent(rect, prop_spriteContent);
            }
        }

        private void DrawSpriteContent(Rect rect, SerializedProperty prop_spriteContent)
        {
            var prop_sprite = prop_spriteContent.FindPropertyRelative("sprite");
            var prop_anchor = prop_spriteContent.FindPropertyRelative("anchor");
            var prop_width = prop_spriteContent.FindPropertyRelative("preferredWidth");
            var prop_height = prop_spriteContent.FindPropertyRelative("preferredHeight");

            var spritewidth = 3 * EditorGUIUtility.singleLineHeight;
            var spriteRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, spritewidth, spritewidth);
            prop_sprite.objectReferenceValue = EditorGUI.ObjectField(spriteRect, prop_sprite.objectReferenceValue, typeof(Sprite),false);

            var infoRect = new Rect(rect.x + spritewidth, rect.y + EditorGUIUtility.singleLineHeight, rect.width - spritewidth, rect.height - EditorGUIUtility.singleLineHeight);

            var rectWidth = new Rect(infoRect.x, infoRect.y, infoRect.width, EditorGUIUtility.singleLineHeight);
            var rect_label = new Rect(rectWidth.x +2, rectWidth.y, rectWidth.width * 0.3f,rectWidth.height);
            var rect_content = new Rect(rectWidth.x + rectWidth.width * 0.3f, rectWidth.y, rect.width * 0.6f, rectWidth.height);

            EditorGUI.LabelField(rect_label, "Anchor");
            prop_anchor.enumValueIndex = EditorGUI.Popup(rect_content, prop_anchor.enumValueIndex, prop_anchor.enumNames);

            rect_label.y += EditorGUIUtility.singleLineHeight;
            rect_content.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(rect_label, "PreferredWidth");
            prop_width.intValue = EditorGUI.IntSlider(rect_content, prop_width.intValue,0,16000);

            rect_label.y += EditorGUIUtility.singleLineHeight;
            rect_content.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(rect_label, "PreferredHeight");
            prop_height.intValue = EditorGUI.IntSlider(rect_content, prop_height.intValue,0,8000);
        }

        private void DrawTextContent(Rect rect, SerializedProperty prop_textContent)
        {
            var prop_text = prop_textContent.FindPropertyRelative("text");
            var prop_anchor = prop_textContent.FindPropertyRelative("anchor");
            var prop_fontSize = prop_textContent.FindPropertyRelative("fontSize");
            var prop_fontStyle = prop_textContent.FindPropertyRelative("fontStyle");

            var textRect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight, rect.width * 0.5f, 3 * EditorGUIUtility.singleLineHeight);
            prop_text.stringValue = EditorGUI.TextField(textRect, prop_text.stringValue);

            var rectLabel = new Rect(rect.x + rect.width * 0.5f, rect.y + EditorGUIUtility.singleLineHeight, rect.width * 0.2f, EditorGUIUtility.singleLineHeight);
            var rectItem = new Rect(rect.x + rect.width * 0.7f, rect.y + EditorGUIUtility.singleLineHeight, rect.width * 0.3f, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(rectLabel, "Anchor");
            prop_anchor.enumValueIndex = EditorGUI.Popup(rectItem, prop_anchor.enumValueIndex, prop_anchor.enumNames);

            rectItem.y += EditorGUIUtility.singleLineHeight;
            rectLabel.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(rectLabel, "FontSize");
            prop_fontSize.intValue = EditorGUI.IntField(rectItem, prop_fontSize.intValue);

            rectItem.y += EditorGUIUtility.singleLineHeight;
            rectLabel.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(rectLabel, "FontStyle");
            prop_fontStyle.enumValueIndex = EditorGUI.Popup(rectItem, prop_fontStyle.enumValueIndex, prop_fontStyle.enumNames);

        }

     

    }
}
