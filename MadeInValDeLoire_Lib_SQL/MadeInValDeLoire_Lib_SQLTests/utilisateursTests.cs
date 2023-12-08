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
    public class utilisateursTests
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
            maTestConnexion = new connexion();
            UneTestConnexion = maTestConnexion.seConnecter();
            if (UneTestConnexion == null)
            {
                Assert.Fail("La connexion à la base de données a échoué.");
            }
        }
        #endregion

        #region TestMethode logIn


        /// <summary>
        /// Test permettant de savoir si la méthode logIn marche
        /// </summary>
        [TestMethod()]
        public void logInTest()
        {
            utilisateurs utils = new utilisateurs();
            DataSet result = utils.logIn("Petit", "Kalvin", "mdptest", UneTestConnexion);
            // Vérifie si l'utilisateur est dans la bdd
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Tables.Contains("login"));
            Assert.IsTrue(result.Tables["login"].Rows.Count > 0);
        }
        #endregion

        #region TestMethode createUser


        /// <summary>
        /// Test permettant de savoir si la méthode createUser marche
        /// </summary>
        [TestMethod()]
        public void createUserTest()
        {
            string nom = "Ben";
            string prenom = "Ben";
            string login = "Ben10";
            string motdepasse = "Ben#60";

            utilisateurs utils = new utilisateurs();


            object result = utils.createUser(nom, prenom, login, motdepasse, UneTestConnexion);
            // Vérifie si l'utilisateur crée est dans la bdd
            Assert.IsNotNull(result);
            Assert.AreEqual(result, 31);
        }
        #endregion

        #region TestMethode isAdmin


        /// <summary>
        /// Test permettant de savoir si la méthode isAdmin marche
        /// </summary>
        [TestMethod()]
        public void isAdminTest()
        {
            utilisateurs utils = new utilisateurs();


            object result = utils.isAdmin(24, UneTestConnexion);
            // Vérifie qu'il ne s'agit pas d'un admin
            Assert.IsNotNull(result);
            Assert.AreEqual(result, false);
        }
        #endregion

        #region TestMethode getNonAdminList


        /// <summary>
        /// Test permettant de savoir si la méthode getNonAdminList marche
        /// </summary>
        [TestMethod()]
        public void getNonAdminListTest()
        {
            utilisateurs utils = new utilisateurs();
            DataSet result = utils.getNonAdminList(UneTestConnexion);

            Assert.IsNotNull(result);
            // Vérifie qu'il s'agit bien de la liste de non admin
            Assert.IsTrue(result.Tables.Contains("listnonadmin"));
            Assert.IsTrue(result.Tables["listnonadmin"].Rows.Count > 0);
        }
        #endregion

        #region TestMethode addAdmin


        /// <summary>
        /// Test permettant de savoir si la méthode addAdmin marche
        /// </summary>
        [TestMethod()]
        public void addAdminTest()
        {
            utilisateurs utils = new utilisateurs();


            object result = utils.addAdmin("bob", "bob", UneTestConnexion);
            // Vérifie que l'admin mise dans l'objet result est bien ajouté
            Assert.IsNotNull(result);
            Assert.AreEqual(result, "valide");
        }
        #endregion
    }
}