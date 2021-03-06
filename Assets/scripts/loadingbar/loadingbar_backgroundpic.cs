using UnityEngine;
using KBEngine;
using System;
using System.Collections;

public class loadingbar_backgroundpic : MonoBehaviour {

	public static UITexture obj = null;
	public static loadingbar_backgroundpic inst = null;
	public static Shader Transparent_Colored = null;

	private float fadeMax = 1.5f;
	private float fadeTime;
	public bool startFadeOut = false;
	
	void Awake ()     
	{
		obj = GetComponent<UITexture>();
		inst = this;
	}
	
	// Use this for initialization
	void Start () {
		obj.shader = Transparent_Colored;
		loader.inst.StartCoroutine(getFullTexture());
		NGUITools.SetActive(obj.gameObject, false);
	}
	 
	IEnumerator getFullTexture(){
		// if(obj.mainTexture == null || obj.mainTexture.name == "" || obj.mainTexture.name.IndexOf("_min") > -1)
		{
			//string[] res = obj.mainTexture.name.Split(new char[]{'_', 'm', 'i', 'n'});
			
			System.Random Random1 = new System.Random();
			string path = "/ui/bg/loadingscreen_" + Random1.Next(1,6) + ".jpg";
			Common.DEBUG_MSG("loadingbar_backgroundpic::getFullTexture: starting download(" + Common.safe_url(path) + ") backgroundpic! curr=" + obj.mainTexture.name);
			
			WWW www = new WWW(Common.safe_url(path));  
			yield return www; 
			
	        if (www.error != null)  
	            Common.ERROR_MSG(www.error);  
			else if(www.texture != null)
			{
				obj.mainTexture = www.texture;
				obj.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			}
		
			Common.DEBUG_MSG("loadingbar_backgroundpic::getFullTexture: download backgroundpic is finished!");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void FixedUpdate () {  
		if(startFadeOut == true)
		{
			Color c = obj.color;
			if (fadeTime <= 0.0f)
			{
				NGUITools.SetActive(obj.gameObject, false);
				c.a = 1.0f;
				obj.color = c;
				Common.DEBUG_MSG("fadeout_close: over, shader=" + obj.shader);
				startFadeOut = false;
				loader.inst.StartCoroutine(getFullTexture());
			}
			else
			{
				c.a = Mathf.InverseLerp(0.0f, 1.0f, fadeTime / fadeMax);
				if(fadeTime <= 0.8f)
				{
					NGUITools.SetActive(loadingbar.label.gameObject, false);
				}
			}

			obj.color = c;
			//Common.DEBUG_MSG("fadeout: a=" + obj.color.a);
			fadeTime -= Time.deltaTime;
		}
	}  
		
	public void fadeout_close()
	{
		fadeTime = fadeMax;
		startFadeOut = true;
		NGUITools.SetActive(loadingbar.obj.gameObject, false);
	}	
}
