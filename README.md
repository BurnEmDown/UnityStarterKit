# UnityStarterKit  
A lightweight, modular, production-ready foundation for starting new Unity projects.

UnityStarterKit is a reusable project skeleton that provides clean architecture,
essential systems, service-based design, and ready-to-extend components that every
mobile or desktop game needs — so you can focus on gameplay instead of boilerplate.

---

## ✨ Features

### 🧩 **Modular Service Architecture**
A central `Services` registry provides clean dependency-injection-like access to core systems:
- `IAnalyticsService`
- `ICrashReportingService`
- `IRemoteConfigService`
- `IAudioManager`
- `ILocalizationService`
- `ISaveSystem`
- `ITimeService`
- `ISceneLoader`
- `IPoolManager`
- `IEventManager`, `IEventListenerManager`
- `IFactoryManager`

Each service includes a **stub implementation** for editor/testing and can be swapped
with a real implementation (e.g., Firebase, Addressables, custom audio, etc.)

---

## ⚙️ Core Systems

### 🔧 **Object Pooling**
Reusable `PoolManager` using async object generation with a custom main-thread task
dispatcher (Firebase-free), allowing performant, GC-friendly spawning.

### 🔥 **Events System**
Lightweight event bus with both listener registrations and runtime event dispatching.

### 📁 **Save System (JSON + Newtonsoft)**
Simple and extensible JSON save system:
- Generic `Save<T>()` / `Load<T>()`
- Versioned container
- Backed by `Application.persistentDataPath`
- Swap with encrypted or cloud saves anytime

### 🎵 **Audio Stubs**
Stubbed `IAudioManager` (logs instead of playing audio),
easy to replace with mixer-based or Addressables audio implementation.

### 🌍 **Localization Stubs**
Stubbed `ILocalizationService` that returns keys directly.
Great for UI development before hooking into real localization files.

### ⏱️ **Time Service**
Centralized control over:
- `DeltaTime`
- `UnscaledDeltaTime`
- Pause/Resume
- Custom time scaling

### 🌐 **Firebase Integration (Optional)**
Under `Integrations/Firebase/`:
- Analytics
- Crashlytics
- Remote Config
- Unified initializer

These automatically override stub services when Firebase is installed
and the `FIREBASE_INSTALLED` define symbol is set.

## 🧱 Architecture Overview

UnityStarterKit is built around four principles:

### 1. **Service-Oriented Structure**
All core systems are accessed via interfaces and registered through a central
`Services` class. This allows:
- Easy swapping of implementations
- Decoupled gameplay code
- Clear separation between core and integrations

### 2. **Minimal Dependencies**
No third-party frameworks required.
Firebase is optional. Async systems do **not** depend on Firebase utilities.

### 3. **Reusable Across Projects**
Everything inside `/Core` is project-agnostic.
Drop it into any new Unity project and immediately have:
- Save system  
- Event bus  
- Pooling  
- Audio & localization stubs  
- Time system  
- Update managers  
- Factory system  

### 4. **Clean Assembly Definition Layout**
Each module has its own `.asmdef`, ensuring:
- Fast compile times
- Strict separation of editor/runtime code
- No accidental UnityEditor usage in builds

---

## 🚀 Getting Started

### 1. Clone the repository
```sh
git clone https://github.com/BurnEmDown/UnityStarterKit.git
```

### 2. Open in Unity Hub
Recommended Unity version: Unity 6 or newer

### 3. Register or override services
In Loader or your bootstrap script:
```
Services.Register<IAudioManager>(new StubAudioManager());
Services.Register<ILocalizationService>(new StubLocalizationService());
Services.Register<ISaveSystem>(new JsonFileSaveSystem());

// Optional Firebase
await FirebaseInitializer.InitializeAndOverrideServicesAsync();
```

### 4. Start building your game
Create your gameplay logic under /Gameplay and use services freely:
```
var audio = Services.Get<IAudioManager>();
audio.PlaySfx("button_click");

var save = Services.Get<ISaveSystem>();
save.Save("player", new PlayerData { level = 3 });
```
