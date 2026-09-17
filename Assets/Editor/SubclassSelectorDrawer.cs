#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SubclassSelectorAttribute))]
public class SubclassSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.ManagedReference)
        {
            EditorGUI.PropertyField(position, property, label, true);
            return;
        }

        Type baseType = fieldInfo.FieldType;
        if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>))
        {
            baseType = baseType.GetGenericArguments()[0];
        }
        else if (baseType.IsArray)
        {
            baseType = baseType.GetElementType();
        }

        float singleLine = EditorGUIUtility.singleLineHeight;
        float spacing = EditorGUIUtility.standardVerticalSpacing;

        // 1. Przycisk wyboru klasy na górze
        Rect buttonRect = new Rect(position.x, position.y, position.width, singleLine);
        string currentTypeName = property.managedReferenceValue?.GetType().Name ?? "Brak (null)";

        if (GUI.Button(buttonRect, $"{label.text} : [{currentTypeName}]", EditorStyles.miniPullDown))
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Brak (null)"), property.managedReferenceValue == null, () =>
            {
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            });

            var derivedTypes = TypeCache.GetTypesDerivedFrom(baseType)
                .Where(t => !t.IsAbstract && !t.IsInterface);

            foreach (var type in derivedTypes)
            {
                menu.AddItem(new GUIContent(type.Name), property.managedReferenceValue?.GetType() == type, () =>
                {
                    property.managedReferenceValue = Activator.CreateInstance(type);
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            menu.ShowAsContext();
        }

        // 2. Rysowanie pól wnętrza wybranej klasy
        if (property.managedReferenceValue != null)
        {
            EditorGUI.indentLevel++;
            float currentY = position.y + singleLine + spacing;

            SerializedProperty child = property.Copy();
            SerializedProperty end = property.GetEndProperty();

            // Wchodzimy do wnętrza struktury obiektu
            bool enterChildren = true;
            while (child.NextVisible(enterChildren) && !SerializedProperty.EqualContents(child, end))
            {
                enterChildren = false;
                float childHeight = EditorGUI.GetPropertyHeight(child, true);
                Rect childRect = new Rect(position.x, currentY, position.width, childHeight);

                EditorGUI.PropertyField(childRect, child, true);

                currentY += childHeight + spacing;
            }

            EditorGUI.indentLevel--;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float totalHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        if (property.propertyType == SerializedPropertyType.ManagedReference && property.managedReferenceValue != null)
        {
            SerializedProperty child = property.Copy();
            SerializedProperty end = property.GetEndProperty();

            bool enterChildren = true;
            while (child.NextVisible(enterChildren) && !SerializedProperty.EqualContents(child, end))
            {
                enterChildren = false;
                totalHeight += EditorGUI.GetPropertyHeight(child, true) + EditorGUIUtility.standardVerticalSpacing;
            }
        }

        return totalHeight;
    }
}
#endif