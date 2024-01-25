using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AlienTierType))]
public class AlienTierTypeDrawer : PropertyDrawer
{
     Color32[] tierTypeColours = new Color32[]
    {
        new Color32(220,220,240,255),   //Tier 1
        new Color32(51,153,255,255),    //Tier 2
        new Color32(0, 255, 153, 255),  //Tier 3
        new Color32(255,204,0,255)      //Tier 4
    };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //saving the current GUI Colour before we make changes
        var cashe = GUI.color;

        //Choose the colour based on the tier type (property.intValue)
        //Clamp here as an edgecase if we have more rarities than colours
        GUI.color = tierTypeColours[Mathf.Clamp(property.intValue,0,tierTypeColours.Length - 1)];

        //Now draw the Enum popup now
        //Since we override the GUI here, if we don't include this it's just not drawn
        property.intValue = (int)(AlienTierType)EditorGUI.EnumPopup(position, label, (AlienTierType)property.intValue);

        //Set the inspector colour back to what it was before
        //We need to apply the GUI colour before we changed it or else everything after will get affected too
        GUI.color = cashe;
    }
}
