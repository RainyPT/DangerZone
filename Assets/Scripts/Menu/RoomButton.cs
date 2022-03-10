using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class RoomButton : MonoBehaviour
{
    public TMP_Text RoomName;
    public TMP_Text GamemodeText;
    public TMP_Text PlayerCountText;
    private RoomInfo roomInfo;
    

    public void SetButtonDetails(RoomInfo info)
    {
        roomInfo = info;
        RoomName.text = info.Name;
        PlayerCountText.text = info.PlayerCount + "\\" + info.MaxPlayers;
    }
}
