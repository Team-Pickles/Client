using System;
using UnityEngine;

[Serializable]
public class MapDataClass {
    [SerializeField] public int map_id;
    [SerializeField] public string map_info;
    [SerializeField] public string map_tag;
    [SerializeField] public int map_grade;
    [SerializeField] public int map_difficulty;
    [SerializeField] public string map_maker;
}