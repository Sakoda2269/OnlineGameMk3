using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMagic : MagicBase
{
    

    public TestMagic(){
        maxCooldown = 100;
    }

    void Start()
    {
        life = 100;
        rigid = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(life > 0) life--;
        if(life == 0){
            Destroy(this.gameObject);
        }
        rigid.AddForce(1000 * this.transform.forward);

    }

    override public string GetName(){
        return "testMagic";
    }
}
