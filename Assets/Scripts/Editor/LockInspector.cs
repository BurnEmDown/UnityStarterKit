using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class LockInspector
    {
        [MenuItem("Edit/Lock Inspector %l")]
        public static void Lock()
        {
            ActiveEditorTracker tracker = ActiveEditorTracker.sharedTracker;
            tracker.isLocked = !tracker.isLocked;

            // lock scale constrain for transform in locked inspector
            foreach (var activeEditor in tracker.activeEditors)
            {
                if (activeEditor.target is not Transform) continue;

                var transform = (Transform)activeEditor.target;

                var propInfo = transform.GetType().GetProperty("constrainProportionsScale",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                if (propInfo == null) continue;

                var value = (bool)propInfo.GetValue(transform, null);
                propInfo.SetValue(transform, !value, null);
            }
            
            tracker.ForceRebuild();
        }

        [MenuItem("Edit/Lock Inspector %l", true)]
        public static bool Valid()
        {
            return ActiveEditorTracker.sharedTracker.activeEditors.Length != 0;
        }
    }
}