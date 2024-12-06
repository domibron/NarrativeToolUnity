using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace SmashKeyboardStudios.NarrativeTool.Editor
{
	public abstract class BaseNode : Node
	{
		/// <summary>
		/// The unqiue generated id for the node, it must be unique to this node and only this.
		/// </summary>
		internal GUID NodeGUID;


	}
}
