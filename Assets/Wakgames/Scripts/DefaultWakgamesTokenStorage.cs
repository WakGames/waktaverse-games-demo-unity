using UnityEngine;
using WakSDK.ApiRequest;

namespace WakSDK
{
    public class DefaultWakgamesTokenStorage : MonoBehaviour, IWakgamesTokenStorage
    {
        public string AccessToken => _token.accessToken;
        public string RefreshToken => _token.refreshToken;
        public int IdToken => _token.idToken;

        private readonly WakgamesToken _token = new();

        public void UpdateToken(string accessToken, string refreshToken, int idToken)
        {
            _token.accessToken = accessToken;
            _token.refreshToken = refreshToken;
            _token.idToken = idToken;

            PlayerPrefs.SetString("Wakgames_Access_Token", accessToken);
            PlayerPrefs.SetString("Wakgames_Refresh_Token", refreshToken);
            PlayerPrefs.SetInt("Wakgames_ID_Token", idToken);
        }

        public string GetAccessToken()
        {
            return PlayerPrefs.GetString("Wakgames_Access_Token", string.Empty);
        
        }

        public string GetRefreshToken()
        {
            return PlayerPrefs.GetString("Wakgames_Refresh_Token", string.Empty);
        }

        public int GetIDToken()
        {
            return PlayerPrefs.GetInt("Wakgames_ID_Token", -1);
        }

        public void ClearToken()
        {
            UpdateToken(string.Empty, string.Empty, -1);
        }
    }
}
