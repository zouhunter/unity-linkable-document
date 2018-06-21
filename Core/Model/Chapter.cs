﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace LinkAbleDocument
{
    [System.Serializable]
    public class Chapter:ScriptableObject
    {
        public string title;
        public List<Paragraph> paragraphs;
    }

}