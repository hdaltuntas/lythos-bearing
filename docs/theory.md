# Lythos Bearing — theory

What the program computes, how, and where each expression comes from. Symbols: B and L the
width and length of the base (B ≤ L), B′ and L′ their effective values, Df the founding depth,
c′ and φ′ the effective strength, cu the undrained shear strength, γ the unit weight, q the
overburden at the founding depth.

## 1. The general bearing capacity equation

    q_ult = c·Nc·sc·dc·ic·bc·gc·Fc  +  q·Nq·sq·dq·iq·bq·gq·Fq  +  ½·γ·B′·Nγ·sγ·dγ·iγ·bγ·gγ·Fγ

s, d, i, b, g and F are the shape, depth, load-inclination, base-tilt, ground-slope and
compressibility corrections. Each method brings its own factors and its own corrections; a
correction the method does not define is 1 and is reported as 1.

| method | Nq | Nc | Nγ | corrections |
|---|---|---|---|---|
| Terzaghi (1943) | e^((3π/2 − φ)tan φ) / (2 cos²(45 + φ/2)) | (Nq − 1)cot φ, 5.7 at φ = 0 | Bowles's fit 2(Nq + 1)tan φ / (1 + 0.4 sin 4φ) | shape only |
| Meyerhof (1963) | e^(π tan φ)·tan²(45 + φ/2) | (Nq − 1)cot φ, π + 2 at φ = 0 | (Nq − 1)tan(1.4φ) | s, d, i |
| Brinch Hansen (1970) | as Meyerhof | as Meyerhof | 1.5(Nq − 1)tan φ | s, d, i, b, g |
| Vesić (1973) | as Meyerhof | as Meyerhof | 2(Nq + 1)tan φ | s, d, i, b, g, F |
| EN 1997-1 Annex D | as Meyerhof | as Meyerhof | 2(Nq − 1)tan φ | s, i, b (d, g borrowed on request) |
| Skempton (1951) | — | 5(1 + 0.2B/L)(1 + 0.2D/B) ≤ 7.5(1 + 0.2B/L) | — | inside Nc |

Terzaghi gave Nγ as a chart of Kpγ, not a formula. The program uses Bowles's closed-form fit,
which is within a few per cent of the usual tables near φ′ = 30° and further from them at low
angles (about 10 % low at 20°). Where Nγ matters and a closed form is wanted, use one of the
other sets.

### Shape

* Terzaghi: sc = 1 + 0.3B/L, sγ = 1 − 0.2B/L (1.3 and 0.8 for a square); 1.3 and 0.6 for a
  circle.
* Meyerhof: sc = 1 + 0.2Kp·B/L; sq = sγ = 1 + 0.1Kp·B/L for φ ≥ 10°, Kp = tan²(45 + φ/2).
* Hansen, Vesić: sc = 1 + (Nq/Nc)·B/L, sq = 1 + (B/L)tan φ, sγ = 1 − 0.4B/L ≥ 0.6.
* EN 1997-1: sq = 1 + (B′/L′)sin φ′, sγ = 1 − 0.3B′/L′, sc = (sq·Nq − 1)/(Nq − 1);
  undrained sc = 1 + 0.2B′/L′.

A circle takes B/L = 1; a strip has no shape correction.

### Depth

* Meyerhof: dc = 1 + 0.2√Kp·D/B; dq = dγ = 1 + 0.1√Kp·D/B for φ ≥ 10°.
* Hansen and Vesić: k = D/B for D/B ≤ 1, arctan(D/B) beyond; dc = 1 + 0.4k,
  dq = 1 + 2 tan φ (1 − sin φ)²·k, dγ = 1.
* EN 1997-1 Annex D has none; when depth factors are switched on the program uses Hansen's and
  says so in a warning.

### Load inclination

H is the resultant horizontal load, V the vertical load, A′ the effective area, ca the
cohesion (c′ drained, cu undrained).

* Meyerhof: α = arctan(H/V); ic = iq = (1 − α/90°)², iγ = (1 − α/φ)².
* Hansen: iq = [1 − 0.5H/(V + A′·ca·cot φ)]⁵, iγ = [1 − 0.7H/(V + A′·ca·cot φ)]⁵,
  ic = iq − (1 − iq)/(Nq − 1).
* Vesić and EN 1997-1: iq = [1 − H/(V + A′·ca·cot φ)]^m, iγ = [...]^(m+1); Vesić
  ic = iq − (1 − iq)/(Nq − 1), EN 1997-1 ic = (iq·Nq − 1)/(Nq − 1). The exponent is
  m = mB·cos²θ + mL·sin²θ, mB = (2 + B/L)/(1 + B/L), mL = (2 + L/B)/(1 + L/B), θ the direction
  of H in plan.
