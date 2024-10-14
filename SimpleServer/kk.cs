using UnityEngine;
using System.Collections;
using System.Diagnostics;
public class Test : MonoBehaviour
{ // Use this for initialization 
    void Start()
    { }
    // Update is called once per frame 
    void Update()
    {
        Debug.Log(getTime());
    }
    string getTime()
    {
        string mtime = "00:00:00";
        mtime = getTimeHours(Time.time) + ":" + getTimeMinute(Time.time) + ":" + getTimeSecond(Time.time);
        return mtime;
    }
    string getTimeSecond(float time)
    {
        string mtime = null;
        int emptime = (int)(time % 60);
        if (emptime < 10)
        {
            mtime = "0" + emptime.ToString();
        }
        else
        {
            mtime = emptime.ToString();
        }
        return mtime;
    }
    string getTimeMinute(float time)
    {
        string mtime = null;
        int emptime = (int)(time / 60);
        if (emptime < 10)
        {
            mtime = "0" + emptime.ToString();
        }
        else
        {
            mtime = emptime.ToString();
        }
        return mtime;
    }
    string getTimeHours(float time)
    {
        string mtime = null;
        int emptime = (int)(time / 3600);
        if (emptime < 10)
        {
            mtime = "0" + emptime.ToString();
        }
        else
        {
            mtime = emptime.ToString();
        }
        return mtime;
    }
}
