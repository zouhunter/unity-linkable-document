using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using LinkAbleDocument;
using System;

public class Demo : MonoBehaviour
{
    public DocumentView documentView;
    private float fontSize = 1;
    private void Awake()
    {
        documentView.onClickItem += OnClickItem;
    }
    private void OnGUI()
    {
        fontSize = GUILayout.HorizontalSlider(fontSize, 0.1f, 10f,GUILayout.Width(100));
        documentView.SetFontSize(fontSize);
    }

    private void OnClickItem(string arg0)
    {
        Debug.Log(arg0);
        switch (arg0)
        {
            case "dog":
                Debug.Log("他好像一条狗！");
                break;
            case "精*神":
                Debug.Log("世界上没有精神病人。");
                break;
            default:
                break;
        }
    }
}
