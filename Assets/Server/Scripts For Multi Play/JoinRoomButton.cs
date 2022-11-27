using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoinRoomButton : MonoBehaviour
{
    public static void JoinRoom()
    {
        string _roomId = EventSystem.current.currentSelectedGameObject.name.Split('_')[1];
        UIManagerInMultiPlayer.instance.JoinClicked(_roomId);
        UIManagerInMultiPlayer.instance.lobbyUI.SetActive(false);
    }
}
