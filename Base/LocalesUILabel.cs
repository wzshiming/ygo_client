using UnityEngine;
// 用于 NGUI/UIInput 的文本自动替换类

public class LocalesUILabel : MonoBehaviour {
    Locales.delegateReplace dr;

    /// <summary>
    /// 改变文本 
    /// </summary>
    void Start() {
        // 向locale注册自己 至于为什么不用继承 呵呵
        dr = (s, b) => {
            var l = transform.GetComponent<UILabel>();
            if (b) {
                l.text = s;
            } else {
                return l.text;
            }
            return "";
        };
        Locales.Register(dr);
    }
    /// <summary>
    /// 向locale注销自己
    /// </summary>
    void OnDestroy() {
        Locales.Unregister(dr);
    }
}
