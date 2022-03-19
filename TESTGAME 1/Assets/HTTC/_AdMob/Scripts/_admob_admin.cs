using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if ADMOB
using GoogleMobileAds.Api;



[System.Serializable]
public enum _admob_size{Standard_320x50,SmartBanner,IAB_Medium_Rectangle_300x250,IAB_Full_Size_728x90,IAB_Leaderboard_728x90};

[System.Serializable]
public enum _admob_position{Top_Center,Top_Left,Top_Right,Bottom_center,Bottom_Left,Bottom_Right};

[System.Serializable]
public enum _admob_active{Open_Game,Start_Game,Game_Over};

[System.Serializable]
public enum _admob_hidden{No_Hidden,Start_Game,Game_Over};

[System.Serializable]
public class _admob_banner{
	public string _ID;
	public _admob_size _banner_size;
	public _admob_position _banner_position;
	public _admob_active _display_banner_on;
	public _admob_hidden _hidden_banner_on;
}

[System.Serializable]
public class _admob_interstitial{
	public string _ID;
	public _admob_active _display_banner_on;
}

public class _admob_admin : MonoBehaviour {
	public static _admob_admin Instance;

	public _admob_banner[] _banner;
	public _admob_interstitial[] _interstitial;

	private List<BannerView> bannerView = new List<BannerView>();
	private List<InterstitialAd> interstitial = new List<InterstitialAd>();


	void Awake(){
		DontDestroyOnLoad (this);

		if(_admob_admin.Instance==null){
				Instance = this;
		}

		// Check banners
		//---------------------------------------
		if (bannerView.Count < _banner.Length) {
			_create_banners ();
		}

		if (interstitial.Count < _interstitial.Length) {
			_create_interstitial ();
		}
	}

	//---------------------------------------
	//---------------------------------------

	void _create_banners(){

		for (int i=0; i<_banner.Length; i++) {
			// CREATE NEW BANNER
			//---------------------------------------
			Debug.Log(_banner[i]._ID);
			BannerView BV = new BannerView(_banner[i]._ID, _adsize(_banner[i]._banner_size), _adpos(_banner[i]._banner_position));
			// Create an empty ad request.
			AdRequest request = new AdRequest.Builder().Build();
			// Load the banner with the request.
			BV.LoadAd(request);
			bannerView.Add(BV);
		}

	}
	
	//---------------------------------------
	//---------------------------------------
	
	void _create_interstitial(){

		for (int i=0; i<_interstitial.Length; i++) {
			// CREATE NEW INTERSTITIAL
			//---------------------------------------
			InterstitialAd BI = new InterstitialAd (_interstitial[i]._ID);
			// Create an empty ad request.
			AdRequest request = new AdRequest.Builder ().Build ();
			// Load the banner with the request.
			BI.LoadAd (request);
			interstitial.Add(BI);
		}
	}

	//---------------------------------------
	//---------------------------------------

	AdPosition _adpos(_admob_position _pos){
		AdPosition _r = AdPosition.Bottom;
		
		switch (_pos) {
			
		case _admob_position.Bottom_center:
			_r = AdPosition.Bottom;
			break;
			//---------------------------------------
		case _admob_position.Bottom_Left:
			_r = AdPosition.BottomLeft;
			break;
			//---------------------------------------
		case _admob_position.Bottom_Right:
			_r = AdPosition.BottomRight;
			break;
			//---------------------------------------
		case _admob_position.Top_Center:
			_r = AdPosition.Top;
			break;
			//---------------------------------------
		case _admob_position.Top_Left:
			_r = AdPosition.TopLeft;
			break;
			//---------------------------------------
		case _admob_position.Top_Right:
			_r = AdPosition.TopRight;
			break;
		}
		
		return _r;
	}

	//---------------------------------------
	//---------------------------------------

	AdSize _adsize(_admob_size _siz){
		AdSize _r = AdSize.SmartBanner;

		switch (_siz) {

		case _admob_size.Standard_320x50:
			_r = AdSize.Banner;
			break;
		//---------------------------------------
		case _admob_size.SmartBanner:
			_r = AdSize.SmartBanner;
			break;
		//---------------------------------------
		case _admob_size.IAB_Leaderboard_728x90:
			_r = AdSize.Leaderboard;
			break;
		//---------------------------------------
		case _admob_size.IAB_Medium_Rectangle_300x250:
			_r = AdSize.MediumRectangle;
			break;
			//---------------------------------------
		}

		return _r;
	}

	//INTERSTITIAL
	//---------------------------------------
	//---------------------------------------
	
	public void show_interstitial (_admob_active _C) {
		for(int i=0;i<_interstitial.Length;i++){
			if(_interstitial[i]._display_banner_on == _C){
				interstitial[i].Show();
				break;
			}
		}
	}

	//---------------------------------------
	//---------------------------------------

	//BANNER
	//---------------------------------------
	//---------------------------------------

	public void show_banner (_admob_active _C) {
		for(int i=0;i<_banner.Length;i++){
				if(_banner[i]._display_banner_on == _C){
					bannerView[i].Show();
					break;
				}
		}
	}

	//---------------------------------------
	//---------------------------------------

	public void hidden_banner (_admob_hidden _C) {
		for(int i=0;i<_banner.Length;i++){
				if(_banner[i]._hidden_banner_on == _C){
					bannerView[i].Hide();
					break;
				}
		}
	}
	//---------------------------------------
	//---------------------------------------

	public void _check_banner_action(_admob_active _show, _admob_hidden _hidden = _admob_hidden.No_Hidden){
		show_banner (_show);
		show_interstitial (_show);

		if (_hidden != null) {
			hidden_banner (_hidden);
		}
	}

}
#endif