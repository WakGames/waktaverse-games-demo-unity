using System;
using System.Collections.Generic;

namespace WakSDK.ApiRequest
{
    /// <summary>
    /// 전체 사용자 통계 조회 결과.
    /// </summary>
    [Serializable]
    public class GetStatBoardResult
    {
        #region Classes
    
        /// <summary>
        /// 한 사용자 통계 정보.
        /// </summary>
        [Serializable]
        public class GetStatBoardResultItem
        {
            /// <summary>
            /// 사용자 정보.
            /// </summary>
            [Serializable]
            public class GetStatBoardResultUser
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
                public string img;
            }
        
            /// <summary>
            /// 사용자 정보.
            /// </summary>
            public GetStatBoardResultUser user;
        
            /// <summary>
            /// 통계 값.
            /// </summary>
            public int val;
        }

        /// <summary>
        /// 통계 정보.
        /// </summary>
        [Serializable]
        public class GetStatBoardResultStat
        {
            /// <summary>
            /// 통계 이름.
            /// </summary>
            public string name;
            /// <summary>
            /// 최대 통계 값. (없으면 0.)
            /// </summary>
            public int max;

            /// <summary>
            /// 통계 최댓값 유무.
            /// </summary>
            public bool HasMax => max != 0;
        }

        #endregion
    
        /// <summary>
        /// 전체 사용자 통계들 개수.
        /// </summary>
        public int size;
        /// <summary>
        /// 전체 사용자 통계들.
        /// </summary>
        public List<GetStatBoardResultItem> board;
        /// <summary>
        /// 대상 통계.
        /// </summary>
        public GetStatBoardResultStat stat;
        /// <summary>
        /// 현재 사용자 ID.
        /// </summary>
        public int me;

        /// <summary>
        /// 전체 사용자 통계 목록에서 현재 사용자의 인덱스.
        /// </summary>
        public int BoardIndex => board.FindIndex((i) => i.user.id == me);
    }
}