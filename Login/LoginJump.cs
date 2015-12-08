using UnityEngine;
using System.Collections;

public class LoginJump : MonoBehaviour {
    //Stack s = new Stack();
    // Use this for initialization
    static Vector3 ou = new Vector3(0, 8.0f, 0);


    string current = "";

    void Start() {
        //DecksManage.Init ();
        ToLogin();
    }

    // Update is called once per frame
    void Update() {

    }


    public void ToRegister() {
        hallJump("Register");
    }

    public void ToLogin() {
        hallJump("Login");
    }


    void hallJump(string level) {

        lock (current) {
            if (current == level) {
                return;
            }
            var d = GameObject.Find(level);
            iTween.MoveTo(d, Vector3.zero, 0.5f);
            if (current != "") {
                var h = GameObject.Find(current);
                iTween.MoveTo(h, ou, 0.5f);
            }
            current = level;
        }
    }

    void join(GameObject g) {
        iTween.MoveTo(g, Vector3.zero, 1.0f);
    }
    void join(string level) {
        var g = GameObject.Find(level);
        join(g);
    }
    void leave(GameObject g) {
        iTween.MoveTo(g, ou, 1.0f);
    }
    void leave(string level) {
        var g = GameObject.Find(level);
        leave(g);
    }

}