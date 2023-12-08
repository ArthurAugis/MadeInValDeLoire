using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadeInValDeLoire_Lib_SQL
{
    public class utilisateurs
    {
        #region Méthode logIn


        /// <summary>
        /// Méthode permettant de se connecter grâce au nom, prénom et le mot de passe
        /// </summary>
        /// <param name="nom">Nom de l'utilisateur</param>
        /// <param name="prenom">Prénom de l'utilisateur</param>
        /// <param name="mdp">Mot de passe</param>
        /// <param name="connexion">Connexion à la base de données</param>
        /// <returns>Retourne le dataset</returns>
        public DataSet logIn(String nom, String prenom, String mdp, MySqlConnection connexion)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.Connection = connexion;
            cmd.CommandText = "proc_login";
            cmd.CommandType = CommandType.StoredProcedure;

            // Transfère le nom, le prénom ainsi que le pot de passede l'utilisateur vers des variables utilisables dans la fonction sql
            MySqlParameter unnom = new MySqlParameter("@nom", MySqlDbType.VarChar);
            MySqlParameter unprenom = new MySqlParameter("@prenom", MySqlDbType.VarChar);
            MySqlParameter unmotdepasse = new MySqlParameter("@motDePasse", MySqlDbType.VarChar);
            unnom.Value = nom;
            unprenom.Value = prenom;
            unmotdepasse.Value = mdp;
            cmd.Parameters.Add(unnom);
            cmd.Parameters.Add(unprenom);
            cmd.Parameters.Add(unmotdepasse);

            da.Fill(ds, "login");

            return ds;
        }
        #endregion

        #region Methode createUser


        /// <summary>
        /// Méthode permettant de crée l'utilisateur
        /// </summary>
        /// <param name="nom">Nom de l'utilisateur</param>
        /// <param name="prenom">Prénom de l'utilisateur</param>
        /// <param name="login">Login de l'utilisateur</param>
        /// <param name="motdepasse">Mot de passe de l'utilisateur</param>
        /// <param name="connexion">Connexion à la base de données</param>
        /// <returns>retourne l'objet resultAjout</returns>
        public object createUser(string nom, string prenom, string login, string motdepasse, MySqlConnection connexion)
        {
            MySqlCommand cmdFunc = new MySqlCommand("SELECT func_createUser(@nom, @prenom, @login, @motdepasse)");

            cmdFunc.Connection = connexion;

            // Transfère le nom, prénom, le login ainsi que le mot de passe de l'utilisateur vers des variables utilisables dans la fonction sql
            MySqlParameter unNom = new MySqlParameter("@nom", MySqlDbType.VarChar);
            MySqlParameter unPrenom = new MySqlParameter("@prenom", MySqlDbType.VarChar);
            MySqlParameter unLogin = new MySqlParameter("@login", MySqlDbType.VarChar);
            MySqlParameter unMotDePasse = new MySqlParameter("@motdepasse", MySqlDbType.VarChar);

            unNom.Value = nom;
            unPrenom.Value = prenom;
            unLogin.Value = login;
            unMotDePasse.Value = motdepasse;

            cmdFunc.Parameters.Add(unNom);
            cmdFunc.Parameters.Add(unPrenom);
            cmdFunc.Parameters.Add(unLogin);
            cmdFunc.Parameters.Add(unMotDePasse);

            object resultAjout = cmdFunc.ExecuteScalar();

            return resultAjout;
        }
        #endregion

        #region Methode isAdmin


        /// <summary>
        /// Methode permettant de déterminer si il s'agit d'un admin
        /// </summary>
        /// <param name="idJoueur">id du joueur</param>
        /// <param name="connexion">Connexion à la bdd</param>
        /// <returns>Retourne l'objet resultAjout</returns>
        public object isAdmin(int idJoueur, MySqlConnection connexion)
        {
            MySqlCommand cmdFunc = new MySqlCommand("SELECT func_isAdmin(@idJoueur)");

            cmdFunc.Connection = connexion;

            // Transfère l'id du joueur vers des variables utilisables dans la fonction sql
            MySqlParameter unJoueur = new MySqlParameter("@idJoueur", MySqlDbType.VarChar);

            unJoueur.Value = idJoueur;

            cmdFunc.Parameters.Add(unJoueur);

            object resultAjout = cmdFunc.ExecuteScalar();

            return resultAjout;
        }
        #endregion

        #region Methode getyNonAdminList


        /// <summary>
        /// Méthode permettant d'afficher tout les utilisateur non admin
        /// </summary>
        /// <param name="connexion">Connexion à la bdd</param>
        /// <returns>Retourne le dataset</returns>
        public DataSet getNonAdminList(MySqlConnection connexion)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.Connection = connexion;
            cmd.CommandText = "proc_getNonAdminList";
            cmd.CommandType = CommandType.StoredProcedure;

            da.Fill(ds, "listnonadmin");

            return ds;
        }
        #endregion

        #region Methode addAdmin


        /// <summary>
        /// Méthode permettant d'ajouter un admin
        /// </summary>
        /// <param name="nom">Nom de l'utilisateur</param>
        /// <param name="prenom">Prénom de l'utlisateur</param>
        /// <param name="connexion">Connexion à la bdd</param>
        /// <returns>Retourne l'objet resultAjout</returns>
        public object addAdmin(String nom, String prenom, MySqlConnection connexion)
        {
            MySqlCommand cmdFunc = new MySqlCommand("SELECT func_addAdmin(@nom, @prenom)");

            cmdFunc.Connection = connexion;

            // Transfère le nom et le prénom de l'utilisateur vers des variables utilisables dans la fonction sql
            MySqlParameter unNom = new MySqlParameter("@nom", MySqlDbType.VarChar);
            MySqlParameter unPrenom = new MySqlParameter("@prenom", MySqlDbType.VarChar);

            unNom.Value = nom;
            unPrenom.Value = prenom;

            cmdFunc.Parameters.Add(unNom);
            cmdFunc.Parameters.Add(unPrenom);

            object resultAjout = cmdFunc.ExecuteScalar();

            return resultAjout;
        }
        #endregion
    }
}
