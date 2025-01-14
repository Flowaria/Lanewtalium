﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class LimAutosaver : MonoBehaviour
{
    private Coroutine AutosaveCoroutineRef;
    private static string LastAutosave = "";
    public void Start()
    {
        if (LimSystem.ChartContainer == null) return;
        AutosaveCoroutineRef = StartCoroutine(AutosaveCoroutine());
    }
    public static string CurrentTimeString()
    {
        return string.Format("{0}-{1}-{2} {3}-{4}-{5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
    }
    IEnumerator AutosaveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            yield return new WaitForEndOfFrame();
            if (LimSystem.Preferences.Autosave)
            {
                Task.Run(() => 
                {
                    string ChartPath = LimSystem.ChartContainer.ChartProperty.ChartFolder + string.Format("/AutoSave {0}.txt", CurrentTimeString());
                    File.WriteAllText(ChartPath, LimSystem.ChartContainer.ChartData.ToString());
                    if (LastAutosave != "") File.Delete(LastAutosave);
                    LastAutosave = ChartPath;
                });
            }
        }
    }
    public void StopAutosave()
    {
        if (AutosaveCoroutineRef != null) StopCoroutine(AutosaveCoroutineRef);
    }
    public static void Autosave()
    {
        if (LimSystem.ChartContainer == null) return;
        if (LimSystem.Preferences.Autosave)
        {
            string ChartPath = LimSystem.ChartContainer.ChartProperty.ChartFolder + string.Format("/AutoSave {0}.txt", CurrentTimeString());
            File.WriteAllText(ChartPath, LimSystem.ChartContainer.ChartData.ToString());
            if (File.Exists(LastAutosave)) File.Delete(LastAutosave);
            LastAutosave = ChartPath;
        }
    }
    private void OnApplicationQuit()
    {
        if (LimSystem.ChartContainer == null) return;
        if (!LimSystem.Preferences.Autosave) return;
        if (LimSystem.ChartContainer.ChartData == null) return;
        string ChartPath = LimSystem.ChartContainer.ChartProperty.ChartFolder + string.Format("/AutoSave {0}.txt", CurrentTimeString());
        if (!string.IsNullOrEmpty(LastAutosave)) if (File.Exists(LastAutosave)) File.Delete(LastAutosave);
        LastAutosave = ChartPath;
        File.WriteAllText(ChartPath, LimSystem.ChartContainer.ChartData.ToString());
    }
}
