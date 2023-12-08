using MadeInValDeLoire_Lib_SQL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MadeInValDeLoire_Interface
{
    public partial class Resultats : Form
    {
        #region variables
        private quiz quiz;
        private connexion maConnexion;
        private MySqlConnection UneConnexion;
        private int points, numquiz, idjoueur, nbquestions;
        private bool isInGame;
        #endregion

        #region Constructeur

        /// <summary>
        /// Contructeur de la classe resultats
        /// </summary>
        /// <param name="points">Points du joueur</param>
        /// <param name="nbquestions">Nombre de questions du quiz</param>
        /// <param name="numquiz">Numéro du quiz</param>
        public Resultats(int points, int nbquestions, int numquiz)
        {
            InitializeComponent();
            this.points = points;
            this.numquiz = numquiz;
            this.nbquestions = nbquestions;
            double pourcentage = ((double)points / nbquestions) * 100;
            double pourcentageArrondi = Math.Round(pourcentage, 0);
            FondScore(pourcentageArrondi);
            lblresult.Text = $"Ton score est de {pourcentageArrondi}%";
            isInGame = true;
        }

        /// <summary>
        /// Contructeur de la classe resultats
        /// </summary>
        /// <param name="points">Points du joueur</param>
        /// <param name="nbquestions">Nombre de questions du quiz</param>
        /// <param name="numquiz">Numéro du quiz</param>
        /// <param name="isInGame">Boolean pour savoir si le joueur à déjà enregistré son score</param>
        public Resultats(int points, int nbquestions, int numquiz, bool isInGame)
        {
            InitializeComponent();
            this.points = points;
            this.numquiz = numquiz;
            this.nbquestions = nbquestions;
            double pourcentage = ((double)points / nbquestions) * 100;
            double pourcentageArrondi = Math.Round(pourcentage, 0);
            FondScore(pourcentageArrondi);

            // Affiche le score
            lblresult.Text = $"Ton score est de {pourcentageArrondi}%";
            this.isInGame = isInGame;
            if(!isInGame)
            {
                btnenregistrer.Visible = false;
            }
        }

        /// <summary>
        /// Constructeur de la classe resultats
        /// </summary>
        /// <param name="points">Points du joueur</param>
        /// <param name="nbquestions">Nombre de questions du quiz</param>
        /// <param name="numquiz">Numéro du quiz</param>
        /// <param name="idjoueur">id du joueur</param>
        public Resultats(int points, int nbquestions, int numquiz, int idjoueur)
        {
            InitializeComponent();
            this.points = points;
            this.numquiz = numquiz;
            this.nbquestions = nbquestions;
            double pourcentage = ((double)points / nbquestions) * 100;
            double pourcentageArrondi = Math.Round(pourcentage, 0);
            FondScore(pourcentageArrondi);

            // Affiche le score
            lblresult.Text = $"Ton score est de {pourcentageArrondi}%";
            isInGame = true;
            this.idjoueur = idjoueur;
        }

        /// <summary>
        /// Constructeur de la classe resultats
        /// </summary>
        /// <param name="points">Points du joueur</param>
        /// <param name="nbquestions">Nombre de questions du quiz</param>
        /// <param name="numquiz">Numéro du quiz</param>
        /// <param name="isInGame">Boolean pour savoir si le joueur à déjà enregistré son score</param>
        /// <param name="idjoueur">id du joueur</param>
        public Resultats(int points, int nbquestions, int numquiz, bool isInGame, int idjoueur)
        {
            InitializeComponent();
            this.points = points;
            this.numquiz = numquiz;
            this.nbquestions = nbquestions;
            double pourcentage = ((double)points / nbquestions) * 100;
            double pourcentageArrondi = Math.Round(pourcentage, 0);
            FondScore(pourcentageArrondi);

            // Affiche le score
            lblresult.Text = $"Ton score est de {pourcentageArrondi}%"; 
            this.isInGame = isInGame;
            this.idjoueur = idjoueur;
            if (!isInGame)
            {
                btnenregistrer.Visible = false;
            }
        }
        #endregion

        #region Evenement click btnClose

        /// <summary>
        /// Evenement du bouton de fermeture
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // Ferme l'application
            Application.Exit();
        }
        #endregion

        #region Hover & Leave btnScoreBoard
        private void btnscoreboard_MouseHover(object sender, EventArgs e)
        {
            btnscoreboard.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnscoreboard_MouseLeave(object sender, EventArgs e)
        {
            btnscoreboard.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
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

        #region Hover & Leave btnenregistrer
        private void btnenregistrer_MouseHover(object sender, EventArgs e)
        {
            btnenregistrer.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnenregistrer_MouseLeave(object sender, EventArgs e)
        {
            btnenregistrer.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Evenement click btnAccueil
        private void btnAccueil_Click(object sender, EventArgs e)
        {

            // Vérifie si on est connecté
            // Ferme la connection
            if (UneConnexion != null)
            {
                maConnexion.seDeconnecter(UneConnexion);
            }

            // Vérifie si l'utilisateur est connecté
            // Retourne à la page d'accueil
            if (idjoueur > 0)
            {
                this.Hide();
                Accueil accueil = new Accueil(idjoueur);
                accueil.ShowDialog();
                accueil.Closed += (s, args) => this.Close();
            }
            // Retourne à la page d'accueil
            else
            {
                this.Hide();
                Accueil accueil = new Accueil();
                accueil.ShowDialog();
                accueil.Closed += (s, args) => this.Close();
            }
        }
        #endregion

        #region Evenement click btnScoreBoard

        /// <summary>
        /// Evenement click du bouton de scoreboard
        /// </summary>
        private void btnscoreboard_Click(object sender, EventArgs e)
        {
            // Vérifie qu'on soit connecté à la base de données
            if (UneConnexion != null)
            {
                maConnexion.seDeconnecter(UneConnexion);
            }

            // Vérifie que le joueur est connecté
            // Accède à la page du tableau des scores
            if (idjoueur > 0)
            {
                this.Hide();
                Scoreboard scoreboard = new Scoreboard(points, nbquestions, numquiz, isInGame, idjoueur);
                scoreboard.ShowDialog();
                scoreboard.Closed += (s, args) => this.Close();
            }
            // Accède à la page du tableau des scores
            else
            {
                this.Hide();
                Scoreboard scoreboard = new Scoreboard(points, nbquestions, numquiz, isInGame);
                scoreboard.ShowDialog();
                scoreboard.Closed += (s, args) => this.Close();
            }
        }
        #endregion

        #region Evenement click btnEnregistrer

        /// <summary>
        /// Evenement click du bouton d'enregistrement du résultat
        /// </summary>
        private void btnenregistrer_Click(object sender, EventArgs e)
        {
            maConnexion = new connexion();
            quiz = new quiz();
            UneConnexion = maConnexion.seConnecter();

            // Vérifie si le joueur à déja enregistré sont score
            if (isInGame)
            {
                // Vérifie si le joueur est connecté
                if (idjoueur > 0)
                {
                    // Ajoute le résultat dans la base de données
                    // Vérifie si on peut se connecter
                    if (UneConnexion != null)
                    {

                        quiz.addResult(numquiz, idjoueur, points, UneConnexion);
                        maConnexion.seDeconnecter(UneConnexion);
                        isInGame = false;
                        this.Hide();
                        Scoreboard scoreboard = new Scoreboard(points, nbquestions, numquiz, isInGame, idjoueur);
                        scoreboard.ShowDialog();
                        scoreboard.Closed += (s, args) => this.Close();
                    }
                }

                else
                {
                    // Ferme la connexion avant d'amener l'utilisateur vers la page d'inscription
                    if(UneConnexion != null)
                    {
                        maConnexion.seDeconnecter(UneConnexion);
                    }
                    this.Hide();
                    Inscription inscription = new Inscription(points, nbquestions, numquiz);
                    inscription.ShowDialog();
                    inscription.Closed += (s, args) => this.Close();
                }
            }
        }
        #endregion

        #region Methode FondScore

        /// <summary>
        /// Change la picturebox et le message en fonction du résultat.
        /// </summary>
        /// <param name="lescore">Le pourcentage de réussite de l'utilisateur</param>
        private void FondScore(double lescore)
        {
            if(lescore <= 40)
            {
                lbl_resultExplain.Text = "Dommage, tu auras plus de chance la prochaine fois.";
                pbmascotte.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.Group_30;
            }

            if(lescore >= 41 && lescore <=75)
            {
                lbl_resultExplain.Text = "C'est bien, mais tu peux encore faire mieux. Accroche-toi !!";
                pbmascotte.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.Group;
            }

            if (lescore >= 76 && lescore <= 99)
            {
                lbl_resultExplain.Text = "Tu est vraiment très bon en Cybersécurité, mais tu as encore beaucoup à apprendre.";
                pbmascotte.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.Group_31;
            }

            if (lescore == 100)
            {
                lbl_resultExplain.Text = "Tu es vraiment un champion, tu t'y connais parfaitement en cybersécurité. Félicitations.";
                pbmascotte.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.Group_29;
            }
        }

        #endregion
    }
}
