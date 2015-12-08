using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Base : MonoBehaviour {

    public static string Sputs(object o) {
        var ret = "";
        if (o is Hashtable) {
            var h = o as Hashtable;
            ret += "{";
            foreach (DictionaryEntry de in h) {
                ret += de.Key.ToString();
                ret += ":";
                ret += Sputs(de.Value);
                ret += ",";
            }
            ret += "}";
        } else if (o is ArrayList) {
            var h = o as ArrayList;
            ret += "[";
            foreach (object ob in h) {
                ret += Sputs(ob);
                ret += ",";
            }
            ret += "]";
        } else {
            ret += o.ToString();
        }

        return ret;
    }

}

