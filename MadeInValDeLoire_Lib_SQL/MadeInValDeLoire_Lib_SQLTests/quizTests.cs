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
    public class quizTests
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

        #region TestMethode getQuiz

        /// <summary>
        /// Test permettant de savoir si la méthode getQuiz marche
        /// </summary>
        [TestMethod()]
        public void getQuizTest()
        {
            string difficulty = "Difficile";
            string theme = "Sans thème";
            quiz unQuiz = new quiz();


            DataSet result = unQuiz.getQuiz(difficulty, theme, UneTestConnexion);

            // Vérifie si les valeurs mise dans la variable result sont dans la bdd
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Tables.Contains("quiz"));
            Assert.IsTrue(result.Tables["quiz"].Rows.Count > 0);
        }
        #endregion

        #region TestMethode getQuestion


        /// <summary>
        /// Test permettant de savoir si la méthode getQuestions marche
        /// </summary>
        [TestMethod()]
        public void getQuestionsTest()
        {
            string quiz = "testquiz";
            quiz unQuiz = new quiz();


            DataSet result = unQuiz.getQuestions(quiz, UneTestConnexion);

            // Vérifie si les valeurs mise dans la variable result sont dans la bdd
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Tables.Contains("questions"));
            Assert.IsTrue(result.Tables["questions"].Rows.Count > 0);
        }
        #endregion

        #region TestMethode getReponses


        /// <summary>
        /// Test permettant de savoir si la méthode getReponse marche
        /// </summary>
        [TestMethod()]
        public void getReponsesTest()
        {
            string question = "What's 9 + 10 ?";
            quiz unQuiz = new quiz();


            DataSet result = unQuiz.getReponses(question, UneTestConnexion);
            // Vérifie si les valeurs mise dans la variable result sont dans la bdd
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Tables.Contains("reponses"));
            Assert.IsTrue(result.Tables["reponses"].Rows.Count > 0);
        }
        #endregion

        #region TestMethode addPropositions


        /// <summary>
        /// Test permettant de savoir si la méthode addPropositions marche
        /// </summary>
        [TestMethod()]
        public void addPropositionsTest()
        {
            string unePropositions = "L'autre réponse 2";
            quiz unQuiz = new quiz();


            object result = unQuiz.addPropositions(unePropositions, UneTestConnexion);
            // Vérifie si les valeurs mise dans la variable result sont dans la bdd
            Assert.IsNotNull(result);
            Assert.AreEqual(result, 10);
        }
        #endregion

        #region TestMethode addQuestion


        /// <summary>
        /// Test permettant de savoir si la méthode addQuestion marche
        /// </summary>
        [TestMethod()]
        public void addQuestionTest()
        {
            string uneQuestion = "Comment2 ?";
            string uneDifficulte = "D";
            int unTheme = 1;
            quiz unQuiz = new quiz();


            object result = unQuiz.addQuestion(uneQuestion, uneDifficulte, unTheme, UneTestConnexion);

            // Vérifie si les valeurs mise dans la variable result sont dans la bdd
            Assert.IsNotNull(result);
            Assert.AreEqual(result, 6);
        }
        #endregion

        #region TestMethode addQuestionQuiz


        /// <summary>
        /// Test permettant de savoir si la méthode addQuestionQuiz marche
        /// </summary>
        [TestMethod()]
        public void addQuestionQuizTest()
        {
            int unNumeroQuestion = 1;
            int unNumeroQuiz = 3;
            quiz unQuiz = new quiz();


            object result = unQuiz.addQuestionQuiz(unNumeroQuestion, unNumeroQuiz, UneTestConnexion);


            // Vérifie si les valeurs mise dans la variable result sont dans la bdd
            Assert.IsNotNull(result);
            Assert.AreEqual(result, "valide");
        }
        #endregion

        #region TestMethode addQuiz


        /// <summary>
        /// Test permettant de savoir si la méthode addQuiz marche
        /// </summary>
        [TestMethod()]
        public void addQuizTest()
        {
            string unTitre = "Le Fameux titre";
            int unNbQuestion = 4;
            string uneDifficulte = "F";
            int unTheme = 5;
            quiz unQuiz = new quiz();


            object result = unQuiz.addQuiz(unTitre, unNbQuestion, uneDifficulte, unTheme, UneTestConnexion);
            // Vérifie si les valeurs mise dans la variable result sont dans la bdd
            Assert.IsNotNull(result);
            Assert.AreEqual(result, "valide");
        }
        #endregion

        #region TestMethode addReponse


        /// <summary>
        /// Test permettant de savoir si la méthode addReponse marche
        /// </summary>
        [TestMethod()]
        public void addReponseTest()
        {
            quiz unQuiz = new quiz();

            object result = unQuiz.addReponse(5, 9, 1, UneTestConnexion);
            // Vérifie si les valeurs mise dans la variable result sont dans la bdd
            Assert.IsNotNull(result);
            Assert.AreEqual(result, "valide");
        }
        #endregion

        #region TestMethode addResult


        /// <summary>
        /// Test permettant de savoir si la méthode addResult marche
        /// </summary>
        [TestMethod()]
        public void addResultTest()
        {
            quiz unQuiz = new quiz();

            object result = unQuiz.addResult(2, 24, 9, UneTestConnexion);
            // Vérifie si les valeurs mise dans la variable result sont dans la bdd
            Assert.IsNotNull(result);
            Assert.AreEqual(result, "valide");
        }
        #endregion

        #region TestMethode getTop


        /// <summary>
        /// Test permettant de savoir si la méthode getTop marche
        /// </summary>
        [TestMethod()]
        public void getTopTest()
        {
            quiz unQuiz = new quiz();


            DataSet result = unQuiz.getTop(2, UneTestConnexion);
            // Vérifie si les bonnes lignes sont récupéré
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Tables.Contains("top"));
            Assert.IsTrue(result.Tables["top"].Rows.Count > 0);
        }
        #endregion

        #region TestMethode getQuizList


        /// <summary>
        /// Test permettant de savoir si la méthode getQuizList marche
        /// </summary>
        [TestMethod()]
        public void getQuizListTest()
        {
            quiz unQuiz = new quiz();


            DataSet result = unQuiz.getQuizList(UneTestConnexion);
            // Vérifie si les bonnes lignes sont récupéré
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Tables.Contains("listquiz"));
            Assert.IsTrue(result.Tables["listquiz"].Rows.Count > 0);
        }
        #endregion

        #region TestMethode getQuizInfo


        /// <summary>
        /// Test permettant de savoir si la méthode getQuizInfo marche
        /// </summary>
        [TestMethod()]
        public void getQuizInfoTest()
        {
            quiz unQuiz = new quiz();


            DataSet result = unQuiz.getQuizInfo("testquiz",UneTestConnexion);
            // Vérifie si les bonnes lignes sont récupéré
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Tables.Contains("infoquiz"));
            Assert.IsTrue(result.Tables["infoquiz"].Rows.Count > 0);
        }
        #endregion
    }
}