using UnityEngine;
using TMPro;

public class LexicalAnalizer : MonoBehaviour
{

    #region Varibales
    [SerializeField] TextMeshProUGUI InputField;
    [SerializeField] ControlTablas CT;
    public SymbolsTable SymbolsTable;
    public Console_Errors console_Errors;
    [SerializeField, TextArea] private string Texto = default;
    private int NumLine = default,i=0;

    int ASCII = default;
    string palabra = default;
    string DebugMessage = default;

    #endregion

    private void FixedUpdate()
    {
        Texto = InputField.text;
    }

    #region Inicio_Analisis

    [ContextMenu("Iniciar_Analisis")]
    public void Analize()
    {
        //limpio las listas para realizar un nuevo analisis
        ClearAnalizers();
        // Inicializo el numero de linea siempre en 1
        NumLine = 1;
        for (i = 0; i < Texto.Length-1; i++)
        {
            //limpio la variable
            palabra = default;
            // Aqui solo le doy el valor ASCII de cada uno de los caracteres
            ASCII = Texto[i];

            // Reviso si hay un espacio, un salto de linea
            if (ASCII == 32 || ASCII == 10)
            {              
                if (ASCII == 10)
                {
                    // y aqui solo voy aumentando la variable cada que hay un salto de linea
                    NumLine++;
                }
            }
            else
            {
                //veo si es un identificador
                if (ASCII >= 97 && ASCII <= 122 || ASCII>=65 && ASCII<=90 || ASCII == 95)
                {
                    EsIdentificador(NumLine);
                }
                // veo si es numero
                else if (ASCII >= 48 && ASCII <= 57)
                {
                    esNumero(NumLine);
                }else if (ASCII==34 || ASCII == 39)
                {
                    CadenasCaracter();
                }
                else
                {
                    Operadores(NumLine);
                }
            }
        }
        CT.AgregarMensaje("Notificacion","Se completo el analisis","");
    }


    #endregion


    #region Analizadores

    void EsIdentificador(int num_line)
    {
        int e;
        //conformo el identificador
        for (e=i; e < Texto.Length; e++)
        {
            ASCII = Texto[e];
            //voy agregando los caracteres a la palabra hasta que no se reconosca un caracter
            if (ASCII >= 97 && ASCII <= 122 || ASCII >= 65 && ASCII <= 90 || ASCII == 95 || ASCII >= 48 && ASCII <= 57)
            {
                palabra += Texto[e];
            }
            else
            {
                break;
            }
        }
        e--;
        //se da el avance e a i en los caracteres
        i = e;
        //Debug.Log("Identificador Encontrado: " + palabra);
        CT.ComprobarIdentificador(palabra,"IDENTIFICADOR","identificador",num_line);
        //SymbolsTable.Add_New_Token(palabra,"Identificador", num_line);
    }  
    
    void esNumero(int num_line)
    {
        DebugMessage = "ERROR Numero decimal Incompleto: " + palabra;
        int e;
        //forma el numero entero
        for (e = i; e < Texto.Length; e++)
        {
            ASCII = Texto[e];
            if (ASCII >= 48 && ASCII <= 57)
            {
                palabra += Texto[e];
            }
            else
            {
                break;
            }
        }
        //comprueba si es un numero decimal
        if (ASCII==46)
        {
            //agrego el punto decimal
            palabra += Texto[e];
            //compruebo que no sea el ultimo caracter
            if (e!=Texto.Length-1)
            {
                //compruebo que continue el numero decimal
                if (Texto[e + 1] >= 48 && Texto[e + 1] <= 57)
                {
                    //guardo el avance anterior
                    i = e;

                    for (e = i + 1; e < Texto.Length; e++)
                    {
                        ASCII = Texto[e];
                        if (ASCII >= 48 && ASCII <= 57)
                        {
                            palabra += Texto[e];
                        }
                        else
                        {
                            break;
                        }
                    }
                    e--;
                    //se da el avance de e a i en los caracteres
                    i = e;
                    CT.AgregarToken(palabra, "Decimal");
                    //SymbolsTable.Add_New_Token(palabra, "Decimal", num_line);
                }
                else
                {
                    //se da el avance de e a i en los caracteres
                    i = e;
                    //tira el error a consola
                    CT.AgregarMensaje("ERROR","Numero decimal incompleto: "+palabra,""+num_line);
                    //console_Errors.DebugConsole(num_line, DebugMessage);
                }
            }
            else
            {
                //se da el avance de e a i en los caracteres
                i = e;
                //tira el error a consola
                CT.AgregarMensaje("ERROR", "Numero decimal incompleto: " + palabra, "" + num_line);
                //console_Errors.DebugConsole(num_line, DebugMessage);
            }

        }
        else
        {
            e--;
            //se da el avance e a i en los caracteres
            i = e;
            CT.AgregarToken(palabra, "Entero");
            //SymbolsTable.Add_New_Token(palabra, "Entero", num_line);
        }
    }
    
