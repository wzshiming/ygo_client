using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// 决斗场地布局类
public class Area : MonoBehaviour {

    public bool you;
    const float offset_x = 1.13f;
    const float offset_z = 2.0f;
    Dictionary<string, Group> area;


    void addGroup(string name, Group g) {
        g.Name = name;
        g.Master = transform;
        area.Add(name, g);
    }


    // 初始化 决斗场地数据
    void Start() {
        area = new Dictionary<string, Group>();

        addGroup("portrait", new Group((c, i, j) => {
            c.MoveTo(4.5f, 0.02f * (i + 1), -1.2f);
        }, (c) => {
            c.FaceUpAttack();
        }));

        if (you) {
            addGroup("hand", new Group((c, i, j) => {
                c.MoveTo(offset_x * (i - j / 2) * 0.55f, 0.2f, -2.0f);
            }, (c, i, j) => {
                c.MoveTo(offset_x * (i - j / 2) * 0.55f, 0.5f, -1.6f);
            }, (c) => {
                c.FaceUpAttack();
            }));
        } else {
            addGroup("hand", new Group((c, i, j) => {
                c.MoveTo(offset_x * (i - j / 2) * 0.55f, 0.2f, -1.8f);
            }, (c, i, j) => {
                c.MoveTo(offset_x * (i - j / 2) * 0.55f, 0.5f, -1.4f);
            }, (c) => {
                c.FaceDownAttack();
            }));
        }


        addGroup("deck", new Group((c, i, j) => {
            c.MoveTo(3.43f, 0.02f * (i + 1), -1.3f);
        }, (c) => {
            c.FaceDownAttack();
        }));

        addGroup("removed", new Group((c, i, j) => {
            c.MoveTo(-4.5f, 0.02f * (i + 1), 1.0f);
        }, (c) => {
            c.FaceUpAttack();
        }));

        addGroup("grave", new Group((c, i, j) => {
            c.MoveTo(3.43f, 0.02f * (i + 1), 1.3f);
        }, (c, i, j) => {
            c.MoveTo(3.43f - offset_x * (int)(i % 8) / 2, 0.02f * (i + 1), 1.3f + offset_z * (int)(i / 8) / 2);
        }, (c) => {
            c.FaceUpAttack();
        }));

        addGroup("extra", new Group((c, i, j) => {
            c.MoveTo(-3.43f, 0.02f * (i + 1), -1.3f);
        }, (c, i, j) => {
            c.MoveTo(-3.43f + offset_x * (int)(i % 5) / 2, 0.02f * (i + 1), -1.3f + offset_z * (int)(i / 5) / 2);
        }, (c) => {
            c.FaceUpAttack();
        }));

        addGroup("mzone", new Group((c, i, j) => {
            c.MoveTo(offset_x * (i - 2), 0.02f, 0.8f);
        }, (c) => {
            Async.PushDelay(1.0f, () => {
                c.OnAppend = true;
            });
        }, (c) => {
            Async.PushDelay(0.8f, () => {
                c.OnAppend = false;
            });
        }));

        addGroup("szone", new Group((c, i, j) => {
            c.MoveTo(offset_x * (i - 2), 0.02f, -0.7f);
        }));


        addGroup("field", new Group((c, i, j) => {
            c.MoveTo(-3.43f, 0.02f * (i + 1), 1.3f);
        }));

        addGroup("select", new Group((c, i, j) => {
            c.MoveTo(offset_x * (i - j / 2) * 0.55f, 0.2f, 1.3f);
        }));
    }


    // 卡牌在决斗场地中移动
    public void Move(Card c, string pos) {
        Async.Push(() => {
            area[pos].Join(c);
        });
    }

}

