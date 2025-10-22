using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class NoArvore<Dado> : IComparable<NoArvore<Dado>> 
             where Dado : IComparable<Dado>, IRegistro, new()
{
  Dado info;           // informação armazenada neste nó da árvore
  private NoArvore<Dado> esq, dir;
  //int altura;

  public NoArvore(Dado informacao)
  {
    info = informacao;  
    esq = dir = null;
  }

  public NoArvore(Dado dados, NoArvore<Dado> esquerdo, NoArvore<Dado> direito)
  {
    this.Info = dados;
    this.Esq = esquerdo;
    this.Dir = direito;
  }

  public Dado Info 
  { 
    get => info; 
    set => info = value; 
  }
  public NoArvore<Dado> Esq { get => esq; set => esq = value; }
  public NoArvore<Dado> Dir { get => dir; set => dir = value; }


  public int CompareTo(NoArvore<Dado> other)
  {
    throw new NotImplementedException();
  }
}

