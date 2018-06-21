using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace LinkAbleDocument
{
    public class Document : ScriptableObject
    {
        public List<Keyward> keywards = new List<Keyward>();
        public List<Chapter> chapters = new List<Chapter>();

        public void RegistOnClick(string keyward)
        {

        }
    }
}