using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using NativeWebSocket;
using Newtonsoft.Json;

public class FireBall : MagicBase
{

    Vector3 dir;
    [SerializeField] private ParticleSystem particle;

    public FireBall(){
        maxCooldown = 50;
        mp = 10;
    }
    // Start is called before the first frame update
    void Start()
    {
        life = 100;
        rigid = this.GetComponent<Rigidbody>();
        dir = this.transform.forward;
        rigid.AddForce(75 * dir, ForceMode.Impulse);
        active = true;
        damage = 10;
    }

    public override void Use(GameObject user, WebSocket ws){
        Player p = user.GetComponent<Player>();
        SendData sendData;
        sendData.method = "event";
        sendData.id = p.id;
        Transform mytrans = p.magicRoot.transform;
        sendData.data = new Dictionary<string, string>{
            {"name", GetName()},
            {"pos_x", mytrans.position.x.ToString()},
            {"pos_y", mytrans.position.y.ToString()},
            {"pos_z", mytrans.position.z.ToString()},
            {"rot_x", mytrans.eulerAngles.x.ToString()},
            {"rot_y", mytrans.eulerAngles.y.ToString()},
            {"rot_z", mytrans.eulerAngles.z.ToString()}
        };
        ws.SendText(JsonConvert.SerializeObject(sendData));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        if(life > 0) life--;
        if(life == 0){
            Remove();
        }
    }

    override public string GetName(){
        return "fireball";
    }

    void OnCollisionEnter(Collision collision)
    {
        if(active){
            if(collision.gameObject.tag.Equals("Untagged")){
                Remove();
            }
            if(collision.gameObject.tag.Equals("Player")){
                if(!collision.gameObject.GetComponent<Player>().id.Equals(userId)){
                    collision.gameObject.GetComponent<Player>().Damage(damage, userId);
                    Remove();
                }
            }
            if(collision.gameObject.tag.Equals("Enemy")){
                if(!collision.gameObject.GetComponent<Enemy>().id.Equals(userId)){
                    Remove();
                }
            }
        }
        
    }


    void Remove(){
        active = false;
        particle.Stop();
        StartCoroutine(DelayCoroutine(2.0f, () =>{
            Destroy(this.gameObject);
        }));
    }
}
