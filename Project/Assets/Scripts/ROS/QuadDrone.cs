﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Ros_CSharp;
using hector_uav_msgs;
using Messages;
using TwistStamped = Messages.geometry_msgs.TwistStamped;
using GVector3 = Messages.geometry_msgs.Vector3;
using PoseStamped = Messages.geometry_msgs.PoseStamped;
using Wrench = Messages.geometry_msgs.Wrench;
using Imu = Messages.sensor_msgs.Imu;
using Image = Messages.sensor_msgs.Image;
using Path = Messages.nav_msgs.Path;
using GetPlan = Messages.nav_msgs.GetPlan;


/*
 * QuadDrone: receives messages from a QRKeyboardTeleop, and applies force/torque to a QuadController
 */

public class QuadDrone : MonoBehaviour
{
	public QuadController droneController;
	public bool active;

	NodeHandle nh;
	ServiceServer setOrientSrv;
	ServiceServer enableMotorSrv;
	Publisher<PoseStamped> posePub;
	Publisher<Imu> imuPub;
	Publisher<Image> imgPub;
	ServiceServer pathSrv;
	Subscriber<TwistStamped> twistSub;
	Subscriber<Wrench> wrenchSub;
	Thread pubThread;

	uint frameSeq = 0;

	void Awake ()
	{
		if ( !active )
		{
			enabled = false;
			return;
		}
		ROSController.StartROS ( OnRosInit );
	}

	void OnRosInit ()
	{
		nh = new NodeHandle ( "~" );
		pathSrv = nh.advertiseService<GetPlan.Request, GetPlan.Response> ( "quad_rotor/path", PathService );
//		setOrientSrv = nh.advertiseService<Messages.std_srvs.Empty.Request>
//		enableMotorSrv = nh.advertiseService<EnableMotors.Request, EnableMotors.Response> ( "enable_motors", OnEnableMotors );
		nh.setParam ( "control_mode", "wrench" ); // for now force twist mode
//		twistSub = nh.subscribe<TwistStamped> ( "command/twist", 10, TwistCallback );
		wrenchSub = nh.subscribe<Wrench> ( "quad_rotor/cmd_force", 10, WrenchCallback );
		posePub = nh.advertise<PoseStamped> ( "quad_rotor/pose", 10, false );
		imuPub = nh.advertise<Imu> ( "quad_rotor/imu", 10, false );
		imgPub = nh.advertise<Image> ( "quad_rotor/image", 10, false );
		pubThread = new Thread ( PublishAll );
		pubThread.Start ();
	}

	bool OnEnableMotors (EnableMotors.Request req, ref EnableMotors.Response resp)
	{
		if ( droneController != null )
		{
			droneController.MotorsEnabled = req.enable;
			resp.success = true;
			return true;
		}

		resp.success = false;
		return false;
	}

	void TwistCallback (TwistStamped msg)
	{
		Vector3 linear = msg.twist.linear.ToUnityVector ();
		Vector3 angular = msg.twist.angular.ToUnityVector ();
		if ( droneController != null )
		{
			droneController.ApplyMotorForce ( linear, true );
			droneController.ApplyMotorTorque ( angular, true );
//			droneController.ApplyMotorForce ( (float) linear.x, (float) linear.y, (float) linear.z, true, true );
//			droneController.ApplyMotorTorque ( (float) angular.x, (float) angular.y, (float) angular.z, true, true );
		}
	}

	void WrenchCallback (Wrench msg)
	{
		Vector3 force = msg.force.ToUnityVector ();
		Vector3 torque = msg.torque.ToUnityVector ();
		if ( droneController != null )
		{
			if ( !droneController.MotorsEnabled )
				droneController.MotorsEnabled = true;
			droneController.ApplyMotorForce ( force, true );
			droneController.ApplyMotorTorque ( torque, true );
//			droneController.ApplyMotorForce ( force.x, force.y, force.z, true, true );
//			droneController.ApplyMotorTorque ( torque.x, torque.y, torque.z, true, true );
		}
	}

