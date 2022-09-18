using UnityEngine;

public class LexicalAnalizer : MonoBehaviour
{

    #region Varibales
    public SymbolsTable SymbolsTable;
    [SerializeField, TextArea] private string Texto = default;
    private int NumLine = default,i=0;

    int ASCII = default;
    string palabra = default;


    #endregion

    
    #region Inicio_Analisis

    [ContextMenu("Iniciar_Analisis")]
    void ComenzarAnalisis()
    {
        // Inicializo el numero de linea siempre en 1
        NumLine = 1;
        for (i = 0; i < Texto.Length; i++)
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
                    esNumero();
                }else if (ASCII==34 || ASCII == 39)
                {
                    CadenasCaracter();
                }
                else
                {
                    Operadores();
                }
            }
        }
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
        SymbolsTable.Add_New_Token(palabra,"Identificador", num_line);
    }  
    
    void esNumero()
    {
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
                    Debug.Log("Numero decimal Encontrado: " + palabra);
                }
                else
                {
                    //se da el avance de e a i en los caracteres
                    i = e;
                    Debug.Log("ERROR Numero decimal Incompleto: " + palabra+" Linea "+NumLine);
                }
            }
            else
            {
                //se da el avance de e a i en los caracteres
                i = e;
                Debug.Log("ERROR Numero decimal Incompleto: " + palabra + " Linea " + NumLine);
            }

        }
        else
        {
            e--;
            //se da el avance e a i en los caracteres
            i = e;
            Debug.Log("Numero Entero Encontrado: " + palabra);
        }
    }
    
    void Operadores()
    {
        //verifica que operador es si no lo mando a otro metodo
        switch (ASCII)
        {
            case 42:
                Debug.Log("Multiplicacion: "+Texto[i]);
                break;
            case 43:
                //verifica si es el ultimo caracter y si es un incremento
                if (i!=Texto.Length-1)
                {
                    ASCII = Texto[i + 1];
                    if (ASCII==43)
                    {
                        i++;
                        Debug.Log("Incremento: " + Texto[i-1]+Texto[i]);
                    }
                    else
                    {
                        Debug.Log("Suma: " + Texto[i]);
                    }
                }
                else
                {
                    Debug.Log("Suma: " + Texto[i]);
                }
                break;
            case 45:
                if (i != Texto.Length - 1)
                {
                    ASCII = Texto[i + 1];
                    if (ASCII == 45)
                    {
                        i++;
                        Debug.Log("Decremento: " + Texto[i - 1] + Texto[i]);
                    }
                    else
                    {
                        Debug.Log("Resta: " + Texto[i]);
                    }
                }
                else
                {
                    Debug.Log("Resta: " + Texto[i]);
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
                        Debug.Log("Divicion: " + Texto[i]);
                    }
                }
                else
                {
                    Debug.Log("Divicion: " + Texto[i]);
                }
                break;
            default:
                Comparadores();
                break;
        }
    }
    
    void Comparadores()
    {
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
                        Debug.Log("Comparacion: " + Texto[i - 1] + Texto[i]);
                    }
                    else
                    {
                        Debug.Log("Asignacion: " + Texto[i]);
                    }
                }
                else
                {
                    Debug.Log("Asignacion: " + Texto[i]);
                }
                break;
            case 33:
                if (i != Texto.Length - 1)
                {
                    ASCII = Texto[i + 1];
                    if (ASCII == 33)
                    {
                        i++;
                        Debug.Log("Diferencia: " + Texto[i - 1] + Texto[i]);
                    }
                    else
                    {
                        Debug.Log("Negacion: " + Texto[i]);
                    }
                }
                else
                {
                    Debug.Log("Negacion: " + Texto[i]);
                }
                break;
            case 60:
                if (i != Texto.Length - 1)
                {
                    ASCII = Texto[i + 1];
                    if (ASCII == 61)
                    {
                        i++;
                        Debug.Log("Menor o Igual que: " + Texto[i - 1] + Texto[i]);
                    }
                    else
                    {
                        Debug.Log("Menor que: " + Texto[i]);
                    }
                }
                else
                {
                    Debug.Log("Menor que: " + Texto[i]);
                }
                break;
            case 62:
                if (i != Texto.Length - 1)
                {
                    ASCII = Texto[i + 1];
                    if (ASCII == 61)
                    {
                        i++;
                        Debug.Log("Mayor o Igual que: " + Texto[i - 1] + Texto[i]);
                    }
                    else
                    {
                        Debug.Log("Mayor que: " + Texto[i]);
                    }
                }
                else
                {
                    Debug.Log("Mayor que: " + Texto[i]);
                }
                break;
            default:
                SigosAdicionales();
                break;
        }
    }
    
    void SigosAdicionales()
    {
        //se  busca identificar si es signo valio si no manda error
        switch (ASCII)
        {
            case 40:
                Debug.Log("Parentesis abierto: " + Texto[i]);
                break;
            case 41:
                Debug.Log("Parentesis cerrado: " + Texto[i]);
                break;
            case 91:
                Debug.Log("Corchete abierto: " + Texto[i]);
                break;
            case 93:
                Debug.Log("Corchete cerrado: " + Texto[i]);
                break;
            case 123:
                Debug.Log("Llave abierto: " + Texto[i]);
                break;
            case 125:
                Debug.Log("Llave cerrado: " + Texto[i]);
                break;
            case 59:
                Debug.Log("Punto y coma: " + Texto[i]);
                break;
            default:
                Debug.LogError("ERROR-Sinbolo no reconocido o no valido: " + Texto[i]);
                break;
        }
    }
    
    void comentarios()
    {
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
                    Debug.Log("Salto de Linea");
                    break;
                }
                else
                {
                    palabra += Texto[e];
                }
            }
            Debug.Log("Comentario en contrado: "+palabra);
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
                    Debug.Log("Salto de Linea");
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
                            Debug.Log("Comentario en contrado: " + palabra);
                            break;
                        }
                    }
                }
                palabra += Texto[e];

            }
            i = e;
            if (!Termino)
            {
                Debug.Log("ERROR: No se cerro el comentario linea: "+numL);
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
            int e, numL = NumLine;
            bool Termino = false;
            //busco hasta encontrar "
            for (e = i; e < Texto.Length; e++)
            {
                ASCII = Texto[e];
                if (ASCII == 10)
                {
                    NumLine++;
                    Debug.Log("Salto de Linea");
                }
                if (ASCII == 34)
                {
                    palabra += Texto[e];
                    Termino = true;
                    Debug.Log("Cadena de caracteres encontrado: " + palabra);
                    break;                   
                }
                palabra += Texto[e];
            }
            i = e;
            if (!Termino)
            {
                Debug.Log("ERROR - No se cerro la cadena de caracteres linea: " + numL);
            }
        }
        else if (ASCII==39)
        {
            if (i != Texto.Length)
            {
                ASCII = Texto[i]; 
                if (ASCII == 39)
                {
                    palabra += Texto[i];
                    Debug.Log("Caracter Encontrado: " + palabra);
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
                            Debug.Log("Caracter Encontrado: " + palabra);
                        }
                        else
                        {
                            i--;
                            Debug.Log("ERROR - No se cerro el caracter linea: " + NumLine);
                        }
                    }
                    else
                    {
                        Debug.Log("ERROR - No se cerro el caracter linea: " + NumLine);
                    }
                }
            }
            else
            {
                Debug.Log("ERROR - No se cerro el caracter linea: " +NumLine);
            }

        }
    }

    #endregion

}
