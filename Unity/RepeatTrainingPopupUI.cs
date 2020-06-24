using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingResultBaseInfo
{
	public  TrainingPhase   phase;
	public  TrainingGrade   grade;
	public  PlayerBase      prevPlayer;
	public  PlayerBase      curPlayer;
	public  int[]           addStats    = new int[ (int)PlayerAddStat.Max ];

	public void SetInfo( TrainingPhase phase, TrainingGrade grade, PlayerBase prevPlayer, PlayerBase curPlayer, int[] addStats )
	{
		this.phase = phase;
		this.grade = grade;
		this.prevPlayer = prevPlayer.Clone();
		this.curPlayer = curPlayer.Clone();
		this.addStats = (int[])addStats.Clone();
	}
}

public class TrainingResultInfo
{
	public int  selectCount; //!< 훈련 몇번 할 지 선택한 숫자
	public int  completeCount; //!< 훈련 완료한 숫자
	public byte	targetGrade; //!< 훈련 목표
	public byte	currGrade; //!< 현재 훈련 결과

	public bool	completeTarget	{ get { return ( targetGrade == currGrade ) ? true : false; } } //!< 선택한 목표에 이르렀을 때
	public bool	completeTraining	{ get { return ( selectCount <= completeCount ) ? true : false; } } //!< 정해진 횟수를 채웠을 때
	public bool	isTraining	{ get { return ( 0 < completeCount ) ? true : false; } } //!< 훈련 완료 숫자 1이상

	public List<TrainingGrade>		trainingResults	=	new List<TrainingGrade>(); //!< 그동안 나온 결과들 grade
	
	public bool	isValid	{ get { return ( ( isTraining && !completeTraining ) || !completeTarget ) ? true : false; } }
	public bool	isEnd	{ get { return ( ( isTraining && completeTraining ) || completeTarget ) ? true : false; } } //!< 정해진 횟수를 채웠거나 목표를 완료했을 때, 반복 훈련 완료

	public int	selectIdx	{ get { return ( selectCount - 1 ); } }		//!< 선택 갯수
	public int	resultIdx	{ get { return ( completeCount - 1 ); } }	//!< result icon 위한 결과 index
	
	public void SetTrainingInfo( int selectCount, int completeCount, byte targetGrade, byte currGrade, List<TrainingGrade> lstGrade )
	{
		this.selectCount = selectCount;
		this.completeCount = completeCount;
		this.targetGrade = targetGrade;
		this.currGrade = currGrade;
		this.trainingResults = lstGrade;
	}
}

public partial class RepeatTrainingPopupUI : UIBase
{
	static public RepeatTrainingPopupUI Open( int originPoint, int originResetCost, System.Action<int, int, int, int> onClickOk )
	{
		RepeatTrainingPopupUI uiBox = MainUI.ActiveUI<RepeatTrainingPopupUI>( UI.RepeatTrainingPopupUI, new UIResc( "TeamManage/Upgrade/RepeatTrainingPopupUI" ) );

		if( uiBox != null )
		{
			uiBox.Apply( originPoint, originResetCost );
			uiBox.onClickOk = onClickOk;
		}
		uiBox.SetActive( true );
		return uiBox;
	}
}

public partial class RepeatTrainingPopupUI : UIBase
{
	[SerializeField] RepeatTrainingInfoUI[] trainingGoalUI	= null;
	[SerializeField] RepeatTrainingInfoUI[] trainingCountUI	= null;

	[Space( 10 )] //!< title
	[SerializeField] UILabel	label_targetTitle		= null;
	[SerializeField] UILabel	label_countTitle		= null;

	[Space( 10 )] //!< totalCost
	[SerializeField] UILabel	label_OriginPoint		= null;
	[SerializeField] UILabel	label_Point				= null;	
	[SerializeField] UILabel	label_OriginResetDia	= null;
	[SerializeField] UILabel	label_ResetDia			= null;

	[Space( 10 )] //!< common
	[SerializeField] ToolTip	toolTipUI			= null;
	[SerializeField] UILabel	label_TrnningDesc	= null;
	
	[Space( 10 )] //!< Btn_Ok_Cancel
	[SerializeField] GameObject go_OKBtn        = null;
	[SerializeField] UILabel    label_OKBtn     = null;
	[SerializeField] GameObject	go_CancelBtn	= null;
	[SerializeField] UILabel	label_CancelBtn	= null;

	[Space( 10 )] //!< perfect event
	[SerializeField] EventDiscount  perfectEvent		= null;
	
	private int originPoint			= -1;
	private int originResetCost		= -1;

	private int selectGrade			= -1;
	private int selectTranningCount = -1;
	private int nResetCount			{ get { return ( selectTranningCount - 1 ); } }

	private int	needPoint		= -1;
	private int	needResetCost	= -1;

	private bool isEnableTranning = false;

	private System.Action<int, int, int, int>	onClickOk		= null;

	public void OnClose()
	{
		//AppSound.PlayUISound( UIEffectSound.Cancel );
		SetActive( false );
	}

	private void Initailize()
	{
		selectGrade = (int)TrainingGrade.Perfect; //!< 퍼펙트
		selectTranningCount = 1; //!< 1회

		originPoint = -1;
		originResetCost = -1;

		needPoint = -1;
		needResetCost = -1;

		isEnableTranning = false;
		
		InitLable();
	}
	private void InitLable()
	{
		UIUtil.SetLabel( label_targetTitle, 925760 ); //!< 훈련 목표
		UIUtil.SetLabel( label_countTitle, 925761 ); //!< 훈련 진행 횟수
		UIUtil.SetLabel( label_OKBtn, 900081 );
		UIUtil.SetLabel( label_CancelBtn, 900082 );
		UIUtil.SetLabel( label_TrnningDesc, 925758 );

		if( toolTipUI != null )
			toolTipUI.SetString( Manage.Utility.GetString( 925763 ) ); //!< 훈련 진행 중 목표한 훈련 결과가 나올 경우 훈련이 자동 정지 됩니다.
		
		UIUtil.SetActive( toolTipUI, true );
	}

