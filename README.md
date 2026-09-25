**English** | [Türkçe](https://github.com/hdaltuntas/lythos-bearing/blob/main/README.tr.md)

# Lythos Bearing

[![Tests](https://github.com/hdaltuntas/lythos-bearing/actions/workflows/tests.yml/badge.svg)](https://github.com/hdaltuntas/lythos-bearing/actions/workflows/tests.yml)
[![PyPI](https://img.shields.io/pypi/v/lythosbearing)](https://pypi.org/project/lythosbearing/)
[![Python](https://img.shields.io/pypi/pyversions/lythosbearing)](https://pypi.org/project/lythosbearing/)

Bearing capacity of shallow foundations, driven from your browser. A rectangular, square,
strip or circular footing on a layered soil profile, under a vertical load, horizontal loads
and moments, is checked by **every method an engineer is likely to be asked for**, side by
side:

1. **The general bearing capacity equation** with the factor sets of **Terzaghi** (1943),
   **Meyerhof** (1963), **Brinch Hansen** (1970), **Vesić** (1973) and **EN 1997-1 Annex D**,
   and **Skempton's** (1951) undrained Nc — each with its own shape, depth, load inclination,
   base tilt, ground slope and (Vesić) compressibility factors; drained, undrained, or
   whichever governs; general or local shear.
2. **Eccentric and inclined loads** — Meyerhof's effective area, the contact pressure under the
   base and the middle-third rule.
3. **Layered ground** — the strength averaged over the Prandtl failure zone, or the two-layer
   **punching** check of Meyerhof & Hanna and the **load-spread** check for a strong layer over
   a weak one.
4. **In-situ tests** — Meyerhof's SPT and CPT rules and Ménard's pressuremeter rule.
5. **Rock** — Hoek–Brown turned into an equivalent c′, φ′, and the CFEM discontinuity-spacing
   method.
6. **Earthquake** — pseudo-static inertia of the structure, and Paolucci & Pecker's reduction
   for the inertia of the soil.
7. **Checks** — bearing, sliding and eccentricity against a factor of safety, or
   **EN 1997-1 Design Approaches 1, 2 and 3**, and the width the foundation needs.

On top of it, a **parametric or reliability study** sweeps any input — a range, or a
distribution — and reports sensitivities and the probability of failure in bearing, sliding
and eccentricity, with a confidence interval and the reliability index β.

The whole program — every label, result text, figure and report — is bilingual in
**English and Turkish**, switchable while it runs.

The interface is a small HTTP server on your own machine, driven from a browser. That
keeps the program usable over a remote session or inside a container, where a desktop
toolkit would need a display it does not have, and it costs no dependency beyond the
standard library.

> This is the sibling of [LythosFEA](https://github.com/hdaltuntas/lythos),
> [Lythos Settle](https://github.com/hdaltuntas/lythos-settle),
> [Lythos Kinematic](https://github.com/hdaltuntas/lythoskinematic),
> [Lythos SPWA](https://github.com/hdaltuntas/lythosspwa) and
> [LythosLE](https://github.com/hdaltuntas/lythosle), and follows the same architecture,
> theme and fonts.

## Screenshots

| Results summary | Comparison of the methods |
|---|---|
| ![Results summary](https://raw.githubusercontent.com/hdaltuntas/lythos-bearing/main/screenshots/bearing_summary.png) | ![Comparison](https://raw.githubusercontent.com/hdaltuntas/lythos-bearing/main/screenshots/bearing_comparison.png) |

| Section and failure mechanism, dark theme, Turkish | Capacity against width |
|---|---|
| ![Section](https://raw.githubusercontent.com/hdaltuntas/lythos-bearing/main/screenshots/bearing_schematic_dark_tr.png) | ![Width](https://raw.githubusercontent.com/hdaltuntas/lythos-bearing/main/screenshots/bearing_width.png) |

| V–H failure envelope | Reliability study |
|---|---|
| ![Envelope](https://raw.githubusercontent.com/hdaltuntas/lythos-bearing/main/screenshots/bearing_envelope.png) | ![Study](https://raw.githubusercontent.com/hdaltuntas/lythos-bearing/main/screenshots/bearing_study.png) |

## Install & run

From [PyPI](https://pypi.org/project/lythosbearing/):

```bash
pip install lythosbearing
lythos-bearing                     # opens the interface in your browser
```

Word reports need `python-docx` and the spreadsheet export of a study needs `openpyxl`;
both are extras: `pip install "lythosbearing[docx,xlsx]"`. Python 3.10+ is required.

From a clone, with nothing installed but the scientific stack:

```bash
pip install numpy matplotlib reportlab
python main.py
```

or install the clone itself with `pip install .` (extras: `pip install ".[docx,xlsx]"`).
`main.py` puts its own directory first on the import path, so the clone's code is what
runs even when `lythosbearing` is also installed.

## Command line

```bash
lythos-bearing                                  # web interface (the default)
lythos-bearing web --port 9000 --lang tr --no-browser
lythos-bearing example -o project.bearing       # a starter project file
lythos-bearing run project.bearing -o report.pdf  # analyse, print the results, write a report
lythos-bearing study project.bearing -o samples.csv
```

`run` and `study` read the same `.bearing` file the interface saves, so a case set up in
the browser can be re-run unattended. The interface listens on port 8781 by default.

## Inputs

* **Foundation:** shape (rectangle, square, strip, circle), B (diameter of a circle), L,
  depth Df, base tilt η, ground slope β.
* **Actions at the base:** V, horizontal loads along B and L, moments about both axes, and
  the variable share of the actions (for the Eurocode's partial factors).
* **Groundwater:** depth of the water table, γw.
* **Soil profile**, from the surface down, one row per layer: thickness, granular or cohesive,
  γ, γsat, c′, φ′, cu, E, ν.
* **Method:** the factor set, drained / undrained / whichever governs, general or local shear,
  and each correction switched on or off, the effective area included.
* **Layered ground:** averaging over the failure zone (with a depth factor), or the two-layer
  punching (Ks, ca/c₁) or load-spread (angle) check.
* **Earthquake:** kh, kv, soil inertia on or off.
* **In-situ test:** SPT N60, CPT qc, or pressuremeter pl, p0 and soil category; the tolerable
  settlement of the SPT / CPT rules.
* **Rock:** σci, GSI, mi, D and the rock mass unit weight; or discontinuity spacing and
  aperture.
* **Verification:** factor of safety, or EN 1997-1 DA1 / DA2 / DA3; required FS against
  sliding; allowable eccentricity; base friction δ/φ′.

## What it computes

| quantity | method |
|---|---|
| Nc, Nq, Nγ | Terzaghi, Meyerhof, Hansen, Vesić, EN 1997-1 Annex D; Skempton's undrained Nc |
| shape, depth, inclination factors | each method's own; EN 1997-1 borrows Hansen's depth factors on request |
| base tilt, ground slope | Hansen, Vesić, EN 1997-1 (base) |
| compressibility | Vesić's rigidity index Ir against Ir,cr |
| undrained, Hansen | the additive 5.14·cu·(1 + s′c + d′c − i′c − b′c − g′c) + q |
| eccentricity | Meyerhof's effective area; Vesić's equivalent rectangle for a circle |
| contact pressure | trapezoid in the middle third, triangle outside it |
| layered profile | strength averaged over the Prandtl zone, (B′/2)·cos φ/cos(45 + φ/2)·e^((π/4 + φ/2)tan φ) |
| strong over weak layer | Meyerhof & Hanna punching; load spread onto the weak layer |
| earthquake | kh·V added to H, V(1 − kv); Paolucci & Pecker's (1 − kh/tan φ)^0.35 |
| SPT / CPT / PMT | Meyerhof (as revised by Bowles), Meyerhof, Ménard |
| rock | Hoek–Brown 2002 equivalent c′, φ′; CFEM Ksp |
| checks | FS on the net capacity; sliding; e/B; EN 1997-1 DA1 / DA2 / DA3 |
| required width | bisection on B until the check is met |

The expressions, their sources and their limits are in
[docs/theory.md](https://github.com/hdaltuntas/lythos-bearing/blob/main/docs/theory.md).

## Figures

Section with the Prandtl failure mechanism · bearing capacity factors against φ′ for every
method · comparison of the methods (ultimate and allowable against the applied pressure) ·
the cohesion, surcharge and self-weight terms · contact pressure and effective area ·
capacity and factor of safety against width and against depth · the V–H failure envelope.
Study figures: one-at-a-time sweep, histogram, scatter, tornado.

## Reports

Choose PDF, self-contained HTML or Word in the header and press *Export report…*. The
report carries the inputs, the geometry and stresses, the capacity of every method, the
factors and terms of the governing one, the other methods, the checks, the Eurocode
verification, the required width, the figures, the warnings, the method notes and — if one
was run — the study, in whichever language the interface is in. All three formats are
assembled from one place, so they say the same thing.

## Project files (`.bearing`)

JSON. *Save* writes the inputs and the study definition; *Open…* reads them back. Missing
entries keep their defaults.

## Modules

| file | content |
|---|---|
| `lythosbearing/factors.py` | Bearing capacity factors and every correction, per method |
| `lythosbearing/capacity.py` | The general equation, effective area, contact pressure, failure zone, sliding |
| `lythosbearing/layered.py` | Two-layer punching and load spread |
| `lythosbearing/seismic.py` | Pseudo-static loads and soil-inertia factors |
| `lythosbearing/insitu.py`, `rock.py` | SPT / CPT / pressuremeter rules; Hoek–Brown and Ksp |
| `lythosbearing/engine.py` | The analysis: profile, strength over the zone, all methods, checks, Eurocode, width |
| `lythosbearing/study.py`, `study_plots.py` | Parametric and reliability studies, P of failure with 95 % CI and β, Spearman sensitivities, CSV / XLSX |
| `lythosbearing/plotting.py`, `plot_style.py`, `render.py` | Matplotlib figures, theme-aware, off-screen |
| `lythosbearing/report.py`, `pdf.py` | Calculation report: one HTML assembly, exported as PDF, HTML or DOCX |
| `lythosbearing/forms.py` | Input schema and readers, with the conditions under which each field applies |
| `lythosbearing/summary.py` | The results as cards and as text, for the browser and the command line alike |
| `lythosbearing/i18n.py` | Every text, English and Turkish, written side by side |
| `lythosbearing/web/` | The local HTTP server, the session, and the browser interface |

## Development

```bash
pip install -e ".[dev]"
pytest -q                 # factors against the published tables, the equation by hand, methods, engine, study, report, web, packaging
ruff check .
```

The tests check the factors against the published tables, the equation against hand
calculations (Prandtl's 5.14·cu, a Vesić square footing term by term, the contact pressure
trapezoid and triangle), each auxiliary method against its expression, the engine's checks
and refusals, the report in all three formats, the input schema and its round trips, and the
interface itself — the session and the HTTP layer both, so the browser is exercised without a
browser.

Releasing to PyPI is described in [docs/releasing.md](https://github.com/hdaltuntas/lythos-bearing/blob/main/docs/releasing.md);
`tools/upload_to_pypi.py` does it from an editor, without a terminal.

## License

Copyright © 2026 Hasan Deniz Altuntaş

Lythos Bearing is free software: you can redistribute it and/or modify it under the terms of the
[GNU Affero General Public License, version 3](https://github.com/hdaltuntas/lythos-bearing/blob/main/LICENSE) as published by the Free Software
Foundation. It is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

Whoever runs a modified version for users over a network must offer them the source of that
version (section 13 of the licence). Versions published before this change were released
under the MIT licence and remain available under it.
