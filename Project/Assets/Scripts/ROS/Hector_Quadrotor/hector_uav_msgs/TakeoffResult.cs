﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using uint8 = System.Byte;
using Messages.geometry_msgs;
using Messages.sensor_msgs;
using Messages.actionlib_msgs;
using Messages;
using Messages.std_msgs;
using String=System.String;
using hector_uav_msgs;
using actionlib;

// WTF?? completely empty struct..

namespace hector_uav_msgs
{
	#if !TRACE
	[System.Diagnostics.DebuggerStepThrough]
	#endif
	public class TakeoffResult : AResult
	{

		TakeoffResult (TakeoffResult tr) {}

		public override AResult Clone ()
		{
			return new TakeoffResult ( this );
		}


		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "d41d8cd98f00b204e9800998ecf8427e"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return true; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @""; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__TakeoffResult; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public TakeoffResult()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public TakeoffResult(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public TakeoffResult(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override byte[] Serialize(bool partofsomethingelse)
		{
			return new byte[0];
		}

		public override void Randomize()
		{
		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			return true;
		}
	}
}
