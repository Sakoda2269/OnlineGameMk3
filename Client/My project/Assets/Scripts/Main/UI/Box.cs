using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box : MonoBehaviour
{
    [SerializeField] GameObject white_panel;
    [SerializeField] Slider slider;
    public GameObject panel;
    public bool selected; 
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetCoolTime(float nextUse){
        slider.value = nextUse;
    }

    // Update is called once per frame
    void Update()
    {
        white_panel.SetActive(selected);
        if(selected){
            transform.SetAsLastSibling();
        }

    }
}
