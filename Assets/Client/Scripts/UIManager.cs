using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;


[Serializable]
public class Data
{
    public string ok;
    public Token[] tokens;
}

[Serializable]
public class Token
{
    public string accessToken;
    public string refreshToken;
}

public class UIManager : MonoBehaviour
{
    //�� private field�� serialize�� �ȵɱ�
    [Serializable]
    private class User
    {
        public string user_id;
        public string password;
        public string accessToken;
        public string refreshToken;
    }

    User user = new User();
    Data d;
    Token token = new Token();
    public string authorization;

    public static UIManager instance;

    public GameObject startMenu;
    public GameObject multiMenu;
    public GameObject loginMenu;
    public GameObject logintestMenu;
    public GameObject connectingMenu;
    public Button refreshButton;
    public InputField usernameField;
    public InputField idField;
    public InputField passwordField;

    private Coroutine loginCoroutine = null;
    private Coroutine refreshCoroutine = null;
    private Coroutine logoutCoroutine = null;



    private void Awake()
    {
        //Singleton ����
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists,destroying object!");
            Destroy(this);
        }
    }

    public void SinglePlayButtonClicked()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void MultyPlayButtonClicked()
    {
        startMenu.SetActive(false);
        multiMenu.SetActive(true);
    }

    public void SubmitButtonClicked()
    {
        SceneManager.LoadScene("MultiplaySample");
    }

    public void LoginButtonClicked()
    {
        StartCoroutine(IneternetConnectCheck(isConnected =>
        {
            if (isConnected)
            {
                Debug.Log("Server Available!");
            }
            else
            {
                Debug.Log("Internet or server Not Available");
            }
        }));

        IEnumerator IneternetConnectCheck(Action<bool> action)
        {
            using (UnityWebRequest request = new UnityWebRequest("http://localhost:3001/"))
            {

                request.downloadHandler = new DownloadHandlerBuffer();
;
                yield return request.SendWebRequest();
                if (request.error != null)
                {
                    action(false);

                }
                else
                {
                    action(true);
                    startMenu.SetActive(false);
                    loginMenu.SetActive(true);
                }
                request.downloadHandler.Dispose();
                request.Dispose();
            }
        }
    }
    public void LoginSubmitButtonClicked()
    {
        if (idField.text == "" || passwordField.text == "")
            return;

        user.user_id = idField.text;
        user.password = passwordField.text;
        string json = JsonUtility.ToJson(user);

        if(loginCoroutine == null) {
            Debug.Log(json);
            loginCoroutine = StartCoroutine(Login());
        } else {
            Debug.Log("Can not start loginCoroutine. Already logined.");
        }
        IEnumerator Login(){
            bool isDone = false;
            yield return StartCoroutine(LoginProcess((_Tocken) =>
            {
                d = JsonUtility.FromJson<Data>(_Tocken);
                user.accessToken = d.tokens[0].accessToken;
                user.refreshToken = d.tokens[0].refreshToken;
                Debug.Log($"{d.tokens[0].accessToken},{d.tokens[0].refreshToken}");
                loginMenu.SetActive(false);
                logintestMenu.SetActive(true);
                isDone = true;
            }));
            if(isDone) loginCoroutine = null;
        }

        IEnumerator LoginProcess(Action<string> Tocken)
        {
            using ( UnityWebRequest request = UnityWebRequest.Post("http://localhost:3001/api/auth/login", json))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                request.SetRequestHeader("Content-Type", "application/json");

                // dH = new DownloadHandlerBuffer();
                request.uploadHandler.Dispose();

                request.uploadHandler = new UploadHandlerRaw(jsonToSend);;
                request.downloadHandler = new DownloadHandlerBuffer();


                //�� �� ��ũ�� ������ ����ϸ�..
                //UnityWebRequest�� ����Ͽ� �ؽ��ĸ� �ٿ�ε带 �� ��, �ٿ�ε�� ���ҽ��� ó���ϱ� ���� ����
                //www.downloadHandler.data�� Access �ϰ� �Ǹ�, ������ ���� ���ο��� ���ο� ����Ʈ �迭�� �����ǹǷ� �޸� ������ �ſ� ����ϴٴ� ����.
                //�� ������ ������������ �� �޸� ���� ���� ����� �������� �𸣰���


                yield return request.SendWebRequest();
                authorization = request.downloadHandler.text;

                string temp = authorization;
                string json2 = JsonUtility.ToJson(temp);
                //Tocken jsontemp = JsonUtility.FromJson<Tocken>(temp);

                if (request.error != null)
                {
                    Debug.Log(request.error);
                    //Tocken(request.downloadHandler.text);
                }
                else
                {
                    Debug.Log(temp);
                    Tocken(temp);
                }
                request.uploadHandler.Dispose();
                request.downloadHandler.Dispose();
                request.Dispose();
            }
        }
    }

    
        public void RefreshButtonClicked()
    {
        refreshButton.interactable = false;

        string json = JsonUtility.ToJson(user);

        if(refreshCoroutine == null) {
            refreshCoroutine = StartCoroutine(Refresh((_Tocken) =>
            {
                d = JsonUtility.FromJson<Data>(_Tocken);
                user.accessToken = d.tokens[0].accessToken;
                user.refreshToken = d.tokens[0].refreshToken;
                Debug.Log($"{d.tokens[0].accessToken},{d.tokens[0].refreshToken}");
            }));
            Invoke("setRefreshButtonEnable", 10f);
        } else {
            Debug.Log("Can not start refreshCoroutine.");
        }

        IEnumerator Refresh(Action<string> Tocken)
        {
            using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3001/api/auth/refresh", json))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

                request.uploadHandler.Dispose();

                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + user.accessToken);

                request.SetRequestHeader("refresh", user.refreshToken);

                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();
                authorization = request.downloadHandler.text;
                string temp = authorization;
                string json2 = JsonUtility.ToJson(temp);
                //Tocken jsontemp = JsonUtility.FromJson<Tocken>(temp);

                if (request.error != null)
                {
                    Debug.Log(request.error);
                    //Tocken(request.downloadHandler.text);
                }
                else
                {
                    Debug.Log(temp);
                    Tocken(temp);
                }

                request.uploadHandler.Dispose();
                request.downloadHandler.Dispose();
                request.Dispose();
            }
        }
    }

    public void LogOutButtonClicked()
    {
        string json = JsonUtility.ToJson(user);
        if(logoutCoroutine == null) {
            logoutCoroutine = StartCoroutine(Logout((isLogout) =>
            {
                user.accessToken = null;
                user.refreshToken = null;
                token.accessToken = null;
                token.refreshToken = null;

                logintestMenu.SetActive(false);
                startMenu.SetActive(true);
            }));
        }

        IEnumerator Logout(Action<bool> isLogout)
        {
            using (UnityWebRequest request = UnityWebRequest.Delete("http://localhost:3001/api/auth/logout"))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

                
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + user.accessToken);
                Debug.Log(authorization);

                request.uploadHandler = new UploadHandlerRaw(jsonToSend);

                yield return request.SendWebRequest();

                if (request.error != null)
                {
                    Debug.Log("failed");
                    Debug.Log(request.error);
                    isLogout(false);
                }
                else
                {
                    Debug.Log("log out complete");
                    isLogout(true);
                }

                
                request.Dispose();
                request.uploadHandler.Dispose();
                logoutCoroutine = null;
            }
        }
    }

    private void setRefreshButtonEnable() {
        refreshButton.interactable = true;
        refreshCoroutine = null;
    }

    public void QuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
