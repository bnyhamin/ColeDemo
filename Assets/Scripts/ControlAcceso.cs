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
    [SerializeField] private int idUsuario;
    [SerializeField] private string nombreUsuario;
    [SerializeField] private string correo;
    [SerializeField] private string dni;
    [SerializeField] private int puntaje;    
    [SerializeField] private int idPersonaje;
    [SerializeField] private int fg_permiso;
    [SerializeField] private GameObject panelPago;

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

    public void Registrarpago()
    {
        StartCoroutine(Registrar_pago());
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
            print(conneccion.text);
            string[] nDatos = conneccion.text.Split("|");            
            if(nDatos.Length != 6)
            {
                print("Error en la conección");
            }
            else
            {
                
                idUsuario = int.Parse(nDatos[0]);
                nombreUsuario = nDatos[1];
                correo = nDatos[2];
                puntaje = int.Parse(nDatos[3]);
                idPersonaje = int.Parse(nDatos[4]);
                fg_permiso = int.Parse(nDatos[5]);
                print("idPersonaje:" + idPersonaje);
                sesionIniciada = true;
                PlayerPrefs.SetInt("varglobal_idUsuario", idUsuario);
                PlayerPrefs.SetString("varglobal_nombreUsuario", nombreUsuario);
                PlayerPrefs.SetInt("varglobal_puntaje", puntaje);
                PlayerPrefs.SetString("varglobal_correo", correo);
                PlayerPrefs.SetInt("varglobal_idPersonaje", idPersonaje);

                if (fg_permiso == 1)
                {
                    
                    SceneManager.LoadScene("SceneGame");
                    print("Entro a SceneGame");
                }
                else
                {
                    print("No tiene permiso, debe realizar pago");
                    panelPago.SetActive(true);
                }

                
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
            puntaje = int.Parse("0");
            sesionIniciada = true;
            print("registró correctamente");

            PlayerPrefs.SetInt("varglobal_idUsuario", idUsuario);
            PlayerPrefs.SetString("varglobal_nombreUsuario", nombreUsuario);
            PlayerPrefs.SetInt("varglobal_puntaje", puntaje);
            PlayerPrefs.SetString("varglobal_correo", correo);
            PlayerPrefs.SetInt("varglobal_idPersonaje", idPersonaje);
        }
        else
        {
            Debug.LogError("Error en la conección con la base de datos");
            panelPago.SetActive(true);
        }
    }

    
    IEnumerator Registrar_pago()
    {
        print("idusuario:" + idUsuario);
        WWW conneccion = new WWW("http://167.71.101.78/colegio3d/registra_pago.php?id_usuario=" + idUsuario);
        yield return (conneccion);
        if (conneccion.text == "201")
        {
            
            print("pagó correctamente");

            panelPago.SetActive(false);
            SceneManager.LoadScene("SceneGame");
        }
        else
        {
            Debug.LogError("Error en la conección con la base de datos");
            panelPago.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
