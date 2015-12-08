using UnityEngine;
using System.Collections;

// 场景初始化类
// 每个场景 都有一个  放在主光源里
// 用于异步 和 延时 的实现 以及一些类的初始化
public class Init : MonoBehaviour {

    public static bool b = true;
    static Stack s = new Stack();
    static string fname = "Login";
    void Start() {

        //Application.runInBackground = true;

        if (b) {
            b = false;
            var c = Sockets.Client;
        }
    }

    // Update is called once per frame

    // 调用 异步更新
    void Update() {
        
        Async.Update();
    }

    // 场景转跳
    public static void Jump(string level) {
        s.Push(fname);
        fname = level;
        Application.LoadLevel(level);

    }


    // 场景退回
    public static void Ret() {
        if (s.Count != 0) {
            fname = s.Pop() as string;
            Application.LoadLevel(fname);
        }
        if (s.Count == 0) {
            Sockets.DisConn();
        }
    }

    // 退出到登入界面
    public static void First() {
        while (s.Count > 1) {
            s.Pop();
        }
        Ret();
    }


    public void ToFirst() {
        First();
    }




    //public void InitRet() {
    //    Ret();
    //}

    //public void InitJump(string level) {
    //    Jump(level);
    //}

    // 退出
    public void Quit() {
        Application.Quit();
    }
}

