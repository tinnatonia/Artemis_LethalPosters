﻿using System.Collections.Generic;
using System.IO;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace LethalPosters.Patches;

internal class StartOfRoundPatches
{
    private static ManualLogSource Logger { get; set; }

    public static void Init(ManualLogSource logger)
    {
        Logger = logger;
    }

    [HarmonyPatch(typeof(StartOfRound), "Start")]
    [HarmonyPostfix]
    private static void StartPatch()
    {
        var rnd = new System.Random();
        var materials = GameObject.Find("Plane.001").GetComponent<MeshRenderer>().materials;

        UpdateTexture(rnd, Plugin.PosterFiles, materials[0]);
        UpdateTexture(rnd, Plugin.TipFiles, materials[1]);
    }

    private static void UpdateTexture(System.Random rnd, List<string> files, Material material)
    {
        if (files.Count == 0) {return;}

        var index = rnd.Next(files.Count - 1);
        var texture = new Texture2D(2, 2);
        texture.LoadImage(File.ReadAllBytes(files[index]));
        
        material.mainTexture = texture;
    }
}