using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Timers;
using System;
public class Cbs : MonoBehaviour {
    public delegate void HashFunc(Hashtable h);

    public static long step;
    public static bool self;

    Dictionary<string, HashFunc> cbs;
    long selfIndex;
    GameObject[] duels;
    GameObject[] players;
    string[] names;
    // Use this for initialization
    GameObject maincamera;

    GameObject chat;

    DateTime s;

    //Timer time;

    public GameObject Cd, Wt, Nc, Dp, Sp, Mp1, Bp, Mp2, Ep, St;

    public GameObject Flag0, Flag1;


    public int re;
    void Start() {
        chat = GameObject.Find("Chat Area");
        maincamera = GameObject.Find("Camera");
        cbs = new Dictionary<string, HashFunc>();

        cbs.Add("init", (args) => {
            selfIndex = Codes.ToInt(args["index"]);
            var users = args["users"] as ArrayList;
            duels = new GameObject[users.Count];
            players = new GameObject[users.Count];
            names = new string[users.Count];
            for (var i = 0; i != users.Count; i++) {
                var j = i + selfIndex;
                if (j >= users.Count) {
                    j -= users.Count;
                }
                duels[i] = GameObject.Find("Duel" + j);
                players[i] = GameObject.Find("Player" + j);
                var h = users[i] as Hashtable;
                var hp = Codes.ToInt(h["hp"]);
                var name = h["name"] as String;
                names[i] = name;
                players[i].transform.GetChild(0).GetComponent<UILabel>().text = name;
                players[i].transform.GetChild(1).GetComponent<UILabel>().text = hp.ToString();
            }

        });

        cbs.Add("changeHp", (args) => {
            var i = Codes.ToInt(args["master"]);
            var hp = Codes.ToInt(args["hp"]);
            var ul = players[i].transform.GetChild(1);
            ul.GetComponent<UILabel>().text = hp.ToString();

            iTween.PunchScale(ul.gameObject, Vector3.one, 2.0f);
        });

        cbs.Add("over", (args) => {
            Init.Ret();
        });


        cbs.Add("onTouch", (args) => {
            var uniq = Codes.ToInt(args["uniq"]);
            var c = Card.Get(uniq);
            c.Remind = true;
        });

        cbs.Add("offTouch", (args) => {
            var uniq = Codes.ToInt(args["uniq"]);
            var c = Card.Get(uniq);
            c.Remind = false;
        });


        cbs.Add("moveCard", (args) => {
            var i = Codes.ToInt(args["master"]);
            var uniq = Codes.ToInt(args["uniq"]);
            var pos = args["pos"] as string;
            moveTo(i, uniq, pos);
        });

        cbs.Add("setFront", (args) => {
            var uniq = Codes.ToInt(args["uniq"]);
            var desk = Codes.ToInt(args["desk"]);

            Card.Get(uniq).Id = desk;
        });

        cbs.Add("exprCard", (args) => {
            var uniq = Codes.ToInt(args["uniq"]);
            var expr = Codes.ToInt(args["expr"]);

            bool up = false;
            if ((expr & (1 << 30)) != 0) {
                up = true;
            } else if ((expr & (1 << 29)) != 0) {
                up = false;
            }
            if ((expr & (1 << 28)) != 0) {
                if (up) {
                    Card.Get(uniq).FaceUpAttack();

                } else {
                    Card.Get(uniq).FaceDownAttack();
                }

            } else if ((expr & (1 << 27)) != 0) {
                if (up) {
                    Card.Get(uniq).FaceUpDefense();
                } else {
                    Card.Get(uniq).FaceDownDefense();
                }
            }
        });

        Dictionary<long, bool> tri = new Dictionary<long, bool>();
        cbs.Add("setPick", (args) => {
            var uniq = args["uniqs"] as ArrayList;
            //var sole = args["sole"] as string;
            var use = args["use"] as string;
            foreach (object un in uniq) {
                var u = Codes.ToInt(un);
                var c = Card.Get(u);
                c.Rim = true;
                c.use = use;
                //c.use = use;

                tri.Add(u, true);
                if (c.Pos.Name == "deck") {
                    Async.PushDelay(0.2f, () => {
                        if (c.Pos.Name == "deck") {
                            moveTo( c.Uniq, "select");
                        }
                    });
                }
            }
        });

        cbs.Add("cloPick", (args) => {
            foreach (KeyValuePair<long, bool> de in tri) {
                var c = Card.Get(de.Key);
                c.use = "";
                //c.use = "";
                c.Rim = false;
            }
            foreach (KeyValuePair<long, bool> de in tri) {
                var c = Card.Get(de.Key);
                if (c.Pos.Name == "select") {
                    moveTo(c.Uniq, "deck");
                }
            }
            tri.Clear();
        });

        cbs.Add("setCardFace", (args) => {
            var uniq = Codes.ToInt(args["uniq"]);
            var param = args["params"] as Hashtable;
            var r = "";
            foreach (DictionaryEntry de in param) {
                var k = de.Key;
                var v = de.Value;
                r += String.Format("{0}:{1}\n", k, v);
            }
            var c = Card.Get(uniq);
            c.Append = r;
        });

        cbs.Add("setPortrait", (args) => {
            var uniq = Codes.ToInt(args["uniq"]);
            var desk = Codes.ToInt(args["desk"]);

            var c = Card.Get(uniq);
            c.Portrait = desk;


        });

        cbs.Add("message", (args) => {
            var param = args["params"] as Hashtable;
            var message = args["format"] as string;
            message = Locales.Get(message);


            foreach (DictionaryEntry de in param) {
                var k = de.Key as String;

                if (de.Value is string) {
                    var ss = de.Value as string;
                    var kk = de.Key as string;
                    if (kk == "self" || kk == "rival") {
                        if (ss == names[selfIndex]) {
                            ss = string.Format(" 「{0}」 ",Locales.Get("msg.You"));
                        }else {
                            ss = string.Format(" {0}「{1}」 ", Locales.Get("msg.Player"), ss);
                        }
                    }
                    message = message.Replace("{" + k + "}", ss);
                } else {
                    var i = Codes.ToInt(de.Value);
                    if (Card.IsExist(i)) {
                        var info = Buffer.GetInfo(Card.Get(i).Id);
                        var v = string.Format(" {0}「{1}」 ", info.type, info.name);
                        if (v == null) {
                            continue;
                        }
                        message = message.Replace("{" + k + "}", v);
                    }
                }
            }
            Debug.Log(message);
            chat.GetComponent<UITextList>().Add(message);
        });

        s = DateTime.Now;

        //time = new Timer(1000);

        //time.Elapsed += new ElapsedEventHandler((source, e) => {
        //    var ss = s.Subtract(DateTime.Now);
//
        //    Async.Push(() => {
        //        Cd.GetComponent<UILabel>().text = string.Format("{0}:{1}", ss.Minutes, ss.Seconds);
        //        iTween.PunchScale(Cd.gameObject, Vector3.one, 0.9f);
        //    });
        //});

        Async.PuahRound(1.0f, () => {
            var ss = s.Subtract(DateTime.Now);
            Cd.GetComponent<UILabel>().text = string.Format("{0}:{1}", ss.Minutes, ss.Seconds);
            iTween.PunchScale(Cd.gameObject, Vector3.one, 0.9f);
        });

        //time.AutoReset = true;
        //time.Enabled = true;

        cbs.Add("flagStep", (args) => {
            step = Codes.ToInt(args["step"]);
            var wait = Codes.ToInt(args["wait"]) / 1000000000;

            var master = Codes.ToInt(args["master"]);
            self = (selfIndex == master);

            s = DateTime.Now.AddSeconds(wait);
            //Cd.GetComponent<UILabel>().text = string.Format("{0} S",wait);
            GameObject flag;
            GameObject flagt;
            if (selfIndex == master) {
                flag = Flag0;
                flagt = Flag1;
            } else {
                flag = Flag1;
                flagt = Flag0;
            }
            iTween.MoveTo(flagt, Wt.transform.position, 0.2f);
            Nc.GetComponent<UILabel>().text = Locales.Get("ui.Chain");
            if (step == 0) {
                iTween.MoveTo(flag, Nc.transform.position, 0.2f);
                if (self) {
                    Nc.GetComponent<UILabel>().text = Locales.Get("ui.NoChain");
                    iTween.PunchScale(Nc.gameObject, Vector3.one, 2.0f);
                }
                //Nc.SetActive(true);
            } else {
                //Nc.SetActive(false);
                for (var i = 0; i != St.transform.childCount; i++) {
                    var b = St.transform.GetChild(i);
                    if (i == step - 1) {
                        iTween.MoveTo(flag, b.position, 0.2f);
                        b.GetComponent<UIButton>().isEnabled = true;
                        iTween.PunchScale(b.gameObject, Vector3.one, 2.0f);
                    } else if ((i == step) || (i == 5 && step > 1)) {
                        b.GetComponent<UIButton>().isEnabled = true;
                    } else {
                        b.GetComponent<UIButton>().isEnabled = false;
                    }
                }

            }

        });
        cbs.Add("impact", (args) => {
            var uniq = Codes.ToInt(args["uniq"]);
            var target = Codes.ToInt(args["target"]);
            var c1 = Card.Get(uniq);
            var c2 = Card.Get(target);

            iTween.MoveTo(c1.gameObject, c2.transform.position, 0.5f);
            Async.PushDelay(0.5f, () => {
                c2.Flash();
                c1.Flash();
            });

        });

        Async.PushDelay(0.5f, () => {
            re = Sockets.Client.Register("Chan.Room.GameRegister", iTween.Hash(), (hash) => {

                if (hash != null && hash.Contains("method")) {
                    var k = hash["method"] as string;
                    var args = hash["args"] as Hashtable;
                    if (cbs.ContainsKey(k)) {
                        try {
                            cbs[k](args);
                        } catch (Exception ex) {
                            Debug.Log(ex.Message);
                        }
                    } else {
                        Debug.Log(k);
                    }
                }
            });
        });
    }



    public void send(long i, long j) {
        Sockets.Client.Send("Chan.Room.GameCardActionSelectable", iTween.Hash("uniq", i, "method", j));
    }

    public void NoChain() {
        send(0, 11);
    }
    public void BP() {
        send(0, 4);
    }
    public void MP2() {
        send(0, 5);
    }
    public void EP() {
        send(0, 6);
    }

    public void Lose() {
        send(0, 666);
    }



    void moveTo(Transform m, Card c, string pos) {
        c.transform.localScale = Vector3.one;
        m.GetComponent<Area>().Move(c, pos);
    }

    void moveTo( long u, string pos) {
        var c = Card.Get(u);
        var m = c.Pos.Master;
        moveTo(m, c, pos);
    }
    void moveTo(long i, long u, string pos) {
        var c = Card.Get(u);
        var m = duels[i].transform;
        moveTo(m, c, pos);
    }
    void OnDestroy() {
        //time.Close();
        Sockets.Client.Unregister(re);
    }
}