    void Operadores(int num_line)
    {
        //verifica que operador es si no lo mando a otro metodo
        switch (ASCII)
        {
            case 42:
                //SymbolsTable.Add_New_Token(""+Texto[i], "Operador", num_line);
                CT.AgregarToken("" + Texto[i], "Multiplicacion");
                //Debug.Log("Multiplicacion: "+Texto[i]);
                break;
            case 43:
                //verifica si es el ultimo caracter y si es un incremento
                if (i!=Texto.Length-1)
                {
                    ASCII = Texto[i + 1];
                    if (ASCII==43)
                    {
                        i++;
                        CT.AgregarToken(Texto[i - 1] + "" + Texto[i], "Incremento");
                        //SymbolsTable.Add_New_Token(Texto[i - 1] + "" + Texto[i], "Incremento", num_line);
                        //Debug.Log("Incremento: " + Texto[i-1]+Texto[i]);
                    }
                    else
                    {
                        //Debug.Log("Suma: " + Texto[i]);
                        CT.AgregarToken("" + Texto[i], "Suma");
                        //SymbolsTable.Add_New_Token("" + Texto[i], "Suma", num_line);
                    }
                }
                else
                {
                    //Debug.Log("Suma: " + Texto[i]);
                    CT.AgregarToken("" + Texto[i], "Suma");
                    //SymbolsTable.Add_New_Token("" + Texto[i], "Suma", num_line);
                }
                break;
            case 45:
                if (i != Texto.Length - 1)
                {
                    ASCII = Texto[i + 1];
                    if (ASCII == 45)
                    {
                        i++;
                        CT.AgregarToken(Texto[i - 1] + "" + Texto[i], "Decremento");
                        SymbolsTable.Add_New_Token(Texto[i - 1] + "" + Texto[i], "Decremento", num_line);
                        //Debug.Log("Decremento: " + Texto[i - 1] + Texto[i]);
                    }
                    else
                    {
                        CT.AgregarToken("" + Texto[i], "Resta");
                        //Debug.Log("Resta: " + Texto[i]);
                        //SymbolsTable.Add_New_Token("" + Texto[i], "Resta", num_line);
                    }
                }
                else
                {
                    CT.AgregarToken("" + Texto[i], "Resta");
                    //Debug.Log("Resta: " + Texto[i]);
                    //SymbolsTable.Add_New_Token("" + Texto[i], "Resta", num_line);
                }
                break;
            case 47:
                if (i != Texto.Length - 1)
                {
                    ASCII = Texto[i + 1];
                    //Comprueba si es un comentario para mandarlo al metodo correspondiente
                    if (ASCII == 47||ASCII==42)
                    {
                        i++;
                        comentarios();
                    }
                    else
                    {
                        //Debug.Log("Divicion: " + Texto[i]);
                        CT.AgregarToken("" + Texto[i], "Divicion");
                        //SymbolsTable.Add_New_Token("" + Texto[i], "Divicion", num_line);
                    }
                }
                else
                {
                    //Debug.Log("Divicion: " + Texto[i]);
                    CT.AgregarToken("" + Texto[i], "Divicion");
                    //SymbolsTable.Add_New_Token("" + Texto[i], "Divicion", num_line);
                }
                break;
            default:
                Comparadores(num_line);
                break;
        }
    }
    
