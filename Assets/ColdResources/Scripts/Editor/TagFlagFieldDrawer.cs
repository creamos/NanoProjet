using Cinemachine.Editor;
using FMODUnity;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TagFlagField))]
public class TagFlagFieldDrawer : PropertyDrawer
{
    public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
    {
        var tags = property.FindPropertyRelative("_tags");

        if (tags.isArray == false) {
            Debug.Log("Warning, tags is not an array");
            return;
        }
        int count = tags.arraySize;
        string[] allTags = UnityEditorInternal.InternalEditorUtility.tags;

        //GENERATE CURRENT FLAG

        int currentFlag = 0;

        if (count == allTags.Count()) {
            currentFlag = -1;
        } else if (count > 0) {
            for (int i = 0; i < allTags.Count(); i++) {
                if (tags.ArrayContains(elem => elem.stringValue == allTags[i])) {
                    currentFlag += (int)Mathf.Pow(2, i);
                }
            }
        }

        //RETRIEVE USER SELECTION

        int newFlag = EditorGUI.MaskField(position, label, currentFlag, allTags);

        //UPDATE LIST IN CASE OF CHANGES

        if (newFlag != currentFlag) {

            tags.ArrayClear();
            if (newFlag == -1) {
                // Fill array with all default tags
                Array.ForEach(allTags, (tag) => {
                    tags.InsertArrayElementAtIndex(tags.arraySize);
                    tags.GetArrayElementAtIndex(tags.arraySize - 1).stringValue = tag;
                });
            } else if (newFlag > 0) {
                // Fill array with corresponding tags
                for (int tagID = allTags.Count() - 1; tagID >= 0 && newFlag > 0; tagID--) {
                    int flagBit = (int) Mathf.Pow(2, tagID);
                    if (flagBit <= newFlag) {
                        newFlag -= flagBit;
                        var tag = allTags[tagID];

                        tags.InsertArrayElementAtIndex(tags.arraySize);
                        tags.GetArrayElementAtIndex(tags.arraySize - 1).stringValue = tag;
                    }
                }
            }
        }


    }
}
