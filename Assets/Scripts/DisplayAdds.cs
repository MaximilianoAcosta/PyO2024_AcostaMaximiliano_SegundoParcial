using UnityEngine.Advertisements;
using UnityEngine;
using System;

public class DisplayAdds : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] bool _testMode = true;
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;
    [SerializeField] string _androidAdUnitId = "Banner_Android";
    [SerializeField] string _androidAdUnitId2 = "Interstitial_Android";
    [SerializeField] string _androidAdUnitId3 = "Rewarded_Android";
    private string _gameId;
    private string _adUnitId = null;
    private string _adUnitId2 = null;
    private string _adUnitId3 = null;

    void Awake()
    {
#if UNITY_ANDROID
        InitializeAds();
#endif
    }
    private void Start()
    {
#if UNITY_ANDROID
        Advertisement.Banner.SetPosition(_bannerPosition);
        /*
        LoadAd();
        LoadBanner();
        LoadRewardAd();
        */
#endif


    }
    public void InitializeAds()
    {
#if UNITY_ANDROID
        _gameId = _androidGameId;
        _adUnitId = _androidAdUnitId;
        _adUnitId2 = _androidAdUnitId2;
        _adUnitId3 = _androidAdUnitId3;
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
#endif
    }
    public void OnInitializationComplete()
    {
        LoadAd();
        LoadBanner();
        LoadRewardAd();
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    //-------------------------------banner--------------------------//
    public void LoadBanner()
    {
#if UNITY_ANDROID
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_adUnitId, options);
#endif
    }
    void OnBannerLoaded()
    {
#if UNITY_ANDROID
        Debug.Log("Banner loaded");
        ShowBannerAd();
#endif
    }
    void OnBannerError(string message)
    {

        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }
    void ShowBannerAd()
    {
#if UNITY_ANDROID
        Debug.Log("BannerShow");
        Advertisement.Banner.Show(_adUnitId);
#endif
        // Show the loaded Banner Ad Unit:
    }
    void HideBannerAd()
    {
#if UNITY_ANDROID
        Advertisement.Banner.Hide();
#endif
        // Hide the banner:
    }
    //-------------------------------intersticial--------------------------//

    public void LoadAd()
    {
#if UNITY_ANDROID
        Debug.Log("Loading Ad: " + _adUnitId2);
        Advertisement.Load(_adUnitId2, this);
#endif
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
    }
    public void ShowAd()
    {
#if UNITY_ANDROID
        Debug.Log("Showing Ad: " + _adUnitId2);
        Advertisement.Show(_adUnitId2, this);
        LoadAd();
#endif
        // Note that if the ad content wasn't previously loaded, this method will fail
    }
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
    }

    public void OnUnityAdsFailedToLoad(string _adUnitId, UnityAdsLoadError error, string message)
    {
#if UNITY_ANDROID
        LoadAd();
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
#endif
    }

    public void OnUnityAdsShowFailure(string _adUnitId, UnityAdsShowError error, string message)
    {
#if UNITY_ANDROID
        LoadAd();
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
#endif
    }

    public void OnUnityAdsShowStart(string _adUnitId) { }
    public void OnUnityAdsShowClick(string _adUnitId) { }
    public void OnUnityAdsSdhowComplete(string _adUnitId, UnityAdsShowCompletionState showCompletionState) { LoadAd(); }

    //-------------------------------Rewarded--------------------------//
    public event Action OnRewardGiven;
    public void LoadRewardAd()
    {
#if UNITY_ANDROID
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId3);
        Advertisement.Load(_adUnitId3, this);
#endif
    }
    public void ShowRewardAd()
    {
#if UNITY_ANDROID
        Advertisement.Show(_adUnitId3, this);
#endif
    }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
#if UNITY_ANDROID
        if (adUnitId.Equals(_adUnitId3) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            OnRewardGiven?.Invoke();
            LoadRewardAd();
        }
#endif
    }
    public void OnUnityRewardAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
#if UNITY_ANDROID
        LoadRewardAd();
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
#endif
    }
    public void OnUnityRewardAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
#if UNITY_ANDROID
        LoadRewardAd();
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Use the error details to determine whether to try to load another ad.
#endif
    }
}

