using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ListLINE : MonoBehaviour
{
    [SerializeField] GameObject roomNodePrefab;
    // Start is called before the first frame update
    private void Start()
    {
        //button = button.GetComponent<Button>();
    }
    public void updateList()
    {   
        GasGet gasGet = GetComponent<GasGet>();
        getData[] data = gasGet.inputJson.events;
        TalkLINE talk = GetComponent<TalkLINE>();

        HashSet<string> idList = new HashSet<string>();
        List<List<string>> talkList = new List<List<string>>();
        int i = 0;
        foreach (getData d in data) {
            if(idList.Add(d.userId)) {
                talkList.Add(new List<string>());
                talkList[i].Add(d.userId);
                talkList[i].Add(d.name);
                i++;
            }
        }
        var scrollView = GameObject.Find("TalkRoomList");
        var viewPort = scrollView.transform.Find("Viewport");
        Transform content = viewPort.transform.Find("Content");
        foreach (Transform child in content) {
            Destroy(child.gameObject);
        }
        for (int j = 0; j < i; j++)
        {
            var roomNode = Instantiate<GameObject>(roomNodePrefab, content.transform, false);
            var button = roomNode.GetComponent<Button>();
            var name = roomNode.transform.Find("Text").GetComponent<Text>();
            string talkid = talkList[j][0];
            button.onClick.AddListener(() => talk.DispTalk(talkid));
            name.text = talkList[j][1];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
