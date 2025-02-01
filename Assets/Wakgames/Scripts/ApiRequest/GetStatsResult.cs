using System;
using System.Collections.Generic;

namespace WakSDK.ApiRequest
{
    /// <summary>
    /// 통계 목록.
    /// </summary>
    [Serializable]
    public class GetStatsResult
    {
        #region Classes
    
        /// <summary>
        /// 한 통계 정보.
        /// </summary>
        [Serializable]
        public class GetStatsResultItem
        {
            /// <summary>
            /// 통계 ID.
            /// </summary>
            public string id;
            /// <summary>
            /// 통계 이름.
            /// </summary>
            public string name;
            /// <summary>
            /// 현재 통계 값.
            /// </summary>
            public int val;
            /// <summary>
            /// 최대 통계 값. (없으면 0.)
            /// </summary>
            public int max;
            /// <summary>
            /// 최초 누적일. (UNIX 시간(ms))
            /// </summary>
            public long regDate;
            /// <summary>
            /// 마지막 누적일. (UNIX 시간(ms))
            /// </summary>
            public long chgDate;

            /// <summary>
            /// 통계 최댓값 유무.
            /// </summary>
            public bool HasMax => max != 0;
            /// <summary>
            /// 최초 누적일.
            /// </summary>
            public DateTimeOffset RegDate => DateTimeOffset.FromUnixTimeMilliseconds(regDate);
            /// <summary>
            /// 마지막 누적일.
            /// </summary>
            public DateTimeOffset ChgDate => DateTimeOffset.FromUnixTimeMilliseconds(chgDate);
        }

        #endregion
    
        /// <summary>
        /// 개수.
        /// </summary>
        public int size;
        /// <summary>
        /// 통계 목록.
        /// </summary>
        public List<GetStatsResultItem> stats;
    }
}