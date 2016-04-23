using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 卡牌类
public class Card : MonoBehaviour {
    private static Dictionary<long, Card> uniqs = new Dictionary<long, Card>();
    private static GameObject CardPrefabs = Resources.Load<GameObject>("GameObject/Card");

    // 卡牌 各种 面状态数据
    private static Vector3 ua = new Vector3(-90, 180, 0);
    private static Vector3 da = new Vector3(90, 0, 0);
    private static Vector3 ud = new Vector3(-90, 90, 0);
    private static Vector3 dd = new Vector3(90, 180, -90);

    Vector3 ang;
    private static Vector3 tad = new Vector3(90, 0, 0);

    public List<string> Use = new List<string>();
    //public string use = ""; 
    

    // 获取指定uniq的卡牌 不存在则创建
    public static Card Get(long uniq) {
        if (!IsExist(uniq)) {
            var g = Instantiate<GameObject>(CardPrefabs);
            g.AddMissingComponent<Card>();
            g.AddMissingComponent<Mouse>();
            g.AddMissingComponent<Rigidbody>();
            var c = g.GetComponent<Card>();
            c.uniq = uniq;
            uniqs[uniq] = c;
        }
        return uniqs[uniq];
    }

   
    public static bool IsExist(long uniq) {
        return uniqs.ContainsKey(uniq);
    }



    private long uniq;
    public long Uniq {
        get {
            return uniq;
        }
    }

    // 卡牌id
    private long id;
    public long Id {
        set {
            if (id == value) {
                return;
            }
            id = value;
            transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.mainTexture = Buffer.GetFront(Id);
        }
        get {
            return id;
        }
    }


    // 仅用于角色卡 id =0
    public long Portrait {
        set {
            id = 0;
            transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material.mainTexture = Buffer.GetPortrait(value);
        }
    }


    // 卡牌底部信息
    public bool OnAppend {
        set {
            var t = transform.GetChild(2);
            t.gameObject.SetActive(value);
        }
    }

    public string Append {
        set {
            var t = transform.GetChild(2);
            var t2 = t.GetChild(0);
            if (value == null || value == "") {
                t.gameObject.SetActive(false);
                t2.GetComponent<TextMesh>().text = "";
                return;
            }
            t.gameObject.SetActive(true);
            t2.GetComponent<TextMesh>().text = value;
        }
        get {
            return transform.GetChild(2).GetChild(0).GetComponent<TextMesh>().text;
        }
    }

    // 卡牌边缘高亮
    public bool Rim {
        set {
            transform.GetChild(1).gameObject.SetActive(value);
        }
        get {
            return transform.GetChild(1).gameObject.activeSelf;
        }
    }


    // 卡牌属于哪一牌堆
    public Group Pos {
        get {
            return pos;
        }

        set {
            pos = value;
        }
    }

    
    private Group pos;



    void Start() {

        Pos = null;

        Ridid = false;
        Rim = false;
        transform.Rotate(da);
    }

    // 启用卡牌物理特性
    public bool Ridid {
        set {
            var r = gameObject.GetComponent<Rigidbody>();
            r.isKinematic = !value;
        }
        get {
            var r = gameObject.GetComponent<Rigidbody>();
            return r.isKinematic ;
        }
    }



    public void FaceUpAttack() {
        ang = ua;
        iTween.RotateTo(gameObject, ua, 0.5f);
        iTween.RotateTo(transform.GetChild(2).gameObject, tad, 0.5f);
    }

    public void FaceDownAttack() {
        ang = da;
        OnAppend = false;
        iTween.RotateTo(gameObject, da, 0.5f);
    }

    public void FaceUpDefense() {
        ang = ud;
        iTween.RotateTo(gameObject, ud, 0.5f);
        iTween.RotateTo(transform.GetChild(2).gameObject, tad, 0.5f);
    }

    public void FaceDownDefense() {
        ang = dd;
        OnAppend = false;
        iTween.RotateTo(gameObject, dd, 0.5f);
    }


    // 卡牌 跳起来
    bool remind = false;
    public bool Remind {
        get {

            return remind;
        }

        set {
            if(value) {
                if (!remind) {
                    iTween.MoveTo(gameObject, new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), 0.3f);
                    remind = true;
                }
            }else {
                if (remind) {
                    if (Pos != null) {
                        Pos.Flash(this);
                        remind = false;
                    }
                }
            }
        }
    }

    public void Flash() {
        if (Pos != null) {
            Pos.Flash(this);
        }
    }

    // 卡牌移动到全局坐标

    public void Pick() {
        if (Pos != null) {
            Pos.Leave(this);
        }
    }

    public void MoveTo(Vector3 v3) {


        if (Pos != null) {
            v3 = Pos.Master.rotation * v3;
            v3 += Pos.Master.position;
        }
        iTween.MoveTo(gameObject, v3, 0.5f);
        
        iTween.RotateTo(gameObject, ang, 0.5f);
    }

    public void MoveTo(float x, float y, float z) {
        var p = new Vector3(x, y, z);
        MoveTo(p);
    }
}
