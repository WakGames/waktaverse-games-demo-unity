using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using WakSDK.ApiRequest;

namespace WakSDK
{
    public class WakgamesAchieve : MonoBehaviour
    {
        private readonly List<AchievePanel> _achievePanels = new();
        private readonly Queue<AchievePanel> _achievePanelPool = new();
    
        private AchievePanel _achievePanel;
        private AudioSource _audioSource;

        private bool _isAlarmActive;
        private AchievePopupPosition _popupPosition;
        private bool _isPlaySfx;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _achievePanel = Resources.Load<AchievePanel>("Prefabs/AchievePanel");
        }

        public void SetUp(bool alarmState, AchievePopupPosition position, bool isAlarmPlaySFx)
        {
            _isAlarmActive = alarmState;
            _popupPosition = position;
            _isPlaySfx = isAlarmPlaySFx;
        }

        private void PopupAlarm(string achieveName, string achieveDesc, Texture2D texture)
        {
            if(!_isAlarmActive) return;
            
            foreach (AchievePanel panel in _achievePanels)
                panel.SlideUp();
        
            // 새로운 알림창 생성
            AchievePanel achievePanel = GetActivePanel();
            achievePanel.SetOnHide(() => RemoveAlarm(achievePanel));
            achievePanel.Setup(achieveName, achieveDesc, texture, _popupPosition);
            _achievePanels.Add(achievePanel);
            if(_isPlaySfx) _audioSource.Play();
        }

        public void AddAlarm(AchievementsResultItem achievement)
        {
            if(!_isAlarmActive) return;
            // 이미지 로드 후, 알람창 띄우기
            StartCoroutine(LoadImage(achievement.img, (texture, _) => PopupAlarm(achievement.name, achievement.desc, texture)));
        }
    
        public IEnumerator AddAlarms(SetStatsResultAchieveItem[] achievements)
        {
            Dictionary<int, Texture2D> textures = new();
        
            // 이미지 전부 로드하기
            for(int i = 0; i < achievements.Length; i++)
            {
                int index = i;
                StartCoroutine(LoadImage(achievements[i].img, (texture, resCode) => textures.Add(index, texture)));
            }
        
            // 로드 끝날 때까지 대기
            yield return new WaitUntil(() => textures.Count == achievements.Length);
        
            // 알람창 띄우기
            for(int i = 0; i < achievements.Length; i++)
            {
                PopupAlarm(achievements[i].name, achievements[i].desc, textures[i]);
            }
        }
    
        private void RemoveAlarm(AchievePanel panel)
        {
            panel.gameObject.SetActive(false);
            _achievePanels.Remove(panel);
            _achievePanelPool.Enqueue(panel);
        }

        private AchievePanel GetActivePanel()
        {
            if (_achievePanelPool.Count > 0)
            {
                AchievePanel panel = _achievePanelPool.Dequeue();
                panel.gameObject.SetActive(true);
                return panel;
            }
        
            return Instantiate(_achievePanel, transform, false);
        }

        //도전과제 이미지 가져오기
        private IEnumerator LoadImage(string img, Wakgames.CallbackDelegate<Texture2D> callback)
        {
            // 이미지 주소가 없으면 종료
            if (img == null)
            {
                callback(null, -1);
                yield break;
            }

            if (!img.StartsWith($"{Wakgames.Host}/img"))
                img = $"{Wakgames.Host}/img/{img}";
        
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(img);
            webRequest.SetRequestHeader("User-Agent", $"WakGames_Game/{Wakgames.Instance.ClientData.ClientID}");

            yield return webRequest.SendWebRequest();

            callback(
                webRequest.result == UnityWebRequest.Result.Success 
                    ? DownloadHandlerTexture.GetContent(webRequest) : null,
                (int)webRequest.responseCode);
        }
    }
}