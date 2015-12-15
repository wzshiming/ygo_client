
using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Net;

public class Sockets {
    private static AsyncTCPClient client;
    public static AsyncTCPClient Client {
        get {
            if (client == null) {
                Init();
            }
            return client;
        }
    }

    public static void Init() {



        //var host = "zsms.site";
        //var host = "www.mohegame.com";
        var host = "127.0.0.1";
        var port = 3008;
        client = new AsyncTCPClient(host, port, () => {
            client.WriteMsg(new byte[] { 255, 255, 255, 255 });
            client.ReadMsg((byte[] d) => {
                var m0 = d[0];
                var m1 = d[1];
                var m2 = d[2];
                var m3 = d[3];
                var json = System.Text.Encoding.UTF8.GetString(d, 4, d.Length - 4);
                Debug.Log(json);

                var index = Codes.DeJson(json) as Hashtable;
                //foreach (DictionaryEntry de in index){
                client.om.Add(index);
                //}
                client.ReadLoop((byte[] dd) => {
                    if (dd.Length < 6) {
                        return;
                    }
                    var hand = new byte[] { dd[0], dd[1], dd[2], dd[3] };

                    var i = System.BitConverter.ToInt32(hand, 0);


                    AsyncTCPClient.cb f;

                    if (client.rmaps.ContainsKey(i)) {
                        f = client.rmaps[i];
                    } else if (client.maps.ContainsKey(i)) {
                        f = client.maps[i];
                        client.maps.Remove(i);
                    } else {
                        Debug.Log("miss");
                        return;
                    }

                    var jso = System.Text.Encoding.UTF8.GetString(dd, 4, dd.Length - 4);
                    //Debug.Log (jso);
                    var obj = Json.jsonDecode(jso) as Hashtable;

                    //Debug.Log("loop");
                    Async.Push(() => {
                        f(obj);
                    });
                });
            });
        });
        Async.PushDelay(2.0f, () => {
            if (!client.socket.Connected) {
                Ok.Show("hint.l001");
            }
        });

    }

    public static void DisConn() {
        if (client == null || client.socket == null) {
            return;
        }
        client.socket.Close();
        client = null;
    }
}

public class AsyncTCPClient {

    public orderMap om;
    public Socket socket;

    public delegate void bytefunc(byte[] b);

    public delegate void cb(Hashtable hash);

    public delegate void func();

    public Dictionary<int, cb> maps;
    public Dictionary<int, cb> rmaps;
    public static string Hostname2ip(string hostname) {
        try {
            IPAddress ip;
            if (IPAddress.TryParse(hostname, out ip))
                return ip.ToString();
            else
                return Dns.GetHostEntry(hostname).AddressList[0].ToString();
        } catch (Exception) {
            throw new Exception("IP Address Error");
        }
    }

    public AsyncTCPClient(string addr, int port, func fun) {
        om = new orderMap();
        rmaps = new Dictionary<int, cb>();
        maps = new Dictionary<int, cb>();

        IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(Hostname2ip(addr)), port);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.BeginConnect(ipe, asyncResult => {
            socket.EndConnect(asyncResult);
            fun();
        }, socket);
    }

    private void AsynSend(byte[] data) {
        try {
            socket.BeginSend(data, 0, data.Length, SocketFlags.None, asyncResult => {
                int length = socket.EndSend(asyncResult);
            }, null);
        } catch (Exception ex) {
            Init.First("hint.l001");
            Debug.Log(ex.Message);
        }
    }

    private void AsynRecive(int i, bytefunc func) {
        try {
            byte[] data = new byte[i];
            socket.BeginReceive(data, 0, data.Length, SocketFlags.None, asyncResult => {
                var length = socket.EndReceive(asyncResult);
                if (length < i) {

                    AsynRecive(i - length, (byte[] da) => {
                        da.CopyTo(data, length);
                        func(data);
                    });
                } else {
                    func(data);
                }

            }, null);
        } catch (Exception ex) {
            Init.First("hint.l001");
            Debug.Log(ex.Message);
        }

    }

    public void ReadMsg(bytefunc func) {
        AsynRecive(2, (byte[] d) => {
            var len = Codes.DeUint16(d);
            AsynRecive(len, func);
        });
    }

    public void ReadLoop(bytefunc func) {
        ReadMsg((b) => {
            func(b);
            ReadLoop(func);
        });
    }

    public void WriteMsg(byte[] data) {
        var len = Codes.EnUint16((ushort)data.Length);
        byte[] bytes = new byte[data.Length + 2];
        len.CopyTo(bytes, 0);
        data.CopyTo(bytes, 2);
        AsynSend(bytes);
    }

    public void Send(byte[] hand, object obj) {
        var body = Json.jsonEncode(obj);
        var d = new byte[4 + body.Length];
        hand.CopyTo(d, 0);
        Codes.GetBytes(body).CopyTo(d, 4);
        WriteMsg(d);
    }

    public void Send(string codes, object obj) {
        var hand = om.Get(codes);
        Send(hand, obj);
    }

    public void CallBack(string codes, object obj, cb func) {
        var hand = om.Get(codes);
        Send(hand, obj);
        var i = System.BitConverter.ToInt32(hand, 0);
        if (maps.ContainsKey(i)) {
            maps[i] = func;
        } else {
            maps.Add(i, func);
        }

    }

    public int Register(string codes, object obj, cb func) {
        var hand = om.Get(codes);
        Send(hand, obj);
        var i = System.BitConverter.ToInt32(hand, 0);
        if (rmaps.ContainsKey(i)) {
            rmaps[i] = func;
        } else {
            rmaps.Add(i, func);
        }
        return i;
    }

    public void Unregister(int i) {
        rmaps.Remove(i);
    }
}

public class orderMap {
    Dictionary<string, byte[]> dic;
    byte index;

    public orderMap() {
        dic = new Dictionary<string, byte[]>();
    }

    public void Add(Hashtable h) {
        var list0 = h["CodeMaps"] as ArrayList;
        for (var i = 0; i != list0.Count; i++) {
            var map1 = list0[i] as Hashtable;
            var name1 = map1["Name"];
            var list1 = map1["Classs"] as ArrayList;
            for (var j = 0; j != list1.Count; j++) {
                var map2 = list1[j] as Hashtable;
                var name2 = map2["Name"];
                var list2 = map2["Methods"] as ArrayList;
                for (var k = 0; k != list2.Count; k++) {
                    var name3 = list2[k] as string;
                    var keys = String.Format("{0}.{1}.{2}", name1, name2, name3);
                    dic.Add(keys, new byte[] { (byte)i, (byte)j, (byte)k });
                }
            }
        }
    }

    public byte[] Get(string keys) {
        if (!dic.ContainsKey(keys)) {
            return new byte[] { 255, 255, 0, 0 };
        }
        var ret = new byte[] { index, 0, 0, 0 };
        dic[keys].CopyTo(ret, 1);
        index++;
        return ret;
    }

    public int Count {
        get {
            return dic.Count;
        }
    }
}
