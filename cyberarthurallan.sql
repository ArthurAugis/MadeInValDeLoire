-- phpMyAdmin SQL Dump
-- version 4.2.7.1
-- http://www.phpmyadmin.net
--
-- Client :  localhost
-- Généré le :  Lun 06 Novembre 2023 à 18:28
-- Version du serveur :  5.6.20-log
-- Version de PHP :  5.4.31

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Base de données :  `cyberarthurallan`
--
CREATE DATABASE IF NOT EXISTS `cyberarthurallan` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `cyberarthurallan`;

DELIMITER $$
--
-- Procédures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `proc_getNonAdminList`()
    NO SQL
BEGIN

SELECT quizjoueur.nom, quizjoueur.prenom
FROM quizjoueur
WHERE quizjoueur.isAdmin = 0;


END$$

CREATE DEFINER=`root`@`%` PROCEDURE `proc_getQuestions`(IN `Quiz` VARCHAR(50))
    NO SQL
BEGIN

SELECT question.libelle
FROM question
WHERE question.niveauDifficulte = 
(SELECT quiz.niveauDifficulte 
 FROM quiz
 WHERE quiz.titre = Quiz
)
AND question.theme = 
(SELECT quiz.theme 
 FROM quiz
 WHERE quiz.titre = Quiz
)
ORDER BY RAND()
LIMIT 15;

END$$

CREATE DEFINER=`root`@`%` PROCEDURE `proc_getQuiz`(IN `Difficulte` VARCHAR(11), IN `Theme` VARCHAR(50))
    NO SQL
BEGIN

SELECT quiz.titre, theme.libelle AS "Theme", niveaudifficulte.libelle AS "Difficulté", quiz.nombreQuestions, quiz.numero
FROM quiz
INNER JOIN theme ON theme.numero = quiz.theme
INNER JOIN niveaudifficulte ON niveaudifficulte.id = quiz.niveauDifficulte
WHERE theme.libelle = Theme
AND niveaudifficulte.libelle = Difficulte
ORDER BY RAND();


END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `proc_getQuizInfo`(IN `nom` VARCHAR(50))
    NO SQL
BEGIN

SELECT quiz.numero, quiz.niveauDifficulte, quiz.theme
FROM quiz
WHERE quiz.titre = nom;

END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `proc_getQuizList`()
    NO SQL
BEGIN

SELECT quiz.titre, theme.libelle AS "Theme", niveaudifficulte.libelle AS "Difficulté", quiz.nombreQuestions, quiz.numero
FROM quiz
INNER JOIN theme ON theme.numero = quiz.theme
INNER JOIN niveaudifficulte ON niveaudifficulte.id = quiz.niveauDifficulte;

END$$

CREATE DEFINER=`root`@`%` PROCEDURE `proc_getReponses`(IN `Question` VARCHAR(200))
    NO SQL
BEGIN

SELECT propositions.libelle, reponse.bonneReponse
FROM question
INNER JOIN reponse ON reponse.numeroQuestion = question.numero
INNER JOIN propositions ON propositions.numero = reponse.numeroProposition
WHERE question.libelle = Question;

END$$

CREATE DEFINER=`root`@`%` PROCEDURE `proc_getTop`(IN `numeroQuiz` INT)
BEGIN
   
    SELECT quizjoueur.nom, quizjoueur.prenom, quizjoueur.login, resultat.nbBonnesRep 
    FROM resultat
    INNER JOIN quizjoueur ON resultat.idJoueur = quizjoueur.id
    WHERE resultat.numeroQuiz = numeroQuiz 
    ORDER BY resultat.nbBonnesRep DESC 
    LIMIT 10;

END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `proc_login`(IN `nom` VARCHAR(50), IN `prenom` VARCHAR(50), IN `motDePasse` VARCHAR(255))
BEGIN
    DECLARE mdpSel VARCHAR(200);
    DECLARE motpasse VARCHAR(200);
    DECLARE lesel VARCHAR(8);

    SELECT sel INTO lesel
    FROM quizjoueur
    WHERE quizjoueur.nom = nom AND quizjoueur.prenom = prenom;

    SET motpasse = SHA2(motDePasse, 512);
    SET mdpSel = SHA2(CONCAT(lesel, motpasse), 512);

    SELECT id
    FROM quizjoueur
    WHERE quizjoueur.nom = nom
    AND quizjoueur.prenom = prenom
    AND quizjoueur.motPasse = mdpSel;
END$$

--
-- Fonctions
--
CREATE DEFINER=`root`@`localhost` FUNCTION `func_addAdmin`(`nom` VARCHAR(50), `prenom` VARCHAR(50)) RETURNS varchar(30) CHARSET utf8
    NO SQL
BEGIN
DECLARE retour varchar(30) DEFAULT "valide";


DECLARE CONTINUE HANDLER FOR 1062
	SET retour  = "cle_primaire";

DECLARE CONTINUE HANDLER FOR 1452
	SET retour  = "cle_etrangere";

    
UPDATE quizjoueur SET isAdmin = 1
WHERE quizjoueur.nom = nom
AND quizjoueur.prenom = prenom;
    
RETURN retour;

END$$

CREATE DEFINER=`root`@`%` FUNCTION `func_addPropositions`(`libelle` VARCHAR(200)) RETURNS int(11)
    NO SQL
BEGIN
    DECLARE retour INT;
    

    DECLARE CONTINUE HANDLER FOR 1062
         SET retour = -1062;

    DECLARE CONTINUE HANDLER FOR 1452
         SET retour = -1452;


    INSERT INTO propositions (libelle) VALUES (libelle);
	SET retour = LAST_INSERT_ID();
   
 
    RETURN retour;
END$$

CREATE DEFINER=`root`@`%` FUNCTION `func_addQuestion`(`question` VARCHAR(200), `lvlDifficulte` VARCHAR(1), `letheme` INT(20)) RETURNS int(11)
    NO SQL
BEGIN
DECLARE retour INT;


DECLARE CONTINUE HANDLER FOR 1062
	SET retour  = -1062;

DECLARE CONTINUE HANDLER FOR 1452
	SET retour  = -1452;


INSERT INTO question (question.libelle,question.niveaudifficulte,question.theme)
VALUES (question, lvlDifficulte, letheme);
SET retour = LAST_INSERT_ID();


RETURN retour;

END$$

CREATE DEFINER=`root`@`%` FUNCTION `func_addQuestionQuiz`(`numeroQuestion` INT, `numeroQuiz` INT) RETURNS varchar(30) CHARSET utf8
    NO SQL
BEGIN

DECLARE retour varchar(30) DEFAULT "valide";


DECLARE CONTINUE HANDLER FOR 1062
	SET retour  = "cle_primaire";

DECLARE CONTINUE HANDLER FOR 1452
	SET retour  = "cle_etrangere";
    

INSERT INTO questionquiz(numeroQuestion, numeroQuiz) VALUES (numeroQuestion, numeroQuiz);

UPDATE quiz 
SET quiz.nombreQuestions = quiz.nombreQuestions + 1;

    
RETURN retour;
    
END$$

CREATE DEFINER=`root`@`localhost` FUNCTION `func_addQuiz`(`quiz_titre` VARCHAR(50), `quiz_nombreQuestions` INT, `quiz_niveauDifficulte` VARCHAR(1), `quiz_theme` INT) RETURNS varchar(30) CHARSET utf8
    NO SQL
BEGIN
    DECLARE retour VARCHAR(30) DEFAULT "valide";

    DECLARE CONTINUE HANDLER FOR 1062
        SET retour = "cle_primaire";

    DECLARE CONTINUE HANDLER FOR 1452
        SET retour = "cle_etrangere";

    INSERT INTO quiz (titre, nombreQuestions, niveauDifficulte, theme)
    VALUES (quiz_titre, quiz_nombreQuestions, quiz_niveauDifficulte, quiz_theme);

    RETURN retour;
END$$

CREATE DEFINER=`root`@`%` FUNCTION `func_addReponse`(`numeroQuestion` INT(11), `numeroProposition` INT(11), `bonneReponse` TINYINT(1)) RETURNS varchar(30) CHARSET utf8
    NO SQL
BEGIN

DECLARE retour varchar(30) DEFAULT "valide";


DECLARE CONTINUE HANDLER FOR 1062
	SET retour  = "cle_primaire";

DECLARE CONTINUE HANDLER FOR 1452
	SET retour  = "cle_etrangere";


INSERT INTO reponse (numeroQuestion, numeroProposition, bonneReponse)
VALUES (numeroQuestion , numeroProposition,bonneReponse);
            

RETURN retour;
END$$

CREATE DEFINER=`root`@`%` FUNCTION `func_addResult`(`numeroQuiz` INT, `idJoueur` INT, `nbBonnesRep` INT) RETURNS varchar(30) CHARSET utf8
    NO SQL
BEGIN
DECLARE retour varchar(30) DEFAULT "valide";


DECLARE CONTINUE HANDLER FOR 1062
	SET retour  = "cle_primaire";

DECLARE CONTINUE HANDLER FOR 1452
	SET retour  = "cle_etrangere";

    
INSERT INTO resultat (numeroQuiz, idJoueur, nbBonnesRep)
VALUES (numeroQuiz, idJoueur, nbBonnesRep);
    
RETURN retour;

END$$

CREATE DEFINER=`root`@`%` FUNCTION `func_addSection`(`nom` VARCHAR(20), `annee` VARCHAR(20), `specialite` VARCHAR(50)) RETURNS varchar(30) CHARSET utf8
    NO SQL
BEGIN

DECLARE retour varchar(30) DEFAULT "valide";


DECLARE CONTINUE HANDLER FOR 1062
	SET retour  = "cle_primaire";

DECLARE CONTINUE HANDLER FOR 1452
	SET retour  = "cle_etrangere";
        

        
INSERT INTO section(nom, annee, specialite) 
VALUES (nom, annee, specialite);
            



    RETURN retour;
END$$

CREATE DEFINER=`root`@`localhost` FUNCTION `func_createUser`(`nom` VARCHAR(50), `prenom` VARCHAR(50), `login` VARCHAR(50), `mdp` VARCHAR(200)) RETURNS int(11)
    DETERMINISTIC
BEGIN
    DECLARE sel VARCHAR(8);
    DECLARE motdepasse VARCHAR(200);
    DECLARE mdpSel VARCHAR(200);
    DECLARE retour INT;

    SET sel = LEFT(UUID(), 8);
    SET motdepasse = SHA2(mdp, 512);
    SET mdpSel = SHA2(CONCAT(sel, motdepasse), 512);

    SELECT COUNT(*) INTO retour FROM quizjoueur WHERE quizjoueur.nom = nom AND quizjoueur.prenom = prenom;

    IF retour > 0 THEN
        SET retour = -2;
    ELSE
        IF (login IS NOT NULL AND login <> '') THEN
            SELECT COUNT(*) INTO retour FROM quizjoueur WHERE quizjoueur.login = login;
            IF retour > 0 THEN
                SET retour = -1;
            ELSE
                INSERT INTO quizjoueur (nom, prenom, login, sel, motPasse)
                VALUES (nom, prenom, login, sel, mdpSel);
                SET retour = LAST_INSERT_ID();
            END IF;
        ELSE
            INSERT INTO quizjoueur (nom, prenom, login, sel, motPasse)
            VALUES (nom, prenom, login, sel, mdpSel);
            SET retour = LAST_INSERT_ID();
        END IF;
    END IF;

    RETURN retour;
END$$

CREATE DEFINER=`root`@`%` FUNCTION `func_isAdmin`(`idJoueur` INT) RETURNS tinyint(1)
    NO SQL
BEGIN
    DECLARE boolAdmin BOOL;
    
    SELECT isAdmin INTO boolAdmin 
    FROM quizjoueur 
    WHERE quizjoueur.id = idJoueur;
    
    RETURN boolAdmin;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `niveaudifficulte`
--