    void Comparadores(int num_line)
    {
        DebugMessage = "No esta completo el comparador: " + palabra;
        //Ve que tipo de comparador es si no pasa al siguiente filtro
        switch (ASCII)
        {
            case 61:
                //Comprueba si es el ultimo caracter
                if (i != Texto.Length - 1)
                {  
                    ASCII = Texto[i + 1];
                    if (ASCII == 61)
                    {
                        i++;
                        //es un ==
                        CT.AgregarToken(Texto[i - 1] + "" + Texto[i], "Comparacion");
                        //SymbolsTable.Add_New_Token("" + Texto[i - 1] +""+ Texto[i], "Comparador", num_line);
                        //Debug.Log("Comparacion: " + Texto[i - 1] + Texto[i]);
                    }
                    else
                    {
                        CT.AgregarToken("" + Texto[i], "Asignacion");
                        //SymbolsTable.Add_New_Token("" + Texto[i], "Asignacion", num_line);
                        //Debug.Log("Asignacion: " + Texto[i]);
                    }
                }
                else
                {
                    CT.AgregarToken("" + Texto[i], "Asignacion");
                    //SymbolsTable.Add_New_Token("" + Texto[i], "Asignacion", num_line);
                    //Debug.Log("Asignacion: " + Texto[i]);
                }
                break;
            case 33:
                if (i != Texto.Length - 1)
                {
                    ASCII = Texto[i + 1];
                    if (ASCII == 33)
                    {
                        i++;
                        CT.AgregarToken(Texto[i - 1] + "" + Texto[i], "Diferencia");
                        //SymbolsTable.Add_New_Token("" + Texto[i - 1] + "" + Texto[i], "Diferente_de", num_line);
                        //Debug.Log("Diferencia: " + Texto[i - 1] + Texto[i]);
                    }
                    else
                    {
                        CT.AgregarToken("" + Texto[i], "Negacion");
                        //SymbolsTable.Add_New_Token("" + Texto[i], "Negacion", num_line);
                        //Debug.Log("Negacion: " + Texto[i]);
                    }
                }
                else
                {
                    CT.AgregarToken("" + Texto[i], "Negacion");
                    //SymbolsTable.Add_New_Token("" + Texto[i], "Negacion", num_line);
                    //Debug.Log("Negacion: " + Texto[i]);
                }
                break;
            case 60:
                if (i != Texto.Length - 1)
                {
                    ASCII = Texto[i + 1];
                    if (ASCII == 61)
                    {
                        i++;
                        CT.AgregarToken(Texto[i - 1] + "" + Texto[i], "Menor o igual que");
                        SymbolsTable.Add_New_Token("" + Texto[i - 1] + "" + Texto[i], "Menor_Igual", num_line);
                        //Debug.Log("Menor o Igual que: " + Texto[i - 1] + Texto[i]);
                    }
                    else
                    {
                        CT.AgregarToken("" + Texto[i], "Menor que");
                        //SymbolsTable.Add_New_Token("" + Texto[i], "Menor_que", num_line);
                        //Debug.Log("Menor que: " + Texto[i]);
                    }
                }
                else
                {
                    CT.AgregarToken("" + Texto[i], "Menor que");
                    //SymbolsTable.Add_New_Token("" + Texto[i], "Menor_que", num_line);
                    //Debug.Log("Menor que: " + Texto[i]);
                }
                break;
            case 62:
                if (i != Texto.Length - 1)
                {
                    ASCII = Texto[i + 1];
                    if (ASCII == 61)
                    {
                        i++;
                        CT.AgregarToken(Texto[i - 1] + "" + Texto[i], "Mayor o igual que");
                        //SymbolsTable.Add_New_Token("" + Texto[i - 1] + "" + Texto[i], "Mayor_Igual", num_line);
                        //Debug.Log("Mayor o Igual que: " + Texto[i - 1] + Texto[i]);
                    }
                    else
                    {
                        CT.AgregarToken("" + Texto[i], "Mayor que");
                        //SymbolsTable.Add_New_Token("" + Texto[i], "Mayor_que", num_line);
                        //Debug.Log("Mayor que: " + Texto[i]);
                    }
                }
                else
                {
                    CT.AgregarToken("" + Texto[i], "Mayor que");
                    //SymbolsTable.Add_New_Token("" + Texto[i], "Mayor_que", num_line);
                    //Debug.Log("Mayor que: " + Texto[i]);
                }
                break;
            case 38:
                if (i != Texto.Length - 1)
                {
                    ASCII = Texto[i + 1];
                    if (ASCII == 38)
                    {
                        i++;
                        palabra = ""+ Texto[i - 1] + Texto[i];
                        CT.AgregarToken(palabra, "&&");
                        //SymbolsTable.Add_New_Token("" + Texto[i - 1] + "" + Texto[i], "And", num_line);
                        //es &&(y)
                    }
                    else
                    {
                        CT.AgregarMensaje("ERROR", "Error falta un &", ""+num_line);
                        //console_Errors.DebugConsole(num_line, DebugMessage);
                        //error falta un & 
                    }
                }
                else
                {
                    CT.AgregarMensaje("ERROR", "Error falta un &", "" + num_line);
                    //console_Errors.DebugConsole(num_line, DebugMessage);
                    //error falta un & 
                }
                break;

            case 124:
                if (i != Texto.Length - 1)
                {
                    ASCII = Texto[i + 1];
                    if (ASCII == 124)
                    {
                        i++;
                        palabra = "" + Texto[i - 1] + Texto[i];
                        CT.AgregarToken(palabra, "||");
                        //SymbolsTable.Add_New_Token("" + Texto[i - 1] + "" + Texto[i], "Or", num_line);
                        //es ||(o)
                    }
                    else
                    {
                        CT.AgregarMensaje("ERROR", "Error falta un |", "" + num_line);
                        //console_Errors.DebugConsole(num_line, DebugMessage);
                        //error falta un |
                    }
                }
                else
                {
                    CT.AgregarMensaje("ERROR", "Error falta un |", "" + num_line);
                    //console_Errors.DebugConsole(num_line, DebugMessage);
                    //error falta un |
                }
                break;
            default:
                SigosAdicionales(num_line);
                break;
        }
    }
    
