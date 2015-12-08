using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct cbg {
    public long id;
    public long limit;
    public long state;
}

public struct card {
    public long id;
    public long size;
    public long pos;
}

public static class DecksManage {
    public delegate void Action();
    static DecksManage() {

        Find();
    }


    static cbg[] cards;
    public static cbg[] GetCards() {
        return cards;
    }

    static Dictionary<string, card[]> decks;
    public static card[] GetDeck(string name) {
        return decks[name];
    }
    public static string[] GetNames() {
        var ss = new string[decks.Count];
        decks.Keys.CopyTo(ss, 0);
        return ss;
    }

    static string defdeck = "";
    public static string GetDefName() {
        return defdeck;
    }

    public static void Find() {
        Sockets.Client.CallBack("Chan.Room.CardAll", iTween.Hash(), (Hashtable h) => {
            var b = h["list"] as ArrayList;
            if (b == null) {
                return;
            }
            cards = new cbg[b.Count];
            for (var i = 0; i != b.Count; i++) {
                var v = b[i] as Hashtable;
                cards[i].id = Codes.ToInt(v["id"]);
                cards[i].limit = Codes.ToInt(v["limit"]);
                cards[i].state = Codes.ToInt(v["state"]);
            }

        });
    }

    //	public static void LoadDecks() {
    //		LoadDecks (() => {});
    //	}

    public static void LoadDeck(Action act) {
        Sockets.Client.CallBack("Chan.Room.GetDecksInfo", iTween.Hash(), (Hashtable hash) => {
            var a = hash["list"] as ArrayList;
            if (a == null) {
                return;
            }
            decks = new Dictionary<string, card[]>();
            foreach (Hashtable h in a) {
                var nam = h["name"] as string;
                var mai = h["main"] as ArrayList;
                var arr = new card[mai.Count];

                for (var i = 0; i != mai.Count; i++) {
                    var h1 = mai[i] as Hashtable;

                    arr[i].id = Codes.ToInt(h1["id"]);
                    arr[i].size = Codes.ToInt(h1["size"]);
                    arr[i].pos = Codes.ToInt(h1["pos"]);
                }
                decks[nam] = arr;
            }
            defdeck = hash["def"] as string;
            act();
        });
    }
    public static void UploadDeck(string name, ArrayList deck) {
        Sockets.Client.CallBack("Chan.Room.SaveDeckForName", iTween.Hash("name", name, "main", deck), (Hashtable hash) => {
            var err = hash["error"] as string;
            if (err == null || err == "") {
                Ok.Show("ui.Savefinished");
            } else {
                Ok.Show(err);
            }
            LoadDeck(() => { });
        });
    }
}

