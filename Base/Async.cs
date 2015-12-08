
using System.Collections;
using System.Collections.Generic;
using System;

public class Async {
    public delegate void Action();

    private static Queue<Action> wait;

    static Async() {
        wait = new Queue<Action>();
    }
    public static void Push(Action callback) {
        lock (wait) {
            wait.Enqueue(callback);
        }
    }


    public static void PushDelay(float waitTime, Action a) {
        var start = DateTime.Now;
        var end = start.AddSeconds(waitTime);
        PushTiming(end, a);
    }

    public static void PushTiming(DateTime t,Action a) {
        Action s = null;
        s = () => {
            if (DateTime.Now > t) {
                a();
            } else {
                Push(s);
            }
        };
        Push(s);
    }

    public static void PuahRound(float waitTime, Action a) {
        PushDelay(waitTime, () => {
            PuahRound(waitTime, a);
            a();
        });
    }

    public static void PushTimes(float waitTime, Action a,int size) {
        if (size <= 0) {
            return;
        }
        PushDelay(waitTime, () => {
            PushTimes(waitTime, a, size - 1);
            a();
        });
    }


    // Òì²½¸üÐÂ
    public static void Update() {

        Update(5);

    }


    public static void Update(int j) {
        lock (wait) {
            if (wait == null || wait.Count == 0) {
                return;
            }

            for (var i = 0; i != j && wait.Count != 0; i++) {
                var f = wait.Dequeue();
                try {
                    f();
                } catch {

                }
            }
        }
    }

    //	public static void Clear (){
    //
    //		
    //		if (wait == null) {
    //			wait = new Queue ();
    //		}
    //		wait.Clear ();
    //
    //	}
}

