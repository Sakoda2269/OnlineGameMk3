using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

using NativeWebSocket;

public class PlayerManager : MonoBehaviour
{

    WebSocket ws;

    public GameObject player;
    public GameObject enemy;

    Dictionary<string, GameObject> players;
    Queue<string[]> instantQue;

    public Player myPlayer;

    public struct IdPos{
        public string id;
        public Vector3 pos;

        public IdPos(string id, Vector3 pos){
            this.id = id;
            this.pos = pos;
        }
    }
    Queue<IdPos> posQue;

    string myid;

    public void OthrePlayerJoin(string id){
        Debug.Log("other joined!");
        string[] tmp = new string[2];
        tmp[0] = id;
        tmp[1] = "enemy";
        instantQue.Enqueue(tmp);
    }

    public void OtherPlayerLeave(string id){
        Debug.Log(id + " left!");
        Destroy(players[id]);
        players.Remove(id);
    }

    public void MyPlayerJoin(string id, JArray other_players){
        Debug.Log("you joined!");
        string[] tmp = new string[2];
        tmp[0] = id;
        tmp[1] = "player";
        instantQue.Enqueue(tmp);
        myid = id;
        for(int i = 0; i < other_players.Count; i++){
            string[] tmp_othre = new string[2];
            tmp_othre[0] = other_players[i].ToString();
            tmp_othre[1] = "enemy";
            instantQue.Enqueue(tmp_othre);
        }
    }

    public void OtherPlayerPosChange(string id, Vector3 pos, Vector3 rot, Vector3 head){
        // posQue.Enqueue(new IdPos(id, pos));
        if(id.Equals(myid)) return;
        if(players.ContainsKey(id)){
            players[id].GetComponent<Enemy>().setPos(pos);
            players[id].GetComponent<Enemy>().setRot(rot);
            players[id].GetComponent<Enemy>().setHead(head);
        }
    }

    public void OthrePlayerAnimation(string id, Dictionary<string, string> data){
        if(id.Equals(myid)) return;
        if(players.ContainsKey(id)){
            players[id].GetComponent<Enemy>().setAnim(data);
        }
    }

    public void MyPlayerUpdate(WebSocket ws){
        if(players.ContainsKey(myid)){
            players[myid].GetComponent<Player>().Send(ws);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        players = new Dictionary<string, GameObject>();
        instantQue = new Queue<string[]>();
        posQue = new Queue<IdPos>();
        myid = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(instantQue.Count > 0){
            string[] tmp = instantQue.Dequeue();
            Debug.Log(tmp[0]);
            if(tmp[1].Equals("player")){
                players[tmp[0]] = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
                players[tmp[0]].GetComponent<Player>().id = tmp[0];
                myPlayer = players[tmp[0]].GetComponent<Player>();
                myPlayer.SetMagic(0, this.GetComponent<EventManager>().GetMagic(1));
            }
            if(tmp[1].Equals("enemy")){
                players[tmp[0]] = Instantiate(enemy, new Vector3(0, 0, 0), Quaternion.identity);
                players[tmp[0]].GetComponent<Enemy>().id = tmp[0];
            }
        }

        if(posQue.Count > 0){
            IdPos idpos = posQue.Dequeue();
            if(!idpos.id.Equals(myid)){
                players[idpos.id].GetComponent<Enemy>().setPos(idpos.pos);
            }
            
        }
    }
}
