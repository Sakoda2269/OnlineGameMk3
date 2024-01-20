using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBar : MonoBehaviour
{

    public List<GameObject> boxes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectBox(int index){
        for(int i = 0; i < 6; i++){
            boxes[i].GetComponent<Box>().selected = false;
        }
        boxes[index].GetComponent<Box>().selected = true;
    }
}
