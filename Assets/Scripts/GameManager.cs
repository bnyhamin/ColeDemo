using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int time = 30;
    public int difficulty = 1;    
    public AudioClip[] audios;
    [SerializeField] int score;
    public GameObject pausePanel;
    [SerializeField] private GameObject prefabPanelDialog;
    [SerializeField] private GameObject panelStatus;
    public GameObject personajePanel;
    public GameObject[] personajes;
    [SerializeField] private GameObject _camCanvas;

    [SerializeField] private int idUsuario;
    [SerializeField] private string nombreUsuario;
    [SerializeField] private string correo;
    [SerializeField] private string dni;
    [SerializeField] private int puntaje;
    [SerializeField] private int idPersonaje;
    [SerializeField] private bool _puntero_visible=true;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            //UIManager.Instance.UpdateUIScore(score);

        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //UIManager.Instance.UpdateUIScore(score);
        //Time.timeScale = 1;
        
        idUsuario = PlayerPrefs.GetInt("varglobal_idUsuario");
        nombreUsuario = PlayerPrefs.GetString("varglobal_nombreUsuario");
        puntaje = PlayerPrefs.GetInt("varglobal_puntaje");
        correo = PlayerPrefs.GetString("varglobal_correo");
        idPersonaje = PlayerPrefs.GetInt("varglobal_idPersonaje");

        //si no tiene personaje configurado abre panel para escoger personaje
        print("idUsuario:" + idUsuario);
        print("idPersonaje:" + idPersonaje);
        UIManager.Instance.UpdateUINombre(nombreUsuario);

        if (idPersonaje == 0)
        {
            personajePanel.SetActive(true);
            _camCanvas.SetActive(true);
            panelStatus.SetActive(false);
        }
        else
        {
            choisePersonaje();
            _camCanvas.SetActive(false);
            panelStatus.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("escape"))
        {
            //SceneManager.LoadScene("Menu");
            //pausePanel.SetActive(true);
            
            PunteroVisible(_puntero_visible);

        }
    }

    public void PunteroVisible(bool visible)
    {
        print("visible:"+visible);
        if (visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            _camCanvas.SetActive(true);
            panelStatus.SetActive(true);
            _puntero_visible = false; //al gatillar cambia a este valor para la siguiente vez
            cleanPersonaje();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
            _camCanvas.SetActive(false);
            panelStatus.SetActive(true);
            _puntero_visible = true;//al gatillar cambia a este valor para la siguiente vez
            choisePersonaje();
        }


    }

    public void startTime()
    {
        //GameObject.Find("Time").SetActive(true);
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        yield return new WaitForSeconds(1);
        /*while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            UIManager.Instance.UpdateUITime(time);
        }
        if (time <= 0)
        {
            UIManager.Instance.showUITime(false);
        }*/
    }

    public void Unpause()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Ajustes()
    {
        Debug.Log("Ajustes");
        personajePanel.SetActive(true);
        //_camCanvas.SetActive(true);
    }


    public void ChangePersonaje(int id_personaje)
    {
        StartCoroutine(Change_Personaje(id_personaje));
    }

    IEnumerator Change_Personaje(int id_personaje)
    {
        idPersonaje = id_personaje;
        WWW conneccion = new WWW("http://167.71.101.78/colegio3d/actualizar_personaje.php?id_usuario=" + idUsuario + "&id_personaje=" + idPersonaje);
        yield return (conneccion);
        if (conneccion.text == "201")
        {
            personajePanel.SetActive(false);
            choisePersonaje();
            print("registró correctamente");
        }
        else
        {
            Debug.LogError("Error en la conección con la base de datos");
        }
    }

    public void cleanPersonaje()
    {
        for(int i = 0; i < personajes.Length; i++)
        {
            personajes[i].SetActive(false);
        }
        //GameObject.Find("Man").SetActive(false);
        //GameObject.Find("Woman").SetActive(false);
    }

    
    private void choisePersonaje()
    {        
        cleanPersonaje();
        if (!_puntero_visible)
        {   
            Time.timeScale = 1;
            _camCanvas.SetActive(false);
            panelStatus.SetActive(true);
            _puntero_visible = true;
        }
            _camCanvas.SetActive(false);
        if (idPersonaje == 1) personajes[0].SetActive(true);
        if (idPersonaje == 2) personajes[1].SetActive(true);
        
    }

    public void showLivingInformation(bool fgvisible, string text)
    {        
        string mensaje = string.Empty;
        if(text == "ColliderAula1")
        {
            mensaje = "Bienvenido al Aula de Historia!." +
                "                          " +
                "En este curso conocerás la Historia del Perú y del Mundo";
        }
        prefabPanelDialog.SetActive(fgvisible);
        UIManager.Instance.ChangeDialog(mensaje);
    }


    public void Logout()
    {
        Debug.Log("Logout");
        Time.timeScale = 1;
        SceneManager.LoadScene("SceneLogin");
    }




}
