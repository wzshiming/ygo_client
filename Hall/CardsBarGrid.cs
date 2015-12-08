using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class CardsBarGrid : MonoBehaviour {
    // Use this for initialization

    long page = 0;
    GameObject CardPrefabs;
    // Use this for initialization
    //int size = 8;
    GameObject Edit;
    GameObject Index;
    GameObject Max;
    GameObject inp;

    List<cbg> finer;

    public GameObject NewCardItem() {


        var g = Instantiate(CardPrefabs, transform.position, transform.rotation) as GameObject;
        return g;
    }

    void Start() {
        Index = GameObject.Find("Index");
        Max = GameObject.Find("Max");
        Edit = GameObject.Find("EditBarGrid");
        inp = GameObject.Find("Input");
        CardPrefabs = Resources.Load("GameObject/CardItem") as GameObject;
        finer = new List<cbg>();

    }

    HashSet<string> attr, race, type;
    void makeIndex() {
        var cc = DecksManage.GetCards();
        foreach (var c in cc) {
            var inf = Buffer.GetInfo(c.id);
            type.Add(inf.type);
            if (inf.attr != "") {
                attr.Add(inf.attr);
            }
            if (inf.race != "") {
                race.Add(inf.race);
            }
        }
    }

    public void GotoForInput() {
        var i = Index.GetComponent<UILabel>().text;
        if (i == "") {
            i = "0";
        }
        JumpPage(Codes.ToInt(i) - 1);
    }


    public delegate bool cardid(long id);

    public void Finds(cardid ci) {
        var cc = DecksManage.GetCards();
        Async.Push(() => {
            finer.Clear();
            foreach (var c in cc) {
                if (ci(c.id)) {
                    finer.Add(c);
                }
            }
            Max.GetComponent<UILabel>().text = string.Format("{0}", finer.Count / transform.childCount + (finer.Count % transform.childCount != 0 ? 1 : 0));
            JumpPage(0);
        });
    }

    public void Finds(string name) {
        var cc = DecksManage.GetCards();
        finer.Clear();
        if (name != null && name != "") {
            foreach (var c in cc) {
                var inf = Buffer.GetInfo(c.id);
                if (inf.name.IndexOf(name) > -1) {
                    finer.Add(c);
                }
            }
        } else {
            foreach (var c in cc) {
                finer.Add(c);
            }
        }
        Max.GetComponent<UILabel>().text = string.Format("{0}", finer.Count / transform.childCount + (finer.Count % transform.childCount != 0 ? 1 : 0));
        JumpPage(0);
    }

    public void FindForInput() {
        Finds((i) => {
            return true;
        });
    }

    public void Flash() {
        JumpPage(page);
    }

    public void JumpPage(long s) {

        if (s < 0 || s * transform.childCount > finer.Count) {
            return;
        }
        for (var i = 0; i != transform.childCount; i++) {
            var ob = transform.GetChild(i);
            long id = 0;
            var index = (int)(i + s * transform.childCount);
            if (index < finer.Count) {
                id = finer[index].id;
            }
            var sr = ob.GetComponent<CardItem>();
            sr.Id = id;

            var y = Edit.GetComponent<EditBarGrid>().GetCardSize(id);
            if (id != 0) {

                ob.gameObject.SetActive(true);
                ob.GetChild(2).gameObject.SetActive(true); // new mark flag
                sr.Size = 3 - y;
                sr.Max = 3;
            } else {
                ob.gameObject.SetActive(false);
                sr.Size = 0;
                sr.Max = 0;
            }
        }
        Index.GetComponent<UILabel>().text = string.Format("{0}", s + 1);
        transform.GetComponent<UIGrid>().enabled = true;
        page = s;
    }

    public void NextPage() {
        JumpPage(page + 1);
    }

    public void PrevPage() {
        JumpPage(page - 1);
    }

    void NewCard(long id) {
        var t = NewCardItem().transform;
        t.SetParent(transform);
        t.localScale = Vector3.one;
    }

    void OnChildClick(long id) {
        Edit.GetComponent<EditBarGrid>().AddCard(id);
        Flash();
    }

    public void Filter() {
        Ok.ShowFilter();
    }
}