CREATE TABLE IF NOT EXISTS `niveaudifficulte` (
  `id` varchar(1) NOT NULL COMMENT 'Clé primaire de chaque difficulté',
  `libelle` varchar(11) NOT NULL COMMENT 'libellé de la difficulté'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Contenu de la table `niveaudifficulte`
--

INSERT INTO `niveaudifficulte` (`id`, `libelle`) VALUES
('D', 'Difficile'),
('E', 'Expert'),
('F', 'Facile');

-- --------------------------------------------------------

--
-- Structure de la table `propositions`
--

CREATE TABLE IF NOT EXISTS `propositions` (
`numero` int(11) NOT NULL COMMENT 'numero de la proposition',
  `libelle` varchar(200) NOT NULL COMMENT 'libelle de la proposition'
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COMMENT='table stockant les propositions' AUTO_INCREMENT=750 ;

--
-- Contenu de la table `propositions`
--

INSERT INTO `propositions` (`numero`, `libelle`) VALUES
(1, ' je demande au responsable du réseau social concerné de le faire\r'),
(2, ' je vais régler la configuration de mon compte dans les paramètres\r'),
(3, ' ça se fait tout seul non ?\r'),
(4, ' je laisse la configuration par défaut car elle doit être bonne\r'),
(5, ' je dois faire quelque chose ?\r'),
(6, ' je change le mot de passe avec un mot de passe classique password par exemple pour ne pas l’oublier quand je reviendrais.\r'),
(7, 'je le garde comme il est'),
(8, ' je le supprime pour éviter que mon compte soit utilisé à mon insu ou que mes informations ne soient récupérées par des tiers\r'),
(9, ' il n’y aucun risque puisque internet c’est sécurisé.\r'),
(10, 'ça ne craint rien\r'),
(11, 'c’est se mettre en danger\r'),
(12, 'tout le monde le fait !\r'),
(13, 'oui\r'),
(14, 'non\r'),
(15, 'peut être, ça doit être important\r'),
(16, 'je ne sais pas\r'),
(17, 'Je fais preuve de discernement lorsque j’évoque mon travail car cela pourrait me porter préjudice ainsi qu’à mon entreprise\r'),
(18, 'Sur les réseaux sociaux, je peux m’exprimer sans crainte que mes propos soient interprétés ou rediffusés\r'),
(19, 'Je suis vigilant sur les contenus partagés par mes contacts, qui peuvent publier des contenus malveillants à leur insu\r'),
(20, 'Je vérifie régulièrement qu’il n’y a aucune connexion sur mon compte depuis un appareil inconnu\r'),
(21, '9 ans\r'),
(22, '11 ans\r'),
(23, '13 ans\r'),
(24, '15 ans\r'),
(25, 'Bien sûr puisqu''ils ont donné leurs noms ou leurs prénoms\r'),
(26, 'Oui ils sont tous mes amis\r'),
(27, 'Snapchat\r'),
(28, 'Instagram\r'),
(29, 'FaceBook\r'),
(30, 'Tik Tok\r'),
(31, 'Ma photo de profil\r'),
(32, 'Mon nom, mon prénom\r'),
(33, 'Mon pseudo\r'),
(34, 'Ben tout ça en fait !\r'),
(35, 'La minute qui suit la demande\r'),
(36, 'L’heure qui suit la demande\r'),
(37, 'Les 15 jours qui suivent la demande\r'),
(38, 'Les 30 jours qui suivent la demande\r'),
(39, 'Ce que j''aimerais que l''on dise de moi\r'),
(40, 'Ce que je publie, ce que ‘jaime’ , commente, etc.\r'),
(41, 'Que les commentaires désagréables me concernant\r'),
(42, 'Une nouvelle mode de défilé sur Internet\r'),
(43, '2 %\r'),
(44, '18 %\r'),
(45, '48 %\r'),
(46, '100 %\r'),
(47, 'Je ne pense pas que ça change grand chose\r'),
(48, 'Non, c’est un certificat pour l’utilisation du numérique\r'),
(49, 'Non, c’est un jeu de hasard\r'),
(50, 'Non, c’est une dédicace par l''image\r'),
(51, 'un terme qui désigne une méthode journalistique\r'),
(52, 'une distribution publique de câlins, filmée et partagée sur le net\r'),
(53, 'un terme qui désigne le fait d’être heureux de publier une photo.\r'),
(54, 'le fait de filmer avec un téléphone portable l’agression physique d’une personne et à faire circuler la vidéo de cette agression.\r'),
(55, 'Les noter sur un post-it pour s’en souvenir\r'),
(56, 'Choisir un mot de passe suffisamment complexe\r'),
(57, 'Les confier à quelqu’un d’autre en cas de besoin\r'),
(58, 'Ne pas en mettre\r'),
(59, 'est facile le prénom de mes enfants/parents, ma date de naissance, etc.\r'),
(60, 'comporte au moins 12 caractères mélangeant des majuscules, des minuscules, des chiffres et des caractères spéciaux\r'),
(61, 'est composé du nom de mon acteur préféré\r'),
(62, 'est le mot ‘password’\r'),
(63, 'Je change à chaque fois le mot de passe\r'),
(64, 'J’utilise un logiciel de gestion mots de passe\r'),
(65, 'Je le note sur mon téléphone\r'),
(66, 'Je le note sur un papier, mais je le cache dans le tiroir de mon bureau.\r'),
(67, 'Je n’enregistre pas les mots de passe et me déconnecte après utilisation\r'),
(68, 'J’enregistre les mots de passe.\r'),
(69, 'Je laisse ma session ouverte\r'),
(70, 'J''enlève les mots de passe, j’utilise le même ordinateur\r'),
(71, 'Ce n’est pas utile de sécuriser mes réseaux sociaux car seuls mes amis peuvent voir mon profil\r'),
(72, 'J’utilise le même mot de passe sur tous mes réseaux sociaux car c’est plus simple à retenir\r'),
(73, 'J’utilise des mots de passe complexes\r'),
(74, 'J’utilise la date de naissance de chaque membre de ma famille sur les différents réseaux, comme ça ils sont bien différents.\r'),
(75, 'Votre nom\r'),
(76, 'Une combinaison de minuscules et de majuscules\r'),
(77, 'Votre numéro de téléphone\r'),
(78, 'Votre date de naissance mais à l’envers bien entendu\r'),
(79, 'Inutile, le mien est super robuste\r'),
(80, 'Judicieux toutes les semaines\r'),
(81, 'Conseillé si c’est fait régulièrement avec un nouveau mot de passe\r'),
(82, 'Inutile de toute façon les hackers vont les trouver, autant garder le même\r'),
(83, 'Force Brute\r'),
(84, 'Dictionnaire\r'),
(85, 'Man in The Middle\r'),
(86, 'KeyLogger ou Enregistreur de frappe\r'),
(87, 'Enpass\r'),
(88, 'Mozilla\r'),
(89, 'Skype\r'),
(90, 'Security\r'),
(91, 'Bien sûr, c’est gentil de sa part\r'),
(92, 'Surtout pas\r'),
(93, 'Bien sûr c’est un ami il ne le répètera à personne\r'),
(94, 'Evidemment c’est mon anniversaire quand même !\r'),
(95, 'Les messages privés seront automatiquement supprimés.\r'),
(96, 'Les utilisateurs recevront un avertissement.\r'),
(97, 'Les comptes peuvent être piratés.\r'),
(98, 'Les photos de profil seront floues.\r'),
(99, 'Utiliser le même mot de passe pour tous vos comptes en ligne.\r'),
(100, 'Changer vos mots de passe uniquement quand vous êtes attaqué\r'),
(101, 'Utiliser un gestionnaire de mots de passe pour stocker et générer des mots de passe forts.\r'),
(102, 'Écrire tous vos mots de passe sur un post-it et le coller sur votre écran d''ordinateur.\r'),
(103, '123456\r'),
(104, '123456789\r'),
(105, 'azerty\r'),
(106, '1234561\r'),
(107, 'Des propos répétés en ligne qui dégradent les conditions de vie de la victime\r'),
(108, 'L’affichage de publicités ciblées intempestives lors de la navigation sur internet\r'),
(109, 'Le partage systématique et répété de fake news sur internet\r'),
(110, 'Le piratage des données personnelles de la victime\r'),
(111, '1 ado sur 1000\r'),
(112, '1 ado sur 10\r'),
(113, '1 ado sur 100\r'),
(114, '1 ado sur 10000\r'),
(115, '12 %\r'),
(116, '25 %\r'),
(117, '78 %\r'),
(118, '96 %\r'),
(119, 'Piratage de compte, usurpation didentité\r'),
(120, 'Divergence d’opinion\r'),
(121, 'Intimidations, insultes, moqueries\r'),
(122, 'Rumeurs en ligne\r'),
(123, 'Jalousie\r'),
(124, 'Vengeance\r'),
(125, 'L’amour\r'),
(126, 'Religion\r'),
(127, 'Le signaler en appelant le 3018 ou le 3020\r'),
(128, 'Appeler la police\r'),
(129, 'On ne peut rien faire malheureusement\r'),
(130, 'Rien j’attend que ça passe\r'),
(131, 'Le changement de comportement ou d’humeur\r'),
(132, 'Les réactions moqueuses ou/et l’isolement dun élève\r'),
(133, 'Le rejet du numérique par le jeune\r'),
(134, 'L’utilisation excessive des réseaux sociaux\r'),
(135, '18 ans\r'),
(136, '11 ans\r'),
(137, 'Être ennuyés ou harcelés\r'),
(138, 'Se faire voler leur données personnelles\r'),
(139, 'Perdre leurs mots de passe\r'),
(140, 'Ne pas utiliser le bon filtre sur Instagram\r'),
(141, '3018\r'),
(142, '2018\r'),
(143, '1998\r'),
(144, '1978\r'),
(145, 'Amélioration de l''estime de soi\r'),
(146, 'Traumatisme émotionnel et détresse\r'),
(147, 'Croissance des amitiés en ligne\r'),
(148, 'Gains de popularité\r'),
(149, 'Aucune loi ne traite spécifiquement du cyber-harcèlement\r'),
(150, 'Les lois contre le cyber-harcèlement sont nombreuses, mais rarement appliquées\r'),
(151, 'Les lois contre le cyber-harcèlement ne sont pas nécessaires\r'),
(152, 'Il existe des lois spécifiques contre le cyber-harcèlement dans de nombreuses juridictions\r'),
(153, 'Les réseaux sociaux encouragent activement le cyber-harcèlement\r'),
(154, 'Les réseaux sociaux mettent en place des politiques et des outils pour signaler et lutter contre le cyber-harcèlement\r'),
(155, 'Les réseaux sociaux ne jouent aucun rôle dans la prévention du cyber-harcèlement\r'),
(156, 'Les réseaux sociaux promeuvent le cyber-harcèlement\r'),
(157, 'Travaux d''intérêt général.\r'),
(158, '18 mois de prison et une amende de 7 500 euros.\r'),
(159, '2 ans de prison et 30 000 euros d''amende.\r'),
(160, 'Rien la victime est mineure.\r'),
(161, 'En encourageant le respect en ligne\r'),
(162, 'En gardant le silence et en subissant le harcèlement\r'),
(163, 'En se moquant des victimes en ligne\r'),
(164, 'En partageant des informations personnelles en ligne\r'),
(165, 'Je ne fais jamais fonctionner le Wi-Fi et le Bluetooth en même temps\r'),
(166, 'Je mets régulièrement mes appareils à jour\r'),
(167, 'Je mets le code Pin à 0000 .\r'),
(168, 'J’équipe mes appareils d’une coque et d’une protection d’écran\r'),
(169, 'sur le site officiel du fournisseur\r'),
(170, 'sur les magasins officiels d’applications Google Play\r'),
(171, 'sur n’importe quel site\r'),
(172, 'sur les magasins officiels d’applications App Store\r'),
(173, 'J’utilise un stockage de données professionnelles distinct du stockage de données personnelles\r'),
(174, 'J’utilise ma connexion professionnelle uniquement pour mes besoins professionnels\r'),
(175, 'J’utilise mon matériel professionnel pour des besoins personnels\r'),
(176, 'J’effectue les mises à jour de mes systèmes très régulièrement\r'),
(177, 'J’accepte tout ce que l’on me demande\r'),
(178, 'Je n’autorise pas l’accès à mes photos, mes contacts et mes messages\r'),
(179, 'Je vais sur le premier site que je trouve\r'),
(180, 'Je n’autorise pas l’accès à mes photos, mais j’accepte l’accès à mes contacts et mes messages\r'),
(181, 'Je regarde le contenu, si ça se trouve c’est important\r'),
(182, 'Je n’ouvre pas la pièce jointe\r'),
(183, 'Je regarde, pour confirmer que c’est une attaque\r'),
(184, 'Ma messagerie est sécurisée, je me fais donc une peur pour rien, j’ouvre.\r'),
(185, 'Un message d’avertissement concernant votre utilisation d’Internet\r'),
(186, 'Un conseil pour pirater des films sur Internet\r'),
(187, 'Un avis sur les jeux en ligne destinés aux enfants\r'),
(188, 'Un niveau d’utilisation des jeux vidéos.\r'),
(189, 'C’est la peur de louper un appel\r'),
(190, 'C’est la peur de louper une notification\r'),
(191, 'C''est la peur de se retrouver sans son portable\r'),
(192, 'C’est la peur des appels de démarchage qui n’arrêtent JAMAIS\r'),
(193, 'La renommée\r'),
(194, 'L’appât du gain\r'),
(195, 'L''espionnage industriel\r'),
(196, 'Le chantage intellectuel\r'),
(197, 'Open Web Application Public\r'),
(198, 'Open Worldwide Application Security Project\r'),
(199, 'Obsolescence Web Appliquée Projets\r'),
(200, 'Ouverture du Web Aux Personnes\r'),
(201, 'Prévention\r'),
(202, 'Création\r'),
(203, 'Réaction\r'),
(204, 'Evolution\r'),
(205, 'Les ‘White Hats’ et les ‘Black Hats’\r'),
(206, 'Les ‘Data Hackers’ et les ‘Data Scientists’\r'),
(207, 'Les  ‘All Blacks’ et les ‘All Whites’\r'),
(208, 'Les ‘wicked Hats’ et les ‘Kind Hats’\r'),
(209, 'hoax\r'),
(210, 'buzz\r'),
(211, 'hack\r'),
(212, 'Virus\r'),
(213, 'Une organisation à but lucratif\r'),
(214, 'Une réglementation européenne sur la protection des données personnelles\r'),
(215, 'Une agence gouvernementale chargée de la cybersécurité\r'),
(216, 'Un réseau social populaire\r'),
(217, 'Une attitude qui valorise la sécurité en ligne dans une organisation\r'),
(218, 'Une approche pour maximiser les likes sur les réseaux sociaux\r'),
(219, 'Une façon de gagner de l''argent en ligne\r'),
(220, 'Une méthode pour pirater des comptes en ligne\r'),
(221, 'Ils appartiennent tous à la même entreprise\r'),
(222, 'Tous leurs logos sont bleu et rouge\r'),
(223, 'Aucun ne propose la possibilité de faire des stories\r'),
(224, 'Il n’y en a pas !\r'),
(225, 'Accepter sa demande\r'),
(226, 'Discuter avec la personne et décider après\r'),
(227, 'Supprimer la demande, si la personne te connaît, elle te le dira en Message Privé\r'),
(228, 'Accepter sa demande en m’étant une réserve.\r'),
(229, 'Contrôler les commentaires une fois en ligne\r'),
(230, 'Autoriser des commentaires libres pour responsabiliser les visiteurs\r'),
(231, 'Lire chaque commentaire avant d’autoriser la publication\r'),
(232, 'Autoriser les commentaires par défaut et attendre les retours des utilisateurs\r'),
(233, '6 %\r'),
(234, '16 %\r'),
(235, '26 %\r'),
(236, '56 %\r'),
(237, 'Un type de pêche en ligne\r'),
(238, 'Une technique pour voler des informations personnelles\r'),
(239, 'Une nouvelle fonctionnalité de partage d''images\r'),
(240, 'Une méthode pour gagner des abonnés\r'),
(241, 'Pour personnaliser les publicités que vous voyez\r'),
(242, 'Pour améliorer la sécurité de votre compte\r'),
(243, 'Pour augmenter le nombre de ‘j’aime'' sur vos publications\r'),
(244, 'Pour masquer vos amis de la liste d''amis\r'),
(245, 'Maximiser le nombre d''abonnés\r'),
(246, 'Rendre les publications plus amusantes\r'),
(247, 'Protéger la vie privée des utilisateurs\r'),
(248, 'Accélérer la vitesse de connexion\r'),
(249, '1997\r'),
(250, '2003\r'),
(251, '2001\r'),
(252, '1999\r'),
(253, 'Ransomware\r'),
(254, 'Spam\r'),
(255, 'Phishing (hameçonnage)\r'),
(256, 'Usurpation d''identité\r'),
(257, 'Partager votre mot de passe avec des amis proches ou votre copine\r'),
(258, 'Télécharger tous les fichiers partagés sans vérification\r'),
(259, 'Ignorer les demandes d''amis de personnes inconnues\r'),
(260, 'Utiliser un mot de passe complexe et unique\r'),
(261, 'Signaler le message comme spam ou suspect\r'),
(262, 'Ignorer le message\r'),
(263, 'Réponde en partageant des informations personnelles\r'),
(264, 'Ajouter l''expéditeur en ami\r'),
(265, 'Utiliser notre identité\r'),
(266, 'Se faire passer pour quelqu’un d’autre\r'),
(267, 'Modifier nos noms et prénoms\r'),
(268, 'Un jeu populaire\r'),
(269, 'Un community manager\r'),
(270, 'Un modérateur\r'),
(271, 'Un influenceur\r'),
(272, 'Un générateur\r'),
(273, 'Un organisme pour aider les seniors.\r'),
(274, 'Un groupe de personnes ayant les mêmes revenus.\r'),
(275, 'Un site où on peut publier des informations.\r'),
(276, 'Un groupe de personnes ayant les mêmes idées.\r'),
(277, 'Un document numérisé disponible sur mon téléphone\r'),
(278, 'Une carte d’identité scannée\r'),
(279, 'Une carte d''identité qui permet d’accéder à des services sur internet\r'),
(280, 'Toutes nos traces numériques sur Internet\r'),
(281, ' Sauvegarder mes données\r'),
(282, 'Défragmenter mon disque dur\r'),
(283, 'Nettoyer les données\r'),
(284, 'Nettoyer mon disque dur\r'),
(285, 'Non, les attaques viennent du réseau\r'),
(286, 'Oui, cela fait parti des attaques à prévoir\r'),
(287, 'Non, personne ne rentrera sur notre site.\r'),
(288, 'Non, les attaques sont logicielles.\r'),
(289, 'Recommandée parce que ça évite d’utiliser les réseaux\r'),
(290, 'Contrôlée parce que c’est aussi un vecteur d’attaque\r'),
(291, 'Ignorée, les clés USB ne sont que des supports de stockage\r'),
(292, 'Recommandée parce que ça permet d’isoler les données\r'),
(293, 'Plus risqué que d’utiliser des appareils fournis par mon entreprise\r'),
(294, 'Aussi risqué que d’utiliser des appareils fournis par mon entreprise\r'),
(295, 'Moins risqué que d’utiliser des appareils fournis par mon entreprise puisque c’est le mien !\r'),
(296, 'Conseillé, comme ça j’ai toujours mon travail avec moi.\r'),
(297, 'Je ne le fais qu’à partir de mon ordinateur professionnel\r'),
(298, 'Je ne le fais qu’à partir de mon ordinateur personnel\r'),
(299, 'Je ne le fais jamais\r'),
(300, 'Je ne le fais quà partir de mon téléphone personnel\r'),
(301, 'tout le monde devrait avoir un accès total à tous les systèmes et données d''un SI\r'),
(302, 'plus de privilèges signifie une meilleure sécurité\r'),
(303, 'accorder aux utilisateurs uniquement les privilèges dont ils ont besoin pour leur travail, limitant ainsi les risques de sécurité\r'),
(304, 'partager les mots de passe avec tous les utilisateurs\r'),
(305, 'Ouvrir tous les fichiers joints aux e-mails sans hésitation.\r'),
(306, 'Cliquer sur des liens provenant d''e-mails non sollicités.\r'),
(307, 'Utiliser un logiciel antivirus et le maintenir à jour.\r'),
(308, 'Ignorer les mises à jour de sécurité.\r'),
(309, 'Simplifier l''accès aux données sensibles.\r'),
(310, 'Réduire les coûts de sécurité.\r'),
(311, 'Renforcer l''authentification des utilisateurs.\r'),
(312, 'Encourager le partage d''identifiants.\r'),
(313, 'Augmenter les heures de travail.\r'),
(314, 'Réduire les coûts de formation.\r'),
(315, 'Sensibiliser aux menaces de sécurité.\r'),
(316, 'Améliorer la satisfaction des employés.\r'),
(317, 'Prévenir toutes les attaques.\r'),
(318, 'Déterminer qui est responsable des incidents.\r'),
(319, 'Fournir des services de messagerie sécurisés.\r'),
(320, 'Gérer et réagir de manière appropriée aux incidents de sécurité.\r'),
(321, 'Fuite de documents internes.\r'),
(322, 'Accès non autorisé aux locaux.\r'),
(323, 'Manipulation des employés pour obtenir des informations confidentielles.\r'),
(324, 'Changement fréquent des politiques de sécurité.\r'),
(325, 'Réduction Générale des Privilèges pour les Données\r'),
(326, 'Rapport Global sur les Préoccupations de Développement\r'),
(327, 'Réglementation Générale des Programmes associés aux Datas\r'),
(328, 'Règlement Général sur la Protection des Données\r'),
(329, 'HTTPS\r'),
(330, 'FTP\r'),
(331, 'SMTP\r'),
(332, 'HTTP\r'),
(333, 'Je laisse mon ordinateur comme il est, je suis tout seul dans mon bureau ça ne craint rien\r'),
(334, 'Je ferme ma session\r'),
(335, 'J''éteins mon écran\r'),
(336, 'Je demande à mon collègue de surveiller mon ordinateur\r'),
(337, 'Utiliser des mots de passe simples et faciles à retenir\r'),
(338, 'Réutiliser le même mot de passe sur tous les application de l’entreprise, c’est plus facile à retenir\r'),
(339, 'Utiliser un mot de passe d’au moins 14 caractères disposant de lettres majuscules, minuscules, chiffres et caractères spéciaux\r'),
(340, 'Partager vos mots de passe avec vos collègues, en cas d’absence c’est plus simple\r'),
(341, 'Pour promouvoir le piratage illimité d''informations\r'),
(342, 'Pour maximiser les profits de l''entreprise\r'),
(343, 'Pour protéger les données et les actifs contre les menaces en ligne\r'),
(344, 'Pour encourager le télétravail\r'),
(345, 'Former les employés sur les bonnes pratiques en matière de cybersécurité\r'),
(346, 'Externaliser la gestion de la sécurité\r'),
(347, 'Supprimer tous les accès à internet\r'),
(348, 'Ignorer complètement la sécurité en ligne\r'),
(349, 'Philosophie qui suppose que tout utilisateur (appareil, réseau) , même s''il se trouve à l''intérieur du réseau, ne peut pas être considéré comme sûr par défaut et doit être vérifié.\r'),
(350, 'Méthode pour supprimer toutes les mesures de sécurité en ligne\r'),
(351, 'Stratégie qui ne nécessite aucune forme de sécurité en ligne\r'),
(352, 'Approche qui consiste à accorder une confiance totale à tous les utilisateurs et appareils dès leur connexion au réseau\r'),
(353, 'Une faille de sécurité qui ne peut pas être exploitée\r'),
(354, 'Une faille de sécurité déjà connue du public\r'),
(355, 'Une faille de sécurité qui affecte uniquement les utilisateurs inexpérimentés\r'),
(356, 'Une faille de sécurité qui n''a pas encore été découverte par les éditeurs de logiciels ou les chercheurs en sécurité\r'),
(357, 'Le plat préféré de Winnie L''ourson\r'),
(358, 'Un serveur (un système) vulnérable délibérément configuré pour attirer les attaquants et étudier leurs méthodes\r'),
(359, 'Un outil de détection d''intrusion\r'),
(360, 'Une technique pour masquer des données sensibles\r'),
(361, 'Une méthode d''attaque qui cible les pêcheurs professionnels\r'),
(362, 'Une technique pour tromper les pirates informatiques\r'),
(363, 'Une technique où l''attaquant prétend être la victime\r'),
(364, 'Une approche de sécurité pour renforcer la protection des e-mails\r'),
(365, 'Un programme de récompense pour encourager les chercheurs en sécurité à trouver et signaler des vulnérabilités dans leurs systèmes\r'),
(366, 'Une prime offerte aux employés en cas d''intrusion réussie\r'),
(367, 'Une prime offerte aux pirates informatiques pour les récompenser de leurs attaques\r'),
(368, 'Une prime sous forme de barre chocolatée offerte aux employés en cas de violation de données\r'),
(369, 'Une attaque qui cible les systèmes de contrôle industriel\r'),
(370, 'Une attaque qui vise les pare-feu d''entreprise\r'),
(371, 'Une attaque qui bloque le trafic réseau\r'),
(372, 'Une attaque qui intercepte la communication entre deux parties, sans leur consentement, en se plaçant entre elles.\r'),
(373, 'Une attaque qui écrase les données des disques durs\r'),
(374, 'Une attaque qui supprime les données de la mémoire\r'),
(375, 'Une attaque qui exécuter du code malveillant en écrivant plus de données que l''espace alloué en mémoire\r'),
(376, 'Une attaque qui inonde un réseau de données pour le surcharger\r'),
(377, 'Une politique qui laisse libre l''utilisation des dispositifs personnels au travail\r'),
(378, 'Une politique interdisant aux employés de brancher (connecter) leurs dispositifs personnels au réseau de l''entreprise\r'),
(379, 'Une politique encourageant activement les employés à connecter leurs appareils personnels au réseau de l''entreprise\r'),
(380, 'Une politique autorisant uniquement l''utilisation d''appareils d''entreprise sur le réseau de l''entreprise\r'),
(381, 'C''est une prime, je clique et je valide.\r'),
(382, 'C''est la RH donc pas de soucis, je clique et je valide.\r'),
(383, 'Je me renseigne auprès de ma RH pour vérifier.\r'),
(384, 'C''est un mail interne puisque c’est la RH, je fais confiance\r'),
(385, 'En les chiffrant\r'),
(386, 'En les hachant c’est plus sûr\r'),
(387, 'En les envoyant vers des supports externes ou vers le Cloud\r'),
(388, 'En les cryptant\r'),
(389, 'Un logiciel malveillant qui vole des informations personnelles\r'),
(390, 'Un logiciel malveillant qui chiffre les fichiers de l''ordinateur de la victime, exigeant un paiement pour les déchiffrer\r'),
(391, 'Un logiciel malveillant qui efface les données de l''ordinateur de la victime\r'),
(392, 'Un logiciel malveillant qui crée des copies de fichiers confidentiels\r'),
(393, 'Un logiciel malveillant qui vole des informations personnelles\r'),
(394, 'Un logiciel malveillant qui chiffre les fichiers de l''ordinateur de la victime, exigeant un paiement pour les déchiffrer\r'),
(395, 'Un logiciel malveillant qui efface les données de l''ordinateur de la victime\r'),
(396, 'Un logiciel malveillant qui crée des copies de fichiers confidentiels\r'),
(397, 'Spoofing\r'),
(398, 'Impersonation\r'),
(399, 'Phishing\r'),
(400, 'Déni de service (DDoS)\r'),
(401, 'Malware\r'),
(402, 'Cheval de Troie\r'),
(403, 'Spam\r'),
(404, 'Ver\r'),
(405, 'nom et prénom\r'),
(406, 'date de naissance et lieu\r'),
(407, 'Lieu de résidence principale\r'),
(408, 'pratique religieuse\r'),
(409, 'authentification\r'),
(410, 'routage\r'),
(411, 'sauvegarde\r'),
(412, 'identification\r'),
(413, 'Empêcher l’exécution de programmes malveillants sur un PC\r'),
(414, 'Bloquer les intrusions malveillantes sur un réseau ou un PC\r'),
(415, 'Bloquer les tentatives de phishing\r'),
(416, 'Bloquer la réception des spams sur la messagerie\r'),
(417, 'Récupérer des ressources identifiées par une URL sur un serveur Web\r'),
(418, 'Partager des documents\r'),
(419, 'Supprimer des fichiers\r'),
(420, 'Trier des ressources pour l’utilisateur\r'),
(421, 'd’accès et de rectification\r'),
(422, 'à la limitation de données\r'),
(423, 'à la portabilité\r'),
(424, 'à la vie privée\r'),
(425, 'un virus\r'),
(426, 'une adresse mail\r'),
(427, 'une adresse de site Internet\r'),
(428, 'une adresse postale\r'),
(429, 'un message internet privé\r'),
(430, 'un message non désiré\r'),
(431, 'une récompense\r'),
(432, 'un message de client\r'),
(433, 'Chiffrement\r'),
(434, 'Spoofing\r'),
(435, 'Sniffing\r'),
(436, 'Destruction des données\r'),
(437, 'jamais\r'),
(438, 'une fois par an\r'),
(439, 'une fois par trimestre\r'),
(440, 'tous les jours\r'),
(441, 'les informations ne sont pas mises à disposition  ou divulguées à des personnes non autorisées\r'),
(442, 'maintenir et assurer l''exactitude et l''exhaustivité des données tout au long de leur cycle de vie\r'),
(443, 'systèmes, applications et données disponibles pour les utilisateurs lorsqu''il en ont besoin\r'),
(444, 'une des parties de la transaction ne peut pas nier l''avoir reçue, l''autre l''avoir envoyée\r'),
(445, 'Décryptage\r'),
(446, 'Cryptage\r'),
(447, 'Transformation\r'),
(448, 'Aucune de ces réponses n’est vraie.\r'),
(449, 'certificat numérique\r'),
(450, 'aucune de ces réponses\r'),
(451, 'Référent Système en Sécurité de l’Information\r'),
(452, 'Responsable de la Sûreté des Systèmes d’Information\r'),
(453, 'Réforme de la Sécurité Sociale Informatisée\r'),
(454, 'Responsable de la Sécurité des Systèmes d’Information\r'),
(455, 'Un principe visant à protéger le partage des données au sein des entreprises\r'),
(456, 'Une méthodologie de documentation de toutes les actions de mise en conformité initiées par les responsables de traitement\r'),
(457, 'Une approche visant à intégrer la protection de la vie privée dès la conception des nouveaux systèmes et processus.\r'),
(458, 'Une nouvelle façon de programmer à la mode\r'),
(459, 'Pourriel\r'),
(460, 'Courriel\r'),
(461, 'Corbeille\r'),
(462, 'Poubelle\r'),
(463, 'Traceur\r'),
(464, 'Connecteur\r'),
(465, 'Détecteur\r'),
(466, 'Facteur\r'),
(467, 'Crypter\r'),
(468, 'Chiffrer\r'),
(469, 'Heu, on peut tout faire en fait\r'),
(470, 'Décrypter\r'),
(471, 'Pour augmenter le nombre de publicités\r'),
(472, 'Pour améliorer les performances de votre téléphone\r'),
(473, 'Pour corriger les vulnérabilités de sécurité et bénéficier des dernières fonctionnalités de sécurité\r'),
(474, 'Pour se tenir au courant des dernières news\r'),
(475, 'Virus\r'),
(476, 'Ver\r'),
(477, 'Clickjacking\r'),
(478, 'Cheval de Troie\r'),
(479, 'Ignorer complètement tous les commentaires\r'),
(480, 'Bloquer ou signaler les utilisateurs harceleurs\r'),
(481, 'Engager une conversation agressive\r'),
(482, 'Critiquer tout le monde\r'),
(483, 'Pour s''assurer que le message est drôle\r'),
(484, 'Pour éviter de partager de fausses informations ou des arnaques\r'),
(485, 'Pour voir combien de likes vous pouvez obtenir\r'),
(486, 'Pour voir depuis quand elle existe\r'),
(487, 'Virtual Private Network\r'),
(488, 'Very Personal Network\r'),
(489, 'Very Public Network\r'),
(490, 'Virtual Personal in NewYork\r'),
(491, 'Vous risquez de recevoir trop de messages\r'),
(492, 'Vous pourriez partager trop d''informations personnelles\r'),
(493, 'Vous risquez de perdre des amis existants\r'),
(494, 'Je ne risque rien, c’est virtuel !\r'),
(495, 'Amis\r'),
(496, 'Ennemis\r'),
(497, 'Public\r'),
(498, 'Tout le monde\r'),
(499, 'TED Talks\r'),
(500, 'Black Hat\r'),
(501, 'DEFCON\r'),
(502, 'Copy Cat\r'),
(503, 'Alan Turing\r'),
(504, 'Claude Shannon\r'),
(505, 'Julius Caesar\r'),
(506, 'Steven Levy\r'),
(507, 'Steganographie\r'),
(508, 'Cryptanalyse\r'),
(509, 'Ingénierie inverse\r'),
(510, 'Cryptographie\r'),
(511, 'Authentification à deux facteurs\r'),
(512, 'Authentification multifactorielle\r'),
(513, 'Authentification biométrique\r'),
(514, 'Authentification bioéthique\r'),
(515, 'ISO 27001\r'),
(516, 'PCI DSS\r'),
(517, 'NIST SP 800-53\r'),
(518, 'ISO 9020\r'),
(519, 'Pour économiser de l''énergie\r'),
(520, 'Pour éviter que vos collègues n''utilisent votre ordinateur\r'),
(521, 'Pour prévenir un accès non autorisé à vos données\r'),
(522, 'Pour montrer votre joli fond d’écran\r'),
(523, 'Pour augmenter les performances de l''ordinateur\r'),
(524, 'Pour empêcher les mises à jour du système d''exploitation\r'),
(525, 'Pour protéger contre les nouvelles menaces de virus et de logiciels malveillants\r'),
(526, 'Pour faire plaisir à votre responsable\r'),
(527, 'Je la branche sur mon ordinateur pour voir ce qu’elle contient\r'),
(528, 'Je le signale au service informatique ou à la sécurité\r'),
(529, 'Je ne fais rien\r'),
(530, 'Je la dépose discrètement sur le bureau de mon collègue\r'),
(531, 'Visiter des sites web de médias sociaux pour des pauses\r'),
(532, 'Naviguer uniquement sur des sites web approuvés par l''entreprise\r'),
(533, 'Télécharger des logiciels à partir de sites web inconnus\r'),
(534, 'Utiliser son téléphone personnel\r'),
(535, 'Pour augmenter la vitesse de la connexion\r'),
(536, 'Pour protéger vos données contre les interceptions\r'),
(537, 'Pour économiser de la batterie\r'),
(538, 'Pour supprimer toutes mes traces\r'),
(539, 'Advanced Persistent Threat (Menace Persistante Avancée)\r'),
(540, 'Application Protection Technology (Technologie de Protection des Applications)\r'),
(541, 'All Possible Threats (Toutes les Menaces Possibles)\r'),
(542, 'All Possible Technology’s (Toutes les Technologies Possibles)\r'),
(543, 'Griefing\r'),
(544, 'Catfishing\r'),
(545, 'Shaming\r'),
(546, 'Spoofing\r'),
(547, 'Doxing\r'),
(548, 'Trolling\r'),
(549, 'Boxing\r'),
(550, 'Spamming\r'),
(551, 'virus informatique se reproduisant rapidement et susceptible d''infecter votre système en seulement 15 minutes.\r'),
(552, 'animal bizarre qui se prénomme Andy\r'),
(553, 'cheval de Troie qui détruit votre machine en 15 secondes\r'),
(554, 'virus informatique qui affiche des images en couleur sur votre écran pour le vérouiller\r'),
(555, 'un cheval de Troie dangereux capable de désactiver votre logiciel antivirus\r'),
(556, 'un virus qui dessine Betty Boop sur votre écran\r'),
(557, 'un ver qui n’affecte que vos réseaux sociaux\r'),
(558, 'un spam sans conséquence\r'),
(559, 'Worm / Ver\r'),
(560, 'Trojan\r'),
(561, 'Ransomware\r'),
(562, 'Spam\r'),
(563, 'Le nettoyage physique des ordinateurs\r'),
(564, 'Les pratiques et mesures visant à maintenir un environnement informatique propre et sûr\r'),
(565, 'La surveillance des activités en ligne des employés\r'),
(566, 'Le nettoyage de ses mains avant de taper au clavier\r'),
(567, 'Un moyen de s''authentifier en utilisant deux mots de passe\r'),
(568, 'Un processus d''authentification qui requiert deux méthodes différentes pour vérifier l''identité d''un utilisateur\r'),
(569, 'Un système de chiffrement à deux clés\r'),
(570, 'Un moyen de s’identifier en utilisant un login et une adresse mail\r'),
(571, '11 %\r'),
(572, '21 %\r'),
(573, '31 %\r'),
(574, '41 %\r'),
(575, 'Un compte administrateur\r'),
(576, 'Un compte invité\r'),
(577, 'Un compte utilisateur\r'),
(578, 'Aucun compte n’est nécéssaire\r'),
(579, 'Plus sécurisé\r'),
(580, 'Moins sécurisé\r'),
(581, 'Aussi sécurisé\r'),
(582, 'Je ne sais pas\r'),
(583, 'La fraude au président\r'),
(584, 'Le déni de service\r'),
(585, 'L’attaque de Princeton\r'),
(586, 'L’attaque MITM\r'),
(587, 'http://\r'),
(588, 'https://\r'),
(589, 'httpsécu://\r'),
(590, 'htts://\r'),
(591, 'Sécurité du Cloud\r'),
(592, 'Sécurité du réseau\r'),
(593, 'Sécurité des applications\r'),
(594, 'En fait toutes les réponses.\r'),
(595, 'Advanced Encryption Standard (AES)\r'),
(596, 'William Gibson\r'),
(597, 'Andrew Tannenbaum\r'),
(598, 'Richard Stallman\r'),
(599, 'Wallace Emerson\r'),
(600, '1960\r'),
(601, '1970\r'),
(602, '1980\r'),
(603, '1990\r'),
(604, 'Une attaque par pharming\r'),
(605, 'Le contenu dupliqué ( Website-Duplication )\r'),
(606, 'Les spam\r'),
(607, 'Le streaming\r'),
(608, 'Un traqueur d’IP\r'),
(609, 'Les emails\r'),
(610, 'Les sites Web\r'),
(611, 'Les pages Web\r'),
(612, 'Les attaques par botnet\r'),
(613, 'Les programmes informatiques\r'),
(614, 'WPA3\r'),
(615, 'WPA2\r'),
(616, 'WPA\r'),
(617, 'WEP\r'),
(618, 'Antivirus\r'),
(619, 'Injection Html\r'),
(620, 'Injection SQL\r'),
(621, 'Injection XML\r'),
(622, 'Injection AntiCovid\r'),
(623, 'Cross Site Scripting\r'),
(624, 'Cross Site Security\r'),
(625, 'eXtrem Site Scripting\r'),
(626, 'eXtreme Secure Scripting\r'),
(627, 'Détournement de session\r'),
(628, 'Cyber Crime\r'),
(629, 'Cyber Attaque\r'),
(630, 'Cyber Cyborg\r'),
(631, 'Keyloggers\r'),
(632, 'Keyjacking\r'),
(633, 'Keyhijacking\r'),
(634, 'KeyTraceur\r'),
(635, 'Ils aident à mieux comprendre le hacking\r'),
(636, 'Ce sont des éléments clés d’une faille de sécurité\r'),
(637, 'Ils aident à mieux comprendre la sécurité et ses composants\r'),
(638, 'Ils aident à mieux comprendre la cybercriminalité\r'),
(639, 'La reconnaissance\r'),
(640, 'Le balayage du réseau ( scanner)\r'),
(641, 'L’obtention de l’accès\r'),
(642, 'La maintenance de l’accès\r'),
(643, 'Appels téléphoniques à la victime ciblée\r'),
(644, 'Un attaquant se faisant passer pour le support du Help Desk\r'),
(645, 'Parlez à l''utilisateur cible en personne\r'),
(646, 'Rechercher des enregistrements cibles dans la base de données de personnes en ligne\r'),
(647, 'Nexpose\r'),
(648, 'NMAP\r'),
(649, 'Nessus\r'),
(650, 'Maltego\r'),
(651, 'DEFCON\r'),
(652, 'COMICON\r'),
(653, 'SECCON\r'),
(654, 'CYBERCON\r'),
(655, 'Phreaking\r'),
(656, 'Breaking\r'),
(657, 'Cracking\r'),
(658, '1978\r'),
(659, '1988\r'),
(660, '1998\r'),
(661, '2008\r'),
(662, 'Copie de données\r'),
(663, 'Violation de données\r'),
(664, 'Obscurcissement des données\r'),
(665, 'Duplication de données\r'),
(666, 'Les requêtes automatisées\n'),
(667, 'Les robots cookies\r'),
(668, 'Les robots\r'),
(669, 'Les bots\r'),
(670, 'Internet Programme\r'),
(671, 'Internet Protocole\r'),
(672, 'Internet Padawan\r'),
(673, 'Internet Plateforme\r'),
(674, 'Sensibles\r'),
(675, 'Puissantes\r'),
(676, 'Inutiles\r'),
(677, 'Emotionnelles\r'),
(678, 'empoisonnement DNS\r'),
(679, 'la Reconnaissance (Footprinting)\r'),
(680, 'L''empoisonnement ARP\r'),
(681, 'L’énumération\r'),
(682, 'Social engineering\r'),
(683, 'Reverse engineering\r'),
(684, 'Planting malware\r'),
(685, 'Injection engineering\r'),
(686, 'La pensée créative\r'),
(687, 'Capacité de résolution de problèmes\r'),
(688, 'Persévérance\r'),
(689, 'Toutes ses réponses\r'),
(690, 'SQL\r'),
(691, 'HTML\r'),
(692, 'C#\r'),
(693, 'F#\r'),
(694, 'Injection de chevaux de Troie à une victime cible\r'),
(695, 'Les détails de la carte de crédit déposés dans le Deep Web\r'),
(696, 'une bonne pratique éthique\r'),
(697, 'très bonne pratique d''ingénierie sociale\r'),
(698, 'une très mauvaise pratique éthique\r'),
(699, 'Très bonne pratique de triche\r'),
(700, 'Logiciel de base de données\r'),
(701, 'Applications automatisées\r'),
(702, 'Messenger\r'),
(703, 'Les salons de discussion ( chat-rooms)\r'),
(704, 'Les sites de tutoriels\r'),
(705, 'Les réseaux sociaux\r'),
(706, 'Ne gardez jamais votre mot de passe avec des noms facile à trouver\r'),
(707, 'Conservez des traces écrites de vos mots de passe\r'),
(708, 'Conservez des enregistrements de votre mot de passe au format audio sur votre téléphone\r'),
(709, 'Les mots de passe utilisés sont de plus petite taille pour être mémorisés\r'),
(710, 'Le scan rétinien\r'),
(711, 'La numérisation d''empreintes digitales\r'),
(712, 'La vérification en deux étapes\r'),
(713, 'Le clic intempestif\r'),
(714, 'Le cyberstalking\r'),
(715, 'Le body shaming\r'),
(716, 'Partager sa localisation exacte avec ses amis\r'),
(717, 'Activer les publicités géolocalisées\r'),
(718, 'Permettre à d''autres de connaître l''emplacement où vous avez posté une publication\r'),
(719, 'Disposer de ses cours de géographie\r'),
(720, 'Je ne vérifie pas c’est forcément véridique\r'),
(721, 'En faisant des recherches supplémentaires, en utilisant des sources fiables\r'),
(722, 'En partageant ces informations avec mes amis\r'),
(723, 'En regardant le nom de la personne à la source\r'),
(724, 'Oui le navigateur est équipé pour ça\r'),
(725, 'Bien sûr , ça m’évite de l’oublier\r'),
(726, 'La simplicité\r'),
(727, 'Le gain de temps\r'),
(728, 'La facilité d’exécution\r'),
(729, 'Très difficile et inefficace\r'),
(730, 'technique active de collecte d’informations\r'),
(731, 'technique inactive de collecte d’informations\r'),
(732, 'technique permissive de collecte d’informations\r'),
(733, 'technique endormie de collecte d’informations\r'),
(734, 'Virus du secteur d''amorçage\r'),
(735, 'Virus polymorphe\r'),
(736, 'Trojans\r'),
(737, 'Virus de secteur informatique\r'),
(738, 'Les données bancaires\r'),
(739, 'Le numéro de téléphone\r'),
(740, 'Le mots de passe\r'),
(741, 'Les applications installées\r'),
(742, 'Deep Web\r'),
(743, 'World Wide Web\r'),
(744, 'Web de Surface\r'),
(745, 'Haunted Web\r'),
(746, 'Mr. Michael K. Bergman\r'),
(747, 'Mr. Ross Ulbricht\r'),
(748, 'Mr. Tim Berners-Lee\r'),
(749, 'Mr. Robert Elliot Kahn\r');

-- --------------------------------------------------------

--
-- Structure de la table `question`
--

CREATE TABLE IF NOT EXISTS `question` (
`numero` int(11) NOT NULL COMMENT 'numero de la question',
  `libelle` varchar(200) NOT NULL COMMENT 'libelle de la question',
  `niveauDifficulte` varchar(1) NOT NULL COMMENT 'niveau de difficulte de la question',
  `theme` int(11) NOT NULL COMMENT 'theme de la question'
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COMMENT='table stockant les questions' AUTO_INCREMENT=201 ;

--
-- Contenu de la table `question`
--

INSERT INTO `question` (`numero`, `libelle`, `niveauDifficulte`, `theme`) VALUES
(1, 'Divulguer trop d’informations à caractère personnel sur Internet ...', 'F', 1),
(2, 'Un ami vous envoi un mail en anglais avec des photos. Faut-il ouvrir ce mail ?', 'F', 1),
(3, 'Je n’utilise plus mon compte de réseau social…', 'F', 1),
(4, 'Je souhaite restreindre la visibilité de mes informations personnelles et de mes publications', 'F', 1),
(5, 'Quelle est la mauvaise pratique d’utilisation des réseaux sociaux :', 'F', 1),
(6, 'Quel âge est requis pour avoir un compte Facebook ?', 'D', 1),
(7, 'Connaît-on les personnes avec qui on échange sur les réseaux sociaux ?', 'F', 1),
(8, 'Quel est le réseau social de communication le moins utilisé par les 16-25 ans ?', 'F', 1),
(9, 'Mon profil sur internet, c’est...', 'F', 1),
(10, 'Supprimer son compte Facebook est fait dans …', 'F', 1),
(11, 'La e-réputation, c’est …', 'D', 1),
(12, 'Quel pourcentage des 14-17 ans ont déjà été contactés par des inconnus sur les réseaux sociaux ?', 'E', 1),
(13, '“Taguer“, identifier,  la photo d’une personne l''expose davantage sur les réseaux sociaux et sur internet en général.', 'F', 1),
(14, 'Le Dedipix est un jeu d’images sur les smartphones', 'E', 1),
(15, 'Le happy slapping, cest...', 'D', 1),
(16, 'Quelle est la bonne pratique à mettre en œuvre pour assurer la sécurité de vos mots de passe ?', 'F', 2),
(17, 'Un mot de passe sécurisé …', 'F', 2),
(18, 'Je ne me souviens jamais de mes mots de passe…', 'F', 2),
(19, 'Je travaille toujours sur le même ordinateur à la bibliothèque', 'F', 2),
(20, 'Quelle est la bonne pratique à mettre en œuvre pour éviter le piratage de compte ?', 'F', 2),
(21, 'Quels sont, parmi les éléments suivants, ceux qui devraient être inclus dans un mot de passe sûr ?', 'F', 2),
(22, 'Les changements de mots de passe périodiques, c’est…', 'F', 2),
(23, 'Contre quelles méthodes de piratage la longueur d’un mot de passe vous protège-t-il ? ', 'D', 2),
(24, 'Contre quelles méthodes de piratage la complexité d’un mot de passe vous protège-t-il ? ', 'D', 2),
(25, 'Contre quelles méthodes de piratage la présence de caractères spécieux d’un mot de passe vous protège-t-il ? ', 'D', 2),
(26, 'Lequel de ces logiciels est un gestionnaire de mots de passe ?', 'D', 2),
(27, 'Un ami veut avoir votre mot de passe pour vous acheter un cadeau d''anniversaire sur une plateforme de jeux, faut-il le lui donner?', 'F', 2),
(28, 'Quels sont les risques liés à l''utilisation de mots de passe faibles sur les réseaux sociaux ?', 'F', 2),
(29, 'Quelle est la meilleure approche pour gérer vos mots de passe en toute sécurité ?', 'F', 2),
(30, 'En France, quel est le mot de passe le plus répandu ? ', 'F', 2),
(31, 'Qu’est ce que le cyber-harcèlement ? ', 'F', 1),
(32, 'Combien d’ados déclarent avoir été victimes de cyber-harcèlement ?', 'E', 1),
(33, 'Quel pourcentage des parents n''ont pas connaissance de ce que leurs enfants font sur les réseaux sociaux ?', 'E', 1),
(34, 'Qu’est ce qui n’est pas du cyber-harcèlement ?', 'F', 1),
(35, 'Quelle est  la raison principale évoquée par les victimes de cyber-harcèlement ?', 'F', 1),
(36, 'Que faire si je suis victime ou témoin de cyber-harcèlement ?', 'F', 1),
(37, 'Quel signe ne reflète pas une situation de cyber-harcèlement ?', 'F', 1),
(38, 'Quel est l’âge de la majorité numérique en France ?', 'E', 1),
(39, 'De quoi les jeunes ont-ils le plus peur ?', 'E', 1),
(40, 'Quel est le numéro gratuit "Net écoute" dédié au cyber-harcèlement en France ?', 'E', 1),
(41, 'Quelles conséquences peut avoir le cyber-harcèlement sur les victimes ?', 'F', 1),
(42, 'Quelles sont les lois contre le cyber-harcèlement ?', 'E', 1),
(43, 'Quel est le rôle des réseaux sociaux dans la lutte contre le cyberharcèlement ?', 'D', 1),
(44, 'Quelles sont les peines si l''auteur du cyber-harcèlement est un mineur de + 13 ans et la victime a - 15 ans ?', 'E', 1),
(45, 'Quel est le moyen courant de prévenir le cyber-harcèlement ?', 'F', 1),
(46, 'Parmi les propositions, quelle est la bonne pratique à mettre en œuvre pour assurer au mieux la sécurité numérique de vos appareils mobiles?', 'F', 2),
(47, 'J’ai besoin d’une application mobile. Je ne vais pas :', 'F', 2),
(48, 'Pour protéger mes usages numériques pro/perso…', 'F', 2),
(49, 'Je télécharge un jeu sur mon téléphone', 'F', 2),
(50, 'Je viens de recevoir un e-mail suspect contenant une pièce jointe', 'F', 2),
(51, 'Qu’est-ce qu’une recommandation Hadopi ?', 'D', 2),
(52, 'Qu’est-ce que la nomophobie ? ', 'D', 2),
(53, 'Quelle est la plus grande motivation des attaquants ?', 'F', 2),
(54, 'OWASP … mais encore ? ', 'E', 2),
(55, 'En Cybersécurité, il y a une notion de :', 'D', 2),
(56, 'En Cyberdéfense, il y a une notion de :', 'D', 2),
(57, 'Parmi les hackers, pirates informatiques, on peut trouver :', 'D', 2),
(58, 'Quel est le terme anglais désignant les canulars qui circulent sur internet ?', 'F', 2),
(59, 'Qu''est-ce que le RGPD (Règlement Général sur la Protection des Données) ?', 'D', 2),
(60, 'Qu''est-ce que la culture de la cybersécurité ?', 'F', 2),
(61, 'Quel est le point commun entre Facebook, Instagram et Whatsapp ?', 'D', 1),
(62, 'Que dois-je faire si je reçois une demande d’ajout sur mes réseaux sociaux d’une personne que je ne connais pas ?', 'F', 1),
(63, 'Vous décidez de créer un blog, avec la fonction ‘commentaires'' ouverte. Vous souhaitez maîtriser les dérives possibles; pour cela, vous devez…', 'D', 1),
(64, 'Les filles qui déclarent avoir déjà été confrontées à des questions indiscrètes, du harcèlement, des insultes, des menaces et des moqueries représentent…', 'D', 1),
(65, 'Qu''est-ce qu''un ‘phishing’ sur les réseaux sociaux ?', 'F', 1),
(66, 'Comment les réseaux sociaux utilisent-ils vos données personnelles ?', 'D', 1),
(67, 'Quel est l''objectif d''une politique de confidentialité sur les réseaux sociaux ?', 'F', 1),
(68, 'Quand a été créé le premier réseau social ?', 'E', 1),
(69, 'Quel est l''arnaque la plus répandue sur les réseaux sociaux', 'F', 1),
(70, 'Quelle est la meilleure pratique pour protéger votre compte sur les réseaux sociaux ?', 'F', 1),
(71, 'Quelle est la meilleure façon de réagir à un message suspect sur les réseaux sociaux ?', 'F', 1),
(72, 'L''usurpation d''identité est ...', 'F', 1),
(73, 'La personne chargée de gérer les réseaux sociaux d’une entreprise est…', 'D', 1),
(74, 'Qu''est-ce qu''un réseau social ?', 'F', 1),
(75, 'L’identité numérique, c’est :', 'D', 1),
(76, 'Parmi les principes de base, je dois absolument :', 'D', 5),
(77, 'Les stations de programmation doivent être protégées physiquement…', 'D', 5),
(78, 'L’utilisation de clés usb pour véhiculer de l’information doit être…', 'F', 5),
(79, 'Apporter votre appareil personnel est généralement…', 'F', 5),
(80, 'Je consulte mes messages professionnels…', 'F', 5),
(81, 'Qu''est-ce que le principe du ‘moindre privilège’ en matière de cybersécurité ?', 'F', 5),
(82, 'Quelle est la meilleure pratique pour protéger votre ordinateur contre les logiciels malveillants et les virus?', 'D', 5),
(83, 'Quel est le but de l''authentification multi-facteurs (MFA) en entreprise?', 'D', 5),
(84, 'Quel est l''objectif principal de la formation à la sécurité pour les employés?', 'F', 5),
(85, 'Quel rôle joue un plan de réponses aux incidents dans la cybersécurité d''entreprise?', 'D', 5),
(86, 'Quelle est la principale menace pour la sécurité liée à l''ingénierie sociale en entreprise?', 'F', 5),
(87, 'Que signifie le RGPD ? ', 'F', 5),
(88, 'Quelle technologie permet de sécuriser les communications entre un navigateur web et un site web ?', 'D', 5),
(89, 'Je m''absente pendant quelques instants pour allez prendre de l’eau en salle de repos…', 'F', 5),
(90, 'Quelle est la meilleure pratique pour créer des mots de passe fort ?', 'F', 5),
(91, 'Pourquoi la culture de la cybersécurité est-elle importante dans une organisation ?', 'F', 2),
(92, 'Quelle est la première étapes pour promouvoir la culture de la cybersécurité au sein d''une organisation ?', 'F', 2),
(93, 'Que signifie le terme ‘Zero Trust’ ?', 'E', 5),
(94, 'Qu''est-ce qu''une vulnérabilité ‘zero-day’ ?', 'E', 5),
(95, 'Qu''est-ce qu''un ‘honeypot’ ou ‘pot de miel’ ? ', 'D', 5),
(96, 'Qu''est-ce que le ‘phishing inversé’ ?', 'D', 5),
(97, 'Qu''est-ce que le ‘bug bounty’?', 'E', 5),
(98, 'Qu''est-ce qu''une attaque ‘Man-In-The-Middle’ (MITM) ?', 'E', 5),
(99, 'Qu''est-ce que l''attaque ‘buffer overflow’ ?', 'E', 5),
(100, 'Qu''est-ce que le terme ‘BYOD’ (Bring Your Own Device) ?', 'D', 5),
(101, 'Vous recevez un courrier électronique de votre RH vous demandant de lui donner vos coordonnées bancaires (prime exceptionnelle) allouée uniquement en cliquant sur le lien du mail…', 'F', 5),
(102, 'Comment pouvez-vous protéger la confidentialité des données de vos clients ?', 'D', 5),
(103, 'Qu''est-ce qu''un ransomware ?', 'F', 2),
(104, 'Quel terme désigne le fait de se faire passer pour quelqu''un d''autre en ligne dans le but de harceler une personne ?', 'E', 1),
(105, 'Parmi les propositions ci-dessous laquelle n’est pas considérée comme un virus ?', 'E', 2),
(106, 'Quelle information ne peut-on pas récupérer des utilisateurs d’une application sans autorisation préalable de l’organisme de contrôle ?', 'D', 2),
(107, 'Comment appelle-t-on la procédure qui vérifie l’identité d’une personne en vue de lui donner accès à des services ?', 'E', 5),
(108, 'Quelle est l’utilité d’un pare feu ?', 'F', 5),
(109, 'Quelle est la principale fonctionnalité d’un navigateur ?', 'F', 2),
(110, 'Je peux demander à un réseau social de geler l’utilisation des photos fournies, il s’agit du droit ..', 'E', 2),
(111, 'Je demande à modifier mes données car j’ai constaté des erreurs de la part des sites sur lesquels j’intervient, c''est mon droit ', 'D', 2),
(112, 'Qu’est-ce qu’une URL ?', 'F', 2),
(113, 'Qu’est-ce qu’un spam ?', 'F', 2),
(114, 'Lequel des procédés suivants peut être utilisé pour capturer des mots de passe sur un réseau ?', 'D', 5),
(115, 'Quelle fréquence vous semble le plus adapté pour parlez de cybersécurité dans une entreprise ?', 'D', 5),
(116, 'C’est quoi l’intégrité en Cyber-Sécurité ? ', 'E', 5),
(117, 'C’est quoi la confidentialité en Cyber-Sécurité ? ', 'E', 5),
(118, 'Quelle méthode transforme le message en format qui ne peut pas être lu par les pirates ?', 'D', 2),
(119, 'Quelle méthode est utilisée pour valider l’identité de l’expéditeur du message auprès du destinataire ?', 'E', 5),
(120, 'Que signifie le sigle RSSI ?', 'F', 5),
(121, 'Qu’est-ce que le privacy-by-design ?', 'E', 5),
(122, 'Quel est l’équivalent français d’un spam ?', 'D', 2),
(123, 'Quel est l’équivalent français d’un cookie ?', 'F', 2),
(124, 'En cyber je ne peux pas ', 'D', 2),
(125, 'Pourquoi est-il important de mettre à jour régulièrement vos applications de réseaux sociaux ?', 'F', 1),
(126, 'Quel type de malware peut se propager via les réseaux sociaux en se faisant passer pour une vidéo ou une application inoffensive ?', 'E', 1),
(127, 'Quelle action peut aider à prévenir le harcèlement en ligne sur les réseaux sociaux ?', 'D', 1),
(128, 'Pourquoi est-il important de vérifier la source d''un message avant de le partager sur les réseaux sociaux ?', 'D', 1),
(129, 'Que signifie l''acronyme VPN, souvent utilisé pour renforcer la sécurité en ligne ?', 'D', 5),
(130, 'Quelle est la principale menace liée à l''acceptation d''amis inconnus sur les réseaux sociaux ?', 'D', 1),
(131, 'Quel paramètre de confidentialité permet de contrôler qui peut voir vos publications sur Facebook ?', 'F', 1),
(132, 'Quelle conférence annuelle est célèbre pour ses présentations sur la cybersécurité et ses révélations de vulnérabilités ?', 'E', 2),
(133, 'Qui est considéré comme le père de la cryptographie moderne avec la machine Enigma ?', 'E', 2),
(134, 'Quel concept fait référence à la technique qui consiste à cacher des informations sensibles en les dissimulant dans d''autres données ?', 'E', 2),
(135, 'Quelle technique de cybersécurité utilise des identifiants biométriques tels que les empreintes digitales ou la reconnaissance faciale pour renforcer l''authentification ?', 'E', 5),
(136, 'Quelle norme de sécurité informatique spécifie les exigences pour la gestion de la sécurité de l''information dans une entreprise ?', 'D', 5),
(137, 'Pourquoi est-il important de verrouiller votre ordinateur lorsque vous vous éloignez de votre poste de travail au bureau ?', 'F', 5),
(138, 'Pourquoi est-il important de garder votre logiciel antivirus à jour sur votre ordinateur de travail ?', 'F', 5),
(139, 'Vous trouvez un périphérique USB non identifié sur votre lieu de travail…', 'F', 5),
(140, 'Quelle est la meilleure pratique en matière de navigation sur Internet au travail ?', 'D', 5),
(141, 'Pourquoi est-il important d''utiliser une connexion sécurisée (https://) lors de la saisie d''informations sensibles en ligne ?', 'D', 2),
(142, 'Que signifie l''acronyme APT en cybersécurité ?', 'E', 5),
(143, 'Quel terme désigne la création de faux profils ou l''utilisation d''identités fictives en ligne pour tromper ou harceler les autres ?', 'E', 1),
(144, 'Quelle action constitue une violation de la vie privée en diffusant des informations sans le consentement afin de lui nuire ?', 'E', 2),
(145, 'Le ver Warhol est un …', 'E', 2),
(146, 'Beta Bot est …', 'E', 2),
(147, 'Quel terme désigne un logiciel malveillant qui se dissimule dans un programme légitime et s''active lorsqu''il est exécuté ?', 'D', 2),
(148, 'Quel terme désigne un programme informatique malveillant qui se duplique et se propage d''un ordinateur à un autre sans nécessiter d''hôte ?', 'D', 2),
(149, 'Qu''est-ce que la cyberhygiène ?', 'D', 2),
(150, 'Qu''est-ce que l''authentification à deux facteurs ?', 'D', 5),
(151, 'Dans quelle proportion les failles de sécurité informatique sont-elles dues à une erreur humaine ?', 'D', 5),
(152, 'Pour leurs usages quotidiens (naviguer sur Internet, lire leurs courriels, utiliser des logiciels de bureautique), les salariés doivent utiliser', 'D', 5),
(153, 'Par rapport à un ordinateur, un smartphone est :', 'D', 2),
(154, 'Parmi ces attaques, laquelle n’est pas une cyberattaque', 'E', 5),
(155, 'Quel préfixe d''URL est un gage de sécurité ?', 'D', 2),
(156, 'Lequel des éléments suivants constitue un type de cybersécurité ?', 'E', 2),
(157, 'Lequel des éléments suivants ne constitue pas un cybercrime ?', 'D', 5),
(158, 'Le ‘cyberespace’ a été inventé par ', 'E', 2),
(159, 'En quelle année le piratage informatique est-il devenu un délit pratique et un sujet de préoccupation dans le domaine de la Cybertechnologie ?', 'E', 2),
(160, 'L''approche de piratage par laquelle les cybercriminels conçoivent de faux sites Web ou de fausses pages pour tromper ou obtenir du trafic supplémentaire…', 'E', 2),
(161, 'Parmi les éléments suivants, lesquels sont généralement ciblés par les cyber-attaquants pour récupérer l''adresse IP d''un utilisateur cible ou victime ?', 'E', 2),
(162, 'Parmi les attaques DDoS suivantes dans les systèmes mobiles, lesquelles attendent que le propriétaire déclenche la cyberattaque ?', 'E', 2),
(163, 'Parmi les propositions suivantes, laquelle est la norme de cryptage de sécurité la moins sécurisé ?', 'E', 2),
(164, 'Lequel des éléments suivants est un Stuxnet ?', 'E', 2),
(165, 'Méthode d''injection de code utilisée pour attaquer la base de données d''un système/site Web', 'E', 2),
(166, 'XSS c’est…', 'E', 2),
(167, 'Une tentative visant à nuire, à endommager ou à menacer un système ou un réseau est généralement qualifiée de', 'F', 2),
(168, 'Quelle méthode de piratage enregistrera toutes vos frappes au clavier ?', 'E', 2),
(169, 'Pourquoi la confidentialité, l’intégrité, l’authenticité et la disponibilité sont-ils considérés comme fondamentaux ?', 'E', 5),
(170, 'Comment se nomme la phase de collecte d’informations dans le piratage éthique auprès de l’utilisateur cible ?', 'E', 2),
(171, 'Lequel des éléments suivants est un exemple de reconnaissance passive ?', 'D', 2),
(172, 'Lequel d’entre eux n’est pas un outil d’analyse ?', 'E', 5),
(173, 'Quel est le nom de la première conférence des hackers ?', 'E', 2),
(174, 'Quelle est la plus ancienne technique de piratage téléphonique utilisée par les pirates pour passer des appels gratuits ?', 'E', 2),
(175, 'En quelle année les informaticiens tentent d''intégrer les techniques de chiffrement dans le protocole TCP/IP ?', 'E', 5),
(176, 'Le masquage des données est également appelé', 'E', 5),
(177, '…. automatise une action ou une attaque afin que les tâches répétitives soient effectuées plus rapidement', 'D', 2),
(178, 'Dans adresse IP, IP c’est …', 'D', 2),
(179, 'La chasse à la baleine/fraude au président est la technique utilisée sur tout individu pour recueillir des informations approfondies et . …', 'D', 2),
(180, 'Lequel des éléments suivants ne relève pas de l’ingénierie sociale ?', 'D', 2),
(181, 'Quelle est la première phase du piratage éthique.', 'E', 2),
(182, 'Cela permet à un pirate informatique d''ouvrir un programme (application) et de le reconstruire avec des fonctionnalités et des capacités supplémentaires..', 'E', 2),
(183, 'Lequel des éléments suivants ne relève pas des compétences incontournables des hackers ?', 'D', 2),
(184, 'Pour pirater une base de données ou accéder et manipuler des données, lequel des langages suivants le pirate informatique doit-il connaître ?', 'E', 5),
(185, 'Lequel des éléments suivants ne constitue pas un type de cybercriminalité peer-to-peer ?', 'E', 5),
(186, 'Le shoulder surfing (ou regard discret par-dessus une épaule) afin de vérifier le mot de passe d''autrui est une pratique ..', 'E', 2),
(187, 'Quel ‘outil’ a maintenant évolué pour devenir l’un des outils automatisés les plus populaires pour le piratage contraire à l’éthique', 'E', 2),
(188, 'Quelle est, parmi les propositions ci-dessous, la plus virale d’Internet ?', 'E', 1),
(189, 'Laquelle d’entre elles constitue une mesure appropriée pour sécuriser un compte sur un réseau social ?', 'D', 1),
(190, 'Pour la vérification légitime des comptes, de nombreux sites de médias sociaux utilisent …', 'D', 1),
(191, 'Dans lequel des cas suivants, une personne est constamment suivie / poursuivie par une autre personne ou un groupe de plusieurs personnes ?', 'E', 1),
(192, 'Qu''est-ce que la géolocalisation sur les réseaux sociaux ?', 'E', 1),
(193, 'Comment pouvez-vous vérifier la véracité des informations partagées sur les réseaux sociaux ?', 'F', 1),
(194, 'L’enregistrement des mots de passe dans le navigateur est une bonne habitude.', 'F', 1),
(195, 'Lequel des éléments suivants n’est pas un avantage de l’attaque par dictionnaire ?', 'E', 2),
(196, 'L’utilisation de logiciels espions est un exemple de collecte de renseignements de type …', 'E', 2),
(197, 'Quel virus infecte l’enregistrement de boot maître et qui est difficile à supprimer ?', 'F', 2),
(198, 'Lequel des types de données suivants, les ‘hameçonneurs’ ne peuvent pas voler à ses victimes cibles ?', 'D', 2),
(199, 'C’est tout ce que votre moteur de recherche ne peut pas rechercher..', 'E', 2),
(200, 'Qui a inventé le terme « deep web » en 2001 ?', 'E', 2);

-- --------------------------------------------------------

--
-- Structure de la table `questionquiz`
--

CREATE TABLE IF NOT EXISTS `questionquiz` (
  `numeroQuestion` int(11) NOT NULL COMMENT 'Numéro de la question',
  `numeroQuiz` int(11) NOT NULL COMMENT 'Numéro du quiz'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `quiz`
--

CREATE TABLE IF NOT EXISTS `quiz` (
`numero` int(11) NOT NULL COMMENT 'numero du quiz',
  `titre` varchar(50) NOT NULL COMMENT 'titre du quiz',
  `description` varchar(200) DEFAULT NULL COMMENT 'description du quiz',
  `dateCreation` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'date de creation du quiz',
  `dureeEstimee` time DEFAULT NULL COMMENT 'duree estimee pour le quiz',
  `nombreQuestions` smallint(6) NOT NULL COMMENT 'nombre de questions du quiz',
  `niveauDifficulte` varchar(1) NOT NULL COMMENT 'niveau de la difficulte du quiz',
  `theme` int(11) NOT NULL COMMENT 'theme du quiz'
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COMMENT='table stockant les quiz' AUTO_INCREMENT=22 ;

--
-- Contenu de la table `quiz`
--

INSERT INTO `quiz` (`numero`, `titre`, `description`, `dateCreation`, `dureeEstimee`, `nombreQuestions`, `niveauDifficulte`, `theme`) VALUES
(7, 'Quiz Facile Thème 1', NULL, '2023-11-06 15:42:02', NULL, 15, 'F', 1),
(8, 'Quiz Facile Thème 2', NULL, '2023-11-06 15:42:02', NULL, 15, 'F', 2),
(9, 'Quiz Facile Thème 3', NULL, '2023-11-06 15:42:02', NULL, 15, 'F', 3),
(10, 'Quiz Facile Thème 4', NULL, '2023-11-06 15:42:02', NULL, 15, 'F', 4),
(11, 'Quiz Facile Thème 5', NULL, '2023-11-06 15:42:02', NULL, 15, 'F', 5),
(12, 'Quiz Expert Thème 1', NULL, '2023-11-06 15:42:02', NULL, 15, 'E', 1),
(13, 'Quiz Expert Thème 2', NULL, '2023-11-06 15:42:02', NULL, 15, 'E', 2),
(14, 'Quiz Expert Thème 3', NULL, '2023-11-06 15:42:02', NULL, 15, 'E', 3),
(15, 'Quiz Expert Thème 4', NULL, '2023-11-06 15:42:02', NULL, 15, 'E', 4),
(16, 'Quiz Expert Thème 5', NULL, '2023-11-06 15:42:02', NULL, 15, 'E', 5),
(17, 'Quiz Difficile Thème 1', NULL, '2023-11-06 15:42:02', NULL, 15, 'D', 1),
(18, 'Quiz Difficile Thème 2', NULL, '2023-11-06 15:42:02', NULL, 15, 'D', 2),
(19, 'Quiz Difficile Thème 3', NULL, '2023-11-06 15:42:02', NULL, 15, 'D', 3),
(20, 'Quiz Difficile Thème 4', NULL, '2023-11-06 15:42:02', NULL, 15, 'D', 4),
(21, 'Quiz Difficile Thème 5', NULL, '2023-11-06 15:42:02', NULL, 15, 'D', 5);

-- --------------------------------------------------------

--
-- Structure de la table `quizjoueur`
--

CREATE TABLE IF NOT EXISTS `quizjoueur` (
`id` int(11) NOT NULL COMMENT 'id du joueur',
  `nom` varchar(50) NOT NULL COMMENT 'nom du joueur',
  `prenom` varchar(50) NOT NULL COMMENT 'prénom du joueur',
  `dateNaissance` date DEFAULT NULL COMMENT 'date de naissance du joueur',
  `mail` varchar(200) DEFAULT NULL COMMENT 'mail du joueur',
  `login` varchar(50) DEFAULT NULL COMMENT 'login du joueur',
  `motPasse` varchar(500) NOT NULL COMMENT 'mot de passe du joueur',
  `sel` varchar(8) NOT NULL COMMENT 'Le sel du mot de passe',
  `isAdmin` tinyint(1) NOT NULL DEFAULT '0',
  `section` int(11) DEFAULT NULL COMMENT 'section du joueur'
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=41 ;

--
-- Contenu de la table `quizjoueur`
--

INSERT INTO `quizjoueur` (`id`, `nom`, `prenom`, `dateNaissance`, `mail`, `login`, `motPasse`, `sel`, `isAdmin`, `section`) VALUES
(39, 'Augis', 'Arthur', NULL, NULL, '', '05949505da7be54ac13f3f21337e21a7ccf86e1160814155504c5039a0f50b6b4781a49b010658fb61d81027501cbef5563caa1c73d2b154251c9cae59c63896', 'a3b7ad5e', 1, NULL),
(40, 'bob', 'bob', NULL, NULL, 'bobbob', '4feff37297cfd9ea6e5713e8c2f438f1043d80418bddfbe81130e1b5156456b898c227e41c4a7841dd677b4abefd74052600c1d57e240da30b8723ee2d52fb93', '11c53061', 0, NULL);

-- --------------------------------------------------------

--
-- Structure de la table `reponse`
--

CREATE TABLE IF NOT EXISTS `reponse` (
  `numeroQuestion` int(11) NOT NULL COMMENT 'cle etrangere vers la table question',
  `numeroProposition` int(11) NOT NULL COMMENT 'cle etrangere vers la table proposition',
  `bonneReponse` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='table liaison question/proposition';

--
-- Contenu de la table `reponse`
--

INSERT INTO `reponse` (`numeroQuestion`, `numeroProposition`, `bonneReponse`) VALUES
(1, 9, 0),
(1, 10, 0),
(1, 11, 1),
(1, 12, 0),
(2, 13, 0),
(2, 14, 1),
(2, 15, 0),
(2, 16, 0),
(3, 5, 0),
(3, 6, 0),
(3, 7, 0),
(3, 8, 1),
(4, 1, 0),
(4, 2, 1),
(4, 3, 0),
(4, 4, 0),
(5, 17, 1),
(5, 18, 0),
(5, 19, 0),
(5, 20, 0),
(6, 21, 0),
(6, 22, 0),
(6, 23, 1),
(6, 24, 0),
(7, 13, 0),
(7, 14, 1),
(7, 25, 0),
(7, 26, 0),
(8, 27, 0),
(8, 28, 0),
(8, 29, 1),
(8, 30, 0),
(9, 31, 0),
(9, 32, 0),
(9, 33, 0),
(9, 34, 1),
(10, 35, 0),
(10, 36, 0),
(10, 37, 0),
(10, 38, 1),
(11, 39, 0),
(11, 40, 1),
(11, 41, 0),
(11, 42, 0),
(12, 43, 0),
(12, 44, 0),
(12, 45, 1),
(12, 46, 0),
(13, 13, 1),
(13, 14, 0),
(13, 16, 0),
(13, 47, 0),
(14, 13, 0),
(14, 48, 0),
(14, 49, 0),
(14, 50, 1),
(15, 51, 0),
(15, 52, 0),
(15, 53, 0),
(15, 54, 1),
(16, 55, 0),
(16, 56, 1),
(16, 57, 0),
(16, 58, 0),
(17, 59, 0),
(17, 60, 1),
(17, 61, 0),
(17, 62, 0),
(18, 63, 0),
(18, 64, 1),
(18, 65, 0),
(18, 66, 0),
(19, 67, 1),
(19, 68, 0),
(19, 69, 0),
(19, 70, 0),
(20, 71, 0),
(20, 72, 0),
(20, 73, 1),
(20, 74, 0),
(21, 75, 0),
(21, 76, 1),
(21, 77, 0),
(21, 78, 0),
(22, 79, 0),
(22, 80, 0),
(22, 81, 1),
(22, 82, 0),
(23, 83, 1),
(23, 84, 0),
(23, 85, 0),
(23, 86, 0),
(24, 83, 0),
(24, 84, 1),
(24, 85, 0),
(24, 86, 0),
(25, 83, 0),
(25, 84, 0),
(25, 85, 0),
(25, 86, 1),
(26, 87, 1),
(26, 88, 0),
(26, 89, 0),
(26, 90, 0),
(27, 91, 0),
(27, 92, 1),
(27, 93, 0),
(27, 94, 0),
(28, 95, 0),
(28, 96, 0),
(28, 97, 1),
(28, 98, 0),
(29, 99, 0),
(29, 100, 0),
(29, 101, 1),
(29, 102, 0),
(30, 103, 1),
(30, 104, 0),
(30, 105, 0),
(30, 106, 0),
(31, 107, 1),
(31, 108, 0),
(31, 109, 0),
(31, 110, 0),
(32, 111, 0),
(32, 112, 1),
(32, 113, 0),
(32, 114, 0),
(33, 115, 0),
(33, 116, 0),
(33, 117, 1),
(33, 118, 0),
(34, 119, 0),
(34, 120, 1),
(34, 121, 0),
(34, 122, 0),
(35, 123, 1),
(35, 124, 0),
(35, 125, 0),
(35, 126, 0),
(36, 127, 1),
(36, 128, 0),
(36, 129, 0),
(36, 130, 0),
(37, 131, 0),
(37, 132, 0),
(37, 133, 0),
(37, 134, 1),
(38, 23, 0),
(38, 24, 1),
(38, 135, 0),
(38, 136, 0),
(39, 137, 1),
(39, 138, 0),
(39, 139, 0),
(39, 140, 0),
(40, 141, 1),
(40, 142, 0),
(40, 143, 0),
(40, 144, 0),
(41, 145, 0),
(41, 146, 1),
(41, 147, 0),
(41, 148, 0),
(42, 149, 0),
(42, 150, 0),
(42, 151, 0),
(42, 152, 1),
(43, 153, 0),
(43, 154, 1),
(43, 155, 0),
(43, 156, 0),
(44, 157, 0),
(44, 158, 1),
(44, 159, 0),
(44, 160, 0),
(45, 161, 1),
(45, 162, 0),
(45, 163, 0),
(45, 164, 0),
(46, 165, 0),
(46, 166, 1),
(46, 167, 0),
(46, 168, 0),
(47, 169, 0),
(47, 170, 0),
(47, 171, 1),
(47, 172, 0),
(48, 173, 0),
(48, 174, 0),
(48, 175, 1),
(48, 176, 0),
(49, 177, 0),
(49, 178, 1),
(49, 179, 0),
(49, 180, 0),
(50, 181, 0),
(50, 182, 1),
(50, 183, 0),
(50, 184, 0),
(51, 185, 1),
(51, 186, 0),
(51, 187, 0),
(51, 188, 0),
(52, 189, 0),
(52, 190, 0),
(52, 191, 1),
(52, 192, 0),
(53, 193, 0),
(53, 194, 1),
(53, 195, 0),
(53, 196, 0),
(54, 197, 0),
(54, 198, 1),
(54, 199, 0),
(54, 200, 0),
(55, 201, 1),
(55, 202, 0),
(55, 203, 0),
(55, 204, 0),
(56, 201, 0),
(56, 202, 0),
(56, 203, 1),
(56, 204, 0),
(57, 205, 1),
(57, 206, 0),
(57, 207, 0),
(57, 208, 0),
(58, 209, 1),
(58, 210, 0),
(58, 211, 0),
(58, 212, 0),
(59, 213, 0),
(59, 214, 1),
(59, 215, 0),
(59, 216, 0),
(60, 217, 1),
(60, 218, 0),
(60, 219, 0),
(60, 220, 0),
(61, 221, 1),
(61, 222, 0),
(61, 223, 0),
(61, 224, 0),
(62, 225, 0),
(62, 226, 0),
(62, 227, 1),
(62, 228, 0),
(63, 229, 0),
(63, 230, 0),
(63, 231, 1),
(63, 232, 0),
(64, 233, 0),
(64, 234, 0),
(64, 235, 0),
(64, 236, 1),
(65, 237, 0),
(65, 238, 1),
(65, 239, 0),
(65, 240, 0),
(66, 241, 1),
(66, 242, 0),
(66, 243, 0),
(66, 244, 0),
(67, 245, 0),
(67, 246, 0),
(67, 247, 1),
(67, 248, 0),
(68, 249, 1),
(68, 250, 0),
(68, 251, 0),
(68, 252, 0),
(69, 253, 0),
(69, 254, 0),
(69, 255, 1),
(69, 256, 0),
(70, 257, 0),
(70, 258, 0),
(70, 259, 0),
(70, 260, 1),
(71, 261, 1),
(71, 262, 0),
(71, 263, 0),
(71, 264, 0),
(72, 265, 0),
(72, 266, 1),
(72, 267, 0),
(72, 268, 0),
(73, 269, 1),
(73, 270, 0),
(73, 271, 0),
(73, 272, 0),
(74, 273, 0),
(74, 274, 0),
(74, 275, 1),
(74, 276, 0),
(75, 277, 0),
(75, 278, 0),
(75, 279, 0),
(75, 280, 1),
(76, 281, 1),
(76, 282, 0),
(76, 283, 0),
(76, 284, 0),
(77, 285, 0),
(77, 286, 1),
(77, 287, 0),
(77, 288, 0),
(78, 289, 0),
(78, 290, 1),
(78, 291, 0),
(78, 292, 0),
(79, 293, 1),
(79, 294, 0),
(79, 295, 0),
(79, 296, 0),
(80, 297, 1),
(80, 298, 0),
(80, 299, 0),
(80, 300, 0),
(81, 301, 0),
(81, 302, 0),
(81, 303, 1),
(81, 304, 0),
(82, 305, 0),
(82, 306, 0),
(82, 307, 1),
(82, 308, 0),
(83, 309, 0),
(83, 310, 0),
(83, 311, 1),
(83, 312, 0),
(84, 313, 0),
(84, 314, 0),
(84, 315, 1),
(84, 316, 0),
(85, 317, 0),
(85, 318, 0),
(85, 319, 0),
(85, 320, 1),
(86, 321, 0),
(86, 322, 0),
(86, 323, 1),
(86, 324, 0),
(87, 325, 0),
(87, 326, 0),
(87, 327, 0),
(87, 328, 1),
(88, 329, 1),
(88, 330, 0),
(88, 331, 0),
(88, 332, 0),
(89, 333, 1),
(89, 334, 0),
(89, 335, 0),
(89, 336, 0),
(90, 337, 0),
(90, 338, 0),
(90, 339, 1),
(90, 340, 0),
(91, 341, 0),
(91, 342, 0),
(91, 343, 1),
(91, 344, 0),
(92, 345, 1),
(92, 346, 0),
(92, 347, 0),
(92, 348, 0),
(93, 349, 1),
(93, 350, 0),
(93, 351, 0),
(93, 352, 0),
(94, 353, 0),
(94, 354, 0),
(94, 355, 0),
(94, 356, 1),
(95, 357, 0),
(95, 358, 1),
(95, 359, 0),
(95, 360, 0),
(96, 361, 0),
(96, 362, 0),
(96, 363, 1),
(96, 364, 0),
(97, 365, 1),
(97, 366, 0),
(97, 367, 0),
(97, 368, 0),
(98, 369, 0),
(98, 370, 0),
(98, 371, 0),
(98, 372, 1),
(99, 373, 0),
(99, 374, 0),
(99, 375, 1),
(99, 376, 0),
(100, 377, 0),
(100, 378, 1),
(100, 379, 0),
(100, 380, 0),
(101, 381, 0),
(101, 382, 0),
(101, 383, 1),
(101, 384, 0),
(102, 385, 1),
(102, 386, 0),
(102, 387, 0),
(102, 388, 0),
(103, 393, 0),
(103, 394, 1),
(103, 395, 0),
(103, 396, 0),
(104, 397, 0),
(104, 398, 1),
(104, 399, 0),
(104, 400, 0),
(105, 401, 0),
(105, 402, 0),
(105, 403, 1),
(105, 404, 0),
(106, 405, 0),
(106, 406, 0),
(106, 407, 0),
(106, 408, 1),
(107, 409, 1),
(107, 410, 0),
(107, 411, 0),
(107, 412, 0),
(108, 413, 0),
(108, 414, 1),
(108, 415, 0),
(108, 416, 0),
(109, 417, 1),
(109, 418, 0),
(109, 419, 0),
(109, 420, 0),
(110, 421, 0),
(110, 422, 1),
(110, 423, 0),
(110, 424, 0),
(111, 421, 1),
(111, 422, 0),
(111, 423, 0),
(111, 424, 0),
(112, 425, 0),
(112, 426, 0),
(112, 427, 1),
(112, 428, 0),
(113, 429, 0),
(113, 430, 1),
(113, 431, 0),
(113, 432, 0),
(114, 433, 0),
(114, 434, 0),
(114, 435, 1),
(114, 436, 0),
(115, 437, 0),
(115, 438, 0),
(115, 439, 1),
(115, 440, 0),
(116, 441, 0),
(116, 442, 1),
(116, 443, 0),
(116, 444, 0),
(117, 441, 1),
(117, 442, 0),
(117, 443, 0),
(117, 444, 0),
(118, 445, 0),
(118, 446, 0),
(118, 447, 0),
(118, 448, 1),
(119, 445, 0),
(119, 446, 0),
(119, 449, 1),
(119, 450, 0),
(120, 451, 0),
(120, 452, 0),
(120, 453, 0),
(120, 454, 1),
(121, 455, 0),
(121, 456, 0),
(121, 457, 1),
(121, 458, 0),
(122, 459, 1),
(122, 460, 0),
(122, 461, 0),
(122, 462, 0),
(123, 463, 1),
(123, 464, 0),
(123, 465, 0),
(123, 466, 0),
(124, 467, 1),
(124, 468, 0),
(124, 469, 0),
(124, 470, 0),
(125, 471, 0),
(125, 472, 0),
(125, 473, 1),
(125, 474, 0),
(126, 475, 0),
(126, 476, 0),
(126, 477, 1),
(126, 478, 0),
(127, 479, 0),
(127, 480, 1),
(127, 481, 0),
(127, 482, 0),
(128, 483, 0),
(128, 484, 1),
(128, 485, 0),
(128, 486, 0),
(129, 487, 1),
(129, 488, 0),
(129, 489, 0),
(129, 490, 0),
(130, 491, 0),
(130, 492, 1),
(130, 493, 0),
(130, 494, 0),
(131, 495, 1),
(131, 496, 0),
(131, 497, 0),
(131, 498, 0),
(132, 499, 0),
(132, 500, 1),
(132, 501, 0),
(132, 502, 0),
(133, 503, 1),
(133, 504, 0),
(133, 505, 0),
(133, 506, 0),
(134, 507, 1),
(134, 508, 0),
(134, 509, 0),
(134, 510, 0),
(135, 511, 0),
(135, 512, 0),
(135, 513, 1),
(135, 514, 0),
(136, 515, 1),
(136, 516, 0),
(136, 517, 0),
(136, 518, 0),
(137, 519, 0),
(137, 520, 0),
(137, 521, 1),
(137, 522, 0),
(138, 523, 0),
(138, 524, 0),
(138, 525, 1),
(138, 526, 0),
(139, 527, 0),
(139, 528, 1),
(139, 529, 0),
(139, 530, 0),
(140, 531, 0),
(140, 532, 1),
(140, 533, 0),
(140, 534, 0),
(141, 535, 0),
(141, 536, 1),
(141, 537, 0),
(141, 538, 0),
(142, 539, 1),
(142, 540, 0),
(142, 541, 0),
(142, 542, 0),
(143, 543, 0),
(143, 544, 1),
(143, 545, 0),
(143, 546, 0),
(144, 547, 1),
(144, 548, 0),
(144, 549, 0),
(144, 550, 0),
(145, 551, 1),
(145, 552, 0),
(145, 553, 0),
(145, 554, 0),
(146, 555, 1),
(146, 556, 0),
(146, 557, 0),
(146, 558, 0),
(147, 559, 0),
(147, 560, 1),
(147, 561, 0),
(147, 562, 0),
(148, 559, 1),
(148, 560, 0),
(148, 561, 0),
(148, 562, 0),
(149, 563, 0),
(149, 564, 1),
(149, 565, 0),
(149, 566, 0),
(150, 567, 0),
(150, 568, 1),
(150, 569, 0),
(150, 570, 0),
(151, 571, 0),
(151, 572, 1),
(151, 573, 0),
(151, 574, 0),
(152, 575, 0),
(152, 576, 0),
(152, 577, 1),
(152, 578, 0),
(153, 579, 0),
(153, 580, 1),
(153, 581, 0),
(153, 582, 0),
(154, 583, 0),
(154, 584, 0),
(154, 585, 1),
(154, 586, 0),
(155, 587, 0),
(155, 588, 1),
(155, 589, 0),
(155, 590, 0),
(156, 591, 0),
(156, 592, 0),
(156, 593, 0),
(156, 594, 1),
(157, 85, 0),
(157, 400, 0),
(157, 401, 0),
(157, 595, 1),
(158, 596, 1),
(158, 597, 0),
(158, 598, 0),
(158, 599, 0),
(159, 600, 0),
(159, 601, 1),
(159, 602, 0),
(159, 603, 0),
(160, 604, 1),
(160, 605, 0),
(160, 606, 0),
(160, 607, 0),
(161, 608, 0),
(161, 609, 0),
(161, 610, 1),
(161, 611, 0),
(162, 212, 0),
(162, 404, 0),
(162, 612, 1),
(162, 613, 0),
(163, 614, 0),
(163, 615, 0),
(163, 616, 0),
(163, 617, 1),
(164, 475, 0),
(164, 476, 1),
(164, 560, 0),
(164, 618, 0),
(165, 619, 0),
(165, 620, 1),
(165, 621, 0),
(165, 622, 0),
(166, 623, 1),
(166, 624, 0),
(166, 625, 0),
(166, 626, 0),
(167, 627, 0),
(167, 628, 0),
(167, 629, 1),
(167, 630, 0),
(168, 631, 1),
(168, 632, 0),
(168, 633, 0),
(168, 634, 0),
(169, 635, 0),
(169, 636, 0),
(169, 637, 1),
(169, 638, 0),
(170, 639, 1),
(170, 640, 0),
(170, 641, 0),
(170, 642, 0),
(171, 643, 0),
(171, 644, 0),
(171, 645, 0),
(171, 646, 1),
(172, 647, 0),
(172, 648, 0),
(172, 649, 0),
(172, 650, 1),
(173, 651, 1),
(173, 652, 0),
(173, 653, 0),
(173, 654, 0),
(174, 399, 0),
(174, 655, 1),
(174, 656, 0),
(174, 657, 0),
(175, 658, 1),
(175, 659, 0),
(175, 660, 0),
(175, 661, 0),
(176, 662, 0),
(176, 663, 0),
(176, 664, 1),
(176, 665, 0),
(177, 666, 0),
(177, 667, 0),
(177, 668, 0),
(177, 669, 1),
(178, 670, 0),
(178, 671, 1),
(178, 672, 0),
(178, 673, 0),
(179, 674, 1),
(179, 675, 0),
(179, 676, 0),
(179, 677, 0),
(180, 399, 0),
(180, 550, 1),
(180, 655, 0),
(180, 657, 0),
(181, 678, 0),
(181, 679, 0),
(181, 680, 0),
(181, 681, 1),
(182, 682, 0),
(182, 683, 1),
(182, 684, 0),
(182, 685, 0),
(183, 686, 0),
(183, 687, 0),
(183, 688, 0),
(183, 689, 1),
(184, 690, 1),
(184, 691, 0),
(184, 692, 0),
(184, 693, 0),
(185, 255, 0),
(185, 586, 0),
(185, 694, 0),
(185, 695, 1),
(186, 696, 0),
(186, 697, 0),
(186, 698, 1),
(186, 699, 0),
(187, 401, 1),
(187, 559, 0),
(187, 700, 0),
(187, 701, 0),
(188, 702, 0),
(188, 703, 0),
(188, 704, 0),
(188, 705, 1),
(189, 706, 1),
(189, 707, 0),
(189, 708, 0),
(189, 709, 0),
(190, 710, 0),
(190, 711, 0),
(190, 712, 1),
(190, 713, 0),
(191, 255, 0),
(191, 256, 0),
(191, 714, 1),
(191, 715, 0),
(192, 716, 0),
(192, 717, 0),
(192, 718, 1),
(192, 719, 0),
(193, 720, 0),
(193, 721, 1),
(193, 722, 0),
(193, 723, 0),
(194, 13, 0),
(194, 14, 1),
(194, 724, 0),
(194, 725, 0),
(195, 726, 0),
(195, 727, 0),
(195, 728, 0),
(195, 729, 1),
(196, 730, 1),
(196, 731, 0),
(196, 732, 0),
(196, 733, 0),
(197, 734, 1),
(197, 735, 0),
(197, 736, 0),
(197, 737, 0),
(198, 738, 0),
(198, 739, 0),
(198, 740, 0),
(198, 741, 1),
(199, 742, 1),
(199, 743, 0),
(199, 744, 0),
(199, 745, 0),
(200, 746, 1),
(200, 747, 0),
(200, 748, 0),
(200, 749, 0);

-- --------------------------------------------------------

--
-- Structure de la table `resultat`
--

CREATE TABLE IF NOT EXISTS `resultat` (
  `numeroQuiz` int(11) NOT NULL COMMENT 'Clé primaire représentant le numéro de la question',
  `idJoueur` int(11) NOT NULL COMMENT 'Id du joueur',
  `dateObtention` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP COMMENT 'Date de l''obtention d''une clé',
  `nbBonnesRep` int(11) NOT NULL COMMENT 'Nombre de bonnes réponses'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Contenu de la table `resultat`
--

INSERT INTO `resultat` (`numeroQuiz`, `idJoueur`, `dateObtention`, `nbBonnesRep`) VALUES
(8, 40, '2023-11-06 18:25:06', 1);

-- --------------------------------------------------------

--
-- Structure de la table `section`
--

CREATE TABLE IF NOT EXISTS `section` (
`id` int(11) NOT NULL COMMENT 'id de la section',
  `nom` varchar(20) NOT NULL COMMENT 'nom de la section',
  `annee` varchar(20) NOT NULL COMMENT 'année de la section',
  `specialite` varchar(50) NOT NULL COMMENT 'spécialité de la section'
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=6 ;

--
-- Contenu de la table `section`
--

INSERT INTO `section` (`id`, `nom`, `annee`, `specialite`) VALUES
(3, 'SIO', '2ème', 'SLAM'),
(5, 'SIO', '1ère', 'SISR');

-- --------------------------------------------------------

--
-- Structure de la table `theme`
--

CREATE TABLE IF NOT EXISTS `theme` (
`numero` int(11) NOT NULL COMMENT 'numero du theme',
  `libelle` varchar(50) NOT NULL COMMENT 'nom du theme'
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 COMMENT='table stockant les themes' AUTO_INCREMENT=6 ;

--
-- Contenu de la table `theme`
--

INSERT INTO `theme` (`numero`, `libelle`) VALUES
(1, 'Réseaux Sociaux'),
(2, 'Mots de passes'),
(3, 'Cyber-harcèlement'),
(4, 'Culture Cyber'),
(5, 'Entreprise');

--
-- Index pour les tables exportées
--

--
-- Index pour la table `niveaudifficulte`
--
ALTER TABLE `niveaudifficulte`
 ADD PRIMARY KEY (`id`);

--
-- Index pour la table `propositions`
--
ALTER TABLE `propositions`
 ADD PRIMARY KEY (`numero`);

--
-- Index pour la table `question`
--
ALTER TABLE `question`
 ADD PRIMARY KEY (`numero`), ADD KEY `ce_nivdiff_2` (`niveauDifficulte`), ADD KEY `ce_theme_2` (`theme`);

--
-- Index pour la table `questionquiz`
--
ALTER TABLE `questionquiz`
 ADD PRIMARY KEY (`numeroQuestion`,`numeroQuiz`), ADD KEY `ce_numquiz_2` (`numeroQuiz`);

--
-- Index pour la table `quiz`
--
ALTER TABLE `quiz`
 ADD PRIMARY KEY (`numero`), ADD KEY `ce_nivdiff` (`niveauDifficulte`), ADD KEY `ce_theme` (`theme`);

--
-- Index pour la table `quizjoueur`
--
ALTER TABLE `quizjoueur`
 ADD PRIMARY KEY (`id`), ADD KEY `ce_section` (`section`);

--
-- Index pour la table `reponse`
--
ALTER TABLE `reponse`
 ADD PRIMARY KEY (`numeroQuestion`,`numeroProposition`), ADD KEY `ce_proposition` (`numeroProposition`);

--
-- Index pour la table `resultat`
--
ALTER TABLE `resultat`
 ADD PRIMARY KEY (`numeroQuiz`,`idJoueur`,`dateObtention`), ADD KEY `ce_idjoueur` (`idJoueur`);

--
-- Index pour la table `section`
--
ALTER TABLE `section`
 ADD PRIMARY KEY (`id`);

--
-- Index pour la table `theme`
--
ALTER TABLE `theme`
 ADD PRIMARY KEY (`numero`);

--
-- AUTO_INCREMENT pour les tables exportées
--

--
-- AUTO_INCREMENT pour la table `propositions`
--
ALTER TABLE `propositions`
MODIFY `numero` int(11) NOT NULL AUTO_INCREMENT COMMENT 'numero de la proposition',AUTO_INCREMENT=750;
--
-- AUTO_INCREMENT pour la table `question`
--
ALTER TABLE `question`
MODIFY `numero` int(11) NOT NULL AUTO_INCREMENT COMMENT 'numero de la question',AUTO_INCREMENT=201;
--
-- AUTO_INCREMENT pour la table `quiz`
--
ALTER TABLE `quiz`
MODIFY `numero` int(11) NOT NULL AUTO_INCREMENT COMMENT 'numero du quiz',AUTO_INCREMENT=22;
--
-- AUTO_INCREMENT pour la table `quizjoueur`
--
ALTER TABLE `quizjoueur`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id du joueur',AUTO_INCREMENT=41;
--
-- AUTO_INCREMENT pour la table `section`
--
ALTER TABLE `section`
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id de la section',AUTO_INCREMENT=6;
--
-- AUTO_INCREMENT pour la table `theme`
--
ALTER TABLE `theme`
MODIFY `numero` int(11) NOT NULL AUTO_INCREMENT COMMENT 'numero du theme',AUTO_INCREMENT=6;
--
-- Contraintes pour les tables exportées
--

--
-- Contraintes pour la table `question`
--
ALTER TABLE `question`
ADD CONSTRAINT `ce_nivdiff_2` FOREIGN KEY (`niveauDifficulte`) REFERENCES `niveaudifficulte` (`id`),
ADD CONSTRAINT `ce_theme_2` FOREIGN KEY (`theme`) REFERENCES `theme` (`numero`);

--
-- Contraintes pour la table `questionquiz`
--
ALTER TABLE `questionquiz`
ADD CONSTRAINT `ce_numquestion` FOREIGN KEY (`numeroQuestion`) REFERENCES `question` (`numero`),
ADD CONSTRAINT `ce_numquiz_2` FOREIGN KEY (`numeroQuiz`) REFERENCES `quiz` (`numero`);

--
-- Contraintes pour la table `quiz`
--
ALTER TABLE `quiz`
ADD CONSTRAINT `ce_nivdiff` FOREIGN KEY (`niveauDifficulte`) REFERENCES `niveaudifficulte` (`id`),
ADD CONSTRAINT `ce_theme` FOREIGN KEY (`theme`) REFERENCES `theme` (`numero`);

--
-- Contraintes pour la table `quizjoueur`
--
ALTER TABLE `quizjoueur`
ADD CONSTRAINT `ce_section` FOREIGN KEY (`section`) REFERENCES `section` (`id`);

--
-- Contraintes pour la table `reponse`
--
ALTER TABLE `reponse`
ADD CONSTRAINT `ce_proposition` FOREIGN KEY (`numeroProposition`) REFERENCES `propositions` (`numero`),
ADD CONSTRAINT `ce_question` FOREIGN KEY (`numeroQuestion`) REFERENCES `question` (`numero`);

--
-- Contraintes pour la table `resultat`
--
ALTER TABLE `resultat`
ADD CONSTRAINT `ce_idjoueur` FOREIGN KEY (`idJoueur`) REFERENCES `quizjoueur` (`id`),
ADD CONSTRAINT `ce_numquiz` FOREIGN KEY (`numeroQuiz`) REFERENCES `quiz` (`numero`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
