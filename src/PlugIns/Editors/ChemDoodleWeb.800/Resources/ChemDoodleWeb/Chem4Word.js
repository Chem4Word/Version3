var X_OFFSET = 5;
var Y_OFFSET = 75;

var clientWidth;
var clientHeight;

function resize_canvas() {
    try {
        clientWidth = document.getElementById('container').clientWidth;
        clientHeight = document.getElementById('container').clientHeight;

        sketcher.resize(clientWidth - X_OFFSET, clientHeight - Y_OFFSET);
    } catch (e) {
        //
    }
}

function setup_sketcher() {
    try {
        // changes the default JMol color of hydrogen so it appears on white backgrounds
        ChemDoodle.ELEMENT['H'].jmolColor = '#808080';
        // darkens the default JMol color of sulphur so it appears on white backgrounds
        ChemDoodle.ELEMENT['S'].jmolColor = '#808000';

        // sets terminal carbon labels to display
        sketcher.specs.atoms_displayTerminalCarbonLabels_2D = true;
        // sets atom labels to be colored by JMol colors, which are easy to recognize
        sketcher.specs.atoms_useJMOLColors = true;
        // enables overlap clear widths, so that some depth is introduced to overlapping bonds
        sketcher.specs.bonds_clearOverlaps_2D = true;
        // sets the shape color to improve contrast when drawing figures
        sketcher.specs.shapes_color = '#c10000';
    } catch (e) {
        //
    }
}

// Converts from degrees to radians.
Math.radians = function (degrees) {
    return degrees * Math.PI / 180;
};

// Converts from radians to degrees.
Math.degrees = function (radians) {
    return radians * 180 / Math.PI;
};

function Flip() {
    var dummy = {};

    // Scale molecules
    if (sketcher.molecules && sketcher.molecules.length > 0) {
        dummy.m = [];
        for (var im = 0, iml = sketcher.molecules.length; im < iml; im++) {
            for (var ia = 0, ial = sketcher.molecules[im].atoms.length; ia < ial; ia++) {
                sketcher.molecules[im].atoms[ia].y = -sketcher.molecules[im].atoms[ia].y;
            }
            dummy.m.push(sketcher.molecules[im]);
        }
    }

    // Scale Shapes
    if (sketcher.shapes && sketcher.shapes.length > 0) {
        dummy.s = [];
        for (var is = 0, isl = sketcher.shapes.length; is < isl; is++) {
            var ps = sketcher.shapes[is].getPoints();
            for (p = 0, pp = ps.length; p < pp; p++) {
                ps[p].y = -ps[p].y;
            }
            dummy.s.push(sketcher.shapes[is]);
        }
    }

    // Loading twice seems to fix rendering glitch
    sketcher.loadContent(dummy.m, dummy.s);
    sketcher.loadContent(dummy.m, dummy.s);

    sketcher.center();
    sketcher.repaint();
}

function Mirror() {
    var dummy = {};

    // Scale molecules
    if (sketcher.molecules && sketcher.molecules.length > 0) {
        dummy.m = [];
        for (var im = 0, iml = sketcher.molecules.length; im < iml; im++) {
            for (var ia = 0, ial = sketcher.molecules[im].atoms.length; ia < ial; ia++) {
                sketcher.molecules[im].atoms[ia].x = -sketcher.molecules[im].atoms[ia].x;
            }
            dummy.m.push(sketcher.molecules[im]);
        }
    }

    // Scale Shapes
    if (sketcher.shapes && sketcher.shapes.length > 0) {
        dummy.s = [];
        for (var is = 0, isl = sketcher.shapes.length; is < isl; is++) {
            var ps = sketcher.shapes[is].getPoints();
            for (p = 0, pp = ps.length; p < pp; p++) {
                ps[p].x = -ps[p].x;
            }
            dummy.s.push(sketcher.shapes[is]);
        }
    }

    // Loading twice seems to fix rendering glitch
    sketcher.loadContent(dummy.m, dummy.s);
    sketcher.loadContent(dummy.m, dummy.s);

    sketcher.center();
    sketcher.repaint();
}

