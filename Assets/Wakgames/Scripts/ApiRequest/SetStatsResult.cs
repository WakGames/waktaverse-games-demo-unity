using System;
using System.Collections.Generic;

namespace WakSDK.ApiRequest
{
	/// <summary>
	/// 한 통계 입력 결과.
	/// </summary>
	[Serializable]
	public class SetStatsResultStatItem
	{
		/// <summary>
		/// 통계 ID.
		/// </summary>
		public string id;
		/// <summary>
		/// 입력된 통계 값.
		/// </summary>
		public int val;
	}

	/// <summary>
	/// 한 도전과제 달성 결과.
	/// </summary>
	[Serializable]
	public class SetStatsResultAchieveItem
	{
		/// <summary>
		/// 도전과제 ID.
		/// </summary>
		public string id;
		/// <summary>
		/// 도전과제 이름.
		/// </summary>
		public string name;
		/// <summary>
		/// 도전과제 설명.
		/// </summary>
		public string desc;
		/// <summary>
		/// 도전과제 아이콘 이미지 ID.
		/// </summary>
		public string img;
		/// <summary>
		/// 도전과제 달성 시간. (UNIX 시간(ms))
		/// </summary>
		public long regDate;
		/// <summary>
		/// 연동된 통계 ID. (없으면 공백.)
		/// </summary>
		public string statId;
		/// <summary>
		///  연동된 통계 목푯값. (없으면 0.)
		/// </summary>
		public int targetStatVal;
		/// <summary>
		/// 도전과제 아이콘 이미지 ID.
		/// </summary>
		public string ImageUrl => $"{Wakgames.Host}/img/{img}";
		/// <summary>
		/// 도전과제 달성 시간.
		/// </summary>
		public DateTimeOffset RegDate => DateTimeOffset.FromUnixTimeMilliseconds(regDate);
		/// <summary>
		/// 연동된 통계 유무.
		/// </summary>
		public bool StatConnected => !string.IsNullOrEmpty(statId) && targetStatVal != 0;
	}

	/// <summary>
	/// 통계 입력 결과 목록.
	/// </summary>
	[Serializable]
	public class SetStatsResult
	{
		/// <summary>
		/// 입력된 통계들.
		/// </summary>
		public List<SetStatsResultStatItem> stats;
		/// <summary>
		/// 새로 달성된 도전과제들.
		/// </summary>
		public List<SetStatsResultAchieveItem> achieves;
	}
}