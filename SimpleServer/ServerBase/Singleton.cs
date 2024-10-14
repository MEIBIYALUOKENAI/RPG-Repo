using System;
using System.Collections.Generic;
using System.Linq;

//单例类模板
public class Singleton<T> where T : new()
{
    private static T ms_instance;

    public static T Instance
    {
        get
        {
            //懒汉模式
            if (ms_instance == null)
            {
                ms_instance = new T();
            }

            return ms_instance;
        }
    }
}