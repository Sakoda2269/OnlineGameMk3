using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireBall : MagicBase
{

    Vector3 dir;
    [SerializeField] private ParticleSystem particle;

    public FireBall(){
        maxCooldown = 50;
    }
    // Start is called before the first frame update
    void Start()
    {
        life = 100;
        rigid = this.GetComponent<Rigidbody>();
        dir = this.transform.forward;
        rigid.AddForce(75 * dir, ForceMode.Impulse);
        active = true;
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
        // rigid.AddForce(100 * dir);
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