    void SigosAdicionales(int num_line)
    {

        //int hola = Texto[i];
        DebugMessage = "ERROR-Sinbolo no reconocido o no valido: " + Texto[i] + Texto[i-1];
        //se  busca identificar si es signo valio si no manda error
        switch (ASCII)
        {
            case 40:
                CT.AgregarToken("" + Texto[i], "Parentesis abierto");
                //SymbolsTable.Add_New_Token("" + Texto[i], "Parentesis_abierto", num_line);
                break;
            case 41:
                CT.AgregarToken("" + Texto[i], "Parentesis cerrado");
                //SymbolsTable.Add_New_Token("" + Texto[i], "Parentesis_cerrado", num_line);
                break;
            case 91:
                CT.AgregarToken("" + Texto[i], "Corchete abre");
                //SymbolsTable.Add_New_Token("" + Texto[i], "Corchete_abierto", num_line);
                break;
            case 93:
                CT.AgregarToken("" + Texto[i], "Corchete cerrado");
                //SymbolsTable.Add_New_Token("" + Texto[i], "Corchete_cerrado", num_line);
                break;
            case 123:
                CT.AgregarToken("" + Texto[i], "Llave abierto");
                //SymbolsTable.Add_New_Token("" + Texto[i], "Llave_abierto", num_line);
                break;
            case 125:
                CT.AgregarToken("" + Texto[i], "Llave cerrado");
                //SymbolsTable.Add_New_Token("" + Texto[i], "Llave_cerrado", num_line);
                break;
            case 59:
                CT.AgregarToken("" + Texto[i], "PYC");
                //SymbolsTable.Add_New_Token("" + Texto[i], "PYC", num_line);
                break;
            default:
                CT.AgregarMensaje("ERROR","Caracter no reconocido: " + Texto[i], ""+num_line);
                //console_Errors.DebugConsole(num_line, DebugMessage);
                break;
        }
    }
    
