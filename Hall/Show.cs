using UnityEngine;
using System.Collections;

public class Show : MonoBehaviour {

    // Use this for initialization
    int i;
    void Start() {
        Async.PushDelay(0.5f,() => {
            i = Sockets.Client.Register("Chan.Room.InfoRegister", iTween.Hash(), delegate (Hashtable hash) {
                var err = hash["error"] as string;
                if (err == null || err == "") {
                    transform.GetChild(0).GetComponent<UILabel>().text = string.Format("{0}", hash["inHallNum"]);
                    transform.GetChild(1).GetComponent<UILabel>().text = string.Format("{0}", hash["inMatchNum"]);
                    transform.GetChild(2).GetComponent<UILabel>().text = string.Format("{0}", hash["inGameNum"]);
                } else {
                    Sockets.Client.Unregister(i);
                    Ok.Show(err);
                }
            });
        });
    }

    void OnDestroy() {
        Sockets.Client.Unregister(i);
    }
}
