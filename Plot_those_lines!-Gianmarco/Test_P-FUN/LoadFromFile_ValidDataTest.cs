using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plot_those_lines__Gianmarco;

namespace Plot_those_lines__Gianmarco.Tests
{
    /// <summary>
    /// Classe de tests unitaires vérifiant le chargement de données valides 
    /// depuis un fichier texte/JSON via le service SolarWindService.
    /// </summary>
    [TestClass]
    public class LoadFromFile_ValidDataTests
    {
        // Service contenant la logique métier à tester (chargement / parsing)
        private SolarWindService _service = null!;

        // Chemin d'accès du fichier temporaire créé pendant les tests
        private string _tempFilePath = null!;

        /// <summary>
        /// Méthode exécutée automatiquement AVANT chaque test unitaires.
        /// Prépare l'environnement de test (instanciation et fichier temporaire).
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _service = new SolarWindService();
            _tempFilePath = Path.GetTempFileName(); // Crée un fichier temporaire sur le système
        }

        /// <summary>
        /// Méthode exécutée automatiquement APRÈS chaque test unitaires.
        /// Nettoie les ressources en supprimant le fichier temporaire créé.
        /// </summary>
        [TestCleanup]
        public void TearDown()
        {
            // Vérifie si le fichier temporaire existe toujours avant de le supprimer
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
        }

        /// <summary>
        /// Teste le comportement de LoadFromFile avec du contenu JSON valide (Source SOLAR1).
        /// Suit le patron de conception classique Arrange - Act - Assert (AAA).
        /// </summary>
        [TestMethod]
        public void LoadFromFile_WithValidSolar1Data_ReturnsParsedPoints()
        {
            // -------------------------------------------------------------------
            // Arrange (Préparation des données)
            // -------------------------------------------------------------------
            // Définition d'un contenu JSON simulé représentant un relevé vent solaire
            string jsonContent = @"[
                {
                    ""time_tag"": ""2025-06-25T00:00:00Z"",
                    ""source"": ""SOLAR1"",
                    ""by_gse"": 1.23,
                    ""bz_gse"": -4.56,
                    ""bt"": 5.0
                }
            ]";

            // Écriture du JSON dans le fichier temporaire
            File.WriteAllText(_tempFilePath, jsonContent);

            // -------------------------------------------------------------------
            // Act (Exécution de l'action à tester)
            // -------------------------------------------------------------------
            // Appel de la méthode LoadFromFile pour lire et parser le fichier
            List<SolarWindPoint> result = _service.LoadFromFile(_tempFilePath);

            // -------------------------------------------------------------------
            // Assert (Vérification des résultats obtenus)
            // -------------------------------------------------------------------
            // Vérifie que la liste retournée contient bien un seul élément
            Assert.AreEqual(1, result.Count);

            // Vérifie que chaque champ de l'objet désérialisé correspond exactement aux données du JSON
            Assert.AreEqual(1.23, result[0].ByGse);
            Assert.AreEqual(-4.56, result[0].BzGse);
            Assert.AreEqual(5.0, result[0].Bt);
        }
    }
}