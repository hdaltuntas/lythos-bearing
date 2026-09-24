"""
The methods beside the general equation — two layers, earthquake, in-situ
tests and rock — each against its published expression.
"""
import math

import pytest

from lythosbearing import insitu, layered, rock, seismic

# --------------------------------------------------------------------------- #
#  Two layers
# --------------------------------------------------------------------------- #

def test_the_punching_coefficient_defaults_to_the_at_rest_value():
    assert layered.punching_coefficient(30.0) == pytest.approx(0.5)
    assert layered.punching_coefficient(30.0, 4.2) == pytest.approx(4.2)


def test_clay_over_clay_punching_is_the_adhesion_term_by_hand():
    """φ₁ = 0: q = q_b + (1 + B/L)·2·ca·H/B − γ₁·H, Meyerhof & Hanna's clay case."""
    got = layered.punching(q_bottom=200.0, q_top=1000.0, B=2.0, L=4.0, H=1.5, Df=1.0,
                           gamma1=18.0, c1=80.0, phi1=0.0, adhesion_ratio=0.9)
    expected = 200.0 + 1.5 * 2.0 * 0.9 * 80.0 * 1.5 / 2.0 - 18.0 * 1.5
    assert got["q_ult"] == pytest.approx(expected)
    assert got["friction"] == 0.0


def test_a_thick_strong_layer_fails_on_its_own():
    got = layered.punching(q_bottom=200.0, q_top=500.0, B=1.0, L=1.0, H=6.0, Df=1.0,
                           gamma1=19.0, c1=0.0, phi1=38.0)
    assert got["q_ult"] == pytest.approx(500.0)
    assert got["governs_top"]


def test_the_load_spreads_two_to_one_by_default():
    got = layered.spread_dimensions(2.0, 3.0, 1.0)
    assert got["B"] == pytest.approx(3.0, abs=1e-3)
    assert got["L"] == pytest.approx(4.0, abs=1e-3)


def test_a_spread_strip_keeps_its_length():
    got = layered.spread_dimensions(2.0, 1.0, 1.0, strip=True)
    assert got["L"] == pytest.approx(1.0)


# --------------------------------------------------------------------------- #
#  Earthquake
# --------------------------------------------------------------------------- #

def test_the_seismic_angle_is_the_tilt_of_the_body_force():
    assert seismic.seismic_angle(0.2, 0.0) == pytest.approx(math.degrees(math.atan(0.2)))
    assert seismic.seismic_angle(0.2, 0.1) == pytest.approx(math.degrees(math.atan(0.2 / 0.9)))


def test_paolucci_and_peckers_factors_by_hand():
    z = seismic.paolucci_pecker(0.15, 30.0)
    assert z["q"] == pytest.approx((1 - 0.15 / math.tan(math.radians(30))) ** 0.35)
    assert z["g"] == z["q"]
    assert z["c"] == pytest.approx(1 - 0.32 * 0.15)


def test_no_shaking_no_reduction():
    assert seismic.paolucci_pecker(0.0, 30.0)["q"] == 1.0


def test_a_coefficient_beyond_tan_phi_leaves_no_frictional_capacity():
    z = seismic.paolucci_pecker(0.8, 30.0)
    assert z["exceeded"] and z["q"] == 0.0 and z["g"] == 0.0


def test_the_structures_inertia_is_added_to_the_loads():
    got = seismic.apply_to_loads(1000.0, 50.0, 0.0, 100.0, 0.0, kh=0.1, kv=0.05, height=3.0)
    assert got["V"] == pytest.approx(950.0)
    assert got["Hb"] == pytest.approx(150.0)
    assert got["Mb"] == pytest.approx(100.0 + 0.1 * 1000.0 * 3.0)


# --------------------------------------------------------------------------- #
#  In-situ tests
# --------------------------------------------------------------------------- #

