using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SampleBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numText;
    [SerializeField] private Button addButton;
    [SerializeField] private Button resetButton;
    private int _num;

    private void Awake()
    {
        addButton.onClick.AddListener(OnAddButtonClicked);
        resetButton.onClick.AddListener(OnResetButtonClicked);
    }

    void Start()
    {
        numText.text = "Loading";
        LoadClickCount();

        UnlockAchievement("start_game", "게임 시작");
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Counter", _num);
    }

    public void OnAddButtonClicked()
    {
        _num += 1;
        numText.text = $"{_num}";
        
        if (_num % 10 == 0)
        {
            SaveClickCount();
        }
    }
    
    public void OnResetButtonClicked()
    {
        _num = 0;
        numText.text = "0";
        SaveClickCount();
        UnlockAchievement("reset", "큰 결심");
    }
    public void OnHiddenClicked()
    {
        UnlockAchievement("follow_ine", "쉿.");
    }

    private void UnlockAchievement(string id, string name)
    {
        WakSDK.Wakgames.UnlockAchievement(id, (achievement) =>
        {
            if (achievement == WakSDK.Wakgames.AchievementState.Success)
            {
                Debug.Log($"{name} 도전과제 달성!");
            }
            else if (achievement == WakSDK.Wakgames.AchievementState.NotFound)
            {
                Debug.LogError("존재하지 않는 도전과제.");
            }
            else if (achievement == WakSDK.Wakgames.AchievementState.AlreadyAchieved)
            {
                Debug.Log($"{name} 도전과제 이미 달성됨.");
            }
            else
            {
                Debug.LogError($"알 수 없는 오류.");
            }
        });
    }

    private void LoadClickCount()
    {
        WakSDK.Wakgames.GetStats((stats) =>
        {
            if (stats != null)
            {
                var stat = stats.stats.Find((s) => s.id == "click_cnt");
                int num = stat?.val ?? 0;

                _num = num;
                numText.text = num.ToString();
                PlayerPrefs.SetInt("Counter", num);

                Debug.Log($"클릭 수 : {num}");
            }
            else
            {
                _num = PlayerPrefs.GetInt("Counter", 0);
                numText.text = _num.ToString();
            }
        });
    }

    private void SaveClickCount()
    {
        PlayerPrefs.SetInt("Counter", _num);
        
        WakSDK.Wakgames.SetStat("click_cnt", _num, (stat) =>
        {
            if (stat != null)
            {
                var s = stat.stats.Find((s) => s.id == "click_cnt");
                if (s != null)
                {
                    Debug.Log($"클릭 수 기록됨 : {s.val}");
                }
                else
                {
                    Debug.LogError($"클릭 수 기록 실패.");
                }

                foreach (var achieve in stat.achieves)
                {
                    Debug.Log($"{achieve.name} 도전과제 달성!");
                }
            }
        });
    }
}
