﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingUI : MonoBehaviour
{
	public bool isTrainingMode;

	public FPSRobotInput robotInput;
	public IRobotController robotController;

	public RectTransform trainingArea;
	public RectTransform rightArea;
	public Text saveStatus;
	public Text recordStatus;

	bool recording;
	bool saveRecording;

	void Awake ()
	{
		robotController = robotInput.controller;
		SetTrainingMode ( isTrainingMode );
		saveStatus.text = "";
	}

	void LateUpdate ()
	{
		if ( robotController.getSaveStatus () )
		{
			saveStatus.text = "Capturing Data: " + (int) ( 100 * robotController.getSavePercent () ) + "%";
		}
		else if(saveRecording) 
		{
			saveStatus.text = "";
			recording = false;
//			RecordingPause.SetActive(false);
			recordStatus.text = "Not Recording";
			recordStatus.color = Color.red;
			saveRecording = false;
		}

		if ( Input.GetButtonDown ( "Record" ) )
			ToggleRecording ();


	}

	void ToggleRecording ()
	{
		if ( recording )
		{
			saveRecording = true;
			robotController.IsRecording = false;
//			robotInput.DisableFocus = true;
//			robotInput.Unfocus ();

		} else
		{
			if ( robotController.CheckSaveLocation ( OnBeginRecord ) )
			{
				OnBeginRecord ();
//				recording = true;
//				robotController.IsRecording = true;
//				recordStatus.text = "RECORDING";
//				robotInput.DisableFocus = false;
//				robotInput.Focus ();
			} else
			{
				robotInput.DisableFocus = true;
				robotInput.Unfocus ();
			}
		}
	}

	void OnBeginRecord ()
	{
		recording = true;
		robotController.IsRecording = true;
		recordStatus.text = "RECORDING";
		recordStatus.color = Color.green;
		robotInput.DisableFocus = false;
		robotInput.Focus ();
	}

	public void SetTrainingMode (bool training)
	{
		isTrainingMode = training;
		trainingArea.gameObject.SetActive ( isTrainingMode );
		rightArea.gameObject.SetActive ( isTrainingMode );
		enabled = isTrainingMode;
	}
}