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
    public partial class Theme : Form
    {
        #region Variables
        private String _difficulte;
        private int idJoueur;
        #endregion

        #region Constructeurs

        /// <summary>
        /// Constructeur de la classe theme
        /// </summary>
        /// <param name="difficulte">Difficulté du thème</param>
        public Theme(String difficulte)
        {
            InitializeComponent();
            _difficulte = difficulte;
        }

        /// <summary>
        /// Constructeur de la classe theme
        /// </summary>
        /// <param name="difficulte">Difficulté du thème</param>
        /// <param name="idJoueur">Id du joueur</param>
        public Theme(String difficulte, int idJoueur)
        {
            InitializeComponent();
            _difficulte = difficulte;
            this.idJoueur = idJoueur;
        }
        #endregion

        #region Evenement click btnDifficulte

        /// <summary>
        /// Evenement du bouton permettant de revenir au choix de la difficulté
        /// </summary>
        private void btnDifficulte_Click(object sender, EventArgs e)
        {
            // Vérifie si l'utilisateur est connecté avant d'accéder à la page de difficulté
            if (idJoueur > 0)
            {
                this.Hide();
                Difficulte difficulte = new Difficulte(idJoueur);
                difficulte.ShowDialog();
                difficulte.Closed += (s, args) => this.Close();
            }

            // Accède à la page de difficulté
            else
            {
                this.Hide();
                Difficulte difficulte = new Difficulte();
                difficulte.ShowDialog();
                difficulte.Closed += (s, args) => this.Close();
            }
        }
        #endregion

        #region Evenement click btnSansTheme

        /// <summary>
        /// Evenement click du bouton sans thème
        /// </summary>
        private void btnSans_Click(object sender, EventArgs e)
        {
            // Donne le paramètre "sans thème" à la méthode choisirTheme
            choisirTheme("Mots de passes");
        }
        #endregion

        #region Evenement click btnSio

        /// <summary>
        /// Evenement click du bouton Sio
        /// </summary>
        private void btnSio_Click(object sender, EventArgs e)
        {
            // Donne le paramètre "Sio" à la méthode choisirTheme
            choisirTheme("Cyber-harcèlement");
        }
        #endregion

        #region Evenement click btnReseaux

        /// <summary>
        /// Evenement click du bouton Reseaux
        /// </summary>
        private void btnReseaux_Click(object sender, EventArgs e)
        {
            // Donne le paramètre "Reseaux" à la méthode choisirTheme
            choisirTheme("Réseaux Sociaux");
        }
        #endregion

        #region Evenement click btnVie

        /// <summary>
        /// Evenement click du bouton vie de tous les jours
        /// </summary>
        private void btnVie_Click(object sender, EventArgs e)
        {
            // Donne le paramètre "Vie de tous les jours" à la méthode choisirTheme
            choisirTheme("Culture Cyber");
        }
        #endregion

        #region Evenement click btnEntreprise

        /// <summary>
        /// Evenement click du bouton Entreprise
        /// </summary>
        private void btnEntreprise_Click(object sender, EventArgs e)
        {
            // Donne le paramètre "Entreprise" à la méthode choisirTheme
            choisirTheme("Entreprise");
        }
        #endregion

        #region Methode choisirTheme

        /// <summary>
        /// Méthode choisissant le thème
        /// </summary>
        /// <param name="theme">Thème des questions</param>
        private void choisirTheme(String theme)
        {

            // Vérifie si le joueur est connecté avant d'accéder à la page quiz
            if (idJoueur > 0)
            {
                this.Hide();
                Quiz quiz = new Quiz(_difficulte, theme, idJoueur);
                quiz.ShowDialog();
                quiz.Closed += (s, args) => this.Close();
            }
            // Accède à la page quiz
            else
            {
                this.Hide();
                Quiz quiz = new Quiz(_difficulte, theme);
                quiz.ShowDialog();
                quiz.Closed += (s, args) => this.Close();
            }
        }
        #endregion

        #region Hover & Leave btnEntreprise
        private void btnEntreprise_MouseHover(object sender, EventArgs e)
        {
            btnEntreprise.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }



        private void btnEntreprise_MouseLeave(object sender, EventArgs e)
        {
            btnEntreprise.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Hover & Leave btnVie
        private void btnVie_MouseHover(object sender, EventArgs e)
        {
            btnVie.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnVie_MouseLeave(object sender, EventArgs e)
        {
            btnVie.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Hover & Leave btnReseaux
        private void btnReseaux_MouseHover(object sender, EventArgs e)
        {
            btnReseaux.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnReseaux_MouseLeave(object sender, EventArgs e)
        {
            btnReseaux.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Hover & Leave btnSio
        private void btnSio_MouseHover(object sender, EventArgs e)
        {
            btnSio.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnSio_MouseLeave(object sender, EventArgs e)
        {
            btnSio.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }

        #endregion

        #region Hover & Leave btnSans
        private void btnSans_MouseHover(object sender, EventArgs e)
        {
            btnSans.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnSans_MouseLeave(object sender, EventArgs e)
        {
            btnSans.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Hover & Leave btnDifficulte
        private void btnDifficulte_MouseHover(object sender, EventArgs e)
        {
            btnDifficulte.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnDifficulte_MouseLeave(object sender, EventArgs e)
        {
            btnDifficulte.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion
    }
}
