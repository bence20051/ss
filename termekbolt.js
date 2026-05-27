const API_URL = "http://localhost:4000/api";
const KOSAR_KULCS = "termekbolt_kosar";

// ─────────────────────────────────────────────────────────────
// Termékképek hozzárendelése
// A termékkártyák képei a termék neve alapján jelennek meg.
// Ha nincs megadott kép, az alapértelmezett kép töltődik be.
// ─────────────────────────────────────────────────────────────


const TERMEK_KEPEK = {
    "Laptop": "laptop.jpg",
    "Tablet": "tablet.jpg",
    "Pendrive": "pendrive.jpg",
    "Webkamera": "webkamera.jpg",
    "Fejhallgató": "fejhallgato.jpg",
    "Billentyűzet": "billentyuzet.jpg",
    "Vezeték nélküli egér": "eger.jpg",
    "Egérpad": "egerpad.jpg",
    "Laptop táska": "laptop-taska.jpg",
    "USB-C töltő": "usb-c-tolto.jpg",
    "HDMI kábel": "hdmi-kabel.jpg",
    "USB elosztó": "usb-eloszto.jpg",
    "Külső merevlemez": "kulso-merevlemez.jpg",
    "Monitor": "monitor.jpg",
    "Laptop állvány": "laptop-allvany.jpg"
};

let kosar = [];
let termekek = [];

// ─────────────────────────────────────────────────────────────
// Kosár kezelése
// A kosár tartalmának mentése localStorage-be.
// Így a kosár tartalma a Rendelés oldalra átlépve is megmarad.
// ─────────────────────────────────────────────────────────────

function kosarMentes() {
    localStorage.setItem(KOSAR_KULCS, JSON.stringify(kosar));
}

// ─────────────────────────────────────────────────────────────
// Kosár kezelése
// A korábban elmentett kosár betöltése localStorage-ből.
// ─────────────────────────────────────────────────────────────

function kosarBetoltes() {
    const mentes = localStorage.getItem(KOSAR_KULCS);

    if (mentes) {
        kosar = JSON.parse(mentes);
    }
}

// ─────────────────────────────────────────────────────────────
// Termékkártyák megjelenítése
// A termékek lekérése a backend GET api/termekek végpontjáról.
// A termékekből kártyák készülnek a főoldalon.
// ─────────────────────────────────────────────────────────────

function termekekBetoltese() {
    const termekKartyaTarolo = document.getElementById("termekKartyaTarolo");

    if (!termekKartyaTarolo) {
        return;
    }

    termekKartyaTarolo.innerHTML = "";

    fetch(API_URL + "/termekek")
        .then(valasz => valasz.json())
        .then(adatok => {
            termekek = adatok;

            if (termekek.length === 0) {
                alert("Nincs megjeleníthető termék.");
                return;
            }

            termekek.forEach(termek => {
                const kepFajlnev = TERMEK_KEPEK[termek.Nev] || "alapertelmezett.jpg";

                termekKartyaTarolo.innerHTML += `
                    <div class="col-12 col-md-6 col-xl-3 mb-3">
                        <div class="termek-kartya card">
                            <img src="img/${kepFajlnev}" alt="${termek.Nev}">

                            <div class="card-body">
                                <h3 class="card-title">${termek.Nev}</h3>
                                <p class="termek-meta mb-2">Egységár: ${termek.Egysegar} Ft</p>
                                <p class="termek-meta mb-2">Készleten: ${termek.Keszlet} db</p>
                                <p class="card-text mb-0">${termek.Leiras}</p>
                            </div>

                            <div class="card-footer">
                                <button class="btn btn-sajat w-100"
                                    onclick="termekModalMegnyitasa()">
                                    Tovább
                                </button>
                            </div>
                        </div>
                    </div>
                `;
            });
        })
        .catch(() => {
            alert("Nem sikerült betölteni a termékeket.");
        });
}

// ─────────────────────────────────────────────────────────────
// Modal ablak megjelenítése
// A Tovább gombra kattintva megnyílik a termékeket tartalmazó modal.
//
// Terméklista dinamikus megjelenítése
// A modalban megjelennek a termékek.
//
// Interaktív elemek hozzáadása
// Minden termék mellett megjelenik egy numerikus mező és egy Kosárba gomb.
// ─────────────────────────────────────────────────────────────

