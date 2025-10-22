namespace Proj4
{
  partial class Form1
  {
    /// <summary>
    /// Variável de designer necessária.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Limpar os recursos que estão sendo usados.
    /// </summary>
    /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Código gerado pelo Windows Form Designer

    /// <summary>
    /// Método necessário para suporte ao Designer - não modifique 
    /// o conteúdo deste método com o editor de código.
    /// </summary>
    private void InitializeComponent()
    {
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tpCadastro = new System.Windows.Forms.TabPage();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.lbDistanciaTotal = new System.Windows.Forms.Label();
      this.dgvRotas = new System.Windows.Forms.DataGridView();
      this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.btnBuscarCaminho = new System.Windows.Forms.Button();
      this.cbxCidadeDestino = new System.Windows.Forms.ComboBox();
      this.label6 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.btnExcluirCaminho = new System.Windows.Forms.Button();
      this.btnIncluirCaminho = new System.Windows.Forms.Button();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
      this.txtNovoDestino = new System.Windows.Forms.TextBox();
      this.dgvLigacoes = new System.Windows.Forms.DataGridView();
      this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.btnExcluirCidade = new System.Windows.Forms.Button();
      this.btnAlterarCidade = new System.Windows.Forms.Button();
      this.btnBuscarCidade = new System.Windows.Forms.Button();
      this.udY = new System.Windows.Forms.NumericUpDown();
      this.label3 = new System.Windows.Forms.Label();
      this.udX = new System.Windows.Forms.NumericUpDown();
      this.label2 = new System.Windows.Forms.Label();
      this.txtNomeCidade = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.btnIncluirCidade = new System.Windows.Forms.Button();
      this.pbMapa = new System.Windows.Forms.PictureBox();
      this.tpArvore = new System.Windows.Forms.TabPage();
      this.pnlArvore = new System.Windows.Forms.Panel();
      this.dlgAbrir = new System.Windows.Forms.OpenFileDialog();
      this.tabControl1.SuspendLayout();
      this.tpCadastro.SuspendLayout();
      this.groupBox3.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvRotas)).BeginInit();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvLigacoes)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.udY)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.udX)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.pbMapa)).BeginInit();
      this.tpArvore.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabControl1
      // 
      this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl1.Controls.Add(this.tpCadastro);
      this.tabControl1.Controls.Add(this.tpArvore);
      this.tabControl1.Location = new System.Drawing.Point(2, 4);
      this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(1658, 865);
      this.tabControl1.TabIndex = 0;
      // 
      // tpCadastro
      // 
      this.tpCadastro.Controls.Add(this.groupBox3);
      this.tpCadastro.Controls.Add(this.groupBox1);
      this.tpCadastro.Controls.Add(this.pbMapa);
      this.tpCadastro.Location = new System.Drawing.Point(4, 27);
      this.tpCadastro.Margin = new System.Windows.Forms.Padding(4);
      this.tpCadastro.Name = "tpCadastro";
      this.tpCadastro.Padding = new System.Windows.Forms.Padding(4);
      this.tpCadastro.Size = new System.Drawing.Size(1650, 834);
      this.tpCadastro.TabIndex = 0;
      this.tpCadastro.Text = "Cidades e Caminhos";
      this.tpCadastro.UseVisualStyleBackColor = true;
      this.tpCadastro.Click += new System.EventHandler(this.tpCadastro_Click);
      // 
      // groupBox3
      // 
      this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.groupBox3.BackColor = System.Drawing.Color.Ivory;
      this.groupBox3.Controls.Add(this.lbDistanciaTotal);
      this.groupBox3.Controls.Add(this.dgvRotas);
      this.groupBox3.Controls.Add(this.btnBuscarCaminho);
      this.groupBox3.Controls.Add(this.cbxCidadeDestino);
      this.groupBox3.Controls.Add(this.label6);
      this.groupBox3.Location = new System.Drawing.Point(8, 453);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(419, 374);
      this.groupBox3.TabIndex = 2;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Busca de rotas";
      // 
      // lbDistanciaTotal
      // 
      this.lbDistanciaTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lbDistanciaTotal.AutoSize = true;
      this.lbDistanciaTotal.Location = new System.Drawing.Point(11, 345);
      this.lbDistanciaTotal.Name = "lbDistanciaTotal";
      this.lbDistanciaTotal.Size = new System.Drawing.Size(109, 18);
      this.lbDistanciaTotal.TabIndex = 4;
      this.lbDistanciaTotal.Text = "Distância total: ";
      // 
      // dgvRotas
      // 
      this.dgvRotas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.dgvRotas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvRotas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
      this.dgvRotas.Location = new System.Drawing.Point(11, 100);
      this.dgvRotas.Name = "dgvRotas";
      this.dgvRotas.Size = new System.Drawing.Size(347, 242);
      this.dgvRotas.TabIndex = 3;
      // 
      // dataGridViewTextBoxColumn1
      // 
      this.dataGridViewTextBoxColumn1.HeaderText = "Rota passando por";
      this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
      this.dataGridViewTextBoxColumn1.Width = 200;
      // 
      // dataGridViewTextBoxColumn2
      // 
      this.dataGridViewTextBoxColumn2.HeaderText = "Distância";
      this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
      this.dataGridViewTextBoxColumn2.Width = 75;
      // 
      // btnBuscarCaminho
      // 
      this.btnBuscarCaminho.Location = new System.Drawing.Point(271, 52);
      this.btnBuscarCaminho.Name = "btnBuscarCaminho";
      this.btnBuscarCaminho.Size = new System.Drawing.Size(132, 32);
      this.btnBuscarCaminho.TabIndex = 2;
      this.btnBuscarCaminho.Text = "Buscar caminhos";
      this.btnBuscarCaminho.UseVisualStyleBackColor = true;
      // 
      // cbxCidadeDestino
      // 
      this.cbxCidadeDestino.FormattingEnabled = true;
      this.cbxCidadeDestino.Location = new System.Drawing.Point(11, 56);
      this.cbxCidadeDestino.Name = "cbxCidadeDestino";
      this.cbxCidadeDestino.Size = new System.Drawing.Size(243, 26);
      this.cbxCidadeDestino.TabIndex = 1;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(14, 34);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(129, 18);
      this.label6.TabIndex = 0;
      this.label6.Text = "Cidade de Destino";
      // 
      // groupBox1
      // 
      this.groupBox1.BackColor = System.Drawing.Color.AliceBlue;
      this.groupBox1.Controls.Add(this.groupBox2);
      this.groupBox1.Controls.Add(this.btnExcluirCidade);
      this.groupBox1.Controls.Add(this.btnAlterarCidade);
      this.groupBox1.Controls.Add(this.btnBuscarCidade);
      this.groupBox1.Controls.Add(this.udY);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.Controls.Add(this.udX);
      this.groupBox1.Controls.Add(this.label2);
      this.groupBox1.Controls.Add(this.txtNomeCidade);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.Controls.Add(this.btnIncluirCidade);
      this.groupBox1.Location = new System.Drawing.Point(7, 7);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(420, 439);
      this.groupBox1.TabIndex = 1;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Cadastro de Cidades";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.btnExcluirCaminho);
      this.groupBox2.Controls.Add(this.btnIncluirCaminho);
      this.groupBox2.Controls.Add(this.label5);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.numericUpDown1);
      this.groupBox2.Controls.Add(this.txtNovoDestino);
      this.groupBox2.Controls.Add(this.dgvLigacoes);
      this.groupBox2.Location = new System.Drawing.Point(6, 137);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(408, 296);
      this.groupBox2.TabIndex = 11;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Ligações";
      // 
      // btnExcluirCaminho
      // 
      this.btnExcluirCaminho.Location = new System.Drawing.Point(356, 243);
      this.btnExcluirCaminho.Name = "btnExcluirCaminho";
      this.btnExcluirCaminho.Size = new System.Drawing.Size(29, 35);
      this.btnExcluirCaminho.TabIndex = 9;
      this.btnExcluirCaminho.Text = "-";
      this.btnExcluirCaminho.UseVisualStyleBackColor = true;
      // 
      // btnIncluirCaminho
      // 
      this.btnIncluirCaminho.Location = new System.Drawing.Point(321, 243);
      this.btnIncluirCaminho.Name = "btnIncluirCaminho";
      this.btnIncluirCaminho.Size = new System.Drawing.Size(29, 35);
      this.btnIncluirCaminho.TabIndex = 8;
      this.btnIncluirCaminho.Text = "+";
      this.btnIncluirCaminho.UseVisualStyleBackColor = true;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(233, 233);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(69, 18);
      this.label5.TabIndex = 7;
      this.label5.Text = "Distância";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 232);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(96, 18);
      this.label4.TabIndex = 6;
      this.label4.Text = "Novo destino";
      // 
      // numericUpDown1
      // 
      this.numericUpDown1.Location = new System.Drawing.Point(236, 254);
      this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
      this.numericUpDown1.Name = "numericUpDown1";
      this.numericUpDown1.Size = new System.Drawing.Size(67, 24);
      this.numericUpDown1.TabIndex = 5;
      // 
      // txtNovoDestino
      // 
      this.txtNovoDestino.Location = new System.Drawing.Point(9, 253);
      this.txtNovoDestino.MaxLength = 25;
      this.txtNovoDestino.Name = "txtNovoDestino";
      this.txtNovoDestino.Size = new System.Drawing.Size(216, 24);
      this.txtNovoDestino.TabIndex = 3;
      // 
      // dgvLigacoes
      // 
      this.dgvLigacoes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dgvLigacoes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
      this.dgvLigacoes.Location = new System.Drawing.Point(7, 24);
      this.dgvLigacoes.Name = "dgvLigacoes";
      this.dgvLigacoes.Size = new System.Drawing.Size(347, 205);
      this.dgvLigacoes.TabIndex = 0;
      // 
      // Column1
      // 
      this.Column1.HeaderText = "Ligada às cidades";
      this.Column1.Name = "Column1";
      this.Column1.Width = 200;
      // 
      // Column2
      // 
      this.Column2.HeaderText = "Distância";
      this.Column2.Name = "Column2";
      this.Column2.Width = 75;
      // 
      // btnExcluirCidade
      // 
      this.btnExcluirCidade.Location = new System.Drawing.Point(306, 94);
      this.btnExcluirCidade.Name = "btnExcluirCidade";
      this.btnExcluirCidade.Size = new System.Drawing.Size(93, 34);
      this.btnExcluirCidade.TabIndex = 9;
      this.btnExcluirCidade.Text = "Excluir";
      this.btnExcluirCidade.UseVisualStyleBackColor = true;
      // 
      // btnAlterarCidade
      // 
      this.btnAlterarCidade.Location = new System.Drawing.Point(207, 94);
      this.btnAlterarCidade.Name = "btnAlterarCidade";
      this.btnAlterarCidade.Size = new System.Drawing.Size(93, 34);
      this.btnAlterarCidade.TabIndex = 8;
      this.btnAlterarCidade.Text = "Alterar";
      this.btnAlterarCidade.UseVisualStyleBackColor = true;
      // 
      // btnBuscarCidade
      // 
      this.btnBuscarCidade.Location = new System.Drawing.Point(108, 94);
      this.btnBuscarCidade.Name = "btnBuscarCidade";
      this.btnBuscarCidade.Size = new System.Drawing.Size(93, 34);
      this.btnBuscarCidade.TabIndex = 7;
      this.btnBuscarCidade.Text = "Buscar";
      this.btnBuscarCidade.UseVisualStyleBackColor = true;
      // 
      // udY
      // 
      this.udY.DecimalPlaces = 4;
      this.udY.Location = new System.Drawing.Point(327, 57);
      this.udY.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.udY.Name = "udY";
      this.udY.Size = new System.Drawing.Size(72, 24);
      this.udY.TabIndex = 6;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(213, 59);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(108, 18);
      this.label3.TabIndex = 5;
      this.label3.Text = "Y proporcional:";
      // 
      // udX
      // 
      this.udX.DecimalPlaces = 4;
      this.udX.Location = new System.Drawing.Point(123, 57);
      this.udX.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.udX.Name = "udX";
      this.udX.Size = new System.Drawing.Size(72, 24);
      this.udX.TabIndex = 4;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(9, 59);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(109, 18);
      this.label2.TabIndex = 3;
      this.label2.Text = "X proporcional:";
      this.label2.Click += new System.EventHandler(this.label2_Click);
      // 
      // txtNomeCidade
      // 
      this.txtNomeCidade.Location = new System.Drawing.Point(123, 21);
      this.txtNomeCidade.MaxLength = 25;
      this.txtNomeCidade.Name = "txtNomeCidade";
      this.txtNomeCidade.Size = new System.Drawing.Size(216, 24);
      this.txtNomeCidade.TabIndex = 2;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 24);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(120, 18);
      this.label1.TabIndex = 1;
      this.label1.Text = "Nome da cidade:";
      // 
      // btnIncluirCidade
      // 
      this.btnIncluirCidade.Location = new System.Drawing.Point(9, 94);
      this.btnIncluirCidade.Name = "btnIncluirCidade";
      this.btnIncluirCidade.Size = new System.Drawing.Size(93, 34);
      this.btnIncluirCidade.TabIndex = 0;
      this.btnIncluirCidade.Text = "Incluir";
      this.btnIncluirCidade.UseVisualStyleBackColor = true;
      // 
      // pbMapa
      // 
      this.pbMapa.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pbMapa.Image = global::Proj4.Properties.Resources.SaoPaulo_MesoMicroSemMunicip;
      this.pbMapa.Location = new System.Drawing.Point(433, 7);
      this.pbMapa.Name = "pbMapa";
      this.pbMapa.Size = new System.Drawing.Size(1208, 820);
      this.pbMapa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pbMapa.TabIndex = 0;
      this.pbMapa.TabStop = false;
      // 
      // tpArvore
      // 
      this.tpArvore.Controls.Add(this.pnlArvore);
      this.tpArvore.Location = new System.Drawing.Point(4, 27);
      this.tpArvore.Margin = new System.Windows.Forms.Padding(4);
      this.tpArvore.Name = "tpArvore";
      this.tpArvore.Padding = new System.Windows.Forms.Padding(4);
      this.tpArvore.Size = new System.Drawing.Size(1650, 834);
      this.tpArvore.TabIndex = 1;
      this.tpArvore.Text = "Árvore balanceada";
      this.tpArvore.UseVisualStyleBackColor = true;
      // 
      // pnlArvore
      // 
      this.pnlArvore.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlArvore.Location = new System.Drawing.Point(6, 3);
      this.pnlArvore.Name = "pnlArvore";
      this.pnlArvore.Size = new System.Drawing.Size(1641, 828);
      this.pnlArvore.TabIndex = 0;
      this.pnlArvore.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlArvore_Paint);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1656, 864);
      this.Controls.Add(this.tabControl1);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4);
      this.Name = "Form1";
      this.Text = "Projeto 4 2025 - Cadastro de Cidades e Caminhos - Busca de Caminhos";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.tabControl1.ResumeLayout(false);
      this.tpCadastro.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dgvRotas)).EndInit();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.dgvLigacoes)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.udY)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.udX)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.pbMapa)).EndInit();
      this.tpArvore.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tpCadastro;
    private System.Windows.Forms.TabPage tpArvore;
    private System.Windows.Forms.PictureBox pbMapa;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.NumericUpDown udX;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtNomeCidade;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button btnIncluirCidade;
    private System.Windows.Forms.NumericUpDown udY;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btnExcluirCidade;
    private System.Windows.Forms.Button btnAlterarCidade;
    private System.Windows.Forms.Button btnBuscarCidade;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.DataGridView dgvLigacoes;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.NumericUpDown numericUpDown1;
    private System.Windows.Forms.TextBox txtNovoDestino;
    private System.Windows.Forms.Button btnExcluirCaminho;
    private System.Windows.Forms.Button btnIncluirCaminho;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.ComboBox cbxCidadeDestino;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
    private System.Windows.Forms.Button btnBuscarCaminho;
    private System.Windows.Forms.DataGridView dgvRotas;
    private System.Windows.Forms.Label lbDistanciaTotal;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private System.Windows.Forms.Panel pnlArvore;
    private System.Windows.Forms.OpenFileDialog dlgAbrir;
  }
}

