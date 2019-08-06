// source: http://www.reddit.com/r/Unity3D/comments/2qsor5/heres_a_way_to_call_methods_on_components_without/

using UnityEngine;
using System.Reflection;


namespace HutongGames.PlayMaker.Ecosystem.Utils
{
	public class ButtonAttribute : PropertyAttribute {
		public string methodName;
		public string buttonName;
		public bool useValue;
		public BindingFlags flags;
		
		public ButtonAttribute(string methodName, string buttonName, bool useValue, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance) {
			this.methodName = methodName;
			this.buttonName = buttonName;
			this.useValue = useValue;
			this.flags = flags;
		}
		public ButtonAttribute(string methodName, bool useValue, BindingFlags flags) : this (methodName, methodName, useValue, flags) {}
		public ButtonAttribute(string methodName, bool useValue) : this (methodName, methodName, useValue) {}
		public ButtonAttribute(string methodName, string buttonName, BindingFlags flags) : this (methodName, buttonName, false, flags) {}
		public ButtonAttribute(string methodName, string buttonName) : this (methodName, buttonName, false) {}
		public ButtonAttribute(string methodName, BindingFlags flags) : this (methodName, methodName, false, flags) {}
		public ButtonAttribute(string methodName) : this (methodName, methodName, false) {}
	}
}