//Mariana Marietti da Costa - 24140
//Rafaelly Maria Nascimento da Silva - 24153

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using AgendaAlfabetica;

namespace Proj4
{
    public partial class Form1 : Form
    {
        // A Árvore fica declarada aqui no Form
        Arvore<Cidade> arvore = new Arvore<Cidade>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LerArquivoDeCidades();
                LerArquivoDeCaminhos();
                AtualizarCombos();
                pnlArvore.Invalidate(); // Força o desenho
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao iniciar: " + ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            GravarDados();
        }

        // --- MÉTODOS DE ARQUIVO (Lógica trazida para o Form) ---

        private void LerArquivoDeCidades()
        {
            if (!File.Exists("Cidades Sao Paulo.dat")) return;

            using (FileStream arq = new FileStream("Cidades Sao Paulo.dat", FileMode.Open))
            using (BinaryReader leitor = new BinaryReader(arq))
            {
                while (leitor.BaseStream.Position < leitor.BaseStream.Length)
                {
                    Cidade cidade = new Cidade();
                    cidade.LerRegistro(leitor, 0); // Índice 0 pois é sequencial
                    arvore.IncluirNovoDado(cidade);
                }
            }
        }

        private void LerArquivoDeCaminhos()
        {
            if (!File.Exists("GrafoOnibusSaoPaulo.txt")) return;

            string[] linhas = File.ReadAllLines("GrafoOnibusSaoPaulo.txt");
            foreach (var linha in linhas)
            {
                var dados = linha.Split(';');
                if (dados.Length >= 3)
                {
                    string nomeOrig = dados[0].Trim();
                    string nomeDest = dados[1].Trim();
                    int dist = int.TryParse(dados[2].Trim(), out int d) ? d : 0;

                    // Busca manual na árvore
                    Cidade origem = BuscarCidadeNaArvore(nomeOrig);
                    Cidade destino = BuscarCidadeNaArvore(nomeDest);

                    if (origem != null && destino != null)
                    {
                        // Insere IDA e VOLTA
                        origem.Ligacoes.InserirAposFim(new Ligacao(nomeOrig, nomeDest, dist));
                        destino.Ligacoes.InserirAposFim(new Ligacao(nomeDest, nomeOrig, dist));
                    }
                }
            }
        }

        private void GravarDados()
        {
            using (FileStream fs = new FileStream("Cidades Sao Paulo.dat", FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs))
            using (StreamWriter sw = new StreamWriter("GrafoOnibusSaoPaulo.txt"))
            {
                // Usa o VisitarEmOrdem da própria árvore
                List<Cidade> lista = new List<Cidade>();
                arvore.VisitarEmOrdem(lista);

                foreach (var cidade in lista)
                {
                    // 1. Grava Cidade (Binário)
                    cidade.GravarRegistro(bw);

                    // 2. Grava Caminhos (Texto)
                    var no = cidade.Ligacoes.Primeiro; // Propriedade da ListaSimples
                    while (no != null)
                    {
                        Ligacao lig = no.Info;
                        sw.WriteLine($"{cidade.Nome.Trim()};{lig.Destino.Trim()};{lig.Distancia}");
                        no = no.Prox;
                    }
                }
            }
        }

        // --- MÉTODOS AUXILIARES ---

        // Método manual para buscar na árvore sem modificar a classe Arvore
        private Cidade BuscarCidadeNaArvore(string nome)
        {
            NoArvore<Cidade> atual = arvore.Raiz;
            Cidade chave = new Cidade(nome);

            while (atual != null)
            {
                int comp = chave.CompareTo(atual.Info);
                if (comp == 0) return atual.Info;
                if (comp < 0) atual = atual.Esq;
                else atual = atual.Dir;
            }
            return null;
        }

        private void AtualizarCombos()
        {
            //cbxCidadeOrigem.Items.Clear();
            cbxCidadeDestino.Items.Clear();

            List<Cidade> lista = new List<Cidade>();
            arvore.VisitarEmOrdem(lista);

            foreach (var c in lista)
            {
                //cbxCidadeOrigem.Items.Add(c.Nome.Trim());
                cbxCidadeDestino.Items.Add(c.Nome.Trim());
            }
        }

