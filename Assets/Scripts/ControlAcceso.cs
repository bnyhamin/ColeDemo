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
    [SerializeField] private int estado;
    [SerializeField] private GameObject panelPago;
    [SerializeField] private GameObject panelExpulsion;

    public bool sesionIniciada = false;
    [SerializeField] GameObject panelLogin;
    [SerializeField] GameObject panelRegistrar;
    [SerializeField] Text textError;

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

    public void cerrarPanel(string name)
    {
        if(name == "panelPago") panelRegistrar.SetActive(false);
        if (name == "panelExpulsion") panelExpulsion.SetActive(false);
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
            //textError.text = "Usuario o contraseña incorrecta";
            MostrarError("Usuario o contraseña incorrecta");
        }
        else
        {
            print("Error en la conección con la base de datos");
            //textError.text = "Error en la conección con la base de datos";
            MostrarError("Error en la conección con la base de datos");
        }
    }

    IEnumerator datos()
    {
        WWW conneccion = new WWW("http://167.71.101.78/colegio3d/datos.php?usuario=" + txtUsuario.text);
        yield return (conneccion);
        if (conneccion.text == "401")
        {
            print("Usuario incorrecto");
            //textError.text = "Usuario incorrecto";
            MostrarError("Usuario incorrecto");
        }
        else
        {
            print(conneccion.text);
            string[] nDatos = conneccion.text.Split("|");            
            if(nDatos.Length != 7)
            {
                print("Error en la conección");
                //textError.text = "Error en la conección";
                MostrarError("Error en la conección");
            }
            else
            {
                
                idUsuario = int.Parse(nDatos[0]);
                nombreUsuario = nDatos[1];
                correo = nDatos[2];
                puntaje = int.Parse(nDatos[3]);
                idPersonaje = int.Parse(nDatos[4]);
                fg_permiso = int.Parse(nDatos[5]);
                estado = int.Parse(nDatos[6]);
                print("idPersonaje:" + idPersonaje);
                sesionIniciada = true;
                PlayerPrefs.SetInt("varglobal_idUsuario", idUsuario);
                PlayerPrefs.SetString("varglobal_nombreUsuario", nombreUsuario);
                PlayerPrefs.SetInt("varglobal_puntaje", puntaje);
                PlayerPrefs.SetString("varglobal_correo", correo);
                PlayerPrefs.SetInt("varglobal_idPersonaje", idPersonaje);

                if (fg_permiso == 1 && estado == 1)
                {
                    
                    SceneManager.LoadScene("SceneGame");
                    print("Entro a SceneGame");
                }
                else
                {
                    if(fg_permiso == 0)
                    {
                        print("No tiene permiso, debe realizar pago");
                        panelPago.SetActive(true);
                        //textError.text = "No tiene permiso, debe realizar pago";
                        MostrarError("No tiene permiso, debe realizar pago");
                    }
                        
                    if (estado == 0)
                    {
                        print("No tiene permiso, no tiene permiso para entrar");
                        panelExpulsion.SetActive(true);
                        //textError.text = "No tiene permiso, no tiene permiso para entrar";
                        MostrarError("No tiene permiso, no tiene permiso para entrar");
                    }
                        
                    
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
            //textError.text = "Usuario ya existe";
            MostrarError("Usuario ya existe");
        }
        else if (conneccion.text == "401")
        {
            Debug.LogError("Error en la conección con la base de datos");
            panelPago.SetActive(true);
            //textError.text = "Error en la conección con la base de datos";
            MostrarError("Error en la conección con la base de datos");
        }
        else
        {//201
            if (conneccion.text.Contains("|"))
            {
                string[] nDatos = conneccion.text.Split("|");
                if (nDatos[0] == "201")
                {
                    print("registró correctamente [conneccion.text]:" + nDatos[1]);

                    idUsuario = int.Parse(nDatos[1]);
                    PlayerPrefs.SetInt("varglobal_idUsuario", idUsuario);
                    PlayerPrefs.SetString("varglobal_nombreUsuario", txtUsuario1.text);
                    PlayerPrefs.SetInt("varglobal_puntaje", 0);
                    PlayerPrefs.SetString("varglobal_correo", txtCorreo.text);
                    PlayerPrefs.SetInt("varglobal_idPersonaje", 0);

                    txtUsuario1.text = string.Empty;
                    txtCorreo.text = string.Empty;
                    txtDni.text = string.Empty;
                    txtClave1.text = string.Empty;
                    nombreUsuario = txtUsuario.text;
                    puntaje = int.Parse("0");
                    sesionIniciada = true;

                    SceneManager.LoadScene("SceneGame");
                }
            }
            else
            {
                MostrarError("No obtiene resultado esperado "+ conneccion.text);
            }
            


            
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
            MostrarError("Error en la conección con la base de datos");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MostrarError(string mensaje)
    {
        print("Entra mensaje 1:" + mensaje);
                
        StartCoroutine(Mostrar_Error(mensaje));
    }

    IEnumerator Mostrar_Error(string mensaje)
    {
        print("Entra mensaje 2:" + mensaje);
        textError.text = mensaje;
        yield return new WaitForSeconds(3f);
        textError.text = "";

    }


}
