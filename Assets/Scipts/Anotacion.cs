using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anotacion : MonoBehaviour
{
    [SerializeField] List<string> Operadores,Salida;
    [SerializeField] List<int> ValorOperacion;
    [SerializeField] List<string> Lexemas;
    [SerializeField] List<float> LisResul;
    [SerializeField] float Resultado;
    public float HacerCalculo(List<string> Lex)
    {
        limpiar();
        Lexemas = Lex;
        for (int i=0;i<Lexemas.Count;i++)
        {
            if (Lexemas[i]=="+"|| Lexemas[i] == "-"|| Lexemas[i] == "*"|| Lexemas[i] == "/"|| Lexemas[i] == "^")
            {
                if (Operadores.Count>0)
                {
                    if (Operadores[Operadores.Count-1]!="(" && Operadores[Operadores.Count - 1] != ")")
                    {
                        if (ValorOperador(Lexemas[i])>ValorOperacion[ValorOperacion.Count-1])
                        {
                            Debug.Log("re" + Lexemas[i]);
                            Operadores.Add(Lexemas[i]);
                            ValorOperacion.Add(ValorOperador(Lexemas[i]));
                        }
                        else if (ValorOperador(Lexemas[i])== ValorOperacion[ValorOperacion.Count - 1])
                        {
                            Salida.Add(Operadores[Operadores.Count-1]);
                            Operadores[Operadores.Count - 1] = Lexemas[i];
                        }else if (ValorOperador(Lexemas[i]) < ValorOperacion[ValorOperacion.Count - 1])
                        {                              
                            for (int e=Operadores.Count-1; e>=0 && Operadores[e] != "(";e--)
                            {
                                Salida.Add(Operadores[e]);
                                Operadores.RemoveAt(e);
                                ValorOperacion.RemoveAt(e);
                            }
                            Operadores.Add(Lexemas[i]);
                            ValorOperacion.Add(ValorOperador(Lexemas[i]));
                        }
                    }
                    else
                    {                       
                        Operadores.Add(Lexemas[i]);
                        ValorOperacion.Add(ValorOperador(Lexemas[i]));
                    }
                }
                else
                {
                    Operadores.Add(Lexemas[i]);
                    ValorOperacion.Add(ValorOperador(Lexemas[i]));
                }
            }
            else if (Lexemas[i] == "(")
            {
                Operadores.Add("(");
                ValorOperacion.Add(3);
            }
            else if (Lexemas[i] == ")")
            {
                for (int e=Operadores.Count-1;Operadores[e]!="(" ;e--)
                {
                    Salida.Add(Operadores[e]);
                    Operadores.RemoveAt(e);
                    ValorOperacion.RemoveAt(e);
                }
                Operadores.RemoveAt(Operadores.Count-1);
                ValorOperacion.RemoveAt(ValorOperacion.Count-1);
            }
            else
            {
                Salida.Add(Lexemas[i]);
            }
        }
        for (int i= Operadores.Count-1; i>=0;i--)
        {
            Salida.Add(Operadores[i]);
        }
        for (int i=0;i<Salida.Count;i++)
        {
            if (Salida[i]=="-"|| Salida[i] == "+"|| Salida[i] == "*"||Salida[i] == "/"|| Salida[i] == "^")
            {
                CalcularOperacion(Salida[i]);
            }
            else
            {
                LisResul.Add(float.Parse(Salida[i]));
            }
        }
        Resultado = LisResul[0];
        return Resultado;
    }
    float CalcularOperacion(string ope)
    {
        float va = 0;
        switch (ope)
        {
            case "+":
                va = LisResul[LisResul.Count - 2] + LisResul[LisResul.Count - 1];
                LisResul.RemoveAt(LisResul.Count-1);
                LisResul[LisResul.Count - 1] = va;
                break;
            case "-":
                va = LisResul[LisResul.Count - 2] - LisResul[LisResul.Count - 1];
                LisResul.RemoveAt(LisResul.Count - 1);
                LisResul[LisResul.Count - 1] = va;
                break;
            case "*":
                va = LisResul[LisResul.Count - 1] * LisResul[LisResul.Count - 2];
                LisResul.RemoveAt(LisResul.Count - 1);
                LisResul[LisResul.Count - 1] = va;
                break;
            case "/":
                va = 1;
                va = LisResul[LisResul.Count - 2] / LisResul[LisResul.Count - 1];
                LisResul.RemoveAt(LisResul.Count - 1);
                LisResul[LisResul.Count - 1] = va;
                break;
            case "^":
                va =Mathf.Pow(LisResul[LisResul.Count - 2], LisResul[LisResul.Count - 1]);
                LisResul.RemoveAt(LisResul.Count - 1);
                LisResul[LisResul.Count - 1] = va;
                break;
        }
        return va;
    }
    int ValorOperador(string ope)
    {
        int va = 0;
        switch (ope)
        {
            case "+":
                va =0;
                break;
            case "-":
                va =0;
                break;
            case "*":
                va =1;
                break;
            case "/":
                va =1;
                break;
            case "^":
                va =2;
                break;
        }
        return va;
    }
    void limpiar()
    {
        Operadores = new List<string>();
        Salida = new List<string>();
        Lexemas= new List<string>();
        ValorOperacion = new List<int>();
        LisResul = new List<float>();
        Resultado = 0;
    }
}
