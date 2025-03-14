using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;

public class EvaluationModule : MonoBehaviour
{
    private static Dictionary<string, object> playerData;
    
    private void Start()
    {
        SetupCloudSave();
        
        playerData = new Dictionary<string, object>()
        {
            { "gameMode", "unknown" },
        };
        
        foreach (EvaluationTrigger trigger in FindObjectsOfType<EvaluationTrigger>())
        {
            playerData.Add(trigger.TriggerLabel + "_triggered", false);
            playerData.Add(trigger.TriggerLabel + "_duration_in_area", 0.0);
            playerData.Add(trigger.TriggerLabel + "_times_triggered", 0);
        }
    }

    private async void SetupCloudSave()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        UpdatePlayerData();
    }

    private static async void UpdatePlayerData()
    {
        await CloudSaveService.Instance.Data.ForceSaveAsync(playerData);
    }

    public static void UpdateKey(string key, object value)
    {
        playerData[key] = value;
        UpdatePlayerData();
    }

    private void OnApplicationQuit()
    {
        UpdatePlayerData();
    }
}
