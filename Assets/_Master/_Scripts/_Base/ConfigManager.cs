﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ConfigManager :Singleton<ConfigManager> 
{
// #if UNITY_STANDALONE
//     [SerializeField] private DataConfigRef standAloneConfigRef;
//     
// #else 
//     [SerializeField] private DataConfigRef mobileConfigRef;
// #endif
//
//     private DataConfigRef configRef;
//     public ConfigUnit ConfigUnit => configRef.ConfigUnit;
//     public ConfigWaves ConfigWaves => configRef.ConfigWaves;
//     
//     // Start is called before the first frame update
    public void InitConfig(Action callback)
    {
        callback?.Invoke();
    }
}