* Undrained: Vesić ic = 1 − m·H/(A′·cu·Nc); EN 1997-1 ic = ½(1 + √(1 − H/(A′·cu))).

### Base tilt η and ground slope β

* Hansen: bc = 1 − η°/147°, bq = e^(−2η tan φ), bγ = e^(−2.7η tan φ); gc = 1 − β°/147°,
  gq = gγ = (1 − 0.5 tan β)⁵.
* Vesić: bq = bγ = (1 − η tan φ)², bc = bq − (1 − bq)/(Nc tan φ); gq = gγ = (1 − tan β)²,
  gc = gq − (1 − gq)/(Nc tan φ); at φ = 0, bc = 1 − 2η/(π + 2) and gc = 1 − 2β/(π + 2).
* EN 1997-1: bq = bγ = (1 − η tan φ′)², bc = (bq·Nq − 1)/(Nq − 1). Ground slope factors are
  borrowed from Vesić on request.

### Hansen's undrained expression

At φ = 0 Hansen's corrections are added, not multiplied:

    q_ult = 5.14·cu·(1 + s′c + d′c − i′c − b′c − g′c) + q

with s′c = 0.2B′/L′, d′c = 0.4k, i′c = ½ − ½√(1 − H/(A′·cu)), b′c = η°/147°,
g′c = β°/147°. The program writes it out this way, so a correction with nothing to correct is
zero here and one everywhere else.

### Compressibility (Vesić)

Rigidity index Ir = G/(c + q̄·tan φ), with G from E and ν and q̄ the effective overburden at
B/2 below the base. Critical Ir,cr = ½·exp[(3.30 − 0.45B/L)·cot(45 − φ/2)]. Below it,

    Fq = Fγ = exp{(−4.4 + 0.6B/L)tan φ + 3.07 sin φ·log₁₀(2Ir)/(1 + sin φ)}
    Fc = Fq − (1 − Fq)/(Nq tan φ)          (φ = 0: Fc = 0.32 + 0.12B/L + 0.60 log₁₀ Ir)

### Local shear

Terzaghi's reduction for loose or soft soil: c* = 2c/3, φ* = arctan(2 tan φ/3), used in
place of c and φ throughout.

## 2. Surcharge, unit weight and the failure zone

The surcharge q is the effective overburden σ′v0(Df) in a drained analysis and the total
overburden σv0(Df) in an undrained one. Below the base the unit weight is γ above the water
table and γsat − γw below it (drained).

The failure zone reaches the deepest point of Prandtl's log spiral, r = r₀·e^(θ tan φ) with
r₀ = (B′/2)/cos(45 + φ/2), which lies where the radius makes 90° − φ with the horizontal:

    z_max = (B′/2)·cos φ / cos(45 + φ/2) · e^((π/4 + φ/2)·tan φ)

0.707·B′ at φ = 0, about 1.6·B′ at 30°. The strength of a layered profile is averaged over
that depth (times the *failure zone factor*): tan φ, c′, γ, E and ν by thickness, and cu over
the cohesive layers only — a sand is drained whatever the rate of loading. The zone depends on
the φ it averages, so the two are settled by repetition.

"Whichever governs" runs both analyses and keeps the one with the lower net capacity.

## 3. Eccentric load

e_B = M_B/V, e_L = M_L/V. Meyerhof's effective area is the rectangle B′ = B − 2e_B,
L′ = L − 2e_L centred on the resultant. For a circle of radius R and eccentricity e, the
loaded segment A′ = 2[R²·arccos(e/R) − e√(R² − e²)] is replaced by the equivalent rectangle
with L′/B′ = √((R + e)/(R − e)) (Vesić).

The contact pressure on the whole base is reported beside it: q = V/A·(1 ± 6e_B/B ± 6e_L/L)
inside the middle third, and the triangle q_max = 2V/(3L(B/2 − e)) over 3(B/2 − e) outside it.

## 4. Two layers

Meyerhof & Hanna (1978), a strong layer of thickness H below the base over a weak one:

    q_ult = q_b + (1 + B/L)·[2·ca·H/B + γ₁·H²·(1 + 2Df/H)·Ks·tan φ₁/B] − γ₁·H  ≤  q_t

q_b is the capacity of the lower layer with the footing brought down onto it, q_t that of the
top layer alone. Each layer is taken in the condition that governs it (a sand drained, a clay
with a cu undrained). Meyerhof and Hanna read Ks off a chart against φ₁ and q₂/q₁; the program
uses the value entered, or 1 − sin φ₁ — Bowles's suggestion, on the safe side of the chart.
ca = (adhesion ratio)·c₁.

The alternative *load spread* check spreads the load from the base at the chosen angle (2:1 by
default) and checks the weak layer as a footing of the spread size founded on its surface.

