using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MoveFloor))]
[CanEditMultipleObjects]
public class MoveFloorEditor : Editor
{
    //MoveFloorのinspectorを見えやすくするためのスクリプト
    SerializedProperty movementTypeProp;
    SerializedProperty speedProp;
    SerializedProperty directionXProp;
    SerializedProperty directionYProp;
    SerializedProperty centerPointProp;
    SerializedProperty radiusProp;
    SerializedProperty orbitalSpeedProp;
    SerializedProperty pointAProp;
    SerializedProperty pointBProp;

    private void OnEnable()
    {
        movementTypeProp = serializedObject.FindProperty("movementType");
        speedProp = serializedObject.FindProperty("speed");
        directionXProp = serializedObject.FindProperty("directionX");
        directionYProp = serializedObject.FindProperty("directionY");
        centerPointProp = serializedObject.FindProperty("centerPoint");
        radiusProp = serializedObject.FindProperty("radius");
        orbitalSpeedProp = serializedObject.FindProperty("orbitalSpeed");
        pointAProp = serializedObject.FindProperty("pointA");
        pointBProp = serializedObject.FindProperty("pointB");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(movementTypeProp);
        EditorGUILayout.Space();

        MoveFloor.MovementType selectedType = (MoveFloor.MovementType)movementTypeProp.enumValueIndex;
        
        switch (selectedType)
        {
            case MoveFloor.MovementType.Horizontal:
                EditorGUILayout.PropertyField(speedProp);
                EditorGUILayout.PropertyField(directionXProp);
                break;

            case MoveFloor.MovementType.Vertical:
                EditorGUILayout.PropertyField(speedProp);
                EditorGUILayout.PropertyField(directionYProp);
                break;
            
            case MoveFloor.MovementType.Circular:
                EditorGUILayout.PropertyField(centerPointProp);
                EditorGUILayout.PropertyField(speedProp, new GUIContent("円運動の速度"));
                EditorGUILayout.PropertyField(radiusProp);
                break;
            
            case MoveFloor.MovementType.OrbitalPatrol:
                EditorGUILayout.LabelField("中心の往復移動", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(pointAProp);
                EditorGUILayout.PropertyField(pointBProp);
                EditorGUILayout.PropertyField(speedProp, new GUIContent("中心の移動速度"));
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("円運動", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(radiusProp);
                EditorGUILayout.PropertyField(orbitalSpeedProp, new GUIContent("公転の速度"));
                break;
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}