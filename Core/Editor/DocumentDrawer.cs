using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Linq;

namespace LinkAbleDocument
{
    [CustomEditor(typeof(Document))]
    public class DocumentDrawer : Editor
    {
        private SerializedProperty prop_script;
        private SerializedProperty prop_keywards;
        private SerializedProperty prop_chapters;
        private ReorderableList list_keywards;
        private ReorderableList list_chapters;
        private int selected;
        private string[] options = { chapterName, keys };
        private const string chapterName = "单元";
        private const string keys = "超链接";
        private const string editorprefer_selected = "editorprefer_documentdrawer_selected";
        private Editor chapterDrawer;
        private bool _drawScript = true;
        public bool drawScript { get { return _drawScript; } set { _drawScript = value; } }

        private void OnEnable()
        {
            InitPrefer();
            FindProps();
            InitReorderLists();
        }
        private void OnDisable()
        {
            UpdateScriptObject();
        }
        private void InitReorderLists()
        {
            list_keywards = new ReorderableList(serializedObject, prop_keywards);
            list_keywards.drawHeaderCallback = (rect) =>
             {
                 rect = new Rect(rect.x + 10, rect.y, rect.width - 10, rect.height);
                 var rect1 = new Rect(rect.x, rect.y, rect.width * 0.5f, EditorGUIUtility.singleLineHeight);
                 var rect2 = new Rect(rect.x + rect.width * 0.5f,rect.y, rect.width * 0.5f, EditorGUIUtility.singleLineHeight);
                 EditorGUI.LabelField(rect1, "正则表达式");
                 EditorGUI.LabelField(rect2, "风格");
             };
            list_keywards.drawElementCallback = DrawKeywardElement;


            list_chapters = new ReorderableList(serializedObject, prop_chapters);
            list_chapters.drawHeaderCallback = (rect) =>
            {
                EditorGUI.LabelField(rect, chapterName);
            };
            list_chapters.elementHeight = EditorGUIUtility.singleLineHeight + GUIUtil.padding * 2;
            list_chapters.onAddCallback = OnAddChapter;
            list_chapters.drawElementCallback = DrawChapterElement;
        }

        private void OnAddChapter(ReorderableList list)
        {
            var chapter = ScriptableObject.CreateInstance<Chapter>();
            ScriptableObjUtility.AddSubAsset(chapter, target as Document);
            prop_chapters.InsertArrayElementAtIndex(prop_chapters.arraySize);
            var prop = prop_chapters.GetArrayElementAtIndex(prop_chapters.arraySize - 1);
            prop.objectReferenceValue = chapter;
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawChapterElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = GUIUtil.DrawBoxRect(rect, index.ToString());
            var rect1 = new Rect(rect.x, rect.y, rect.width - 60, rect.height);
            var prop = prop_chapters.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(rect1, prop, new GUIContent());
            if (isActive && prop.objectReferenceValue != null)
            {
                DrawChapterDetail(prop.objectReferenceValue);
            }
            var rect2 = new Rect(rect.x + rect.width - 60, rect.y, 60, rect.height);
            if (GUI.Button(rect2,"new"))
            {
                GUIUtil.CreateNewScriptObjectToProp<Chapter>((x)=> {
                    prop_chapters = serializedObject.FindProperty("chapters");
                    prop = prop_chapters.GetArrayElementAtIndex(index);
                    prop.objectReferenceValue = x;
                    prop.serializedObject.ApplyModifiedProperties();
                });
            }
        }

        

        private void DrawKeywardElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var prop = prop_keywards.GetArrayElementAtIndex(index);

            var regexRect = new Rect(rect.x, rect.y, rect.width * 0.5f, EditorGUIUtility.singleLineHeight);
            var colorRect = new Rect(rect.x + rect.width * 0.5f, rect.y, rect.width * 0.5f, EditorGUIUtility.singleLineHeight);

            var prop_regex = prop.FindPropertyRelative("regex");
            var prop_style = prop.FindPropertyRelative("style");
            prop_regex.stringValue = EditorGUI.TextField(regexRect, prop_regex.stringValue);
            prop_style.stringValue = EditorGUI.TextField(colorRect, prop_style.stringValue);

        }

        private void DrawChapterDetail(UnityEngine.Object obj)
        {
            Editor.CreateCachedEditor(obj, typeof(ChapterDrawer), ref chapterDrawer);
            if(chapterDrawer != null)
            {
                chapterDrawer.OnInspectorGUI();
            }
        }

        private void InitPrefer()
        {
            selected = EditorPrefs.GetInt("editorprefer_documentdrawer_selected");
        }
        private void FindProps()
        {
            prop_script = serializedObject.FindProperty("m_Script");
            prop_chapters = serializedObject.FindProperty("chapters");
            prop_keywards = serializedObject.FindProperty("keywards");
        }

        public override void OnInspectorGUI()
        {
            if (drawScript)
                DrawScript();

            serializedObject.Update();
            OnInspectorGUI_Worp();
            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateScriptObject()
        {
            var mainAssets = target as Document;
            var subAssets = mainAssets.chapters.Where(x => x != null).ToArray();
            ScriptableObjUtility.SetSubAssets(subAssets, mainAssets, true);
        }

        private void DrawScript()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(prop_script);
            EditorGUI.EndDisabledGroup();
        }

        private void OnInspectorGUI_Worp()
        {
            DrawToolBarOption();
            SwitchDraw();
        }

        private void SwitchDraw()
        {
            switch (options[selected])
            {
                case keys:
                    DrawKeysArray();
                    break;
                case chapterName:
                    DrawChapterArray();
                    break;
                default:
                    break;
            }
        }

        private void DrawChapterArray()
        {
            list_chapters.DoLayoutList();
        }

        private void DrawKeysArray()
        {
            list_keywards.DoLayoutList();
        }

        private void DrawToolBarOption()
        {
            EditorGUI.BeginChangeCheck();
            selected = GUILayout.Toolbar(selected, options);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetInt(editorprefer_selected, selected);
            }
        }
    }
}