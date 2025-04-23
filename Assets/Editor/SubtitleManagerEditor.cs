using PyrrhicSilva.UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SyncedSubtitleController))]
public class SubtitleManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get reference to the SyncedSubtitleController
        SyncedSubtitleController myScript = (SyncedSubtitleController)target;

        // Add a space for organization
        EditorGUILayout.Space();

        // Add an "Upload" button
        if (GUILayout.Button("Upload Subtitles (.txt)"))
        {
            // Open file dialog to select the .txt file
            string path = EditorUtility.OpenFilePanel("Upload Subtitles", "", "txt");

            // If a valid path is selected, load subtitles
            if (!string.IsNullOrEmpty(path))
            {
                myScript.LoadSubtitlesFromFile(path);

                // Mark the object as dirty to register changes in the editor
                EditorUtility.SetDirty(myScript);
            }
        }
    }
}
