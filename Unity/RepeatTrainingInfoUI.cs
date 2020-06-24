using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatTrainingInfoUI : UIBase
{
	[SerializeField] UISprite		sprite_Color	= null;
	[SerializeField] UILabel		label_Desc		= null;
	[SerializeField] UIToggle		toggleUI		= null;

	public System.Action<int, int>	onClick	= null;

	private int		type	= -1;
	private int		value	= -1;

	private bool	isValid	{ get { return ( type > 0 || value > 0 ) ? true : false;  } }
	
	public void Apply( int type, int value )
	{
		this.type = type;
		this.value = value;

		SetLabel();
	}

	private void SetLabel()
	{
		if( type == (int)ManageTrainingOptionData.Type.Grade )
		{
			UIUtil.SetColor( sprite_Color, TeamManage.Upgrade.Utility.GetColor_TrainingGrade( (TrainingGrade)value ) );
			UIUtil.SetColor( label_Desc, TeamManage.Upgrade.Utility.GetColor_TrainingGrade( (TrainingGrade)value ) );
			UIUtil.SetLabel( label_Desc, ManageTrainingOptionTBL.GetGradeStringID( value ) );
		}
		else
		{
			UIUtil.SetLabel( label_Desc, string.Format( Manage.Utility.GetString( 925759 ), value ) );
		}
	}

	public void SetToggle( int value )
	{
		if( toggleUI != null )
		{
			toggleUI.Set( ( value == this.value ) ? true : false );
		}
	}

	public void OnClick()
	{
		if( isValid == false )
			return;

		if( onClick != null )
			onClick( type, value );
	}
}