A two-layer result governs when it is lower than the averaged capacity.

## 5. Earthquake (pseudo-static)

The structure's inertia kh·V is added to the horizontal load along B and V becomes V(1 − kv)
(kv positive upwards); γ and q are scaled by (1 − kv) too. The seismic tilt of the body force is
ψ = arctan(kh/(1 − kv)). The soil's own inertia reduces the terms by Paolucci & Pecker (1997):

    z_q = z_γ = (1 − kh/tan φ)^0.35         z_c = 1 − 0.32·kh

written for a strip on dry soil and valid for kh < tan φ; beyond that the frictional terms are
zero and a warning says so.

## 6. In-situ tests

* SPT, Meyerhof (1956) as revised by Bowles, net pressure for a settlement Se [mm]:
  q = 19.16·N60·Fd·(Se/25.4) for B ≤ 1.22 m,
  q = 11.98·N60·((3.28B + 1)/(3.28B))²·Fd·(Se/25.4) above, Fd = 1 + 0.33D/B ≤ 1.33.
  φ′ from Wolff (1989): 27.1 + 0.3N60 − 0.00054N60².
* CPT, Meyerhof: q = qc/30 for B ≤ 1.2 m, (qc/50)·((B + 0.3)/B)² above, scaled by Se/25 and
  Fd. φ′ from Robertson & Campanella (1983): arctan[0.1 + 0.38 log₁₀(qc/σ′v0)].
* Pressuremeter, Ménard: q_net,ult = kp·(pl − p0),
  kp = kp₀·[1 + a·(0.6 + 0.4B/L)·De/B] with (kp₀, a) = (0.8, 0.25) clay/silt A,
  (0.8, 0.35) clay/silt B, (1.0, 0.35) sand A, (1.3, 0.50) sand and gravel B,
  (1.0, 0.27) weathered rock; De/B capped at 2.

The SPT and CPT rules are settlement rules: they give an allowable pressure, not a rupture
capacity, and are shown for comparison only.

## 7. Rock

* Hoek–Brown (Hoek, Carranza-Torres & Corkum 2002):
  mb = mi·e^((GSI − 100)/(28 − 14D)), s = e^((GSI − 100)/(9 − 3D)),
  a = ½ + (e^(−GSI/15) − e^(−20/3))/6. The equivalent c′ and φ′ are fitted up to
  σ3max = σci/4 and run through the general equation of the chosen method.
* CFEM discontinuity spacing: Ksp = (3 + s/B)/(10√(1 + 300δ/s)), q_all = Ksp·σci·d,
  d = 1 + 0.4D/B ≤ 3.4. A factor of safety of about 3 is inside Ksp; the program reports
  q_ult = 3·q_all and warns outside s > 0.3 m, δ < 5 mm.

## 8. Checks

* Bearing: FS = q_net,ult / q_net with q_net = V/A′ − q; q_all = q_net,ult/FS + q.
* Sliding: R = V·tan δ + A′·ca (drained, δ = δ/φ′·φ′), R = A′·cu (undrained); FS = R/H.
* Eccentricity: e/B against the chosen limit (B/6, the middle third, by default).
* EN 1997-1 Design Approaches, recommended values of Annex A:

  | set | γG | γQ | γφ′ | γc′ | γcu | γR;v | γR;h |
  |---|---|---|---|---|---|---|---|
  | A1 | 1.35 | 1.50 | | | | | |
  | A2 | 1.00 | 1.30 | | | | | |
  | M1 | | | 1.00 | 1.00 | 1.00 | | |
  | M2 | | | 1.25 | 1.25 | 1.40 | | |
  | R1 / R2 / R3 | | | | | | 1.0 / 1.4 / 1.0 | 1.0 / 1.1 / 1.0 |

  DA1 checks A1+M1+R1 and A2+M2+R1, DA2 A1+M1+R2, DA3 A1+M2+R3. The variable share of the
  actions splits them into G and Q. Rd = (q_net,ult/γR;v)·A′ + q·A′ against Ed = Vd, and the
  sliding resistance divided by γR;h against Hd.
* Required width: the smallest B (30 cm – 60 m) that satisfies the bearing check, by
  bisection with every other input held; a rectangle keeps L/B.

## 9. Limits

* Shallow foundations: the equations lose meaning much beyond D/B ≈ 2–4.
* The failure zone average is a simplification for a layered profile; where a strong layer of
  limited thickness overlies a weak one, use the two-layer check.
* Settlement is not checked here beyond the SPT/CPT rules; see Lythos Settle.
* Terzaghi's Nγ is a fit, Ks defaults to a conservative estimate, and the rock methods are
  empirical: each is flagged where it is used.
