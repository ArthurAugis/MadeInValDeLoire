using MadeInValDeLoire_Lib_SQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MadeInValDeLoire_Interface
{
    public partial class Administrateur : Form
    {
        #region Variables
        private int idJoueur, theme, numquiz, idquestion, reponse;
        private utilisateurs utils;
        private quiz unquiz;
        private connexion maConnexion;
        private MySqlConnection UneConnexion;
        private String difficulte;
        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeurs de la classe administrateur
        /// </summary>
        /// <param name="idJoueur">id du joueur</param>
        public Administrateur(int idJoueur)
        {
            InitializeComponent();
            this.idJoueur = idJoueur;
            maConnexion = new connexion();
            unquiz = new quiz();
            utils = new utilisateurs();
            UneConnexion = maConnexion.seConnecter();

            // Vérifie qu'on est connecté
            if (UneConnexion != null)
            {
                DataSet listquiz = unquiz.getQuizList(UneConnexion);

                // Instancie la liste de questions dans la combo box
                for (int i = 0; i < listquiz.Tables["listquiz"].Rows.Count; i++)
                {
                    String quiz = listquiz.Tables["listquiz"].Rows[i][0].ToString();
                    cbQuiz.Items.Add(quiz);
                }

                DataSet listNonAdmin = utils.getNonAdminList(UneConnexion);

                // Instancie la liste d'utilisateur dans la combo box
                for (int i = 0; i < listNonAdmin.Tables["listnonadmin"].Rows.Count; i++)
                {
                    String nonadmin = $"{listNonAdmin.Tables["listnonadmin"].Rows[i][0]} {listNonAdmin.Tables["listnonadmin"].Rows[i][1]}";
                    cbNonAdministrateur.Items.Add(nonadmin);
                }
            }
        }
        #endregion

        #region Hover & Leave btnAjoutAdmin
        private void btnAjoutAdmin_MouseHover(object sender, EventArgs e)
        {
            btnAjoutAdmin.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnAjoutAdmin_MouseLeave(object sender, EventArgs e)
        {
            btnAjoutAdmin.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Hover & Leave btnajoutquestion
        private void btnajoutquestion_MouseHover(object sender, EventArgs e)
        {
            btnajoutquestion.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnajoutquestion_MouseLeave(object sender, EventArgs e)
        {
            btnajoutquestion.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Evenement click btnAccueil

        /// <summary>
        /// Evenement click du bouton d'accueil
        /// </summary>
        private void btnAccueil_Click(object sender, EventArgs e)
        {
            // Ferme la connexion avant de retourner à la page d'accueil
            if (UneConnexion != null)
            {
                maConnexion.seDeconnecter(UneConnexion);
            }
            this.Hide();
            Accueil accueil = new Accueil(idJoueur);
            accueil.ShowDialog();
            accueil.Closed += (s, args) => this.Close();
        }
        #endregion

        #region Evenement click btnAjoutQuestion

        /// <summary>
        /// Evenement click du bouton permettant d'ajouter des questions
        /// </summary>
        private void btnajoutquestion_Click(object sender, EventArgs e)
        {
            // Vérifie que le nom du quiz ne soit pas vide 
            if (cbQuiz.SelectedIndex != -1 && !String.IsNullOrEmpty(tbQuestion.Text))
            {
                DataSet infoquiz = unquiz.getQuizInfo(cbQuiz.Text,UneConnexion);

                numquiz = Int32.Parse(infoquiz.Tables["infoquiz"].Rows[0][0].ToString());
                difficulte = infoquiz.Tables["infoquiz"].Rows[0][1].ToString();
                theme = Int32.Parse(infoquiz.Tables["infoquiz"].Rows[0][2].ToString());

                idquestion = Int32.Parse(unquiz.addQuestion(tbQuestion.Text, difficulte, theme, UneConnexion).ToString());
                // Ajoute les réponses écrites par l'utilisateur
                AjoutReponse(tbreponse1.Text, cbbonnerep1);
                AjoutReponse(tbreponse2.Text, cbbonnerep2);
                AjoutReponse(tbreponse3.Text, cbbonnerep3);
                AjoutReponse(tbreponse4.Text, cbbonnerep4);

                unquiz.addQuestionQuiz(idquestion, numquiz, UneConnexion);

                // Confirme l'ajout de la question à l'utilisateur
                lblMessage.Text = "Ajout de la question réussie";
            }
        }
        #endregion

        #region Méthode AjoutReponse


        /// <summary>
        /// Ajoute les réponses à la question dans la base de données
        /// </summary>
        /// <param name="Reponse">Réponse à la question</param>
        /// <param name="cb">Combo Box</param>
        private void AjoutReponse(String Reponse, ComboBox cb)
        {
            // Vérifie qu'il y ai des réponses 
            if (!String.IsNullOrEmpty(Reponse) && cb.SelectedIndex != -1)
            {
                // Ajoute ces réponses à la question
                reponse = Int32.Parse(unquiz.addPropositions(Reponse, UneConnexion).ToString());
                unquiz.addReponse(idquestion, reponse, cb.SelectedIndex, UneConnexion);
            }
        }
        #endregion

        #region Evenement click btnAjoutAdmin

        /// <summary>
        /// Evenement click du bouton d'ajout de l'admin
        /// </summary>
        private void btnAjoutAdmin_Click(object sender, EventArgs e)
        {
            // Vérifie qu'il y ai des utilisateur dans la combo box
            if(cbNonAdministrateur.SelectedIndex != -1) 
            {
                string[] utilisateur = cbNonAdministrateur.Text.Split(' ');
                // Ajoute un administrateur
                utils.addAdmin(utilisateur[0], utilisateur[1], UneConnexion);
                cbNonAdministrateur.Items.RemoveAt(cbNonAdministrateur.SelectedIndex);
                lblMessage.Text = "Ajout de l'admin réussi";
            }
        }
        #endregion
    }
}
