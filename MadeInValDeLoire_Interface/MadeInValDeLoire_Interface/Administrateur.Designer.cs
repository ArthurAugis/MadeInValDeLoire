namespace MadeInValDeLoire_Interface
{
    partial class Administrateur
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblAjoutQuestion = new System.Windows.Forms.Label();
            this.lblquiz = new System.Windows.Forms.Label();
            this.cbQuiz = new System.Windows.Forms.ComboBox();
            this.lblQuestion = new System.Windows.Forms.Label();
            this.tbQuestion = new System.Windows.Forms.TextBox();
            this.lblReponses = new System.Windows.Forms.Label();
            this.tbreponse1 = new System.Windows.Forms.TextBox();
            this.tbreponse2 = new System.Windows.Forms.TextBox();
            this.tbreponse3 = new System.Windows.Forms.TextBox();
            this.tbreponse4 = new System.Windows.Forms.TextBox();
            this.cbbonnerep1 = new System.Windows.Forms.ComboBox();
            this.btnAccueil = new System.Windows.Forms.Button();
            this.cbbonnerep2 = new System.Windows.Forms.ComboBox();
            this.cbbonnerep3 = new System.Windows.Forms.ComboBox();
            this.cbbonnerep4 = new System.Windows.Forms.ComboBox();
            this.btnajoutquestion = new System.Windows.Forms.Button();
            this.lblAjoutAdmin = new System.Windows.Forms.Label();
            this.lblListNonAdmin = new System.Windows.Forms.Label();
            this.cbNonAdministrateur = new System.Windows.Forms.ComboBox();
            this.btnAjoutAdmin = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblAjoutQuestion
            // 
            this.lblAjoutQuestion.AutoSize = true;
            this.lblAjoutQuestion.BackColor = System.Drawing.Color.Transparent;
            this.lblAjoutQuestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAjoutQuestion.ForeColor = System.Drawing.Color.White;
            this.lblAjoutQuestion.Location = new System.Drawing.Point(388, 203);
            this.lblAjoutQuestion.Name = "lblAjoutQuestion";
            this.lblAjoutQuestion.Size = new System.Drawing.Size(230, 25);
            this.lblAjoutQuestion.TabIndex = 0;
            this.lblAjoutQuestion.Text = "Ajouter une question";
            // 
            // lblquiz
            // 
            this.lblquiz.AutoSize = true;
            this.lblquiz.BackColor = System.Drawing.Color.Transparent;
            this.lblquiz.ForeColor = System.Drawing.Color.White;
            this.lblquiz.Location = new System.Drawing.Point(390, 254);
            this.lblquiz.Name = "lblquiz";
            this.lblquiz.Size = new System.Drawing.Size(28, 13);
            this.lblquiz.TabIndex = 1;
            this.lblquiz.Text = "Quiz";
            // 
            // cbQuiz
            // 
            this.cbQuiz.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbQuiz.FormattingEnabled = true;
            this.cbQuiz.Location = new System.Drawing.Point(393, 270);
            this.cbQuiz.Name = "cbQuiz";
            this.cbQuiz.Size = new System.Drawing.Size(121, 21);
            this.cbQuiz.TabIndex = 2;
            // 
            // lblQuestion
            // 
            this.lblQuestion.AutoSize = true;
            this.lblQuestion.BackColor = System.Drawing.Color.Transparent;
            this.lblQuestion.ForeColor = System.Drawing.Color.White;
            this.lblQuestion.Location = new System.Drawing.Point(390, 294);
            this.lblQuestion.Name = "lblQuestion";
            this.lblQuestion.Size = new System.Drawing.Size(49, 13);
            this.lblQuestion.TabIndex = 3;
            this.lblQuestion.Text = "Question";
            // 
            // tbQuestion
            // 
            this.tbQuestion.Location = new System.Drawing.Point(393, 311);
            this.tbQuestion.Name = "tbQuestion";
            this.tbQuestion.Size = new System.Drawing.Size(121, 20);
            this.tbQuestion.TabIndex = 4;
            // 
            // lblReponses
            // 
            this.lblReponses.AutoSize = true;
            this.lblReponses.BackColor = System.Drawing.Color.Transparent;
            this.lblReponses.ForeColor = System.Drawing.Color.White;
            this.lblReponses.Location = new System.Drawing.Point(390, 345);
            this.lblReponses.Name = "lblReponses";
            this.lblReponses.Size = new System.Drawing.Size(55, 13);
            this.lblReponses.TabIndex = 5;
            this.lblReponses.Text = "Réponses";
            // 
            // tbreponse1
            // 
            this.tbreponse1.Location = new System.Drawing.Point(393, 361);
            this.tbreponse1.Name = "tbreponse1";
            this.tbreponse1.Size = new System.Drawing.Size(121, 20);
            this.tbreponse1.TabIndex = 7;
            // 
            // tbreponse2
            // 
            this.tbreponse2.Location = new System.Drawing.Point(393, 387);
            this.tbreponse2.Name = "tbreponse2";
            this.tbreponse2.Size = new System.Drawing.Size(121, 20);
            this.tbreponse2.TabIndex = 8;
            // 
            // tbreponse3
            // 
            this.tbreponse3.Location = new System.Drawing.Point(393, 413);
            this.tbreponse3.Name = "tbreponse3";
            this.tbreponse3.Size = new System.Drawing.Size(121, 20);
            this.tbreponse3.TabIndex = 9;
            // 
            // tbreponse4
            // 
            this.tbreponse4.Location = new System.Drawing.Point(393, 439);
            this.tbreponse4.Name = "tbreponse4";
            this.tbreponse4.Size = new System.Drawing.Size(121, 20);
            this.tbreponse4.TabIndex = 10;
            // 
            // cbbonnerep1
            // 
            this.cbbonnerep1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbonnerep1.FormattingEnabled = true;
            this.cbbonnerep1.Items.AddRange(new object[] {
            "Mauvaise réponse",
            "Bonne réponse"});
            this.cbbonnerep1.Location = new System.Drawing.Point(520, 361);
            this.cbbonnerep1.Name = "cbbonnerep1";
            this.cbbonnerep1.Size = new System.Drawing.Size(101, 21);
            this.cbbonnerep1.TabIndex = 11;
            // 
            // btnAccueil
            // 
            this.btnAccueil.BackColor = System.Drawing.Color.Transparent;
            this.btnAccueil.BackgroundImage = global::MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
            this.btnAccueil.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAccueil.FlatAppearance.BorderSize = 0;
            this.btnAccueil.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAccueil.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAccueil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccueil.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold);
            this.btnAccueil.ForeColor = System.Drawing.Color.White;
            this.btnAccueil.Location = new System.Drawing.Point(1250, 780);
            this.btnAccueil.Name = "btnAccueil";
            this.btnAccueil.Size = new System.Drawing.Size(338, 108);
            this.btnAccueil.TabIndex = 12;
            this.btnAccueil.Text = "Revenir à l\'accueil";
            this.btnAccueil.UseVisualStyleBackColor = false;
            this.btnAccueil.Click += new System.EventHandler(this.btnAccueil_Click);
            // 
            // cbbonnerep2
            // 
            this.cbbonnerep2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbonnerep2.FormattingEnabled = true;
            this.cbbonnerep2.Items.AddRange(new object[] {
            "Mauvaise réponse",
            "Bonne réponse"});
            this.cbbonnerep2.Location = new System.Drawing.Point(520, 388);
            this.cbbonnerep2.Name = "cbbonnerep2";
            this.cbbonnerep2.Size = new System.Drawing.Size(101, 21);
            this.cbbonnerep2.TabIndex = 13;
            // 
            // cbbonnerep3
            // 
            this.cbbonnerep3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbonnerep3.FormattingEnabled = true;
            this.cbbonnerep3.Items.AddRange(new object[] {
            "Mauvaise réponse",
            "Bonne réponse"});
            this.cbbonnerep3.Location = new System.Drawing.Point(520, 413);
            this.cbbonnerep3.Name = "cbbonnerep3";
            this.cbbonnerep3.Size = new System.Drawing.Size(101, 21);
            this.cbbonnerep3.TabIndex = 14;
            // 
            // cbbonnerep4
            // 
            this.cbbonnerep4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbonnerep4.FormattingEnabled = true;
            this.cbbonnerep4.Items.AddRange(new object[] {
            "Mauvaise réponse",
            "Bonne réponse"});
            this.cbbonnerep4.Location = new System.Drawing.Point(520, 439);
            this.cbbonnerep4.Name = "cbbonnerep4";
            this.cbbonnerep4.Size = new System.Drawing.Size(101, 21);
            this.cbbonnerep4.TabIndex = 15;
            // 
            // btnajoutquestion
            // 
            this.btnajoutquestion.BackColor = System.Drawing.Color.Transparent;
            this.btnajoutquestion.BackgroundImage = global::MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
            this.btnajoutquestion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnajoutquestion.FlatAppearance.BorderSize = 0;
            this.btnajoutquestion.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnajoutquestion.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnajoutquestion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnajoutquestion.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnajoutquestion.ForeColor = System.Drawing.Color.White;
            this.btnajoutquestion.Location = new System.Drawing.Point(393, 466);
            this.btnajoutquestion.Name = "btnajoutquestion";
            this.btnajoutquestion.Size = new System.Drawing.Size(228, 51);
            this.btnajoutquestion.TabIndex = 16;
            this.btnajoutquestion.Text = "Ajouter la question";
            this.btnajoutquestion.UseVisualStyleBackColor = false;
            this.btnajoutquestion.Click += new System.EventHandler(this.btnajoutquestion_Click);
            this.btnajoutquestion.MouseEnter += new System.EventHandler(this.btnajoutquestion_MouseHover);
            this.btnajoutquestion.MouseLeave += new System.EventHandler(this.btnajoutquestion_MouseLeave);
            this.btnajoutquestion.MouseHover += new System.EventHandler(this.btnajoutquestion_MouseHover);
            // 
            // lblAjoutAdmin
            // 
            this.lblAjoutAdmin.AutoSize = true;
            this.lblAjoutAdmin.BackColor = System.Drawing.Color.Transparent;
            this.lblAjoutAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblAjoutAdmin.ForeColor = System.Drawing.Color.White;
            this.lblAjoutAdmin.Location = new System.Drawing.Point(964, 203);
            this.lblAjoutAdmin.Name = "lblAjoutAdmin";
            this.lblAjoutAdmin.Size = new System.Drawing.Size(277, 25);
            this.lblAjoutAdmin.TabIndex = 17;
            this.lblAjoutAdmin.Text = "Ajouter un administrateur";
            // 
            // lblListNonAdmin
            // 
            this.lblListNonAdmin.AutoSize = true;
            this.lblListNonAdmin.BackColor = System.Drawing.Color.Transparent;
            this.lblListNonAdmin.ForeColor = System.Drawing.Color.White;
            this.lblListNonAdmin.Location = new System.Drawing.Point(1034, 254);
            this.lblListNonAdmin.Name = "lblListNonAdmin";
            this.lblListNonAdmin.Size = new System.Drawing.Size(138, 13);
            this.lblListNonAdmin.TabIndex = 18;
            this.lblListNonAdmin.Text = "Liste des non administrateur";
            // 
            // cbNonAdministrateur
            // 
            this.cbNonAdministrateur.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNonAdministrateur.FormattingEnabled = true;
            this.cbNonAdministrateur.Location = new System.Drawing.Point(1037, 270);
            this.cbNonAdministrateur.Name = "cbNonAdministrateur";
            this.cbNonAdministrateur.Size = new System.Drawing.Size(135, 21);
            this.cbNonAdministrateur.TabIndex = 19;
            // 
            // btnAjoutAdmin
            // 
            this.btnAjoutAdmin.BackColor = System.Drawing.Color.Transparent;
            this.btnAjoutAdmin.BackgroundImage = global::MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
            this.btnAjoutAdmin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAjoutAdmin.FlatAppearance.BorderSize = 0;
            this.btnAjoutAdmin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAjoutAdmin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAjoutAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAjoutAdmin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAjoutAdmin.ForeColor = System.Drawing.Color.White;
            this.btnAjoutAdmin.Location = new System.Drawing.Point(1037, 294);
            this.btnAjoutAdmin.Name = "btnAjoutAdmin";
            this.btnAjoutAdmin.Size = new System.Drawing.Size(135, 48);
            this.btnAjoutAdmin.TabIndex = 20;
            this.btnAjoutAdmin.Text = "Ajouter un administrateur";
            this.btnAjoutAdmin.UseVisualStyleBackColor = false;
            this.btnAjoutAdmin.Click += new System.EventHandler(this.btnAjoutAdmin_Click);
            this.btnAjoutAdmin.MouseEnter += new System.EventHandler(this.btnAjoutAdmin_MouseHover);
            this.btnAjoutAdmin.MouseLeave += new System.EventHandler(this.btnAjoutAdmin_MouseLeave);
            this.btnAjoutAdmin.MouseHover += new System.EventHandler(this.btnAjoutAdmin_MouseHover);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(12, 334);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 13);
            this.lblMessage.TabIndex = 21;
            // 
            // Administrateur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::MadeInValDeLoire_Interface.Properties.Resources.Fond_12;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1600, 900);
            this.ControlBox = false;
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnAjoutAdmin);
            this.Controls.Add(this.cbNonAdministrateur);
            this.Controls.Add(this.lblListNonAdmin);
            this.Controls.Add(this.lblAjoutAdmin);
            this.Controls.Add(this.btnajoutquestion);
            this.Controls.Add(this.cbbonnerep4);
            this.Controls.Add(this.cbbonnerep3);
            this.Controls.Add(this.cbbonnerep2);
            this.Controls.Add(this.btnAccueil);
            this.Controls.Add(this.cbbonnerep1);
            this.Controls.Add(this.tbreponse4);
            this.Controls.Add(this.tbreponse3);
            this.Controls.Add(this.tbreponse2);
            this.Controls.Add(this.tbreponse1);
            this.Controls.Add(this.lblReponses);
            this.Controls.Add(this.tbQuestion);
            this.Controls.Add(this.lblQuestion);
            this.Controls.Add(this.cbQuiz);
            this.Controls.Add(this.lblquiz);
            this.Controls.Add(this.lblAjoutQuestion);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Administrateur";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administrateur";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAjoutQuestion;
        private System.Windows.Forms.Label lblquiz;
        private System.Windows.Forms.ComboBox cbQuiz;
        private System.Windows.Forms.Label lblQuestion;
        private System.Windows.Forms.TextBox tbQuestion;
        private System.Windows.Forms.Label lblReponses;
        private System.Windows.Forms.TextBox tbreponse1;
        private System.Windows.Forms.TextBox tbreponse2;
        private System.Windows.Forms.TextBox tbreponse3;
        private System.Windows.Forms.TextBox tbreponse4;
        private System.Windows.Forms.ComboBox cbbonnerep1;
        private System.Windows.Forms.Button btnAccueil;
        private System.Windows.Forms.ComboBox cbbonnerep2;
        private System.Windows.Forms.ComboBox cbbonnerep3;
        private System.Windows.Forms.ComboBox cbbonnerep4;
        private System.Windows.Forms.Button btnajoutquestion;
        private System.Windows.Forms.Label lblAjoutAdmin;
        private System.Windows.Forms.Label lblListNonAdmin;
        private System.Windows.Forms.ComboBox cbNonAdministrateur;
        private System.Windows.Forms.Button btnAjoutAdmin;
        private System.Windows.Forms.Label lblMessage;
    }
}