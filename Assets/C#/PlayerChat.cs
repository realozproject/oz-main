using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
            float posy = -(canvasHeight / 2) + inputFieldToDown + (chatHistoryHeight / 2) * (i + 1) + chatHistoryHeight * i + chatHistoryHeight / 2;  //最下部＋InputFieldの上部までの距離＋隙間等
            Vector2 pos = new Vector2(0, posy);
            text.rectTransform.anchoredPosition = pos;  //場所指定
            text.rectTransform.sizeDelta = new Vector2(chatSizeX, chatHistoryHeight);  //サイズ指定
            text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;  //フォント指定
            text.fontSize = chatHistoryTextSize;  //フォントサイズ指定
            text.color = Color.black;  //色指定

            text.text = i.ToString();  //文字指定
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
