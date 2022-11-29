using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager instance;
    //  { get; private set; }
    public string username;
    public string accessToken;
    public string refreshToken;
    public bool isLogined = false;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists,destroying object!");
            Destroy(this);
        }

        Reset();
        DontDestroyOnLoad(gameObject);
    }
    
    public void Reset() {
        username = "";
        accessToken = "";
        refreshToken = "";
        isLogined = false;
    }

    public void SetAccessToken(string _accessToken)
    {
        accessToken = _accessToken;
    }

    public void SetToken(string _accessToken, string _refreshToken)
    {
        accessToken = _accessToken;
        refreshToken = _refreshToken;
    }

    public void SetUserName(string _username) {
        username = _username;
    }
}
