﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace LinkAbleDocument
{
    [System.Serializable]
    public class Paragraph
    {
        public ParagraphType type;
        public TextAnchor anchor;
        public TextContent text;
        public SpriteContent sprite;
    }
}