using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plot_those_lines__Gianmarco;

namespace Plot_those_lines__Gianmarco.Tests
{
    /// <summary>
    /// Classe de tests unitaires vérifiant la levée d'exceptions 
    /// lorsque des données invalides ou non conformes sont transmises à LoadFromFile.
    /// </summary>
    [TestClass]
    public class LoadFromFile_ExceptionTests
    {
        // Service contenant la logique de chargement de données à tester
        private SolarWindService _service = null!;

        // Emplacement du fichier temporaire créé pour la durée du test
        private string _tempFilePath = null!;

        /// <summary>
        /// S'exécute automatiquement AVANT chaque test.
        /// Instancie le service et crée un fichier temporaire vide sur le disque.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _service = new SolarWindService();
            _tempFilePath = Path.GetTempFileName();
        }

        /// <summary>
        /// S'exécute automatiquement APRÈS chaque test.
        /// Supprime le fichier temporaire pour nettoyer l'environnement de test.
        /// </summary>
        [TestCleanup]
        public void TearDown()
        {
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
        }

        /// <summary>
        /// Teste si LoadFromFile lève bien une InvalidDataException lorsque le fichier JSON
        /// ne contient QUE des données issues de la source "ACE" (et donc aucune donnée validée de type "SOLAR1").
        /// </summary>
        [TestMethod]
        // Indique à MSTest que l'exécution de ce test DOIT impérativement déclencher une InvalidDataException
        [ExpectedException(typeof(InvalidDataException))]
        public void LoadFromFile_WithAceDataOnly_ThrowsInvalidDataException()
        {
            // -------------------------------------------------------------------
            // Arrange (Préparation des données)
            // -------------------------------------------------------------------
            // Création d'un contenu JSON ne contenant qu'une donnée provenant de "ACE"
            string jsonContent = @"[
                {
                    ""time_tag"": ""2025-06-25T00:00:00Z"",
                    ""source"": ""ACE"",
                    ""by_gse"": 1.23
                }
            ]";

            // Écriture du JSON dans le fichier temporaire
            File.WriteAllText(_tempFilePath, jsonContent);

            // -------------------------------------------------------------------
            // Act & Assert (Exécution et vérification de l'exception)
            // -------------------------------------------------------------------
            // L'appel à LoadFromFile devrait lever une InvalidDataException.
            // Grâce à l'attribut [ExpectedException], le test réussira uniquement si l'exception est levée.
            _service.LoadFromFile(_tempFilePath);
        }
    }
}