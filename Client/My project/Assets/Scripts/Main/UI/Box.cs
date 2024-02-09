using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Box : MonoBehaviour
{
    [SerializeField] GameObject white_panel;
    [SerializeField] Slider slider;
    public GameObject panel;
    public bool selected; 

    string address = Join.address;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetTexture(string name){
        StartCoroutine(GetTex(name));
    }

    IEnumerator GetTex(string name){
        string URI = "http://" + address + "image/";
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(URI + name + "/");
        Debug.Log("dk;jlfds");
        yield return www.SendWebRequest();
        Texture2D texture = new Texture2D(0, 0);
        if (www.isNetworkError || www.isHttpError){
            Debug.Log(www.error);
        }
        else{
            texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
        panel.GetComponent<Image>().sprite = Sprite.Create(
                    texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero
        );
    }


    public void SetCoolTime(float nextUse){
        slider.value = nextUse;
    }

    // Update is called once per frame
    void Update()
    {
        white_panel.SetActive(selected);
        if(selected){
            transform.SetAsLastSibling();
        }

    }
}
