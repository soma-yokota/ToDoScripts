using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ToDolistManager : MonoBehaviour
{
    public Transform    content;
    public GameObject   addPanel;
    public Button       createButton;
    public GameObject   todolistItemPrefab;

    string filePath;

    private List<ToDolistObject> todolistObjects = new List<ToDolistObject>();
    
     
    private InputField[] addInputFields;

    public class ToDolistItem
    {
        public string objName;
        public string type;
        public int index;

        public ToDolistItem(string name, string type, int index)
        {
            this.objName = name;
            this.type = type;
            this.index = index;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        filePath = Application.persistentDataPath + "/todolist.txt";
        LoadJSONData();
        addInputFields = addPanel.GetComponentsInChildren<InputField>();

        createButton.onClick.AddListener(delegate {CreateToDolistItem(addInputFields[0].text, addInputFields[1].text); });
    }

    public void SwitchMode(int mode)
    {
        switch (mode)
        {
            //regular todolist mode
            case 0:
                addPanel.SetActive(false);
                break;

            //adding a new todolist item
            case 1:
                addPanel.SetActive(true);
                break;
        }
    }

    void CreateToDolistItem(string name, string type, int loadIndex = 0, bool loading = false)
    {
        GameObject item = Instantiate(todolistItemPrefab);

        item.transform.SetParent(content);
        ToDolistObject itemObject = item.GetComponent<ToDolistObject>();
        int index = loadIndex;
        if(loading)
        {
            index = todolistObjects.Count;
        }
        itemObject.SetOjectInfo(name, type, index);
        todolistObjects.Add(itemObject);
        ToDolistObject temp = itemObject;
        itemObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate {CheckItem(temp); });

        if(!loading)
        {
            SaveJSONData();
            SwitchMode(0);
        }
    }


    void CheckItem(ToDolistObject item)
    {
        todolistObjects.Remove(item);
        SaveJSONData();
        Destroy(item.gameObject);

    }

    void SaveJSONData()
    {
        string contents = "";

        for(int i = 0; i < todolistObjects.Count; i++)
        {
                ToDolistItem temp = new ToDolistItem(todolistObjects[i].objName, todolistObjects[i].type, todolistObjects[i].index);
                contents += JsonUtility.ToJson(todolistObjects[i]) + "\n";
        }

        File.WriteAllText(filePath, contents);
    
    }

    void LoadJSONData()
    {
        if(File.Exists(filePath))
        {
            string contents = File.ReadAllText(filePath);

            string[] splitContents = contents.Split('\n');

            foreach(string content in splitContents)
            {
                if(content.Trim() != "")
                {
                    Debug.Log(content);
                    ToDolistItem temp = JsonUtility.FromJson<ToDolistItem>(content);
                    CreateToDolistItem(temp.objName, temp.type, temp.index, true);
                }

            }

        }
        else
        {
            Debug.Log("No file!!");
        }
    }
}
