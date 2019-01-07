using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseMessage  {

    public long frame;
    public int id;
    public int id1;
    public string name;

    public BaseMessage(){
        id = 999;
        id1 = 99999;
        name = "dddddddddddd";
    }

}
