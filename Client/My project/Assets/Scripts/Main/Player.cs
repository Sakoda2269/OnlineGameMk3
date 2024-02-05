using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using Newtonsoft.Json; 
using UnityEngine.UI;

public class Player : Entity
{

    MagicBase[] magics = new MagicBase[6];
    int select = 0;
    public GameObject hotBar;
    public Slider hpSlider;
    public Slider mpSlider;
    public GameObject healBox;

    public GameObject head;
    public GameObject player;

    int maxMp;
    int mp;

    int mpHealTime = 100;
    int maxMpHealTime = 100;

    int healCoolDown = 0;
    int maxHealCoolDown = 100;
    int healing = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        maxHP = 100;
        HP = 100;
        maxMp = 100;
        mp = 100;
    }

    public void Init(){
        for(int i = 0; i < magics.Length; i++){
            if(magics[i] is MagicBase m){
                hotBar.GetComponent<HotBar>().boxes[i].GetComponent<Box>().SetTexture(m.GetName());
            }
        }
        healBox.GetComponent<Box>().SetTexture("heal");
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
        if(Input.GetKey(KeyCode.V) &&  healCoolDown == 0){
            healing = 30;
            healCoolDown = maxHealCoolDown;
        }
    }

    void FixedUpdate(){
        for(int i = 0; i < magics.Length; i++){
            if(magics[i] is MagicBase m){
                m.CoolDown();
                hotBar.GetComponent<HotBar>().SetCoolTime(i, m.CulcNextUse());
            }
        }
        if(healCoolDown > 0){
            healCoolDown -= 1;
        }
        healBox.GetComponent<Box>().SetCoolTime((float)healCoolDown / maxHealCoolDown);

        if(healing > 0){
            HP += 1;
            healing -= 1;
            if(HP > maxHP){
                HP = maxHP;
            }
        }
        hpSlider.value = (float)HP / (float)maxHP;
        mpSlider.value = (float)mp / (float)maxMp;
        if(mpHealTime > 0){
            mpHealTime -= 1;
        }else if(mp < maxMp){
            mp += 1;
        }
    }

    public void SetMagic(int i, MagicBase mb){
        magics[i] = mb;
    }

    public void Damage(int damage, string enemyId){
        HP -= damage;
    }

    public void OnClick(WebSocket ws){
        if(magics[select] is MagicBase select_magic){
            if(select_magic.CanUse() && mp >= select_magic.mp){
                select_magic.Use(player, ws);
                select_magic.SetCoolDown();
                mp -= select_magic.mp;
                mpHealTime = maxMpHealTime;
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
