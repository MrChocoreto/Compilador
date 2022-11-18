using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analisis3 : MonoBehaviour
{
    [SerializeField] ControlTablas CT;
    List<string> Token, Tipo;
    int acumulado;
    public void Despasamientos()
    {
        acumulado = 0;
        Token = CT.RegresarListaTokens2();
        Tipo = CT.RegresarListaTipo();
        for (int i=0;i<Token.Count;i++)
        {
            if (Token[i]=="IDENTIFICADOR")
            {
                CT.agregarDes(i,acumulado);
                acumulado += ValorDespasamientos(Tipo[i]);
            }
        }
    }
    int ValorDespasamientos(string t)
    {
        int valor=0;
        switch (t)
        {
            case "int":
                valor = 4;
                break;
            case "float":
                valor = 8;
                break;
            case "string":
                valor = 256;
                break;
            case "char":
                valor = 1;
                break;
            case "bool":
                valor = 1;
                break;
        }
        return valor;
    }
}
