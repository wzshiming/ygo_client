using UnityEngine;
using System.Collections;

// ������ʼ����
// ÿ������ ����һ��  ��������Դ��
// �����첽 �� ��ʱ ��ʵ�� �Լ�һЩ��ĳ�ʼ��
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

    // ���� �첽����
    void Update() {
        
        Async.Update();
    }

    // ����ת��
    public static void Jump(string level) {
        s.Push(fname);
        fname = level;
        Application.LoadLevel(level);

    }


    // �����˻�
    public static void Ret() {
        if (s.Count != 0) {
            fname = s.Pop() as string;
            Application.LoadLevel(fname);
        }
        if (s.Count == 0) {
            Sockets.DisConn();
        }
    }

    // �˳����������
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

    // �˳�
    public void Quit() {
        Application.Quit();
    }
}

