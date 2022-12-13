using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(AudioManagerSO))]
public class AudioManagerEditor : Editor
{
    bool _eventsFoldout, _audioFoldout;
    public override void OnInspectorGUI ()
    {
        SerializedProperty prop_event_gameLoaded    = Prop("GameLoaded");
        SerializedProperty prop_event_runStarted    = Prop("RunStarted");
        SerializedProperty prop_event_runFinished   = Prop("RunFinished");
        SerializedProperty prop_event_playerGraze   = Prop("PlayersGraze");
        SerializedProperty prop_event_collision     = Prop("PlayerBoost");
        SerializedProperty prop_event_boost         = Prop("PlayerObstacleCollision");

        SerializedProperty prop_audio_gameLoaded    = Prop("AudioEvent_GameLoaded");
        SerializedProperty prop_audio_runStarted    = Prop("AudioEvent_RunStarted");
        SerializedProperty prop_audio_runFinished   = Prop("AudioEvent_RunFinished");
        SerializedProperty prop_audio_playerGraze   = Prop("AudioEvent_PlayersGraze");
        SerializedProperty prop_audio_collision     = Prop("AudioEvent_PlayerBoost");
        SerializedProperty prop_audio_boost         = Prop("AudioEvent_PlayerObstacleCollision");


        var borderSize = 2;
        var style_foldoutHeader = new GUIStyle(EditorStyles.foldoutHeader) {
            border = new RectOffset(borderSize, borderSize, borderSize, borderSize),
            alignment = TextAnchor.MiddleCenter,
            stretchWidth = true,
            fixedWidth = (EditorGUIUtility.currentViewWidth - 20),
            fontSize = 16,
        };
        var style_centeredHeader = new GUIStyle(EditorStyles.boldLabel) {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 16,
        };

        var style_foldEvent = GUILayout.MaxWidth((EditorGUIUtility.currentViewWidth - 115) / 2f);
        var style_foldAudio = GUILayout.MaxWidth((EditorGUIUtility.currentViewWidth - 115) / 2f);

        if (_eventsFoldout = EditorGUILayout.Foldout(_eventsFoldout, "Events", true, style_foldoutHeader)) {
            using (new EditorGUILayout.HorizontalScope()) {

                using (new EditorGUILayout.VerticalScope("GroupBox", style_foldEvent)) {
                    using (new EditorGUILayout.HorizontalScope("HelpBox")) {
                        EditorGUILayout.LabelField("Game States Related", style_centeredHeader, GUILayout.ExpandWidth(true), style_foldEvent);
                    }

                    EditorGUILayout.PropertyField(prop_event_gameLoaded     , style_foldEvent);
                    EditorGUILayout.PropertyField(prop_event_runStarted     , style_foldEvent);
                    EditorGUILayout.PropertyField(prop_event_runFinished    , style_foldEvent);
                }

                using (new EditorGUILayout.VerticalScope("GroupBox", style_foldEvent)) {
                    using (new EditorGUILayout.HorizontalScope("HelpBox")) {
                        EditorGUILayout.LabelField("Player Actions Related", style_centeredHeader, GUILayout.ExpandWidth(true), style_foldEvent);
                    }

                    EditorGUILayout.PropertyField(prop_event_playerGraze    , style_foldEvent);
                    EditorGUILayout.PropertyField(prop_event_collision      , style_foldEvent);
                    EditorGUILayout.PropertyField(prop_event_boost          , style_foldEvent);
                }
            }
        }
        var labelWidth = EditorGUIUtility.labelWidth;
        if (_audioFoldout = EditorGUILayout.Foldout(_audioFoldout, "FMOD Events", true, style_foldoutHeader)) {
            using (new EditorGUILayout.HorizontalScope()) {

                using (new EditorGUILayout.VerticalScope("GroupBox", style_foldAudio)) {
                    using (new EditorGUILayout.HorizontalScope("HelpBox")) {
                        EditorGUILayout.LabelField("Game States Related", style_centeredHeader, GUILayout.ExpandWidth(true), style_foldAudio);
                    }

                    EditorGUIUtility.labelWidth = 100;
                    EditorGUILayout.PropertyField(prop_audio_gameLoaded, new GUIContent("Game Loaded"), style_foldAudio);
                    EditorGUILayout.PropertyField(prop_audio_runStarted, new GUIContent("Run Started"), style_foldAudio);
                    EditorGUILayout.PropertyField(prop_audio_runFinished, new GUIContent("Run Finished"), style_foldAudio);
                    EditorGUIUtility.labelWidth = labelWidth;
                }

                using (new EditorGUILayout.VerticalScope("GroupBox", style_foldAudio)) {
                    using (new EditorGUILayout.HorizontalScope("HelpBox")) {
                        EditorGUILayout.LabelField("Player Actions Related", style_centeredHeader, GUILayout.ExpandWidth(true), style_foldAudio);
                    }

                    EditorGUIUtility.labelWidth = 100;
                    EditorGUILayout.PropertyField(prop_audio_playerGraze, new GUIContent("Players Graze"), style_foldAudio);
                    EditorGUILayout.PropertyField(prop_audio_collision, new GUIContent("Obstacle Collided"), style_foldAudio);
                    EditorGUILayout.PropertyField(prop_audio_boost, new GUIContent("Boost Used"), style_foldAudio);
                    EditorGUIUtility.labelWidth = labelWidth;
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    SerializedProperty Prop (string name) => serializedObject.FindProperty(name);
}
