namespace WakSDK.ApiRequest
{
    /// <summary>
    /// 사용자 프로필.
    /// </summary>
    [System.Serializable]
    public class UserProfileResult
    {
        /// <summary>
        /// 사용자 ID.
        /// </summary>
        public int id;
        /// <summary>
        /// 닉네임.
        /// </summary>
        public string name;
        /// <summary>
        /// 프로필 이미지 URL.
        /// </summary>
        public string profileImg;
    }
}
