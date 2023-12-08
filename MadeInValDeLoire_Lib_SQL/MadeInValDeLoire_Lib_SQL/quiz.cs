using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MadeInValDeLoire_Lib_SQL
{
    public class quiz
    {
        #region Méthode getQuiz

        /// <summary>
        /// Méthode permettant de récupérer le quiz
        /// </summary>
        /// <param name="Difficulte">Difficulté du quiz</param>
        /// <param name="Theme">Thème du quiz</param>
        /// <param name="connexion">Connexion à la base de données</param>
        /// <returns>Le dataSet</returns>
        public DataSet getQuiz(String Difficulte, String Theme, MySqlConnection connexion)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.Connection = connexion;
            cmd.CommandText = "proc_getQuiz";
            cmd.CommandType = CommandType.StoredProcedure;

            // Transfère la difficulté et le thème vers des variables utilisable dans la fonction sql
            MySqlParameter unedifficulte = new MySqlParameter("@Difficulte", MySqlDbType.VarChar);
            MySqlParameter untheme = new MySqlParameter("@Theme", MySqlDbType.VarChar);
            unedifficulte.Value = Difficulte;
            untheme.Value = Theme;
            cmd.Parameters.Add(unedifficulte);
            cmd.Parameters.Add(untheme);

            da.Fill(ds, "quiz");

            return ds;
        }
        #endregion

        #region Méthode getQuestions


        /// <summary>
        /// Méthode permettant de récupérer la question
        /// </summary>
        /// <param name="Quiz">Nom du quiz</param>
        /// <param name="connexion">Connexion à la base de données</param>
        /// <returns>Retourne le dataset</returns>
        public DataSet getQuestions(String Quiz, MySqlConnection connexion)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.Connection = connexion;
            cmd.CommandText = "proc_getQuestions";
            cmd.CommandType = CommandType.StoredProcedure;

            // Transfère le nom du quiz vers une variable utilisable dans la fonction sql
            MySqlParameter unQuiz = new MySqlParameter("@Quiz", MySqlDbType.VarChar);
            unQuiz.Value = Quiz;
            cmd.Parameters.Add(unQuiz);

            da.Fill(ds, "questions");

            return ds;
        }
        #endregion

        #region Méthode getReponses


        /// <summary>
        /// Méthode permettant de récupérer les réponses
        /// </summary>
        /// <param name="Question">Question dans lequel ce trouve ces réponses</param>
        /// <param name="connexion">Connexion à la base de données</param>
        /// <returns>Retourne le dataset</returns>
        public DataSet getReponses(String Question, MySqlConnection connexion)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.Connection = connexion;
            cmd.CommandText = "proc_getReponses";
            cmd.CommandType = CommandType.StoredProcedure;

            // Transfère la question vers une variable utilisable dans la fonction sql
            MySqlParameter uneQuestion = new MySqlParameter("@Question", MySqlDbType.VarChar);
            uneQuestion.Value = Question;
            cmd.Parameters.Add(uneQuestion);

            da.Fill(ds, "reponses");

            return ds;
        }
        #endregion

        #region Méthode addResult


        /// <summary>
        /// Méthode permettant d'ajouter les résultats
        /// </summary>
        /// <param name="numeroQuiz">numéro du quiz</param>
        /// <param name="idJoueur">Id du joueur</param>
        /// <param name="nbBonnesRep">Nombre de bonnes réponses du joueurs</param>
        /// <param name="connexion">Connexion à la base de données</param>
        /// <returns>Retourne l'objet resultAjout</returns>
        public object addResult(int numeroQuiz, int idJoueur, int nbBonnesRep, MySqlConnection connexion)
        {
            MySqlCommand cmdFunc = new MySqlCommand("SELECT func_addResult(@numeroQuiz, @idJoueur, @nbBonnesRep)");

            cmdFunc.Connection = connexion;

            // Transfère le numéro du quiz, l'id du joueur ainsi que le nombre de bonne réponses vers des variables utilisables dans la fonction sql
            MySqlParameter unQuiz = new MySqlParameter("@numeroQuiz", MySqlDbType.Int32);
            MySqlParameter unJoueur = new MySqlParameter("@idJoueur", MySqlDbType.Int32);
            MySqlParameter uneBonneReponse = new MySqlParameter("@nbBonnesRep", MySqlDbType.Int32);

            unQuiz.Value = numeroQuiz;
            unJoueur.Value = idJoueur;
            uneBonneReponse.Value = nbBonnesRep;

            cmdFunc.Parameters.Add(unQuiz);
            cmdFunc.Parameters.Add(unJoueur);
            cmdFunc.Parameters.Add(uneBonneReponse);

            object resultAjout = cmdFunc.ExecuteScalar();

            return resultAjout;
        }
        #endregion

        #region Méthode addPropositions


        /// <summary>
        /// Méthode ajoutant des proposition
        /// </summary>
        /// <param name="libelle">libelle de la proposition</param>
        /// <param name="connexion">Connexion à la base de données</param>
        /// <returns>Retourne l'objet resultAjout</returns>
        public object addPropositions(String libelle, MySqlConnection connexion)
        {
            MySqlCommand cmdFunc = new MySqlCommand("SELECT func_addPropositions(@libelle)");

            cmdFunc.Connection = connexion;

            // Transfère la libelle de la proposition vers des variables utilisables dans la fonction sql
            MySqlParameter uneLibelle = new MySqlParameter("@libelle", MySqlDbType.VarChar);

            uneLibelle.Value = libelle;

            cmdFunc.Parameters.Add(uneLibelle);

            object resultAjout = cmdFunc.ExecuteScalar();

            return resultAjout;
        }
        #endregion

        #region Methode addQuestion


        /// <summary>
        /// Méthode permettant d'ajouter une question
        /// </summary>
        /// <param name="question">La Question</param>
        /// <param name="lvlDifficulte">Niveau de difficulté de la question</param>
        /// <param name="theme">Thème de la question</param>
        /// <param name="connexion">Connexion à la bdd</param>
        /// <returns>Retourne l'objet resultAjout</returns>
        public object addQuestion(String question, String lvlDifficulte, int theme, MySqlConnection connexion)
        {
            MySqlCommand cmdFunc = new MySqlCommand("SELECT func_addQuestion(@question, @lvlDifficulte, @letheme)");

            cmdFunc.Connection = connexion;

            // Transfère la question, le niveau de difficulté et le thème vers des variables utilisables dans la fonction sql
            MySqlParameter uneQuestion = new MySqlParameter("@question", MySqlDbType.VarChar);
            MySqlParameter uneDifficulte = new MySqlParameter("@lvlDifficulte", MySqlDbType.VarChar);
            MySqlParameter unTheme = new MySqlParameter("@letheme", MySqlDbType.VarChar);

            uneQuestion.Value = question;
            uneDifficulte.Value = lvlDifficulte;
            unTheme.Value = theme;

            cmdFunc.Parameters.Add(uneQuestion);
            cmdFunc.Parameters.Add(uneDifficulte);
            cmdFunc.Parameters.Add(unTheme);

            object resultAjout = cmdFunc.ExecuteScalar();

            return resultAjout;
        }
        #endregion

        #region Methode addQuestionQuiz


        /// <summary>
        /// Méthode permettant d'ajouter une question
        /// </summary>
        /// <param name="numeroQuestion">Numéro à la question</param>
        /// <param name="numeroQuiz">Numéro du quiz</param>
        /// <param name="connexion">Connexio à la bdd</param>
        /// <returns></returns>
        public object addQuestionQuiz(int numeroQuestion, int numeroQuiz, MySqlConnection connexion)
        {
            MySqlCommand cmdFunc = new MySqlCommand("SELECT func_addQuestionQuiz(@numeroQuestion, @numeroQuiz)");

            cmdFunc.Connection = connexion;

            // Transfère le numéro de la question ainsi que le numéro du quiz vers des variables utilisables dans la fonction sql
            MySqlParameter uneQuestion = new MySqlParameter("@numeroQuestion", MySqlDbType.Int32);
            MySqlParameter unQuiz = new MySqlParameter("@numeroQuiz", MySqlDbType.Int32);

            uneQuestion.Value = numeroQuestion;
            unQuiz.Value = numeroQuiz;

            cmdFunc.Parameters.Add(uneQuestion);
            cmdFunc.Parameters.Add(unQuiz); 

            object resultAjout = cmdFunc.ExecuteScalar();

            return resultAjout;
        }
        #endregion

        #region Methode addQuiz


        /// <summary>
        /// Méthode permettant d'ajouter un quiz
        /// </summary>
        /// <param name="titre">titre du quiz</param>
        /// <param name="nombreQuestions">Nombre de question</param>
        /// <param name="niveauDifficulte">Niveau de difficulté</param>
        /// <param name="theme">Thème du quiz</param>
        /// <param name="connexion">Connexion à la bdd</param>
        /// <returns>Retourne l'objet resultAjout</returns>
        public object addQuiz(string titre, int nombreQuestions, string niveauDifficulte, int theme, MySqlConnection connexion)
        {
            MySqlCommand cmdFunc = new MySqlCommand("SELECT func_addQuiz(@titre, @nombreQuestions,@niveauDifficulte, @theme)");
            MySqlDataAdapter da = new MySqlDataAdapter(cmdFunc);


            cmdFunc.Connection = connexion;

            // Transfère le titre, le nombre de questions, le niveau et le thème vers des variables utilisables dans la fonction sql
            MySqlParameter unTitre = new MySqlParameter("@titre", MySqlDbType.VarChar);
            MySqlParameter unNbQuestion = new MySqlParameter("@nombreQuestions", MySqlDbType.VarChar);
            MySqlParameter unNivDifficulte = new MySqlParameter("@niveauDifficulte", MySqlDbType.VarChar);
            MySqlParameter unTheme = new MySqlParameter("@theme", MySqlDbType.VarChar);


            unTitre.Value = titre;
            unNbQuestion.Value = nombreQuestions;
            unNivDifficulte.Value = niveauDifficulte;
            unTheme.Value = theme;


            cmdFunc.Parameters.Add(unTitre);
            cmdFunc.Parameters.Add(unNbQuestion);
            cmdFunc.Parameters.Add(unNivDifficulte);
            cmdFunc.Parameters.Add(unTheme);

            object resultAjout = cmdFunc.ExecuteScalar();

            return resultAjout;
        }
        #endregion

        #region Methode addReponse


        /// <summary>
        /// Méthode permettant d'ajouter une réponse
        /// </summary>
        /// <param name="numeroQuestion">Numéro de la question</param>
        /// <param name="numeroProposition">Numéro de la proposition</param>
        /// <param name="bonneReponse">Bonne réponse à la question</param>
        /// <param name="connexion">Connexion à la bdd</param>
        /// <returns>Retourne l'objet resultAjout</returns>
        public object addReponse(int numeroQuestion, int numeroProposition, int bonneReponse, MySqlConnection connexion)
        {
            MySqlCommand cmdFunc = new MySqlCommand("SELECT func_addReponse(@numeroQuestion, @numeroProposition, @bonneReponse)");
            cmdFunc.Connection = connexion;

            // Transfère le numéro de la question, le numéro de la proposition ainsi que si il s'agit d'une bonne réponse
            // vers des variables utilisables dans la fonction sql
            MySqlParameter uneQuestion = new MySqlParameter("@numeroQuestion", MySqlDbType.Int32);
            MySqlParameter unNumero = new MySqlParameter("@numeroProposition", MySqlDbType.Int32);
            MySqlParameter uneBonneReponse = new MySqlParameter("@bonneReponse", MySqlDbType.Int32);

            uneQuestion.Value = numeroQuestion;
            unNumero.Value = numeroProposition;
            uneBonneReponse.Value = bonneReponse;

            cmdFunc.Parameters.Add(uneQuestion);
            cmdFunc.Parameters.Add(unNumero);
            cmdFunc.Parameters.Add(uneBonneReponse);

            object resultAjout = cmdFunc.ExecuteScalar();

            return resultAjout;
        }
        #endregion

        #region Methode getTop


        /// <summary>
        /// Méthode permettant d'avoir les meilleur joueurs
        /// </summary>
        /// <param name="numQuiz">Numéro du quiz</param>
        /// <param name="connexion">connexion à la base de données</param>
        /// <returns>Retourne le dataset</returns>
        public DataSet getTop(int numQuiz, MySqlConnection connexion)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.Connection = connexion;
            cmd.CommandText = "proc_getTop";
            cmd.CommandType = CommandType.StoredProcedure;

            // Transfère le numéro du quiz vers des variables utilisables dans la procédure sql
            MySqlParameter unquiz = new MySqlParameter("@numeroQuiz", MySqlDbType.Int32);

            unquiz.Value = numQuiz;

            cmd.Parameters.Add(unquiz);

            da.Fill(ds, "top");

            return ds;
        }
        #endregion

        #region Methode getQuizList


        /// <summary>
        /// Méthode permettant de récupérer la liste des quiz
        /// </summary>
        /// <param name="connexion">connexion à la base de données</param>
        /// <returns>Retourne un dataset</returns>
        public DataSet getQuizList(MySqlConnection connexion)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.Connection = connexion;
            cmd.CommandText = "proc_getQuizList";
            cmd.CommandType = CommandType.StoredProcedure;

            da.Fill(ds, "listquiz");

            return ds;
        }
        #endregion

        #region Methode getQuizInfo


        /// <summary>
        /// Méthode permettant de récupérer les informations sur le quiz
        /// </summary>
        /// <param name="nomquiz">Nom du quiz</param>
        /// <param name="connexion">Connexionà la base de données</param>
        /// <returns>Retourne le dataset</returns>
        public DataSet getQuizInfo(String nomquiz, MySqlConnection connexion)
        {
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();

            cmd.Connection = connexion;
            cmd.CommandText = "proc_getQuizInfo";
            cmd.CommandType = CommandType.StoredProcedure;

            // Transfère le nomdu quiz vers des variables utilisables dans la procédure sql
            MySqlParameter unNom = new MySqlParameter("@nom", MySqlDbType.VarChar);

            unNom.Value = nomquiz;

            cmd.Parameters.Add(unNom);

            da.Fill(ds, "infoquiz");

            return ds;
        }
        #endregion
    }
}
