using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShowRoomController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] GameObject _mainCanvas;
    [SerializeField] CarDatabase _database = null;
    [SerializeField] Transform _spownPoint = null;
    [SerializeField] CameraMovementController _camera = null;

    [Header("Buttons")]
    [SerializeField] Button _buttonRight = null;
    [SerializeField] Button _buttonLeft = null;
    [SerializeField] Button _buttonScreenShot = null;
    [SerializeField] Button _buttonAutopan = null;
    [SerializeField] Button _buttonAccesories = null;
    [SerializeField] Button _buttonMenuRight = null;
    [SerializeField] Button _buttonMenuLeft = null;
    [SerializeField] Button _buttonLanguage = null;

    [Header("Color Picker")]
    [SerializeField] ColorPicker _colorPicker;
    [SerializeField] Color32 _color;

    [Header("MenuPanel")]
    [SerializeField] GameObject _panel = null;
    [SerializeField] Text _text = null;
    [SerializeField] GameObject _wheelsPanel = null;
    [SerializeField] InfoPanel _infoPanel = null;
    [SerializeField] WheelPanel _wheelPanel = null;

    public static Action<CarDatabase.Car> OnCarChange;

    private CarModel _selectedModel = null;
    private CarDatabase.Car _selectedCar = null;

    private List<GameObject> _pool = new List<GameObject>();
    private int _index = 0;
    private string _baseResourcePath = "cars";
    private int poolLimit = 40;

    private MeshRenderer _meshRenderer;

    private int _screenShotSize = 1;
    private int _screenShotIndex = 1;

    private MenuStates state = MenuStates.CarInfo;

    private void OnEnable()
    {
        _colorPicker.OnMetalValueChange += CarMetalicValueChange;
        _colorPicker.OnColorPressUp += CarColorSetDefaul;
        _colorPicker.OnColorOver += CarColorChange;
        _colorPicker.OnColorPressed += CarColorChange;
        _buttonRight.onClick.AddListener(delegate { ChoiseNextCar(1); });
        _buttonLeft.onClick.AddListener(delegate { ChoiseNextCar(-1); });
        _buttonScreenShot.onClick.AddListener(OnPressScreenShotButton);
        _buttonAutopan.onClick.AddListener(AutoPan);
        _buttonAccesories.onClick.AddListener(OpenPanel);
        _buttonMenuRight.onClick.AddListener(delegate { NextMenuTab(1); });
        _buttonMenuLeft.onClick.AddListener(delegate { NextMenuTab(-1); });
        _buttonLanguage.onClick.AddListener(OnChangeLanguage);
        _wheelPanel.PassDelegate(SetNewWheels);
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _camera.ReleaseCamera();
        }

        //////**/////////
        // TESTING PURPOSE
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetNewWheels("Wheel1");
        }

        // testing purpose
        if (Input.GetKeyDown(KeyCode.S))
        {
            CarWheelsSetDefault();
        }

        // testing purpose
        if (Input.GetKeyDown(KeyCode.A))
        {
            // NextPanelInfo(-1);
            LanguageManager.ChangeLanguage(LanguageManager.Language.CZ);
        }

        // testing purpose
        if (Input.GetKeyDown(KeyCode.D))
        {
            // NextPanelInfo(1);
            LanguageManager.ChangeLanguage(LanguageManager.Language.ENG);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            // NextPanelInfo(1);
            _camera.ReleaseCamera();
        }
        //////**/////////
    }

    private void Init()
    {
        if (_database.cars.Count > 0)
        {
            ChoiseNextCar(0);
        }
        NextMenuTab(0);
        LanguageManager.OnLanguageChange += delegate { NextMenuTab(0); };
    }

    private void ChoiseNextCar(int index)
    {
        bool isInModelInPool = false;

        if (_selectedModel != null)
            _selectedModel.gameObject.SetActive(false);
      

        CalculateIndex(index);
        _selectedCar = _database.cars[_index];

        // if model is in pool. Activate
        foreach (GameObject model in _pool)
        {
            CarModel cModel = model.GetComponent<CarModel>();
            
            if (cModel != null && cModel.GetID == _selectedCar.uid)
            {
                isInModelInPool = true;
                _selectedModel = model.GetComponent<CarModel>();
                _meshRenderer = _selectedModel.body.GetComponent<MeshRenderer>();
                model.gameObject.SetActive(true);
                break;
            }
        }

        if (!isInModelInPool)
        {
            if (_pool.Count > poolLimit)
            {
                Destroy(_pool[0].gameObject);
                _pool.Remove(_pool[0]);
            }

            CarModel carModel = Resources.Load<CarModel>($"{_baseResourcePath}/{_selectedCar.resourcesPath}");
            GameObject goModel = Instantiate(carModel.gameObject, _spownPoint);
            carModel = goModel.GetComponent<CarModel>();
            carModel?.SetId(_selectedCar.uid);
            goModel.SetActive(true);
            _selectedModel = carModel;
            _meshRenderer = _selectedModel.body.GetComponent<MeshRenderer>();
            _pool.Add(goModel);
        }

        if (_selectedCar != null)
        {
            _infoPanel.SetCarInformation(_selectedCar);
            OnCarChange.Invoke(_selectedCar);
        }
    }

    private void CalculateIndex(int counter)
    {
        _index += counter;
        if (_index > _database.cars.Count - 1)
            _index = 0;
       
        if(_index < 0)
            _index = _database.cars.Count - 1; 
    }

    private void CarColorChange (Color color)
    {
        _color = color;
        if (_meshRenderer != null)
        {
            _meshRenderer.material.color = color;
        }
    }

    private void CarColorSetDefaul()
    {
        if (_meshRenderer != null)
        {
            _meshRenderer.material.color = _selectedModel.GetDefaultColor;
        }
    }
    private void CarWheelsSetDefault()
    {
        if (_selectedModel != null)
        {
            SetWheels(_selectedModel.GetDefaultWheel, setDefault: true);
        }
    }
    private void CarMetalicValueChange(float value)
    {
        _meshRenderer.material.SetFloat("_Metallic", value);
    }

    private void OnPressScreenShotButton()
    {
        _mainCanvas.gameObject.SetActive(false);
        StartCoroutine("TakeScreenShot");
    }

    // Call from OnPressScreenShotButton
    private IEnumerator TakeScreenShot()
    {
        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot($"Screenshot{_selectedCar.carName}{_screenShotIndex}.png", _screenShotSize);
        _screenShotIndex++;
        _mainCanvas.gameObject.SetActive(true);
    }

    private void AutoPan()
    {
        _camera.AutoPan = _camera.AutoPan ? false : true;
        Sprite sprite = _camera.AutoPan ? Resources.Load<Sprite>($"Sprites/darkIcon_restart") : Resources.Load<Sprite>($"Sprites/icon_restart");
        _buttonAutopan.gameObject.GetComponent<Image>().sprite = sprite;
    }

    private void SetNewWheels(string identifikator)
    {
        var newWheels = SetWheels(identifikator);
        SetCameraLookToWheel(newWheels);
    }

    private void SetCameraLookToWheel(List<GameObject> newWheels)
    {
        if (newWheels.Count > 0)
        {
            _camera.SetTargetCamera = newWheels[0].transform;
            _camera.RotateCameraToWheel();
        }
    }

    private List<GameObject> SetWheels(string wheelIdentificator, bool setDefault = false)
    {
        List<Vector3> wheelsPos = new List<Vector3>();
        Transform[] allChildren = _selectedModel.GetComponentsInChildren<Transform>();
        List<GameObject> newWeels = new List<GameObject>();
        GameObject wheel = Resources.Load<GameObject>($"Wheels/{wheelIdentificator}");

        foreach (Transform child in allChildren)
        {
            if (child.gameObject.name.StartsWith("wheel"))
            {
                wheelsPos.Add(child.position);
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < 4; i++)
        {
            var newWheel = Instantiate(wheel, _selectedModel.transform);
            newWheel.transform.position = wheelsPos[i];
            if (i % 2 == 0)
            {
                // prefabs are not consistence.
                if(setDefault)
                    newWheel.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                else
                    newWheel.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                
            }

            newWheel.gameObject.name = $"wheel{i}";
            newWeels.Add(newWheel);
        }

        return newWeels;
    }

    private void OpenPanel()
    {
        if (_panel != null)
        {
            Animator animator = _panel.GetComponent<Animator>();
            if (animator != null)
            {
                bool isOpen = animator.GetBool("Open");
                animator.SetBool("Open", !isOpen);
            }

            _camera.ReleaseCamera();
        }
    }

    public void NextMenuTab(int direction)
    {
        state += direction;

        if ((int)state > Enum.GetNames(typeof(MenuStates)).Length)
            state = (MenuStates)Enum.GetNames(typeof(MenuStates)).Length;
       
        if((int)state <= 0)
            state = (MenuStates)1;
      
        switch (state)
        {
            case MenuStates.CarInfo:
                SetCarInfo();
                break;
            case MenuStates.Accesories:
                _wheelsPanel.SetActive(true);
                _text.text = LanguageManager.GetTextValue("Accesories");
                _infoPanel.gameObject.SetActive(false);
                break;
            default:
                break;
        }

        _camera.ReleaseCamera();
    }

    private void SetCarInfo()
    {
        _wheelsPanel.SetActive(false);
        _text.text = LanguageManager.GetTextValue("CarInfo");
        _infoPanel.gameObject.SetActive(true);
    }

    private void OnChangeLanguage()
    {
        if (LanguageManager.LanguageSetting == LanguageManager.Language.CZ)
            LanguageManager.ChangeLanguage(LanguageManager.Language.ENG);
        else
            LanguageManager.ChangeLanguage(LanguageManager.Language.CZ);
    }

    private void OnDisable()
    {
        _colorPicker.OnMetalValueChange -= CarMetalicValueChange;
        _colorPicker.OnColorPressUp -= CarColorSetDefaul;
        _colorPicker.OnColorOver -= CarColorChange;
        _colorPicker.OnColorPressed -= CarColorChange;
        _buttonRight.onClick.RemoveAllListeners();
        _buttonLeft.onClick.RemoveAllListeners();
        _buttonScreenShot.onClick.RemoveAllListeners();
        _buttonAutopan.onClick.RemoveAllListeners();
        _buttonAccesories.onClick.RemoveAllListeners();
        _buttonMenuRight.onClick.RemoveAllListeners();
        _buttonMenuLeft.onClick.RemoveAllListeners();
        _buttonLanguage.onClick.RemoveAllListeners();
    }

    enum MenuStates
    {
        CarInfo = 1,
        Accesories = 2
    }
}
