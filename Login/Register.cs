using UnityEngine;
using System.Collections;

public class Register : MonoBehaviour {

    // Use this for initialization
    Transform username, password, confirm;
    void Start() {
        username = transform.FindChild("Username");
        password = transform.FindChild("Password");
        confirm = transform.FindChild("Confirm");
    }

    public void Btn() {
        var name = username.GetComponent<UIInput>();
        var pwd = password.GetComponent<UIInput>();
        var con = confirm.GetComponent<UIInput>();
        if (name.value.Length < 1) {
            Ok.Show("hint.l002");
            return;
        }
        if (pwd.value != con.value) {
            Ok.Show("hint.l003");
            return;
        }


        Sockets.Client.CallBack("Auth.Users.Register", iTween.Hash("u", name.value, "p", pwd.value), (hash) => {
            var err = hash["error"] as string;
            if (err == null || err == "") {
                Sockets.Client.CallBack("Auth.Users.LogIn", iTween.Hash("u", name.value, "p", pwd.value), (hash2) => {
                    err = hash2["error"] as string;
                    if (err == null || err == "") {
                        Init.Jump("Hall");
                    } else {
                        Ok.Show(err);
                    }
                });
            } else {
                Ok.Show(err);
            }
        });

        //Auth.Users.LogIn

    }
}
