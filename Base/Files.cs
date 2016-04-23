using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class Files {


    /**
    * path：文件创建目录
    * name：文件的名称
    * info：写入的内容
    */
    static string path = "";
    static Files() {
        if (Application.platform == RuntimePlatform.Android) {
            path = Application.persistentDataPath;
        } else if (Application.platform == RuntimePlatform.WindowsPlayer) {
            path = Application.dataPath;
        } else if (Application.platform == RuntimePlatform.WindowsEditor) {
            path = Application.dataPath;
        } else {
            path = Application.dataPath;
        }
        path += "//cfg//";
        Directory.CreateDirectory(path);
    }

    public static void CreateFile(string name, string info) {
        //文件流信息
        StreamWriter sw;
        FileInfo t = new FileInfo(path +  name);
        if (!t.Exists) {
            //如果此文件不存在则创建
       
            sw = t.CreateText();
        } else {
            //如果此文件存在则打开
            sw = t.AppendText();
        }
        //以行的形式写入信息
        sw.WriteLine(info);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }

 
    public static string LoadText(string name) {
        //使用流的形式读取
        StreamReader sr = null;
        try {
            sr = File.OpenText(path +  name);
        } catch (Exception) {
            //路径与名称未找到文件则直接返回空
            return null;
        }
        string line="";
        ArrayList arrlist = new ArrayList();

        for(;;) {
            var l = sr.ReadLine();
            if (l == null) {
                break;
            }
            line += l;
        }
        //关闭流
        sr.Close();
        //销毁流
        sr.Dispose();
        //将数组链表容器返回
        return line;
    }


    public static void DeleteFile(string name) {
        File.Delete(path +  name);

    }

}