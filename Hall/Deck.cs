using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Deck : MonoBehaviour {

    GameObject edit;
    GameObject pile;
    static Vector3 ou = new Vector3(100.0f, 0, 0);
    void Start() {

        edit = transform.FindChild("EditBarGrid").gameObject;
        pile = transform.FindChild("Pile").gameObject;
    }



    bool iseditmodel;

    public bool IsEditModel {
        set {

            if (iseditmodel == value) {
                return;
            }

            if (iseditmodel) {
                iTween.MoveTo(pile, ou, 0.5f);
                edit.GetComponent<EditBarGrid>().ZoomIn();
                transform.FindChild("ToEdit").GetComponent<UILabel>().text = Locales.Get("ui.Edit");
            } else {
                iTween.MoveTo(pile, Vector3.zero, 0.5f);
                edit.GetComponent<EditBarGrid>().ZoomOut();
                transform.FindChild("ToEdit").GetComponent<UILabel>().text = Locales.Get("ui.Cancel");

            }
            iseditmodel = !iseditmodel;

        }

        get {
            return iseditmodel;
        }
    }
    public void OnEditModel() {
        IsEditModel = true;
    }

    public void OffEditModel() {
        IsEditModel = false;
    }

    public void EditButton() {
        if (IsEditModel) {
            Cancel();
        }
        IsEditModel = !IsEditModel;
    }

    public void LoadDecks() {
        DecksManage.LoadDeck(() => {
            var up = transform.FindChild("Decks").GetComponent<UIPopupList>();
            up.Clear();
            foreach (string n in DecksManage.GetNames()) {
                up.AddItem(n);
            }
            SelectFor(DecksManage.GetDefName());
        });
    }
    public void UploadDeck() {
        var name = transform.FindChild("Decks").GetChild(0).GetComponent<UILabel>().text;
        var main = edit.GetComponent<EditBarGrid>().GetDeck();
        DecksManage.UploadDeck(name, main);

    }
    void SelectFor(string name) {
        transform.FindChild("Decks").GetChild(0).GetComponent<UILabel>().text = name;
        var dbg = edit.GetComponent<EditBarGrid>();
        dbg.OpenDeck(name);
    }
    public void Select() {
        SelectFor(transform.FindChild("Decks").GetComponent<UIPopupList>().value);

    }

    public void OnEditClick(long id) {
        if (iseditmodel) {
            edit.GetComponent<EditBarGrid>().Sub(id);
        } else {

        }
    }
    public void Cancel() {
        LoadDecks();
    }

    public void Save() {
        UploadDeck();
        OffEditModel();
    }
    //	public void OnEdit(){
    //		var t = transform.FindChild ("Edit");
    //		iTween.MoveTo (t.gameObject, Vector3.zero, 1.0f);
    //		var b = transform.FindChild ("EditBarGrid");
    //		b.GetComponent<EditBarGrid> ().ZoomOut ();
    //	}

}