        // --- BOTÃO EXCLUIR CAMINHO ---
        private void btnExcluirCaminho_Click(object sender, EventArgs e)
        {
            if (dgvLigacoes.SelectedRows.Count == 0 || string.IsNullOrEmpty(txtNomeCidade.Text))
            {
                MessageBox.Show("Selecione uma cidade e um caminho no grid.");
                return;
            }

            try
            {
                string nomeOrig = txtNomeCidade.Text;
                // Ajuste os índices das células (0 e 1) conforme a ordem das colunas do seu Grid
                string nomeDest = dgvLigacoes.SelectedRows[0].Cells[0].Value.ToString();
                int dist = int.Parse(dgvLigacoes.SelectedRows[0].Cells[1].Value.ToString());

                Cidade origem = BuscarCidadeNaArvore(nomeOrig);
                Cidade destino = BuscarCidadeNaArvore(nomeDest);

                if (origem != null && destino != null)
                {
                    Ligacao ida = new Ligacao(nomeOrig, nomeDest, dist);
                    Ligacao volta = new Ligacao(nomeDest, nomeOrig, dist);

                    ReconstruirListaSemOItem(origem.Ligacoes, ida);
                    ReconstruirListaSemOItem(destino.Ligacoes, volta);

                    MessageBox.Show("Caminho excluído.");
                    pnlArvore.Invalidate();
                    // Lembre-se de atualizar o Grid aqui (ex: chamar o click do BuscarCidade)
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
        }

        // Método auxiliar para remover da ListaSimples sem mexer na classe dela
        private void ReconstruirListaSemOItem(ListaSimples<Ligacao> lista, Ligacao itemRemover)
        {
            if (lista.EstaVazia) return;

            // 1. Salva os itens que ficam
            List<Ligacao> temp = new List<Ligacao>();
            var atual = lista.Primeiro;
            while (atual != null)
            {
                // Se for diferente, mantém
                if (atual.Info.CompareTo(itemRemover) != 0)
                    temp.Add(atual.Info);
                atual = atual.Prox;
            }

            // 2. Esvazia a lista usando Reflection (para não violar regras de acesso)
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            typeof(ListaSimples<Ligacao>).GetField("primeiro", flags)?.SetValue(lista, null);
            typeof(ListaSimples<Ligacao>).GetField("ultimo", flags)?.SetValue(lista, null);
            typeof(ListaSimples<Ligacao>).GetField("quantosNos", flags)?.SetValue(lista, 0);

            // 3. Reinsere
            foreach (var lig in temp) lista.InserirAposFim(lig);
        }

        // --- BOTÃO BUSCAR (DIJKSTRA) ---
        private void btnBuscarCaminho_Click(object sender, EventArgs e)
        {
            if (cbxCidadeDestino.SelectedItem == null) return;

            string nomeOrig = txtNomeCidade.Text;
            string nomeDest = cbxCidadeDestino.SelectedItem.ToString();

            Cidade origem = BuscarCidadeNaArvore(nomeOrig);
            Cidade destino = BuscarCidadeNaArvore(nomeDest);

            if (origem == null || destino == null) return;

            // Algoritmo Dijkstra implementado aqui no Form
            var distancias = new Dictionary<string, int>();
            var anteriores = new Dictionary<string, Cidade>();
            var visitados = new HashSet<string>();
            var fila = new List<Cidade>();

            distancias[origem.Nome] = 0;
            fila.Add(origem);

            while (fila.Count > 0)
            {
                // Ordena (simula PriorityQueue)
                fila.Sort((a, b) => {
                    int da = distancias.ContainsKey(a.Nome) ? distancias[a.Nome] : int.MaxValue;
                    int db = distancias.ContainsKey(b.Nome) ? distancias[b.Nome] : int.MaxValue;
                    return da.CompareTo(db);
                });

                Cidade u = fila[0];
                fila.RemoveAt(0);

                if (u.Nome == destino.Nome) break;
                if (visitados.Contains(u.Nome)) continue;
                visitados.Add(u.Nome);

                var no = u.Ligacoes.Primeiro;
                while (no != null)
                {
                    Ligacao lig = no.Info;
                    Cidade v = BuscarCidadeNaArvore(lig.Destino);

                    if (v != null && !visitados.Contains(v.Nome))
                    {
                        int alt = distancias[u.Nome] + lig.Distancia;
                        int distV = distancias.ContainsKey(v.Nome) ? distancias[v.Nome] : int.MaxValue;

                        if (alt < distV)
                        {
                            distancias[v.Nome] = alt;
                            anteriores[v.Nome] = u;
                            if (!fila.Contains(v)) fila.Add(v);
                        }
                    }
                    no = no.Prox;
                }
            }

            // Exibir Rota
            dgvRotas.Rows.Clear();
            if (anteriores.ContainsKey(destino.Nome) || origem == destino)
            {
                var caminho = new List<Cidade>();
                Cidade atual = destino;
                while (atual != null)
                {
                    caminho.Add(atual);
                    if (atual == origem) break;
                    anteriores.TryGetValue(atual.Nome, out atual);
                }
                caminho.Reverse();

                int total = 0;
                for (int i = 0; i < caminho.Count - 1; i++)
                {
                    int d = 0;
                    var no = caminho[i].Ligacoes.Primeiro;
                    while (no != null)
                    {
                        if (no.Info.Destino == caminho[i + 1].Nome)
                        {
                            d = no.Info.Distancia;
                            break;
                        }
                        no = no.Prox;
                    }
                    total += d;
                    dgvRotas.Rows.Add(caminho[i].Nome, caminho[i + 1].Nome, d);
                }
                lbDistanciaTotal.Text = total.ToString();
            }
            else
            {
                MessageBox.Show("Caminho não encontrado.");
                lbDistanciaTotal.Text = "0";
            }
        }

        // --- PINTAR MAPA ---
        private void pnlArvore_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            List<Cidade> lista = new List<Cidade>();
            arvore.VisitarEmOrdem(lista);

            // Linhas
            foreach (var c in lista)
            {
                int x1 = (int)(c.X * pnlArvore.Width);
                int y1 = (int)(c.Y * pnlArvore.Height);

                var no = c.Ligacoes.Primeiro;
                while (no != null)
                {
                    Cidade dest = BuscarCidadeNaArvore(no.Info.Destino);
                    if (dest != null)
                    {
                        int x2 = (int)(dest.X * pnlArvore.Width);
                        int y2 = (int)(dest.Y * pnlArvore.Height);
                        g.DrawLine(Pens.Gray, x1, y1, x2, y2);
                    }
                    no = no.Prox;
                }
            }

            // Bolinhas
            foreach (var c in lista)
            {
                int x = (int)(c.X * pnlArvore.Width);
                int y = (int)(c.Y * pnlArvore.Height);

                // Desenha a bolinha
                g.FillEllipse(Brushes.Black, x - 5, y - 5, 10, 10);

                // Desenha o nome
                g.DrawString(c.Nome.Trim(), this.Font, Brushes.Black, x + 6, y - 6);
            }
        }
    }
}