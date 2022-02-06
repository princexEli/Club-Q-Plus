using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

public class PrivateRoomManager : UdonSharpBehaviour
{
    public Toggle lockToggle;
    public GameObject lockUI;

    public GameObject roomHandle;
    public GameObject entryHandle;

    private UdonBehaviour roomHandleTeleport;
    private UdonBehaviour entryHandleTeleport;

    public Text roomText;
    public Text entryText;

    [UdonSynced]
    private bool globalToggleVal;

    const string OPEN_STRING = "OPEN";
    const string LOCKED_STRING = "LOCKED";

    private bool newUser;
    private bool isUpdate;
    
    void Start()
    {
        roomHandleTeleport = (UdonBehaviour)roomHandle.GetComponent(typeof(UdonBehaviour));
        entryHandleTeleport = (UdonBehaviour)entryHandle.GetComponent(typeof(UdonBehaviour));

        newUser = true;
        lockToggle.isOn = false;
        isUpdate = false;

        if (Networking.IsMaster)
        {
            Networking.SetOwner(Networking.LocalPlayer, lockUI);
            globalToggleVal = false;
            newUser = false;
            setDoorsOpen();
        }
    }

    private void Update()
    {
        if (newUser)
        {
            isUpdate = true;
            setDoors();
            newUser = false;
            isUpdate = false;
        }
    }

    public override void OnDeserialization()
    {
        isUpdate = true;
        if ((lockToggle.isOn != globalToggleVal))
        {
            setDoors();
        }
        isUpdate = false;
    }

    //Called by toggle in UI
    public void onToggleChanged()
    {
        if (!isUpdate)
        {
            if (!Networking.IsOwner(lockUI))
            {
                Networking.SetOwner(Networking.LocalPlayer, lockUI);
            }

            globalToggleVal = lockToggle.isOn;
            setDoors();
            RequestSerialization();
        }
    }

    #region Doors
    private void setDoors()
    {
        lockToggle.isOn = globalToggleVal;
        if (lockToggle.isOn)
        {
            setDoorsLocked();
        }
        else
        {
            setDoorsOpen();
        }
    }

    private void setDoorsOpen()
    {
        roomText.text = OPEN_STRING;
        entryText.text = OPEN_STRING;

        roomHandleTeleport.DisableInteractive = false;
        entryHandleTeleport.DisableInteractive = false;
    }

    private void setDoorsLocked()
    {
        roomText.text = LOCKED_STRING;
        entryText.text = LOCKED_STRING;

        roomHandleTeleport.DisableInteractive = true;
        entryHandleTeleport.DisableInteractive = true;
    }
    #endregion
}
