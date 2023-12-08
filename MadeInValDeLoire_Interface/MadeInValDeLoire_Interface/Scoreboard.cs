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
    public partial class Scoreboard : Form
    {
        #region Variables
        private bool isInGame;
        private int points, nbquestions, numquiz, idjoueur;
        private quiz unquiz;
        private connexion maConnexion;
        private MySqlConnection UneConnexion;
        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur de la classe Scoreboard
        /// </summary>
        /// <param name="points">Points du joueur</param>
        /// <param name="nbquestions">Nombre de questions</param>
        /// <param name="numquiz">Numéro de quiz</param>
        /// <param name="isInGame">Boolean pour savoir si le joueur à déjà enregistré son score</param>
        public Scoreboard(int points, int nbquestions, int numquiz, bool isInGame)
        {
            InitializeComponent();
            this.isInGame = isInGame;
            this.points = points;
            this.nbquestions = nbquestions;
            this.numquiz = numquiz;
            // Récupère les 10 meilleurs joueurs
            getTop10();
        }

        /// <summary>
        /// Constructeur de la classe Scoreboard
        /// </summary>
        /// <param name="points">Points du joueur</param>
        /// <param name="nbquestions">Nombre de questions</param>
        /// <param name="numquiz">Numéro de quiz</param>
        /// <param name="isInGame">Boolean pour savoir si le joueur à déjà enregistré son score</param>
        /// <param name="idjoueur">id du joueur</param>
        public Scoreboard(int points, int nbquestions, int numquiz, bool isInGame, int idjoueur)
        {
            InitializeComponent();
            this.isInGame = isInGame;
            this.points = points;
            this.nbquestions = nbquestions;
            this.numquiz = numquiz;
            this.idjoueur = idjoueur;
            // Récupère les 10 meilleurs joueurs
            getTop10();
        }
        #endregion

        #region Hover & Leave btnresult
        private void btnresult_MouseHover(object sender, EventArgs e)
        {
            btnresult.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnresult_MouseLeave(object sender, EventArgs e)
        {
            btnresult.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
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

        #region Méthode getTop10

        /// <summary>
        /// Méthode récupère les 10 meilleurs joueurs
        /// </summary>
        private void getTop10()
        {
            maConnexion = new connexion();
            unquiz = new quiz();
            UneConnexion = maConnexion.seConnecter();

            // Vérifie si on est connecté à la base de données
            if (UneConnexion != null)
            {
                DataSet topReponses = unquiz.getTop(numquiz, UneConnexion);

                // Affiche tout les scores du 1er au 10eme meilleur joueur
                for (int i = 0; i < topReponses.Tables["top"].Rows.Count; i++)
                {
                    int bonnerep = Int32.Parse(topReponses.Tables["top"].Rows[i][3].ToString());
                    double pourcentage = ((double)bonnerep / nbquestions) * 100;
                    double pourcentageArrondi = Math.Round(pourcentage, 0);
                    if (topReponses.Tables["top"].Rows[i][2].ToString() != "")
                    {
                        lbltop.Text += $"{i + 1}. {topReponses.Tables["top"].Rows[i][2]} : {pourcentageArrondi}%{Environment.NewLine}";
                    }
                    else
                    {
                        lbltop.Text += $"{i + 1}. {topReponses.Tables["top"].Rows[i][0]} {topReponses.Tables["top"].Rows[i][1]} : {pourcentageArrondi}%{Environment.NewLine}";
                    }
                }
            }
        }
        #endregion

        #region Evenement click btnResult


        /// <summary>
        /// Evenement click btnResult
        /// </summary>
        private void btnresult_Click(object sender, EventArgs e)
        {
            // Ferme la connexion
            if (UneConnexion != null)
            {
                maConnexion.seDeconnecter(UneConnexion);
            }

            // Vérifie si l'utilisateur est connecté avant d'accéder à la page de résultats
            if (idjoueur > 0)
            {
                this.Hide();
                Resultats resultats = new Resultats(points, nbquestions, numquiz, isInGame, idjoueur);
                resultats.ShowDialog();
                resultats.Closed += (s, args) => this.Close();
            }
            // Accède à la page de résulstats
            else
            {
                this.Hide();
                Resultats resultats = new Resultats(points, nbquestions, numquiz, isInGame);
                resultats.ShowDialog();
                resultats.Closed += (s, args) => this.Close();
            }
        }
        #endregion

        #region Evenement click btnAccueil
        private void btnAccueil_Click(object sender, EventArgs e)
        {
            // Ferme la connexion
            if (UneConnexion != null)
            {
                maConnexion.seDeconnecter(UneConnexion);
            }

            // Vérifie si l'utilisateur est connecté avant d'accéder à la page d'accueil
            if (idjoueur > 0)
            {
                this.Hide();
                Accueil accueil = new Accueil(idjoueur);
                accueil.ShowDialog();
                accueil.Closed += (s, args) => this.Close();
            }
            // Accède à la page d'accueil
            else
            {
                this.Hide();
                Accueil accueil = new Accueil();
                accueil.ShowDialog();
                accueil.Closed += (s, args) => this.Close();
            }
        }
        #endregion
    }
}
