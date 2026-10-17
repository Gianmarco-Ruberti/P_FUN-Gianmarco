# Évaluation intermédiaire (80 %) – Gianmarco Ruberti

> Mise à jour du 16.10.2026 : les méthodes d'extension ne sont **pas exigées** (notion non vue en cours), même si le CdC les mentionne.

---

## En bref

Projet fonctionnel et bien documenté : import JSON NOAA avec filtrage de la source, fusion sans doublon, cache local, 5 séries affichables et masquables, 3 tests unitaires significatifs, rapport déjà largement rédigé. **Lacunes principales** : encore des `foreach` dans la logique métier, 5 séries « en dur » (cases à cocher et configuration) et section IA trop générale.

## Axes de travail prioritaires pour la finalisation

1. **Remplacer les `foreach`** du parsing et du tracé par du LINQ.
2. **Rendre la configuration des séries data-driven** : une seule liste (nom, couleur, sélecteur) qui génère cases à cocher et courbes.
3. **Réécrire la section IA** : outil(s) utilisé(s), mode d'utilisation, exemples concrets (prompt, réponse, ce qui a été compris ou corrigé), avec le recul déjà amorcé.
4. **Compléter le rapport** : conclusion, US et maquettes intégrées (pas seulement des liens), correction WinForms/WPF.
