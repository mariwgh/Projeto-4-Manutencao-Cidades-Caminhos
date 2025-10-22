using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class Funcionario : IComparable<Funcionario>, IRegistro
{
  public const int tamanhoNome = 30;

  private int matricula;
  private string nome;
  DateTime nascimento;
  int codigoSecao;
  int matriculaChefe;
  int quantosDependentes;
  double salario;
  bool afastado;

  const int tamanhoRegistro = sizeof(int) + // matricula
                              tamanhoNome + // nome
                              sizeof(Int64) + // nascimento
                              sizeof(int) + // codigoSecao
                              sizeof(int) + // matriculaChefe;
                              sizeof(int) + // quantosDependentes
                              sizeof(double) + // salario
                              sizeof(bool); // afastado

  public string Nome
  {
    get => nome;
    set => nome = value.PadRight(tamanhoNome, ' ').Substring(0, tamanhoNome);
  }

  public int Matricula 
  { 
    get => matricula; 
    set
    {
      if (value < 0)
        throw new ArgumentException("Matrícula deve ser maior que 0!");
      matricula = value;
    } 
  }
  public DateTime Nascimento { get => nascimento; set => nascimento = value; }
  public int CodigoSecao { get => codigoSecao; set => codigoSecao = value; }
  public int MatriculaChefe { get => matriculaChefe; set => matriculaChefe = value; }
  public int QuantosDependentes { get => quantosDependentes; set => quantosDependentes = value; }
  public double Salario { get => salario; set => salario = value; }
  public bool Afastado { get => afastado; set => afastado = value; }

  public Funcionario()    // implementa a restrição new()
  {
    Matricula = 0;
    Nome = "";
    CodigoSecao = 0;
    MatriculaChefe = 0;
    QuantosDependentes = 0;
    Salario = 0.0f;
    Afastado = false;
  }

  public Funcionario(int matricula, string nome, DateTime nascimento,
                     int codigoSecao, int matriculaChefe,
                     int quantosDependentes, double salario,
                     bool afastado)
  {
    this.Matricula = matricula;
    this.Nome = nome;
    this.Nascimento = nascimento;
    this.CodigoSecao = codigoSecao;
    this.MatriculaChefe = matriculaChefe;
    this.QuantosDependentes = quantosDependentes;
    this.Salario = salario;
    this.Afastado = afastado;
  }

  public Funcionario(int matricula)
  {
    this.matricula = matricula;
  }

  public int CompareTo(Funcionario outroFunc)
  {
    return matricula - outroFunc.matricula;
  }

  public override string ToString()
  {
    return Matricula + "";
  }

  public int TamanhoRegistro { get => tamanhoRegistro; }

  public void LerRegistro(BinaryReader arquivo, long qualRegistro)
  {
    if (arquivo != null)  // arquivo foi aberto pela aplicação
      try
      {
        long qtosBytesAPular = qualRegistro * TamanhoRegistro;

        arquivo.BaseStream.Seek(qtosBytesAPular, SeekOrigin.Begin);

        // lemos o 1o campo: 4 bytes --> matrícula
        this.Matricula = arquivo.ReadInt32();

        char[] umNome = new char[tamanhoNome];

        umNome = arquivo.ReadChars(tamanhoNome);
        string nomeLido = "";
        for (int i = 0; i < tamanhoNome; i++)
            nomeLido += umNome[i];
        Nome = nomeLido;

        Int64 dadoData = arquivo.ReadInt64();
        try
        {
          Nascimento = new DateTime(dadoData);
        }
        catch (Exception e1)
        {
          MessageBox.Show(e1.Message + " " + dadoData);
        }

        CodigoSecao = arquivo.ReadInt32();
        MatriculaChefe = arquivo.ReadInt32();
        QuantosDependentes = arquivo.ReadInt32();
        Salario = arquivo.ReadDouble();
        Afastado = arquivo.ReadBoolean();
      }
      catch (Exception e)
      {
        MessageBox.Show(e.Message);
      }
  }

  public void GravarRegistro(BinaryWriter arquivo)
  {
    if (arquivo != null)    // arquivo de saída foi aberto
    {
      arquivo.Write(Matricula);
      char[] umNome = new char[tamanhoNome];
      for (int i = 0; i < tamanhoNome; i++)
        umNome[i] = nome[i];
      arquivo.Write(umNome);
      arquivo.Write(Nascimento.Ticks);
      arquivo.Write(CodigoSecao);
      arquivo.Write(MatriculaChefe);
      arquivo.Write(QuantosDependentes);
      arquivo.Write(Salario);
      arquivo.Write(Afastado);
    }
  }


}
