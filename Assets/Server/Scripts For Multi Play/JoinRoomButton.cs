using UnityEngine;
using UnityEngine.EventSystems;

public class JoinRoomButton : MonoBehaviour
{
    public static void JoinRoom()
    {
        string _roomId = EventSystem.current.currentSelectedGameObject.name;
        ClientSend.JoinRoom(_roomId);
        UIManagerInMultiPlayer.instance.lobbyUI.SetActive(false);
    }
}
