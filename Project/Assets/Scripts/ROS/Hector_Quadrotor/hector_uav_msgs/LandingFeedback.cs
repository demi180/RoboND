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

namespace hector_uav_msgs
{
	#if !TRACE
	[System.Diagnostics.DebuggerStepThrough]
	#endif
	public class LandingFeedback : AFeedback
//	public class LandingFeedback : IRosMessage
	{
		PoseStamped current_pose;


		LandingFeedback (LandingFeedback lf)
		{
			current_pose = new PoseStamped ();
			current_pose.pose = new Pose ();
			current_pose.pose.orientation = new Quaternion ();
			current_pose.pose.orientation.x = lf.current_pose.pose.orientation.x;
			current_pose.pose.orientation.y = lf.current_pose.pose.orientation.y;
			current_pose.pose.orientation.z = lf.current_pose.pose.orientation.z;
			current_pose.pose.orientation.w = lf.current_pose.pose.orientation.w;
			current_pose.pose.position = new Point ();
			current_pose.pose.position.x = lf.current_pose.pose.position.x;
			current_pose.pose.position.y = lf.current_pose.pose.position.y;
			current_pose.pose.position.z = lf.current_pose.pose.position.z;
			current_pose.header = new Header ();
			current_pose.header.Frame_id = lf.current_pose.header.Frame_id;
			current_pose.header.Seq = lf.current_pose.header.Seq;
			current_pose.header.Stamp = new Time ( lf.current_pose.header.Stamp.data );
		}

		public override AFeedback Clone ()
		{
			return new LandingFeedback ( this );
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MD5Sum() { return "dd7058fae6e1bf2400513fe092a44c92"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool HasHeader() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsMetaType() { return false; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override string MessageDefinition() { return @"PoseFeedback current_pose"; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override MsgTypes msgtype() { return MsgTypes.hector_uav_msgs__LandingFeedback; }
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override bool IsServiceComponent() { return false; }

		[System.Diagnostics.DebuggerStepThrough]
		public LandingFeedback()
		{

		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public LandingFeedback(byte[] SERIALIZEDSTUFF)
		{
			Deserialize(SERIALIZEDSTUFF);
		}

		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public LandingFeedback(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			Deserialize(SERIALIZEDSTUFF, ref currentIndex);
		}



		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override void Deserialize(byte[] SERIALIZEDSTUFF, ref int currentIndex)
		{
			current_pose = new PoseStamped ( SERIALIZEDSTUFF, ref currentIndex );
		}

		[System.Diagnostics.DebuggerStepThrough]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		public override byte[] Serialize(bool partofsomethingelse)
		{
			return current_pose.Serialize ( partofsomethingelse );
		}

		public override void Randomize()
		{
			current_pose.Randomize ();

		}

		public override bool Equals(IRosMessage ____other)
		{
			if (____other == null) return false;
			hector_uav_msgs.LandingFeedback other = (hector_uav_msgs.LandingFeedback)____other;

			return current_pose.Equals ( other.current_pose );
		}
	}
}
