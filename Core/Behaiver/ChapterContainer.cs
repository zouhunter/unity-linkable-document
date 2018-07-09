using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using Candlelight.UI;
using System.Text.RegularExpressions;

namespace LinkAbleDocument
{
    public class ChapterContainer : MonoBehaviour
    {
        [SerializeField]
        private Text m_title;
        [SerializeField]
        private Transform m_parent;
        [SerializeField]
        private HyperText m_textPrefab;
        [SerializeField]
        private AnchorImage m_imagePrefab;
        [SerializeField]
        private HorizontalOrVerticalLayoutGroup layoutGroup;
        [SerializeField]
        private Chapter m_chapter;
        [SerializeField]
        private float _fontSizeScale = 1;
        [SerializeField]
        private ScrollRect scrollRect;

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
                SetClickEvent(value);
            }
        }
        public float fontSizeScale
        {
            get
            {
                return _fontSizeScale;
            }
            set
            {
                if (Mathf.Abs(_fontSizeScale - value) > 0.01f)
                {
                    _fontSizeScale = value;
                    OnFontSizeScaleChanged(_fontSizeScale);
                }
            }
        }


        private List<AnchorImage> imagePools = new List<AnchorImage>();
        private List<HyperText> textPools = new List<HyperText>();
        public event UnityAction<string> onClickKeyward;
        private Dictionary<HyperText, int> defultFontSizeDic = new Dictionary<HyperText, int>();
        private void Awake()
        {
            if(!scrollRect) scrollRect = GetComponentInChildren<ScrollRect>();
            m_textPrefab.gameObject.SetActive(false);
            m_imagePrefab.gameObject.SetActive(false);
            if (m_chapter != null)
            {
                LoadChapterUI(m_chapter);
            }
        }

        public void LoadChapterUI(Chapter chapter)
        {
            scrollRect.verticalNormalizedPosition = 1;
            scrollRect.horizontalNormalizedPosition = 0;
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
                        text.text = LinkAbleDocument.DocUtil.WarpText(paragraph.text.text);
                        text.alignment = paragraph.text.anchor;
                        text.fontStyle = paragraph.text.fontStyle;
                        defultFontSizeDic[text] = paragraph.text.fontSize;
                        break;
                    case ParagraphType.Sprite:
                        var image = GetImageFromPool();
                        image.Init(paragraph.sprite);
                        break;
                    default:
                        break;
                }
            }

            OnFontSizeScaleChanged(fontSizeScale);
        }
        private void SetClickEvent(List<Keyward> keywards)
        {
            foreach (var item in textPools)
            {
                SetTextItemClickEvent(item, keywards);
            }
            foreach (var item in imagePools)
            {
                item.RegistOnClick((x) =>
                {
                    if (this.onClickKeyward != null)
                        onClickKeyward.Invoke(x);
                });
            }
        }

        private void SetTextItemClickEvent(HyperText text, List<Keyward> keywards)
        {
            if (keywards == null) return;
            foreach (var keyward in keywards)
            {
                SetClickAbleText(text, DocUtil.WarpText(keyward.regex), keyward.style);
            }
        }

        private void OnClickText(HyperText arg0, HyperText.LinkInfo arg1)
        {
            if (this.onClickKeyward != null)
                onClickKeyward.Invoke(DocUtil.UnWarpText( arg1.Name));
        }

        private void SetClickAbleText(HyperText text, string regex, string style)
        {
            var pattern = string.Format("({0})(?!</a>)", regex);
            var textInfo = Regex.Replace(text.text, pattern, (match) =>
            {
                if (match.Groups.Count > 1)
                {
                    return string.Format("<a name=\"{0}\" class=\"{1}\">{2}</a>", regex, style, match.Groups[1].Value);
                }
                return string.Format("<a name=\"{0}\" class=\"{1}\">{2}</a>", regex, style, match.Value);
            }, RegexOptions.Multiline);
            text.text = textInfo;
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

        private HyperText GetTextFromPool()
        {
            HyperText text = null;
            text = textPools.Find(x => x != null && !x.gameObject.activeInHierarchy);
            if (text == null)
            {
                text = Instantiate(m_textPrefab);
                text.transform.SetParent(m_parent, false);
                text.ClickedLink.AddListener(OnClickText);
                SetTextItemClickEvent(text, keywards);
                textPools.Add(text);
            }
            else
            {
                text.transform.SetAsLastSibling();
            }
            text.gameObject.SetActive(true);
            return text;
        }

        private void OnFontSizeScaleChanged(float _fontSizeScale)
        {
            foreach (var item in textPools)
            {
                item.fontSize = (int)(defultFontSizeDic[item] * fontSizeScale);
            }
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
            else
            {
                image.transform.SetAsLastSibling();
            }
            image.gameObject.SetActive(true);
            return image;
        }
    }
}