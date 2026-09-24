[English](https://github.com/hdaltuntas/lythos-bearing/blob/main/README.md) | **Türkçe**

# Lythos Bearing

[![Tests](https://github.com/hdaltuntas/lythos-bearing/actions/workflows/tests.yml/badge.svg)](https://github.com/hdaltuntas/lythos-bearing/actions/workflows/tests.yml)
[![PyPI](https://img.shields.io/pypi/v/lythosbearing)](https://pypi.org/project/lythosbearing/)
[![Python](https://img.shields.io/pypi/pyversions/lythosbearing)](https://pypi.org/project/lythosbearing/)

Tarayıcıdan sürülen, sığ temellerin taşıma gücü hesabı. Tabakalı bir zemin profili üzerindeki
dikdörtgen, kare, şerit ya da dairesel bir temel; düşey yük, yatay yükler ve momentler altında,
**bir mühendisten istenebilecek bütün yöntemlerle** yan yana kontrol edilir:

1. **Genel taşıma gücü denklemi** — **Terzaghi** (1943), **Meyerhof** (1963),
   **Brinch Hansen** (1970), **Vesić** (1973) ve **EN 1997-1 Ek D** katsayı takımları ile
   **Skempton'un** (1951) drenajsız Nc'si; her biri kendi şekil, derinlik, yük eğimi, taban
   eğimi, arazi eğimi ve (Vesić) sıkışabilirlik katsayılarıyla; drenajlı, drenajsız ya da
   hangisi belirleyiciyse; genel ya da yerel kayma.
2. **Dışmerkez ve eğik yükler** — Meyerhof etkin alanı, taban basıncı ve orta üçte bir kuralı.
3. **Tabakalı zemin** — Prandtl göçme bölgesinde ortalanan dayanım ya da zayıf tabaka üzerindeki
   sağlam tabaka için Meyerhof & Hanna **zımbalama** ve **yük yayılması** kontrolleri.
4. **Arazi deneyleri** — Meyerhof SPT ve CPT kuralları, Ménard presiyometre kuralı.
5. **Kaya** — eşdeğer c′, φ′'ye çevrilen Hoek–Brown ve CFEM süreksizlik aralığı yöntemi.
6. **Deprem** — yapının psödo-statik ataleti ve zemin ataleti için Paolucci & Pecker azaltması.
7. **Kontroller** — taşıma gücü, kayma ve dışmerkezlik; güvenlik sayısına ya da
   **EN 1997-1 Tasarım Yaklaşımı 1, 2 ve 3**'e göre; temelin gerektirdiği genişlik.

Bunun üzerine bir **parametrik veya güvenilirlik çalışması** istenen girdiyi — bir aralık ya
da bir dağılım olarak — tarar; duyarlılıkları ve taşıma gücü, kayma ve dışmerkezlik için göçme
olasılığını, güven aralığı ve güvenilirlik indeksi β ile raporlar.

Programın tamamı — her etiket, sonuç metni, şekil ve rapor — **Türkçe ve İngilizce** çalışır;
dil çalışma sırasında değiştirilir.

Arayüz, kendi makinenizde çalışan küçük bir HTTP sunucusudur ve tarayıcıdan sürülür. Böylece
program uzak oturumda ya da konteyner içinde de çalışır ve standart kütüphane dışında hiçbir
bağımlılık getirmez.

> [LythosFEA](https://github.com/hdaltuntas/lythos),
> [Lythos Settle](https://github.com/hdaltuntas/lythos-settle),
> [Lythos Kinematic](https://github.com/hdaltuntas/lythoskinematic),
> [Lythos SPWA](https://github.com/hdaltuntas/lythosspwa) ve
> [LythosLE](https://github.com/hdaltuntas/lythosle) programlarının kardeşidir; aynı mimari,
> tema ve yazı tiplerini kullanır.

## Ekran görüntüleri

| Sonuç özeti | Yöntemlerin karşılaştırması |
|---|---|
| ![Özet](https://raw.githubusercontent.com/hdaltuntas/lythos-bearing/main/screenshots/bearing_summary.png) | ![Karşılaştırma](https://raw.githubusercontent.com/hdaltuntas/lythos-bearing/main/screenshots/bearing_comparison.png) |

| Kesit ve göçme mekanizması, koyu tema, Türkçe | Güvenilirlik çalışması |
|---|---|
| ![Kesit](https://raw.githubusercontent.com/hdaltuntas/lythos-bearing/main/screenshots/bearing_schematic_dark_tr.png) | ![Çalışma](https://raw.githubusercontent.com/hdaltuntas/lythos-bearing/main/screenshots/bearing_study.png) |

## Kurulum ve çalıştırma

[PyPI](https://pypi.org/project/lythosbearing/) üzerinden:

```bash
pip install lythosbearing
lythos-bearing                     # arayüzü tarayıcıda açar
```

Word raporu için `python-docx`, çalışmanın Excel çıktısı için `openpyxl` gerekir; ikisi de
isteğe bağlıdır: `pip install "lythosbearing[docx,xlsx]"`. Python 3.10+ gerekir.

Depo kopyasından, yalnızca bilimsel kütüphanelerle:

```bash
pip install numpy matplotlib reportlab
python main.py
```

ya da kopyanın kendisini `pip install .` ile kurarak (ek paketlerle:
`pip install ".[docx,xlsx]"`).

## Komut satırı

```bash
lythos-bearing                                   # web arayüzü (varsayılan)
lythos-bearing web --port 9000 --lang tr --no-browser
lythos-bearing example -o proje.bearing          # başlangıç proje dosyası
lythos-bearing run proje.bearing -o rapor.pdf --lang tr
lythos-bearing study proje.bearing -o ornekler.csv
```

Arayüz varsayılan olarak 8781 numaralı portu dinler.

## Hesaplananlar

| büyüklük | yöntem |
|---|---|
| Nc, Nq, Nγ | Terzaghi, Meyerhof, Hansen, Vesić, EN 1997-1 Ek D; Skempton drenajsız Nc |
| şekil, derinlik, eğim katsayıları | her yöntemin kendi katsayıları |
| taban ve arazi eğimi | Hansen, Vesić, EN 1997-1 (taban) |
| sıkışabilirlik | Vesić rijitlik indeksi Ir ve Ir,kr |
| dışmerkezlik | Meyerhof etkin alanı; dairede Vesić eşdeğer dikdörtgeni |
| tabakalı profil | Prandtl bölgesinde ortalanan dayanım |
| sağlam / zayıf tabaka | Meyerhof & Hanna zımbalama; yük yayılması |
| deprem | kh·V yatay yüke eklenir, V(1 − kv); Paolucci & Pecker |
| SPT / CPT / PMT | Meyerhof (Bowles düzeltmesiyle), Meyerhof, Ménard |
| kaya | Hoek–Brown 2002 eşdeğer c′, φ′; CFEM Ksp |
| kontroller | net taşıma gücü üzerinden GS; kayma; e/B; EN 1997-1 TY1 / TY2 / TY3 |

Denklemler, kaynakları ve sınırları
[docs/theory.md](https://github.com/hdaltuntas/lythos-bearing/blob/main/docs/theory.md)
dosyasındadır.

## Raporlar

Başlıktan PDF, tek dosyalık HTML ya da Word seçilip *Rapor al…* düğmesine basılır. Rapor;
girdileri, geometri ve gerilmeleri, bütün yöntemlerin taşıma gücünü, belirleyici yöntemin
katsayılarını ve terimlerini, diğer yöntemleri, kontrolleri, Eurocode doğrulamasını, gerekli
genişliği, şekilleri, uyarıları, yöntem notlarını ve — yapıldıysa — çalışmayı, arayüz hangi
dildeyse o dilde içerir.

## Geliştirme

```bash
pip install -e ".[dev]"
pytest -q
ruff check .
```

## Lisans

[MIT](https://github.com/hdaltuntas/lythos-bearing/blob/main/LICENSE) © 2026 Hasan Deniz Altuntaş
