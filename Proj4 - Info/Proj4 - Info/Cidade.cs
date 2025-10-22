using AgendaAlfabetica;
using System;
using System.IO;
using System.Windows.Forms;

namespace Proj4
{
  public class Cidade : IComparable<Cidade>, IRegistro
  {
    string nome;
    double x, y;
    ListaSimples<Ligacao> ligacoes = new ListaSimples<Ligacao>();

    const int tamanhoNome = 25;
    const int tamanhoRegistro = tamanhoNome+ (2 * sizeof(double));

    public string Nome
    {
      get => nome;
      set => nome = value.PadRight(tamanhoNome, ' ').Substring(0, tamanhoNome);
    }

    public Cidade(string nome, double x, double y)
    {
      this.Nome = nome;
      this.x = x;
      this.y = y;
    }
    public override string ToString()
    {
      return Nome.TrimEnd() + " (" + ligacoes.QuantosNos + ")";
    }

    public Cidade()
    {
      this.Nome = "";
      this.x = 0;
      this.y = 0;
    }

    public Cidade(string nome)
    {
      this.Nome = nome;
    }

    public int CompareTo(Cidade outraCid)
    {
      return Nome.CompareTo(outraCid.Nome);
    }

    public int TamanhoRegistro { get => tamanhoRegistro; }
    public double X { get => x; set => x = value; }
    public double Y { get => y; set => y = value; }

    public void LerRegistro(BinaryReader arquivo, long qualRegistro)
    {

    }

    public void GravarRegistro(BinaryWriter arquivo)
    {

    }
  }

}
