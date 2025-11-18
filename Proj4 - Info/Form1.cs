//Mariana Marietti da Costa - 24140
//Rafaelly Maria Nascimento da Silva - 24153

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Proj4
{
    public partial class Form1 : Form
    {
        Arvore<Cidade> arvore = new Arvore<Cidade>();
        public Form1()
        {
            InitializeComponent();
        }

        private void tpCadastro_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LerArquivoDeCidades();
        }

        private void pnlArvore_Paint(object sender, PaintEventArgs e)
        {
            arvore.Desenhar(pnlArvore);
        }

        
        private void LerArquivoDeCaminhos()
        {
            // Caminho do arquivo (certifique-se que o arquivo .txt está na pasta do executável ou debug)
            // Se você for colar o texto direto no código para testar, use um StringReader, 
            // mas o ideal é ler do arquivo:
            if (!File.Exists("GrafoOnibusSaoPaulo.txt"))
            {
                MessageBox.Show("Arquivo de caminhos não encontrado!");
                return;
            }

            var linhas = File.ReadAllLines("GrafoOnibusSaoPaulo.txt");

            foreach (var linha in linhas)
            {
                var dados = linha.Split(';');
                if (dados.Length == 3)
                {
                    string nomeOrigem = dados[0].Trim();
                    string nomeDestino = dados[1].Trim();
                    int distancia = int.Parse(dados[2].Trim());

                    // Criar as ligações (ida e volta)
                    Ligacao ida = new Ligacao(nomeOrigem, nomeDestino, distancia);
                    Ligacao volta = new Ligacao(nomeDestino, nomeOrigem, distancia);

                    // Buscar as cidades na árvore para adicionar as ligações
                    // Nota: Assume-se que as Cidades já foram carregadas do Cidades.dat para a 'arvore'

                    Cidade cidadeOrigem = new Cidade(nomeOrigem);
                    Cidade cidadeDestino = new Cidade(nomeDestino);

                    // O método Buscar da sua árvore deve retornar o objeto Cidade armazenado nela
                    // Se sua árvore retorna bool, você precisará de um método que retorne o dado (GetDado ou similar)
                    Cidade origemNaArvore = arvore.Buscar(cidadeOrigem);
                    Cidade destinoNaArvore = arvore.Buscar(cidadeDestino);

                    if (origemNaArvore != null && destinoNaArvore != null)
                    {
                        origemNaArvore.AdicionarLigacao(ida);
                        destinoNaArvore.AdicionarLigacao(volta);
                    }
                }
            }
        }

        private void btnBuscarCaminho_Click(object sender, EventArgs e)
        {
            if (cbxCidadeOrigem.SelectedItem == null || cbxCidadeDestino.SelectedItem == null)
                return;

            string nomeOrigem = cbxCidadeOrigem.SelectedItem.ToString(); // Ajuste conforme seu ComboBox
            string nomeDestino = cbxCidadeDestino.SelectedItem.ToString();

            Cidade origem = arvore.Buscar(new Cidade(nomeOrigem));
            Cidade destino = arvore.Buscar(new Cidade(nomeDestino));

            if (origem == null || destino == null) return;

            var caminho = Dijkstra(origem, destino);

            // Exibir o resultado no DataGridView ou ListBox
            ExibirCaminho(caminho);
        }

        private List<Cidade> Dijkstra(Cidade origem, Cidade destino)
        {
            // Tabelas para controlar distâncias e caminhos
            var distancia = new Dictionary<string, int>();
            var anterior = new Dictionary<string, Cidade>();
            var visitados = new HashSet<string>();
            var filaPrioridade = new List<Cidade>(); // Usaremos uma lista simples como fila

            // Inicialização (na prática, percorre-se a árvore para pegar todas as cidades, 
            // mas podemos inicializar conforme encontramos ou usar Recursão na árvore para encher a lista)
            // Para simplificar, vamos supor que 'distancia' inicia vazia e assumimos infinito.

            distancia[origem.Nome] = 0;
            filaPrioridade.Add(origem);

            while (filaPrioridade.Count > 0)
            {
                // Simula PriorityQueue: pega o nó com menor distância
                filaPrioridade.Sort((c1, c2) =>
                {
                    int d1 = distancia.ContainsKey(c1.Nome) ? distancia[c1.Nome] : int.MaxValue;
                    int d2 = distancia.ContainsKey(c2.Nome) ? distancia[c2.Nome] : int.MaxValue;
                    return d1.CompareTo(d2);
                });

                Cidade atual = filaPrioridade[0];
                filaPrioridade.RemoveAt(0);

                if (visitados.Contains(atual.Nome)) continue;
                visitados.Add(atual.Nome);

                if (atual.Nome == destino.Nome) break; // Chegou

                // Percorrer as ligações da cidade atual
                // Como é ListaSimples, usamos o IniciarPercursoSequencial/PodePercorrer (do seu código)
                atual.Ligacoes.IniciarPercursoSequencial();
                while (atual.Ligacoes.PodePercorrer())
                {
                    Ligacao ligacao = atual.Ligacoes.Atual.Info; // Depende de como sua Lista expõe o atual

                    // Precisamos achar o objeto Cidade do destino dessa ligação na Árvore
                    Cidade vizinho = arvore.Buscar(new Cidade(ligacao.Destino));

                    if (vizinho != null && !visitados.Contains(vizinho.Nome))
                    {
                        int novaDist = distancia[atual.Nome] + ligacao.Distancia;
                        int distVizinho = distancia.ContainsKey(vizinho.Nome) ? distancia[vizinho.Nome] : int.MaxValue;

                        if (novaDist < distVizinho)
                        {
                            distancia[vizinho.Nome] = novaDist;
                            anterior[vizinho.Nome] = atual;
                            filaPrioridade.Add(vizinho);
                        }
                    }
                }
            }

            // Reconstruir o caminho de volta (Destino -> Origem)
            var caminho = new List<Cidade>();
            Cidade passo = destino;
            while (passo != null && anterior.ContainsKey(passo.Nome))
            {
                caminho.Add(passo);
                passo = anterior[passo.Nome];
            }
            caminho.Add(origem);
            caminho.Reverse(); // Virar para ficar Origem -> Destino

            return caminho;
        }

        private void LerArquivoDeCidades()
        {
            if (!File.Exists("Cidades Sao Paulo.dat"))
            {
                MessageBox.Show("Arquivo de Cidades não encontrado!");
                return;
            }

            using (FileStream arq = new FileStream("Cidades Sao Paulo.dat", FileMode.Open))
            using (BinaryReader leitor = new BinaryReader(arq))
            {
                while (leitor.BaseStream.Position < leitor.BaseStream.Length)
                {
                    Cidade cidade = new Cidade();
                    // O método LerRegistro já existe na sua classe Cidade.cs, vamos usá-lo
                    cidade.LerRegistro(leitor, leitor.BaseStream.Position);

                    // Insere na árvore (Assumindo que sua classe Arvore tem o método Inserir)
                    arvore.Inserir(cidade);
                }
            }
        }

        // No Form1.cs

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            GravarCidadesECaminhos();
        }

        private void GravarCidadesECaminhos()
        {
            // Prepara os escritores
            using (FileStream fsCidades = new FileStream("Cidades Sao Paulo.dat", FileMode.Create))
            using (BinaryWriter bwCidades = new BinaryWriter(fsCidades))
            using (StreamWriter swCaminhos = new StreamWriter("GrafoOnibusSaoPaulo.txt"))
            {
                // Precisamos de um método na Árvore que aceite uma Action (Delegate) 
                // para processar cada cidade enquanto percorre a árvore.
                // Se sua árvore não tem isso, você terá que criar ou retornar uma List<Cidade> em ordem.

                // Exemplo supondo que você tenha arvore.PercorrerEmOrdem(Action<Cidade> acao):
                /*
                arvore.PercorrerEmOrdem(cidade => 
                {
                    // 1. Gravar Cidade no Binário
                    cidade.GravarRegistro(bwCidades);

                    // 2. Gravar Caminhos (Ligações) dessa cidade no Texto
                    // Percorre a lista ligada da cidade
                    cidade.Ligacoes.IniciarPercursoSequencial();
                    while (cidade.Ligacoes.PodePercorrer())
                    {
                        Ligacao lig = cidade.Ligacoes.Atual.Info;
                        // Grava apenas num sentido para não duplicar linhas (Opcional, ou grava tudo)
                        // Formato: Origem;Destino;Distancia
                        swCaminhos.WriteLine($"{cidade.Nome.Trim()};{lig.Destino.Trim()};{lig.Distancia}");
                    }
                });
                */

                // OBS: Se não tiver o método com Action, recupere a lista e use foreach:
                // var listaCidades = arvore.ListarEmOrdem(); 
                // foreach (var c in listaCidades) { ... logica acima ... }
            }
        }
    }
}
