using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TalkLINE : MonoBehaviour
{
    [SerializeField] GameObject chatNodePrefab;
    [SerializeField] GameObject timeLinePrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // POST時に使用するID
    public string sendId;
    public string timeline;
    public void DispTalk(string id)
    {
        sendId = id;
        timeline = "";
        GasGet gasGet = GetComponent<GasGet>();
        getData[] data = gasGet.inputJson.events;
        List<Message> messageList = new List<Message>();
        HashSet<string> titleList = new HashSet<string>();
        foreach (getData d in data) {
            if (d.userId == id) {
                Message m = new Message();
                m.name = d.name;
                m.message = d.message;
                m.date = d.date;
                messageList.Add(m);
                titleList.Add(d.name);
            }
        }
        var scrollView = GameObject.Find("MessageList");
        var viewPort = scrollView.transform.Find("Viewport");
        var content = viewPort.transform.Find("Content");
        var title = GameObject.Find("Title");
        var titleText = title.transform.Find("Text").GetComponent<Text>();

        titleList.Remove("VR_bot");
        int member = titleList.Count;
        //Debug.Log(member);
        titleText.text = String.Join("、", titleList);
        
        // 呼び出しごとに初期化
        foreach (Transform child in content) {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < messageList.Count; i++)
        {
            var datetime = messageList[i].date;
            var daytime = datetime.Substring(0, datetime.IndexOf("T"));
            daytime = daytime.Substring(daytime.IndexOf("-") + 1);

            if (timeline != daytime) {
                var month = daytime.Substring(0, 2);
                if (month.IndexOf("0") == 0) month = month.Substring(1);
                var day = daytime.Substring(3);
                if (day.IndexOf("0") == 0) day = day.Substring(1);
                var timeLine = Instantiate<GameObject>(timeLinePrefab, content.transform, false);
                var tldate = timeLine.transform.Find("Text").GetComponent<Text>();
                tldate.text = month + "月" + day + "日";
                timeline = daytime;
            }

            var chatNode = Instantiate<GameObject>(chatNodePrefab, content.transform, false);
            var chatNodeLayoutGroup = chatNode.GetComponent<LayoutGroup>();
            var name = chatNode.transform.Find("Name");
            var message = chatNode.transform.Find("Message");
            var chatBoard = message.transform.Find("ChatBoard").GetComponent<Image>();
            var chatText = chatBoard.transform.Find("Text").GetComponent<Text>();
            var date = message.transform.Find("Date");
            var dateText = date.GetComponent<Text>();
            var dateObject = date.gameObject;

            datetime = datetime.Substring(datetime.IndexOf("T") + 1);
            datetime = datetime.Substring(0, datetime.Length - 8);
            if (datetime.IndexOf("0") == 0) datetime = datetime.Substring(1);
            dateText.text = datetime;

            if (timeline != daytime) {
                var timeLine = Instantiate<GameObject>(timeLinePrefab, content.transform, false);
                var tldate = timeLine.transform.Find("Text").GetComponent<Text>();
                tldate.text = daytime;
                timeline = daytime;
            }
            
            if (messageList[i].name == "VR_bot") {
                Destroy(name.gameObject);
                chatNodeLayoutGroup.childAlignment = TextAnchor.MiddleRight;
                chatNodeLayoutGroup.padding.left = 40;
                chatBoard.color = new Color(0f, 1f, 0.1f);
                dateObject.transform.SetSiblingIndex(0);

            } else {
                if (member == 1) {Destroy(name.gameObject);}
                chatNodeLayoutGroup.childAlignment = TextAnchor.MiddleLeft;
                chatNodeLayoutGroup.padding.right = 40;
                chatBoard.color = new Color(1f, 1f, 1f);
                name.GetComponent<Text>().text = messageList[i].name;
            }
            chatText.text = messageList[i].message;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public struct Message
{
    public string name;
    public string message;
    public string date;
}

