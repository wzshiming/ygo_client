using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CardInfo {
    public long id, level, atk, def;
    public string attr, effect, name, race, type, pwd;
    public string enrace, entype, enattr;
}
public class Buffer {
    public static void WriteFile(string path,byte[] bytes) {
        File.WriteAllBytes(Application.dataPath + path, bytes);
    }


    private static Dictionary<string, Texture2D> texture = new Dictionary<string, Texture2D>();
    public static Texture2D GetTexture(string path) {
        if (!texture.ContainsKey(path)) {
            texture[path] = Resources.Load<Texture2D>(path);
        }
        return texture[path];
    }

    
    public static Texture2D GetFront(long i) {
        return GetTexture("Card/Front/" + i);
    }


    public static Texture2D GetPortrait(long i) {
        return GetTexture("Card/Portrait/" + i);
    }
    
    public static Texture2D GetBack(long i) {
        return GetTexture("Card/Back/" + i);
    }

   

    private static Dictionary<string, TextAsset> text = new Dictionary<string, TextAsset>();
    public static TextAsset GetText(string path) {
        if (!text.ContainsKey(path)) {
            text[path] = Resources.Load<TextAsset>(path);
        }
        return text[path];
    }

    private static Dictionary<string, string> attr = new Dictionary<string, string>();
    public static Dictionary<string, string> Attr {
        get {
            if (attr.Count == 0) {
                var lo = GetText("Card/Build/Json/" + Locales.Lang + "_attr");
                if (lo == null || lo.text == null || lo.text.Length == 0) {
                    return null;
                }
                var h = Codes.DeJson(lo.text) as Hashtable;
                foreach (DictionaryEntry de in h) {
                    attr[de.Key as string] = de.Value as string;
                }
            }
            return attr;
        }

    }
    public static string GetAttr(string k) {
        return Attr[k];
    }

    private static Dictionary<string, string> race = new Dictionary<string, string>();
    public static Dictionary<string, string> Race {
        get {
            if (race.Count == 0) {
                var lo = GetText("Card/Build/Json/" + Locales.Lang + "_race");
                if (lo == null || lo.text == null || lo.text.Length == 0) {
                    return null;
                }
                var h = Codes.DeJson(lo.text) as Hashtable;
                foreach (DictionaryEntry de in h) {
                    race[de.Key as string] = de.Value as string;
                }
            }
            return race;
        }
    }
    public static string GetRace(string k) {
        return Race[k];
    }

    private static Dictionary<string, string> typ = new Dictionary<string, string>();
    public static Dictionary<string, string> Typ {
        get {
            if (typ.Count == 0) {
                var lo = GetText("Card/Build/Json/" + Locales.Lang + "_typ");
                if (lo == null || lo.text == null || lo.text.Length == 0) {
                    return null;
                }
                var h = Codes.DeJson(lo.text) as Hashtable;
                foreach (DictionaryEntry de in h) {
                    typ[de.Key as string] = de.Value as string;
                }
            }
            return typ;
        }
    }

    public static string GetType(string k) {
        
        return Typ[k];
    }


    private static Dictionary<long, CardInfo> info = new Dictionary<long, CardInfo>();



    public static CardInfo GetInfo(long i) {
        if (!info.ContainsKey(i)) {

            var lo = GetText("Card/Build/Json/pub/" + i);
            if (lo == null || lo.text == null || lo.text.Length == 0) {
                return null;
            }
            var lo2 = GetText("Card/Build/Json/" + Locales.Lang + "/" + i);
            if (lo2 == null || lo2.text == null || lo2.text.Length == 0) {
                return null;
            }



            var de = Codes.DeJson(lo.text) as Hashtable;
            var de2 = Codes.DeJson(lo2.text) as Hashtable;
            var ci = new CardInfo();
            ci.effect = de2["depict"] as string;
            ci.name = de2["name"] as string;
            ci.entype = de["type"]as string;
            ci.type = GetType(de["type"] as string);
            ci.pwd = de["pwd"] as string;
            ci.id = Codes.ToInt(de["id"]);
            ci.level = Codes.ToInt(de["level"]);
            if (ci.level != 0) {
                ci.enattr = de["attr"] as string;
                ci.attr = GetAttr(de["attr"] as string);
                ci.enrace = de["race"] as string;
                ci.race = GetRace(de["race"] as string);
                ci.atk = Codes.ToInt(de["atk"]);
                ci.def = Codes.ToInt(de["def"]);
            }
            info[i] = ci;

        }
        return info[i];
    }

    

    public static void Clear() {
        info.Clear();
        Typ.Clear();
        Attr.Clear();
        Race.Clear();
        text.Clear();
    }
}

