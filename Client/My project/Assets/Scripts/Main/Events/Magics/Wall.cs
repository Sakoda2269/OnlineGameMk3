using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using Newtonsoft.Json;

public class Wall : MagicBase
{
    
    float wallSize = 0.01f;
    
    public Wall(){
        maxCooldown = 100;
        mp = 20;
        life = 600;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate(){
        wallSize += wallSize < 2 ? 0.04f : 0;
        transform.localScale = new Vector3(2f, wallSize, 1f);
        life -= 1;
        if(life <= 0){
            Destroy(this.gameObject);
        }
    }

    override public string GetName(){
        return "wall";
    }

    override public void Use(GameObject user, WebSocket ws){
        Player p = user.GetComponent<Player>();
        SendData sendData;
        sendData.method = "event";
        sendData.id = p.id;
        Vector3 pos = new Vector3(user.transform.forward.x + user.transform.position.x, 
                                    user.transform.position.y, 
                                    user.transform.forward.z + user.transform.position.z);
        sendData.data = new Dictionary<string, string>{
            {"name", GetName()},
            {"pos_x", pos.x.ToString()},
            {"pos_y", pos.y.ToString()},
            {"pos_z", pos.z.ToString()},
            {"rot_x", user.transform.eulerAngles.x.ToString()},
            {"rot_y", user.transform.eulerAngles.y.ToString()},
            {"rot_z", user.transform.eulerAngles.z.ToString()}
        };
        ws.SendText(JsonConvert.SerializeObject(sendData));
    }
}
