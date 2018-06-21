using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace LinkAbleDocument
{
    public class AnchorImage : MonoBehaviour
    {
        private Image m_image;
        private LayoutElement layoutElement;

        private void Awake()
        {
            m_image = gameObject.GetComponentInChildren<Image>();
            layoutElement = gameObject.GetComponent<LayoutElement>();
            if (layoutElement == null)
            {
                layoutElement = gameObject.AddComponent<LayoutElement>();
            }
        }

        public void Init(SpriteContent spriteContent)
        {
            layoutElement.preferredHeight = spriteContent.preferredHeight;
            layoutElement.preferredWidth = spriteContent.preferredWidth;
            m_image.sprite = spriteContent.sprite;
            m_image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, spriteContent.preferredWidth);
            m_image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, spriteContent.preferredHeight);
            SetImageAliment(spriteContent.anchor);
        }

        private void SetImageAliment(TextAnchor anchor)
        {
            switch (anchor)
            {
                case TextAnchor.UpperLeft:
                    m_image.rectTransform.pivot =
                    m_image.rectTransform.anchorMin =
                        m_image.rectTransform.anchorMax = new Vector2(0, 1);
                    break;
                case TextAnchor.UpperCenter:
                    m_image.rectTransform.pivot =
                    m_image.rectTransform.anchorMin =
                      m_image.rectTransform.anchorMax = new Vector2(0.5f, 1);
                    break;
                case TextAnchor.UpperRight:
                    m_image.rectTransform.pivot =
                    m_image.rectTransform.anchorMin =
                     m_image.rectTransform.anchorMax = new Vector2(1, 1);
                    break;
                case TextAnchor.MiddleLeft:
                    m_image.rectTransform.pivot =
                    m_image.rectTransform.anchorMin =
                    m_image.rectTransform.anchorMax = new Vector2(0, 0.5f);
                    break;
                case TextAnchor.MiddleCenter:
                    m_image.rectTransform.pivot =
                    m_image.rectTransform.anchorMin =
                   m_image.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    break;
                case TextAnchor.MiddleRight:
                    m_image.rectTransform.pivot =
                    m_image.rectTransform.anchorMin =
                 m_image.rectTransform.anchorMax = new Vector2(1, 0.5f);
                    break;
                case TextAnchor.LowerLeft:
                    m_image.rectTransform.pivot =
                    m_image.rectTransform.anchorMin =
              m_image.rectTransform.anchorMax = new Vector2(0, 0);
                    break;
                case TextAnchor.LowerCenter:
                    m_image.rectTransform.pivot =
                    m_image.rectTransform.anchorMin =
            m_image.rectTransform.anchorMax = new Vector2(0.5f, 0);
                    break;
                case TextAnchor.LowerRight:
                    m_image.rectTransform.pivot =
                    m_image.rectTransform.anchorMin =
         m_image.rectTransform.anchorMax = new Vector2(1, 0);
                    break;
                default:
                    break;
            }
            m_image.transform.localPosition = Vector2.zero;
        }
    }
}