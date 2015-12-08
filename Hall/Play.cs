using UnityEngine;
using System.Collections;

public class Play : MonoBehaviour {

    bool b = true;
    // Use this for initialization
    int i;

    // Update is called once per frame

    public void OnCilck() {
        if (b) {
            Join();
            b = false;
        } else {
            Leave();
            b = true;
        }

        transform.GetComponent<UIButton>().isEnabled = false;

        System.Timers.Timer time = new System.Timers.Timer(1000);

        time.Elapsed += new System.Timers.ElapsedEventHandler((source, e) => {
            Async.Push(() => {
                transform.GetComponent<UIButton>().isEnabled = true;
            });
            time.Close();
        });
        time.AutoReset = false;
        time.Enabled = true;
    }

    void Join() {
        i = Sockets.Client.Register("Chan.Room.MatchCompetitors", iTween.Hash(), (Hashtable hash) => {
            var err = hash["error"] as string;
            if (err == null || err == "") {
                transform.GetComponent<UILabel>().text = Locales.Get("ui.Stop");
            } else {
                Sockets.Client.Unregister(i);
                Ok.Show(err);
            }
            var status = hash["status"] as string;
            if (status == "init") {
                Sockets.Client.Unregister(i);
                Init.Jump("Game");
            }

        });
    }

    void Leave() {
        Sockets.Client.CallBack("Chan.Room.StopMatch", iTween.Hash(), (Hashtable hash) => {
            var err = hash["error"] as string;
            if (err == null || err == "") {
                Sockets.Client.Unregister(i);
                transform.GetComponent<UILabel>().text = Locales.Get("ui.Play");
            } else {
                Ok.Show(err);
            }
        });
    }
    void OnDestroy() {
        Sockets.Client.Unregister(i);
    }

}

