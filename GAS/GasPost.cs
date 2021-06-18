using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GasPost : MonoBehaviour
{
    private const string URI = "https://script.google.com/macros/s/AKfycbzUqSh54rTIXQzbzl63LJD918kLq4qCzlsWNtIM0vNpxYPkwjDbyva4nfx7HOJkkWhq/exec";
    public InputField inputField;
    [SerializeField] GameObject chatNodePrefab;
    private void Start()
    {
        inputField = inputField.GetComponent<InputField>();
    }
    public void InputText(){
        // 送信先IDとメッセージをPOST
        TalkLINE tl = GetComponent<TalkLINE>();
        string id = tl.sendId;
        string text = inputField.text.TrimEnd('\r','\n');
        if (id != "") StartCoroutine(Post(id, text));
        inputField.text = "";
    }

    public IEnumerator Post(string id, string text)
    {
        postData contents = new postData();
        // 送信先のID（関数で取得）
        contents.userId = id;
        // 送信するメッセージを格納
        contents.message = text; 

        // json形式に変換
        string jsonBody = JsonUtility.ToJson(contents);
        var request = new UnityWebRequest(URI, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
         
        switch (request.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("waiting...");
                break;
            case UnityWebRequest.Result.Success:
                var result = request.downloadHandler.text;
                Debug.Log(result); 
                break;
            default: throw new ArgumentOutOfRangeException(); 
        }
        request.Dispose();

        var scrollView = GameObject.Find("MessageList");
        var viewPort = scrollView.transform.Find("Viewport");
        var content = viewPort.transform.Find("Content");
        var chatNode = Instantiate<GameObject>(chatNodePrefab, content.transform, false);
        var chatNodeLayoutGroup = chatNode.GetComponent<LayoutGroup>();
        var name = chatNode.transform.Find("Name");
        var message = chatNode.transform.Find("Message");
        var chatBoard = message.transform.Find("ChatBoard").GetComponent<Image>();
        var chatText = chatBoard.transform.Find("Text").GetComponent<Text>();
        var date = message.transform.Find("Date");
        var dateObject = date.gameObject;
        var dateText = date.GetComponent<Text>();

        Destroy(name.gameObject);
        chatNodeLayoutGroup.childAlignment = TextAnchor.MiddleRight;
        chatNodeLayoutGroup.padding.left = 40;
        chatBoard.color = new Color(0f, 1f, 0.1f);
        dateObject.transform.SetSiblingIndex(0);
        var now = DateTime.Now.ToString("h:mm");
        dateText.text = now;
        chatText.text = text;
    }

    [System.Serializable]
    public class postData
    {
        public string userId;
        public string message;
    }
}