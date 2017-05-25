﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HectorQuadController : MonoBehaviour
{
	public bool MotorsEnabled { get; set; }
	public Vector3 Force { get { return force; } }
	public Vector3 Torque { get { return torque; } }

	public Transform frontLeftRotor;
	public Transform frontRightRotor;
	public Transform rearLeftRotor;
	public Transform rearRightRotor;

	public float thrustForce = 2000;
	public float torqueForce = 500;
	public ForceMode forceMode = ForceMode.Force;
	public ForceMode torqueMode = ForceMode.Force;

	public Transform cameraOrientationTarget;

	Rigidbody rb;
	Transform[] rotors;
	Vector3 force;
	Vector3 torque;

	void Awake ()
	{
		rb = GetComponent<Rigidbody> ();
		rotors = new Transform[4] {
			frontLeftRotor,
			frontRightRotor,
			rearLeftRotor,
			rearRightRotor
		};
	}

	void LateUpdate ()
	{
		if ( Input.GetKeyDown ( KeyCode.Escape ) )
			Application.Quit ();

		if ( Input.GetKeyDown ( KeyCode.R ) )
		{
			ResetOrientation ();
		}


		float zAngle = 0;
		Vector3 up = transform.up;
		if ( up.y >= 0 )
			zAngle = transform.localEulerAngles.z;
		else
			zAngle = -transform.localEulerAngles.z;
		while ( zAngle > 180 )
			zAngle -= 360;
		while ( zAngle < -360 )
			zAngle += 360;
		transform.Rotate ( Vector3.up * -zAngle * Time.deltaTime, Space.World );
//		cameraOrientationTarget.Rotate ( Vector3.up * -zAngle * Time.deltaTime, Space.World );
	}

	void FixedUpdate ()
	{
		if ( MotorsEnabled )
		{
			// add force
			rb.AddRelativeForce ( force * Time.deltaTime, forceMode );

			// add torque
			rb.AddRelativeTorque ( torque * Time.deltaTime, torqueMode );
//			rb.AddTorque ( torque * Time.deltaTime, torqueMode );
		}
	}

	public void ApplyMotorForce (float x, float y, float z, bool swapAxes = false)
	{
		force.x = x;
		force.y = swapAxes ? z : y;
		force.z = swapAxes ? y : z;
		force *= thrustForce;
	}

	public void ApplyMotorTorque (float x, float y, float z, bool swapAxes = false)
	{
		torque.x = x;
		torque.y = swapAxes ? z : y;
		torque.z = swapAxes ? y : z;
		torque *= torqueForce;
	}

	public void ResetOrientation ()
	{
		transform.rotation = Quaternion.identity;
		force = Vector3.zero;
		torque = Vector3.zero;
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}
}