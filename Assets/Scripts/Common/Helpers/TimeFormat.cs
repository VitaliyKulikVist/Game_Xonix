using System;
using UnityEngine;

namespace Assets.Scripts.Common.Helpers {
	public static class TimeFormat {
		public static (int day, int hours, int min, int sec) FormatTime(float second) {
			TimeSpan ts = TimeSpan.FromSeconds(second);
			return (ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
		}
		public static string FormatTimeString(float second) {
			TimeSpan ts = TimeSpan.FromSeconds(second);
			if (ts.Days != 0) return ts.Days.ToString("D2") + "+" + ts.Hours.ToString("D2") + "+" + ts.Minutes.ToString("D2") + ":" + ts.Seconds.ToString("D2");
			else if (ts.Hours != 0) return ts.Hours.ToString("D2") + "+" + ts.Minutes.ToString("D2") + ":" + ts.Seconds.ToString("D2");
			else if (ts.Minutes != 0) return ts.Minutes.ToString("D2") + ":" + ts.Seconds.ToString("D2");
			else if (ts.Seconds != 0) return ts.Seconds.ToString("D2");
			else {
				Debug.Log("Canot convert Time");
				return "";
			}
		}
	}
}


