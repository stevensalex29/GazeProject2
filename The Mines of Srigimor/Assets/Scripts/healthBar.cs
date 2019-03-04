using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour {


	private Transform bar;

	private void Start () {
	 	bar = transform.Find("Bar");

	}
	
	public void SetSize(float sizeNormailzed){
		bar.localScale = new Vector3 (sizeNormailzed, 1f);
	}
}
