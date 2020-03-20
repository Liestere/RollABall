using UnityEngine.Advertisements;
using UnityEngine;

public class AdManager : MonoBehaviour
{

    private string android_store_id = "3516570";
    private string ios_store_id = "3516571";
    private string video_ad = "video";
    private bool testMode = true;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        testMode = true;
#endif
#if UNITY_IOS
        Advertisement.Initialize(ios_store_id, testMode);
#endif
#if UNITY_ANDROID
        Advertisement.Initialize(android_store_id, testMode);
#endif
    }

    public bool ShowAd(string type)
    {
        if (Advertisement.IsReady(video_ad))
        {
            Advertisement.Show(video_ad);
            return true;
        }
        else
        {
            return false;
        }
    }
}
