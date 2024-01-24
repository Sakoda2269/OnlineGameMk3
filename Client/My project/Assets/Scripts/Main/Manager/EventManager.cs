using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class EventManager : MonoBehaviour
{

    public List<GameObject> events;
    public List<MagicBase> magics = new List<MagicBase>();
    Dictionary<string, GameObject> eventDict = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject obj in events){
            eventDict[obj.GetComponent<EventBase>().GetName()] = obj;
        }
        magics.Add(new FireBall());
        magics.Add(new Leap());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public MagicBase GetMagic(int i){
        return magics[i];
    }

    public void CallEvent(JObject data){
        name = data["data"]["name"].ToString();
        float x = float.Parse(data["data"]["pos"]["x"].ToString());
        float y = float.Parse(data["data"]["pos"]["y"].ToString());
        float z = float.Parse(data["data"]["pos"]["z"].ToString());
        float rx = float.Parse(data["data"]["rot"]["x"].ToString());
        float ry = float.Parse(data["data"]["rot"]["y"].ToString());
        float rz = float.Parse(data["data"]["rot"]["z"].ToString());
        string id = data["id"].ToString();
        if(eventDict.ContainsKey(name)){
            GameObject tmp = Instantiate(eventDict[name], new Vector3(x, y, z), Quaternion.Euler(rx, ry, rz));
            tmp.GetComponent<EventBase>().userId = id;
        }
    }

}