    void comentarios()
    {
        DebugMessage = "ERROR: No se cerro el comentario linea ";
        //Identifica que tipo de comentario es
        if (ASCII==47)
        {
            int e;
            palabra += "/";
            //busco hasta encontrar un salto de linea
            for (e = i; e < Texto.Length; e++)
            {
                ASCII = Texto[e];
                if (ASCII == 10)
                {
                    NumLine++;
                    //Debug.Log("Salto de Linea");
                    break;
                }
                else
                {
                    palabra += Texto[e];
                }
            }
            CT.AgregarToken(palabra,"Comentario");
            //SymbolsTable.Add_New_Token(palabra, "comentario", NumLine);
            //Debug.Log("Comentario en contrado: "+palabra);
            i = e;
        }else if (ASCII==42)
        {
            int e;
            int numL = NumLine;
            bool Termino = false;
            palabra += "/";
            //busco hasta encontar un *
            for (e = i; e < Texto.Length; e++)
            {
                ASCII = Texto[e];
                if (ASCII == 10)
                {
                    NumLine++;
                    //Debug.Log("Salto de Linea");
                }
                if (ASCII==42)
                {
                    //conpruebo que si sea el cierre
                    if (e != Texto.Length - 1)
                    {
                        ASCII = Texto[e + 1];
                        if (ASCII == 47)
                        {

                            e++;
                            palabra += "*/";
                            Termino = true;
                            CT.AgregarToken(palabra, "Comentario");
                            //SymbolsTable.Add_New_Token(palabra, "comentario", numL);
                            //Debug.Log("Comentario en contrado: " + palabra);
                            break;
                        }
                    }
                }
                palabra += Texto[e];

            }
            i = e;
            if (!Termino)
            {
                CT.AgregarMensaje("ERROR", "No se cerro el comentario", ""+numL);
                //console_Errors.DebugConsole(numL, DebugMessage);
                //Debug.Log("ERROR: No se cerro el comentario linea: "+numL);
            }
        }
        
    }
    
    void CadenasCaracter()
    {
        
        palabra += Texto[i];
        i++;
        //veo si es una cadeno o un caracter
        if (ASCII==34)
        {
            DebugMessage = "ERROR - No se cerro la cadena de caracteres linea";
            int e, numL = NumLine;
            bool Termino = false;
            //busco hasta encontrar "
            for (e = i; e < Texto.Length; e++)
            {
                ASCII = Texto[e];
                if (ASCII == 10)
                {
                    NumLine++;
                    //Debug.Log("Salto de Linea");
                }
                if (ASCII == 34)
                {
                    palabra += Texto[e];
                    Termino = true;
                    CT.AgregarToken(palabra,"Cadena");
                    //SymbolsTable.Add_New_Token(palabra, "Cadena", NumLine);
                    //Debug.Log("Cadena de caracteres encontrado: " + palabra);
                    break;                   
                }
                palabra += Texto[e];
            }
            i = e;
            if (!Termino)
            {
                CT.AgregarMensaje("ERROR", "No se cerro la cadena de caracteres", ""+numL);
                //console_Errors.DebugConsole(numL, DebugMessage);
                //Debug.Log("ERROR - No se cerro la cadena de caracteres linea: " + numL);
            }
        }
        else if (ASCII==39)
        {
            DebugMessage = "ERROR - No se cerro el caracter";
            if (i != Texto.Length)
            {
                ASCII = Texto[i]; 
                if (ASCII == 39)
                {
                    palabra += Texto[i];
                    CT.AgregarToken(palabra,"Caracter");
                    //SymbolsTable.Add_New_Token(palabra, "Caracter", NumLine);
                    //Debug.Log("Caracter Encontrado: " + palabra);
                }
                else
                {
                    palabra += Texto[i];
                    i++;
                    if (i != Texto.Length)
                    {
                        ASCII = Texto[i];
                        if (ASCII == 39)
                        {
                            palabra += Texto[i];
                            CT.AgregarToken(palabra, "Caracter");
                            //SymbolsTable.Add_New_Token(palabra, "Caracter", NumLine);
                            //Debug.Log("Caracter Encontrado: " + palabra);
                        }
                        else
                        {
                            i--;
                            CT.AgregarMensaje("ERROR", "No se cerro el caracter ", ""+NumLine);
                            //console_Errors.DebugConsole(NumLine, DebugMessage);
                            //Debug.Log("ERROR - No se cerro el caracter linea: " + NumLine);
                        }
                    }
                    else
                    {
                        CT.AgregarMensaje("ERROR", "No se cerro el caracter ", "" + NumLine);
                        //console_Errors.DebugConsole(NumLine, DebugMessage);
                        //Debug.Log("ERROR - No se cerro el caracter linea: " + NumLine);
                    }
                }
            }
            else
            {
                CT.AgregarMensaje("ERROR", "No se cerro el caracter ", "" + NumLine);
                //console_Errors.DebugConsole(NumLine, DebugMessage);
                //Debug.Log("ERROR - No se cerro el caracter linea: " +NumLine);
            }

        }
    }

    public void ClearAnalizers()
    {
        SymbolsTable.Clean();
        console_Errors.Console_Clear();
    }

    public void Exit()
    {
        Application.Quit();
    }

    #endregion

}
