#if FIREBASE_INSTALLED
using Firebase;
#endif
using System.Threading.Tasks;
using UnityEngine;

namespace Integrations.Firebase
{
    public static class FirebaseInitializer
    {
        public static async Task InitializeAndOverrideServicesAsync()
        {
#if FIREBASE_INSTALLED
            Debug.Log("[Firebase] Checking and fixing dependencies...");

            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("[Firebase] Dependencies available, initializing services...");

                // Register Firebase-backed implementations
                Services.Register<IAnalyticsService>(new FirebaseAnalyticsService());
                var remoteConfig = new FirebaseRemoteConfigService();
                Services.Register<IRemoteConfigService>(remoteConfig);
                Services.Register<ICrashReportingService>(new FirebaseCrashReportingService());

                // Initialize remote config as part of startup
                await remoteConfig.InitializeAsync();

                Debug.Log("[Firebase] All Firebase services initialized and registered.");
            }
            else
            {
                Debug.LogError($"[Firebase] Could not resolve dependencies: {dependencyStatus}. Using stub services.");
            }
#else
            Debug.Log("[FirebaseInitializer] FIREBASE_INSTALLED not defined. Using stub services.");
            await Task.CompletedTask;
#endif
        }
    }
}