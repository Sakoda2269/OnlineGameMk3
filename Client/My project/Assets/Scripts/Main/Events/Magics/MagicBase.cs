using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MagicBase : EventBase
{
    protected int cooldown;
    protected int maxCooldown;
    protected int life;
    protected Rigidbody rigid;
    protected bool active;
    public string id;

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
    }

    protected IEnumerator DelayCoroutine(float seconds, UnityAction callback)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }
}
