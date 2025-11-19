using System.Collections.Generic;
using Core.Interfaces;
using Core.Services;
using Core.StubServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Core.Services.CoreServices;

namespace Core
{
    public class Loader : MonoBehaviour
    {
        // Add non-addressable prefabs here via Inspector if they need to be created via factory
        public Dictionary<string, GameObject> PrefabMap;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Register<IEventsManager>(new EventsManager());
            Register<IEventListenerManager>(new EventListenerManager());
            //Register<ISceneLoader>(new SceneLoader());
            Register<IObjectFactory>(new CompositeFactory(new AddressablesFactory(), new PrefabFactory(PrefabMap)));
            Register<IPoolManager>(new PoolManager());
            Register<ISaveSystem>(new JsonFileSaveSystem());
            Register<IAudioManager>(new StubAudioManager());
            Register<ILocalizationService>(new StubLocalizationService());

            
            // Register actual analytics, remote config and crash reporting services instead of stubs
            Register<IAnalyticsService>(new StubAnalyticsService());
            Register<IRemoteConfigService>(new StubRemoteConfigService());
            Register<ICrashReportingService>(new StubCrashReportingService());
            
            // kick off Firebase initialization if module is present
#if FIREBASE_INSTALLED
            await FirebaseInitializer.InitializeAsync();
#endif
             
            // load first scene / show main menu
        }
        
        private void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadMode)
        {
            if (loadedScene.buildIndex != 0)
            {
                Destroy(gameObject);
            }
        }
        
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}