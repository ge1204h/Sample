using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngineInternal;

/// <summary>
/// UnityEngine.Debug를 재정의하여 디버그 로그를 비활성화합니다.
/// Editor 에서만 Debug.log 사용 && 빌드 시 로그 보고싶으면 SHOW_LOGS 디파인 사용
/// 
/// 참고 :
///      [Conditional] 속성은 메서드 호출 또는 속성이
///      지정된 조건부 컴파일 기호가 정의되지 않으면 무시됩니다.
/// 
/// 참조 :
/// http://msdn.microsoft.com/en-us/library/system.diagnostics.conditionalattribute.aspx
/// 
/// </summary>

public static class Debug
{
	public static bool isDebugBuild
	{
		get { return UnityEngine.Debug.isDebugBuild; }
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void Assert( bool condition )
	{
		UnityEngine.Debug.Assert( condition );
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void Break()
	{
		UnityEngine.Debug.Break();
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void DebugBreak()
	{
		UnityEngine.Debug.DebugBreak();
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void LogException( Exception ex )
	{
		UnityEngine.Debug.LogException( ex );
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void DrawLine( Vector3 start, Vector3 end, Color color = default( Color ), float duration = 0.0f, bool depthTest = true )
	{
		UnityEngine.Debug.DrawLine( start, end, color, duration, depthTest );
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void DrawRay( Vector3 start, Vector3 dir, Color color = default( Color ), float duration = 0.0f, bool depthTest = true )
	{
		UnityEngine.Debug.DrawLine( start, dir, color, duration, depthTest );
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void Log( object message )
	{
		UnityEngine.Debug.Log( message );
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void Log( object message, UnityEngine.Object context )
	{
		UnityEngine.Debug.Log( message, context );
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void LogError( object message )
	{
		UnityEngine.Debug.LogError( message );
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void LogError( object message, UnityEngine.Object context )
	{
		UnityEngine.Debug.LogError( message, context );
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void LogWarning( object message )
	{
		UnityEngine.Debug.LogWarning( message );
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void LogWarning( object message, UnityEngine.Object context )
	{
		UnityEngine.Debug.LogWarning( message, context );
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void LogFormat(string format, params object[] args)
	{
		UnityEngine.Debug.LogFormat( format, args );
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void LogWarningFormat( string format, params object[] args )
	{
		UnityEngine.Debug.LogWarningFormat( format, args );
	}

	[System.Diagnostics.Conditional( "UNITY_EDITOR" ), System.Diagnostics.Conditional( "SHOW_LOGS" )]
	public static void LogErrorFormat(string format, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat( format, args );
	}
}