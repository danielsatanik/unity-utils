using UnityEngine;

namespace UnityUtils.Engine.Attributes
{
	[System.AttributeUsage (System.AttributeTargets.Field | System.AttributeTargets.Enum, AllowMultiple = true)]
	public class FlagAttribute : PropertyAttribute
	{
		public string EnumName;

		public FlagAttribute ()
		{
		}

		public FlagAttribute (string name)
		{
			EnumName = name;
		}
	}
}