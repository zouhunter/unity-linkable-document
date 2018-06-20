using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace LinkAbleDocument
{
    public class Document : ScriptableObject
    {
        public List<Keyward> keywards;
        public List<Chapter> chapters;

        public void RegistOnClick(string keyward)
        {

        }
    }
}