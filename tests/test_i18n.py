"""The two languages stay complete and in step."""
import re

from lythosbearing.i18n import ENTRIES, TRANSLATIONS, t
from lythosbearing.web.strings import REUSED, SHELL


def test_every_entry_has_both_languages():
    for key, pair in ENTRIES.items():
        assert isinstance(pair, tuple) and len(pair) == 2, key
        assert pair[0] and pair[1], key


def test_the_placeholders_are_the_same_in_both_languages():
    for key, (en, tr) in ENTRIES.items():
        assert set(re.findall(r"\{(\w+)", en)) == set(re.findall(r"\{(\w+)", tr)), key


def test_the_shell_reuses_only_keys_that_exist():
    for key in REUSED:
        assert key in TRANSLATIONS["en"], key
    for key, pair in SHELL.items():
        assert len(pair) == 2 and all(pair), key


def test_an_unknown_key_comes_back_as_itself():
    assert t("en", "no_such_key") == "no_such_key"


def test_turkish_is_actually_turkish():
    assert t("tr", "res_title") == "TAŞIMA GÜCÜ SONUÇLARI"
