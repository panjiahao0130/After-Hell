using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SectorRangeManager : SingletonMono<SectorRangeManager>
{
    [SerializeField]
    private List<SectorRange> sectorObjects = new List<SectorRange>();

    private Dictionary<string, SectorRange> _sectorDictionary;
    public override void Awake()
    {
        base.Awake();
        InitializeSectorDictionary();
    }
    

    private void InitializeSectorDictionary()
    {
        Debug.Log("初始化扇形范围字典");
        _sectorDictionary=new Dictionary<string, SectorRange>()
        {
            { "attackRange", sectorObjects[0] },
            { "reboundRange", sectorObjects[1] }
        };
    }

    public SectorRange GetSectorRange(string sectorName)
    {
        if (_sectorDictionary.TryGetValue(sectorName,out SectorRange value))
        {
            return value;
        }
        Debug.LogError(sectorName+"这个扇形范围未配置");
        return null;
    }
    
    // 显示指定的 SectorRange
    public void ShowSectorRange(SectorRange sectorRange)
    {
        if (sectorObjects.Contains(sectorRange))
        {
            sectorRange.ShowSector();
        }
    }

    // 隐藏指定的 SectorRange
    public void HideSectorRange(SectorRange sectorRange)
    {
        if (sectorObjects.Contains(sectorRange))
        {
            sectorRange.HideSector();
        }
    }
    
}

