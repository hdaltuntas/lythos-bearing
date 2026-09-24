"""
Lythos Bearing — bearing capacity of shallow foundations, driven from a browser.

The program works out how much load a shallow foundation on a layered soil
profile can carry, by every method an engineer is likely to be asked for, and
checks the foundation against them:

    1. Strength        — the design c', φ' and cu of the failure zone, the
                         effective surcharge at the base and the effective
                         unit weight below it
    2. Capacity        — the general bearing capacity equation with the
                         bearing capacity factors of Terzaghi, Meyerhof,
                         Hansen, Vesić and EN 1997-1, and Skempton's
                         undrained Nc, each with its own shape, depth, load
                         inclination, base tilt, ground slope and
                         compressibility factors
    3. Eccentricity    — Meyerhof's effective area, the contact pressure and
                         the middle-third rule
    4. Layered ground  — strength averaged over the failure zone, or the
                         two-layer punching / load-spread checks
    5. Other methods   — SPT, CPT and pressuremeter correlations, and rock
                         (Hoek–Brown, CFEM Ksp)
    6. Seismic         — the pseudo-static inertia of the structure and of
                         the soil (Paolucci & Pecker)
    7. Checks          — bearing, sliding and eccentricity against a factor
                         of safety, or against EN 1997-1 Design Approaches

On top of it, a parametric or reliability study sweeps any input (a range, or
a distribution) and reports sensitivities and the probability of failure.

The interface is a local web server driven from the browser (standard library
only), so the program also runs over a remote session or inside a container,
where a desktop toolkit would need a display it does not have.

Package layout
--------------
    lythosbearing.config        app identity, defaults, themes, palette
    lythosbearing.i18n          every text of the program, English and Turkish
    lythosbearing.factors       bearing capacity and correction factors
    lythosbearing.capacity      the general bearing capacity equation
    lythosbearing.layered       two-layer punching and load-spread checks
    lythosbearing.seismic       pseudo-static bearing capacity
    lythosbearing.insitu        SPT / CPT / pressuremeter correlations
    lythosbearing.rock          bearing capacity on rock
    lythosbearing.engine        the bearing capacity analysis
    lythosbearing.study         parametric / reliability studies
    lythosbearing.plotting      analysis figures
    lythosbearing.study_plots   study figures
    lythosbearing.report        calculation report: HTML, PDF, DOCX
    lythosbearing.forms         input schema and readers (interface-independent)
    lythosbearing.web           local web server and the browser interface

Run it:  lythos-bearing          (or  python -m lythosbearing)
"""

__version__ = "0.1.0"

APP_NAME = "Lythos Bearing"
ORG = "Lythos"

__all__ = ["__version__", "APP_NAME", "ORG"]
