using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {

    // Use this for initialization
    Transform username, password;
    void Start() {
        username = transform.FindChild("Username");
        password = transform.FindChild("Password");
        //Ok.Show("hello");
    }

    public void Btn() {
        var name = username.GetComponent<UIInput>();
        var pwd = password.GetComponent<UIInput>();
        //Debug.Log(name.value);
        //Debug.Log(pwd.value);
        if (name.value.Length < 1) {
            Ok.Show("hint.l002");
            return;
        }
        Sockets.Client.CallBack("Auth.Users.LogIn", iTween.Hash("u", name.value, "p", pwd.value), (hash) => {
            var err = hash["error"] as string;
            if (err == null || err == "") {
                Init.Jump("Hall");
            } else {
                Ok.Show(err);
            }
        });

        //Auth.Users.LogIn

    }

}
