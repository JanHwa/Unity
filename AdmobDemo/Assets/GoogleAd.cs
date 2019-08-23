using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class GoogleAd : MonoBehaviour
{

    public string adUnitId = "";

    // Start is called before the first frame update
    void Start()
    {

        BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest.Builder().Build();
        bannerView.LoadAd(request);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnButtonClick()
    {

    }
}