	void PublishAll ()
	{
		// pose info
		PoseStamped ps = new PoseStamped ();
		ps.header = new Messages.std_msgs.Header ();
		ps.pose = new Messages.geometry_msgs.Pose ();
		Imu imu = new Imu ();
		// imu info
		imu.header = new Messages.std_msgs.Header ( ps.header );
		imu.angular_velocity_covariance = new double[9] { -1, 0, 0, 0, 0, 0, 0, 0, 0 };
		imu.linear_acceleration_covariance = new double[9] { -1, 0, 0, 0, 0, 0, 0, 0, 0 };
		imu.orientation_covariance = new double[9] { -1, 0, 0, 0, 0, 0, 0, 0, 0 };
		// image info
		Image img = new Image ();
		img.header = new Messages.std_msgs.Header ( ps.header );
		img.width = (uint) QuadController.ImageWidth;
		img.height = (uint) QuadController.ImageHeight;
		img.encoding = "mono16"; // "rgba8";
		img.step = img.width * 2; // * 4
		img.is_bigendian = 1;


		int sleep = 1000 / 60;
		Vector3 testPos = Vector3.zero;
		while ( ROS.ok && !ROS.shutting_down )
		{
			// publish pose
			ps.header.frame_id = "";
			ps.header.seq = frameSeq;
			ps.header.stamp = ROS.GetTime ();
			ps.pose.position = new Messages.geometry_msgs.Point ( droneController.Position.ToRos () );
//			ps.pose.position = new Messages.geometry_msgs.Point ( droneController.Position, true, true );
			ps.pose.orientation = new Messages.geometry_msgs.Quaternion ( droneController.Rotation.ToRos () );
			posePub.publish ( ps );

			// publish imu
			imu.header.frame_id = "";
			imu.header.seq = frameSeq++;
			imu.header.stamp = ps.header.stamp;
			imu.angular_velocity = new GVector3 ( droneController.AngularVelocity.ToRos () );
//			imu.angular_velocity = new GVector3 ( droneController.AngularVelocity, true, true );
			imu.linear_acceleration = new GVector3 ( droneController.LinearAcceleration.ToRos () );
//			imu.linear_acceleration = new GVector3 ( droneController.LinearAcceleration, true, true );
			imu.orientation = new Messages.geometry_msgs.Quaternion ( droneController.Rotation.ToRos () );
			imuPub.publish ( imu );

			// publish image
			img.data = droneController.GetImageData ();
			imgPub.publish ( img );
			
			Thread.Sleep ( sleep );
		}
	}

	bool PathService (GetPlan.Request req, ref GetPlan.Response resp)
	{
		Debug.Log ( "path service called!" );
		Path path = new Path ();
		path.header = new Messages.std_msgs.Header ();
		path.header.frame_id = "";
		path.header.stamp = ROS.GetTime ();
		path.header.seq = 0;
		PathSample[] samples = PathPlanner.GetPath ();
		int count = samples.Length;
		path.poses = new PoseStamped[ count ];
		Debug.Log ( "sending " + count + " samples" );
		for ( int i = 0; i < count; i++ )
		{
			PoseStamped pst = new PoseStamped ();
			pst.header = new Messages.std_msgs.Header ();
			pst.header.frame_id = "";
			pst.header.stamp = ROS.GetTime ();
			pst.header.seq = (uint) i;
			pst.pose = new Messages.geometry_msgs.Pose ();
			pst.pose.position = new Messages.geometry_msgs.Point ( samples [ i ].position.ToRos () );
			pst.pose.orientation = new Messages.geometry_msgs.Quaternion ( samples [ i ].orientation.ToRos () );
			path.poses [ i ] = pst;
		}
		resp.plan = path;
		return true;
	}
}