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
        private Transform m_parent;
        [SerializeField]
        private Text m_textPrefab;
        [SerializeField]
        private Image m_imagePrefab;

        public Chapter chapter { get; set; }
        public List<Keyward> keywards { get; set; }


    }
}