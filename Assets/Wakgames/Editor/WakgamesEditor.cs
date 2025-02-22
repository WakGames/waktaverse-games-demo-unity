using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using WakSDK;

public class WakgamesEditor : EditorWindow
{
    private WakgamesClientData _clientData;
    private TextField _clientIDField;
    private TextField _callbackServerPortField;
    private Toggle _achieveAlarmToggle;
    private Toggle _achieveSfxToggle;
    private EnumField _achieveAlarmPosition;
    
    [MenuItem("Tools/Wakgames")]
    public static void ShowMyEditor()
    {
        EditorWindow wnd = GetWindow<WakgamesEditor>();
        wnd.titleContent = new GUIContent("Wakgames Setup");
        wnd.maxSize = new Vector2(300, 500);
        wnd.minSize = new Vector2(300, 500);
    }
    
    public void CreateGUI()
    {
        Texture2D LogoTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Wakgames/Resources/Sprites/Icon+Text(Color).png"); 
        VisualElement ve = new VisualElement();
        Image logoImg = new Image();
        logoImg.image = LogoTex;
        ve.Add(logoImg);
        rootVisualElement.Add(ve);
        
        _clientData = AssetDatabase.LoadAssetAtPath<WakgamesClientData>("Assets/Wakgames/Resources/ScriptableObjects/ClientData.asset");

        if (_clientData == null)
        {
            Debug.LogError("ClientData를 찾을 수 없습니다. 경로와 타입을 확인하세요.");
            return;
        }
        
        _clientIDField = new TextField("클라이언트 ID") //Client ID
        {
            value = _clientData.ClientID
        };
        rootVisualElement.Add(_clientIDField);
        _callbackServerPortField = new TextField("게임 서버 포트") //Callback Server Port
        {
            value = _clientData.CallbackServerPort.ToString()
        };
        rootVisualElement.Add(_callbackServerPortField);
        _achieveAlarmToggle = new Toggle("도전과제 알림");
        _achieveAlarmToggle.value = _clientData.AchieveAlarmToggle;
        rootVisualElement.Add(_achieveAlarmToggle);
        _achieveSfxToggle = new Toggle("도전과제 SFX");
        _achieveSfxToggle.value = _clientData.AchieveSfxToggle;
        rootVisualElement.Add(_achieveSfxToggle);
        _achieveAlarmPosition = new EnumField(AchievePopupPosition.우측하단)
        {
            label = "도전과제 위치"
        };
        _achieveAlarmPosition.value = _clientData.AchieveAlarmPosition;
        rootVisualElement.Add(_achieveAlarmPosition);
        Button btn = new Button(ApplyChanges)
        {
            text = "변경사항 저장"
        };
        rootVisualElement.Add(btn);
        rootVisualElement.Add(new Label("<a href=\"https://docs.waktaverse.games/\">적용 가이드라인 확인하러가기</a>"));
    }

    private void ApplyChanges()
    {
        if (_clientIDField.value == null) return;
        if (_callbackServerPortField.value == null) return;
        if (!int.TryParse(_callbackServerPortField.value, out int port)) return;
        
        _clientData.ClientID = _clientIDField.value;
        _clientData.CallbackServerPort = port;
        _clientData.AchieveAlarmToggle = _achieveAlarmToggle.value;
        _clientData.AchieveSfxToggle = _achieveSfxToggle.value;
        _clientData.AchieveAlarmPosition = (AchievePopupPosition)_achieveAlarmPosition.value;
        
        // 변경사항 저장
        EditorUtility.SetDirty(_clientData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}