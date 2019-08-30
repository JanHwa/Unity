using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAd : MonoBehaviour
{
	private BannerView bannerView;

	private InterstitialAd interstitial;

    private RewardedAd rewardedAd;

    // 是否Debug，默认测试环境
    public bool isDebug = true;

	public string appID_iOS = "";
	public string appID_AND = "";

	public string banner_adID_iOS = "";
	public string banner_adID_AND = "";

	public string inter_adID_iOS = "";
	public string inter_adID_AND = "";

	public string reward_adID_iOS = "";
	public string reward_adID_AND = "";

	#region Singleton
	private static GoogleAd _instance;
	public static GoogleAd Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GoogleAd();
			}
			return _instance;
		}
	}

	private void Awake()
	{
		_instance = this;
	}
	#endregion


	public void Start()
	{


		InitializeAD(isDebug);

		RequestBanner();

		RequestInterstitial();

		RequestRewarded();

		
	}

	// 初始化AD，正式环境调用
	private void InitializeAD(bool debug)
	{
		if (!debug)
		{
#if UNITY_ANDROID
            string appId = appID_AND;
#elif UNITY_IPHONE
            string appId = appID_iOS;
#else
			string appId = "unexpected_platform";
#endif

			// Initialize the Google Mobile Ads SDK.
			MobileAds.Initialize(appId);
		}

	}


	private void RequestBanner()
	{
#if UNITY_ANDROID
            string adUnitId = banner_adID_AND;
#elif UNITY_IPHONE
            string adUnitId = banner_adID_iOS;
#else
		string adUnitId = "unexpected_platform";
#endif

		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);


		AdRequest request;

		if (isDebug)
		{
			// FOR TESTING
			request = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
		}
		else
		{
			// FOR REAL APP
			request = new AdRequest.Builder().Build();
		}

		// Load the banner with the request.
		bannerView.LoadAd(request);
        this.HandleBannerEvents(true);
    }

    // 插页式广告

    private void RequestInterstitial()
	{
#if UNITY_ANDROID
            string adUnitId = inter_adID_AND;

#elif UNITY_IPHONE
            string adUnitId = inter_adID_iOS;
#else
		string adUnitId = "unexpected_platform";
#endif

		interstitial = new InterstitialAd(adUnitId);


		AdRequest request;

		if (isDebug)
		{
			// FOR TESTING
			request = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
		}
		else
		{
			// FOR REAL APP
			request = new AdRequest.Builder().Build();
		}

		// Load the banner with the request.
		interstitial.LoadAd(request);

        this.HandleInterstitialEvents(true);
	}

	// 激励广告（视频类)
	private void RequestRewarded()
	{
#if UNITY_ANDROID
            string adUnitId = reward_adID_AND;
#elif UNITY_IPHONE
            string adUnitId = reward_adID_iOS;
#else
		string adUnitId = "unexpected_platform";
#endif

        rewardedAd = new RewardedAd(adUnitId);
		// Create an empty ad request.

		AdRequest request;

		if (isDebug)
		{
			// FOR TESTING
			request = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
		}
		else
		{
			// FOR REAL APP
			request = new AdRequest.Builder().Build();
		}

		// Load the interstitial with the request.
		rewardedAd.LoadAd(request);
        this.HandleRewardedEvents(true);
	}


	public void Display_Banner()
	{
		bannerView.Show();
	}

	public void Display_Interstitial()
	{
		if (interstitial.IsLoaded())
		{
			interstitial.Show();
			Debug.Log("已展示插页广告");
		}
	}

	public void Display_Reward()
	{
		if (rewardedAd.IsLoaded())
		{
			rewardedAd.Show();
			Debug.Log("已展示激励广告");
		}
	}


	// HANDLE EVENTS


	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		this.Display_Banner();
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		this.RequestBanner();

		this.RequestInterstitial();
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdOpened event received");
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
        this.RequestInterstitial();

        MonoBehaviour.print("HandleAdClosed event received");
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLeavingApplication event received");
	}



	

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        this.RequestRewarded();
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        this.RequestRewarded();
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
    }

    // 订阅或取消AD回调事件
    private void HandleRewardedEvents(bool subscribe)
    {
        if (subscribe)
        {

            // Called when an ad request has successfully loaded.
            this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
            // Called when an ad request failed to load.
            this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            // Called when an ad is shown.
            this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
            // Called when an ad request failed to show.
            this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            // Called when the user should be rewarded for interacting with the ad.
            this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            // Called when the ad is closed.
            this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        }
        else
        {

            // Called when an ad request has successfully loaded.
            this.rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
            // Called when an ad request failed to load.
            this.rewardedAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
            // Called when an ad is shown.
            this.rewardedAd.OnAdOpening -= HandleRewardedAdOpening;
            // Called when an ad request failed to show.
            this.rewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
            // Called when the user should be rewarded for interacting with the ad.
            this.rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
            // Called when the ad is closed.
            this.rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
        }
    }

    // 订阅或取消AD回调事件
    private void HandleBannerEvents(bool subscribe)
    {
        if (subscribe)
        {
            // Called when an ad request has successfully loaded.
            bannerView.OnAdLoaded += HandleOnAdLoaded;
            // Called when an ad request failed to load.
            bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            bannerView.OnAdOpening += HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            bannerView.OnAdClosed += HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            bannerView.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        }
        else
        {
            // Called when an ad request has successfully loaded.
            bannerView.OnAdLoaded -= HandleOnAdLoaded;
            // Called when an ad request failed to load.
            bannerView.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            bannerView.OnAdOpening -= HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            bannerView.OnAdClosed -= HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            bannerView.OnAdLeavingApplication -= HandleOnAdLeavingApplication;

        }
    }

    // 订阅或取消AD回调事件
    private void HandleInterstitialEvents(bool subscribe)
    {
        if (subscribe)
        {

            // Called when an ad request has successfully loaded.
            this.interstitial.OnAdLoaded += HandleOnAdLoaded;
            // Called when an ad request failed to load.
            this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            // Called when an ad is shown.
            this.interstitial.OnAdOpening += HandleOnAdOpened;
            // Called when the ad is closed.
            this.interstitial.OnAdClosed += HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        }
        else
        {

            // Called when an ad request has successfully loaded.
            this.interstitial.OnAdLoaded -= HandleOnAdLoaded;
            // Called when an ad request failed to load.
            this.interstitial.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
            // Called when an ad is shown.
            this.interstitial.OnAdOpening -= HandleOnAdOpened;
            // Called when the ad is closed.
            this.interstitial.OnAdClosed -= HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            this.interstitial.OnAdLeavingApplication -= HandleOnAdLeavingApplication;
        }
    }

    private void OnEnable()
	{
		//HandleBannerEvents(true);
	}

	private void OnDisable()
	{
		this.HandleBannerEvents(false);
        this.HandleInterstitialEvents(false);
        this.HandleRewardedEvents(false);


        this.bannerView.Destroy();
        this.interstitial.Destroy();
      
    }


    

}




















































