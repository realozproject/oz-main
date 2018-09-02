using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerChat : NetworkBehaviour {

    [SerializeField] private int chatHistoryLength;
    private Text[] chatHistory;
    [SerializeField]  private int chatSizeX = 1500;
    private InputField inputField;

	// Use this for initialization
	void Start () {
        GameObject canvas = GameObject.Find("Canvas");
        float canvasHeight = canvas.GetComponent<RectTransform>().sizeDelta.y;  //Canvasの高さ取得
        float inputFieldToDown = canvas.transform.Find("InputField").GetComponent<RectTransform>().sizeDelta.y / 2 + canvas.transform.Find("InputField").GetComponent<RectTransform>().anchoredPosition.y;
        //画面下から入力文字フィールドの上部までの距離取得
        float allHistoryHeight = canvasHeight - inputFieldToDown;  //chatHistoryに使用できる幅
        float chatHistoryHeight = allHistoryHeight / ((float)chatHistoryLength + ((float)chatHistoryLength + 1) / 2);  //一つのテキスト履歴に使用できる幅。一つのテキスト履歴の幅を半分にした隙間を作るので(float)chatHistoryLength + 1) / 2を追加する。
        int chatHistoryTextSize = Mathf.FloorToInt(chatHistoryHeight * 9 / 10) - 1;  //文字サイズはテキスト幅の9/10までの整数型なので、切り捨て

        for (int i = 0; i < chatHistoryLength; i++)
        {
            GameObject textObj = new GameObject("Text");
            textObj.name = "Text" + i.ToString();
            textObj.transform.parent = canvas.transform.Find("Texts").transform;
            Text text = textObj.AddComponent<Text>();
            text.rectTransform.anchorMin = new Vector2(0.5f ,0f);
            text.rectTransform.anchorMax = new Vector2(0.5f, 0f);  //アンカー位置調整
            float posy = -(canvasHeight / 2) + inputFieldToDown + (chatHistoryHeight / 2) * (i + 2) + chatHistoryHeight * i + chatHistoryHeight / 2;  //最下部＋InputFieldの上部までの距離＋隙間等
            Vector2 pos = new Vector2(0, posy);
            text.rectTransform.anchoredPosition = pos;  //場所指定
            text.rectTransform.sizeDelta = new Vector2(chatSizeX, chatHistoryHeight);  //サイズ指定
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;  //フォント指定
            text.fontSize = chatHistoryTextSize;  //フォントサイズ指定
            text.color = Color.black;  //色指定
            text.text = "";  //文字指定
        }

        chatHistory = new Text[chatHistoryLength];
        GameObject texts = GameObject.Find("Texts");
        for (int i = 0; i < chatHistory.Length; i++)
        {
            chatHistory[i] = texts.transform.GetChild(i).GetComponent<Text>();
            chatHistory[i].text = "";
        }
	}

    public override void OnStartLocalPlayer()
    {
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer) { return; }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(inputField.text.Length > 0)
            {
                CmdPost(inputField.text);
                inputField.text = "";
            }
        }
        if (Input.GetMouseButtonDown(0)) { RayFire(); }
	}

    void RayFire()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        pointer.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointer, raycastResults);
        /*for(int i = 0; i < chatHistory.Length; i++)
        {
            chatHistory[i].color = Color.black;
        }*/
        for(int i = 0; i < raycastResults.Count; i++)
        {
            if(raycastResults[i].gameObject.transform.parent.gameObject == GameObject.Find("Texts").gameObject)
            {
                CmdColor(raycastResults[i].gameObject.name);
            }
        }
    }

    [Command]
    void CmdPost(string text)
    {
        RpcPost(text);
    }

    [Command]
    void CmdColor(string name)
    {
        RpcColor(name);
    }

    [ClientRpc]
    void RpcPost(string text)
    {
        for(int i = chatHistory.Length-1; i > 0; i--)
        {
            chatHistory[i].text = chatHistory[i - 1].text;
            chatHistory[i].color = chatHistory[i - 1].color;
        }
        chatHistory[0].text = text;
        chatHistory[0].color = Color.black;
    }

    [ClientRpc]
    void RpcColor(string name)
    {
        for(int i = 0; i < chatHistory.Length; i++)
        {
            if(name == "Text" + i.ToString()) { chatHistory[i].GetComponent<Text>().color = Color.cyan; }
        }
    }
}
