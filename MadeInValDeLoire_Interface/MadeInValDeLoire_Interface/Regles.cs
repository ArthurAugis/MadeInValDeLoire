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
    public partial class Regles : Form
    {
        #region Variable
        private int idJoueur;
        #endregion

        #region Constructeur

        /// <summary>
        /// Constructeur de la classe regles
        /// </summary>
        public Regles()
        {
            InitializeComponent();
            idJoueur = -1;
        }

        /// <summary>
        /// Constructeur de la classe regles
        /// </summary>
        /// <param name="idJoueur">id du joueur</param>
        public Regles(int idJoueur)
        {
            InitializeComponent();
            this.idJoueur = idJoueur;
        }
        #endregion

        #region 3
        private void btnAccueil_Click(object sender, EventArgs e)
        {
            if (idJoueur > 0)
            {
                this.Hide();
                Accueil accueil = new Accueil(idJoueur);
                accueil.ShowDialog();
                accueil.Closed += (s, args) => this.Close();
            }
            else
            {
                this.Hide();
                Accueil accueil = new Accueil();
                accueil.ShowDialog();
                accueil.Closed += (s, args) => this.Close();
            }
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
    }
}
