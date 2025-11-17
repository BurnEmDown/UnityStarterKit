For each new game, create a Firebase project, download google-services.json (Android) and GoogleService-Info.plist (iOS) and place them here.

How to actually add Firebase SDK (per project)

1. Create Firebase project in console.

2. Add your Android+iOS apps (bundle IDs).

3. Download google-services.json + GoogleService-Info.plist.

4. Import the Firebase Unity SDK (via .unitypackage or UPM, depending on version).

5. Ensure your Firebase.asmdef references the right Firebase assemblies.

6. Define a scripting symbol like FIREBASE_INSTALLED in Player Settings → Scripting Define Symbols.
