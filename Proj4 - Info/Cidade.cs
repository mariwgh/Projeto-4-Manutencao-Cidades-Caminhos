//Mariana Marietti da Costa - 24140
//Rafaelly Maria Nascimento da Silva - 24153

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
        const int tamanhoRegistro = tamanhoNome + (2 * sizeof(double));

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

        public void LerRegistro(System.IO.BinaryReader arquivo, long qualRegistro)
        {
            // Posiciona o ponteiro (se necessário, mas na leitura sequencial não precisa do Seek se ler em ordem)
            // arquivo.BaseStream.Seek(qualRegistro, SeekOrigin.Begin); 

            // Lê o nome (string fixa de 25 bytes ou chars)
            // Nota: Depende de como foi gravado. Se foi com BinaryWriter.Write(string), ele tem um prefixo de tamanho.
            // Se for bytes fixos, precisa ler bytes e converter. Vamos assumir o padrão .NET por enquanto:
            this.Nome = arquivo.ReadString();
            this.X = arquivo.ReadDouble();
            this.Y = arquivo.ReadDouble();
        }

        public void GravarRegistro(BinaryWriter arquivo) {}


        public ListaSimples<Ligacao> Ligacoes
        {
            get => ligacoes;
        }

        public void AdicionarLigacao(Ligacao novaLigacao)
        {
            ligacoes.InserirAposFim(novaLigacao);
        }
    }

}
