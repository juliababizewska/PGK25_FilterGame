using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class InfoButton : MonoBehaviour
{
    public TMP_Text filterName;



    public void OnCursorEnter()
    {
        RectTransform buttonRect = GetComponent<RectTransform>();

        GameObject.Find("GameController")
            .GetComponent<GameController>()
            .DisplayFilterInfo(filterName.text, buttonRect);
    }


    public void OnCursorExit()
    {
        GameObject.Find("GameController").GetComponent<GameController>().DestroyItemInfo();
    }
}
