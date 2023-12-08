using Microsoft.VisualStudio.TestTools.UnitTesting;
using MadeInValDeLoire_Lib_SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace MadeInValDeLoire_Lib_SQL.Tests
{
    [TestClass()]
    public class connexionTests
    {
        #region Variables
        private connexion maTestConnexion;
        private MySqlConnection UneTestConnexion;
        #endregion

        #region Connexion

        /// <summary>
        /// Méthode permettant de lancer la connexion à la bdd
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            // Lance la connexion vers la base de données
            maTestConnexion = new connexion();
            UneTestConnexion = maTestConnexion.seConnecter();
        }
        #endregion

        #region TestMethode seConnecter
        /// <summary>
        /// Test permettant de savoir si la méthode seConnecter marche
        /// </summary>
        [TestMethod()]
        public void seConnecterTest()
        {
            connexion connexion = new connexion();
            MySqlConnection UneConnexion = connexion.seConnecter();
            Assert.IsTrue(UneConnexion.State == ConnectionState.Open, "Connexion à la base de données réussie.");
        }
        #endregion

        #region TestMethode seDeconnecter

        /// <summary>
        /// Test permettant de savoir si la méthode seDeconnecter marche
        /// </summary>
        [TestMethod()]
        public void seDeconnecterTest()
        {
            Assert.IsTrue(UneTestConnexion.State == ConnectionState.Open, "Base de données bien connectée.");

            maTestConnexion.seDeconnecter(UneTestConnexion);

            Assert.IsTrue(UneTestConnexion.State == ConnectionState.Closed, "La connexion doit être fermée après la déconnexion.");
        }
        #endregion
    }
}