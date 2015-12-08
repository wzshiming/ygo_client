using UnityEngine;
using System.Collections;

public class OptionBox : MonoBehaviour {

    void Start() {
        transform.FindChild("Versions").GetComponent<UILabel>().text = Locales.Get("ui.Version") + Application.version;
        //transform.GetComponent<UILabel> ()
        var pl = transform.FindChild("Languages").GetComponent<UIPopupList>();
        pl.Clear();
        Async.Push(() => {
            foreach(var k in Locales.GetLocales()) {
                pl.AddItem(k);
            }
        });
        //pl.AddItem("English");
        //pl.AddItem("简体中文");
    }

    // Update is called once per frame
    void Update() {

    }
    public void Quit() {
        Application.Quit();
    }

    public void LangChange() {
        var lang = transform.FindChild("Languages").GetComponent<UIPopupList>().value;
        //transform.parent.GetComponent<UIPopupList>().value;
        //transform.GetComponent<UILabel> ().text = lang;
        Locales.Flash(lang);
        Buffer.Clear();
        Ok.Reset();
        //Debug.Log(transform.parent.GetComponent<UIPopupList>().value);
        //Debug.Log("OnSelect");
    }
}

