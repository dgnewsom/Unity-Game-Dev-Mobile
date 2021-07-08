using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private bool testMode = true;
    public static AdManager Instance;

    public const string continueAdString = "ContinueGameAd";
    public const string restartAdString = "RestartGameAd";
    public const string mainMenuAdString = "MainMenuAd";

    private UIScript uiScript;

    void Awake()
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
            Advertisement.Initialize("4208609",testMode);
        }
    }

    public void ShowAd(UIScript uiScript, string placementId)
    {
        this.uiScript = uiScript;
        Advertisement.Show(placementId);
    }

    public void OnUnityAdsReady(string placementId)
    {
        print("Unity Ads Ready");
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError($"Unity Ads Error! : {message}");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        print("Unity Ad Started");
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (placementId)
        {
            case continueAdString:
                switch (showResult)
                {
                    case ShowResult.Failed:
                        Debug.LogWarning("Ad Failed");
                        break;
                    case ShowResult.Skipped:
                        Debug.LogWarning("Shouldn't be able to skip?");
                        break;
                    case ShowResult.Finished:
                        uiScript.ContinueGame();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(showResult), showResult, null);
                }
                break;

            case restartAdString:
                switch (showResult)
                {
                    case ShowResult.Failed:
                        Debug.LogError("Ad Failed");
                        break;
                    case ShowResult.Skipped:
                        uiScript.RestartGame();
                        break;
                    case ShowResult.Finished:
                        uiScript.RestartGame();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(showResult), showResult, null);
                }
                break;

            case mainMenuAdString:
                switch (showResult)
                {
                    case ShowResult.Failed:
                        Debug.LogError("Ad Failed");
                        break;
                    case ShowResult.Skipped:
                        uiScript.LoadMainMenu();
                        break;
                    case ShowResult.Finished:
                        uiScript.LoadMainMenu();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(showResult), showResult, null);
                }
                break;

            default:
                Debug.LogWarning($"{placementId} not recognized!");
                break;
        }
    }
}
