using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Singleton class to map input devices to a player index.
public class PlayerDeviceManager : MonoBehaviour {
    public const int MAX_PLAYERS = 4;

    private static PlayerDeviceManager instance;

    // We want this to be static so that it persists between scenes.
    private static int[] deviceIds = new int[MAX_PLAYERS]; // When the same device is disconnected and reconnected, it will be assigned a different id.
    private static int numPlayers = 0;

    public delegate void PlayerAssignCallback(int playerIndex, int deviceId);
    public PlayerAssignCallback onPlayerAssigned;
    public PlayerAssignCallback onPlayerUnassigned;

    public static PlayerDeviceManager GetInstance() { return instance; }

    public int GetNumPlayers() {
        return numPlayers;
    }

    public bool IsPlayerDevice(int playerIndex, int deviceId) {
        return deviceIds[playerIndex] == deviceId;
    }

    private void Awake() {
        if (instance == null) {
            Initialise();
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Update() {
    }

    private void OnDestroy() {
        if (instance == this) {
            Uninitialise();
        }
    }

    private void Initialise() {
        ResetDevices();

        // Iterate through all existing devices, and assign them to players.
        for (int i = 0; i < InputSystem.devices.Count; ++i) {
            // TODO: Will there ever be a case where a device is disconnected but not removed?
            if (!IsSuitableDevice(InputSystem.devices[i])) continue;
            TryAssignDevice(InputSystem.devices[i].deviceId);
        }

        // Listen for any changes to connected devices.
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void Uninitialise() {
        InputSystem.onDeviceChange -= OnDeviceChange;
        ResetDevices();
    }

    private void ResetDevices() {
        for (int i = 0; i < deviceIds.Length; ++i) { deviceIds[i] = InputDevice.InvalidDeviceId; }
        numPlayers = 0;
    }

    // Change this function to fit your game's needs.
    private bool IsSuitableDevice(InputDevice device) {
        // Ignore anything that isn't a gamepad or joystick.
        return (device is Gamepad) || (device is Joystick);
    }

    private bool IsDeviceAssigned(int deviceId) {
        // This runs in O(n) time, but I'm fine with it since n = 4.
        for (int i = 0; i < deviceIds.Length; ++i) {
            if (deviceIds[i] == deviceId) {
                return true;
            }
        }
        return false;
    }

    // Assign this device to the first player that does not yet have a device.
    private void TryAssignDevice(int deviceId) {
        for (int i = 0; i < deviceIds.Length; ++i) {
            if (deviceIds[i] != InputDevice.InvalidDeviceId) { continue; }
            deviceIds[i] = deviceId;
            ++numPlayers;
            onPlayerAssigned?.Invoke(i, deviceId);
            Debug.Log("Player " + i.ToString() + " assigned device " + deviceId.ToString() + ".");
            break;
        }
    }

    // Unassign this device if it has already been assigned to a player.
    private void TryUnassignDevice(int deviceId) {
        for (int i = 0; i < deviceIds.Length; ++i) {
            if (deviceIds[i] != deviceId) { continue; }
            deviceIds[i] = InputDevice.InvalidDeviceId;
            --numPlayers;
            onPlayerUnassigned?.Invoke(i, deviceId);
            Debug.Log("Player " + i.ToString() + " unassigned device " + deviceId.ToString() + ".");
            break;
        }
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change) {
        if (!IsSuitableDevice(device)) return;

        switch (change) {
            case InputDeviceChange.Added:
            case InputDeviceChange.Reconnected:
            case InputDeviceChange.Enabled:
                if (!IsDeviceAssigned(device.deviceId)) {
                    TryAssignDevice(device.deviceId);
                }
                break;
            case InputDeviceChange.Removed:
            case InputDeviceChange.Disconnected:
            case InputDeviceChange.Disabled:
                if (IsDeviceAssigned(device.deviceId)) {
                    TryUnassignDevice(device.deviceId);
                }
                break;
            default:
                break;
        }
    }
}