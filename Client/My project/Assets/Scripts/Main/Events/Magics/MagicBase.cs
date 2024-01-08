using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagicBase : EventBase
{
    protected int cooldown;
    protected int maxCooldown;
    protected int life;
    protected Rigidbody rigid;

    public void CoolDonw(){
        if(0 < cooldown){
            cooldown--;
        }
    }
    public bool CanUse(){
        return cooldown==0 ? true : false;
    }
    public void SetCoolDown(){
        cooldown = maxCooldown;
        Debug.Log(cooldown);
    }
}
