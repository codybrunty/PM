using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class TimeManager : MonoBehaviour {
    public static TimeManager sharedInstance = null;
    //private string _url = "http://leatonm.net/wp-content/uploads/2017/candlepin/getdate.php"; //change this to your own
    private string _timeData;
    private string _currentTime;
    public string _currentDate;

    void Awake() {
        if (sharedInstance == null) {
            sharedInstance = this;
        }
        else if (sharedInstance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        Debug.Log("TimeManager script is Ready.");
    }

    public IEnumerator getTime() {
        Debug.Log("Connecting to Internet");
        UnityWebRequest myHttpWebRequest = UnityWebRequest.Get("https://www.microsoft.com");
        yield return myHttpWebRequest.SendWebRequest();

        if (myHttpWebRequest.error == null) {
            Debug.Log("Got DateTime from the Internet");
            string netTime = myHttpWebRequest.GetResponseHeader("date");
            DateTime netTimeParsed = DateTime.ParseExact(netTime, "ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.AssumeUniversal);
            _timeData = netTimeParsed.ToString("MM-dd-yyyy/HH:mm:ss");
        }
        
        else{
            Debug.Log("Error Connecting to Internet, Grabbing Local DateTime");
            DateTime localTime = DateTime.Now;
            _timeData = localTime.ToString("MM-dd-yyyy/HH:mm:ss");
        }
        
        if (_timeData != ""){
            string[] words = _timeData.Split('/');
            Debug.Log("The date is: " + words[0]);
            Debug.Log("The time is: " + words[1]);
            _currentDate = words[0];
            _currentTime = words[1];
        }
    }


    public int getCurrentDateNow() {
        string[] words = _currentDate.Split('-');
        int x = int.Parse(words[0] + words[1] + words[2]);
        return x;
    }

    public string getCurrentTimeNow() {
        return _currentTime;
    }
}



