// ファイル名: PrefabParentSettings.cs
// 配置場所: Assets/Editor/

using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class PrefabParentMapEntry
{
    // ★変更: 個別の有効フラグを削除
    public string parentName = "New Parent";
    public List<GameObject> prefabs = new List<GameObject>();
}

[CreateAssetMenu(fileName = "PrefabParentSettings", menuName = "Editor/Prefab Parent Settings")]
public class PrefabParentSettings : ScriptableObject
{
    // 機能全体を有効化/無効化するためのフラグ
    [SerializeField]
    public bool isGloballyEnabled = true;

    [SerializeField]
    public List<PrefabParentMapEntry> prefabMappings = new List<PrefabParentMapEntry>();
}