using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace LinkAbleDocument
{
    public class ChapterContainer : MonoBehaviour
    {
        [SerializeField]
        private Text m_title;
        [SerializeField]
        private Transform m_parent;
        [SerializeField]
        private RegexHypertext m_textPrefab;
        [SerializeField]
        private AnchorImage m_imagePrefab;
        [SerializeField]
        private HorizontalOrVerticalLayoutGroup layoutGroup;
        [SerializeField]
        private Chapter m_chapter;
        public Chapter chapter
        {
            get { return m_chapter; }
            set
            {
                m_chapter = value;
                LoadChapterUI(m_chapter);
            }
        }
        [SerializeField]
        private List<Keyward> _keywards;
        public List<Keyward> keywards
        {
            get
            {
                return _keywards;
            }
            set
            {
                _keywards = value;
            }
        }

        private List<AnchorImage> imagePools = new List<AnchorImage>();
        private List<RegexHypertext> textPools = new List<RegexHypertext>();

        private void Awake()
        {
            m_textPrefab.gameObject.SetActive(false);
            m_imagePrefab.gameObject.SetActive(false);
            if (m_chapter != null)
            {
                LoadChapterUI(m_chapter);
            }
        }

        public void LoadChapterUI(Chapter chapter)
        {
            if (m_title) m_title.text = chapter.title;
            //if (layoutGroup) layoutGroup.childAlignment = chapter.anchor;

            ClearLast();

            for (int i = 0; i < chapter.paragraphs.Count; i++)
            {
                var paragraph = chapter.paragraphs[i];
                switch (paragraph.type)
                {
                    case ParagraphType.Text:
                       var text = GetTextFromPool();
                        text.text = paragraph.text.text;
                        text.alignment = paragraph.text.anchor;
                        text.fontSize = paragraph.text.fontSize;
                        text.fontStyle = paragraph.text.fontStyle;
                        break;
                    case ParagraphType.Sprite:
                        var image = GetImageFromPool();
                        image.Init(paragraph.sprite);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ClearLast()
        {
            foreach (var item in imagePools)
            {
                item.gameObject.SetActive(false);
            }
            foreach (var item in textPools)
            {
                item.gameObject.SetActive(false);
            }
        }

        private RegexHypertext GetTextFromPool()
        {
            RegexHypertext text = null;
            text = textPools.Find(x => x != null && !x.gameObject.activeInHierarchy);
            if(text == null)
            {
                text = Instantiate(m_textPrefab);
                text.transform.SetParent(m_parent, false);
                textPools.Add(text);
            }
            text.gameObject.SetActive(true);
            return text;
        }

        private AnchorImage GetImageFromPool()
        {
            AnchorImage image = null;
            image = imagePools.Find(x => x != null && !x.gameObject.activeInHierarchy);
            if (image == null)
            {
                image = Instantiate(m_imagePrefab);
                image.transform.SetParent(m_parent, false);
                imagePools.Add(image);
            }
            image.gameObject.SetActive(true);
            return image;
        }
    }
}