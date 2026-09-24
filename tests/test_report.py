"""The calculation report in all three formats, and the figures it carries."""
import copy
import zipfile

import pytest
from matplotlib.figure import Figure

from lythosbearing import render, report
from lythosbearing.config import DEFAULT_CONFIG
from lythosbearing.engine import BearingAnalysis
from lythosbearing.plotting import PLOT_KEYS, Plotter


@pytest.fixture(scope="module")
def analysis():
    a = BearingAnalysis(DEFAULT_CONFIG)
    a.run()
    return a


@pytest.mark.parametrize("theme", ["light", "dark", "paper"])
def test_every_figure_draws_in_every_theme(analysis, theme):
    for key in PLOT_KEYS:
        fig = Figure(figsize=(8, 6))
        Plotter(analysis, "en", theme).draw(key, fig)
        assert fig.axes, key


def test_the_figures_come_out_as_png(analysis):
    png = render.figure_to_png(render.analysis_figure(analysis, "comparison", "tr"))
    assert png[:8] == b"\x89PNG\r\n\x1a\n"


def test_an_unknown_figure_is_refused(analysis):
    with pytest.raises(ValueError):
        render.analysis_figure(analysis, "nonsense")


def test_every_case_draws_its_figures():
    """The less common paths through the figures: a strip, a circle, a triangle
    of pressure, rock, two layers and an earthquake."""
    cases = [
        {"foundation": {"shape": "strip"}},
        {"foundation": {"shape": "circle"}},
        {"loading": {"V": 1000.0, "Mb": 600.0}},
        {"rock": {"enabled": True}, "insitu": {"enabled": True}},
        {"options": {"layer_model": "two_layer"}},
        {"seismic": {"enabled": True, "kh": 0.2}},
        {"options": {"analysis": "undrained"}},
    ]
    for changes in cases:
        cfg = copy.deepcopy(DEFAULT_CONFIG)
        for section, values in changes.items():
            cfg[section].update(values)
        a = BearingAnalysis(cfg)
        a.run()
        for key in PLOT_KEYS:
            fig = Figure(figsize=(8, 6))
            Plotter(a, "en", "light").draw(key, fig)


def test_the_html_report_is_self_contained(analysis, tmp_path):
    path = tmp_path / "r.html"
    report.export_html(str(path), analysis, "en")
    text = path.read_text(encoding="utf-8")
    assert "data:image/png;base64," in text
    assert "Bearing capacity by method" not in text or "3.1 By method" in text
    for word in ("Vesić", "Skempton", "Eccentricity", "Method notes"):
        assert word in text


def test_the_turkish_report_is_in_turkish(analysis, tmp_path):
    path = tmp_path / "r.html"
    report.export_html(str(path), analysis, "tr")
    text = path.read_text(encoding="utf-8")
    assert "Taşıma gücü" in text and "Yöntem notları" in text


def test_the_pdf_report(analysis, tmp_path):
    path = tmp_path / "r.pdf"
    report.export_pdf(str(path), analysis, "tr")
    data = path.read_bytes()
    assert data[:5] == b"%PDF-" and len(data) > 50_000


def test_the_word_report(analysis, tmp_path):
    path = tmp_path / "r.docx"
    report.export_docx(str(path), analysis, "en")
    with zipfile.ZipFile(path) as archive:
        body = archive.read("word/document.xml").decode("utf-8")
    assert "Bearing capacity" in body


def test_the_eurocode_and_the_extra_methods_reach_the_report(tmp_path):
    cfg = copy.deepcopy(DEFAULT_CONFIG)
    cfg["criteria"]["approach"] = "da1"
    cfg["insitu"]["enabled"] = True
    cfg["seismic"]["enabled"] = True
    a = BearingAnalysis(cfg)
    a.run()
    text = report.build_html(a, "en", {})
    assert "DA1-1" in text and "DA1-2" in text
    assert "Other methods" in text and "Earthquake" in text