function termekModalMegnyitasa() {
    const modalCim = document.getElementById("modalCim");
    const termekLista = document.getElementById("termekLista");

    if (!modalCim || !termekLista) {
        return;
    }

    modalCim.innerText = "Termékek";
    termekLista.innerHTML = "";

    termekek.forEach(termek => {
        termekLista.innerHTML += `
            <div class="termek-lista-sor d-flex justify-content-between align-items-center mb-2 p-2 rounded"
                 style="border-bottom: 1px solid #ddd;">
                <div class="termek-lista-adat">
                    <strong>${termek.Nev}</strong>
                    <span>${termek.Egysegar} Ft</span>
                </div>

                <div class="termek-lista-muvelet d-flex align-items-center gap-2">
                    <input id="qty-${termek.Id}"
                           class="form-control form-control-sm"
                           type="number"
                           min="0"
                           max="${termek.Keszlet}"
                           value="0"
                           style="width: 70px; text-align: center;">

                    <button class="btn btn-sajat btn-sm"
                        onclick='kosarbaTesz(${termek.Id}, ${JSON.stringify(termek.Nev)}, ${termek.Egysegar}, ${termek.Keszlet})'>
                        Kosárba
                    </button>
                </div>
            </div>
        `;
    });

    const modal = new bootstrap.Modal(document.getElementById("termekModal"));
    modal.show();
}

// ─────────────────────────────────────────────────────────────
// Kosárba gomb működése
// A felhasználó által megadott mennyiség kosárba helyezése.
// A 0 vagy annál kisebb értéket a rendszer nem fogadja el.
// A rendszer azt is ellenőrzi, hogy van-e elegendő készlet.
//
// Kosár kezelése
// Ha a termék már szerepel a kosárban, a mennyiség növekszik.
// A kosár módosítás után localStorage-be kerül.
// ─────────────────────────────────────────────────────────────

function kosarbaTesz(termekId, nev, egysegar, keszlet) {
    const mennyisegMezo = document.getElementById("qty-" + termekId);
    const mennyiseg = parseInt(mennyisegMezo.value);

    if (keszlet <= 0) {
        alert("Ez a termék jelenleg nincs készleten.");
        return;
    }

    if (mennyiseg <= 0 || isNaN(mennyiseg)) {
        alert("Legalább 1 darabot kell megadni.");
        return;
    }

    const letezoTermek = kosar.find(tetel => tetel.id === termekId);

    if (letezoTermek) {
        if (letezoTermek.mennyiseg + mennyiseg > keszlet) {
            alert("Ebből a termékből nincs ennyi készleten.");
            return;
        }

        letezoTermek.mennyiseg += mennyiseg;
    } else {
        if (mennyiseg > keszlet) {
            alert("Ebből a termékből nincs ennyi készleten.");
            return;
        }

        kosar.push({
            id: termekId,
            nev: nev,
            egysegar: egysegar,
            mennyiseg: mennyiseg
        });
    }

    kosarMentes();
    mennyisegMezo.value = 0;

    kosarTartalma();

    alert("A termék bekerült a kosárba.");
}

// ─────────────────────────────────────────────────────────────
// Diákazonosító ellenőrzése
// A rendszer ellenőrzi a megadott diákazonosítót a backend alapján.
// Sikeres ellenőrzés esetén kitölti a vásárló adatait.
// Az ellenőrzött diák 20% kedvezményt kap.
// ─────────────────────────────────────────────────────────────

function diakEllenorzese() {
    const diakAzonosito = document.getElementById("diakAzonosito");
    const vasarloNev = document.getElementById("vasarloNev");
    const telefon = document.getElementById("telefon");
    const email = document.getElementById("email");

    const azonosito = diakAzonosito.value.trim();

    if (azonosito === "") {
        alert("A diákazonosító megadása kötelező.");
        return;
    }

    fetch(API_URL + "/diakok/" + azonosito)
        .then(valasz => {
            if (!valasz.ok) {
                throw new Error("A megadott diákazonosító nem található.");
            }

            return valasz.json();
        })
        .then(adat => {
            if (!adat.Id) {
                diakAzonosito.dataset.ellenorzottAzonosito = "";
                alert("A megadott diákazonosító nem található.");
                kosarTartalma();
                return;
            }

            diakAzonosito.dataset.ellenorzottAzonosito = adat.Azonosito;

            vasarloNev.value = adat.Nev;
            telefon.value = adat.Telefon;
            email.value = adat.Email;

            kosarTartalma();

            alert("A diákazonosító érvényes, az adatok betöltve.");
        })
        .catch(hiba => {
            diakAzonosito.dataset.ellenorzottAzonosito = "";
            alert(hiba.message);
            kosarTartalma();
        });
}

