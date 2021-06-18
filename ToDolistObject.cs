using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToDolistObject : MonoBehaviour
{
    public string   objName;
    public string   type;
    public int      index;

    private Text    itemText;

    // Start is called before the first frame update
    private void Start()
    {
        itemText = GetComponentInChildren<Text>();
        itemText.text =objName;
    }

    public void SetOjectInfo(string name, string type, int index)
    {
        this.objName = name;
        this.type = type;
        this.index = index;
    }
    
}
