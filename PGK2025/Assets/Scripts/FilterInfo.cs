using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilterInfo : MonoBehaviour
{
    public TMP_Text filterNameText;
    public TMP_Text filterDesc;

    public void SetUp(string name)
    {
        InfoClass info = new InfoClass(name);
        filterNameText.text = info.name;
        filterDesc.text = info.description; 
    }
}
