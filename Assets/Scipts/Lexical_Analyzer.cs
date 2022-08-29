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
                WordAnalyzer(word, NumLine);

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

    void WordAnalyzer(string Word, int Line)
    {
        //int ASCII = default;
        string NewWord = default;

        // Recorro cada caracter de la palabra
        for (int i = 0; i < Word.Length; i++)
        {
            // corroboro que el primer caracter de la palabra sea una letra o un guion bajo
            if (Word[i] == 95 || Word[i] >= 65 && Word[i] <= 90 || Word[i] >= 97 && Word[i] <= 122)
            {
                NewWord = NewWord + Word[i];
            }
            else
            {
                Debug.Log(NewWord);
                Debug.Log(Word[i] + " es un simbolo, num. linea " + Line);
                NewWord = default;
            }
        }
        Debug.Log(NewWord);
    }

}
