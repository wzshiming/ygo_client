using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CardFilter : MonoBehaviour {

    // Use this for initialization
    //string[] type, attr, race;
    //Dictionary<string,string>  inverted;
    Transform tc, rc, ac, nc;
    GameObject card;
    void Start() {
        card = GameObject.Find("CardsBarGrid");
        
        tc = transform.FindChild("Type");
        rc = transform.FindChild("Race");
        ac = transform.FindChild("Attr");
        nc = transform.FindChild("Name");
        //inverted = new Dictionary<string,string > ();

        var tss = new HashSet<string>();
        var rss = new HashSet<string>();
        var ass = new HashSet<string>();
        var cc = card.GetComponent<CardsBarGrid>();

        tss.Add(Locales.Get("cards.AllType"));
        rss.Add(Locales.Get("cards.AllRace"));
        ass.Add(Locales.Get("cards.AllAttr"));

        cc.Finds((i) => {
            var inf = Buffer.GetInfo(i);

            if (inf.type != "") {
                tss.Add(inf.type);
            }
            
            if (inf.race != "") {
                rss.Add(inf.race);
            }

            if (inf.attr != "") {
                ass.Add(inf.attr);
            }
            return true;
        });
        Async.Push(() => {
        
            var tp = tc.GetComponent<UIPopupList>();
            var tl = new List<string>(tss);
            tl.Sort();
            foreach (var s in tl) {
                //var cs = Locales.Get("cards." + s);
                //inverted[s] =cs;
                tp.AddItem(s);
            }
            var rp = rc.GetComponent<UIPopupList>();
            var rl = new List<string>(rss);
            tl.Sort();
            foreach (var s in rl) {
                //var cs = Locales.Get("cards." + s);
                //inverted[s] =cs;
                rp.AddItem(s);
            }
            var ap = ac.GetComponent<UIPopupList>();
            foreach (var s in ass) {
                //var cs = Locales.Get("cards." + s);
                //inverted[s] =cs;
                ap.AddItem(s);
            }
            Reset();
        });
        
    }
    //	string Inverted(string s){
    //		if (inverted.ContainsKey (s)) {
    //			return inverted[s];
    //		}
    //		return s;
    //	}

    public void Changed() {
        var t = tc.GetComponent<UIPopupList>().value;
        tc.GetChild(0).GetComponent<UILabel>().text = t;
        var r = rc.GetComponent<UIPopupList>().value;
        rc.GetChild(0).GetComponent<UILabel>().text = r;
        var a = ac.GetComponent<UIPopupList>().value;
        ac.GetChild(0).GetComponent<UILabel>().text = a;
        var n = nc.GetComponent<UIInput>().value;
        var cc = card.GetComponent<CardsBarGrid>();

        var at = Locales.Get("cards.AllType");
        var ar = Locales.Get("cards.AllRace");
        var aa = Locales.Get("cards.AllAttr");
        cc.Finds((i) => {
            var inf = Buffer.GetInfo(i);
            if (n != "" && inf.name.IndexOf(n) < 0) {
                return false;
            }

            if (t != "" && t != at && inf.type != t) {
                return false;
            }
            if (r != "" && r != ar && inf.race != r) {
                return false;
            }
            if (a != "" && a != aa && inf.attr != a) {
                return false;
            }
            return true;
        });
    }
    public void Reset() {
        var at = Locales.Get("cards.AllType");
        var ar = Locales.Get("cards.AllRace");
        var aa = Locales.Get("cards.AllAttr");
        tc.GetComponent<UIPopupList>().value = at;
        rc.GetComponent<UIPopupList>().value = ar;
        ac.GetComponent<UIPopupList>().value = aa;
        nc.GetComponent<UIInput>().value = "";

    }
    // Update is called once per frame

    //	public Dictionary<string,string> Get(){
    //		var ds = new Dictionary<string,string> ();
    //		var tp = tc.GetComponent<UIPopupList> ();
    //		ds.Add ("Type", tp.value);
    //		var rp = rc.GetComponent<UIPopupList> ();
    //		ds.Add ("Race", tp.value);
    //		var ap = ac.GetComponent<UIPopupList> ();
    //		ds.Add ("Attr", tp.value);
    //		var nl = nc.GetComponent<UIInput> ();
    //		ds.Add ("Name", nl.value);
    //		return ds;
    //	}
    void Update() {

    }
}

