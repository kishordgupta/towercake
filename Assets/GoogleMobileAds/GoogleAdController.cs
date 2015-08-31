using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class GoogleAdController : MonoBehaviour
{

		static GameObject googleAdBannerObject;
		public string googleAdID;
		public string googleInterestialAdId;
		public string amazonAdID;
		public string iosAdID;
		private string selectedAdID;
		

		public enum platforms
		{
				Google,
				Amazon,
				iOS,
		}
		public platforms targetPlatform;

		public enum adLocations
		{
				Top,
				Bottom,
		}
		public adLocations adLocation;
		private GoogleMobileAds.Api.AdPosition adPostion;
		private static BannerView bannerView;
		private static InterstitialAd interstitialView;

		void GameOver ()
		{
				ShowInterstital ();
		}

		void Awake ()
		{
				//check if GoogleAdBannerObject Exits
				if (googleAdBannerObject) {
						//it does so destory this object
						Destroy (gameObject);
				} else {
						//else make set GoogleAdbannerObject to this object and dont destroy on load
						googleAdBannerObject = gameObject;
						DontDestroyOnLoad (gameObject);
				}

				switch (targetPlatform) {
				case platforms.Google:
						selectedAdID = googleAdID;
						break;
				case platforms.Amazon:
						selectedAdID = amazonAdID;
						break;
				case platforms.iOS:
						selectedAdID = iosAdID;
						break;
				}

				switch (adLocation) {
				case adLocations.Top:
						adPostion = AdPosition.Top;
						break;
				case adLocations.Bottom:
						adPostion = AdPosition.Bottom;
						break;
				}

		}


		// Use this for initialization
		void Start ()
		{	
				
				// Create a banner
				bannerView = new BannerView (selectedAdID, AdSize.SmartBanner, adPostion);
				interstitialView = new InterstitialAd (googleInterestialAdId);
				// Create an empty ad request.
				AdRequest request = new AdRequest.Builder ().Build ();
				AdRequest request1 = new AdRequest.Builder ().Build ();
				// Load the banner with the request.
				bannerView.LoadAd (request);
				interstitialView.LoadAd (request1);

		}

		public static void HideInterstital ()
		{
//		interstitialView.Show ();
				interstitialView.Destroy ();
		}

		public static void ShowInterstital ()
		{
				AdRequest request1 = new AdRequest.Builder ().Build ();
				// Load the banner with the request.
				interstitialView.LoadAd (request1);
		
				if (interstitialView.IsLoaded ()) {
						interstitialView.Show ();
				}
		}

		public static void HideBanner ()
		{
				bannerView.Hide ();
		}

		public static void ShowBanner ()
		{
				bannerView.Show ();
		}
}
