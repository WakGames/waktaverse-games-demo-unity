using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button quitButton;
    private TextMeshProUGUI _loginButtonText;

    private void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        
        _loginButtonText = loginButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        WakSDK.Wakgames.GetUserProfile((profile) =>
        {
            if (profile != null)
            {
                descText.text = $"{profile.name} 계정으로 로그인 되었습니다.";
                _loginButtonText.text = "Logout";
                AppendAchievementMessage();
            }
            else
            {
                descText.text = "로그아웃 상태입니다.";
            }
        });

        WakSDK.Wakgames.GetStatBoard("click_cnt", (stat) =>
        {
            if (stat != null)
            {
                int rank = stat.BoardIndex + 1;
                Debug.Log($"현재 등수 : {rank}");
            }
        });
    }

    private void AppendAchievementMessage()
    {
        WakSDK.Wakgames.GetUnlockedAchievements((achievement) =>
        {
            if (achievement != null)
            {
                string achieveNames = string.Join(", ", achievement.achieves.Select((a) => a.name));
                descText.text += $"\n달성한 도전과제 : {achievement.size}개\n{achieveNames}";
            }
        });
    }

    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void OnLoginButtonClicked()
    {
        if (_loginButtonText.text == "Logout")
        {
            WakSDK.Wakgames.Logout();

            descText.text = "로그아웃 상태입니다.";
            _loginButtonText.text = "Login";
        }
        else
        {
            WakSDK.Wakgames.Login((profile) =>
            {
                if (profile != null)
                {
                    descText.text = $"{profile.name} 계정으로 로그인 되었습니다.";
                    _loginButtonText.text = "Logout";
                    AppendAchievementMessage();

                    WakSDK.Wakgames.UnlockAchievement("first_login");
                }
                else
                {
                    descText.text = "로그인에 실패하였습니다.";
                }
            });
        }
    }

    private void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
