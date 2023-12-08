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
    public partial class Difficulte : Form
    {

        #region Variables
        private int idJoueur;
        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur de la classe difficulte
        /// </summary>
        public Difficulte()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructeur de la classe difficulte
        /// </summary>
        /// <param name="idJoueur">id du joueur</param>
        public Difficulte(int idJoueur)
        {
            InitializeComponent();
            this.idJoueur = idJoueur;
        }
        #endregion

        #region Evenement click btnFacile

        /// <summary>
        /// Evenement click du bouton facile
        /// </summary>
        private void btnFacile_Click(object sender, EventArgs e)
        {
            // Selectionne la difficulté facile
            choixDifficulte("Facile");
        }
        #endregion

        #region Evenement click btnDifficile

        /// <summary>
        /// Evenement click du bouton difficile
        /// </summary>
        private void btnDifficile_Click(object sender, EventArgs e)
        {
            // Selectionne la difficulté difficile
            choixDifficulte("Difficile");
        }
        #endregion

        #region Evenement click btnExpert

        /// <summary>
        /// Evenement click du bouton expert
        /// </summary>
        private void btnExpert_Click(object sender, EventArgs e)
        {
            // Selectionne la difficulté expert
            choixDifficulte("Expert");
        }
        #endregion

        #region Méthode choixDifficulte

        /// <summary>
        /// Méthode choisissant la difficulté
        /// </summary>
        /// <param name="difficulte">Difficulté des questions</param>
        private void choixDifficulte(String difficulte)
        {
            // Accède à la page des thèmes tout en fermant cette page
            // Vérifie que le joueur est connecté
            if (idJoueur > 0)
            {
                
                this.Hide();
                Theme themes = new Theme(difficulte, idJoueur);
                themes.ShowDialog();
                themes.Closed += (s, args) => this.Close();
            }
            // Accède à la page des thèmes tout en fermant cette page
            else
            {
                this.Hide();
                Theme themes = new Theme(difficulte);
                themes.ShowDialog();
                themes.Closed += (s, args) => this.Close();
            }
        }
        #endregion

        #region Evenement click btnAccueil

        /// <summary>
        /// Evenement click du bouton d'accueil
        /// </summary>
        private void btnAccueil_Click(object sender, EventArgs e)
        {
            // Accède à la page d'accueil tout en fermant cette page
            // Vérifie que le joueur est connecté
            if (idJoueur > 0)
            {
                this.Hide();
                Accueil accueil = new Accueil(idJoueur);
                accueil.ShowDialog();
                accueil.Closed += (s, args) => this.Close();
            }

            // Accède à la page d'accueil tout en fermant cette page
            else
            {
                this.Hide();
                Accueil accueil = new Accueil();
                accueil.ShowDialog();
                accueil.Closed += (s, args) => this.Close();
            }
        }
        #endregion



        #region Hover & Leave btnFacile
        private void btnFacile_MouseLeave(object sender, EventArgs e)
        {
            btnFacile.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_débutant;
        }

        private void btnFacile_MouseHover(object sender, EventArgs e)
        {
            btnFacile.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_débutant_blur;
        }
        #endregion

        #region Hover & Leave btnDifficile
        private void btnDifficile_MouseHover(object sender, EventArgs e)
        {
            btnDifficile.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_intermediaire_blur;
        }

        private void btnDifficile_MouseLeave(object sender, EventArgs e)
        {
            btnDifficile.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_intermediaire;
        }
        #endregion

        #region Hover & Leave btnExpert
        private void btnExpert_MouseHover(object sender, EventArgs e)
        {
            btnExpert.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_confirmé_blur;
        }

        private void btnExpert_MouseLeave(object sender, EventArgs e)
        {
            btnExpert.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_confirmé;
        }
        #endregion

        #region Hover & Leave btnAccueil
        private void btnAccueil_MouseLeave(object sender, EventArgs e)
        {
            btnAccueil.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }

        private void btnAccueil_MouseHover(object sender, EventArgs e)
        {
            btnAccueil.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }
        #endregion
    }
}
