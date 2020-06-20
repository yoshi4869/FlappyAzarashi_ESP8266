﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SerialLight : MonoBehaviour {

	public SerialHandler serialHandler;
	public Text text;

	// Use this for initialization
	void Start () {
		//信号を受信したときに、そのメッセージの処理を行う
		serialHandler.OnDataReceived += OnDataReceived;
	}
	// Update is called once per frame
	void Update () {
	}
	/*
	 * シリアルを受け取った時の処理
	 */
	void OnDataReceived(string message) {
		try {
			text.text = message; // シリアルの値をテキストに表示
		} catch (System.Exception e) {//tryの処理中に例外が発生したらtryを中断してエラーメッセージを出す
			Debug.LogWarning(e.Message);
		}
	}
}