using UnityEngine;
using System.Collections;

public class CardItem : MonoBehaviour {

    private long id = 0;
    private long size = 0;
    private long max = 3;

    private long atk = 0, def = 0;



    public long Id {
        set {
            id = value;
            flash();
        }
        get {
            return id;
        }
    }


    void Srart() {
        IsNew = false;
        IsHot = false;
    }
    public bool IsNew {
        set {
            transform.FindChild("New").gameObject.SetActive(value);
        }
    }

    public bool IsHot {
        set {
            transform.FindChild("Hot").gameObject.SetActive(value);
        }
    }


    void flash() {
        Async.Push(() => {
            var t = transform.FindChild("Front").GetComponent<UITexture>();
            t.mainTexture = Buffer.GetFront(id);
            var fo = Buffer.GetInfo(id);
            transform.FindChild("Name").GetComponent<UILabel>().text = fo.name;

            if (!isThumb) {

                var cif = transform.FindChild("Info");
                cif.gameObject.SetActive(true);
                cif.GetComponent<UILabel>().text = fo.effect;


                var ctp = transform.FindChild("Type");
                ctp.gameObject.SetActive(true);
                ctp.GetComponent<UILabel>().text = fo.type;

                var cr = transform.FindChild("Race");
                if (fo.level != 0) {
                    cr.GetComponent<UILabel>().text = fo.race;
                    cr.gameObject.SetActive(true);
                } else {
                    cr.gameObject.SetActive(false);
                }

                if (fo.level != 0) {
                    SetAtkAndDef(fo.atk, fo.def);
                } else {
                    transform.FindChild("Atk&DEF").gameObject.SetActive(false);
                }
            } else {
                for (var i = 3; i != transform.childCount; i++) {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        });
    }
    bool isThumb;

    public bool IsThumb {
        set {
            if (isThumb == value) {
                return;
            }
            isThumb = value;
            if (isThumb) {
                iTween.ScaleTo(transform.FindChild("Name").gameObject, new Vector3(2, 2, 2), 0.5f);
                transform.FindChild("Size").transform.localPosition = new Vector3(52, 36, 0);
            } else {
                iTween.ScaleTo(transform.FindChild("Name").gameObject, new Vector3(1, 1, 1), 0.5f);
                transform.FindChild("Size").transform.localPosition = new Vector3(52, -36, 0);
            }

            flash();
        }
        get {

            return isThumb;
        }

    }



    public long Max {
        set {
            max = value;
        }
        get {
            return max;
        }
    }

    public long Size {
        set {
            size = value;
            transform.FindChild("Size").GetComponent<UILabel>().text = string.Format("x{0}", size);
        }
        get {
            return size;
        }
    }

    public void SetAtkAndDef(long a, long d) {
        atk = a;
        def = d;

        var t = transform.FindChild("Atk&DEF");
        t.gameObject.SetActive(true);
        t.GetComponent<UILabel>().text = string.Format("ATK/{0} DEF/{1}", atk, def);
    }


    void OnClick() {
        iTween.PunchScale(gameObject, Vector3.one, 0.5f);
        if (size != 0) {
            transform.parent.SendMessage("OnChildClick", id);
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddOne() {
        if (size != max) {
            Size++;
        }
    }

    public void SubOne() {
        if (size != 0) {
            Size--;
        }
    }


}
