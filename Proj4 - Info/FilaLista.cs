using AgendaAlfabetica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class FilaLista<Tipo> : ListaSimples<Tipo>, IQueue<Tipo>
  where Tipo : IComparable<Tipo>
  {
    public FilaLista()
    {
    }

    public int Tamanho => base.QuantosNos;

    public bool EstaVazia => base.EstaVazia;

    public List<Tipo> Listar()
    {
      return base.Listar();
    }

    public Tipo Retirar()
    {
      if (EstaVazia)
        throw new Exception("Fila vazia! Não é possível desenfileirar.");
      return RemoverOPrimeiro();  // remove o 1o nó e retorna seu Info
    }

    public void Enfileirar(Tipo novoDado)
    {
      InserirAposFim(novoDado);
    }

    public Tipo OInicio()
    {
      if (EstaVazia)
        throw new Exception("Underflow da fila");
      return Primeiro.Info;
    }

    public Tipo OFim()
    {
      if (EstaVazia)
        throw new Exception("Underflow da fila");
      return Ultimo.Info;
    }
  }