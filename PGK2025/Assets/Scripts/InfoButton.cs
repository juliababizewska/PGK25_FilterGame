using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UIElements;

public class InfoButton : MonoBehaviour
{
    public TMP_Text filterName;

    public void OnCursorEnter()
    {
       Debug.Log("Cursor entered on " + filterName.text);
       Vector3 positon = transform.position;
       positon += new Vector3(-230, 0, 0);
       GameObject.Find("GameController").GetComponent<GameController>().DisplayFilterInfo(filterName.text, positon);
    }

    public void OnCursorExit()
    {
        GameObject.Find("GameController").GetComponent<GameController>().DestroyItemInfo();
    }
}
