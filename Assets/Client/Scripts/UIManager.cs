using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Text.RegularExpressions;

[Serializable]
public class Data
{
    public string ok;
    public Token[] tokens;
    public string username;
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

    [Serializable]
    private class SignUpData
    {
        public string user_id;
        public string password;
        public string email;
        public string username;
    }


    User user = new User();
    SignUpData signUpData = new SignUpData();
    Data d;

    public string authorization;

    public static UIManager instance;

    public NoticeUI _noticeUI;

    public GameObject startMenu;
    
    public GameObject loginMenu;
    public GameObject signUpMenu;
    public GameObject signUpPanel;
    public GameObject logintestMenu;

    public Button MultiPlayButton;

    public Button refreshButton;
    public Button loginSubmitButton;
    public GameObject logoutButton;
    public GameObject[] inactiveButtons;
    public InputField idField;
    public InputField passwordField;

    private Coroutine loginCoroutine = null;
    private Coroutine refreshCoroutine = null;
    private Coroutine logoutCoroutine = null;
    private Coroutine signUpCoroutine = null;

    private Regex emailRegression = new Regex(@"^([0-9a-zA-Z]+)@([0-9a-zA-Z]+)(\.[0-9a-zA-Z]+){1,}$");

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
        MultiPlayButton.interactable = false;
        if(UserDataManager.instance.isLogined)
        {
            logoutButton.SetActive(true);
            foreach(GameObject _button in inactiveButtons)
            {
                _button.SetActive(false);
            }
            MultiPlayButton.interactable = true;
        }
    }

    public void SinglePlayButtonClicked()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void MultyPlayButtonClicked()
    {

        SceneManager.LoadScene("MultiplaySample");
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
            using (UnityWebRequest request = new UnityWebRequest(UserDataManager.instance.apiUrl))
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
                UserDataManager.instance.SetToken(d.tokens[0].accessToken, d.tokens[0].refreshToken);
                UserDataManager.instance.SetUserName(d.username);
                UserDataManager.instance.isLogined = true;
                UserDataManager.instance.SetSocket();
                Debug.Log($"{d.tokens[0].accessToken},{d.tokens[0].refreshToken}");
                loginMenu.SetActive(false);
                startMenu.SetActive(true);
                idField.text = "";
                passwordField.text = "";
                logoutButton.SetActive(true);
                foreach(GameObject _button in inactiveButtons)
                {
                    _button.SetActive(false);
                }
                MultiPlayButton.interactable = true;
                isDone = true;
            }));
            if(isDone) loginCoroutine = null;
            else {
                _noticeUI.AlertBox("로그인 실패");
                Invoke("setLoginButtonEnable", 1.0f);
            }
        }

        IEnumerator LoginProcess(Action<string> Tocken)
        {
            using ( UnityWebRequest request = UnityWebRequest.Post(UserDataManager.instance.apiUrl + "api/auth/login", json))
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

    public void SignUpButtonClicked()
    {
        int dataCnt = signUpPanel.transform.childCount;
        for(int i = 0; i < dataCnt; ++i){
            InputField inputField;
            signUpPanel.transform.GetChild(i).gameObject.TryGetComponent<InputField>(out inputField);
            if(inputField != null) {
                inputField.text = "";
            }
        }
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
            using (UnityWebRequest request = new UnityWebRequest(UserDataManager.instance.apiUrl))
            {
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();
                if (request.error != null)
                {
                    action(false);
                }
                else
                {
                    action(true);
                    startMenu.SetActive(false);
                    signUpMenu.SetActive(true);
                }
                request.downloadHandler.Dispose();
                request.Dispose();
            }
        }
    }

    public void SignUpSubmitButtonClicked()
    {
        Dictionary<string, string> datas = new Dictionary<string, string>();
        int dataCnt = signUpPanel.transform.childCount;
        for(int i = 0; i < dataCnt; ++i){
            InputField inputField;
            signUpPanel.transform.GetChild(i).gameObject.TryGetComponent<InputField>(out inputField);
            if(inputField != null)
                datas.Add(inputField.name, inputField.text);

        }

        Debug.Log(datas.Count);
        foreach(KeyValuePair<string, string> _data in datas)
        {
            string _message = "OK";
            switch(_data.Key) {
                case "Id":
                    _message = CheckIdOrUsername(_data.Value, "Id");
                    break;
                case "Username":
                    _message = CheckIdOrUsername(_data.Value, "Username");
                    break;
                case "Email":
                    if(!emailRegression.IsMatch(_data.Value))
                        _message = "잘못된 이메일 형식입니다.";
                    break;
                case "Password":
                    if(_data.Value == "")
                        _message = "Password를 입력해주세요.";
                    break;
            }
            if(_message != "OK") {
                _noticeUI.AlertBox(_message);
                return;
            }
        }

        signUpData.user_id = datas["Id"];
        signUpData.email = datas["Email"];
        signUpData.username = datas["Username"];
        signUpData.password = datas["Password"];

        string json = JsonUtility.ToJson(signUpData);

        if(signUpCoroutine == null) {
            Debug.Log(json);
            signUpCoroutine = StartCoroutine(SignUp());
        } else {
            Debug.Log("Can not start signInCoroutine. Already logined.");
        }

        IEnumerator SignUp(){
            yield return StartCoroutine(SignUpProcess((isProcessDone) =>
            {
                signUpMenu.SetActive(false);
                startMenu.SetActive(true);
            }));
            signUpCoroutine = null;
        }

        IEnumerator SignUpProcess(Action<bool> Result)
        {
            using ( UnityWebRequest request = UnityWebRequest.Put(UserDataManager.instance.apiUrl + "api/user/apply", json))
            {
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                request.SetRequestHeader("Content-Type", "application/json");

                request.uploadHandler.Dispose();

                request.uploadHandler = new UploadHandlerRaw(jsonToSend);;
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();

                authorization = request.downloadHandler.text;

                string temp = authorization;
                string json2 = JsonUtility.ToJson(temp);

                if (request.error != null)
                {
                    Debug.Log(request.error);
                    _noticeUI.AlertBox("이미 존재하는 아이디입니다.");
                }
                else
                {
                    Debug.Log(temp);
                    Result(true);
                }
                request.uploadHandler.Dispose();
                request.downloadHandler.Dispose();
                request.Dispose();
            }
        }
    }

    private string CheckIdOrUsername(string _id, string _type)
    {
        string _msg = "";
        if(_id == "") 
            _msg = _type + "를 입력해주세요.";
        else if(_id.Contains("-") || _id.Contains("_"))
            _msg = _type + "에는 _ 또는 - 가 포함될 수 없습니다.";
        else
            _msg = "OK";
        return _msg;
    }

    public void RefreshButtonClicked()
    {
        refreshButton.interactable = false;

        // string json = JsonUtility.ToJson(user);

        if(refreshCoroutine == null) {
            refreshCoroutine = StartCoroutine(Refresh((_Tocken) =>
            {
                d = JsonUtility.FromJson<Data>(_Tocken);
                UserDataManager.instance.SetAccessToken(d.tokens[0].accessToken);
                Debug.Log($"{d.tokens[0].accessToken},{d.tokens[0].refreshToken}");
            }));
            Invoke("setRefreshButtonEnable", 10f);
        } else {
            Debug.Log("Can not start refreshCoroutine.");
        }

        IEnumerator Refresh(Action<string> Tocken)
        {
            using (UnityWebRequest request = UnityWebRequest.Post(UserDataManager.instance.apiUrl + "api/auth/refresh", ""))
            {
                string _accessToken = UserDataManager.instance.accessToken;
                string _refreshToken = UserDataManager.instance.refreshToken;

                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + _accessToken);
                request.SetRequestHeader("refresh", _refreshToken);
                request.downloadHandler = new DownloadHandlerBuffer();

                yield return request.SendWebRequest();
                authorization = request.downloadHandler.text;
                string temp = authorization;


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
                //
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
                if(isLogout) {
                    UserDataManager.instance.Reset();

                    logoutButton.SetActive(false);
                    foreach(GameObject _button in inactiveButtons)
                    {
                        _button.SetActive(true);
                    }
                    MultiPlayButton.interactable = false;
                    startMenu.SetActive(true);
                }
            }));
        }

        IEnumerator Logout(Action<bool> isLogout)
        {
            using (UnityWebRequest request = UnityWebRequest.Delete(UserDataManager.instance.apiUrl + "api/auth/logout"))
            {
                string _accessToken = UserDataManager.instance.accessToken;
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + _accessToken);
                Debug.Log(authorization);
                //
                yield return request.SendWebRequest();

                if (request.error != null)
                {
                    Debug.Log("failed");
                    Debug.Log(request.error);

                }
                else
                {
                    Debug.Log("log out complete");
                    isLogout(true);
                }

                
                request.Dispose();

                logoutCoroutine = null;
            }
        }
    }

    private void setRefreshButtonEnable() {
        refreshButton.interactable = true;
        refreshCoroutine = null;
    }

    private void setLoginButtonEnable() {
        loginSubmitButton.interactable = true;
        loginCoroutine = null;
    }
//

    public void BackToMainFromLogin() {
        idField.text = "";
        passwordField.text = "";
        loginMenu.SetActive(false);
        startMenu.SetActive(true);
        loginCoroutine = null;
    }

    public void BackToMainFromSignUp() {
        int dataCnt = signUpPanel.transform.childCount;
        for(int i = 0; i < dataCnt; ++i){
            InputField _inputField;
            signUpPanel.transform.GetChild(i).gameObject.TryGetComponent<InputField>(out _inputField);
            if(_inputField != null)
            {
                _inputField.text = "";
            }
        }
        signUpMenu.SetActive(false);
        startMenu.SetActive(true);
        signUpCoroutine = null;
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
