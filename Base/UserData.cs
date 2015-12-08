using UnityEngine;
using System.Collections;

public class UserData {

    public static Hashtable UserDatas = new Hashtable();

    public static Hashtable Get(Transform t) {
        var id = t.GetInstanceID();
        if (!UserDatas.Contains(id)) {
            UserDatas.Add(id, new Hashtable());
        }
        return UserDatas[t.GetInstanceID()] as Hashtable;
    }

}

