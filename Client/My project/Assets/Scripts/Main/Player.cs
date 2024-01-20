using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using Newtonsoft.Json; 

public class Player : Entity
{

    MagicBase[] magics = new MagicBase[6];
    int select = 0;
    public GameObject hotBar;

    public GameObject head;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float wh = Input.GetAxis("Mouse ScrollWheel");
        if(wh < 0){
            select += 1;
            select %= 6;
        }
        if(wh > 0){
            select -= 1;
            if(select < 0){
                select = 5;
            }
        }
        hotBar.GetComponent<HotBar>().SelectBox(select);
    }

    void FixedUpdate(){
        foreach(MagicBase m in magics){
            if(m is MagicBase mm){
                mm.CoolDonw();
            }
            
        }
    }

    public void SetMagic(int i, MagicBase mb){
        magics[i] = mb;
    }

    public void OnClick(WebSocket ws){
        if(magics[select] is MagicBase select_magic){
            if(select_magic.CanUse()){
                Transform mytrans = magicRoot.transform;
                SendData sendData;
                sendData.method = "event";
                sendData.id = id;
                sendData.data = new Dictionary<string, string>{
                    {"name", select_magic.GetName()},
                    {"pos_x", mytrans.position.x.ToString()},
                    {"pos_y", mytrans.position.y.ToString()},
                    {"pos_z", mytrans.position.z.ToString()},
                    {"rot_x", mytrans.eulerAngles.x.ToString()},
                    {"rot_y", mytrans.eulerAngles.y.ToString()},
                    {"rot_z", mytrans.eulerAngles.z.ToString()}
                };
                ws.SendText(JsonConvert.SerializeObject(sendData));
                select_magic.SetCoolDown();
            }
        }
    }

    public void Send(WebSocket ws){
        SendPos(ws);
        SendAnim(ws);
    }

    public void SendPos(WebSocket ws){
        Transform mytrans = this.transform;
        SendData sendData;
        sendData.method = "update";
        sendData.id = id;
        sendData.data = new Dictionary<string, string>{
            {"pos_x", mytrans.position.x.ToString()},
            {"pos_y", mytrans.position.y.ToString()},
            {"pos_z", mytrans.position.z.ToString()},
            {"rot_x", mytrans.localEulerAngles.x.ToString()},
            {"rot_y", mytrans.localEulerAngles.y.ToString()},
            {"rot_z", mytrans.localEulerAngles.z.ToString()},
            {"head_x", head.transform.eulerAngles.x.ToString()}
        };
        ws.SendText(JsonConvert.SerializeObject(sendData));
    }

    public void SendAnim(WebSocket ws){
        SendData sendData;
        sendData.method = "anim_update";
        sendData.id = id;
        sendData.data = new Dictionary<string, string>{
            {"Speed", anim.GetFloat("Speed").ToString()},
            {"Jump", anim.GetBool("Jump").ToString()},
            {"Grounded", anim.GetBool("Grounded").ToString()},
            {"FreeFall", anim.GetBool("FreeFall").ToString()},
            {"MotionSpeed", anim.GetFloat("MotionSpeed").ToString()}
        };
        ws.SendText(JsonConvert.SerializeObject(sendData));
    }
}
