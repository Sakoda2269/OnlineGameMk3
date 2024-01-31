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
        // user.GetComponent<Rigidbody>().AddForce(10 * new Vector3(0, 1, 0), ForceMode.Impulse);
        user.GetComponent<CharacterController>().Move(1.5f * user.GetComponent<Player>().magicRoot.transform.forward);
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
