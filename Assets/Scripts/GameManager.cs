using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int time = 30;
    public int difficulty = 1;    
    public AudioClip[] audios;
    [SerializeField] int score;
    public GameObject pausePanel;
    public GameObject personajePanel;

    [SerializeField] private int idUsuario;
    [SerializeField] private string nombreUsuario;
    [SerializeField] private string correo;
    [SerializeField] private string dni;
    [SerializeField] private int puntaje;
    [SerializeField] private int idPersonaje;

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
        if (idPersonaje == 0)
        {
            personajePanel.SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            //SceneManager.LoadScene("Menu");
            pausePanel.SetActive(true);
            Time.timeScale = 0;

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
            print("registró correctamente");
        }
        else
        {
            Debug.LogError("Error en la conección con la base de datos");
        }
    }




}