function AddHydrogensToAtom(atoms, bonds, bondlength, atom) {
    var atomIdx = atoms.length;
    var bondIdx = bonds.length;

    var hToAdd = atom.getImplicitHydrogenCount();

    if (hToAdd > 0) {
        var seperation = 90;
        if (atom.bonds.length > 1) {
            seperation = 30;
        }

        var atomAngle = atom.angleOfLeastInterference;

        var atomDegrees = Math.degrees(atomAngle);
        switch (hToAdd) {
            case 1:
                atomDegrees = atomDegrees;
                break;
            case 2:
                atomDegrees = atomDegrees + 0.5 * seperation;
                break;
            case 3:
                atomDegrees = atomDegrees + 1.0 * seperation;
                break;
        }

        for (var i = 0; i < hToAdd; i++) {
            var hAtomAngle = Math.radians(atomDegrees);
            var newAtomX = atom.x + bondlength * Math.cos(hAtomAngle);
            var newAtomY = atom.y - bondlength * Math.sin(hAtomAngle);
            // Detect and bump any overlapping atoms
            for (var j = 0, jj = atoms.length; j < jj; j++) {
                var xdelta = Math.abs(Math.floor(atoms[j].x) - Math.floor(newAtomX));
                var ydelta = Math.abs(Math.floor(atoms[j].y) - Math.floor(newAtomY));
                if (xdelta < 2 && ydelta < 2) {
                    hAtomAngle = Math.radians(atomDegrees - 45);
                    newAtomX = atom.x + bondlength / 2 * Math.cos(hAtomAngle);
                    newAtomY = atom.y - bondlength / 2 * Math.sin(hAtomAngle);
                    break;
                }
            }
            var hydrogen = new ChemDoodle.structures.Atom('H');
            hydrogen.x = atom.x + bondlength * Math.cos(hAtomAngle);
            hydrogen.y = atom.y - bondlength * Math.sin(hAtomAngle);
            var bond = new ChemDoodle.structures.Bond(atom, hydrogen, 1);
            atoms[atomIdx++] = hydrogen;
            bonds[bondIdx++] = bond;
            atomDegrees -= seperation;
        }
    }
}

function AddExplicitHydrogensToMol(mol) {
    var avBondLength = mol.getAverageBondLength();
    if (avBondLength === 0) {
        avBondLength = 20;
    }

    var atoms = mol.atoms;
    var bonds = mol.bonds;

    for (var i = 0, ii = atoms.length; i < ii; i++) {
        AddHydrogensToAtom(atoms, bonds, avBondLength, atoms[i]);
    }
}

function AddExplicitHydrogens() {
    //alert(sketcher.oneMolecule);
    for (var i = 0, ii = sketcher.molecules.length; i < ii; i++) {
        AddExplicitHydrogensToMol(sketcher.molecules[i]);
    }

    // Loading twice seems to fix rendering glitch
    sketcher.loadContent(sketcher.molecules, sketcher.shapes);
    sketcher.loadContent(sketcher.molecules, sketcher.shapes);

    sketcher.center();
    sketcher.repaint();
}

function RemoveHydrogensFromMol(mol) {
    var atoms = [];
    var bonds = [];

    var i;
    var ii;
    var j;
    var jj;

    for (i = 0, ii = mol.bonds.length; i < ii; i++) {
        if (mol.bonds[i].a1.label !== 'H' && mol.bonds[i].a2.label !== 'H') {
            bonds.push(mol.bonds[i]);
        }
        else if (mol.bonds[i].stereo !== 'none') {
            bonds.push(mol.bonds[i]);
        }
    }

    for (i = 0, ii = mol.atoms.length; i < ii; i++) {
        if (mol.atoms[i].label !== 'H') {
            atoms.push(mol.atoms[i]);
        }
        else {
            for (j = 0, jj = mol.atoms[i].bonds.length; j < jj; j++) {
                if (mol.atoms[i].bonds[j].stereo !== 'none') {
                    atoms.push(mol.atoms[i]);
                    break;
                }
            }
        }
    }

    mol.atoms = atoms;
    mol.bonds = bonds;
}

