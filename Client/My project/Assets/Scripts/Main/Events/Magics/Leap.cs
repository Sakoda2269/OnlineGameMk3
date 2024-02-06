using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using Newtonsoft.Json;

public class Leap : MagicBase
{

    public Leap(){
        maxCooldown = 100;
        mp = 20;
    }

    override public string GetName(){
        return "leap";
    }

    override public void Use(GameObject user, WebSocket ws){
        Vector3 forward = user.GetComponent<Player>().magicRoot.transform.forward;
        user.GetComponent<StarterAssets.MyCont>().leaping = true;
        Vector3 moveVector = new Vector3(forward.x, forward.y, forward.z);
        moveVector.Normalize(); 
        user.GetComponent<Rigidbody>().AddForce(30f * moveVector, ForceMode.Impulse);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
