using UnityEngine;
#if ML
using Il2Cpp;
#elif BIE
using BepInEx.IL2CPP;
#endif

namespace Mod;

public static class SprintUnlocker
{
    private static bool _enabled = true;
    public static bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;

            if (value)
            {
                ModCore.Loader.Update += OnUpdate;
            }
            else
            {
                ModCore.Loader.Update -= OnUpdate;
                if (IsPlayerMoveValid())
                {
                    SetPlayerRunState(false);
                }
            }

            ModCore.Log(value ? "Enabled" : "Disabled");
        }
    }

    private static PlayerMove? _playerMove;

    public static void Init()
    {
        if (Enabled)
        {
            ModCore.Loader.Update += OnUpdate;
        }

        ModCore.Log("Mod Initialized");
    }

    public static void SetPlayerRunState(bool value)
    {
        try
        {
            PlayerMove? playerMove = GetPlayerMove();
            if (playerMove == null)
            {
                return;
            }

            playerMove.canRun = value;
        }
        catch (Exception e)
        {
            ModCore.LogError(e.Message);
        }
    }

    private static void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetPlayerRunState(true);
        }
    }

    private static PlayerMove? GetPlayerMove()
    {
        if (!IsPlayerMoveValid())
        {
            _playerMove = GameObject.Find("Player")?.GetComponent<PlayerMove>();
        }
        return _playerMove;
    }

    private static bool IsPlayerMoveValid()
    {
        return _playerMove != null && _playerMove.gameObject != null;
    }
}
