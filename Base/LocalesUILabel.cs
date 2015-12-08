using UnityEngine;
// ���� NGUI/UIInput ���ı��Զ��滻��

public class LocalesUILabel : MonoBehaviour {
    Locales.delegateReplace dr;

    /// <summary>
    /// �ı��ı� 
    /// </summary>
    void Start() {
        // ��localeע���Լ� ����Ϊʲô���ü̳� �Ǻ�
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
    /// ��localeע���Լ�
    /// </summary>
    void OnDestroy() {
        Locales.Unregister(dr);
    }
}
