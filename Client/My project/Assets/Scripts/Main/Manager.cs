using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json.Linq;

using NativeWebSocket;

public class Manager : MonoBehaviour
{
    WebSocket ws;
    PlayerManager pm;
    EventManager em;

    async void Start()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        string address = Join.address;
        Debug.Log(address);
        ws = new WebSocket("ws://" + address + "/ws/game/room1");
        pm = this.gameObject.GetComponent<PlayerManager>();
        em = this.gameObject.GetComponent<EventManager>();
        ws.OnOpen += () => {
            Debug.Log("Connection Open!");
        };
        ws.OnError += (e) => {
            Debug.Log("Error! " + e);
        };
        ws.OnClose += (e) => {
            Debug.Log("Connection closed!");
        };


        ws.OnMessage += (bytes) =>{

            var message_in = System.Text.Encoding.UTF8.GetString(bytes);
            JObject message = JObject.Parse(message_in);
            // Debug.Log(message_in);
            string id = message["id"].ToString();
            switch(message["method"].ToString()){
                case "my_join":
                    pm.MyPlayerJoin(id, (JArray)message["data"]["players"]);
                    break;
                case "other_join":
                    pm.OthrePlayerJoin(id);
                    break;
                case "other_leave":
                    pm.OtherPlayerLeave(id);
                    break;
                case "update":
                    float x = float.Parse(message["data"]["pos"]["x"].ToString());
                    float y = float.Parse(message["data"]["pos"]["y"].ToString());
                    float z = float.Parse(message["data"]["pos"]["z"].ToString());
                    float rx = float.Parse(message["data"]["rot"]["x"].ToString());
                    float ry = float.Parse(message["data"]["rot"]["y"].ToString());
                    float rz = float.Parse(message["data"]["rot"]["z"].ToString());
                    float hx = float.Parse(message["data"]["head"]["x"].ToString());
                    pm.OtherPlayerPosChange(id, new Vector3(x, y, z), new Vector3(rx, ry, rz), new Vector3(hx, 0, 0));
                    break;
                case "anim_update":
                    Dictionary<string, string> data = new Dictionary<string, string>{
                        {"Speed", message["data"]["param"]["Speed"].ToString()},
                        {"Jump", message["data"]["param"]["Jump"].ToString()},
                        {"Grounded", message["data"]["param"]["Grounded"].ToString()},
                        {"FreeFall", message["data"]["param"]["FreeFall"].ToString()},
                        {"MotionSpeed", message["data"]["param"]["MotionSpeed"].ToString()}
                    };
                    pm.OthrePlayerAnimation(id, data);
                    break;
                case "event":
                    em.CallEvent(message);
                    break;
                default :
                    break;
            }

        };
        await ws.Connect();
    }

    // Update is called once per frame
    async void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if(Input.GetMouseButton(0)){
            pm.myPlayer.OnClick(ws);
        }
    }

    async void FixedUpdate(){
        if(ws.State == WebSocketState.Open){
            #if !UNITY_WEBGL || UNITY_EDITOR
                ws.DispatchMessageQueue();
            #endif
        }
        pm.MyPlayerUpdate(ws);
    }

    private async void OnApplicationQuit() {
        await ws.Close();
    }

}
