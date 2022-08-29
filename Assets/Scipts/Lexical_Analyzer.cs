using UnityEngine;

public class Lexical_Analyzer : MonoBehaviour
{
    [SerializeField, TextArea] private string Text = default;
    private int NumLine = default;


    [ContextMenu("Lexical_Analyzer")]
    void SpaceAnalyzer()
    {
        // Inicializo el numero de linea siempre en 1
        NumLine = 1;

        // Creo las variables donde almacenare cada palabra y donde comparare los ASCII
        int ASCII = default;
        string word = default;


        // Recorro cada letra de la frase para poder identificar cada palabra y saber
        // el numero de linea de cada linea
        for (int i = 0; i < Text.Length; i++)
        {
            // Aqui solo le doy el valor ASCII de cada uno de los caracteres
            ASCII = Text[i];

            // Reviso si hay un espacio, un salto de linea o si es el final de
            // la frase
            if (ASCII == 32 || ASCII == 10 || i == Text.Length - 1)
            {

                // si es cierto entonces es por que ya completamos una palabra
                // y por si acaso reviso que en el caso de que sea el final de
                // la frase acomplete la palabra
                if (i == Text.Length - 1)
                {
                    word = word + Text[Text.Length-1];
                }


                // le mando la palabra y el numero de linea al metodo
                TextAnalyzer(word, NumLine);

                // y vuelvo a vaciar al variable que almacena cada frase
                word = default;
            }
            else
            {
                // aqui solo añado una letra cada que recorro los caracteres
                word = word + Text[i];
            }
            if (ASCII == 10)
            {
                // y aqui solo voy aumentando la variable cada que hay un salto de linea
                NumLine++;
            }
        }
    }

    void TextAnalyzer(string Word, int Line)
    {
        //int ASCII = default;
        string NewWord = default;
        // Recorro cada caracter de la palabra
        for (int i = 0; i < Word.Length; i++)
        {
            NewWord = NewWord + Word[i];
            // corroboro que el primer caracter de la palabra sea una letra o un guion bajo
            if (i==0)
            {
                if (Word[i] == 95 || Word[i] >= 65 && Word[i] <= 90 || Word[i] >= 97 && Word[i] <= 122)
                {
                    Debug.Log(Word +" es una variable, num. linea "+Line);
                }
                
                if (Word[i] == 125)
                {
                    Debug.Log(Word[i] + " es una Llave cerrada, num. linea " + Line);
                    Word = Word.Remove(i);
                    goto Extra;
                }
            }
            else
            {
                if (Word[i] == 43)
                {
                    if (Word[i+1] == 43)
                    {
                        Debug.Log(Word[i] + "" + Word[i+1] + " es un Incrementador, num. linea " + Line);
                        Word = Word.Remove(i, 2);
                        i -= 1;
                        goto Extra;
                    }
                }

                if (Word[i] == 123)
                {
                    Debug.Log(Word[i] + " es una Llave abierta, num. linea " + Line);
                    goto Extra;
                }

                if (Word[i] == 125)
                {
                    Debug.Log("Hello World");
                    Debug.Log(Word[i] + " es una Llave cerrada, num. linea " + Line);
                    Word = Word.Remove(i);
                    Debug.Log(Word);
                    goto Extra;
                }

                if (Word[i] == 59)
                {
                    Debug.Log(Word[i] + " es un PyC, num. linea " + Line);
                    Word = Word.Remove(i);
                    goto Extra;
                }

                
            }
            Extra:;
        }
    }

}
