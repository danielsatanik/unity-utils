using UnityEngine;
using System;
using UnityUtils.CustomTypes;
using UnityUtils.Engine.Attributes;

namespace UnityUtils.Debugging
{
	[Serializable]
	public sealed class LoggerSettings : ScriptableObject
	{
		public bool AutoLoad;
		public SceneList SceneNames;
		public string CustomPrefix;
		[Flag]
		public LoggerOption Options = 
			LoggerOption.ADD_TIME_STAMP |
			LoggerOption.BREAK_ON_ASSERT |
			LoggerOption.BREAK_ON_ERROR |
			LoggerOption.ECHO_TO_CONSOLE;
		public LoggerErrorType ErrorType = LoggerErrorType.Trace;
		public uint RotateSize;
	}
}