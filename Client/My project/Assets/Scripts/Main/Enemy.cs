using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    public GameObject head;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {

    }

    public void setPos(Vector3 pos){
        Transform myTrans = this.transform;
        myTrans.position = pos;
    }

    public void setRot(Vector3 rot){
        Transform myTrans = this.transform;
        myTrans.localEulerAngles = rot;
    }

    public void setHead(Vector3 head){
        Transform myTrans = this.head.transform;
        myTrans.eulerAngles = head;
    }

    public void setAnim(Dictionary<string, string> param){
        anim.SetFloat("Speed", float.Parse(param["Speed"]));
        anim.SetBool("Jump", param["Jump"].Equals("True"));
        anim.SetBool("Grounded", param["Grounded"].Equals("True"));
        anim.SetBool("FreeFall", param["FreeFall"].Equals("True"));
        anim.SetFloat("MotionSpeed", float.Parse(param["MotionSpeed"]));
    }

    void OnLand(){

    }

    void OnFootstep(){

    }

}
