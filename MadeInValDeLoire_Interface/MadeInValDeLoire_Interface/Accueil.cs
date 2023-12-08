using MadeInValDeLoire_Lib_SQL;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Crmf;
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
    public partial class Accueil : Form
    {
        #region variables
        private int idJoueur;
        private utilisateurs utils;
        private connexion maConnexion;
        private MySqlConnection UneConnexion;
        #endregion

        #region Constructeurs


        /// <summary>
        /// Constructeurs de la classe accueil
        /// </summary>
        public Accueil()
        {
            InitializeComponent();
            btnadmin.Visible = false;
        }


        /// <summary>
        /// Constructeurs de la classe accueil
        /// </summary>
        /// <param name="idJoueur">id du joueur</param>
        public Accueil(int idJoueur)
        {
            InitializeComponent();
            this.idJoueur = idJoueur;
            // Vérifie si le joueur est connecté
            if(idJoueur > 0)
            {
               
                btninscription.Text = "Se déconnecter";
                maConnexion = new connexion();
                utils = new utilisateurs();
                UneConnexion = maConnexion.seConnecter();
                if (UneConnexion != null)
                {
                    object result = utils.isAdmin(idJoueur, UneConnexion);

                    // Vérifie si l'utilisateur est un admin
                    if(Boolean.Parse(result.ToString()) == false)
                    {
                        btnadmin.Visible = false;
                    }
                }
            }
        }
        #endregion

        #region Evenement click btn_jouer


        /// <summary>
        /// Evenement click du bouton jouer
        /// </summary>
        private void btnjouer_Click(object sender, EventArgs e)
        {
            // Vérifie si l'utilisateur est connecté
            if (idJoueur > 0)
            {
                // Ferme la connexion si celle-ci n'est pas null et que le joueur est connecté
                if (UneConnexion != null)
                {
                    maConnexion.seDeconnecter(UneConnexion);
                }
                this.Hide();
                Difficulte difficultes = new Difficulte(idJoueur);
                difficultes.ShowDialog();
                difficultes.Closed += (s, args) => this.Close();
            }
            else
            {
                // Ferme la connexion si celle-ci n'est pas null et que personne est connecté
                if (UneConnexion != null)
                {
                    maConnexion.seDeconnecter(UneConnexion);
                }
                this.Hide();
                Difficulte difficultes = new Difficulte();
                difficultes.ShowDialog();
                difficultes.Closed += (s, args) => this.Close();
            }
        }
        #endregion

        #region Evenement click btnClose

        /// <summary>
        /// Evenement click du bouton close
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // Ferme la connexion avant de fermer le jeu
            if (UneConnexion != null)
            {
                maConnexion.seDeconnecter(UneConnexion);
            }
            Application.Exit();
        }
        #endregion

        #region Evenement click btninscription

        /// <summary>
        /// Evenement click du bouton inscription
        /// </summary>
        private void btninscription_Click(object sender, EventArgs e)
        {
            // Ferme la connexion avant d'aller sur la page inscription 
            if (UneConnexion != null)
            {
                maConnexion.seDeconnecter(UneConnexion);
            }
            this.Hide();
            Inscription inscription = new Inscription();
            inscription.ShowDialog();
            inscription.Closed += (s, args) => this.Close();
        }
        #endregion

        #region Evenement click btnregles


        /// <summary>
        /// Evenement click du bouton regles
        /// </summary>
        private void btnregles_Click(object sender, EventArgs e)
        {
            // Ferme la connexion avant d'acceder à la page des règles si le joueur est connecté
            if (idJoueur > 0)
            {
                if (UneConnexion != null)
                {
                    maConnexion.seDeconnecter(UneConnexion);
                }
                this.Hide();
                Regles regles = new Regles(idJoueur);
                regles.ShowDialog();
                regles.Closed += (s, args) => this.Close();
            }
            // Ferme la connexion avant d'acceder à la page des règles si le joueur n'est pas connecté
            else
            {
                
                if (UneConnexion != null)
                {
                    maConnexion.seDeconnecter(UneConnexion);
                }
                this.Hide();
                Regles regles = new Regles();
                regles.ShowDialog();
                regles.Closed += (s, args) => this.Close();
            }
        }
        #endregion

        #region Evenement click btnadmin

        /// <summary>
        /// Evenement click du bouton administrateur
        /// </summary>
        private void btnadmin_Click(object sender, EventArgs e)
        {
            // Ferme la connexion avant d'acceder à la page admin
            if (UneConnexion != null)
            {
                maConnexion.seDeconnecter(UneConnexion);
            }
            this.Hide();
            Administrateur admin = new Administrateur(idJoueur);
            admin.ShowDialog();
            admin.Closed += (s, args) => this.Close();
        }
        #endregion

        #region Hover & Leave btnJouer
        private void btnjouer_MouseLeave(object sender, EventArgs e)
        {
            btnjouer.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }

        private void btnjouer_MouseHover(object sender, EventArgs e)
        {
            btnjouer.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }
        #endregion

        #region Hover & Leave btnregles
        private void btnregles_MouseHover(object sender, EventArgs e)
        {
            btnregles.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnregles_MouseLeave(object sender, EventArgs e)
        {
            btnregles.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Hover & Leave btninscription
        private void btninscription_MouseHover(object sender, EventArgs e)
        {
            btninscription.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btninscription_MouseLeave(object sender, EventArgs e)
        {
            btninscription.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Hover & Leave btnClose
        private void btnClose_MouseHover(object sender, EventArgs e)
        {
            btnClose.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion

        #region Hover & Leave btnadmin
        private void btnadmin_MouseHover(object sender, EventArgs e)
        {
            btnadmin.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros_hover;
        }

        private void btnadmin_MouseLeave(object sender, EventArgs e)
        {
            btnadmin.BackgroundImage = MadeInValDeLoire_Interface.Properties.Resources.bouton_vièrge_gros__je_ne_suis_pas_grossophobe_mais____;
        }
        #endregion
    }
}