def test_meyerhofs_spt_rule_for_a_narrow_footing():
    got = insitu.spt_pressure(20, 1.0, 0.5, settlement=25.4)
    fd = 1 + 0.33 * 0.5 / 1.0
    assert got["q_all"] == pytest.approx(19.16 * 20 * fd)


def test_meyerhofs_spt_rule_for_a_wide_footing():
    got = insitu.spt_pressure(20, 3.0, 0.0, settlement=25.4)
    assert got["q_all"] == pytest.approx(11.98 * 20 * ((3.28 * 3 + 1) / (3.28 * 3)) ** 2)


def test_the_two_spt_branches_nearly_meet_at_their_boundary():
    narrow = insitu.spt_pressure(20, 1.22, 0.0)["q_all"]
    wide = insitu.spt_pressure(20, 1.2201, 0.0)["q_all"]
    assert wide == pytest.approx(narrow, rel=0.03)


def test_the_depth_factor_is_capped():
    assert insitu.spt_pressure(20, 1.0, 10.0)["Fd"] == pytest.approx(1.33)


def test_the_cpt_rule_by_hand():
    assert insitu.cpt_pressure(6.0, 1.0, 0.0)["q_all"] == pytest.approx(6000.0 / 30.0)
    assert insitu.cpt_pressure(6.0, 3.0, 0.0)["q_all"] == pytest.approx(
        6000.0 / 50.0 * (3.3 / 3.0) ** 2)


def test_the_friction_angle_correlations_are_plausible():
    assert 30 < insitu.phi_from_spt(15) < 36
    assert 35 < insitu.phi_from_cpt(10.0, 50.0) < 46


def test_menards_bearing_factor_by_hand():
    kp = insitu.menard_kp("sand_a", 2.0, 4.0, 2.0)
    assert kp == pytest.approx(1.0 * (1 + 0.35 * (0.6 + 0.4 * 0.5) * 1.0))
    got = insitu.pmt_capacity(1.0, 0.1, "sand_a", 2.0, 4.0, 2.0, q0=30.0)
    assert got["q_net_ult"] == pytest.approx(kp * 900.0)
    assert got["q_ult"] == pytest.approx(kp * 900.0 + 30.0)


# --------------------------------------------------------------------------- #
#  Rock
# --------------------------------------------------------------------------- #

def test_intact_rock_has_the_hoek_brown_constants_of_intact_rock():
    k = rock.hoek_brown_constants(100.0, 10.0, 0.0)
    assert k["mb"] == pytest.approx(10.0)
    assert k["s"] == pytest.approx(1.0)
    assert k["a"] == pytest.approx(0.5, abs=1e-3)


def test_the_rock_mass_gets_weaker_as_the_gsi_falls():
    strong = rock.equivalent_mohr_coulomb(50.0, 75.0, 10.0)
    weak = rock.equivalent_mohr_coulomb(50.0, 35.0, 10.0)
    assert weak["c"] < strong["c"]
    assert weak["phi"] < strong["phi"]
    assert weak["sigma_cm"] < strong["sigma_cm"]


def test_the_equivalent_strength_is_reported_in_kpa():
    got = rock.equivalent_mohr_coulomb(50.0, 55.0, 10.0)
    assert 1000 < got["c"] < 10000          # a few MPa, in kPa
    assert got["sigma3_max"] == pytest.approx(12.5)


def test_the_cfem_ksp_by_hand():
    got = rock.ksp_capacity(40.0, 1.0, 1.0, 2.0, Df=1.0)
    ksp = (3 + 1.0 / 2.0) / (10 * math.sqrt(1 + 300 * 0.001 / 1.0))
    assert got["Ksp"] == pytest.approx(ksp)
    assert got["q_all"] == pytest.approx(ksp * 40000.0 * 1.2)
    assert got["q_ult"] == pytest.approx(3 * got["q_all"])
    assert got["valid"]


def test_the_ksp_method_says_when_it_is_out_of_range():
    assert not rock.ksp_capacity(40.0, 0.1, 10.0, 2.0)["valid"]
