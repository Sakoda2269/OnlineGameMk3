using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MagicBase
{

    public FireBall(){
        maxCooldown = 50;
    }
    // Start is called before the first frame update
    void Start()
    {
        life = 100;
        rigid = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        if(life > 0) life--;
        if(life == 0){
            Destroy(this.gameObject);
        }
        rigid.AddForce(200 * this.transform.forward);
    }

    override public string GetName(){
        return "fireball";
    }   
}
