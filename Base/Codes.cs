using UnityEngine;
using System.Collections;

public class Codes : MonoBehaviour {

    static public object DeJson(string s) {
        return Json.jsonDecode(s);
    }

    static public string EnJson(object o) {

        return Json.jsonEncode(o);
    }

    static public byte[] GetBytes(string s) {
        return System.Text.Encoding.Default.GetBytes(s);
    }

    static public string GetString(byte[] s) {
        return System.Text.Encoding.Default.GetString(s);
    }

    static public string DeBase64(string s) {
        return System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(s));
    }

    static public string EnBase64(string s) {
        return System.Text.Encoding.Default.GetString(System.Convert.FromBase64String(s));
    }

    static public string deBase64(byte[] s) {
        return System.Convert.ToBase64String(s);
    }

    static public byte[] enBase64(string s) {
        return System.Convert.FromBase64String(s);
    }

    static public ushort DeUint16(byte[] b) {
        var m1 = (ushort)b[1];
        var m2 = ((ushort)b[0]) << 8;
        return (ushort)(m1 | m2);
    }

    static public byte[] EnUint16(ushort m) {
        var arry = new byte[2];
        arry[1] = (byte)(m & 0xFF);
        arry[0] = (byte)((m & 0xFF00) >> 8);
        return arry;
    }

    static public byte[] EnCodeStream(byte[] m, object o) {
        var s = EnJson(o);
        var b = GetBytes(s);
        var len = EnUint16((ushort)(b.Length + 4));
        byte[] bytes = new byte[b.Length + 6];
        len.CopyTo(bytes, 0);
        m.CopyTo(bytes, 2);
        b.CopyTo(bytes, 6);
        return bytes;
    }

    static public long ToInt(object o) {
        if (o == null) {
            return 0;
        } else if (o is double) {
            return System.Convert.ToInt64((double)o);
        } else if (o is string) {
            return System.Convert.ToInt64((string)o);
        }

        return System.Convert.ToInt64(o);
    }


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}

