using UnityEngine;

namespace WakSDK
{
    public class WakgamesClientData : ScriptableObject
    {
        public string ClientID;
        public int CallbackServerPort;
        public bool AchieveAlarmToggle;
        public bool AchieveSfxToggle;
        public AchievePopupPosition AchieveAlarmPosition;
    }
    
    public enum AchievePopupPosition : sbyte
    {
        우측하단 = -2, //815, -580
        우측상단 = +2, //815, 580
        좌측하단 = -1, //-815, -580
        죄측상단 = +1, //-815, 580
    }
}
