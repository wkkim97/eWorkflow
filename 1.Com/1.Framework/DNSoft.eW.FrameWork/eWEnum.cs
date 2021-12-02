using System;

namespace DNSoft.eW.FrameWork
{
	/// <summary>
	/// 서브시스템타입
	/// </summary>
	public enum SubSystemType 
	{ 
		Common	= 0, 
		eManage = 10000, 
		Approval = 10100
	};

	/// <summary>
	/// Layer타입
	/// </summary>
	public enum LayerType 
	{ 
		GUI = 10, 
		BSL = 20, 
		SQL = 30, 
		DAL = 40, 
		USR = 50, 
		DSL = 60, 
		EXCH = 70,
		AD = 80,
		MQ = 90,
		ETC = 100,
		SPS = 110,
		WIN = 120
	};

	/// <summary>
	/// 메시지타입
	/// </summary>
	public enum MessageItem
	{
		SubSystemType = 0,
		MessageID = 1,
		MessageType = 2,
		DisplayMessage = 3,
		SummaryMessage = 4
	}


	#region 장기적 삭제대상 (안기홍)
	/// <summary>
	/// Language타입
	/// </summary>
	public enum LanguageSet
	{
		Korean,
		English,
		Japanese
	}
     

	/// <summary>
	/// Frame타입
	/// </summary>
	public enum FrameType
	{
		TopFrame = 1,
		SubFrame,
		MenuFrame,
		ContentFrame
	};

	/// <summary>
	/// 권한의 종류
	/// </summary>
	public enum AuthorityEnum
	{
		NULL = 0,
		ALLOW = 1,
		DENY = 2
	};
	#endregion
}
