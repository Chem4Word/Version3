namespace Chem4WordTests
{
    public static class ChemistryValues
    {
        public const string TESTOSTERONE = @"<cml:cml
    xmlns:conventions='http://www.xml-cml.org/convention/'
    xmlns:cml='http://www.xml-cml.org/schema' 
    xmlns:cmlDict='http://www.xml-cml.org/dictionary/cml/' 
    xmlns:nameDict='http://www.xml-cml.org/dictionary/cml/name/' 
    xmlns:c4w='http://www.chem4word.com/cml' 
    conventions='convention:molecular'>
  <cml:molecule id='m1'>
    <cml:formula id='m1.f0' concise='C 19 H 28 O 2' />
    <cml:formula id='m1.f1' convention='chemspider:Smiles' inline='C[C@]12CC[C@H]3[C@H]([C@@H]1CC[C@@H]2O)CCC4=CC(=O)CC[C@]34C' concise='C 19 H 28 O 2' />
    <cml:formula id='m1.f2' convention='chemspider:Formula' inline='C19H28O2' concise='C 19 H 28 O 2' />
    <cml:name id='m1.n1' dictRef='chemspider:Id'>5791</cml:name>
    <cml:name id='m1.n2' dictRef='chemspider:Synonym'>(17beta)-17-Hydroxyandrost-4-en-3-one</cml:name>
    <cml:name id='m1.n3' dictRef='chemspider:Inchikey'>MUMGGOZAMZWBJJ-DYKIIFRCSA-N</cml:name>
    <cml:name id='m1.n4' dictRef='chem4word:Synonym'>testosterone</cml:name>
    <cml:atom id='a1' elementType='C' x2='301.24677590909027' y2='178.24672786488574' />
    <cml:atom id='a2' elementType='C' x2='279.62489693599679' y2='190.7304917061636' />
    <cml:atom id='a3' elementType='C' x2='279.62489693599679' y2='215.69801938871814' />
    <cml:atom id='a4' elementType='C' x2='301.24677590909027' y2='228.18178322999597' />
    <cml:atom id='a5' elementType='C' x2='322.86865488218257' y2='215.69801938871814' />
    <cml:atom id='a6' elementType='C' x2='322.86865488218257' y2='190.7304917061636' />
    <cml:atom id='a7' elementType='C' x2='344.49053385527475' y2='178.24672786488574' />
    <cml:atom id='a8' elementType='C' x2='366.11241282836829' y2='190.7304917061636' />
    <cml:atom id='a9' elementType='C' x2='366.11241282836829' y2='215.69801938871814' />
    <cml:atom id='a10' elementType='C' x2='344.49053385527475' y2='228.18178322999597' />
    <cml:atom id='a11' elementType='C' x2='387.73429153838521' y2='178.24672740923887' />
    <cml:atom id='a12' elementType='C' x2='411.4794097723684' y2='185.96012207758241' />
    <cml:atom id='a13' elementType='C' x2='426.15389231488371' y2='165.76253931792553' />
    <cml:atom id='a14' elementType='C' x2='411.47913926042588' y2='145.56348902434493' />
    <cml:atom id='a15' elementType='C' x2='387.73429101223456' y2='153.27919972668315' />
    <cml:atom id='a16' elementType='C' x2='366.11241177606587' y2='140.7954363410534' />
    <cml:atom id='a17' elementType='C' x2='344.49053306604878' y2='153.27920063797816' />
    <cml:atom id='a18' elementType='C' x2='388.56060205926906' y2='128.44898267129551' />
    <cml:atom id='a19' elementType='O' x2='419.19439029609691' y2='121.81849141582973' />
    <cml:atom id='a20' elementType='H' x2='436.9971406062632' y2='140.50957587554976' />
    <cml:atom id='a21' elementType='H' x2='387.73429153838521' y2='203.21370580014332' />
    <cml:atom id='a22' elementType='H' x2='366.11241282836829' y2='165.76351331525922' />
    <cml:atom id='a23' elementType='H' x2='344.49053385527475' y2='203.21370625579021' />
    <cml:atom id='a24' elementType='C' x2='322.86865488218257' y2='165.76351331525922' />
    <cml:atom id='a25' elementType='O' x2='258.0028593937368' y2='228.18150858417027' />
    <cml:bond id='b1' atomRefs2='a1 a2' order='S' />
    <cml:bond id='b2' atomRefs2='a1 a6' order='S' />
    <cml:bond id='b3' atomRefs2='a2 a3' order='S' />
    <cml:bond id='b4' atomRefs2='a3 a4' order='S' />
    <cml:bond id='b5' atomRefs2='a3 a25' order='D' />
    <cml:bond id='b6' atomRefs2='a4 a5' order='D' />
    <cml:bond id='b7' atomRefs2='a6 a5' order='S' />
    <cml:bond id='b8' atomRefs2='a5 a10' order='S' />
    <cml:bond id='b9' atomRefs2='a7 a6' order='S' />
    <cml:bond id='b10' atomRefs2='a6 a24' order='S'>
      <cml:bondStereo>W</cml:bondStereo>
    </cml:bond>
    <cml:bond id='b11' atomRefs2='a7 a8' order='S' />
    <cml:bond id='b12' atomRefs2='a17 a7' order='S' />
    <cml:bond id='b13' atomRefs2='a7 a23' order='S'>
      <cml:bondStereo>H</cml:bondStereo>
    </cml:bond>
    <cml:bond id='b14' atomRefs2='a9 a8' order='S' />
    <cml:bond id='b15' atomRefs2='a8 a11' order='S' />
    <cml:bond id='b16' atomRefs2='a8 a22' order='S'>
      <cml:bondStereo>W</cml:bondStereo>
    </cml:bond>
    <cml:bond id='b17' atomRefs2='a10 a9' order='S' />
    <cml:bond id='b18' atomRefs2='a11 a12' order='S' />
    <cml:bond id='b19' atomRefs2='a15 a11' order='S' />
    <cml:bond id='b20' atomRefs2='a11 a21' order='S'>
      <cml:bondStereo>H</cml:bondStereo>
    </cml:bond>
    <cml:bond id='b21' atomRefs2='a12 a13' order='S' />
    <cml:bond id='b22' atomRefs2='a14 a13' order='S' />
    <cml:bond id='b23' atomRefs2='a14 a15' order='S' />
    <cml:bond id='b24' atomRefs2='a14 a19' order='S'>
      <cml:bondStereo>W</cml:bondStereo>
    </cml:bond>
    <cml:bond id='b25' atomRefs2='a14 a20' order='S'>
      <cml:bondStereo>H</cml:bondStereo>
    </cml:bond>
    <cml:bond id='b26' atomRefs2='a16 a15' order='S' />
    <cml:bond id='b27' atomRefs2='a15 a18' order='S'>
      <cml:bondStereo>W</cml:bondStereo>
    </cml:bond>
    <cml:bond id='b28' atomRefs2='a16 a17' order='S' />
  </cml:molecule>
</cml:cml>";

        public const string PHTHALOCYANINE = @"<cml convention='cmlDict:cmllite' cml='http://www.xml-cml.org/schema' cmlDict='http://www.xml-cml.org/dictionary/cml/' bondDict='http://www.xml-cml.org/dictionary/bond/' pc='http://www.ncbi.nlm.nih.gov'>
<molecule formalCharge='0' id='m1' spinMultiplicity='1'>
<name dictRef='pc:CID'>475701</name>
<atomArray>
<atom id='a1' x2='142.896116420753' y2='0.689661551738648' elementType='Cu'/>
<atom id='a2' x2='142.896116420753' y2='25.8776828107795' elementType='N'/>
<atom id='a3' x2='168.084137679794' y2='0.689661551738648' elementType='N'/>
<atom id='a4' x2='117.708095161712' y2='0.689661551738648' elementType='N'/>
<atom id='a5' x2='142.896116420753' y2='-24.4983597073022' elementType='N'/>
<atom id='a6' x2='104.83514498738' y2='-34.5731289360448' elementType='N'/>
<atom id='a7' x2='179.839133300511' y2='-35.6932798640279' elementType='N'/>
<atom id='a8' x2='179.839133300511' y2='39.3107084491029' elementType='N'/>
<atom id='a9' x2='106.515371379355' y2='38.7506329851114' elementType='N'/>
<atom id='a10' x2='125.085716665821' y2='43.6880825657116' elementType='C'/>
<atom id='a11' x2='185.894537434726' y2='18.5000613066708' elementType='C'/>
<atom id='a12' x2='99.8998917811486' y2='-17.1207382031935' elementType='C'/>
<atom id='a13' x2='160.706516175685' y2='-42.3087594622343' elementType='C'/>
<atom id='a14' x2='160.706516175685' y2='43.6880825657116' elementType='C'/>
<atom id='a15' x2='99.8998917811486' y2='18.5000613066708' elementType='C'/>
<atom id='a16' x2='185.894537434726' y2='-17.1207382031935' elementType='C'/>
<atom id='a17' x2='125.085716665821' y2='-42.3087594622343' elementType='C'/>
<atom id='a18' x2='132.261271728019' y2='59.4602469065881' elementType='C'/>
<atom id='a19' x2='203.346928167577' y2='11.8845817084644' elementType='C'/>
<atom id='a20' x2='80.7672746563226' y2='-9.94518314099554' elementType='C'/>
<atom id='a21' x2='155.211187505462' y2='-57.5208483391193' elementType='C'/>
<atom id='a22' x2='154.091036577479' y2='58.9001714425966' elementType='C'/>
<atom id='a23' x2='80.7672746563226' y2='11.3245062444728' elementType='C'/>
<atom id='a24' x2='202.786852703586' y2='-11.0653340689787' elementType='C'/>
<atom id='a25' x2='132.261271728019' y2='-57.5208483391193' elementType='C'/>
<atom id='a26' x2='121.832886225932' y2='78.6455770162604' elementType='C'/>
<atom id='a27' x2='223.5008593738' y2='22.8730426745423' elementType='C'/>
<atom id='a28' x2='62.3462828269212' y2='-20.5822242080983' elementType='C'/>
<atom id='a29' x2='166.685047206999' y2='-77.3958400005302' elementType='C'/>
<atom id='a30' x2='165.490219550484' y2='77.5254260882773' elementType='C'/>
<atom id='a31' x2='62.3462828269212' y2='21.959350937207' elementType='C'/>
<atom id='a32' x2='222.380708445816' y2='-23.0245925059753' elementType='C'/>
<atom id='a33' x2='120.787412026482' y2='-77.3958400005302' elementType='C'/>
<atom id='a34' x2='133.232069198938' y2='97.2708316619411' elementType='C'/>
<atom id='a35' x2='243.09471511603' y2='10.9137842375457' elementType='C'/>
<atom id='a36' x2='43.9274873718884' y2='-9.94518314099554' elementType='C'/>
<atom id='a37' x2='155.211187505462' y2='-97.2708316619411' elementType='C'/>
<atom id='a38' x2='155.059637674029' y2='96.7107561979495' elementType='C'/>
<atom id='a39' x2='43.9274873718884' y2='11.3245062444728' elementType='C'/>
<atom id='a40' x2='242.534639652039' y2='-12.0339351655288' elementType='C'/>
<atom id='a41' x2='132.261271728019' y2='-97.2708316619411' elementType='C'/>
<atom id='a42' x2='108.219757889384' y2='78.9948005408669' elementType='H'/>
<atom id='a43' x2='223.832511903457' y2='36.4861710110905' elementType='H'/>
<atom id='a44' x2='62.3462828269212' y2='-34.1997452933837' elementType='H'/>
<atom id='a45' x2='180.302568292285' y2='-77.3958400005302' elementType='H'/>
<atom id='a46' x2='179.103347887032' y2='77.1762025636707' elementType='H'/>
<atom id='a47' x2='62.3462828269212' y2='35.5768720224924' elementType='H'/>
<atom id='a48' x2='222.049055916159' y2='-36.6377208425235' elementType='H'/>
<atom id='a49' x2='107.169890941196' y2='-77.3958400005302' elementType='H'/>
<atom id='a50' x2='126.726408319161' y2='109.234482847675' elementType='H'/>
<atom id='a51' x2='255.051777178658' y2='17.4348197379025' elementType='H'/>
<atom id='a52' x2='32.135153386905' y2='-16.7539436836382' elementType='H'/>
<atom id='a53' x2='162.019948048105' y2='-109.063165646925' elementType='H'/>
<atom id='a54' x2='162.169301505169' y2='108.325183859077' elementType='H'/>
<atom id='a55' x2='32.135153386905' y2='18.1332667871155' elementType='H'/>
<atom id='a56' x2='254.160049185009' y2='-19.1282243760888' elementType='H'/>
<atom id='a57' x2='125.452511185376' y2='-109.063165646925' elementType='H'/>
</atomArray>
<bondArray>
<bond id='a1_a2' order='other' atomRefs2='a1 a2' dictRef='bondDict:complex' source='
              ftp://ftp.ncbi.nih.gov/pubchem/specifications/pubchem.xsd
            ' xp_0='http://purl.org/dc/terms' cyclic='Cyclic' p8='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a1_a3' order='other' atomRefs2='a1 a3' dictRef='bondDict:complex' source='
              ftp://ftp.ncbi.nih.gov/pubchem/specifications/pubchem.xsd
            ' xp_0='http://purl.org/dc/terms' cyclic='Cyclic' p8='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a1_a4' order='other' atomRefs2='a1 a4' dictRef='bondDict:complex' source='
              ftp://ftp.ncbi.nih.gov/pubchem/specifications/pubchem.xsd
            ' xp_0='http://purl.org/dc/terms' cyclic='Cyclic' p8='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a1_a5' order='other' atomRefs2='a1 a5' dictRef='bondDict:complex' source='
              ftp://ftp.ncbi.nih.gov/pubchem/specifications/pubchem.xsd
            ' xp_0='http://purl.org/dc/terms' cyclic='Cyclic' p8='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a2_a10' order='D' atomRefs2='a2 a10' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a2_a14' order='S' atomRefs2='a2 a14' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a3_a11' order='D' atomRefs2='a3 a11' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a3_a16' order='S' atomRefs2='a3 a16' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a4_a12' order='D' atomRefs2='a4 a12' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a4_a15' order='S' atomRefs2='a4 a15' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a5_a13' order='D' atomRefs2='a5 a13' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a5_a17' order='S' atomRefs2='a5 a17' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a6_a12' order='S' atomRefs2='a6 a12' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a6_a17' order='D' atomRefs2='a6 a17' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a7_a13' order='S' atomRefs2='a7 a13' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a7_a16' order='D' atomRefs2='a7 a16' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a8_a11' order='S' atomRefs2='a8 a11' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a8_a14' order='D' atomRefs2='a8 a14' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a9_a10' order='S' atomRefs2='a9 a10' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a9_a15' order='D' atomRefs2='a9 a15' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a10_a18' order='S' atomRefs2='a10 a18' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a11_a19' order='S' atomRefs2='a11 a19' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a12_a20' order='S' atomRefs2='a12 a20' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a13_a21' order='S' atomRefs2='a13 a21' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a14_a22' order='S' atomRefs2='a14 a22' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a15_a23' order='S' atomRefs2='a15 a23' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a16_a24' order='S' atomRefs2='a16 a24' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a17_a25' order='S' atomRefs2='a17 a25' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a18_a22' order='S' atomRefs2='a18 a22' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a18_a26' order='D' atomRefs2='a18 a26' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a19_a24' order='S' atomRefs2='a19 a24' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a19_a27' order='D' atomRefs2='a19 a27' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a20_a23' order='S' atomRefs2='a20 a23' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a20_a28' order='D' atomRefs2='a20 a28' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a21_a25' order='S' atomRefs2='a21 a25' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a21_a29' order='D' atomRefs2='a21 a29' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a22_a30' order='D' atomRefs2='a22 a30' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a23_a31' order='D' atomRefs2='a23 a31' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a24_a32' order='D' atomRefs2='a24 a32' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a25_a33' order='D' atomRefs2='a25 a33' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a26_a34' order='S' atomRefs2='a26 a34' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a26_a42' order='S' atomRefs2='a26 a42' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a27_a35' order='S' atomRefs2='a27 a35' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a27_a43' order='S' atomRefs2='a27 a43' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a28_a36' order='S' atomRefs2='a28 a36' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a28_a44' order='S' atomRefs2='a28 a44' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a29_a37' order='S' atomRefs2='a29 a37' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a29_a45' order='S' atomRefs2='a29 a45' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a30_a38' order='S' atomRefs2='a30 a38' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a30_a46' order='S' atomRefs2='a30 a46' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a31_a39' order='S' atomRefs2='a31 a39' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a31_a47' order='S' atomRefs2='a31 a47' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a32_a40' order='S' atomRefs2='a32 a40' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a32_a48' order='S' atomRefs2='a32 a48' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a33_a41' order='S' atomRefs2='a33 a41' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a33_a49' order='S' atomRefs2='a33 a49' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a34_a38' order='D' atomRefs2='a34 a38' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a34_a50' order='S' atomRefs2='a34 a50' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a35_a40' order='D' atomRefs2='a35 a40' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a35_a51' order='S' atomRefs2='a35 a51' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a36_a39' order='D' atomRefs2='a36 a39' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a36_a52' order='S' atomRefs2='a36 a52' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a37_a41' order='D' atomRefs2='a37 a41' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a37_a53' order='S' atomRefs2='a37 a53' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a38_a54' order='S' atomRefs2='a38 a54' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a39_a55' order='S' atomRefs2='a39 a55' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a40_a56' order='S' atomRefs2='a40 a56' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a41_a57' order='S' atomRefs2='a41 a57' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
</bondArray>
<cidCommand instruction='NormalizeOnImport' xmlns='http://www.xml-cml.org/dictionary/cmlx/'/>
<formula concise='C 32 H 16 Cu 1 N 8'/>
</molecule>
</cml>
";

        public const string PARAFUCHSIN_CARBOL = @"<cml convention='cmlDict:cmllite'
cml='http://www.xml-cml.org/schema'
cmlDict='http://www.xml-cml.org/dictionary/cml/'
bondDict='http://www.xml-cml.org/dictionary/bond/'
pc='http://www.ncbi.nlm.nih.gov'>
<molecule formalCharge='0' id='m1' spinMultiplicity='1'>
<name dictRef='pc:CID'>44134605</name>
<atomArray>
<atom id='a1' x2='0' y2='147.809984561009' elementType='Cl'/>
<atom id='a2' x2='167.446230082787' y2='86.5812530923711' elementType='O'/>
<atom id='a3' x2='84.5937110669634' y2='292.749872334426' elementType='N'/>
<atom id='a4' x2='250.298749098611' y2='292.749872334426' elementType='N'/>
<atom id='a5' x2='167.446230082787' y2='149.245032954805' elementType='N'/>
<atom id='a6' x2='167.446230082787' y2='244.914925874553' elementType='C'/>
<atom id='a7' x2='167.446230082787' y2='220.997452644616' elementType='C'/>
<atom id='a8' x2='188.161153647236' y2='256.873662489521' elementType='C'/>
<atom id='a9' x2='146.733698265662' y2='256.873662489521' elementType='C'/>
<atom id='a10' x2='146.733698265662' y2='209.038716029647' elementType='C'/>
<atom id='a11' x2='146.733698265662' y2='185.121242799711' elementType='C'/>
<atom id='a12' x2='188.161153647236' y2='209.038716029647' elementType='C'/>
<atom id='a13' x2='126.021166448537' y2='244.914925874553' elementType='C'/>
<atom id='a14' x2='188.161153647236' y2='280.791135719458' elementType='C'/>
<atom id='a15' x2='146.733698265662' y2='280.791135719458' elementType='C'/>
<atom id='a16' x2='208.873685464361' y2='244.914925874553' elementType='C'/>
<atom id='a17' x2='167.446230082787' y2='173.162506184742' elementType='C'/>
<atom id='a18' x2='188.161153647236' y2='185.121242799711' elementType='C'/>
<atom id='a19' x2='105.308634631412' y2='256.873662489521' elementType='C'/>
<atom id='a20' x2='208.873685464361' y2='292.749872334426' elementType='C'/>
<atom id='a21' x2='126.021166448537' y2='292.749872334426' elementType='C'/>
<atom id='a22' x2='229.586217281486' y2='256.873662489521' elementType='C'/>
<atom id='a23' x2='105.308634631412' y2='280.791135719458' elementType='C'/>
<atom id='a24' x2='229.586217281486' y2='280.791135719458' elementType='C'/>
<atom id='a25' x2='126.021166448537' y2='173.162506184742' elementType='C'/>
<atom id='a26' x2='167.446230082787' y2='62.6637798624344' elementType='C'/>
<atom id='a27' x2='146.733698265662' y2='50.705043247466' elementType='C'/>
<atom id='a28' x2='188.161153647236' y2='50.705043247466' elementType='C'/>
<atom id='a29' x2='146.733698265662' y2='26.7875700175292' elementType='C'/>
<atom id='a30' x2='188.161153647236' y2='26.7875700175292' elementType='C'/>
<atom id='a31' x2='167.446230082787' y2='14.8288334025608' elementType='C'/>
<atom id='a32' x2='133.892406888509' y2='216.453132730928' elementType='H'/>
<atom id='a33' x2='201.002445024389' y2='216.453132730928' elementType='H'/>
<atom id='a34' x2='126.021166448537' y2='230.086092471992' elementType='H'/>
<atom id='a35' x2='175.31747052276' y2='288.205552420738' elementType='H'/>
<atom id='a36' x2='159.577381390138' y2='288.205552420738' elementType='H'/>
<atom id='a37' x2='208.873685464361' y2='230.086092471992' elementType='H'/>
<atom id='a38' x2='201.002445024389' y2='177.70682609843' elementType='H'/>
<atom id='a39' x2='208.873685464361' y2='307.578705736987' elementType='H'/>
<atom id='a40' x2='92.4649515069356' y2='249.459245788241' elementType='H'/>
<atom id='a41' x2='242.427508658639' y2='249.459245788241' elementType='H'/>
<atom id='a42' x2='126.021166448537' y2='307.578705736987' elementType='H'/>
<atom id='a43' x2='118.606749747256' y2='186.003797561895' elementType='H'/>
<atom id='a44' x2='113.177483324061' y2='165.748089483462' elementType='H'/>
<atom id='a45' x2='133.435583149817' y2='160.321214807589' elementType='H'/>
<atom id='a46' x2='84.5937110669634' y2='307.578705736987' elementType='H'/>
<atom id='a47' x2='71.7524196898103' y2='285.335455633146' elementType='H'/>
<atom id='a48' x2='263.142432223087' y2='285.335455633146' elementType='H'/>
<atom id='a49' x2='250.298749098611' y2='307.578705736987' elementType='H'/>
<atom id='a50' x2='180.289913207263' y2='141.830616253525' elementType='H'/>
<atom id='a51' x2='133.892406888509' y2='58.1194599487464' elementType='H'/>
<atom id='a52' x2='201.002445024389' y2='58.1194599487464' elementType='H'/>
<atom id='a53' x2='133.892406888509' y2='19.3731533162488' elementType='H'/>
<atom id='a54' x2='201.002445024389' y2='19.3731533162488' elementType='H'/>
<atom id='a55' x2='167.446230082787' y2='0' elementType='H'/>
<atom id='a56' x2='180.289913207263' y2='93.9956697936515' elementType='H'/>
<atom id='a57' x2='23.9174732299368' y2='147.809984561009' elementType='H'/>
</atomArray>
<bondArray>
<bond id='a1_a57' order='S' atomRefs2='a1 a57' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a2_a26' order='S' atomRefs2='a2 a26' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a2_a56' order='S' atomRefs2='a2 a56' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a3_a23' order='S' atomRefs2='a3 a23' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a3_a46' order='S' atomRefs2='a3 a46' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a3_a47' order='S' atomRefs2='a3 a47' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a4_a24' order='S' atomRefs2='a4 a24' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a4_a48' order='S' atomRefs2='a4 a48' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a4_a49' order='S' atomRefs2='a4 a49' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a5_a17' order='D' atomRefs2='a5 a17' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a5_a50' order='S' atomRefs2='a5 a50' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a6_a7' order='D' atomRefs2='a6 a7' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a6_a8' order='S' atomRefs2='a6 a8' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a6_a9' order='S' atomRefs2='a6 a9' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a7_a10' order='S' atomRefs2='a7 a10' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a7_a12' order='S' atomRefs2='a7 a12' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a8_a14' order='D' atomRefs2='a8 a14' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a8_a16' order='S' atomRefs2='a8 a16' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a9_a13' order='D' atomRefs2='a9 a13' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a9_a15' order='S' atomRefs2='a9 a15' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a10_a11' order='D' atomRefs2='a10 a11' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a10_a32' order='S' atomRefs2='a10 a32' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a11_a17' order='S' atomRefs2='a11 a17' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a11_a25' order='S' atomRefs2='a11 a25' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a12_a18' order='D' atomRefs2='a12 a18' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a12_a33' order='S' atomRefs2='a12 a33' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a13_a19' order='S' atomRefs2='a13 a19' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a13_a34' order='S' atomRefs2='a13 a34' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a14_a20' order='S' atomRefs2='a14 a20' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a14_a35' order='S' atomRefs2='a14 a35' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a15_a21' order='D' atomRefs2='a15 a21' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a15_a36' order='S' atomRefs2='a15 a36' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a16_a22' order='D' atomRefs2='a16 a22' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a16_a37' order='S' atomRefs2='a16 a37' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a17_a18' order='S' atomRefs2='a17 a18' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a18_a38' order='S' atomRefs2='a18 a38' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a19_a23' order='D' atomRefs2='a19 a23' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a19_a40' order='S' atomRefs2='a19 a40' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a20_a24' order='D' atomRefs2='a20 a24' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a20_a39' order='S' atomRefs2='a20 a39' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a21_a23' order='S' atomRefs2='a21 a23' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a21_a42' order='S' atomRefs2='a21 a42' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a22_a24' order='S' atomRefs2='a22 a24' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a22_a41' order='S' atomRefs2='a22 a41' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a25_a43' order='S' atomRefs2='a25 a43' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a25_a44' order='S' atomRefs2='a25 a44' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a25_a45' order='S' atomRefs2='a25 a45' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a26_a27' order='D' atomRefs2='a26 a27' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a26_a28' order='S' atomRefs2='a26 a28' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a27_a29' order='S' atomRefs2='a27 a29' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a27_a51' order='S' atomRefs2='a27 a51' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a28_a30' order='D' atomRefs2='a28 a30' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a28_a52' order='S' atomRefs2='a28 a52' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a29_a31' order='D' atomRefs2='a29 a31' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a29_a53' order='S' atomRefs2='a29 a53' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a30_a31' order='S' atomRefs2='a30 a31' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a30_a54' order='S' atomRefs2='a30 a54' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
<bond id='a31_a55' order='S' atomRefs2='a31 a55' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
</bondArray>
<name dictRef='pc:iupac'>4-[(4-aminophenyl)-(4-imino-3-methyl-cyclohexa-2,5-dien-1-ylidene)methyl]aniline;phenol;hydrochloride</name>
<name dictRef='pc:iupac'>4-[(4-aminophenyl)-(4-imino-3-methyl-1-cyclohexa-2,5-dienylidene)methyl]aniline;phenol;hydrochloride</name>
<name dictRef='pc:iupac'>4-[(4-aminophenyl)-(4-imino-3-methylcyclohexa-2,5-dien-1-ylidene)methyl]aniline;phenol;hydrochloride</name>
<name dictRef='pc:iupac'>4-[(4-aminophenyl)-(4-azanylidene-3-methyl-cyclohexa-2,5-dien-1-ylidene)methyl]aniline;phenol;hydrochloride</name>
<name dictRef='pc:iupac'>[4-[(4-aminophenyl)-(4-imino-3-methyl-cyclohexa-2,5-dien-1-ylidene)methyl]phenyl]amine;phenol;hydrochloride</name>
<cidCommand instruction='NormalizeOnImport' xmlns='http://www.xml-cml.org/dictionary/cmlx/'/>
<formula concise='C 26 H 26 Cl 1 N 3 O 1'/>
</molecule>
</cml>
";

        public const string MOLFILE_BENZENE =
            @"Molecule from ChemDoodle Web Components

http://www.ichemlabs.com
  6  6  0  0  0  0            999 v2000
    0.0000    1.0000    0.0000 C   0  0  0  0  0  0
    0.8660    0.5000    0.0000 C   0  0  0  0  0  0
    0.8660   -0.5000    0.0000 C   0  0  0  0  0  0
    0.0000   -1.0000    0.0000 C   0  0  0  0  0  0
   -0.8660   -0.5000    0.0000 C   0  0  0  0  0  0
   -0.8660    0.5000    0.0000 C   0  0  0  0  0  0
  1  2  1  0     0  0
  2  3  2  0     0  0
  3  4  1  0     0  0
  4  5  2  0     0  0
  5  6  1  0     0  0
  6  1  2  0     0  0
M  END
";

        public const string MOLFILE_INSULIN =
            @"
  Marvin  08171212142D

404415  0  0  1  0            999 V2000
    9.8889  -31.0465    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    9.8891  -29.7165    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
    8.7369  -31.7113    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    7.5852  -31.0461    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   11.0388  -31.7037    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   13.3424  -31.7040    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   13.3422  -33.0340    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   12.1907  -31.0388    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   14.4925  -31.0472    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   16.7961  -31.0476    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   16.7963  -29.7176    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   15.6442  -31.7124    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   17.9460  -31.7048    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   20.2496  -31.7051    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   20.2494  -33.0351    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   19.0979  -31.0399    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   21.3998  -31.0483    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   23.7034  -31.0487    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   23.7036  -29.7187    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   22.5514  -31.7135    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   24.8533  -31.7059    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   26.0053  -29.7110    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   24.8535  -29.0459    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   26.0052  -31.0410    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   27.1493  -29.0436    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   28.3012  -27.0487    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   28.3012  -25.7187    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   27.1493  -27.7136    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   29.4512  -27.7057    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   31.7548  -27.7056    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   31.7548  -29.0356    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   30.6030  -27.0406    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   32.9048  -27.0486    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   35.2084  -27.0487    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   35.2084  -25.7187    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   34.0566  -27.7136    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   36.3584  -27.7057    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   38.6621  -27.7056    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   38.6621  -29.0356    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   37.5102  -27.0406    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   39.8121  -27.0486    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   42.1157  -27.0487    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   42.1157  -25.7187    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   40.9639  -27.7136    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   43.2657  -27.7057    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   45.5693  -27.7056    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   45.5693  -29.0356    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   44.4175  -27.0406    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   46.7193  -27.0486    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   49.0230  -27.0487    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   49.0230  -25.7187    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   47.8711  -27.7136    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   50.1730  -27.7057    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   52.4766  -27.7056    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   52.4766  -29.0356    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   51.3247  -27.0406    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   53.6266  -27.0486    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   55.9302  -27.0487    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   55.9302  -25.7187    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   54.7784  -27.7136    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   57.0802  -27.7057    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   59.3838  -27.7056    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   59.3838  -29.0356    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   58.2320  -27.0406    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   60.5338  -27.0486    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   62.8375  -27.0487    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   62.8375  -25.7187    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   61.6856  -27.7136    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   63.9875  -27.7057    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   66.2911  -27.7056    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   67.4429  -27.0405    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   65.1393  -27.0406    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   66.2972  -29.0300    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   67.4491  -31.0246    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   68.6008  -31.6897    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   67.4491  -29.6948    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   66.3051  -31.6920    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   65.1532  -33.6869    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   64.0014  -33.0219    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   66.3051  -33.0220    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   65.1471  -35.0114    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   63.9953  -37.0060    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   65.1471  -37.6710    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   63.9953  -35.6762    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   62.8513  -37.6733    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   61.6994  -39.6683    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   61.6994  -40.9983    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   62.8513  -39.0033    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    8.7370  -33.0333    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    9.8907  -33.6905    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   11.0386  -33.0188    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    9.8984  -35.0205    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   12.1943  -33.6773    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   11.0540  -35.6789    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   12.2020  -35.0072    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   12.1908  -29.7088    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   11.0391  -29.0437    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   13.3427  -29.0440    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   15.6434  -33.0344    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   14.4912  -33.6988    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   16.7949  -33.7001    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   19.0981  -29.7099    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   20.2500  -29.0451    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   20.2501  -27.7151    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   21.4020  -27.0502    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   19.0984  -27.0499    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   22.5512  -33.0435    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   23.7029  -33.7087    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   24.9180  -33.1680    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   23.8416  -35.0315    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   25.8077  -34.1566    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   25.1427  -35.3083    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   27.1569  -31.7061    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   28.3088  -31.0412    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   29.4605  -31.7064    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   28.3089  -29.7112    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   25.9975  -27.0486    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   25.9976  -25.7186    0.0000 S   0  0  0  0  0  0  0  0  0  0  0  0
   34.0565  -29.0436    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   35.2072  -29.7087    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   37.5102  -25.7106    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   38.6620  -25.0456    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   38.8010  -23.7229    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   39.8770  -25.5865    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   40.1019  -23.4463    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   40.7670  -24.5979    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   40.9598  -29.0356    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   42.1096  -29.7042    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   42.1056  -31.0342    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   43.2635  -29.0427    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   44.4174  -25.7106    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   45.5692  -25.0456    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   43.2656  -25.0456    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   47.8711  -29.0436    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   49.0228  -29.7087    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   49.0228  -31.0387    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   50.1745  -31.7038    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   47.8709  -31.7036    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   51.3247  -25.7106    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   54.7783  -29.0436    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   55.9263  -29.7087    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   55.9244  -31.0387    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   57.0791  -29.0453    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   58.2320  -25.7106    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   59.3837  -25.0456    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   59.3837  -23.7156    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   60.5357  -25.7105    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   60.5356  -23.0506    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   61.6874  -25.0455    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   61.6874  -23.7155    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   62.8487  -23.0587    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   61.6856  -29.0436    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   62.8327  -29.7087    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   62.8304  -31.0387    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   63.9857  -29.0457    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   65.1392  -25.7106    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   66.2910  -25.0456    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   63.9874  -25.0456    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   68.6008  -29.0297    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   68.6035  -27.7045    0.0000 S   0  0  0  0  0  0  0  0  0  0  0  0
   62.8435  -35.0111    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   61.6946  -35.6708    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   60.5444  -35.0031    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   59.3910  -35.6654    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   60.5475  -33.6731    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   64.0031  -39.6684    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   64.0030  -40.9984    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   65.1548  -41.6634    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   65.1548  -42.9934    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   66.3066  -43.6584    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   66.3065  -44.9884    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   67.4584  -42.9935    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   60.5476  -39.0033    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   59.3958  -39.6683    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   58.2440  -39.0033    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   57.0922  -39.6683    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   58.2440  -37.6733    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   54.7889  -39.6682    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   54.7889  -40.9982    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   55.9405  -39.0032    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   53.6389  -39.0112    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   51.3353  -39.0113    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   51.3353  -37.6813    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   52.4871  -39.6762    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   50.1853  -39.6683    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   47.8820  -39.6682    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   47.8820  -40.9982    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   49.0336  -39.0032    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   46.7320  -39.0112    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   44.4284  -39.0113    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   44.4283  -37.6813    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   45.5802  -39.6763    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   43.2784  -39.6683    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   55.9405  -37.6732    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   52.4872  -41.0062    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   54.7887  -37.0082    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   53.6369  -37.6732    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   54.7887  -35.6781    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   52.4850  -37.0081    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   53.6369  -35.0131    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   52.4851  -35.6781    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   51.3354  -41.6713    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   51.3354  -43.0013    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   50.1834  -41.0063    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   50.1835  -43.6663    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   49.0317  -41.6713    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   49.0317  -43.0013    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   49.0336  -37.6732    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   47.8818  -37.0082    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   46.7299  -37.6732    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   47.8818  -35.6781    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   45.5781  -37.0081    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   46.7300  -35.0131    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   45.5781  -35.6781    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   44.4276  -35.0212    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   45.5802  -41.0063    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   44.4285  -41.6713    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   46.7321  -41.6712    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   41.5374  -40.9332    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   41.1264  -39.6683    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   42.8674  -40.9332    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   42.2024  -38.8865    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   42.2024  -37.5565    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   43.3542  -36.8915    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   41.0554  -36.8943    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   38.7518  -36.8839    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   38.7578  -35.5539    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   39.9006  -37.5541    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   37.5988  -37.5357    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   35.2955  -37.5251    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   35.2895  -38.8551    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   36.4501  -36.8654    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   34.1485  -36.8629    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   39.8946  -38.8841    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   38.7398  -39.5439    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   38.7337  -40.8739    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   37.5789  -41.5337    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   37.5729  -42.8637    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   36.4502  -35.5354    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   35.2984  -34.8704    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   37.6020  -34.8704    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   68.6463  -25.1934    0.0000 S   0  0  0  0  0  0  0  0  0  0  0  0
   11.0716  -13.2323    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   11.0716  -11.9023    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
    9.9197  -13.8973    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    8.7680  -13.2322    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   12.2216  -13.8893    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   14.5252  -13.8892    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   14.5252  -15.2192    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   13.3734  -13.2243    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   15.6752  -13.2322    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   17.9788  -13.2323    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   17.9788  -11.9023    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   16.8270  -13.8973    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   19.1288  -13.8893    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   21.4325  -13.8892    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   21.4325  -15.2192    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   20.2806  -13.2243    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   22.5825  -13.2322    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   24.8861  -13.2323    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   24.8861  -11.9023    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   23.7343  -13.8973    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   26.0361  -13.8893    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   28.3397  -13.8892    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   28.3397  -15.2192    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   27.1879  -13.2243    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   29.4897  -13.2322    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   31.7934  -13.2323    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   31.7934  -11.9023    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   30.6415  -13.8973    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   32.9434  -13.8893    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   35.2470  -13.8892    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   35.2470  -15.2192    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   34.0951  -13.2243    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   36.3970  -13.2322    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   38.7006  -13.2323    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   38.7006  -11.9023    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   37.5488  -13.8973    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   39.8506  -13.8893    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   42.1542  -13.8892    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   42.1543  -15.2192    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   41.0024  -13.2243    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   43.3060  -13.2242    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   44.4579  -15.2191    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   43.3061  -15.8842    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   44.4579  -13.8891    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   45.6097  -15.8841    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   46.7616  -17.8791    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   46.7617  -19.2091    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   45.6098  -17.2141    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   47.9134  -17.2140    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   50.2170  -17.2140    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   50.2170  -15.8840    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   49.0653  -17.8790    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   51.3689  -17.8789    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   53.6725  -17.8788    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   53.6726  -19.2088    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   52.5207  -17.2139    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   54.8243  -17.2138    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   57.1279  -17.2137    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   57.1279  -15.8837    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   55.9761  -17.8788    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   58.2798  -17.8787    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   60.5834  -17.8786    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   60.5834  -19.2086    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   59.4316  -17.2137    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   61.7352  -17.2136    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   64.0388  -17.2135    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   64.0388  -15.8835    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   62.8870  -17.8785    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   65.1907  -17.8785    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   67.4943  -17.8784    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   68.6461  -17.2134    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   66.3424  -17.2134    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   67.4943  -19.2084    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   68.6462  -21.2034    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   67.4944  -21.8684    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   68.6462  -19.8734    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   69.7980  -21.8683    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   70.9499  -23.8633    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   72.1017  -23.1982    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   69.7981  -23.1983    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   70.9500  -25.1933    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   73.2536  -25.1932    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   73.2535  -23.8632    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   72.1018  -25.8582    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   74.4054  -25.8582    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   13.3733  -11.8943    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   12.2215  -11.2293    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   14.5251  -11.2292    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   14.5251   -9.8992    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   16.8269  -15.2273    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   15.6751  -15.8922    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   17.9787  -15.8923    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   20.2806  -11.8943    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   21.4324  -11.2292    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   21.4323   -9.8992    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   20.2805   -9.2343    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   22.5841   -9.2342    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   23.7342  -15.2273    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   24.8860  -15.8923    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   24.8859  -17.2223    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   26.0377  -17.8874    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   23.7341  -17.8873    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   27.1878  -11.8943    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   30.6415  -15.2273    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   34.0951  -11.8943    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   35.2469  -11.2292    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   32.9433  -11.2293    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   37.5487  -15.2273    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   36.3969  -15.8922    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   41.0024  -11.8943    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   42.1541  -11.2292    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   39.8505  -11.2293    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   39.8505   -9.8993    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   45.6097  -13.2241    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   44.4580  -17.8791    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   44.4580  -19.2092    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   49.0653  -19.2090    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   50.2171  -19.8740    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   50.2172  -21.2040    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   51.3689  -19.2089    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   52.5206  -15.8839    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   53.6724  -15.2188    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   53.6724  -13.8888    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   54.8243  -15.8838    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   54.8242  -13.2238    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   55.9761  -15.2188    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   55.9760  -13.8888    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   57.1278  -13.2237    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   55.9762  -19.2088    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   57.1280  -19.8737    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   57.1281  -21.2037    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   58.2799  -21.8687    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   55.9763  -21.8688    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   59.4315  -15.8837    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   60.5833  -15.2186    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   60.5833  -13.8886    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   61.7351  -15.8836    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   62.8871  -19.2085    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   64.0389  -19.8735    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   64.0389  -21.2035    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   65.1908  -21.8685    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   62.8872  -21.8685    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   66.3424  -15.8834    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   67.4942  -15.2184    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   67.4941  -13.8884    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   68.6460  -15.8834    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   69.7979  -19.2083    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   70.9498  -19.8733    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   70.9498  -21.2033    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   72.1016  -19.2083    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   72.1017  -21.8682    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   73.2534  -19.8732    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   73.2534  -21.2032    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   74.4053  -21.8682    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   68.6463  -23.8634    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   72.1018  -27.1882    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   73.2537  -27.8532    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
   73.2537  -29.1832    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0
   74.4055  -27.1882    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
   45.6096   -7.8881    0.0000 S   0  0  0  0  0  0  0  0  0  0  0  0
   34.0844   -7.9123    0.0000 S   0  0  0  0  0  0  0  0  0  0  0  0
   29.4843  -15.8959    0.0000 S   0  0  0  0  0  0  0  0  0  0  0  0
  2  1  2  0  0  0  0
  3  1  1  0  0  0  0
  3  4  1  6  0  0  0
  1  5  1  0  0  0  0
  7  6  2  0  0  0  0
  8  6  1  0  0  0  0
  8  5  1  1  0  0  0
  6  9  1  0  0  0  0
 11 10  2  0  0  0  0
 12 10  1  0  0  0  0
 12  9  1  6  0  0  0
 10 13  1  0  0  0  0
 15 14  2  0  0  0  0
 16 14  1  0  0  0  0
 16 13  1  1  0  0  0
 14 17  1  0  0  0  0
 19 18  2  0  0  0  0
 20 18  1  0  0  0  0
 20 17  1  6  0  0  0
 18 21  1  0  0  0  0
 23 22  2  0  0  0  0
 24 22  1  0  0  0  0
 24 21  1  6  0  0  0
 22 25  1  0  0  0  0
 27 26  2  0  0  0  0
 28 26  1  0  0  0  0
 28 25  1  1  0  0  0
 26 29  1  0  0  0  0
 31 30  2  0  0  0  0
 32 30  1  0  0  0  0
 32 29  1  0  0  0  0
 30 33  1  0  0  0  0
 35 34  2  0  0  0  0
 36 34  1  0  0  0  0
 36 33  1  6  0  0  0
 34 37  1  0  0  0  0
 39 38  2  0  0  0  0
 40 38  1  0  0  0  0
 40 37  1  1  0  0  0
 38 41  1  0  0  0  0
 43 42  2  0  0  0  0
 44 42  1  0  0  0  0
 44 41  1  6  0  0  0
 42 45  1  0  0  0  0
 47 46  2  0  0  0  0
 48 46  1  0  0  0  0
 48 45  1  1  0  0  0
 46 49  1  0  0  0  0
 51 50  2  0  0  0  0
 52 50  1  0  0  0  0
 52 49  1  6  0  0  0
 50 53  1  0  0  0  0
 55 54  2  0  0  0  0
 56 54  1  0  0  0  0
 56 53  1  1  0  0  0
 54 57  1  0  0  0  0
 59 58  2  0  0  0  0
 60 58  1  0  0  0  0
 60 57  1  6  0  0  0
 58 61  1  0  0  0  0
 63 62  2  0  0  0  0
 64 62  1  0  0  0  0
 64 61  1  1  0  0  0
 62 65  1  0  0  0  0
 67 66  2  0  0  0  0
 68 66  1  0  0  0  0
 68 65  1  6  0  0  0
 66 69  1  0  0  0  0
 71 70  2  0  0  0  0
 72 70  1  0  0  0  0
 72 69  1  1  0  0  0
 70 73  1  0  0  0  0
 75 74  2  0  0  0  0
 76 74  1  0  0  0  0
 76 73  1  1  0  0  0
 74 77  1  0  0  0  0
 79 78  2  0  0  0  0
 80 78  1  0  0  0  0
 80 77  1  0  0  0  0
 78 81  1  0  0  0  0
 83 82  2  0  0  0  0
 84 82  1  0  0  0  0
 84 81  1  6  0  0  0
 82 85  1  0  0  0  0
 87 86  2  0  0  0  0
 88 86  1  0  0  0  0
 88 85  1  1  0  0  0
  3 89  1  0  0  0  0
 89 90  1  0  0  0  0
 91 90  1  0  0  0  0
 92 90  2  0  0  0  0
 93 91  2  0  0  0  0
 94 92  1  0  0  0  0
 95 93  1  0  0  0  0
 95 94  2  0  0  0  0
  8 96  1  0  0  0  0
 96 97  1  0  0  0  0
 96 98  1  0  0  0  0
 12 99  1  0  0  0  0
 99100  2  0  0  0  0
 99101  1  0  0  0  0
 16102  1  0  0  0  0
102103  1  0  0  0  0
103104  1  0  0  0  0
104105  1  0  0  0  0
104106  2  0  0  0  0
 20107  1  0  0  0  0
109108  1  0  0  0  0
110108  2  0  0  0  0
111109  2  0  0  0  0
112110  1  0  0  0  0
112111  1  0  0  0  0
107108  1  0  0  0  0
 24113  1  0  0  0  0
113114  1  0  0  0  0
114115  1  0  0  0  0
114116  1  0  0  0  0
 28117  1  0  0  0  0
117118  1  0  0  0  0
 36119  1  0  0  0  0
119120  1  0  0  0  0
 40121  1  0  0  0  0
123122  2  0  0  0  0
124122  1  0  0  0  0
125123  1  0  0  0  0
126124  2  0  0  0  0
126125  1  0  0  0  0
121122  1  0  0  0  0
 44127  1  0  0  0  0
127128  1  0  0  0  0
128129  1  0  0  0  0
128130  1  0  0  0  0
 48131  1  0  0  0  0
131132  1  0  0  0  0
131133  1  0  0  0  0
 52134  1  0  0  0  0
134135  1  0  0  0  0
135136  1  0  0  0  0
136137  1  0  0  0  0
136138  2  0  0  0  0
 56139  1  0  0  0  0
 60140  1  0  0  0  0
140141  1  0  0  0  0
141142  1  0  0  0  0
141143  1  0  0  0  0
 64144  1  0  0  0  0
146145  1  0  0  0  0
147145  2  0  0  0  0
148146  2  0  0  0  0
149147  1  0  0  0  0
150148  1  0  0  0  0
150149  2  0  0  0  0
144145  1  0  0  0  0
150151  1  0  0  0  0
 68152  1  0  0  0  0
152153  1  0  0  0  0
153154  1  0  0  0  0
153155  1  0  0  0  0
 72156  1  0  0  0  0
156157  1  0  0  0  0
156158  1  0  0  0  0
 76159  1  0  0  0  0
159160  1  0  0  0  0
 84161  1  0  0  0  0
161162  1  0  0  0  0
162163  1  0  0  0  0
163164  1  0  0  0  0
163165  2  0  0  0  0
 88166  1  0  0  0  0
166167  1  0  0  0  0
167168  1  0  0  0  0
168169  1  0  0  0  0
169170  1  0  0  0  0
170171  1  0  0  0  0
170172  2  0  0  0  0
 86173  1  0  0  0  0
173174  1  0  0  0  0
174175  1  0  0  0  0
175176  1  0  0  0  0
175177  2  0  0  0  0
179178  2  0  0  0  0
180178  1  0  0  0  0
180176  1  6  0  0  0
178181  1  0  0  0  0
183182  2  0  0  0  0
184182  1  0  0  0  0
184181  1  1  0  0  0
182185  1  0  0  0  0
187186  2  0  0  0  0
188186  1  0  0  0  0
188185  1  6  0  0  0
186189  1  0  0  0  0
191190  2  0  0  0  0
192190  1  0  0  0  0
192189  1  1  0  0  0
190193  1  0  0  0  0
180194  1  0  0  0  0
184195  1  0  0  0  0
197196  1  0  0  0  0
198196  2  0  0  0  0
199197  2  0  0  0  0
200198  1  0  0  0  0
201199  1  0  0  0  0
201200  2  0  0  0  0
194196  1  0  0  0  0
203202  1  0  0  0  0
204202  2  0  0  0  0
205203  2  0  0  0  0
206204  1  0  0  0  0
207205  1  0  0  0  0
207206  2  0  0  0  0
195202  1  0  0  0  0
188208  1  0  0  0  0
210209  1  0  0  0  0
211209  2  0  0  0  0
212210  2  0  0  0  0
213211  1  0  0  0  0
214212  1  0  0  0  0
214213  2  0  0  0  0
208209  1  0  0  0  0
214215  1  0  0  0  0
192216  1  0  0  0  0
216217  1  6  0  0  0
216218  1  0  0  0  0
220219  1  0  0  0  0
221219  1  0  0  0  0
222220  1  0  0  0  0
193221  1  0  0  0  0
222193  1  1  0  0  0
224223  2  0  0  0  0
222223  1  0  0  0  0
223225  1  0  0  0  0
227226  2  0  0  0  0
228226  1  0  0  0  0
228225  1  1  0  0  0
226229  1  0  0  0  0
231230  2  0  0  0  0
232230  1  0  0  0  0
232229  1  6  0  0  0
230233  1  0  0  0  0
228234  1  0  0  0  0
234235  1  0  0  0  0
235236  1  0  0  0  0
236237  1  0  0  0  0
237238  1  0  0  0  0
232239  1  0  0  0  0
239240  1  1  0  0  0
239241  1  0  0  0  0
244243  2  0  0  0  0
245243  1  0  0  0  0
245246  1  0  0  0  0
243247  1  0  0  0  0
249248  2  0  0  0  0
250248  1  0  0  0  0
250247  1  1  0  0  0
248251  1  0  0  0  0
253252  2  0  0  0  0
254252  1  0  0  0  0
254251  1  6  0  0  0
252255  1  0  0  0  0
257256  2  0  0  0  0
258256  1  0  0  0  0
258255  1  1  0  0  0
256259  1  0  0  0  0
261260  2  0  0  0  0
262260  1  0  0  0  0
262259  1  6  0  0  0
260263  1  0  0  0  0
265264  2  0  0  0  0
266264  1  0  0  0  0
266263  1  1  0  0  0
264267  1  0  0  0  0
269268  2  0  0  0  0
270268  1  0  0  0  0
270267  1  6  0  0  0
268271  1  0  0  0  0
273272  2  0  0  0  0
274272  1  0  0  0  0
274271  1  1  0  0  0
272275  1  0  0  0  0
277276  2  0  0  0  0
278276  1  0  0  0  0
278275  1  6  0  0  0
276279  1  0  0  0  0
281280  2  0  0  0  0
282280  1  0  0  0  0
282279  1  1  0  0  0
280283  1  0  0  0  0
285284  2  0  0  0  0
286284  1  0  0  0  0
286283  1  6  0  0  0
284287  1  0  0  0  0
289288  2  0  0  0  0
290288  1  0  0  0  0
290287  1  6  0  0  0
288291  1  0  0  0  0
293292  2  0  0  0  0
294292  1  0  0  0  0
294291  1  6  0  0  0
292295  1  0  0  0  0
297296  2  0  0  0  0
298296  1  0  0  0  0
298295  1  1  0  0  0
296299  1  0  0  0  0
301300  2  0  0  0  0
302300  1  0  0  0  0
302299  1  6  0  0  0
300303  1  0  0  0  0
305304  2  0  0  0  0
306304  1  0  0  0  0
306303  1  1  0  0  0
304307  1  0  0  0  0
309308  2  0  0  0  0
310308  1  0  0  0  0
310307  1  6  0  0  0
308311  1  0  0  0  0
313312  2  0  0  0  0
314312  1  0  0  0  0
314311  1  1  0  0  0
312315  1  0  0  0  0
317316  2  0  0  0  0
318316  1  0  0  0  0
318315  1  1  0  0  0
316319  1  0  0  0  0
321320  2  0  0  0  0
322320  1  0  0  0  0
322319  1  6  0  0  0
320323  1  0  0  0  0
325324  2  0  0  0  0
326324  1  0  0  0  0
326323  1  6  0  0  0
324327  1  0  0  0  0
250328  1  0  0  0  0
328329  1  6  0  0  0
328330  1  0  0  0  0
330331  1  0  0  0  0
254332  1  0  0  0  0
332333  1  0  0  0  0
332334  1  0  0  0  0
258335  1  0  0  0  0
335336  1  0  0  0  0
336337  1  0  0  0  0
337338  2  0  0  0  0
337339  1  0  0  0  0
262340  1  0  0  0  0
340341  1  0  0  0  0
341342  1  0  0  0  0
342343  1  0  0  0  0
342344  2  0  0  0  0
266345  1  0  0  0  0
270346  1  0  0  0  0
274347  1  0  0  0  0
347348  1  6  0  0  0
347349  1  0  0  0  0
278350  1  0  0  0  0
350351  1  0  0  0  0
282352  1  0  0  0  0
352353  1  1  0  0  0
352354  1  0  0  0  0
354355  1  0  0  0  0
286356  1  0  0  0  0
290357  1  0  0  0  0
357358  1  0  0  0  0
294359  1  0  0  0  0
359360  1  0  0  0  0
360361  1  0  0  0  0
360362  1  0  0  0  0
298363  1  0  0  0  0
365364  1  0  0  0  0
366364  2  0  0  0  0
367365  2  0  0  0  0
368366  1  0  0  0  0
369367  1  0  0  0  0
369368  2  0  0  0  0
363364  1  0  0  0  0
369370  1  0  0  0  0
302371  1  0  0  0  0
371372  1  0  0  0  0
372373  1  0  0  0  0
373374  1  0  0  0  0
373375  2  0  0  0  0
306376  1  0  0  0  0
376377  1  0  0  0  0
377378  1  0  0  0  0
377379  1  0  0  0  0
310380  1  0  0  0  0
380381  1  0  0  0  0
381382  1  0  0  0  0
382383  1  0  0  0  0
382384  2  0  0  0  0
314385  1  0  0  0  0
385386  1  0  0  0  0
386387  2  0  0  0  0
386388  1  0  0  0  0
318389  1  0  0  0  0
391390  1  0  0  0  0
392390  2  0  0  0  0
393391  2  0  0  0  0
394392  1  0  0  0  0
395393  1  0  0  0  0
395394  2  0  0  0  0
389390  1  0  0  0  0
395396  1  0  0  0  0
322397  1  0  0  0  0
397242  1  0  0  0  0
326398  1  0  0  0  0
398399  1  0  0  0  0
399400  2  0  0  0  0
399401  1  0  0  0  0
356402  1  0  0  0  0
345403  1  0  0  0  0
403402  1  0  0  0  0
160242  1  0  0  0  0
346404  1  0  0  0  0
404118  1  0  0  0  0
M  END
"
           ;

        public const string BASIC_PARAFUCHSIN =
            @"11292
  -OEChem-03011708522D

 41 42  0     0  0  0  0  0  0999 V2000
    3.2326    0.0000    0.0000 Cl  0  0  0  0  0  0  0  0  0  0  0  0
    6.9282    8.8100    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
    3.4641    2.8100    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
    0.0000    8.8100    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0
    3.4641    6.8100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    3.4641    5.8100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    4.3301    7.3100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    2.5981    7.3100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    5.1962    6.8100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    2.5981    5.3100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    4.3301    8.3100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    4.3301    5.3100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    2.5981    8.3100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    1.7320    6.8100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    2.5981    4.3100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    6.0622    7.3100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    5.1962    8.8100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    4.3301    4.3100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    3.4641    3.8100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    6.0622    8.3100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    1.7320    8.8100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    0.8660    7.3100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    0.8660    8.3100    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0
    5.1962    6.1900    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    2.0611    5.6200    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    4.8671    5.6200    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    3.7932    8.6200    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    3.1350    8.6200    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    1.7320    6.1900    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    2.0611    4.0000    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    6.5991    7.0000    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    4.8671    4.0000    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    5.1962    9.4300    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    1.7320    9.4300    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    0.3291    7.0000    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    6.9282    9.4300    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    2.9272    2.5000    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    4.0010    2.5000    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    7.4651    8.5000    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    0.0000    9.4300    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
    4.2326    0.0000    0.0000 H   0  0  0  0  0  0  0  0  0  0  0  0
  1 41  1  0  0  0  0
  2 20  1  0  0  0  0
  2 36  1  0  0  0  0
  2 39  1  0  0  0  0
  3 19  1  0  0  0  0
  3 37  1  0  0  0  0
  3 38  1  0  0  0  0
  4 23  2  0  0  0  0
  4 40  1  0  0  0  0
  5  6  1  0  0  0  0
  5  7  1  0  0  0  0
  5  8  2  0  0  0  0
  6 10  2  0  0  0  0
  6 12  1  0  0  0  0
  7  9  2  0  0  0  0
  7 11  1  0  0  0  0
  8 13  1  0  0  0  0
  8 14  1  0  0  0  0
  9 16  1  0  0  0  0
  9 24  1  0  0  0  0
 10 15  1  0  0  0  0
 10 25  1  0  0  0  0
 11 17  2  0  0  0  0
 11 27  1  0  0  0  0
 12 18  2  0  0  0  0
 12 26  1  0  0  0  0
 13 21  2  0  0  0  0
 13 28  1  0  0  0  0
 14 22  2  0  0  0  0
 14 29  1  0  0  0  0
 15 19  2  0  0  0  0
 15 30  1  0  0  0  0
 16 20  2  0  0  0  0
 16 31  1  0  0  0  0
 17 20  1  0  0  0  0
 17 33  1  0  0  0  0
 18 19  1  0  0  0  0
 18 32  1  0  0  0  0
 21 23  1  0  0  0  0
 21 34  1  0  0  0  0
 22 23  1  0  0  0  0
 22 35  1  0  0  0  0
M  END
";
    }
}