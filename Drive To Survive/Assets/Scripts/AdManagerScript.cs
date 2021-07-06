using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// Singleton class to handle Adverts
/// </summary>
public class AdManagerScript : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private bool testAds = true;
    private string gameId = "4192139";
    public static AdManagerScript Instance;
    private MainMenuScript mainMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId,testAds);
        }
    }

    public void ShowAd(MainMenuScript mainMenu)
    {
        this.mainMenu = mainMenu;
        Advertisement.Show("video");
    }

    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("Unity Ads Ready");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.Log($"Unity Ads Error: {message}");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Unity Ad Started");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Finished:
                mainMenu.RechargeEnergy();
                break;
            case ShowResult.Failed:
                Debug.LogWarning("Ad Failed");
                break;
            case ShowResult.Skipped:
                mainMenu.RechargeEnergy();
                break;

        }
    }
}
