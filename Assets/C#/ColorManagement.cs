using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManagement : MonoBehaviour {

    [SerializeField] private GameObject buttonprefab;
    [SerializeField] public List<GameObject> buttons;
    public List<Color> remainColor;

    // Use this for initialization
    void Awake () {
        remainColor = new List<Color>() { Color.red, Color.green, Color.blue };
        buttons = new List<GameObject>();
        Transform parent = GameObject.Find("ColorButtons").transform;
        for(int i = 0; i < remainColor.Count; i++)
        {
            GameObject prefab = (GameObject)Instantiate(buttonprefab);
            prefab.transform.parent = parent;
            buttons.Add(prefab);
        }
    }

    void Start()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponent<RectTransform>().localPosition = new Vector3(-90,-50-i*100,0);
            buttons[i].GetComponent<Image>().color = remainColor[i];
            //buttons[i].GetComponent<Button>().onClick.AddListener(OnClick);
        }
    }
    // Update is called once per frame
    void Update () {
		
	}

    public void OnClick()
    {
        GameObject Player = GameObject.Find("Player(Clone)");
        Debug.Log(Player.name);
    }
}
