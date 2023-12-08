using MadeInValDeLoire_Lib_SQL;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.X500;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MadeInValDeLoire_Interface
{
    public partial class Quiz : Form
    {
        #region Variables
        private String _difficulte, _theme, _nomquiz, questionActuel;
        private int _nbquestions, _avancement, _numquiz, idJoueur, points;
        private quiz quiz;
        private connexion maConnexion;
        private MySqlConnection UneConnexion;
        private List<String> _questionsListe, choix, bonnesReponses;
        private Random random = new Random();
        private Boolean estEnTrainDeRepondre;

        #endregion

        #region Evenement click btnAccueil

        /// <summary>
        /// Evenement click du bouton d'accueil
        /// </summary>
        private void btnAccueil_Click(object sender, EventArgs e)
        {
            // Vérifie si l'utilisateur est connecté avant d'acceder à la page d'accueil
            if (idJoueur > 0)
            {
                this.Hide();
                Accueil accueil = new Accueil(idJoueur);
                accueil.ShowDialog();
                accueil.Closed += (s, args) => this.Close();
            }

            // accède à la page d'accueil
            else
            {
                this.Hide();
                Accueil accueil = new Accueil();
                accueil.ShowDialog();
                accueil.Closed += (s, args) => this.Close();
            }
        }
        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur de la classe Quiz
        /// </summary>
        /// <param name="difficulte">Difficulté du quiz</param>
        /// <param name="theme">Thème du quiz</param>
        public Quiz(String difficulte, String theme)
        {
            InitializeComponent();
            _difficulte = difficulte;
            _questionsListe = new List<String>();
            points = 0;
            _theme = theme;
            maConnexion = new connexion();
            quiz = new quiz();
            UneConnexion = maConnexion.seConnecter();
            _avancement = 0;

            // Vérifie qu'on soit connecté
            if (UneConnexion != null)
            {
                // Récupère un quiz au hasard en fonction de sa difficulté et de son thème
                DataSet resultQuiz = quiz.getQuiz(difficulte, theme, UneConnexion);

                _nomquiz = resultQuiz.Tables["quiz"].Rows[0][0].ToString();
                _nbquestions = Int32.Parse(resultQuiz.Tables["quiz"].Rows[0][3].ToString());
                _numquiz = Int32.Parse(resultQuiz.Tables["quiz"].Rows[0][4].ToString());

                // Met toutes les questions dans une listes
                DataSet resultQuestions = quiz.getQuestions(_nomquiz, UneConnexion);
                for (int i = 0; i < resultQuestions.Tables["questions"].Rows.Count; i++)
                {
                    _questionsListe.Add(resultQuestions.Tables["questions"].Rows[i][0].ToString());
                }
                // Change la questions
                questionSuivant();
            }
        }
        

        /// <summary>
        /// Constructeur de la classe quiz pour si l'utilisateur est connecté
        /// </summary>
        /// <param name="difficulte">Difficulté du quiz</param>
        /// <param name="theme">Thème du quiz</param>
        /// <param name="idJoueur">id du joueur</param>
        public Quiz(String difficulte, String theme, int idJoueur)
        {
            InitializeComponent();
            _difficulte = difficulte;
            _questionsListe = new List<String>();
            this.idJoueur = idJoueur;
            points = 0;
            _theme = theme;
            maConnexion = new connexion();
            quiz = new quiz();
            UneConnexion = maConnexion.seConnecter();
            _avancement = 0;

            // Vérifie qu'on soit connecté
            if (UneConnexion != null)
            {
                // Récupère un quiz au hasard en fonction de sa difficulté et de son thème
                DataSet resultQuiz = quiz.getQuiz(difficulte, theme, UneConnexion);

                _nomquiz = resultQuiz.Tables["quiz"].Rows[0][0].ToString();
                _nbquestions = Int32.Parse(resultQuiz.Tables["quiz"].Rows[0][3].ToString());
                _numquiz = Int32.Parse(resultQuiz.Tables["quiz"].Rows[0][4].ToString());

                // Met toutes les questions dans une listes
                DataSet resultQuestions = quiz.getQuestions(_nomquiz, UneConnexion);
                for (int i = 0; i < resultQuestions.Tables["questions"].Rows.Count; i++)
                {
                    _questionsListe.Add(resultQuestions.Tables["questions"].Rows[i][0].ToString());
                }
                // Change la questions
                questionSuivant();
            }
        }
        #endregion

        #region Evenement click btnValider

        /// <summary>
        /// Evenement bouton de validation de la question
        /// </summary>
        private void btnValider_Click(object sender, EventArgs e)
        {
            Boolean raison = true;
            estEnTrainDeRepondre = false;

            // Regarde si la réponse est bonne ou non
            for(int i = 0; (choix.Count()) > i; i++)
            {
                if (!bonnesReponses.Contains(choix[i]))
                {
                    raison = false;
                    if(reponse1.Text == choix[i])
                    {
                        reponse1.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.Red;
                    }
                    else if (reponse2.Text == choix[i])
                    {
                        reponse2.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.Red;
                    }
                    else if (reponse3.Text == choix[i])
                    {
                        reponse3.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.Red;
                    }
                    else if (reponse4.Text == choix[i])
                    {
                        reponse4.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.Red;
                    }
                }
                else if(bonnesReponses.Contains(choix[i]))
                {
                    bonnesReponses.Remove(choix[i]);
                    if (reponse1.Text == choix[i])
                    {
                        reponse1.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.green;
                    }
                    else if (reponse2.Text == choix[i])
                    {
                        reponse2.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.green;
                    }
                    else if (reponse3.Text == choix[i])
                    {
                        reponse3.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.green;
                    }
                    else if (reponse4.Text == choix[i])
                    {
                        reponse4.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.green;
                    }
                }
            }

            for (int i = 0; (bonnesReponses.Count()) > i; i++)
            {
                if (reponse1.Text == bonnesReponses[i])
                {
                    reponse1.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.green;
                }
                else if (reponse2.Text == bonnesReponses[i])
                {
                    reponse2.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.green;
                }
                else if (reponse3.Text == bonnesReponses[i])
                {
                    reponse3.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.green;
                }
                else if (reponse4.Text == bonnesReponses[i])
                {
                    reponse4.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.green;
                }
            }

                // Vérifie si il y a une mauvaise réponse dans les différentes réponses sélectionné
                if (bonnesReponses.Count() != 0)
            {
                raison = false;
            }

            // Ajoute un point si toutes les réponses sont bonnes
            if(raison == true)
            {
                points++;
            }

            // Remet la variable raison à sa valeur par défaut
            else
            {
                raison = true;
            }

            btnQuestionSuivante.Visible = true;
            btnValider.Visible = false;

        }
        #endregion

        #region Evenement click reponse4

        /// <summary>
        /// Evenement click du bouton de la quatrième réponse
        /// </summary>
        private void reponse4_Click(object sender, EventArgs e)
        {
            // Selectionne la réponse 4 si on clique dessus
            ajoutChoix(reponse4);
        }
        #endregion

        #region Evenement click reponse3

        /// <summary>
        /// Evenement click du bouton de la troisième réponse
        /// </summary>
        private void reponse3_Click(object sender, EventArgs e)
        {
            // Selectionne la réponse 3 si on clique dessus
            ajoutChoix(reponse3);
        }
        #endregion

        #region Evenement click reponse2

        /// <summary>
        /// Evenement click du bouton de la deuxième réponse
        /// </summary>
        private void reponse2_Click(object sender, EventArgs e)
        {
            // Selectionne la réponse 2 si on clique dessus
            ajoutChoix(reponse2);
        }
        #endregion

        #region Evenement click reponse1

        /// <summary>
        /// Evenement click du bouton de la première réponse
        /// </summary>
        private void reponse1_Click(object sender, EventArgs e)
        {
            // Selectionne la réponse 1 si on clique dessus
            ajoutChoix(reponse1);
        }
        #endregion

        #region click btnQuestionSuivante
        private void btnQuestionSuivante_Click(object sender, EventArgs e)
        {
            // Met la question suivante 
            if (_questionsListe.Count >= 1)
            {
                questionSuivant();
            }
            else
            {

                // Ferme la connexion
                if (UneConnexion != null)
                {
                    maConnexion.seDeconnecter(UneConnexion);
                }

                // Vérifie si le joueur est connecté avant d'accéder à la page d'accueil
                if (idJoueur > 0)
                {
                    this.Hide();
                    Resultats resultats = new Resultats(points, _nbquestions, _numquiz, idJoueur);
                    resultats.ShowDialog();
                    resultats.Closed += (s, args) => this.Close();
                }

                // Accède à la page d'accueil
                else
                {
                    this.Hide();
                    Resultats resultats = new Resultats(points, _nbquestions, _numquiz);
                    resultats.ShowDialog();
                    resultats.Closed += (s, args) => this.Close();
                }
            }
        }
        #endregion

        #region Méthode ajoutChoix

        /// <summary>
        /// Méthode ajoutant le choix sélectionné
        /// </summary>
        /// <param name="button">Bouton des réponses</param>
        private void ajoutChoix(Button button)
        {
            if (estEnTrainDeRepondre)
            {
                // Vérifie si le bouton à déja été sélectionné
                if (!choix.Contains(button.Text))
                {
                    choix.Add(button.Text);
                    button.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.Bouton_Orange;
                }
                else
                {
                    choix.Remove(button.Text);
                    button.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
                }
            }
        }
        #endregion

        #region Méthode questionSuivant

        /// <summary>
        /// Méthode de la question suivante
        /// </summary>
        private void questionSuivant()
        {
            // Ajoute les réponses de la prochaines questions
            List<Button> btnReponses = new List<Button>();
            btnReponses.Add(reponse1);
            btnReponses.Add(reponse2);
            btnReponses.Add(reponse3);
            btnReponses.Add(reponse4);
            _avancement++;
            lbl_NumQuest.Text = $"Question N° {_avancement} :";

            if (_avancement == _nbquestions)
            {
                lbl_NumQuest.Text = $"Dernière question :";
            }

            // Réinitialise les couleurs de chaque réponses
            foreach (Button button in btnReponses)
            {
                button.ForeColor = Color.White;
                button.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
                button.Visible = false;
            }

            bonnesReponses = new List<string>();
            choix = new List<string>();
            questionActuel = _questionsListe[random.Next(_questionsListe.Count)];

            // Remplace les réponses
            _questionsListe.Remove(questionActuel);

            question.Text = questionActuel;

            DataSet resultReponses = quiz.getReponses(questionActuel, UneConnexion);

            // Affiche les boutons de réponses
            for (int i = 0; i < resultReponses.Tables["reponses"].Rows.Count; i++)
            {
                btnReponses[i].Visible = true;
                btnReponses[i].Text = resultReponses.Tables["reponses"].Rows[i][0].ToString();
                if (Boolean.Parse(resultReponses.Tables["reponses"].Rows[i][1].ToString()) == true)
                {
                    bonnesReponses.Add(resultReponses.Tables["reponses"].Rows[i][0].ToString());
                }
            }

            estEnTrainDeRepondre = true;
            btnQuestionSuivante.Visible = false;
            btnValider.Visible = true;

        }
        #endregion

        #region Hover & Leave Btnreponse1
        private void reponse1_MouseHover(object sender, EventArgs e)
        {
            if (estEnTrainDeRepondre && !choix.Contains(reponse1.Text))
            {
                reponse1.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
            }
        }

        private void reponse1_MouseLeave(object sender, EventArgs e)
        {
            if (estEnTrainDeRepondre && !choix.Contains(reponse1.Text))
            {
                reponse1.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
            }
        }
        #endregion

        #region Hover & Leave BtnReponse2
        private void reponse2_MouseHover(object sender, EventArgs e)
        {
            if (estEnTrainDeRepondre && !choix.Contains(reponse2.Text))
            {
                reponse2.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
            }
        }

        private void reponse2_MouseLeave(object sender, EventArgs e)
        {
            if (estEnTrainDeRepondre && !choix.Contains(reponse2.Text))
            {
                reponse2.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
            }
        }
        #endregion

        #region Hover & Leave Btnreponse3
        private void reponse3_MouseHover(object sender, EventArgs e)
        {
            if (estEnTrainDeRepondre && !choix.Contains(reponse3.Text))
            {
                reponse3.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
            }
        }

        private void reponse3_MouseLeave(object sender, EventArgs e)
        {
            if (estEnTrainDeRepondre && !choix.Contains(reponse3.Text))
            {
                reponse3.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
            }
        }
        #endregion

        #region Hover & Leave Btnreponse4
        private void reponse4_MouseHover(object sender, EventArgs e)
        {
            if (estEnTrainDeRepondre && !choix.Contains(reponse4.Text))
            {
                reponse4.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
            }
        }

        private void reponse4_MouseLeave(object sender, EventArgs e)
        {
            if (estEnTrainDeRepondre && !choix.Contains(reponse4.Text))
            {
                reponse4.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
            }
        }
        #endregion

        #region Hover & Leave btnValider
        private void btnValider_MouseHover(object sender, EventArgs e)
        {
            btnValider.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnValider_MouseLeave(object sender, EventArgs e)
        {
            btnValider.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Hover & Leave btnAccueil
        private void btnAccueil_MouseHover(object sender, EventArgs e)
        {
            btnAccueil.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnAccueil_MouseLeave(object sender, EventArgs e)
        {
            btnAccueil.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Hover & Leave btnQuestionSuivante
        private void btnQuestionSuivante_MouseHover(object sender, EventArgs e)
        {
            btnQuestionSuivante.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnQuestionSuivante_MouseLeave(object sender, EventArgs e)
        {
            btnQuestionSuivante.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

    }
}
