using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(Screenshot))]
    public class ScreenshotEditor : UnityEditor.Editor {

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            if(GUILayout.Button("Take Screenshot")) {
                ((Screenshot)serializedObject.targetObject).TakeScreenshot();
            }
        }
    }
}