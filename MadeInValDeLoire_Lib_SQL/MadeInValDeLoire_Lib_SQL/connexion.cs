using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadeInValDeLoire_Lib_SQL
{
    public class connexion
    {
        #region Variables
        private MySqlConnection connection;
        #endregion

        #region Methode Connexion

        /// <summary>
        /// Methode permettant de ce connecter à la base de données
        /// </summary>
        /// <returns>Return le résultat de la connexion, un objet MySqlConnection</returns>
        public MySqlConnection seConnecter()
        {
            // Essaie de ce connecter à la bdd
            try
            {
                connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["bddString"].ConnectionString);
                connection.Open();
            }
            // Montre un message d'erreur si cela ne marche pas
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                connection = null;
            }

            return connection;
        }
        #endregion

        #region Methode Deconnection

        /// <summary>
        /// Méthode permettant de ce déconnecter de la base de données
        /// </summary>
        /// <param name="connexion"></param>
        public void seDeconnecter(MySqlConnection connexion)
        {
            // Ferme la connexion à la base de données
            if (connexion != null)
            {
                connexion.Close();
            }
        }
        #endregion
    }
}