// ─────────────────────────────────────────────────────────────
// Kosár tartalmának megjelenítése
// A Rendelés oldalon megjelenik a kosár tartalma.
// A rendszer kiszámolja az eredeti összeget, a kedvezményt,
// valamint a fizetendő végösszeget.
// ─────────────────────────────────────────────────────────────

function kosarTartalma() {
    const kosarTartalom = document.getElementById("kosarTartalom");
    const osszeg = document.getElementById("osszeg");
    const kedvezmenyOsszeg = document.getElementById("kedvezmenyOsszeg");
    const fizetendoOsszeg = document.getElementById("fizetendoOsszeg");
    const diakAzonosito = document.getElementById("diakAzonosito");

    if (!kosarTartalom) {
        return;
    }

    kosarTartalom.innerHTML = "";

    if (kosar.length === 0) {
        kosarTartalom.innerHTML = "<p class='text-center text-muted mb-0'>A kosár üres.</p>";

        if (osszeg) {
            osszeg.innerText = "0";
        }

        if (kedvezmenyOsszeg) {
            kedvezmenyOsszeg.innerText = "0";
        }

        if (fizetendoOsszeg) {
            fizetendoOsszeg.innerText = "0";
        }

        return;
    }

    let teljesOsszeg = 0;

    kosar.forEach((tetel, index) => {
        const reszosszeg = parseInt(tetel.egysegar) * parseInt(tetel.mennyiseg);
        teljesOsszeg += reszosszeg;

        kosarTartalom.innerHTML += `
            <div class="kosar-lista-sor d-flex justify-content-between align-items-center mb-2">
                <div class="kosar-lista-adat">
                    <strong>${tetel.nev}</strong><br>
                    <span>${tetel.mennyiseg} db × ${tetel.egysegar} Ft</span>
                </div>

                <div class="kosar-lista-muvelet text-end">
                    <strong>${reszosszeg} Ft</strong><br>
                    <button class="btn btn-outline-danger btn-sm mt-1"
                        onclick="kosarTetelTorlese(${index})">
                        Eltávolítás
                    </button>
                </div>
            </div>
        `;
    });

    let kedvezmeny = 0;

    if (diakAzonosito) {
        const ellenorzottAzonosito = diakAzonosito.dataset.ellenorzottAzonosito || "";

        if (ellenorzottAzonosito !== "") {
            kedvezmeny = Math.round(teljesOsszeg * 0.2);
        }
    }

    if (osszeg) {
        osszeg.innerText = teljesOsszeg;
    }

    if (kedvezmenyOsszeg) {
        kedvezmenyOsszeg.innerText = kedvezmeny;
    }

    if (fizetendoOsszeg) {
        fizetendoOsszeg.innerText = teljesOsszeg - kedvezmeny;
    }
}

// ─────────────────────────────────────────────────────────────
// Kosártétel törlése
// A kiválasztott termék törlődik a kosárból.
// A módosított kosár újra mentésre és megjelenítésre kerül.
// ─────────────────────────────────────────────────────────────

function kosarTetelTorlese(index) {
    kosar.splice(index, 1);
    kosarMentes();
    kosarTartalma();

    alert("A termék törölve lett a kosárból.");
}

// ─────────────────────────────────────────────────────────────
// Kosár ürítése
// A teljes kosár törlődik a localStorage-ből és a memóriából.
// ─────────────────────────────────────────────────────────────

function kosarUritese() {
    kosar = [];
    kosarMentes();
    kosarTartalma();

    alert("A kosár kiürült.");
}

