using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace LinkAbleDocument
{
    [System.Serializable]
    public class TextContent
    {
        public string text;
        public int fontSize;
        public FontStyle fontStyle;
        public TextAnchor anchor;
    }
}