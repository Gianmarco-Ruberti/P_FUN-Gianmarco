using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plot_those_lines__Gianmarco;

namespace Plot_those_lines__Gianmarco.Tests
{
    /// <summary>
    /// Classe de tests unitaires axée sur la vérification des fonctionnalités 
    /// de filtrage de la méthode LoadFromFile dans SolarWindService.
    /// </summary>
    [TestClass]
    public class LoadFromFile_FilteringTests
    {
        // Service testé contenant la logique de lecture et de filtrage
        private SolarWindService _service = null!;

        // Emplacement du fichier temporaire utilisé pour simuler l'entrée JSON
        private string _tempFilePath = null!;

        /// <summary>
        /// S'exécute automatiquement AVANT chaque test.
        /// Initialise l'instance du service et crée un fichier temporaire unique.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _service = new SolarWindService();
            _tempFilePath = Path.GetTempFileName(); // Génère un fichier temporaire vide sur le disque
        }

        /// <summary>
        /// S'exécute automatiquement APRÈS chaque test.
        /// Supprime le fichier temporaire pour nettoyer le système de fichiers.
        /// </summary>
        [TestCleanup]
        public void TearDown()
        {
            // Vérifie l'existence du fichier avant suppression pour éviter tout plantage
            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
        }

        /// <summary>
        /// Teste si LoadFromFile filtre correctement les entrées provenant de sources multiples
        /// pour ne conserver uniquement que celles dont la source est "SOLAR1".
        /// </summary>
        [TestMethod]
        public void LoadFromFile_WithMixedSources_FiltersAndReturnsOnlySolar1()
        {
            // -------------------------------------------------------------------
            // Arrange (Préparation de l'environnement et des données)
            // -------------------------------------------------------------------
            // Définition d'un contenu JSON comportant 2 objets de sources différentes :
            // - L'un provient de "SOLAR1" (doit être conservé)
            // - L'autre provient de "ACE" (doit être ignoré/filtré)
            string jsonContent = @"[
                {
                    ""time_tag"": ""2025-06-25T00:00:00Z"",
                    ""source"": ""SOLAR1"",
                    ""by_gse"": 2.5
                },
                {
                    ""time_tag"": ""2025-06-25T00:00:00Z"",
                    ""source"": ""ACE"",
                    ""by_gse"": 9.9
                }
            ]";

            // Écriture du JSON mélangé dans le fichier temporaire
            File.WriteAllText(_tempFilePath, jsonContent);

            // -------------------------------------------------------------------
            // Act (Exécution de la méthode sous test)
            // -------------------------------------------------------------------
            // Chargement et traitement du fichier via la méthode du service
            List<SolarWindPoint> result = _service.LoadFromFile(_tempFilePath);

            // -------------------------------------------------------------------
            // Assert (Vérification du résultat)
            // -------------------------------------------------------------------
            // Vérifie qu'un seul élément est conservé (l'élément "ACE" a été exclu)
            Assert.AreEqual(1, result.Count);

            // Vérifie que l'élément conservé est bien celui de "SOLAR1" avec sa valeur ByGse (2.5)
            Assert.AreEqual(2.5, result[0].ByGse);
        }
    }
}