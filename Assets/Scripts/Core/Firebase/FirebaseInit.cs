using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

namespace Core.Firebase
{
    public class FirebaseInit : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            });
        }

    }
}