function RemoveHydrogens() {
    for (var i = 0, ii = sketcher.molecules.length; i < ii; i++) {
        RemoveHydrogensFromMol(sketcher.molecules[i]);
    }

    // Loading twice seems to fix rendering glitch
    sketcher.loadContent(sketcher.molecules, sketcher.shapes);
    sketcher.loadContent(sketcher.molecules, sketcher.shapes);

    sketcher.center();
    sketcher.repaint();
}

function AtomsInColour(value) {
    sketcher.specs.atoms_useJMOLColors = value;

    // Loading twice seems to fix rendering glitch
    sketcher.loadContent(sketcher.molecules, sketcher.shapes);
    sketcher.loadContent(sketcher.molecules, sketcher.shapes);

    sketcher.center();
    sketcher.repaint();
}

function ShowHydrogens(value) {
    sketcher.specs.atoms_implicitHydrogens_2D = value;

    // Loading twice seems to fix rendering glitch
    sketcher.loadContent(sketcher.molecules, sketcher.shapes);
    sketcher.loadContent(sketcher.molecules, sketcher.shapes);

    sketcher.center();
    sketcher.repaint();
}

function ShowCarbons(value) {
    sketcher.specs.atoms_displayAllCarbonLabels_2D = value;

    // Loading twice seems to fix rendering glitch
    sketcher.loadContent(sketcher.molecules, sketcher.shapes);
    sketcher.loadContent(sketcher.molecules, sketcher.shapes);

    sketcher.center();
    sketcher.repaint();
}

function SetJSON(molFile, length) {
    var jsonObject = JSON.parse(molFile);
    if (jsonObject) {
        if (sketcher.oneMolecule) {
            if (jsonObject.a) {
                var mol = new ChemDoodle.io.JSONInterpreter().molFrom(jsonObject);
                ShowMol(mol, length);
            }
        } else {
            var objects = {
                molecules: [],
                shapes: []
            };
            if (jsonObject.m || jsonObject.s) {
                objects = new ChemDoodle.io.JSONInterpreter().contentFrom(jsonObject);
            } else if (jsonObject.a) {
                objects = {
                    molecules: [new ChemDoodle.io.JSONInterpreter().molFrom(jsonObject)],
                    shapes: []
                };
            }
            ShowSketch(objects.molecules, objects.shapes, length);
        }
    }
}

function ShowSketch(mols, shapes, length) {
    var len = parseInt(length);

    // Load original Content
    sketcher.loadContent(mols, shapes);

    var dummy = {};
    var total = 0;
    var scale = 1;

    // Scale molecules
    if (sketcher.molecules && sketcher.molecules.length > 0) {
        dummy.m = [];
        for (var i = 0, ii = sketcher.molecules.length; i < ii; i++) {
            total += sketcher.molecules[i].getAverageBondLength();
            sketcher.molecules[i].scaleToAverageBondLength(len);
            dummy.m.push(sketcher.molecules[i]);
        }
        scale = len / (total / sketcher.molecules.length);
    }

    // Scale Shapes
    if (sketcher.shapes && sketcher.shapes.length > 0) {
        dummy.s = [];
        for (var j = 0, jj = sketcher.shapes.length; j < jj; j++) {
            var ps = sketcher.shapes[j].getPoints();
            for (p = 0, pp = ps.length; p < pp; p++) {
                ps[p].x *= scale;
                ps[p].y *= scale;
            }
            dummy.s.push(sketcher.shapes[j]);
        }
    }

    // Load scaled Content
    // Loading twice seems to fix rendering glitch
    sketcher.loadContent(dummy.m, dummy.s);
    sketcher.loadContent(dummy.m, dummy.s);

    sketcher.center();
    sketcher.repaint();
    sketcher.specs.bondLength_2D = len;
}

