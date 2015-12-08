using UnityEngine;
using System.Collections;

public class MouseOver : MonoBehaviour {

    public long selected;
    public void OnDragOver() {
        transform.parent.GetComponent<SelectBox>().Selected = selected;
        GetComponent<UILabel>().color = Color.green;
    }
    public void OnDragOut() {
        transform.parent.GetComponent<SelectBox>().Selected = 0;
        GetComponent<UILabel>().color = Color.white;
    }

    public void OnMouswEnter() {
        OnDragOver();
    }

    public void OnMouswExit() {
        OnDragOut();
    }
    public void OnClick() {
        var sb = transform.parent.GetComponent<SelectBox>();
        sb.Selected = selected;
        sb.Call();

    }
}

