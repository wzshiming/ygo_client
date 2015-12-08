using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Ok : MonoBehaviour {

    // Use this for initialization Filter
    static GameObject info = Resources.Load("GameObject/InfoBox") as GameObject;
    static GameObject opt = Resources.Load("GameObject/OptionBox") as GameObject;
    static GameObject fil = Resources.Load("GameObject/FilterBox") as GameObject;
    static GameObject insinfo;
    static GameObject insopt;
    static GameObject insfil;
    public delegate void action();
    static action act;




    public static void Reset() {
        Destroy(insinfo);
        Destroy(insopt);
        Destroy(insfil);
        insinfo = null;
        insopt = null;
        insfil = null;
    }

    public static void ShowFilter() {
        if (insfil == null) {
            insfil = Instantiate<GameObject>(fil);
            insfil.SetActive(false);
        }
        insfil.SetActive(true);
        iTween.ScaleFrom(insfil.gameObject, Vector3.zero, 0.3f);

    }


    public static void ShowOption() {
        if (insopt == null) {
            insopt = Instantiate<GameObject>(opt);
            insopt.SetActive(false);
        }
        insopt.SetActive(true);
        iTween.ScaleFrom(insopt.gameObject, Vector3.zero, 0.3f);
    }

    public static void Show(string inf) {
        if (insinfo == null) {
            insinfo = Instantiate<GameObject>(info);
            insinfo.SetActive(false);
        }
        var c = insinfo.transform.GetChild(0);
        var l = c.GetComponent<UILabel>();
        l.text = Locales.Get(inf);
        insinfo.SetActive(true);
        iTween.ScaleFrom(insinfo.gameObject, Vector3.zero, 0.3f);
    }


    public static void Show(string inf, action a) {
        Show(inf);
        act = a;
    }


    void OnClick() {
        transform.parent.gameObject.SetActive(false);
        if (act != null) {
            act();
            act = null;
        }

    }

}
