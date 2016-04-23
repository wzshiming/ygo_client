using UnityEngine;
using System.Collections;

public class SelectBox : MonoBehaviour {

    long selected;
    long uniq;
    public long Selected {
        get {
            return selected;
        }

        set {
            selected = value;
        }
    }

    public long Uniq {
        get {
            return uniq;
        }

        set {
            uniq = value;
        }
    }

    public void Focus() {
        gameObject.SetActive(true);
        for (var i = 0; i != transform.childCount; i++) {
            var b = transform.GetChild(i);
            b.gameObject.SetActive(false);
        }
    }



    public string Use1 {
        set {
            var b = transform.FindChild("Use1");
            b.GetComponent<UILabel>().text = value;
            b.GetComponent<MouseOver>().OnDragOut();
            b.gameObject.SetActive(true);
        }
    }

    public string Use2{
        set {
            var b = transform.FindChild("Use2");
            b.GetComponent<UILabel>().text = value;
            b.GetComponent<MouseOver>().OnDragOut();
            b.gameObject.SetActive(true);
        }
    }

    public string Use3 {
        set {
            var b = transform.FindChild("Use3");
            b.GetComponent<UILabel>().text = value;
            b.GetComponent<MouseOver>().OnDragOut();
            b.gameObject.SetActive(true);
        }
    }


    public string Use4 {
        set {
            var b = transform.FindChild("Use4");
            b.GetComponent<UILabel>().text = value;
            b.GetComponent<MouseOver>().OnDragOut();
            b.gameObject.SetActive(true);
        }
    }


    public void Call() {
        if (Selected != 0) {
            Sockets.Client.Send("Chan.Room.GameCardActionSelectable", iTween.Hash("uniq", Uniq, "method", Selected));
            Selected = 0;
        }
        
        gameObject.SetActive(false);
    }

    void OnClick() {
        Call();
    }
}