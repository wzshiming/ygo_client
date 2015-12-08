
using UnityEngine;
using System.Collections;

public class Mouse : MonoBehaviour {

    public static Mouse currentOver = null;

    public static GameObject selectBox;
    public static GameObject showBox;
    public static Cbs cb;


    string Use1Str, Use2Str;

    void Start() {
        if (selectBox == null) {
            selectBox = GameObject.Find("SelectBox");
        }
        if (showBox == null) {
            showBox = GameObject.Find("ShowBox");
        }
        if (cb == null) {
            cb = GameObject.Find("Global").GetComponent<Cbs>();
        }
    }



    Vector3 mousepos;
    void OnMouseEnter() {
        if (down) {
            return;
        }

        if (currentOver != null) {
            currentOver.OnMouseExit();
        }

        currentOver = this;

        var c = GetComponent<Card>();
        if (c.Pos != null && c.Pos.Name == "deck") {
            return;
        }

        mousepos = Input.mousePosition;
        if (currentOver != null) {
            currentOver.OnMouseExit();
        }
        iTween.PunchScale(gameObject, Vector3.one, 1.0f);


        if (c.Pos != null) {
            if (c.Pos.Name == "deck") {
                return;
            }
            c.Pos.OnMouseEnter();
        }
        c.Remind = true;
        cb.send(c.Uniq, 101);
        if (c.Id == 0) {
            return;
        }
        var curcam = UICamera.currentCamera;
        var b = curcam.ScreenToWorldPoint(Input.mousePosition);


        showBox.transform.position = b;

        var lp = showBox.transform.localPosition;
        if (lp.y < -100) {
            lp.y += 350;
        } else if (lp.y > 100) {
            lp.y -= 200;
        } else if (lp.x < 100) {
            lp.x += 200;
        } else if (lp.x > 100) {
            lp.x -= 200;
        }
        showBox.transform.localPosition = lp;
        showBox.SetActive(true);
        showBox.GetComponent<CardItem>().Id = c.Id;

    }

    void OnMouseExit() {

        if (down) {
            return;
        }
        var c = GetComponent<Card>();
        if (c.Pos != null && c.Pos.Name == "deck") {
            return;
        }
        if (mousepos == Vector3.zero || mousepos == Input.mousePosition) {
            return;
        }
        mousepos = Vector3.zero;

        currentOver = null;

        c.Pos.OnMouseExit();

        c.Remind = false;
        showBox.SetActive(false);
        cb.send(c.Uniq, 102);
    }


    static bool down;
    static bool click;
    void OnMouseDown() {
        OnMouseEnter();

        down = true;
        click = true;
        Async.PushDelay(0.5f, () => {
            click = false;
        });
        var c = GetComponent<Card>();
        selectBox.GetComponent<SelectBox>().Uniq = c.Uniq;

        if (c.Rim) {

            selectBox.transform.position = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
            selectBox.SetActive(true);

            if (c.use == null || c.use == "") {
                Use1Str = Locales.Get("oper.Select");
                Use2Str = "";

            } else {
                var b = c.use.Split(',');
                Use1Str = Locales.Get("oper." + b[0]);
                if (b.Length > 1) {
                    Use2Str = Locales.Get("oper." + b[1]);
                } else {
                    Use2Str = "";
                }
            }
            var sb = selectBox.GetComponent<SelectBox>();
            sb.Use1 = Use1Str;
            sb.Use2 = Use2Str;

        }

    }

    void OnMouseUp() {
        down = false;
        if (!click) {
            selectBox.GetComponent<SelectBox>().Call();
            OnMouseExit();
        }
    }
}