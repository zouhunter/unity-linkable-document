using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LinkAbleDocument
{
    public class DocumentView : MonoBehaviour
    {
        [SerializeField,HideInInspector]
        private Document document;
        [SerializeField]
        private ChapterListView listView;
        [SerializeField]
        private ChapterContainer container;
        public event UnityAction<string> onClickItem;
        private void Awake()
        {
            RegistEvents();
            if (document != null){
                LoadDocumentUI(document);
            }
        }

        public void SetDocument(Document document)
        {
            this.document = document;
            LoadDocumentUI(document);
        }

        public void SetFontSize(float fontSizeScale)
        {
            container.fontSizeScale = fontSizeScale;
        }

        private void RegistEvents()
        {
            listView.onSelectID += OnSwitchChapter;
            container.onClickKeyward += OnClickKeyWard;

        }

        private void OnClickKeyWard(string arg0)
        {
            if (onClickItem != null)
                onClickItem.Invoke(arg0);
        }

        private void OnSwitchChapter(int arg0)
        {
            if(arg0 >=0 && document.chapters.Count > arg0)
            {
                var chapter = document.chapters[arg0];
                container.chapter = chapter;
                container.keywards = document.keywards;
            }
        }

        private void LoadDocumentUI(Document document)
        {
            var options = document.chapters.Select(x => x.title).ToArray();
            if (options.Length == 0) return;

            listView.options = options;
            listView.SetSelect(0, true);
        }
    }
}