// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.

using System;
using HutongGames.PlayMaker;

namespace HutongGames.PlayMaker.Ecosystem.Utils
{

	[AttributeUsage(AttributeTargets.All)]
	public class FsmVariableType : Attribute
	{
		public VariableType variableType;

		public FsmVariableType(VariableType variableType)
		{
			this.variableType = variableType;
		}
	}

}