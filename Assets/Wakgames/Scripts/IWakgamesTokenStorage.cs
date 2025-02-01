namespace WakSDK
{
    public interface IWakgamesTokenStorage
    {
        /// <summary>
        /// 토큰 저장.
        /// </summary>
        /// <param name="accessToken">접근 토큰.</param>
        /// <param name="refreshToken">갱신 토큰.</param>
        /// <param name="idToken">사용자 ID.</param>
        public void UpdateToken(string accessToken, string refreshToken, int idToken);
    
        /// <summary>
        /// 접근 토큰.
        /// </summary>
        public string GetAccessToken();
    
        /// <summary>
        /// 갱신 토큰.
        /// </summary>
        public string GetRefreshToken();
    
        /// <summary>
        /// 사용자 ID.
        /// </summary>
        public int GetIDToken();
    
        /// <summary>
        /// 토큰 삭제.
        /// </summary>
        public void ClearToken();
    }
}
