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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MadeInValDeLoire_Interface
{
    public partial class Inscription : Form
    {
        #region Variables
        private utilisateurs utils;
        private quiz quiz;
        private connexion maConnexion;
        private MySqlConnection UneConnexion;
        private int points, nbquestions, numquiz;
        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur de la classe inscription
        /// </summary>
        public Inscription()
        {
            InitializeComponent();
            maConnexion = new connexion();
            utils = new utilisateurs();
            UneConnexion = maConnexion.seConnecter();
            this.points = -1;
        }

        /// <summary>
        /// Constructeur de la classe inscription
        /// </summary>
        /// <param name="points"></param>
        /// <param name="nbquestions"></param>
        /// <param name="numquiz"></param>
        public Inscription(int points, int nbquestions, int numquiz)
        {
            InitializeComponent();
            maConnexion = new connexion();
            utils = new utilisateurs();
            UneConnexion = maConnexion.seConnecter();
            this.points = points;
            this.nbquestions = nbquestions;
            this.numquiz = numquiz;
        }
        #endregion

        #region Evenement click btninsc


        /// <summary>
        /// Evenement du bouton d'inscription
        /// </summary>
        private void btninsc_Click(object sender, EventArgs e)
        {
            // Vérifie que le prénom, le nom et le mot de passe soient écrits
            if (!String.IsNullOrEmpty(tbinscprenom.Text) && !String.IsNullOrEmpty(tbinscnom.Text) && !String.IsNullOrEmpty(tbinscmdp.Text))
            {
                // Vérifie qu'on soit connecté à la bdd
                if (UneConnexion != null)
                {
                    object result = utils.createUser(tbinscnom.Text, tbinscprenom.Text, tbinscpseudo.Text, tbinscmdp.Text, UneConnexion);
                    quiz = new quiz();

                    // Nous amène à la page d'accueil
                    // Vérifie qu'il y ai quelque chose dans la variable result
                    if (Int32.Parse(result.ToString()) > 0 && points == -1)
                    {
                        this.Hide();
                        Accueil accueil = new Accueil(Int32.Parse(result.ToString()));
                        accueil.ShowDialog();
                        accueil.Closed += (s, args) => this.Close();
                    }

                    // Nous amène à la page des scores
                    // Vérifie qu'il y ai quelque chose dans la variable result
                    else if (Int32.Parse(result.ToString()) > 0 && points >= 0)
                    {
                        // Ajoute le résultat dans la base de données
                        quiz.addResult(numquiz, Int32.Parse(result.ToString()), points, UneConnexion);

                        // Ferme la connexion avant d'aller à la page des scores
                        if (UneConnexion != null)
                        {
                            maConnexion.seDeconnecter(UneConnexion);
                        }
                        this.Hide();
                        Scoreboard scoreboard = new Scoreboard(points, nbquestions, numquiz, false, Int32.Parse(result.ToString()));
                        scoreboard.ShowDialog();
                        scoreboard.Closed += (s, args) => this.Close();
                    }

                    // Vérifie qu'il n'y ai pas un compte indentique déja existant contenant le même prénom et nom
                    else if(Int32.Parse(result.ToString()) == -2)
                    {
                        lblerreurinsc.Text = "Compte déjà existant : nom/prenom déjà utilisé";
                    }

                    // Vérifie qu'il n'y ai pas un compte indentique déja existant contenant le même pseudonyme
                    else if (Int32.Parse(result.ToString()) == -1)
                    {
                        lblerreurinsc.Text = "Compte déjà existant : pseudonyme déjà utilisé";

                    }

                    // Ferme la connexion
                    if (UneConnexion != null)
                    {
                        maConnexion.seDeconnecter(UneConnexion);
                    }

                }
            }
        }
        #endregion

        #region Evenement click btnVoirConn

        /// <summary>
        /// Evenement bouton permettant de voir le mot de passe en se connectant
        /// </summary>
        private void btnVoirConn_Click(object sender, EventArgs e)
        {
            // Montre le mot de passe si celui-ci est caché
            if (tbConnMDP.UseSystemPasswordChar == true)
            {
                tbConnMDP.UseSystemPasswordChar = false;
            }

            // Cache le mot de passe si celui-ci est déja affiché
            else if (tbConnMDP.UseSystemPasswordChar == false)
            {
                tbConnMDP.UseSystemPasswordChar = true;
            }
        }
        #endregion

        #region Evenement click btnVoirInsc

        /// <summary>
        /// Evenement bouton permettant de voir le mot de passe en s'inscrivant
        /// </summary>
        private void btnVoirInsc_Click(object sender, EventArgs e)
        {
            // Montre le mot de passe si celui-ci est caché
            if (tbinscmdp.UseSystemPasswordChar == true)
            {
                tbinscmdp.UseSystemPasswordChar = false;
            }

            // Cache le mot de passe si celui-ci est déja affiché
            else if (tbinscmdp.UseSystemPasswordChar == false)
            {
                tbinscmdp.UseSystemPasswordChar = true;
            }
        }


        #endregion

        #region Evenement click btnconn

        /// <summary>
        /// Evenement click du bouton de connexion
        /// </summary>
        private void btnconn_Click(object sender, EventArgs e)
        {

            // Vérifie que toutes les informations sont bien inscrises
            if (!string.IsNullOrEmpty(tbConnPrenom.Text) && !string.IsNullOrEmpty(tbConnNom.Text) && !string.IsNullOrEmpty(tbConnMDP.Text))
            {

                // Vérifie qu'on soit connecté
                if (UneConnexion != null)
                {
                    DataSet result = utils.logIn(tbConnNom.Text, tbConnPrenom.Text, tbConnMDP.Text, UneConnexion);
                    quiz = new quiz();

                    // Vérifie si le résultat n'est pas null
                    if (result != null && result.Tables["login"].Rows.Count > 0 && points == -1)
                    {
                        int idUser = Convert.ToInt32(result.Tables["login"].Rows[0]["id"]);

                        // Ferme la connexion avant d'acceder à la page d'accueil
                        if (UneConnexion != null)
                        {
                            maConnexion.seDeconnecter(UneConnexion);
                        }
                        this.Hide();
                        Accueil accueil = new Accueil(idUser);
                        accueil.ShowDialog();
                        accueil.Closed += (s, args) => this.Close();
                    }

                    // Vérifie si le résultat n'est pas null
                    else if (result != null && result.Tables["login"].Rows.Count > 0 && points >= 0)
                    {
                        int idUser = Convert.ToInt32(result.Tables["login"].Rows[0]["id"]);
                        quiz.addResult(numquiz, idUser, points, UneConnexion);

                        // Ferme la connexion avant d'acceder à la page du score
                        if (UneConnexion != null)
                        {
                            maConnexion.seDeconnecter(UneConnexion);
                        }
                        this.Hide();
                        Scoreboard scoreboard = new Scoreboard(points, nbquestions, numquiz, false, idUser);
                        scoreboard.ShowDialog();
                        scoreboard.Closed += (s, args) => this.Close();

                    }

                    // Affiche une erreur si le nom, prénom ou le mot de passe est incorrect
                    else
                    {
                        lblerreurconn.Text = "Erreur de connexion : nom/prénom/mot de passe incorrect";
                    }
                }
            }
        }
        #endregion

        #region Evenement click btnAccueil

        /// <summary>
        /// Evenement du bouton d'accueil
        /// </summary>
        private void btnaccueil_Click(object sender, EventArgs e)
        {
            // Vérifie qu'on soit connecter avant d'acceder à la page d'accueil
            if (UneConnexion != null)
            {
                maConnexion.seDeconnecter(UneConnexion);
            }
            this.Hide();
            Accueil accueil = new Accueil();
            accueil.ShowDialog();
            accueil.Closed += (s, args) => this.Close();
        }
        #endregion


        #region Hover & Leave btninsc
        private void btninsc_MouseHover(object sender, EventArgs e)
        {
            btninsc.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btninsc_MouseLeave(object sender, EventArgs e)
        {
            btninsc.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Hover & Leave btnconn
        private void btnconn_MouseHover(object sender, EventArgs e)
        {
            btnconn.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnconn_MouseLeave(object sender, EventArgs e)
        {
            btnconn.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Hover & Leave btnAccueil
        private void btnaccueil_MouseHover(object sender, EventArgs e)
        {
            btnaccueil.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnaccueil_MouseLeave(object sender, EventArgs e)
        {
            btnaccueil.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion
    }
}
