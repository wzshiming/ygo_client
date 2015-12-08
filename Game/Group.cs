using UnityEngine;
using System.Collections;

public class Group {
    ArrayList list;
    public delegate void eventFunc(Card c);
    public delegate void sortFunc(Card c, int i, int l);
    private string name;
    private Transform master;
    bool shift;

    public bool Shift {
        set {
            if (shift != value) {
                shift = value;
                Flash();
            }
        }
        get {
            return shift;
        }
    }

    public bool CanShift {
        get {
            return sort2 != null;
        }
    }

    public string Name {
        get {
            return name;
        }

        set {
            name = value;
        }
    }

    public Transform Master {
        get {
            return master;
        }

        set {
            master = value;
        }
    }

    sortFunc sort;
    sortFunc sort2;
    eventFunc join, leave;
    bool running;

    public Group(sortFunc s) {

        list = new ArrayList();
        shift = false;
        sort = s;
        sort2 = null;
        join = null;
        leave = null;
        running = false;
    }

    public Group(sortFunc s, sortFunc s2) {

        list = new ArrayList();
        shift = false;
        sort = s;
        sort2 = s2;
        join = null;
        leave = null;
        running = false;
    }

    public Group(sortFunc s, sortFunc s2, eventFunc j) {

        list = new ArrayList();
        shift = false;
        sort = s;
        sort2 = s2;
        join = j;
        leave = null;
        running = false;
    }

    public Group(sortFunc s, sortFunc s2, eventFunc j, eventFunc l) {

        list = new ArrayList();
        shift = false;
        sort = s;
        sort2 = s2;
        join = j;
        leave = l;
        running = false;
    }


    public Group(sortFunc s, eventFunc j) {

        list = new ArrayList();
        shift = false;
        sort = s;
        sort2 = null;
        join = j;
        leave = null;
    }

    public Group(sortFunc s, eventFunc j, eventFunc l) {
        list = new ArrayList();
        shift = false;
        sort = s;
        sort2 = null;
        join = j;
        leave = l;
    }

    public Card PeekIndex(int i) {
        return list[i] as Card;
    }

    public void Join(Card c) {
        c.Pick();
        if (join != null) {
            join(c);
        }
        list.Add(c);
        c.Pos = this;
        Flash();
    }

    public void Leave(Card c) {
        if (c.Pos != this) {
            return;
        }
        if (leave != null) {
            leave(c);
        }
        list.Remove(c);
        c.Pos = null; 
        Flash();
    }

    public void Flash(Card c) {
        var so = sort;
        if (shift && sort2 != null) {
            so = sort2;
        }

        so(c, list.IndexOf(c), list.Count);
    }

    public void Flash() {
        if (!running) {
            running = true;
            var so = sort;
            if (shift && sort2 != null) {
                so = sort2;
            }
            Async.PushDelay(0.1f, () => {
                for (var i = 0; i != list.Count; i++) {
                    var c = list[i] as Card;
                    so(c, i, list.Count);
                }
                running = false;
            });
        }
    }
    public void OnMouseEnter() {
        if (CanShift) {
            Shift = true;
        }
    }
    public void OnMouseExit() {
        if (Shift) {
            Async.PushDelay(0.5f, () => {
                if (Mouse.currentOver == null || Mouse.currentOver.GetComponent<Card>().Pos != this) {
                    Shift = false;
                }
            });
        }
    }

}