// ─────────────────────────────────────────────────────────────
// Rendelés leadása
// A rendszer ellenőrzi:
// - ha van diákazonosító, akkor az ellenőrizve lett-e,
// - a vásárlói adatok ki vannak-e töltve,
// - nem üres-e a kosár.
//
// Sikeres validáció után a rendelés JSON formátumban elküldésre kerül
// a backend POST api/rendelesek végpontjára.
// ─────────────────────────────────────────────────────────────

function rendelesLeadasa() {
    const diakAzonosito = document.getElementById("diakAzonosito");
    const vasarloNev = document.getElementById("vasarloNev");
    const telefon = document.getElementById("telefon");
    const email = document.getElementById("email");

    const beirtAzonosito = diakAzonosito.value.trim();
    const ellenorzottAzonosito = diakAzonosito.dataset.ellenorzottAzonosito || "";

    const nev = vasarloNev.value.trim();
    const tel = telefon.value.trim();
    const mail = email.value.trim();

    if (beirtAzonosito !== "" && ellenorzottAzonosito === "") {
        alert("A diákazonosítót először ellenőrizni kell.");
        return;
    }

    if (nev === "" || tel === "" || mail === "") {
        alert("A név, telefon és e-mail mező kitöltése kötelező.");
        return;
    }

    if (kosar.length === 0) {
        alert("Nem lehet üres kosarat leadni.");
        return;
    }

    const rendeltTermekek = [];

    kosar.forEach(tetel => {
        rendeltTermekek.push({
            termekId: tetel.id,
            mennyiseg: tetel.mennyiseg
        });
    });

    const rendeles = {
        vasarloAzonosito: ellenorzottAzonosito,
        vasarloNev: nev,
        telefon: tel,
        email: mail,
        termekek: rendeltTermekek
    };

    fetch(API_URL + "/rendelesek", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(rendeles)
    })
        .then(valasz => {
            if (!valasz.ok) {
                throw new Error("Hiba történt a rendelés leadása közben.");
            }

            return valasz.json();
        })
        .then(valasz => {
            if (!valasz.rendelesId) {
                alert(valasz.message || "Hiba történt a rendelés mentése során.");
                return;
            }

            alert(valasz.message || "A rendelés sikeresen rögzítve.");

            kosar = [];
            kosarMentes();
            kosarTartalma();

            diakAzonosito.value = "";
            diakAzonosito.dataset.ellenorzottAzonosito = "";
            vasarloNev.value = "";
            telefon.value = "";
            email.value = "";
        })
        .catch(hiba => {
            alert(hiba.message);
        });
}

// ─────────────────────────────────────────────────────────────
// Oldalbetöltéskor lefutó műveletek
// A kosár betöltése, a termékek megjelenítése,
// a kosár tartalmának megjelenítése,
// valamint az oldalon található gombok kezelése.
// ─────────────────────────────────────────────────────────────

document.addEventListener("DOMContentLoaded", () => {
    kosarBetoltes();
    termekekBetoltese();
    kosarTartalma();

    const termekekLekerdezesGomb = document.getElementById("termekekLekerdezesGomb");
    const diakEllenorzesGomb = document.getElementById("diakEllenorzesGomb");
    const diakAzonosito = document.getElementById("diakAzonosito");
    const rendelesLeadasGomb = document.getElementById("rendelesLeadasGomb");
    const kosarUriteseGomb = document.getElementById("kosarUriteseGomb");

    if (termekekLekerdezesGomb) {
        termekekLekerdezesGomb.onclick = termekekBetoltese;
    }

    if (diakEllenorzesGomb) {
        diakEllenorzesGomb.onclick = diakEllenorzese;
    }

    if (diakAzonosito) {
        diakAzonosito.oninput = () => {
            diakAzonosito.dataset.ellenorzottAzonosito = "";
            kosarTartalma();
        };
    }

    if (rendelesLeadasGomb) {
        rendelesLeadasGomb.onclick = rendelesLeadasa;
    }

    if (kosarUriteseGomb) {
        kosarUriteseGomb.onclick = kosarUritese;
    }

    const navLinks = document.querySelectorAll(".navbar-nav .nav-link");
    const navbarCollapse = document.querySelector(".navbar-collapse");

    if (navbarCollapse) {
        navLinks.forEach(link => {
            link.addEventListener("click", () => {
                const bsCollapse = new bootstrap.Collapse(navbarCollapse, { toggle: false });
                bsCollapse.hide();
            });
        });
    }
});