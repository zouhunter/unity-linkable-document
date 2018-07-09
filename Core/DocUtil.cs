using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace LinkAbleDocument
{
    public class DocUtil {
        public static readonly string no_breaking_space = "\u00A0";
        public static string WarpText(string text)
        {
            return text.Replace(" ", no_breaking_space);
        }

        internal static string UnWarpText(string text)
        {
            return text.Replace(no_breaking_space, " ");
        }
    }
}