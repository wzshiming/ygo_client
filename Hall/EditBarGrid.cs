using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditBarGrid : MonoBehaviour {
    GameObject CardPrefabs;
    // Use this for initialization
    Dictionary<long, Transform> cs;
    GameObject Card;

    static Vector3 half = new Vector3(0.5f, 0.5f, 1);
    static Vector3 half8 = new Vector3(0.8f, 0.8f, 1);
    void Start() {

        Card = GameObject.Find("CardsBarGrid");
        CardPrefabs = Resources.Load("GameObject/CardItem") as GameObject;
        cs = new Dictionary<long, Transform>();
        Zoom = true;
    }

    // Update is called once per frame
    void Update() {

    }
    bool zoom;
    public bool Zoom {
        set {
            lock (this) {
                if (zoom == value) {
                    return;
                }

                for (var i = 0; i != transform.childCount; i++) {
                    transform.GetChild(i).GetComponent<CardItem>().IsThumb = zoom;
                }
                if (zoom) {
                    iTween.ScaleTo(gameObject, half, 1.0f);
                    var uig = GetComponent<UIGrid>();
                    uig.maxPerLine = 8;
                    uig.cellWidth = 160;
                    uig.cellHeight = 120;
                    uig.enabled = true;

                    //iTween.MoveTo (gameObject, new Vector3 (-0.5f, 0.7f, 0),0.3f);
                } else {
                    iTween.ScaleTo(gameObject, half8, 1.0f);
                    var uig = GetComponent<UIGrid>();
                    uig.maxPerLine = 3;
                    uig.cellWidth = 160;
                    uig.cellHeight = 250;
                    uig.enabled = true;
                    //iTween.MoveTo (gameObject, new Vector3 (-1.0f, 0.7f, 0),0.3f);
                    //iTween.MoveTo (gameObject, new Vector3 (-1.0f, 0.7f, 0),0.3f);
                }
                zoom = !zoom;
            }
        }

    }
    public void ZoomOut() {
        Zoom = false;

    }

    public void ZoomIn() {
        Zoom = true;
    }

    public void Clear() {
        cs.Clear();
        transform.DestroyChildren();
    }

    public ArrayList GetDeck() {
        var a = new ArrayList();
        foreach (KeyValuePair<long, UnityEngine.Transform> de in cs) {
            var h = new Hashtable();
            h.Add("id", de.Key);
            h.Add("size", GetCardSize(de.Key));
            h.Add("pos", 1);
            a.Add(h);
        }
        return a;
    }

    public long GetCardSize(long id) {
        if (!cs.ContainsKey(id)) {
            return 0;
        }
        return cs[id].GetComponent<CardItem>().Size;
    }

    public void OpenDefDeck() {
        OpenDeck(DecksManage.GetDefName());
    }

    public void OpenDeck(string name) {
        Clear();
        var d = DecksManage.GetDeck(name);
        foreach (card c in d) {
            for (var i = 0; i != c.size; i++) {
                AddCard(c.id);
            }
        }
    }

    public void AddCard(long id) {
        if (!cs.ContainsKey(id)) {
            cs.Add(id, NewCard(id));
            transform.GetComponent<UIGrid>().enabled = true;
        }
        cs[id].GetComponent<CardItem>().AddOne();

    }

    Transform NewCard(long id) {
        var t = Instantiate<GameObject>(CardPrefabs).transform;
        t.SetParent(transform);
        t.GetComponent<CardItem>().IsThumb = !zoom;
        t.localScale = Vector3.one;
        var ci = t.GetComponent<CardItem>();
        ci.Id = id;
        ci.IsThumb = !zoom;
        return t;
    }


    void OnChildClick(long id) {
        var d = transform.parent.GetComponent<Deck>();
        d.OnEditClick(id);
    }
    public void Sub(long id) {
        var ci = cs[id].GetComponent<CardItem>();
        ci.SubOne();
        if (ci.Size == 0) {
            Destroy(cs[id].gameObject);
            cs.Remove(id);
            transform.GetComponent<UIGrid>().enabled = true;
        }

        Card.GetComponent<CardsBarGrid>().Flash();
    }

}