	private void Apply( int originPoint, int originResetCost )
	{
		Initailize();

		this.originPoint = originPoint;
		this.originResetCost = originResetCost;
		
		SetPerfectEvent();

		List<ManageTrainingOptionData> data = ManageTrainingOptionTBL.GetData();
		SetToggleUI( (int)ManageTrainingOptionData.Type.Grade, data );
		SetToggleUI( (int)ManageTrainingOptionData.Type.Count, data );
		
		SetCostLabel();
	}
	private void SetPerfectEvent()
	{
		int evtRate = Net.MANAGE_LOBBY_INFO.trainingPerfectEvtRate;
		perfectEvent.SetActive( evtRate > 0 ? true : false );
		perfectEvent.SetString( evtRate, 925765 ); //!< Perfect 확률 {0}배 증가
	}

	private void SetToggleUI( int type, List<ManageTrainingOptionData> data )
	{
		List<ManageTrainingOptionData> toggleData = new List<ManageTrainingOptionData>();

		if( data == null )
			return;

		for( int i = 0; i < data.Count; i++ )
		{
			if( data[ i ].optionType == type )
				toggleData.Add( data[ i ] );
		}

		if( type == 1 )
		{
			toggleData.Sort( ( ManageTrainingOptionData a, ManageTrainingOptionData b ) => b.tranningGrade.CompareTo( a.tranningGrade ) );

			for( int i = 0; i < toggleData.Count; i++ )
			{
				if( trainingGoalUI[ i ] != null && toggleData[ i ] != null )
				{
					trainingGoalUI[ i ].Apply( toggleData[ i ].optionType, toggleData[ i ].tranningGrade );
					trainingGoalUI[ i ].onClick = OnClickToggle;
				}
			}

			//!< toggle icon init
			for( int i = 0; i < trainingGoalUI.Length; i++ )
			{
				trainingGoalUI[ i ].SetToggle( selectGrade );
			}
		}
		else
		{
			for( int i = 0; i < toggleData.Count; i++ )
			{
				if( trainingCountUI[ i ] != null && toggleData[ i ] != null )
				{
					trainingCountUI[ i ].Apply( toggleData[ i ].optionType, toggleData[ i ].tranningCount );
					trainingCountUI[ i ].onClick = OnClickToggle;
				}
			}

			//!< toggle icon init
			for( int i = 0; i < trainingCountUI.Length; i++ )
			{
				trainingCountUI[ i ].SetToggle( selectTranningCount );
			}
		}
	}

	private void SetCostLabel()
	{
		SetPointCostLabel();
		SetResetCostLabel();
	}
	private void SetPointCostLabel()
	{
		int evtRate = Net.MANAGE_LOBBY_INFO.trainingPointEvtRate;
		int totalOriginPoint = originPoint * selectTranningCount;

		needPoint = Mathf.FloorToInt( originPoint * ( 100f - (float)evtRate ) / 100f );
		int totalPoint = needPoint * selectTranningCount;

		bool isPointEvent = ( evtRate > 0 && totalPoint > 0 ) ? true : false;
		UIUtil.SetActive( label_OriginPoint, isPointEvent );

		if( isPointEvent )
			UIUtil.SetLabel( label_OriginPoint, totalOriginPoint.ToString( "N0" ) );

		UIUtil.SetLabel( label_Point, totalPoint.ToString( "N0" ) );

		isEnableTranning = ( totalPoint >= UserBase.Inst.trainingPoint.point ) ? false : true;
	}

	private void SetResetCostLabel()
	{
		int evtResetRate = Net.MANAGE_LOBBY_INFO.trainingResetEvtRate;
		int totalOriginResetCost = originResetCost * nResetCount;
		
		needResetCost = Mathf.FloorToInt( originResetCost * ( 100f - (float)evtResetRate ) / 100f );
		int totalResetCost = needResetCost * nResetCount;

		bool isResetEvent = ( evtResetRate > 0 && totalResetCost > 0 ) ? true : false;
		UIUtil.SetActive( label_OriginResetDia, isResetEvent );
		
		if( isResetEvent )
			UIUtil.SetLabel( label_OriginResetDia, totalOriginResetCost.ToString( "N0" ) );
		
		UIUtil.SetLabel( label_ResetDia, totalResetCost.ToString( "N0" ) );

		isEnableTranning = ( (long)totalResetCost >= UserBase.Inst.GetMoney( E_GAME_MONEY.eGM_CASH ) ) ? false : true;
	}

	public void OnClickToggle( int type, int value )
	{
		if( type == 1 )
			selectGrade = value;
		else
		{
			selectTranningCount = value;
			SetCostLabel();
		}
	}

	public void OnClickOk()
	{
		if( isEnableTranning == false )
		{
			MessageUI.Active( 925757 ); //!< 훈련 포인트와 다이아가 부족하여 훈련을 진행 할 수 없습니다.\n확인 후 다시 시도해 주세요.
			return;
		}

		if( onClickOk != null )
		{
			onClickOk( selectGrade, selectTranningCount, needPoint, needResetCost );
		}

		OnClose();
	}
}