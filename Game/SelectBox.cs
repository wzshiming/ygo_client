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

    public string Use1 {
        set {
            var b = transform.FindChild("Use1");
            b.GetComponent<UILabel>().text = value;
            b.GetComponent<MouseOver>().OnDragOut();
            b.gameObject.SetActive(value != "");
        }
    }

    public string Use2{
        set {
            var b = transform.FindChild("Use2");
            b.GetComponent<UILabel>().text = value;
            b.GetComponent<MouseOver>().OnDragOut();
            b.gameObject.SetActive(value != "");
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