using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ControlAcceso : MonoBehaviour
{
    public InputField txtUsuario;
    public InputField txtClave;
    public InputField txtUsuario1;
    public InputField txtClave1;
    public InputField txtCorreo;
    public InputField txtDni;
    public string nombreUsuario;
    public string correo;    
    public string dni;    
    public string puntaje;
    public int idUsuario;
    public bool sesionIniciada = false;
    [SerializeField] GameObject panelLogin;
    [SerializeField] GameObject panelRegistrar;

    public static ControlAcceso singleton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void iniciarSesion()
    {
        StartCoroutine(Login());
        
    }

    public void RegistrarUsuario()
    {
        StartCoroutine(Registrar());

    }

    public void BotonPanelActive(string namePanel)
    {
        
        Debug.Log(namePanel);        
        panelLogin.SetActive(namePanel == "PanelLogin");
        panelRegistrar.SetActive(namePanel == "PanelRegistrar");

    }

    IEnumerator Login()
    {
        WWW conneccion = new WWW("http://167.71.101.78/colegio3d/login.php?usuario=" + txtUsuario.text + "&clave="+ txtClave.text);
        yield return (conneccion);
        if(conneccion.text == "200")
        {
            print("El usuario si existe");
            StartCoroutine(datos());
        }
        else if (conneccion.text == "401")
        {
            print("Usuario o contraseña incorrecta");
        }
        else
        {
            print("Error en la conección con la base de datos");
        }
    }

    IEnumerator datos()
    {
        WWW conneccion = new WWW("http://167.71.101.78/colegio3d/datos.php?usuario=" + txtUsuario.text);
        yield return (conneccion);
        if (conneccion.text == "401")
        {
            print("Usuario incorrecto");
        }
        else
        {
            string[] nDatos = conneccion.text.Split("|");            
            if(nDatos.Length != 3)
            {
                print("Error en la conección");
            }
            else
            {
                nombreUsuario = nDatos[0];
                correo = nDatos[1];
                puntaje = nDatos[2];
                sesionIniciada = true;
                PlayerPrefs.SetString("varglobal_nombreUsuario", nombreUsuario);
                PlayerPrefs.SetInt("varglobal_puntaje", int.Parse(puntaje));
                PlayerPrefs.SetString("varglobal_correo", correo);
                SceneManager.LoadScene("SceneLogueado");
            }
        }
       
    }

    IEnumerator Registrar()
    {
        
        WWW conneccion = new WWW("http://167.71.101.78/colegio3d/registro.php?usuario=" + txtUsuario1.text + "&correo=" + txtCorreo.text + "&dni=" + txtDni.text + "&clave=" + txtClave1.text);
        yield return (conneccion);
        if (conneccion.text == "402")
        {
            print("Usuario ya existe");
        }
        else if (conneccion.text == "201")
        {
            txtUsuario1.text = string.Empty;
            txtCorreo.text = string.Empty;
            txtDni.text = string.Empty;
            txtClave1.text = string.Empty;
            nombreUsuario = txtUsuario.text;
            puntaje = "0";
            sesionIniciada = true;
            print("registró correctamente");
        }
        else
        {
            Debug.LogError("Error en la conección con la base de datos");
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