function ShowMol(mol, length) {
    var len = parseInt(length);

    mol.scaleToAverageBondLength(len);

    // Loading twice seems to fix rendering glitch
    sketcher.loadMolecule(mol);
    sketcher.loadMolecule(mol);

    sketcher.center();
    sketcher.repaint();
    sketcher.specs.bondLength_2D = len;
}

function ReScale(length) {
    var len = parseInt(length);

    var current = sketcher.specs.bondLength_2D;
    var scale = len / current;

    var dummy = {};

    // Scale molecules
    if (sketcher.molecules && sketcher.molecules.length > 0) {
        dummy.m = [];
        for (var m = 0, mm = sketcher.molecules.length; m < mm; m++) {
            sketcher.molecules[m].scaleToAverageBondLength(len);
            dummy.m.push(sketcher.molecules[m]);
        }
    }

    // Scale Shapes
    if (sketcher.shapes && sketcher.shapes.length > 0) {
        dummy.s = [];
        for (var s = 0, ss = sketcher.shapes.length; s < ss; s++) {
            var ps = sketcher.shapes[s].getPoints();
            for (p = 0, pp = ps.length; p < pp; p++) {
                ps[p].x *= scale;
                ps[p].y *= scale;
            }
            dummy.s.push(sketcher.shapes[s]);
        }
    }

    // Loading twice seems to fix rendering glitch
    sketcher.loadContent(dummy.m, dummy.s);
    sketcher.loadContent(dummy.m, dummy.s);

    sketcher.center();
    sketcher.repaint();
    sketcher.specs.bondLength_2D = len;
}

function GetFirstMolJSON() {
    var mol = sketcher.getMolecule();
    if (mol) {
        var jsonMol = new ChemDoodle.io.JSONInterpreter().molTo(mol);
        if (jsonMol) {
            return JSON.stringify(jsonMol);
        }
    }
}

function GetJSON() {
    if (sketcher.oneMolecule) {
        var mol = sketcher.getMolecule();
        var jsonMol = new ChemDoodle.io.JSONInterpreter().molTo(mol);
        return JSON.stringify(jsonMol);
    } else {
        var jsonSketch = new ChemDoodle.io.JSONInterpreter().contentTo(sketcher.molecules, sketcher.shapes);
        return JSON.stringify(jsonSketch);
    }
}

function GetVersion() {
    return ChemDoodle.getVersion();
}

function GetFormula() {
    function compare(a, b) {
        if (a.label < b.label)
            return -1;
        if (a.label > b.label)
            return 1;
        return 0;
    }

    function getMolFormula(mol) {
        var fatoms = [];
        fatoms.push({ label: 'C', freq: 0 });
        fatoms.push({ label: 'H', freq: 0 });

        var atoms = mol.atoms;
        for (var i = 0, ii = atoms.length; i < ii; i++) {
            var found = false;
            for (var j = 0, jj = fatoms.length; j < jj; j++) {
                if (fatoms[j].label === atoms[i].label) {
                    fatoms[j].freq += 1;
                    found = true;
                    break;
                }
            }
            if (!found) {
                fatoms.push({ label: atoms[i].label, freq: 1 });
            }
            fatoms[1].freq += atoms[i].getImplicitHydrogenCount();
        }

        var result = "";

        var others = [];
        for (var k = 0, kk = fatoms.length; k < kk; k++) {
            if (k < 2) {
                if (fatoms[k].freq > 0) {
                    result += fatoms[k].label + " " + fatoms[k].freq + " ";
                }
            }
            else {
                others.push(fatoms[k]);
            }
        }

        others.sort(compare);

        for (var l = 0, ll = others.length; l < ll; l++) {
            result += others[l].label + " " + others[l].freq + " ";
        }

        return result.trim();
    }

    if (sketcher.oneMolecule) {
        var mole = sketcher.getMolecule();
        return getMolFormula(mole);
    } else {
        var formulae = [];
        for (var i = 0, ii = sketcher.molecules.length; i < ii; i++) {
            formulae.push(getMolFormula(sketcher.molecules[i]));
        }
        return formulae.join(' . ');
    }
}