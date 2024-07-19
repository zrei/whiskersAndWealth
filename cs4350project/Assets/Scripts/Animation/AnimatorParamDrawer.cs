using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AnimatorParam))]
public class AnimatorParamDrawer : PropertyDrawer
{
    private const float m_Indent = 20f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {   
        EditorGUI.BeginProperty(position, label, property);

        Rect folderPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(folderPosition, property.isExpanded, label); 

        if (!property.isExpanded)
            return;

        float xPosition = position.x + m_Indent;
        float width = position.width - m_Indent;

        Rect stringParamPosition = new Rect(xPosition, position.y + EditorGUIUtility.singleLineHeight, width, EditorGUIUtility.singleLineHeight);
        SerializedProperty stringParamProperty = property.FindPropertyRelative("m_Param");
        EditorGUI.PropertyField(stringParamPosition, stringParamProperty);

        Rect paramTypePosition = new Rect(xPosition, position.y + EditorGUIUtility.singleLineHeight * 2, width, EditorGUIUtility.singleLineHeight);
        SerializedProperty paramTypeProperty = property.FindPropertyRelative("m_ParamType");
        EditorGUI.PropertyField(paramTypePosition, paramTypeProperty);

        AnimatorParamType paramType = (AnimatorParamType) paramTypeProperty.enumValueIndex;

        Rect codeSetPosition = new Rect(xPosition, position.y + EditorGUIUtility.singleLineHeight * 3, width, EditorGUIUtility.singleLineHeight);
        SerializedProperty codeSetProperty = property.FindPropertyRelative("m_SetUsingCode");
        EditorGUI.PropertyField(codeSetPosition, codeSetProperty);

        bool codeSet = codeSetProperty.boolValue;

        if (!codeSet && paramType == AnimatorParamType.FLOAT)
        {
            Rect floatDefaultPosition = new Rect(xPosition, position.y + EditorGUIUtility.singleLineHeight * 4, width, EditorGUIUtility.singleLineHeight);
            SerializedProperty floatDefaultProperty = property.FindPropertyRelative("m_FloatDefaultValue");
            EditorGUI.PropertyField(floatDefaultPosition, floatDefaultProperty);
        } else if (!codeSet && paramType == AnimatorParamType.INT)
        {
            Rect intDefaultPosition = new Rect(xPosition, position.y + EditorGUIUtility.singleLineHeight * 4, width, EditorGUIUtility.singleLineHeight);
            SerializedProperty intDefaultProperty = property.FindPropertyRelative("m_IntDefaultValue");
            EditorGUI.PropertyField(intDefaultPosition, intDefaultProperty);
        } else if (!codeSet && paramType == AnimatorParamType.BOOL)
        {
            Rect boolDefaultPosition = new Rect(xPosition, position.y + EditorGUIUtility.singleLineHeight * 4, width, EditorGUIUtility.singleLineHeight);
            SerializedProperty boolDefaultProperty = property.FindPropertyRelative("m_BoolDefaultValue");
            EditorGUI.PropertyField(boolDefaultPosition, boolDefaultProperty);
        } else if (!codeSet && paramType == AnimatorParamType.TRIGGER)
        {
            Rect triggerDefaultPosition = new Rect(xPosition, position.y + EditorGUIUtility.singleLineHeight * 4, width, EditorGUIUtility.singleLineHeight);
            SerializedProperty triggerDefaultProperty = property.FindPropertyRelative("m_TriggerDefaultValue");
            EditorGUI.PropertyField(triggerDefaultPosition, triggerDefaultProperty);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;

        float height = EditorGUIUtility.singleLineHeight * 4;

        SerializedProperty codeSetProperty = property.FindPropertyRelative("m_SetUsingCode");
        bool codeSet = codeSetProperty.boolValue;

        if (!codeSet)
            height += EditorGUIUtility.singleLineHeight;

        return height;
    }
}