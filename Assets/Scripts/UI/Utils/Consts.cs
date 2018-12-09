using System;
using System.Collections;
using System.Collections.Generic;
using ScriptablePattern;
using UnityEngine;

public class Consts
{
    public const string c_profiles_folderName = "Profiles";
    public const string c_profiles_profileName = "Profile_";
    public const string c_data_folderName = "Data";
    public const string c_runtimeAssets_folderName = "RuntimeAssets";
    public const string c_profiles_formatName = ".profile";
    public const string c_utils_folderName = "Utils";
    public const string c_utils_scenesName = "Scenes";
    public const string c_utils_formatName = ".asset";
    public const string c_game_gameController_name = "GameController";
    public const string c_game_fader_name = "Fader";
    public const string c_game_spawnPoint_player1 = "SpawnPlayer1";
    public const string c_game_spawnPoint_player2 = "SpawnPlayer2";
    public const string c_game_LayerName_player1 = "FirstPlayer";
    public const string c_game_LayerName_player2 = "SecondPlayer";

    public const int c_game_liveMaxCount = 3;
    
    public enum BattleType
    {
        SinglePlayer,
        MultiPlayer,
        PvP,
        PvE
    }
}