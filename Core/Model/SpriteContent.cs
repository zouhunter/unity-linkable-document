using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace LinkAbleDocument
{
    [System.Serializable]
    public class SpriteContent
    {
        public Sprite sprite;
        public TextAnchor anchor;
        public int preferredWidth;
        public int preferredHeight;
    }
}