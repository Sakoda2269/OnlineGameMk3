using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using Newtonsoft.Json.Linq;

using UnityEngine.SceneManagement;

public class Join : MonoBehaviour
{

    public static string address;
    string publicAddress;
    string privateAddress;
    

    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath + "/Protected/address.json";
        if(File.Exists(path) is true){
            try{
                using(var stream = new FileStream(path, FileMode.Open)){
                    using(var sr = new StreamReader(stream)){
                        JObject message = JObject.Parse(sr.ReadToEnd());
                        publicAddress = message["public"].ToString();
                        privateAddress = message["private"].ToString();
                    }
                }
            }catch(Exception ex){
                Debug.Log(ex);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClick(){
        SceneManager.LoadSceneAsync("Playground");
    }

    public void PublicJoin(){
        address = publicAddress;
        SceneManager.LoadSceneAsync("Playground");
    }

    public void PrivateJoin(){
        address = privateAddress;
        SceneManager.LoadSceneAsync("Playground");
    }

    public void Localhost(){
        address = "localhost:8000";
        SceneManager.LoadSceneAsync("Playground");
    }

}
