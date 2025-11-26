using AgendaAlfabetica;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Proj4
{
    public partial class Form1 : Form
    {
        // Árvore AVL de cidades
        private Arvore<Cidade> arvoreBuscaBinariaBalanceadaAVL = new Arvore<Cidade>();

        private const string arqCidades = "cidades.dat";
        private const string arqGrafosLigacoes = "GrafoOnibusSaoPaulo.txt";

        // Mapa de fundo (ajuste o recurso se o nome for diferente)
        private Image mapa = Properties.Resources.SaoPaulo_MesoMicroSemMunicip;

        private Cidade cidadeAtual = null;

        // rotaEncontrada fica aqui para quando você plugar o Dijkstra da classe Grafo
        private readonly List<string> rotaEncontrada = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        // ================== LOAD / FECHAR ==================

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LerArquivoDeCidades();
                LerArquivoDeCaminhos();

                AtualizarCombos();
                pnlArvore.Refresh();
                pbMapa.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // grava cidades e ligações usando nosso próprio método
                GravarDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erro ao salvar os dados: {ex.Message}\n" +
                    "O programa será encerrado sem salvar as últimas alterações.",
                    "Erro de Salvamento",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ================== ARQUIVOS ==================

        private void LerArquivoDeCidades()
        {
            if (!File.Exists(arqCidades))
            {
                MessageBox.Show("Arquivo de Cidades não encontrado!");
                return;
            }

            // Árvore lê todas as cidades do arquivo binário
            arvoreBuscaBinariaBalanceadaAVL.LerArquivoDeRegistros(arqCidades);
        }

        private void LerArquivoDeCaminhos()
        {
            if (!File.Exists(arqGrafosLigacoes))
            {
                MessageBox.Show("Arquivo de Caminhos não encontrado!");
                return;
            }

            string[] linhas = File.ReadAllLines(arqGrafosLigacoes);

            foreach (string linha in linhas)
            {
                if (string.IsNullOrWhiteSpace(linha))
                    continue;

                string[] partes = linha.Split(';');

                if (partes.Length < 3)
                    continue;

                string nomeOrigem = partes[0].Trim();
                string nomeDestino = partes[1].Trim();
                if (!int.TryParse(partes[2].Trim(), out int distancia))
                    continue;

                // cria objetos Ligacao (ida e volta)
                Ligacao ida = new Ligacao(nomeOrigem, nomeDestino, distancia);
                Ligacao volta = new Ligacao(nomeDestino, nomeOrigem, distancia);

                // origem
                Cidade cidadeOrigemChave = new Cidade(nomeOrigem);
                if (arvoreBuscaBinariaBalanceadaAVL.Existe(cidadeOrigemChave))
                {
                    arvoreBuscaBinariaBalanceadaAVL.Atual.Info
                        .Ligacoes.InserirAposFim(ida);
                }

                // destino
                Cidade cidadeDestinoChave = new Cidade(nomeDestino);
                if (arvoreBuscaBinariaBalanceadaAVL.Existe(cidadeDestinoChave))
                {
                    arvoreBuscaBinariaBalanceadaAVL.Atual.Info
                        .Ligacoes.InserirAposFim(volta);
                }
            }
        }

        /// <summary>
        /// Grava cidades em binário (cidades.dat) e ligações em texto (GrafoOnibusSaoPaulo.txt)
        /// Usando o padrão de Cidade.GravarRegistro e ListaSimples.
        /// </summary>
        private void GravarDados()
        {
            List<Cidade> lista = new List<Cidade>();
            arvoreBuscaBinariaBalanceadaAVL.VisitarEmOrdem(lista);

            // Grava cidades
            using (FileStream fs = new FileStream(arqCidades, FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                foreach (Cidade cidade in lista)
                {
                    // grava todos os registros, inclusive possíveis excluídos
                    // (o campo Excluido não faz parte do arquivo)
                    cidade.GravarRegistro(bw);
                }
            }

            // Grava ligações (texto)
            using (StreamWriter sw = new StreamWriter(arqGrafosLigacoes))
            {
                foreach (Cidade cidade in lista.Where(c => !c.Excluido))
                {
                    cidade.Ligacoes.IniciarPercursoSequencial();
                    while (cidade.Ligacoes.PodePercorrer())
                    {
                        Ligacao lig = cidade.Ligacoes.Atual.Info;
                        sw.WriteLine($"{cidade.Nome.Trim()};{lig.Destino.Trim()};{lig.Distancia}");
                    }
                }
            }
        }

        // ================== CIDADES ==================

        private void txtNomeCidade_Leave(object sender, EventArgs e)
        {
            string nomeBuscado = txtNomeCidade.Text.Trim();

            if (string.IsNullOrWhiteSpace(nomeBuscado))
                return;

            Cidade chave = new Cidade(nomeBuscado);

            if (!arvoreBuscaBinariaBalanceadaAVL.Existe(chave))
            {
                // cidade não existe -> preparar inclusão
                MessageBox.Show($"A cidade '{nomeBuscado}' não está cadastrada.");
                cidadeAtual = null;
                udX.Value = 0;
                udY.Value = 0;
                dgvLigacoes.Rows.Clear();
            }
            else
            {
                // cidade existe -> carregar dados
                cidadeAtual = arvoreBuscaBinariaBalanceadaAVL.Atual.Info;

                udX.Value = (decimal)cidadeAtual.X;
                udY.Value = (decimal)cidadeAtual.Y;

                AtualizarGridLigacoesCidadeAtual();
            }

            pbMapa.Invalidate();
            pnlArvore.Refresh();
        }

        private void btnIncluirCidade_Click(object sender, EventArgs e)
        {
            string nome = txtNomeCidade.Text.Trim();
            double x = (double)udX.Value;
            double y = (double)udY.Value;

            if (string.IsNullOrWhiteSpace(nome))
            {
                MessageBox.Show("Digite o nome da cidade.");
                return;
            }

            // idealmente x e y já foram setados pelo clique no mapa
            Cidade novaCidade = new Cidade(nome, x, y);
            arvoreBuscaBinariaBalanceadaAVL.IncluirNovoDado(novaCidade);

            cidadeAtual = novaCidade;

            AtualizarCombos();
            AtualizarGridLigacoesCidadeAtual();

            pnlArvore.Refresh();
            pbMapa.Invalidate();

            MessageBox.Show($"Cidade '{nome}' incluída com sucesso!");
        }

        private void btnBuscarCidade_Click(object sender, EventArgs e)
        {
            string nome = txtNomeCidade.Text.Trim();

            if (string.IsNullOrWhiteSpace(nome))
                return;

            Cidade chave = new Cidade(nome);

            if (arvoreBuscaBinariaBalanceadaAVL.Existe(chave))
            {
                cidadeAtual = arvoreBuscaBinariaBalanceadaAVL.Atual.Info;

                udX.Value = (decimal)cidadeAtual.X;
                udY.Value = (decimal)cidadeAtual.Y;

                AtualizarGridLigacoesCidadeAtual();
            }
            else
            {
                MessageBox.Show($"Cidade '{nome}' não encontrada.");
                cidadeAtual = null;
                udX.Value = 0;
                udY.Value = 0;
                dgvLigacoes.Rows.Clear();
            }

            pbMapa.Invalidate();
            pnlArvore.Refresh();
        }

        private void btnAlterarCidade_Click(object sender, EventArgs e)
        {
            if (cidadeAtual == null || cidadeAtual.Excluido)
            {
                MessageBox.Show("Nenhuma cidade válida selecionada.");
                return;
            }

            // Atualiza as coordenadas
            cidadeAtual.X = (double)udX.Value;
            cidadeAtual.Y = (double)udY.Value;

            pbMapa.Invalidate();
            pnlArvore.Refresh();

            MessageBox.Show($"Coordenadas da cidade '{cidadeAtual.Nome}' alteradas com sucesso!");
        }

        private void btnExcluirCidade_Click(object sender, EventArgs e)
        {
            if (cidadeAtual == null)
                return;

            if (cidadeAtual.Ligacoes.QuantosNos > 0)
            {
                MessageBox.Show("Não é possível excluir uma cidade que possui caminhos cadastrados.");
                return;
            }

            cidadeAtual.Excluido = true;

            MessageBox.Show("Cidade excluída logicamente com sucesso!");

            cidadeAtual = null;
            txtNomeCidade.Clear();
            udX.Value = 0;
            udY.Value = 0;
            dgvLigacoes.Rows.Clear();

            AtualizarCombos();
            pbMapa.Invalidate();
            pnlArvore.Refresh();
        }

        // ================== CAMINHOS ==================

        private void AtualizarGridLigacoesCidadeAtual()
        {
            dgvLigacoes.Rows.Clear();

            if (cidadeAtual == null)
                return;

            foreach (Ligacao ligacao in cidadeAtual.Ligacoes.Listar())
            {
                dgvLigacoes.Rows.Add(
                    ligacao.Destino.Trim(),
                    ligacao.Distancia
                );
            }

            if (dgvLigacoes.Rows.Count > 0)
                dgvLigacoes.CurrentCell = dgvLigacoes.Rows[0].Cells[0];
        }

        private void btnIncluirCaminho_Click(object sender, EventArgs e)
        {
            if (cidadeAtual == null || cidadeAtual.Excluido)
                return;

            string nomeDestino = txtNovoDestino.Text.Trim();
            int distancia = (int)numericUpDown1.Value;

            if (string.IsNullOrWhiteSpace(nomeDestino))
            {
                MessageBox.Show("Digite o nome da cidade de destino.");
                return;
            }

            Cidade cidadeDestinoChave = new Cidade(nomeDestino);
            if (!arvoreBuscaBinariaBalanceadaAVL.Existe(cidadeDestinoChave) ||
                arvoreBuscaBinariaBalanceadaAVL.Atual.Info.Excluido)
            {
                MessageBox.Show("Cidade de destino não existe ou está excluída.");
                return;
            }

            Cidade cidadeDestino = arvoreBuscaBinariaBalanceadaAVL.Atual.Info;

            // evita auto-ligação
            if (cidadeDestino.Nome == cidadeAtual.Nome)
            {
                MessageBox.Show("Origem e destino não podem ser a mesma cidade.");
                return;
            }

            // inclusão bidirecional
            Ligacao ida = new Ligacao(cidadeAtual.Nome, cidadeDestino.Nome, distancia);
            Ligacao volta = new Ligacao(cidadeDestino.Nome, cidadeAtual.Nome, distancia);

            cidadeAtual.Ligacoes.InserirAposFim(ida);
            cidadeDestino.Ligacoes.InserirAposFim(volta);

            AtualizarGridLigacoesCidadeAtual();
            pbMapa.Invalidate();
            pnlArvore.Refresh();
        }

        private void btnExcluirCaminho_Click(object sender, EventArgs e)
        {
            if (cidadeAtual == null)
                return;

            var linha = dgvLigacoes.CurrentRow;
            if (linha == null)
                return;

            string nomeDestino = linha.Cells[0].Value.ToString().Trim();
            int distancia = Convert.ToInt32(linha.Cells[1].Value);

            // remove na origem
            Ligacao ligIda = new Ligacao(cidadeAtual.Nome, nomeDestino, distancia);
            cidadeAtual.Ligacoes.RemoverDado(ligIda);

            // remove na cidade de destino
            Cidade destinoChave = new Cidade(nomeDestino);
            if (arvoreBuscaBinariaBalanceadaAVL.Existe(destinoChave))
            {
                Cidade destino = arvoreBuscaBinariaBalanceadaAVL.Atual.Info;
                Ligacao ligVolta = new Ligacao(nomeDestino, cidadeAtual.Nome, distancia);
                destino.Ligacoes.RemoverDado(ligVolta);
            }

            AtualizarGridLigacoesCidadeAtual();
            pbMapa.Invalidate();
            pnlArvore.Refresh();
        }

        // ================== COMBOS / BUSCAS AUXILIARES ==================

        private void AtualizarCombos()
        {
            cbxCidadeDestino.Items.Clear();

            List<Cidade> lista = new List<Cidade>();
            arvoreBuscaBinariaBalanceadaAVL.VisitarEmOrdem(lista);

            foreach (var c in lista.Where(c => !c.Excluido))
                cbxCidadeDestino.Items.Add(c.Nome.Trim());
        }

        // ================== MAPA ==================

        /// <summary>
        /// Converte coordenadas normalizadas (0..1) em pixels do PictureBox.
        /// </summary>
        private PointF MapearCoordenada(double x, double y)
        {
            float posX = (float)(x * pbMapa.Width);
            float posY = (float)(y * pbMapa.Height);
            return new PointF(posX, posY);
        }


        /// <summary>
        /// Clique no mapa: define X e Y normalizados para incluir/alterar cidades.
        /// </summary>


        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            // desenha o mapa de fundo
            if (mapa != null)
            {
                e.Graphics.DrawImage(mapa, 0, 0, pbMapa.Width, pbMapa.Height);
            }

            List<Cidade> todasCidades = new List<Cidade>();
            arvoreBuscaBinariaBalanceadaAVL.VisitarEmOrdem(todasCidades);

            Dictionary<string, PointF> posicoes = new Dictionary<string, PointF>();

            // guarda posição das cidades
            foreach (Cidade c in todasCidades.Where(c => !c.Excluido))
            {
                posicoes[c.Nome] = MapearCoordenada(c.X, c.Y);
            }

            // desenha ligações (caminhos) como traços entre os pontos
            using (Pen penLigacao = new Pen(Color.LightGray, 1))
            {
                foreach (Cidade c in todasCidades.Where(c => !c.Excluido))
                {
                    if (!posicoes.ContainsKey(c.Nome))
                        continue;

                    PointF origem = posicoes[c.Nome];

                    c.Ligacoes.IniciarPercursoSequencial();
                    while (c.Ligacoes.PodePercorrer())
                    {
                        Ligacao lig = c.Ligacoes.Atual.Info;
                        string nomeDestino = lig.Destino.Trim();

                        if (!posicoes.ContainsKey(nomeDestino))
                            continue;

                        // desenha cada aresta só uma vez (A < B) pra não duplicar traços
                        if (string.Compare(c.Nome, nomeDestino, StringComparison.Ordinal) < 0)
                        {
                            PointF destino = posicoes[nomeDestino];
                            e.Graphics.DrawLine(penLigacao, origem, destino);
                        }
                    }
                }
            }

            // se tiver rotaEncontrada (no futuro), podemos destacar em outra cor
            if (rotaEncontrada.Count > 1)
            {
                using (Pen penRota = new Pen(Color.Red, 3))
                {
                    for (int i = 0; i < rotaEncontrada.Count - 1; i++)
                    {
                        string nomeA = rotaEncontrada[i];
                        string nomeB = rotaEncontrada[i + 1];

                        if (posicoes.ContainsKey(nomeA) && posicoes.ContainsKey(nomeB))
                        {
                            e.Graphics.DrawLine(penRota, posicoes[nomeA], posicoes[nomeB]);
                        }
                    }
                }
            }

            // desenha cidades (vértices)
            float raio = 5f;
            using (Font fonte = new Font("Arial", 8))
            {
                foreach (Cidade c in todasCidades.Where(c => !c.Excluido))
                {
                    if (!posicoes.ContainsKey(c.Nome))
                        continue;

                    PointF pos = posicoes[c.Nome];
                    Brush brush = Brushes.Blue;

                    if (cidadeAtual != null && c.Nome == cidadeAtual.Nome)
                        brush = Brushes.Red;

                    if (rotaEncontrada.Contains(c.Nome))
                        brush = Brushes.Green;

                    e.Graphics.FillEllipse(brush, pos.X - raio, pos.Y - raio, 2 * raio, 2 * raio);
                    e.Graphics.DrawString(c.Nome.Trim(), fonte, Brushes.Black, pos.X + raio, pos.Y - raio);
                }
            }
        }

        private void pnlArvore_Paint(object sender, PaintEventArgs e)
        {
            arvoreBuscaBinariaBalanceadaAVL.Desenhar(pnlArvore);
        }

        // ================== BUSCAR CAMINHO (DEPOIS VOCÊ PLUGA O GRAFO AQUI) ==================

        private void btnBuscarCaminho_Click(object sender, EventArgs e)
        {
            // Futuro:
            // - Montar Grafo a partir da árvore
            // - Chamar Grafo.Dijkstra(origem, destino)
            // - Preencher rotaEncontrada e dgvRotas e redesenhar o mapa

            MessageBox.Show("Dijkstra será implementado na classe Grafo. Aqui só vai chamar o Grafo depois.");
        }

        private void pbMapa_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (pbMapa.Width == 0 || pbMapa.Height == 0)
                return;

            // normaliza de pixels para 0..1
            double xNorm = (double)e.X / pbMapa.Width;
            double yNorm = (double)e.Y / pbMapa.Height;

            // atualiza os controles de coordenada
            udX.Value = (decimal)xNorm;
            udY.Value = (decimal)yNorm;

            // se quiser, você pode também já atualizar a cidadeAtual (em caso de alteração):
            if (cidadeAtual != null && !cidadeAtual.Excluido)
            {
                cidadeAtual.X = xNorm;
                cidadeAtual.Y = yNorm;
                pbMapa.Invalidate();
                pnlArvore.Refresh();
            }
        }
    }
}
