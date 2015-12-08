using UnityEngine;
// ���� NGUI/UIInput ���ı��Զ��滻��

public class LocalesUIInput : MonoBehaviour {
    Locales.delegateReplace dr;

    /// <summary>
    /// �ı��ı� 
    /// </summary>
    void Start() {
        // ��localeע���Լ� ����Ϊʲô���ü̳� �Ǻ�
        dr = (s, b) => {
            var l = transform.GetComponent<UIInput>();
            if (b) {
                l.defaultText = s;
            } else {
                return l.defaultText;
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
