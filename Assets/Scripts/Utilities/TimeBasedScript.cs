using UnityEngine;
using System;

public class TimeBasedScript : MonoBehaviour {

	DateTime currentDate;

	void Start()
	{
		//Store the current time when it starts
		currentDate = DateTime.Now;

		//Grab the old time from the player prefs as a long
		long temp = Convert.ToInt64(PlayerPrefs.GetString("sysString"));

		//Convert the old time from binary to a DataTime variable
		DateTime oldDate = DateTime.FromBinary(temp);
		print("oldDate: " + oldDate);

		//Subtract and store the result as a timespan variable
		TimeSpan difference = currentDate.Subtract(oldDate);
		print("Difference: " + difference.TotalMinutes + " minutes.");

	}

	void OnApplicationQuit()
	{
		//Savee the current system time as a string in the player prefs
		PlayerPrefs.SetString("sysString", DateTime.Now.ToBinary().ToString());
	}
}
