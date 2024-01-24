using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public abstract class EventBase : MonoBehaviour
{

    public string userId;
    public abstract string GetName();



}
