using System;
using System.Collections;
using System.Collections.Generic;

namespace WakSDK.ApiRequest
{
    /// <summary>
    /// 한 통계 입력 정보.
    /// </summary>
    [Serializable]
    public class SetStatsInputItem
    {
        /// <summary>
        /// 통계 ID.
        /// </summary>
        public string id;
        /// <summary>
        /// 입력할 통계 값.
        /// </summary>
        public int val;
    }

    /// <summary>
    /// 통계 입력 목록.
    /// </summary>
    [Serializable]
    public class SetStatsInput : IEnumerable<SetStatsInputItem>
    {
        /// <summary>
        /// 입력할 통계들.
        /// </summary>
        public SetStatsInput(string id, int val)
        {
            Add(id, val);
        }
        public List<SetStatsInputItem> stats;
        public void Add(string id, int val)
        {
            stats ??= new List<SetStatsInputItem>();
            stats.Add(new SetStatsInputItem { id = id, val = val });
        }
        public IEnumerator<SetStatsInputItem> GetEnumerator()
        {
            return stats.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return stats.GetEnumerator();
        }
    }
}