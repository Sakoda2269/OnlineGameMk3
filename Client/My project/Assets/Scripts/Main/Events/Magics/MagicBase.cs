using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
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
    public Texture2D texture;
    public int mp;

    #if UNITY_EDITOR
        private readonly string DATAPATH = "Assets/Images/";
    #else
        private string DATAPATH = $"{Application.dataPath}/Images/";
    #endif


    public Texture2D GetTexture(){
        var rawData = System.IO.File.ReadAllBytes(DATAPATH + GetName() + ".png");
        Texture2D texture2D = new Texture2D(0, 0);
        texture2D.LoadImage(rawData);
        return texture2D;
    }

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
