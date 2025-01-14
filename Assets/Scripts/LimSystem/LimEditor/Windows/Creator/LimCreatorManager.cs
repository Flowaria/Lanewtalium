﻿using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimCreatorManager : MonoBehaviour
{
    public RectTransform ViewRect, ContentRect;
    public LimTunerManager TunerManager;
    public LimWindowManager BaseWindow;
    public LimOperationManager OperationManager;
    public LimGizmoMotionManager GizmoMotionManager;
    public Text CreateTapText, CreateHoldText, CreateMotionHorText, CreateMotionVerText,
        CreateMotionRotText, CreateMotionMaunallyText, CreateBpmText, CreateScrollSpeedText,
        CreateCatchRailText, CreateCatchRailQuantityText, AutoHighlightText, DeleteSelectedText, ConvertSelectedToHoldNoteText;
    public Text ClickToCreateText, AngleLineText, CopierText;
    public LimClickToCreateManager ClickToCreateManager;
    public LimAngleLineManager AngleLineManager;
    public LimCopierManager CopierManager;
    public InputField CreateCatchRailQuantityInputField;
    public List<Canvas> SubCanvases = new List<Canvas>();

    public void SetTexts()
    {
        BaseWindow.WindowName = LimLanguageManager.TextDict["Window_Creator_Label"];
        CreateTapText.text = LimLanguageManager.TextDict["Window_Creator_CreateTap"];
        CreateHoldText.text = LimLanguageManager.TextDict["Window_Creator_CreateHold"];
        CreateMotionHorText.text = LimLanguageManager.TextDict["Window_Creator_CreateMotionHor"];
        CreateMotionVerText.text = LimLanguageManager.TextDict["Window_Creator_CreateMotionVer"];
        CreateMotionRotText.text = LimLanguageManager.TextDict["Window_Creator_CreateMotionRot"];
        CreateMotionMaunallyText.text = LimLanguageManager.TextDict["Window_Creator_CreateMotionManually"];
        CreateBpmText.text = LimLanguageManager.TextDict["Window_Creator_CreateBpm"];
        CreateScrollSpeedText.text = LimLanguageManager.TextDict["Window_Creator_CreateScrollSpeed"];
        CreateCatchRailText.text = LimLanguageManager.TextDict["Window_Creator_CreateCatchRail"];
        CreateCatchRailQuantityText.text = LimLanguageManager.TextDict["Window_Creator_CreateCatchRail_Quantity"];
        AutoHighlightText.text = LimLanguageManager.TextDict["Window_Creator_AutoHighlight"];
        DeleteSelectedText.text = LimLanguageManager.TextDict["Window_Creator_DeleteSelected"];
        ConvertSelectedToHoldNoteText.text = LimLanguageManager.TextDict["Window_Creator_ConvertSelectedToHoldNote"];
        ClickToCreateText.text = LimLanguageManager.TextDict["Window_Creator_ClickToCreate"];
        AngleLineText.text = LimLanguageManager.TextDict["Window_Creator_Angleline"];
        CopierText.text = LimLanguageManager.TextDict["Window_Creator_Copier"];
    }
    private void Start()
    {
        ArrangeCreatorsUi();
        BaseWindow.OnWindowSorted.AddListener(OnWindowSorted);
    }
    private void OnWindowSorted(int Order)
    {

    }
    private void Update()
    {
        DetectHotkeys();
        DetectMouseScroll();
    }
    private void DetectHotkeys()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.T)) CreateTapNote();
            else if (Input.GetKeyDown(KeyCode.H)) CreateHoldNote();
            else if (Input.GetKeyDown(KeyCode.B)) CreateBpm();
            else if (Input.GetKeyDown(KeyCode.F)) CreateScrollSpeed();
        }
    }
    private void DetectMouseScroll()
    {
        float Scroll = -Input.GetAxis("Mouse ScrollWheel") * 200;
        if (Scroll != 0)
        {
            Vector3 Mouse = LimMousePosition.MousePosition;
            if (Mouse.x >= ViewRect.anchoredPosition.x && Mouse.x <= ViewRect.anchoredPosition.x + ViewRect.sizeDelta.x)
            {
                if (Mouse.y >= ViewRect.anchoredPosition.y - ViewRect.sizeDelta.y && Mouse.y <= ViewRect.anchoredPosition.y)
                {
                    ContentRect.anchoredPosition = new Vector2(0, Mathf.Clamp(ContentRect.anchoredPosition.y + Scroll, 0, Mathf.Max(0, ContentRect.sizeDelta.y - ViewRect.sizeDelta.y)));
                }
            }
        }
    }

    public void CreateTapNote()
    {
        if (LimSystem.ChartContainer == null) return;
        Lanotalium.Chart.LanotaTapNote New = new Lanotalium.Chart.LanotaTapNote();
        New.Time = TunerManager.ChartTime;
        OperationManager.AddTapNote(New);
        OperationManager.InspectorManager.ArrangeComponentsUi();
    }
    public void CreateHoldNote()
    {
        if (LimSystem.ChartContainer == null) return;
        Lanotalium.Chart.LanotaHoldNote New = new Lanotalium.Chart.LanotaHoldNote();
        New.Type = 5;
        New.Time = TunerManager.ChartTime;
        New.Duration = 1;
        OperationManager.AddHoldNote(New);
        OperationManager.InspectorManager.ArrangeComponentsUi();
    }
    public void CreateMotionHorizontal()
    {
        if (LimSystem.ChartContainer == null) return;
        Lanotalium.Chart.LanotaCameraXZ New = new Lanotalium.Chart.LanotaCameraXZ();
        New.Time = TunerManager.ChartTime;
        New.Type = 8;
        New.Duration = 0.00001f;
        OperationManager.AddHorizontal(New);
        OperationManager.InspectorManager.ArrangeComponentsUi();
    }
    public void CreateMotionVertical()
    {
        if (LimSystem.ChartContainer == null) return;
        Lanotalium.Chart.LanotaCameraY New = new Lanotalium.Chart.LanotaCameraY();
        New.Time = TunerManager.ChartTime;
        New.Type = 10;
        New.Duration = 0.00001f;
        OperationManager.AddVertical(New);
        OperationManager.InspectorManager.ArrangeComponentsUi();
    }
    public void CreateMotionRotation()
    {
        if (LimSystem.ChartContainer == null) return;
        Lanotalium.Chart.LanotaCameraRot New = new Lanotalium.Chart.LanotaCameraRot();
        New.Time = TunerManager.ChartTime;
        New.Type = 13;
        New.Duration = 0.00001f;
        OperationManager.AddRotation(New);
        OperationManager.InspectorManager.ArrangeComponentsUi();
    }
    public void CreateGizmoMotion()
    {
        if (LimSystem.ChartContainer == null) return;
        TunerManager.MediaPlayerManager.IsPlaying = false;
        GizmoMotionManager.Create();
    }
    public void CreateBpm()
    {
        if (LimSystem.ChartContainer == null) return;
        Lanotalium.Chart.LanotaChangeBpm New = new Lanotalium.Chart.LanotaChangeBpm();
        New.Time = TunerManager.ChartTime < 0 ? 0 : TunerManager.ChartTime;
        New.Bpm = 100;
        OperationManager.AddBpm(New);
        OperationManager.InspectorManager.ArrangeComponentsUi();
    }
    public void CreateScrollSpeed()
    {
        if (LimSystem.ChartContainer == null) return;
        if (TunerManager.ScrollManager.DisableChartSpeed) return;
        Lanotalium.Chart.LanotaScroll New = new Lanotalium.Chart.LanotaScroll
        {
            Time = TunerManager.ChartTime < 0 ? 0 : TunerManager.ChartTime,
            Speed = 1
        };
        OperationManager.AddScrollSpeed(New);
        OperationManager.InspectorManager.ArrangeComponentsUi();
    }

    public void CreateCatchRail()
    {
        if (LimSystem.ChartContainer == null) return;
        int Quantity;
        if (OperationManager.SelectedTapNote.Count != 2) { LimNotifyIcon.ShowMessage(LimLanguageManager.TextDict["Window_Creator_CreateCatchRail_ErrSelection"]); return; }
        if (!int.TryParse(CreateCatchRailQuantityInputField.text, out Quantity)) { LimNotifyIcon.ShowMessage(LimLanguageManager.TextDict["Window_Creator_CreateCatchRail_ErrQuantity"]); return; }
        if (Quantity <= 0) { LimNotifyIcon.ShowMessage(LimLanguageManager.TextDict["Window_Creator_CreateCatchRail_ErrRange"]); return; }
        OperationManager.SelectedTapNote.Sort((Lanotalium.Chart.LanotaTapNote a, Lanotalium.Chart.LanotaTapNote b) => { return a.Time.CompareTo(b.Time); });
        float DeltaTime = (OperationManager.SelectedTapNote[1].Time - OperationManager.SelectedTapNote[0].Time) / (Quantity + 1);
        float DeltaDegree = (OperationManager.SelectedTapNote[1].Degree - OperationManager.SelectedTapNote[0].Degree) / (Quantity + 1);
        for (int i = 1; i <= Quantity; ++i)
        {
            Lanotalium.Chart.LanotaTapNote New = OperationManager.SelectedTapNote[0].DeepCopy();
            New.Type = 4;
            New.Time += i * DeltaTime;
            New.Degree += i * DeltaDegree;
            OperationManager.AddTapNote(New, true, false, false);
        }
        LimNotifyIcon.ShowMessage(LimLanguageManager.TextDict["Window_Creator_CreateCatchRail_Success"]);
    }
    public void ConvertSelectedToHoldNote()
    {
        if (LimSystem.ChartContainer == null) return;
        OperationManager.SelectedTapNote.Sort((Lanotalium.Chart.LanotaTapNote a, Lanotalium.Chart.LanotaTapNote b) => { return a.Time.CompareTo(b.Time); });
        int Quantity = OperationManager.SelectedTapNote.Count;
        if (Quantity == 0) return;
        else if (Quantity == 1) { OperationManager.ConvertTapNoteToHoldNote(OperationManager.SelectedTapNote[0]); return; }
        else
        {
            Lanotalium.Chart.LanotaHoldNote New = new Lanotalium.Chart.LanotaHoldNote
            {
                Duration = OperationManager.SelectedTapNote[Quantity - 1].Time - OperationManager.SelectedTapNote[0].Time,
                Time = OperationManager.SelectedTapNote[0].Time,
                Degree = OperationManager.SelectedTapNote[0].Degree,
                Type = 5,
                Size = 1,
                Jcount = Quantity - 1,
                Joints = new List<Lanotalium.Chart.LanotaJoints>()
            };
            float DegreeCount = New.Degree;
            float TimeCount = New.Time;
            for (int i = 1; i < Quantity; ++i)
            {
                Lanotalium.Chart.LanotaJoints NewJ = new Lanotalium.Chart.LanotaJoints();
                NewJ.Cfmi = 0;
                NewJ.dTime = Mathf.Max(0.0001f, OperationManager.SelectedTapNote[i].Time - TimeCount);
                NewJ.dDegree = OperationManager.SelectedTapNote[i].Degree - DegreeCount;
                New.Joints.Add(NewJ);
                TimeCount += NewJ.dTime;
                DegreeCount += NewJ.dDegree;
            }
            foreach (Lanotalium.Chart.LanotaTapNote Tap in OperationManager.SelectedTapNote) OperationManager.DeleteTapNote(Tap);
            OperationManager.SelectedTapNote.Clear();
            OperationManager.AddHoldNote(New, true, true, true);
        }
    }
    public void AutoHighlight()
    {
        Dictionary<string, int> Occurance = new Dictionary<string, int>();
        foreach (Lanotalium.Chart.LanotaTapNote Tap in TunerManager.TapNoteManager.TapNote)
        {
            if (!Occurance.ContainsKey(Tap.Time.ToString("f4"))) Occurance[Tap.Time.ToString("f4")] = 0;
            Occurance[Tap.Time.ToString("f4")]++;
        }
        foreach (Lanotalium.Chart.LanotaHoldNote Hold in TunerManager.HoldNoteManager.HoldNote)
        {
            if (!Occurance.ContainsKey(Hold.Time.ToString("f4"))) Occurance[Hold.Time.ToString("f4")] = 0;
            Occurance[Hold.Time.ToString("f4")]++;
        }
        foreach (Lanotalium.Chart.LanotaTapNote Tap in TunerManager.TapNoteManager.TapNote)
        {
            OperationManager.SetTapNoteCombination(Tap, (Occurance[Tap.Time.ToString("f4")] > 1));
        }
        foreach (Lanotalium.Chart.LanotaHoldNote Hold in TunerManager.HoldNoteManager.HoldNote)
        {
            OperationManager.SetHoldNoteCombination(Hold, (Occurance[Hold.Time.ToString("f4")] > 1));
        }
    }

    public void DeleteSelected()
    {
        if (LimSystem.ChartContainer == null) return;
        OperationManager.DeleteAllSelected();
        OperationManager.InspectorManager.ArrangeComponentsUi();
    }

    public void ArrangeCreatorsUi()
    {
        float Height = -375;
        Height -= ClickToCreateManager.ToolBase.Height;
        AngleLineManager.ToolBase.ToolRect.anchoredPosition = new Vector2(0, Height);
        Height -= AngleLineManager.ToolBase.Height;
        CopierManager.ToolBase.ToolRect.anchoredPosition = new Vector2(0, Height);
        Height -= CopierManager.ToolBase.Height;
        ContentRect.sizeDelta = new Vector2(0, -Height);
    }
}