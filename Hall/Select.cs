using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Select : MonoBehaviour {
    Dictionary<string, ArrayList> decks = new Dictionary<string, ArrayList>();
    GameObject deck;
    void Start() {
        deck = GameObject.Find("LookBarGrid");
    }

    public void LoadDecks() {
        Sockets.Client.CallBack("Chan.Room.GetDecksInfo", iTween.Hash(), (Hashtable hash) => {
            var a = hash["list"] as ArrayList;
            var up = transform.GetComponent<UIPopupList>();
            up.Clear();
            decks.Clear();
            if (a == null) {
                return;
            }
            foreach (Hashtable h in a) {
                var nam = h["name"] as string;
                var mai = h["main"] as ArrayList;
                var arr = new ArrayList();
                up.AddItem(nam);
                foreach (Hashtable h1 in mai) {
                    var c = new card();

                    c.id = Codes.ToInt(h1["id"]);
                    c.size = Codes.ToInt(h1["size"]);
                    c.pos = Codes.ToInt(h1["pos"]);
                    arr.Add(c);
                }
                decks[nam] = arr;
            }
            var def = hash["def"] as string;
            SelectFor(def);
        });
    }

    public void SelectFor(string name) {
        transform.GetChild(0).GetComponent<UILabel>().text = name;
        var dbg = deck.GetComponent<EditBarGrid>();
        dbg.Clear();
        foreach (card c in decks[name]) {
            for (var i = 0; i != c.size; i++) {
                dbg.AddCard(c.id);
            }
        }

    }
    public void Selects() {
        SelectFor(transform.GetComponent<UIPopupList>().value);
    }

}
