using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using NativeWebSocket;
using Newtonsoft.Json;

public abstract class MagicBase : EventBase
{
    protected int cooldown;
    protected int maxCooldown;
    protected int life;
    protected Rigidbody rigid;
    protected bool active;
    protected int damage;
    public int mp;


    public void CoolDown(){
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
    public float CulcNextUse(){
        return (float)cooldown/(float)maxCooldown;
    }

    public abstract void Use(GameObject user, WebSocket ws);


    protected IEnumerator DelayCoroutine(float seconds, UnityAction callback)
    {
        yield return new WaitForSeconds(seconds);
        callback?.Invoke();
    }
}
