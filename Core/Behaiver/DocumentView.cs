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

        private void Awake()
        {
            RegistEvents();
            if (document != null){
                LoadDocumentUI(document);
            }

        }
        private void RegistEvents()
        {
            listView.onSelectID += OnSwitchChapter;
        }

        private void OnSwitchChapter(int arg0)
        {
            if(arg0 >=0 && document.chapters.Count > arg0)
            {
                var chapter = document.chapters[arg0];
                container.chapter = chapter;
            }
        }

        public void SetDocument(Document document)
        {
            this.document = document;
            LoadDocumentUI(document);
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