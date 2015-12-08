using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 多语言
/// </summary>
public class Locales {
    
    static Dictionary<string, string> dic = new Dictionary<string, string>();
    static Dictionary<string, string> langMap = new Dictionary<string, string>();
    private static string lang = "";
    public delegate string delegateReplace(string s, bool b);
    static Dictionary<delegateReplace, string> transfunc = new Dictionary<delegateReplace, string>();

    public static string Lang {
        get {
            return lang;
        }
    }

    

    static Locales() {
        Init();
        langMap["简体中文"] = "zh-CN";
        langMap["English"] = "en-US";
        langMap["日本语"] = "jp-JP";
    }

    public static string[] GetLocales() {
        var ll = langMap.Keys;
        var ss = new string[ll.Count];
        ll.CopyTo(ss, 0);
        return ss;
    }

    static void Init() {

        if (lang == "") {

            if (Application.systemLanguage == SystemLanguage.Chinese ||
                Application.systemLanguage == SystemLanguage.ChineseSimplified ||
                Application.systemLanguage == SystemLanguage.ChineseTraditional) {
                lang = "zh-CN";
            } else if (Application.systemLanguage == SystemLanguage.English) {
                lang = "en-US";
            } else if (Application.systemLanguage == SystemLanguage.Japanese) {
                lang = "jp-JP";
            } else {
                lang = "en-US";
            }
        }


        var json = Resources.Load("Locales/" + Lang) as TextAsset;
        Debug.Log(Lang);
        var h = Json.jsonDecode(json.text);
        Add(h as Hashtable);
        foreach (var g in transfunc) {
            ChangeLang(g.Key);
        }
    }




    public static void Flash(string lan) {
        lang = langMap[lan];
        dic.Clear();
        Init();
    }

    static void Add(Hashtable hash) {
        Add("", hash);
    }

    static void Add(string hand, Hashtable hash) {
        foreach (DictionaryEntry entry in hash) {
            if (entry.Key is string) {
                string nh = entry.Key as string;
                if (hand.Length != 0) {
                    nh = hand + "." + entry.Key;
                }
                if (entry.Value is string) {
                    if (dic.ContainsKey(nh)) {
                        dic[nh] = entry.Value as string;
                    } else {
                        dic.Add(nh, entry.Value as string);
                    }
                } else if (entry.Value is Hashtable) {
                    Add(nh, entry.Value as Hashtable);
                }
            }
        }
    }

    public static string Get(string k) {
        if (dic == null) {
            Init();
        }

        if (!dic.ContainsKey(k)) {
            var vv = k.Split('.');
            return vv[vv.Length - 1];
        }
        return dic[k];
    }
  
    public static void Register(delegateReplace g) {
        transfunc.Add(g,g("",false));
        ChangeLang(g);
    }

    public static void Unregister(delegateReplace g) {
        transfunc.Remove(g);
    }

    static void ChangeLang(delegateReplace g) {
        var str = transfunc[g];
        var t = Locales.Get("ui." + str);
        g(t, true);
    }

}

