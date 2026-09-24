# Changelog

## 0.1.0

First release: bearing capacity of shallow foundations, built on the architecture of the
other Lythos programs (local HTTP server + browser interface, schema-driven forms, the same
theme and fonts, bilingual English / Turkish throughout, PDF / HTML / DOCX reports,
`.bearing` project files, command line, parametric / reliability studies).

- General bearing capacity equation with the factor sets of Terzaghi, Meyerhof, Brinch
  Hansen, Vesić and EN 1997-1 Annex D, and Skempton's undrained Nc; each method's own shape,
  depth, load inclination, base tilt and ground slope factors, Vesić's compressibility
  factors, Hansen's additive undrained form; general or local shear.
- Drained, undrained, or whichever governs; strength averaged over the Prandtl failure zone,
  cu over the cohesive layers only.
- Eccentric loads: Meyerhof's effective area (Vesić's equivalent rectangle for a circle),
  contact pressure, middle-third check.
- Two layers: Meyerhof & Hanna punching and load spread, each layer in its governing condition.
- SPT, CPT and pressuremeter methods; rock by Hoek–Brown equivalent strength and CFEM Ksp.
- Pseudo-static earthquake: structural inertia and Paolucci & Pecker soil-inertia factors.
- Checks: bearing, sliding and eccentricity against a factor of safety, or EN 1997-1 Design
  Approaches 1, 2 and 3; the required width.
- Figures: section with the failure mechanism, factor curves, method comparison, terms,
  contact pressure, capacity against width and depth, V–H failure envelope.
- Studies: one-at-a-time, Latin hypercube, Monte Carlo; statistics, probability of failure with
  95 % CI and β, Spearman sensitivities, CSV / XLSX export.
