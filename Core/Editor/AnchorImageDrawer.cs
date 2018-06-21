//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Events;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEditor.UI;
//namespace LinkAbleDocument
//{
//    [CustomEditor(typeof(AnchorImage))]
//    public class AnchorImageDrawer : ImageEditor
//    {
//        SerializedProperty prop_alignment;
//        protected override void OnEnable()
//        {
//            base.OnEnable();
//            prop_alignment = serializedObject.FindProperty("alignment");
//        }
//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();
//            EditorGUILayout.PropertyField(prop_alignment);
//        }
//    }
//}
