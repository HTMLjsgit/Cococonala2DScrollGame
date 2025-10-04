// ファイル名: PrefabParentSettings.cs
// 配置場所: Assets/Editor/

using UnityEngine;
using System.Collections.Generic;
using System;

// [Serializable] 属性により、このクラスのインスタンスをUnityが保存できるようになる
[Serializable]
public class PrefabParentMapEntry
{
    public string parentName = "New Parent";
    public List<GameObject> prefabs = new List<GameObject>();
}

[CreateAssetMenu(fileName = "PrefabParentSettings", menuName = "Editor/Prefab Parent Settings")]
public class PrefabParentSettings : ScriptableObject
{
    // 新しいデータ構造。PrefabParentMapEntry のリストとして設定を保持する
    [SerializeField]
    public List<PrefabParentMapEntry> prefabMappings = new List<PrefabParentMapEntry>();
}