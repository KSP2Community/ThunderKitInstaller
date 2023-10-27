using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;
using System;
using System.Collections.Generic;

[InitializeOnLoad]
public class Startup
{
    public static List<string> Packages { get; set; } = new List<string>() { "https://github.com/PassivePicasso/ThunderKit.git#8.0.3", "com.unity.ui", "com.unity.ui.builder", "com.unity.burst", "com.unity.inputsystem" };

    static Startup()
    {
        try
        {
            var listRequest = Client.List(true, false);
            bool escape = false;
            while(!listRequest.IsCompleted && !escape)
            {
                bool x = escape; //Break and set in case of unexpected infinite loop
            }
            if(escape)
                return;

            if(listRequest.Status == StatusCode.Success)
            {
                bool found = false;
                foreach (var item in listRequest.Result)
                {
                    if(item.packageId.StartsWith("com.passivepicasso.thunderkit"))
                        found = true;
                }
                if(!found)
                {
                    foreach(string package in Packages)
                    {
                        if(!InstallPackage(package))
                            return;
                    }
                    if(AssetDatabase.DeleteAsset("Assets/Thunderkit Installer"))
                        Debug.Log($"Installation Complete.");
                }
                return;
            }
                Debug.Log($"{listRequest.Error.errorCode}: {listRequest.Error.message}");
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

    public static bool InstallPackage(string package)
    {
        try
        {
            Request result = Client.Add(package);
            bool escape = false;
            while(!result.IsCompleted && !escape)
            {
                bool x = escape; //Break and set in case of unexpected infinite loop
            }
            if(escape)
                return false;

            if(result.Status != StatusCode.Success)
            {
                Debug.Log(result.Error);
                return false;
            }

            Debug.Log($"Installed {package}.");
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }
}