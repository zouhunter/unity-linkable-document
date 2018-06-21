using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

namespace LinkAbleDocument
{
    [CustomEditor(typeof(DocumentView))]
    public class DocumentViewDrawer : Editor
    {
        private SerializedProperty prop_document;
        private Editor documentDrawer;
        private void OnEnable()
        {
            prop_document = serializedObject.FindProperty("document");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            using (var hor = new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.PropertyField(prop_document);
                if (GUILayout.Button("new", GUILayout.Width(60)))
                {
                    GUIUtil.CreateNewScriptObjectToProp<Document>((x)=> {
                        serializedObject.FindProperty("document").objectReferenceValue = x;
                        serializedObject.ApplyModifiedProperties();
                    });
                }
            }
            serializedObject.ApplyModifiedProperties();
            if (prop_document.objectReferenceValue != null)
            {
                DrawDocument(prop_document.objectReferenceValue);
            }
        }

        private void DrawDocument(UnityEngine.Object obj)
        {
            Editor.CreateCachedEditor(obj, typeof(DocumentDrawer), ref documentDrawer);
            if (documentDrawer != null)
            {
                (documentDrawer as DocumentDrawer).drawScript = false;
                documentDrawer.OnInspectorGUI();
            }
        }
    }
}