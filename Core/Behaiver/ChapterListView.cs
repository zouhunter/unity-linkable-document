using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace LinkAbleDocument
{
    public class ChapterListView : ListSelector
    {
        private ToggleGroup group;
        private bool stopEvent;
        private Dictionary<string, Toggle> createdDic = new Dictionary<string, Toggle>();
        public bool AnyToggleOn { get { return group.AnyTogglesOn(); } }

        protected override void Awake()
        {
            base.Awake();
           
            group = m_parent.GetComponentInChildren<ToggleGroup>();

            if (group == null)
            {
                group = m_parent.gameObject.AddComponent<ToggleGroup>();
            }
        }

        public void SetSelect(string key, bool trigger = false)
        {
            stopEvent = trigger ? false : true;

            if (createdDic.ContainsKey(key))
            {
                var tog = createdDic[key];
                tog.isOn = false;
                tog.isOn = true;
            }

            stopEvent = false;
        }
        protected override void OnSaveItem(GameObject instence)
        {
            base.OnSaveItem(instence);
            instence.GetComponent<Toggle>().isOn = false;
        }
        public void SetSelect(int defultValue, bool trigger = false)
        {
            if (options.Length <= defultValue) return;
            var key = options[defultValue];
            SetSelect(key, trigger);
        }
        protected override void OnCreateItem(int id, GameObject instence)
        {
            base.OnCreateItem(id, instence);
            var type = options[id];
            instence.GetComponentInChildren<Text>().text = type;
            var toggle = instence.GetComponentInChildren<Toggle>();
            Debug.Assert(toggle, "预制体或子物体上没有toggle组件");

            if (singleChoise)
            {
                toggle.group = group;
            }

            UnityAction<bool> action = (x) =>
            {
                if (x)
                {
                    if (!stopEvent)
                        Select(id);
                }
                else
                {
                    UnSelect(id);
                }
            };
            toggle.onValueChanged.AddListener(action);
            onResetEvent += () =>
            {
                toggle.onValueChanged.RemoveListener(action);
            };
            createdDic.Add(type, toggle);
        }
        protected override void ClearCreated()
        {
            base.ClearCreated();
            createdDic.Clear();
        }
    }
}