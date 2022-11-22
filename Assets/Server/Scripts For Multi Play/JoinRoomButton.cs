using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoinRoomButton : MonoBehaviour
{
    public static void JoinRoom()
    {
        string _roomId = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        UIManagerInMultiPlayer.instance.JoinClicked(_roomId);
        UIManagerInMultiPlayer.instance.lobbyUI.SetActive(false);
    }
}
