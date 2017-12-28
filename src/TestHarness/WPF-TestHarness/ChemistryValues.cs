// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

namespace Chem4WordTests
{
    public static class ChemistryValues
    {
        public const string INSULIN = @"<cml:cml id='2f32cee477524c249c61a8fe33e4602f' convention='conventions:molecular' xmlns:cml='http://www.xml-cml.org/schema' xmlns:conventions='http://www.xml-cml.org/convention/' xmlns:cmlDict='http://www.xml-cml.org/dictionary/cml/' xmlns:nameDict='http://www.xml-cml.org/dictionary/cml/name/'>
  <molecule xmlns = 'http://www.xml-cml.org/schema' >
    <atomArray>
      <atom id='a1' x2='140.818079927863' y2='442.135731132731' elementType='C' />
      <atom id = 'a2' x2='140.832321238536' y2='423.19478793855' elementType='O' />
      <atom id = 'a3' x2='124.412090033355' y2='451.606202729821' elementType='C' />
      <atom id = 'a4' x2='108.020341449519' y2='442.135731132731' elementType='N' />
      <atom id = 'a5' x2='157.195587201027' y2='451.492272244443' elementType='N' />
      <atom id = 'a6' x2='190.007566990044' y2='451.506513555115' elementType='C' />
      <atom id = 'a7' x2='190.007566990044' y2='470.447456749296' elementType='O' />
      <atom id = 'a8' x2='173.601577095535' y2='442.021800647352' elementType='C' />
      <atom id = 'a9' x2='206.385074263207' y2='442.149972443403' elementType='N' />
      <atom id = 'a10' x2='239.197054052224' y2='442.149972443403' elementType='C' />
      <atom id = 'a11' x2='239.197054052224' y2='423.209029249223' elementType='O' />
      <atom id = 'a12' x2='222.791064157716' y2='451.620444040494' elementType='C' />
      <atom id = 'a13' x2='255.574561325388' y2='451.506513555115' elementType='N' />
      <atom id = 'a14' x2='288.372299803732' y2='451.520754865787' elementType='C' />
      <atom id = 'a15' x2='288.372299803732' y2='470.461698059968' elementType='O' />
      <atom id = 'a16' x2='271.966309909224' y2='442.036041958025' elementType='C' />
      <atom id = 'a17' x2='304.749807076896' y2='442.164213754075' elementType='N' />
      <atom id = 'a18' x2='337.561786865913' y2='442.164213754075' elementType='C' />
      <atom id = 'a19' x2='337.561786865913' y2='423.223270559895' elementType='O' />
      <atom id = 'a20' x2='321.155796971404' y2='451.634685351166' elementType='C' />
      <atom id = 'a21' x2='353.939294139076' y2='451.520754865787' elementType='N' />
      <atom id = 'a22' x2='370.345284033585' y2='423.123581385189' elementType='C' />
      <atom id = 'a23' x2='353.939294139076' y2='413.638868477426' elementType='O' />
      <atom id = 'a24' x2='370.345284033585' y2='442.064524579369' elementType='C' />
      <atom id = 'a25' x2='386.637343442714' y2='413.610385856081' elementType='N' />
      <atom id = 'a26' x2='403.043333337223' y2='385.19897106481' elementType='C' />
      <atom id = 'a27' x2='403.043333337223' y2='366.25802787063' elementType='O' />
      <atom id = 'a28' x2='386.637343442714' y2='394.669442661901' elementType='C' />
      <atom id = 'a29' x2='419.420840610386' y2='394.555512176522' elementType='N' />
      <atom id = 'a30' x2='452.218579088731' y2='394.555512176522' elementType='C' />
      <atom id = 'a31' x2='452.218579088731' y2='413.496455370703' elementType='O' />
      <atom id = 'a32' x2='435.826830504895' y2='385.085040579432' elementType='C' />
      <atom id = 'a33' x2='468.596086361895' y2='385.19897106481' elementType='N' />
      <atom id = 'a34' x2='501.408066150911' y2='385.19897106481' elementType='C' />
      <atom id = 'a35' x2='501.408066150911' y2='366.25802787063' elementType='O' />
      <atom id = 'a36' x2='485.002076256403' y2='394.669442661901' elementType='C' />
      <atom id = 'a37' x2='517.785573424075' y2='394.555512176522' elementType='N' />
      <atom id = 'a38' x2='550.597553213092' y2='394.555512176522' elementType='C' />
      <atom id = 'a39' x2='550.597553213092' y2='413.496455370703' elementType='O' />
      <atom id = 'a40' x2='534.191563318583' y2='385.085040579432' elementType='C' />
      <atom id = 'a41' x2='566.975060486255' y2='385.19897106481' elementType='N' />
      <atom id = 'a42' x2='599.7727989646' y2='385.19897106481' elementType='C' />
      <atom id = 'a43' x2='599.7727989646' y2='366.25802787063' elementType='O' />
      <atom id = 'a44' x2='583.366809070091' y2='394.669442661901' elementType='C' />
      <atom id = 'a45' x2='616.150306237763' y2='394.555512176522' elementType='N' />
      <atom id = 'a46' x2='648.96228602678' y2='394.555512176522' elementType='C' />
      <atom id = 'a47' x2='648.96228602678' y2='413.496455370703' elementType='O' />
      <atom id = 'a48' x2='632.556296132272' y2='385.085040579432' elementType='C' />
      <atom id = 'a49' x2='665.339793299944' y2='385.19897106481' elementType='N' />
      <atom id = 'a50' x2='698.15177308896' y2='385.19897106481' elementType='C' />
      <atom id = 'a51' x2='698.15177308896' y2='366.25802787063' elementType='O' />
      <atom id = 'a52' x2='681.745783194452' y2='394.669442661901' elementType='C' />
      <atom id = 'a53' x2='714.529280362124' y2='394.555512176522' elementType='N' />
      <atom id = 'a54' x2='747.327018840469' y2='394.555512176522' elementType='C' />
      <atom id = 'a55' x2='747.327018840469' y2='413.496455370703' elementType='O' />
      <atom id = 'a56' x2='730.92102894596' y2='385.085040579432' elementType='C' />
      <atom id = 'a57' x2='763.704526113632' y2='385.19897106481' elementType='N' />
      <atom id = 'a58' x2='796.516505902649' y2='385.19897106481' elementType='C' />
      <atom id = 'a59' x2='796.516505902649' y2='366.25802787063' elementType='O' />
      <atom id = 'a60' x2='780.110516008141' y2='394.669442661901' elementType='C' />
      <atom id = 'a61' x2='812.894013175813' y2='394.555512176522' elementType='N' />
      <atom id = 'a62' x2='845.691751654157' y2='394.555512176522' elementType='C' />
      <atom id = 'a63' x2='845.691751654157' y2='413.496455370703' elementType='O' />
      <atom id = 'a64' x2='829.300003070321' y2='385.085040579432' elementType='C' />
      <atom id = 'a65' x2='862.069258927321' y2='385.19897106481' elementType='N' />
      <atom id = 'a66' x2='894.881238716337' y2='385.19897106481' elementType='C' />
      <atom id = 'a67' x2='894.881238716337' y2='366.25802787063' elementType='O' />
      <atom id = 'a68' x2='878.475248821829' y2='394.669442661901' elementType='C' />
      <atom id = 'a69' x2='911.258745989501' y2='394.555512176522' elementType='N' />
      <atom id = 'a70' x2='944.070725778518' y2='394.555512176522' elementType='C' />
      <atom id = 'a71' x2='960.462474362354' y2='385.085040579432' elementType='O' />
      <atom id = 'a72' x2='927.664735884009' y2='385.085040579432' elementType='C' />
      <atom id = 'a73' x2='944.156173642552' y2='413.425248817341' elementType='N' />
      <atom id = 'a74' x2='960.56216353706' y2='441.82242229794' elementType='C' />
      <atom id = 'a75' x2='976.953912120896' y2='451.29289389503' elementType='O' />
      <atom id = 'a76' x2='960.56216353706' y2='422.881479103759' elementType='C' />
      <atom id = 'a77' x2='944.27010412793' y2='451.335617827047' elementType='N' />
      <atom id = 'a78' x2='927.864114233422' y2='479.732791307646' elementType='C' />
      <atom id = 'a79' x2='911.458124338914' y2='470.262319710556' elementType='O' />
      <atom id = 'a80' x2='944.27010412793' y2='470.276561021228' elementType='C' />
      <atom id = 'a81' x2='927.778666369388' y2='498.602527948465' elementType='N' />
      <atom id = 'a82' x2='911.37267647488' y2='527.013942739736' elementType='C' />
      <atom id = 'a83' x2='927.778666369388' y2='536.484414336826' elementType='O' />
      <atom id = 'a84' x2='911.37267647488' y2='508.072999545555' elementType='C' />
      <atom id = 'a85' x2='895.08061706575' y2='536.512896958171' elementType='N' />
      <atom id = 'a86' x2='878.674627171241' y2='564.924311749442' elementType='C' />
      <atom id = 'a87' x2='878.674627171241' y2='583.865254943622' elementType='O' />
      <atom id = 'a88' x2='895.08061706575' y2='555.453840152351' elementType='C' />
      <atom id = 'a89' x2='124.426331344027' y2='470.433215438623' elementType='C' />
      <atom id = 'a90' x2='140.846562549208' y2='479.789756550335' elementType='C' />
      <atom id = 'a91' x2='157.195587201027' y2='470.219595778539' elementType='C' />
      <atom id = 'a92' x2='140.960493034586' y2='498.730699744516' elementType='C' />
      <atom id = 'a93' x2='173.658542338225' y2='479.604619511595' elementType='C' />
      <atom id = 'a94' x2='157.423448171784' y2='508.1014821669' elementType='C' />
      <atom id = 'a95' x2='173.772472823603' y2='498.545562705776' elementType='C' />
      <atom id = 'a96' x2='173.601577095535' y2='423.080857453172' elementType='C' />
      <atom id = 'a97' x2='157.209828511699' y2='413.610385856081' elementType='C' />
      <atom id = 'a98' x2='190.007566990044' y2='413.624627166754' elementType='C' />
      <atom id = 'a99' x2='222.776822847043' y2='470.447456749296' elementType='C' />
      <atom id = 'a100' x2='206.370832952535' y2='479.903687035714' elementType='O' />
      <atom id = 'a101' x2='239.168571430879' y2='479.932169657058' elementType='N' />
      <atom id = 'a102' x2='271.980551219896' y2='423.095098763844' elementType='C' />
      <atom id = 'a103' x2='288.386541114404' y2='413.638868477426' elementType='C' />
      <atom id = 'a104' x2='288.386541114404' y2='394.697925283245' elementType='C' />
      <atom id = 'a105' x2='304.792531008913' y2='385.227453686155' elementType='N' />
      <atom id = 'a106' x2='271.980551219896' y2='385.213212375483' elementType='O' />
      <atom id = 'a107' x2='321.155796971404' y2='470.575628545346' elementType='C' />
      <atom id = 'a108' x2='337.54754555524' y2='480.046100142437' elementType='C' />
      <atom id = 'a109' x2='354.864979332777' y2='472.355792379386' elementType='N' />
      <atom id = 'a110' x2='339.527087738692' y2='498.887354161911' elementType='C' />
      <atom id = 'a111' x2='367.525504520466' y2='486.426207323634' elementType='C' />
      <atom id = 'a112' x2='358.055032923376' y2='502.832197218143' elementType='N' />
      <atom id = 'a113' x2='386.737032617421' y2='451.53499617646' elementType='C' />
      <atom id = 'a114' x2='403.143022511929' y2='442.064524579369' elementType='C' />
      <atom id = 'a115' x2='419.549012406437' y2='451.53499617646' elementType='C' />
      <atom id = 'a116' x2='403.143022511929' y2='423.123581385189' elementType='C' />
      <atom id = 'a117' x2='370.231353548206' y2='385.19897106481' elementType='C' />
      <atom id = 'a118' x2='370.231353548206' y2='366.25802787063' elementType='S' />
      <atom id = 'a119' x2='485.002076256403' y2='413.610385856081' elementType='C' />
      <atom id = 'a120' x2='501.393824840239' y2='423.080857453172' elementType='O' />
      <atom id = 'a121' x2='534.191563318583' y2='366.144097385251' elementType='C' />
      <atom id = 'a122' x2='550.597553213092' y2='356.673625788161' elementType='C' />
      <atom id = 'a123' x2='552.577095396544' y2='337.832371768687' elementType='C' />
      <atom id = 'a124' x2='567.900745679956' y2='364.378174861884' elementType='N' />
      <atom id = 'a125' x2='571.090799270555' y2='333.901770023127' elementType='N' />
      <atom id = 'a126' x2='580.575512178317' y2='350.293518606963' elementType='C' />
      <atom id = 'a127' x2='583.309843827402' y2='413.496455370703' elementType='C' />
      <atom id = 'a128' x2='599.687351100566' y2='423.023892210482' elementType='C' />
      <atom id = 'a129' x2='599.630385857876' y2='441.964835404663' elementType='C' />
      <atom id = 'a130' x2='616.121823616419' y2='413.596144545409' elementType='C' />
      <atom id = 'a131' x2='632.556296132272' y2='366.144097385251' elementType='C' />
      <atom id = 'a132' x2='648.96228602678' y2='356.673625788161' elementType='C' />
      <atom id = 'a133' x2='616.150306237763' y2='356.673625788161' elementType='C' />
      <atom id = 'a134' x2='681.745783194452' y2='413.610385856081' elementType='C' />
      <atom id = 'a135' x2='698.137531778288' y2='423.080857453172' elementType='C' />
      <atom id = 'a136' x2='698.137531778288' y2='442.021800647352' elementType='C' />
      <atom id = 'a137' x2='714.543521672796' y2='451.492272244443' elementType='O' />
      <atom id = 'a138' x2='681.73154188378' y2='451.492272244443' elementType='O' />
      <atom id = 'a139' x2='730.92102894596' y2='366.144097385251' elementType='C' />
      <atom id = 'a140' x2='780.110516008141' y2='413.610385856081' elementType='C' />
      <atom id = 'a141' x2='796.45954065996' y2='423.080857453172' elementType='C' />
      <atom id = 'a142' x2='796.431058038615' y2='442.021800647352' elementType='C' />
      <atom id = 'a143' x2='812.87977186514' y2='413.638868477426' elementType='C' />
      <atom id = 'a144' x2='829.300003070321' y2='366.144097385251' elementType='C' />
      <atom id = 'a145' x2='845.691751654157' y2='356.673625788161' elementType='C' />
      <atom id = 'a146' x2='845.691751654157' y2='337.73268259398' elementType='C' />
      <atom id = 'a147' x2='862.097741548665' y2='366.144097385251' elementType='C' />
      <atom id = 'a148' x2='862.097741548665' y2='328.26221099689' elementType='C' />
      <atom id = 'a149' x2='878.503731443174' y2='356.673625788161' elementType='C' />
      <atom id = 'a150' x2='878.503731443174' y2='337.73268259398' elementType='C' />
      <atom id = 'a151' x2='895.037893133733' y2='328.376141482269' elementType='O' />
      <atom id = 'a152' x2='878.475248821829' y2='413.610385856081' elementType='C' />
      <atom id = 'a153' x2='894.810032162976' y2='423.080857453172' elementType='C' />
      <atom id = 'a154' x2='894.781549541631' y2='442.021800647352' elementType='C' />
      <atom id = 'a155' x2='911.230263368156' y2='413.638868477426' elementType='C' />
      <atom id = 'a156' x2='927.664735884009' y2='366.144097385251' elementType='C' />
      <atom id = 'a157' x2='944.070725778518' y2='356.673625788161' elementType='C' />
      <atom id = 'a158' x2='911.258745989501' y2='356.673625788161' elementType='C' />
      <atom id = 'a159' x2='976.953912120896' y2='413.411007506669' elementType='C' />
      <atom id = 'a160' x2='976.996636052913' y2='394.54127086585' elementType='S' />
      <atom id = 'a161' x2='894.966686580371' y2='498.602527948465' elementType='C' />
      <atom id = 'a162' x2='878.60342061788' y2='507.987551681521' elementType='C' />
      <atom id = 'a163' x2='862.225913344716' y2='498.488597463086' elementType='C' />
      <atom id = 'a164' x2='845.805682139536' y2='507.91634512816' elementType='O' />
      <atom id = 'a165' x2='862.268637276733' y2='479.547654268906' elementType='O' />
      <atom id = 'a166' x2='911.486606960258' y2='564.924311749442' elementType='C' />
      <atom id = 'a167' x2='911.486606960258' y2='583.865254943622' elementType='C' />
      <atom id = 'a168' x2='927.878355544094' y2='593.335726540713' elementType='C' />
      <atom id = 'a169' x2='927.878355544094' y2='612.276669734893' elementType='N' />
      <atom id = 'a170' x2='944.284345438602' y2='621.747141331984' elementType='C' />
      <atom id = 'a171' x2='944.284345438602' y2='640.688084526164' elementType='N' />
      <atom id = 'a172' x2='960.690335333111' y2='612.276669734893' elementType='N' />
      <atom id = 'a173' x2='862.268637276733' y2='555.453840152351' elementType='N' />
      <atom id = 'a174' x2='845.862647382225' y2='564.924311749442' elementType='C' />
      <atom id = 'a175' x2='829.470898798389' y2='555.453840152351' elementType='C' />
      <atom id = 'a176' x2='813.06490890388' y2='564.924311749442' elementType='N' />
      <atom id = 'a177' x2='829.470898798389' y2='536.512896958171' elementType='O' />
      <atom id = 'a178' x2='780.252929114864' y2='564.924311749442' elementType='C' />
      <atom id = 'a179' x2='780.252929114864' y2='583.865254943622' elementType='O' />
      <atom id = 'a180' x2='796.658919009372' y2='555.453840152351' elementType='C' />
      <atom id = 'a181' x2='763.8754218417' y2='555.56777063773' elementType='N' />
      <atom id = 'a182' x2='731.077683363356' y2='555.56777063773' elementType='C' />
      <atom id = 'a183' x2='731.077683363356' y2='536.626827443549' elementType='O' />
      <atom id = 'a184' x2='747.483673257864' y2='565.03824223482' elementType='C' />
      <atom id = 'a185' x2='714.700176090192' y2='564.924311749442' elementType='N' />
      <atom id = 'a186' x2='681.902437611848' y2='564.924311749442' elementType='C' />
      <atom id = 'a187' x2='681.902437611848' y2='583.865254943622' elementType='O' />
      <atom id = 'a188' x2='698.294186195684' y2='555.453840152351' elementType='C' />
      <atom id = 'a189' x2='665.524930338684' y2='555.56777063773' elementType='N' />
      <atom id = 'a190' x2='632.712950549667' y2='555.56777063773' elementType='C' />
      <atom id = 'a191' x2='632.712950549667' y2='536.626827443549' elementType='O' />
      <atom id = 'a192' x2='649.118940444175' y2='565.03824223482' elementType='C' />
      <atom id = 'a193' x2='616.335443276504' y2='564.924311749442' elementType='N' />
      <atom id = 'a194' x2='796.658919009372' y2='536.512896958171' elementType='C' />
      <atom id = 'a195' x2='747.483673257864' y2='583.979185429001' elementType='C' />
      <atom id = 'a196' x2='780.252929114864' y2='527.042425361081' elementType='C' />
      <atom id = 'a197' x2='763.846939220355' y2='536.512896958171' elementType='C' />
      <atom id = 'a198' x2='780.252929114864' y2='508.1014821669' elementType='C' />
      <atom id = 'a199' x2='747.455190636519' y2='527.042425361081' elementType='C' />
      <atom id = 'a200' x2='763.846939220355' y2='498.63101056981' elementType='C' />
      <atom id = 'a201' x2='747.455190636519' y2='508.1014821669' elementType='C' />
      <atom id = 'a202' x2='731.077683363356' y2='593.449657026091' elementType='C' />
      <atom id = 'a203' x2='731.077683363356' y2='612.390600220272' elementType='C' />
      <atom id = 'a204' x2='714.671693468847' y2='583.979185429001' elementType='C' />
      <atom id = 'a205' x2='714.671693468847' y2='621.861071817362' elementType='C' />
      <atom id = 'a206' x2='698.265703574339' y2='593.449657026091' elementType='C' />
      <atom id = 'a207' x2='698.265703574339' y2='612.390600220272' elementType='C' />
      <atom id = 'a208' x2='698.294186195684' y2='536.512896958171' elementType='C' />
      <atom id = 'a209' x2='681.888196301175' y2='527.042425361081' elementType='C' />
      <atom id = 'a210' x2='665.482206406667' y2='536.512896958171' elementType='C' />
      <atom id = 'a211' x2='681.888196301175' y2='508.1014821669' elementType='C' />
      <atom id = 'a212' x2='649.090457822831' y2='527.042425361081' elementType='C' />
      <atom id = 'a213' x2='665.496447717339' y2='498.63101056981' elementType='C' />
      <atom id = 'a214' x2='649.090457822831' y2='508.1014821669' elementType='C' />
      <atom id = 'a215' x2='632.698709238995' y2='498.744941055188' elementType='O' />
      <atom id = 'a216' x2='649.118940444175' y2='583.979185429001' elementType='C' />
      <atom id = 'a217' x2='632.712950549667' y2='593.449657026091' elementType='O' />
      <atom id = 'a218' x2='665.524930338684' y2='593.449657026091' elementType='C' />
      <atom id = 'a219' x2='591.541321396001' y2='582.939569749922' elementType='C' />
      <atom id = 'a220' x2='585.688142709679' y2='564.924311749442' elementType='C' />
      <atom id = 'a221' x2='610.482264590181' y2='582.939569749922' elementType='C' />
      <atom id = 'a222' x2='601.011792993091' y2='553.78760680369' elementType='C' />
      <atom id = 'a223' x2='601.011792993091' y2='534.84666360951' elementType='C' />
      <atom id = 'a224' x2='617.4177828876' y2='525.376192012419' elementType='O' />
      <atom id = 'a225' x2='584.677009651944' y2='525.418915944436' elementType='N' />
      <atom id = 'a226' x2='551.865029862928' y2='525.262261527041' elementType='C' />
      <atom id = 'a227' x2='551.950477726962' y2='506.32131833286' elementType='O' />
      <atom id = 'a228' x2='568.228295825419' y2='534.818180988165' elementType='C' />
      <atom id = 'a229' x2='535.444798657747' y2='534.547596085391' elementType='N' />
      <atom id = 'a230' x2='502.647060179403' y2='534.405182978668' elementType='C' />
      <atom id = 'a231' x2='502.561612315369' y2='553.346126172849' elementType='O' />
      <atom id = 'a232' x2='519.095774005928' y2='525.005917934939' elementType='C' />
      <atom id = 'a233' x2='486.312276838256' y2='524.963194002922' elementType='O' />
      <atom id = 'a234' x2='568.142847961385' y2='553.759124182346' elementType='C' />
      <atom id = 'a235' x2='551.69413413486' y2='563.144147915402' elementType='C' />
      <atom id = 'a236' x2='551.608686270826' y2='582.085091109583' elementType='C' />
      <atom id = 'a237' x2='535.159972444301' y2='591.484356153312' elementType='C' />
      <atom id = 'a238' x2='535.074524580267' y2='610.425299347492' elementType='N' />
      <atom id = 'a239' x2='519.095774005928' y2='506.064974740759' elementType='C' />
      <atom id = 'a240' x2='502.68978411142' y2='496.594503143668' elementType='O' />
      <atom id = 'a241' x2='535.501763900436' y2='496.594503143668' elementType='C' />
      <atom id = 'a242' x2='977.609012411823' y2='358.781339767664' elementType='S' />
      <atom id = 'a243' x2='157.665550453213' y2='188.441022816089' elementType='C' />
      <atom id = 'a244' x2='157.665550453213' y2='169.500079621908' elementType='O' />
      <atom id = 'a245' x2='141.259560558705' y2='197.911494413179' elementType='C' />
      <atom id = 'a246' x2='124.867811974869' y2='188.441022816089' elementType='N' />
      <atom id = 'a247' x2='174.043057726377' y2='197.797563927801' elementType='N' />
      <atom id = 'a248' x2='206.855037515394' y2='197.797563927801' elementType='C' />
      <atom id = 'a249' x2='206.855037515394' y2='216.738507121981' elementType='O' />
      <atom id = 'a250' x2='190.449047620885' y2='188.32709233071' elementType='C' />
      <atom id = 'a251' x2='223.232544788558' y2='188.441022816089' elementType='N' />
      <atom id = 'a252' x2='256.030283266902' y2='188.441022816089' elementType='C' />
      <atom id = 'a253' x2='256.030283266902' y2='169.500079621908' elementType='O' />
      <atom id = 'a254' x2='239.638534683066' y2='197.911494413179' elementType='C' />
      <atom id = 'a255' x2='272.407790540066' y2='197.797563927801' elementType='N' />
      <atom id = 'a256' x2='305.219770329082' y2='197.797563927801' elementType='C' />
      <atom id = 'a257' x2='305.219770329082' y2='216.738507121981' elementType='O' />
      <atom id = 'a258' x2='288.813780434574' y2='188.32709233071' elementType='C' />
      <atom id = 'a259' x2='321.597277602246' y2='188.441022816089' elementType='N' />
      <atom id = 'a260' x2='354.409257391263' y2='188.441022816089' elementType='C' />
      <atom id = 'a261' x2='354.409257391263' y2='169.500079621908' elementType='O' />
      <atom id = 'a262' x2='338.003267496754' y2='197.911494413179' elementType='C' />
      <atom id = 'a263' x2='370.786764664426' y2='197.797563927801' elementType='N' />
      <atom id = 'a264' x2='403.584503142771' y2='197.797563927801' elementType='C' />
      <atom id = 'a265' x2='403.584503142771' y2='216.738507121981' elementType='O' />
      <atom id = 'a266' x2='387.178513248262' y2='188.32709233071' elementType='C' />
      <atom id = 'a267' x2='419.962010415934' y2='188.441022816089' elementType='N' />
      <atom id = 'a268' x2='452.773990204951' y2='188.441022816089' elementType='C' />
      <atom id = 'a269' x2='452.773990204951' y2='169.500079621908' elementType='O' />
      <atom id = 'a270' x2='436.368000310443' y2='197.911494413179' elementType='C' />
      <atom id = 'a271' x2='469.151497478115' y2='197.797563927801' elementType='N' />
      <atom id = 'a272' x2='501.963477267132' y2='197.797563927801' elementType='C' />
      <atom id = 'a273' x2='501.963477267132' y2='216.738507121981' elementType='O' />
      <atom id = 'a274' x2='485.557487372623' y2='188.32709233071' elementType='C' />
      <atom id = 'a275' x2='518.340984540295' y2='188.441022816089' elementType='N' />
      <atom id = 'a276' x2='551.13872301864' y2='188.441022816089' elementType='C' />
      <atom id = 'a277' x2='551.13872301864' y2='169.500079621908' elementType='O' />
      <atom id = 'a278' x2='534.732733124131' y2='197.911494413179' elementType='C' />
      <atom id = 'a279' x2='567.516230291803' y2='197.797563927801' elementType='N' />
      <atom id = 'a280' x2='600.32821008082' y2='197.797563927801' elementType='C' />
      <atom id = 'a281' x2='600.32821008082' y2='216.738507121981' elementType='O' />
      <atom id = 'a282' x2='583.922220186312' y2='188.32709233071' elementType='C' />
      <atom id = 'a283' x2='616.734199975328' y2='188.32709233071' elementType='N' />
      <atom id = 'a284' x2='633.125948559164' y2='216.738507121981' elementType='C' />
      <atom id = 'a285' x2='616.734199975328' y2='226.208978719072' elementType='O' />
      <atom id = 'a286' x2='633.125948559164' y2='197.797563927801' elementType='C' />
      <atom id = 'a287' x2='649.531938453673' y2='226.208978719072' elementType='N' />
      <atom id = 'a288' x2='665.937928348181' y2='254.620393510343' elementType='C' />
      <atom id = 'a289' x2='665.937928348181' y2='273.561336704523' elementType='O' />
      <atom id = 'a290' x2='649.531938453673' y2='245.149921913252' elementType='C' />
      <atom id = 'a291' x2='682.343918242689' y2='245.149921913252' elementType='N' />
      <atom id = 'a292' x2='715.155898031706' y2='245.149921913252' elementType='C' />
      <atom id = 'a293' x2='715.155898031706' y2='226.208978719072' elementType='O' />
      <atom id = 'a294' x2='698.749908137198' y2='254.620393510343' elementType='C' />
      <atom id = 'a295' x2='731.547646615542' y2='254.60615219967' elementType='N' />
      <atom id = 'a296' x2='764.359626404559' y2='254.60615219967' elementType='C' />
      <atom id = 'a297' x2='764.359626404559' y2='273.547095393851' elementType='O' />
      <atom id = 'a298' x2='747.953636510051' y2='245.13568060258' elementType='C' />
      <atom id = 'a299' x2='780.765616299067' y2='245.13568060258' elementType='N' />
      <atom id = 'a300' x2='813.563354777412' y2='245.13568060258' elementType='C' />
      <atom id = 'a301' x2='813.563354777412' y2='226.194737408399' elementType='O' />
      <atom id = 'a302' x2='797.171606193575' y2='254.60615219967' elementType='C' />
      <atom id = 'a303' x2='829.96934467192' y2='254.60615219967' elementType='N' />
      <atom id = 'a304' x2='862.781324460937' y2='254.60615219967' elementType='C' />
      <atom id = 'a305' x2='862.781324460937' y2='273.547095393851' elementType='O' />
      <atom id = 'a306' x2='846.375334566428' y2='245.13568060258' elementType='C' />
      <atom id = 'a307' x2='879.187314355445' y2='245.13568060258' elementType='N' />
      <atom id = 'a308' x2='911.985052833789' y2='245.13568060258' elementType='C' />
      <atom id = 'a309' x2='911.985052833789' y2='226.194737408399' elementType='O' />
      <atom id = 'a310' x2='895.593304249953' y2='254.60615219967' elementType='C' />
      <atom id = 'a311' x2='928.391042728298' y2='254.60615219967' elementType='N' />
      <atom id = 'a312' x2='961.203022517314' y2='254.60615219967' elementType='C' />
      <atom id = 'a313' x2='977.609012411823' y2='245.13568060258' elementType='O' />
      <atom id = 'a314' x2='944.797032622806' y2='245.13568060258' elementType='C' />
      <atom id = 'a315' x2='961.203022517314' y2='273.547095393851' elementType='N' />
      <atom id = 'a316' x2='977.609012411823' y2='301.958510185122' elementType='C' />
      <atom id = 'a317' x2='961.203022517314' y2='311.428981782212' elementType='O' />
      <atom id = 'a318' x2='977.609012411823' y2='283.017566990941' elementType='C' />
      <atom id = 'a319' x2='994.015002306331' y2='311.428981782212' elementType='N' />
      <atom id = 'a320' x2='1010.40675089017' y2='339.840396573483' elementType='C' />
      <atom id = 'a321' x2='1026.81274078468' y2='330.369924976393' elementType='O' />
      <atom id = 'a322' x2='994.015002306331' y2='330.369924976393' elementType='C' />
      <atom id = 'a323' x2='1010.42099220084' y2='358.781339767664' elementType='N' />
      <atom id = 'a324' x2='1043.21873067918' y2='358.781339767664' elementType='C' />
      <atom id = 'a325' x2='1043.21873067918' y2='339.840396573483' elementType='O' />
      <atom id = 'a326' x2='1026.81274078468' y2='368.251811364754' elementType='C' />
      <atom id = 'a327' x2='1059.62472057369' y2='368.251811364754' elementType='O' />
      <atom id = 'a328' x2='190.449047620885' y2='169.38614913653' elementType='C' />
      <atom id = 'a329' x2='174.043057726377' y2='159.915677539439' elementType='C' />
      <atom id = 'a330' x2='206.855037515394' y2='159.915677539439' elementType='C' />
      <atom id = 'a331' x2='206.855037515394' y2='140.974734345259' elementType='C' />
      <atom id = 'a332' x2='239.624293372394' y2='216.85243760736' elementType='C' />
      <atom id = 'a333' x2='223.232544788558' y2='226.32290920445' elementType='C' />
      <atom id = 'a334' x2='256.030283266902' y2='226.32290920445' elementType='C' />
      <atom id = 'a335' x2='288.813780434574' y2='169.38614913653' elementType='C' />
      <atom id = 'a336' x2='305.219770329082' y2='159.915677539439' elementType='C' />
      <atom id = 'a337' x2='305.219770329082' y2='140.974734345259' elementType='C' />
      <atom id = 'a338' x2='288.813780434574' y2='131.504262748168' elementType='O' />
      <atom id = 'a339' x2='321.625760223591' y2='131.504262748168' elementType='O' />
      <atom id = 'a340' x2='338.003267496754' y2='216.85243760736' elementType='C' />
      <atom id = 'a341' x2='354.409257391263' y2='226.32290920445' elementType='C' />
      <atom id = 'a342' x2='354.39501608059' y2='245.263852398631' elementType='C' />
      <atom id = 'a343' x2='370.801005975099' y2='254.734323995721' elementType='N' />
      <atom id = 'a344' x2='338.003267496754' y2='254.734323995721' elementType='O' />
      <atom id = 'a345' x2='387.178513248262' y2='169.38614913653' elementType='C' />
      <atom id = 'a346' x2='436.368000310443' y2='216.85243760736' elementType='C' />
      <atom id = 'a347' x2='485.557487372623' y2='169.38614913653' elementType='C' />
      <atom id = 'a348' x2='501.949235956459' y2='159.915677539439' elementType='O' />
      <atom id = 'a349' x2='469.151497478115' y2='159.915677539439' elementType='C' />
      <atom id = 'a350' x2='534.732733124131' y2='216.85243760736' elementType='C' />
      <atom id = 'a351' x2='518.326743229623' y2='226.32290920445' elementType='O' />
      <atom id = 'a352' x2='583.922220186312' y2='169.38614913653' elementType='C' />
      <atom id = 'a353' x2='600.32821008082' y2='159.915677539439' elementType='C' />
      <atom id = 'a354' x2='567.516230291803' y2='159.915677539439' elementType='C' />
      <atom id = 'a355' x2='567.516230291803' y2='140.974734345259' elementType='C' />
      <atom id = 'a356' x2='649.531938453673' y2='188.32709233071' elementType='C' />
      <atom id = 'a357' x2='633.140189869837' y2='254.620393510343' elementType='C' />
      <atom id = 'a358' x2='633.140189869837' y2='273.561336704523' elementType='O' />
      <atom id = 'a359' x2='698.749908137198' y2='273.561336704523' elementType='C' />
      <atom id = 'a360' x2='715.155898031706' y2='283.031808301614' elementType='C' />
      <atom id = 'a361' x2='715.155898031706' y2='301.972751495794' elementType='C' />
      <atom id = 'a362' x2='731.547646615542' y2='273.547095393851' elementType='C' />
      <atom id = 'a363' x2='747.953636510051' y2='226.194737408399' elementType='C' />
      <atom id = 'a364' x2='764.359626404559' y2='216.724265811309' elementType='C' />
      <atom id = 'a365' x2='764.359626404559' y2='197.783322617128' elementType='C' />
      <atom id = 'a366' x2='780.765616299067' y2='226.194737408399' elementType='C' />
      <atom id = 'a367' x2='780.765616299067' y2='188.312851020038' elementType='C' />
      <atom id = 'a368' x2='797.171606193575' y2='216.724265811309' elementType='C' />
      <atom id = 'a369' x2='797.171606193575' y2='197.783322617128' elementType='C' />
      <atom id = 'a370' x2='813.563354777412' y2='188.312851020038' elementType='O' />
      <atom id = 'a371' x2='797.171606193575' y2='273.547095393851' elementType='C' />
      <atom id = 'a372' x2='813.577596088084' y2='283.017566990941' elementType='C' />
      <atom id = 'a373' x2='813.577596088084' y2='301.958510185122' elementType='C' />
      <atom id = 'a374' x2='829.96934467192' y2='311.428981782212' elementType='N' />
      <atom id = 'a375' x2='797.171606193575' y2='311.428981782212' elementType='O' />
      <atom id = 'a376' x2='846.375334566428' y2='226.194737408399' elementType='C' />
      <atom id = 'a377' x2='862.781324460937' y2='216.724265811309' elementType='C' />
      <atom id = 'a378' x2='862.781324460937' y2='197.783322617128' elementType='C' />
      <atom id = 'a379' x2='879.187314355445' y2='226.194737408399' elementType='C' />
      <atom id = 'a380' x2='895.593304249953' y2='273.547095393851' elementType='C' />
      <atom id = 'a381' x2='911.985052833789' y2='283.017566990941' elementType='C' />
      <atom id = 'a382' x2='911.985052833789' y2='301.958510185122' elementType='C' />
      <atom id = 'a383' x2='928.391042728298' y2='311.428981782212' elementType='O' />
      <atom id = 'a384' x2='895.593304249953' y2='311.428981782212' elementType='O' />
      <atom id = 'a385' x2='944.797032622806' y2='226.194737408399' elementType='C' />
      <atom id = 'a386' x2='961.203022517314' y2='216.724265811309' elementType='C' />
      <atom id = 'a387' x2='961.203022517314' y2='197.783322617128' elementType='O' />
      <atom id = 'a388' x2='977.609012411823' y2='226.194737408399' elementType='N' />
      <atom id = 'a389' x2='994.000760995659' y2='273.547095393851' elementType='C' />
      <atom id = 'a390' x2='1010.40675089017' y2='283.017566990941' elementType='C' />
      <atom id = 'a391' x2='1010.40675089017' y2='301.958510185122' elementType='C' />
      <atom id = 'a392' x2='1026.81274078468' y2='273.547095393851' elementType='C' />
      <atom id = 'a393' x2='1026.81274078468' y2='311.428981782212' elementType='C' />
      <atom id = 'a394' x2='1043.21873067918' y2='283.017566990941' elementType='C' />
      <atom id = 'a395' x2='1043.21873067918' y2='301.958510185122' elementType='C' />
      <atom id = 'a396' x2='1059.62472057369' y2='311.428981782212' elementType='O' />
      <atom id = 'a397' x2='977.609012411823' y2='339.840396573483' elementType='C' />
      <atom id = 'a398' x2='1026.81274078468' y2='387.192754558935' elementType='C' />
      <atom id = 'a399' x2='1043.21873067918' y2='396.663226156025' elementType='C' />
      <atom id = 'a400' x2='1043.21873067918' y2='415.604169350206' elementType='O' />
      <atom id = 'a401' x2='1059.62472057369' y2='387.192754558935' elementType='N' />
      <atom id = 'a402' x2='649.531938453673' y2='112.335458583231' elementType='S' />
      <atom id = 'a403' x2='485.400832955228' y2='112.677250039366' elementType='S' />
      <atom id = 'a404' x2='419.890803862573' y2='226.365633136467' elementType='S' />
    </atomArray>
    <bondArray>
      <bond id = 'b1' atomRefs2='a2 a1' order='D' />
      <bond id = 'b2' atomRefs2='a3 a1' order='S' />
      <bond id = 'b3' atomRefs2='a3 a4' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b4' atomRefs2='a1 a5' order='S' />
      <bond id = 'b5' atomRefs2='a7 a6' order='D' />
      <bond id = 'b6' atomRefs2='a8 a6' order='S' />
      <bond id = 'b7' atomRefs2='a8 a5' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b8' atomRefs2='a6 a9' order='S' />
      <bond id = 'b9' atomRefs2='a11 a10' order='D' />
      <bond id = 'b10' atomRefs2='a12 a10' order='S' />
      <bond id = 'b11' atomRefs2='a12 a9' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b12' atomRefs2='a10 a13' order='S' />
      <bond id = 'b13' atomRefs2='a15 a14' order='D' />
      <bond id = 'b14' atomRefs2='a16 a14' order='S' />
      <bond id = 'b15' atomRefs2='a16 a13' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b16' atomRefs2='a14 a17' order='S' />
      <bond id = 'b17' atomRefs2='a19 a18' order='D' />
      <bond id = 'b18' atomRefs2='a20 a18' order='S' />
      <bond id = 'b19' atomRefs2='a20 a17' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b20' atomRefs2='a18 a21' order='S' />
      <bond id = 'b21' atomRefs2='a23 a22' order='D' />
      <bond id = 'b22' atomRefs2='a24 a22' order='S' />
      <bond id = 'b23' atomRefs2='a24 a21' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b24' atomRefs2='a22 a25' order='S' />
      <bond id = 'b25' atomRefs2='a27 a26' order='D' />
      <bond id = 'b26' atomRefs2='a28 a26' order='S' />
      <bond id = 'b27' atomRefs2='a28 a25' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b28' atomRefs2='a26 a29' order='S' />
      <bond id = 'b29' atomRefs2='a31 a30' order='D' />
      <bond id = 'b30' atomRefs2='a32 a30' order='S' />
      <bond id = 'b31' atomRefs2='a32 a29' order='S' />
      <bond id = 'b32' atomRefs2='a30 a33' order='S' />
      <bond id = 'b33' atomRefs2='a35 a34' order='D' />
      <bond id = 'b34' atomRefs2='a36 a34' order='S' />
      <bond id = 'b35' atomRefs2='a36 a33' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b36' atomRefs2='a34 a37' order='S' />
      <bond id = 'b37' atomRefs2='a39 a38' order='D' />
      <bond id = 'b38' atomRefs2='a40 a38' order='S' />
      <bond id = 'b39' atomRefs2='a40 a37' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b40' atomRefs2='a38 a41' order='S' />
      <bond id = 'b41' atomRefs2='a43 a42' order='D' />
      <bond id = 'b42' atomRefs2='a44 a42' order='S' />
      <bond id = 'b43' atomRefs2='a44 a41' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b44' atomRefs2='a42 a45' order='S' />
      <bond id = 'b45' atomRefs2='a47 a46' order='D' />
      <bond id = 'b46' atomRefs2='a48 a46' order='S' />
      <bond id = 'b47' atomRefs2='a48 a45' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b48' atomRefs2='a46 a49' order='S' />
      <bond id = 'b49' atomRefs2='a51 a50' order='D' />
      <bond id = 'b50' atomRefs2='a52 a50' order='S' />
      <bond id = 'b51' atomRefs2='a52 a49' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b52' atomRefs2='a50 a53' order='S' />
      <bond id = 'b53' atomRefs2='a55 a54' order='D' />
      <bond id = 'b54' atomRefs2='a56 a54' order='S' />
      <bond id = 'b55' atomRefs2='a56 a53' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b56' atomRefs2='a54 a57' order='S' />
      <bond id = 'b57' atomRefs2='a59 a58' order='D' />
      <bond id = 'b58' atomRefs2='a60 a58' order='S' />
      <bond id = 'b59' atomRefs2='a60 a57' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b60' atomRefs2='a58 a61' order='S' />
      <bond id = 'b61' atomRefs2='a63 a62' order='D' />
      <bond id = 'b62' atomRefs2='a64 a62' order='S' />
      <bond id = 'b63' atomRefs2='a64 a61' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b64' atomRefs2='a62 a65' order='S' />
      <bond id = 'b65' atomRefs2='a67 a66' order='D' />
      <bond id = 'b66' atomRefs2='a68 a66' order='S' />
      <bond id = 'b67' atomRefs2='a68 a65' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b68' atomRefs2='a66 a69' order='S' />
      <bond id = 'b69' atomRefs2='a71 a70' order='D' />
      <bond id = 'b70' atomRefs2='a72 a70' order='S' />
      <bond id = 'b71' atomRefs2='a72 a69' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b72' atomRefs2='a70 a73' order='S' />
      <bond id = 'b73' atomRefs2='a75 a74' order='D' />
      <bond id = 'b74' atomRefs2='a76 a74' order='S' />
      <bond id = 'b75' atomRefs2='a76 a73' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b76' atomRefs2='a74 a77' order='S' />
      <bond id = 'b77' atomRefs2='a79 a78' order='D' />
      <bond id = 'b78' atomRefs2='a80 a78' order='S' />
      <bond id = 'b79' atomRefs2='a80 a77' order='S' />
      <bond id = 'b80' atomRefs2='a78 a81' order='S' />
      <bond id = 'b81' atomRefs2='a83 a82' order='D' />
      <bond id = 'b82' atomRefs2='a84 a82' order='S' />
      <bond id = 'b83' atomRefs2='a84 a81' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b84' atomRefs2='a82 a85' order='S' />
      <bond id = 'b85' atomRefs2='a87 a86' order='D' />
      <bond id = 'b86' atomRefs2='a88 a86' order='S' />
      <bond id = 'b87' atomRefs2='a88 a85' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b88' atomRefs2='a3 a89' order='S' />
      <bond id = 'b89' atomRefs2='a89 a90' order='S' />
      <bond id = 'b90' atomRefs2='a91 a90' order='S' />
      <bond id = 'b91' atomRefs2='a92 a90' order='D' />
      <bond id = 'b92' atomRefs2='a93 a91' order='D' />
      <bond id = 'b93' atomRefs2='a94 a92' order='S' />
      <bond id = 'b94' atomRefs2='a95 a93' order='S' />
      <bond id = 'b95' atomRefs2='a95 a94' order='D' />
      <bond id = 'b96' atomRefs2='a8 a96' order='S' />
      <bond id = 'b97' atomRefs2='a96 a97' order='S' />
      <bond id = 'b98' atomRefs2='a96 a98' order='S' />
      <bond id = 'b99' atomRefs2='a12 a99' order='S' />
      <bond id = 'b100' atomRefs2='a99 a100' order='D' />
      <bond id = 'b101' atomRefs2='a99 a101' order='S' />
      <bond id = 'b102' atomRefs2='a16 a102' order='S' />
      <bond id = 'b103' atomRefs2='a102 a103' order='S' />
      <bond id = 'b104' atomRefs2='a103 a104' order='S' />
      <bond id = 'b105' atomRefs2='a104 a105' order='S' />
      <bond id = 'b106' atomRefs2='a104 a106' order='D' />
      <bond id = 'b107' atomRefs2='a20 a107' order='S' />
      <bond id = 'b108' atomRefs2='a109 a108' order='S' />
      <bond id = 'b109' atomRefs2='a110 a108' order='D' />
      <bond id = 'b110' atomRefs2='a111 a109' order='D' />
      <bond id = 'b111' atomRefs2='a112 a110' order='S' />
      <bond id = 'b112' atomRefs2='a112 a111' order='S' />
      <bond id = 'b113' atomRefs2='a107 a108' order='S' />
      <bond id = 'b114' atomRefs2='a24 a113' order='S' />
      <bond id = 'b115' atomRefs2='a113 a114' order='S' />
      <bond id = 'b116' atomRefs2='a114 a115' order='S' />
      <bond id = 'b117' atomRefs2='a114 a116' order='S' />
      <bond id = 'b118' atomRefs2='a28 a117' order='S' />
      <bond id = 'b119' atomRefs2='a117 a118' order='S' />
      <bond id = 'b120' atomRefs2='a36 a119' order='S' />
      <bond id = 'b121' atomRefs2='a119 a120' order='S' />
      <bond id = 'b122' atomRefs2='a40 a121' order='S' />
      <bond id = 'b123' atomRefs2='a123 a122' order='D' />
      <bond id = 'b124' atomRefs2='a124 a122' order='S' />
      <bond id = 'b125' atomRefs2='a125 a123' order='S' />
      <bond id = 'b126' atomRefs2='a126 a124' order='D' />
      <bond id = 'b127' atomRefs2='a126 a125' order='S' />
      <bond id = 'b128' atomRefs2='a121 a122' order='S' />
      <bond id = 'b129' atomRefs2='a44 a127' order='S' />
      <bond id = 'b130' atomRefs2='a127 a128' order='S' />
      <bond id = 'b131' atomRefs2='a128 a129' order='S' />
      <bond id = 'b132' atomRefs2='a128 a130' order='S' />
      <bond id = 'b133' atomRefs2='a48 a131' order='S' />
      <bond id = 'b134' atomRefs2='a131 a132' order='S' />
      <bond id = 'b135' atomRefs2='a131 a133' order='S' />
      <bond id = 'b136' atomRefs2='a52 a134' order='S' />
      <bond id = 'b137' atomRefs2='a134 a135' order='S' />
      <bond id = 'b138' atomRefs2='a135 a136' order='S' />
      <bond id = 'b139' atomRefs2='a136 a137' order='S' />
      <bond id = 'b140' atomRefs2='a136 a138' order='D' />
      <bond id = 'b141' atomRefs2='a56 a139' order='S' />
      <bond id = 'b142' atomRefs2='a60 a140' order='S' />
      <bond id = 'b143' atomRefs2='a140 a141' order='S' />
      <bond id = 'b144' atomRefs2='a141 a142' order='S' />
      <bond id = 'b145' atomRefs2='a141 a143' order='S' />
      <bond id = 'b146' atomRefs2='a64 a144' order='S' />
      <bond id = 'b147' atomRefs2='a146 a145' order='S' />
      <bond id = 'b148' atomRefs2='a147 a145' order='D' />
      <bond id = 'b149' atomRefs2='a148 a146' order='D' />
      <bond id = 'b150' atomRefs2='a149 a147' order='S' />
      <bond id = 'b151' atomRefs2='a150 a148' order='S' />
      <bond id = 'b152' atomRefs2='a150 a149' order='D' />
      <bond id = 'b153' atomRefs2='a144 a145' order='S' />
      <bond id = 'b154' atomRefs2='a150 a151' order='S' />
      <bond id = 'b155' atomRefs2='a68 a152' order='S' />
      <bond id = 'b156' atomRefs2='a152 a153' order='S' />
      <bond id = 'b157' atomRefs2='a153 a154' order='S' />
      <bond id = 'b158' atomRefs2='a153 a155' order='S' />
      <bond id = 'b159' atomRefs2='a72 a156' order='S' />
      <bond id = 'b160' atomRefs2='a156 a157' order='S' />
      <bond id = 'b161' atomRefs2='a156 a158' order='S' />
      <bond id = 'b162' atomRefs2='a76 a159' order='S' />
      <bond id = 'b163' atomRefs2='a159 a160' order='S' />
      <bond id = 'b164' atomRefs2='a84 a161' order='S' />
      <bond id = 'b165' atomRefs2='a161 a162' order='S' />
      <bond id = 'b166' atomRefs2='a162 a163' order='S' />
      <bond id = 'b167' atomRefs2='a163 a164' order='S' />
      <bond id = 'b168' atomRefs2='a163 a165' order='D' />
      <bond id = 'b169' atomRefs2='a88 a166' order='S' />
      <bond id = 'b170' atomRefs2='a166 a167' order='S' />
      <bond id = 'b171' atomRefs2='a167 a168' order='S' />
      <bond id = 'b172' atomRefs2='a168 a169' order='S' />
      <bond id = 'b173' atomRefs2='a169 a170' order='S' />
      <bond id = 'b174' atomRefs2='a170 a171' order='S' />
      <bond id = 'b175' atomRefs2='a170 a172' order='D' />
      <bond id = 'b176' atomRefs2='a86 a173' order='S' />
      <bond id = 'b177' atomRefs2='a173 a174' order='S' />
      <bond id = 'b178' atomRefs2='a174 a175' order='S' />
      <bond id = 'b179' atomRefs2='a175 a176' order='S' />
      <bond id = 'b180' atomRefs2='a175 a177' order='D' />
      <bond id = 'b181' atomRefs2='a179 a178' order='D' />
      <bond id = 'b182' atomRefs2='a180 a178' order='S' />
      <bond id = 'b183' atomRefs2='a180 a176' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b184' atomRefs2='a178 a181' order='S' />
      <bond id = 'b185' atomRefs2='a183 a182' order='D' />
      <bond id = 'b186' atomRefs2='a184 a182' order='S' />
      <bond id = 'b187' atomRefs2='a184 a181' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b188' atomRefs2='a182 a185' order='S' />
      <bond id = 'b189' atomRefs2='a187 a186' order='D' />
      <bond id = 'b190' atomRefs2='a188 a186' order='S' />
      <bond id = 'b191' atomRefs2='a188 a185' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b192' atomRefs2='a186 a189' order='S' />
      <bond id = 'b193' atomRefs2='a191 a190' order='D' />
      <bond id = 'b194' atomRefs2='a192 a190' order='S' />
      <bond id = 'b195' atomRefs2='a192 a189' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b196' atomRefs2='a190 a193' order='S' />
      <bond id = 'b197' atomRefs2='a180 a194' order='S' />
      <bond id = 'b198' atomRefs2='a184 a195' order='S' />
      <bond id = 'b199' atomRefs2='a197 a196' order='S' />
      <bond id = 'b200' atomRefs2='a198 a196' order='D' />
      <bond id = 'b201' atomRefs2='a199 a197' order='D' />
      <bond id = 'b202' atomRefs2='a200 a198' order='S' />
      <bond id = 'b203' atomRefs2='a201 a199' order='S' />
      <bond id = 'b204' atomRefs2='a201 a200' order='D' />
      <bond id = 'b205' atomRefs2='a194 a196' order='S' />
      <bond id = 'b206' atomRefs2='a203 a202' order='S' />
      <bond id = 'b207' atomRefs2='a204 a202' order='D' />
      <bond id = 'b208' atomRefs2='a205 a203' order='D' />
      <bond id = 'b209' atomRefs2='a206 a204' order='S' />
      <bond id = 'b210' atomRefs2='a207 a205' order='S' />
      <bond id = 'b211' atomRefs2='a207 a206' order='D' />
      <bond id = 'b212' atomRefs2='a195 a202' order='S' />
      <bond id = 'b213' atomRefs2='a188 a208' order='S' />
      <bond id = 'b214' atomRefs2='a210 a209' order='S' />
      <bond id = 'b215' atomRefs2='a211 a209' order='D' />
      <bond id = 'b216' atomRefs2='a212 a210' order='D' />
      <bond id = 'b217' atomRefs2='a213 a211' order='S' />
      <bond id = 'b218' atomRefs2='a214 a212' order='S' />
      <bond id = 'b219' atomRefs2='a214 a213' order='D' />
      <bond id = 'b220' atomRefs2='a208 a209' order='S' />
      <bond id = 'b221' atomRefs2='a214 a215' order='S' />
      <bond id = 'b222' atomRefs2='a192 a216' order='S' />
      <bond id = 'b223' atomRefs2='a216 a217' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b224' atomRefs2='a216 a218' order='S' />
      <bond id = 'b225' atomRefs2='a220 a219' order='S' />
      <bond id = 'b226' atomRefs2='a221 a219' order='S' />
      <bond id = 'b227' atomRefs2='a222 a220' order='S' />
      <bond id = 'b228' atomRefs2='a193 a221' order='S' />
      <bond id = 'b229' atomRefs2='a222 a193' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b230' atomRefs2='a224 a223' order='D' />
      <bond id = 'b231' atomRefs2='a222 a223' order='S' />
      <bond id = 'b232' atomRefs2='a223 a225' order='S' />
      <bond id = 'b233' atomRefs2='a227 a226' order='D' />
      <bond id = 'b234' atomRefs2='a228 a226' order='S' />
      <bond id = 'b235' atomRefs2='a228 a225' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b236' atomRefs2='a226 a229' order='S' />
      <bond id = 'b237' atomRefs2='a231 a230' order='D' />
      <bond id = 'b238' atomRefs2='a232 a230' order='S' />
      <bond id = 'b239' atomRefs2='a232 a229' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b240' atomRefs2='a230 a233' order='S' />
      <bond id = 'b241' atomRefs2='a228 a234' order='S' />
      <bond id = 'b242' atomRefs2='a234 a235' order='S' />
      <bond id = 'b243' atomRefs2='a235 a236' order='S' />
      <bond id = 'b244' atomRefs2='a236 a237' order='S' />
      <bond id = 'b245' atomRefs2='a237 a238' order='S' />
      <bond id = 'b246' atomRefs2='a232 a239' order='S' />
      <bond id = 'b247' atomRefs2='a239 a240' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b248' atomRefs2='a239 a241' order='S' />
      <bond id = 'b249' atomRefs2='a244 a243' order='D' />
      <bond id = 'b250' atomRefs2='a245 a243' order='S' />
      <bond id = 'b251' atomRefs2='a245 a246' order='S' />
      <bond id = 'b252' atomRefs2='a243 a247' order='S' />
      <bond id = 'b253' atomRefs2='a249 a248' order='D' />
      <bond id = 'b254' atomRefs2='a250 a248' order='S' />
      <bond id = 'b255' atomRefs2='a250 a247' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b256' atomRefs2='a248 a251' order='S' />
      <bond id = 'b257' atomRefs2='a253 a252' order='D' />
      <bond id = 'b258' atomRefs2='a254 a252' order='S' />
      <bond id = 'b259' atomRefs2='a254 a251' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b260' atomRefs2='a252 a255' order='S' />
      <bond id = 'b261' atomRefs2='a257 a256' order='D' />
      <bond id = 'b262' atomRefs2='a258 a256' order='S' />
      <bond id = 'b263' atomRefs2='a258 a255' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b264' atomRefs2='a256 a259' order='S' />
      <bond id = 'b265' atomRefs2='a261 a260' order='D' />
      <bond id = 'b266' atomRefs2='a262 a260' order='S' />
      <bond id = 'b267' atomRefs2='a262 a259' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b268' atomRefs2='a260 a263' order='S' />
      <bond id = 'b269' atomRefs2='a265 a264' order='D' />
      <bond id = 'b270' atomRefs2='a266 a264' order='S' />
      <bond id = 'b271' atomRefs2='a266 a263' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b272' atomRefs2='a264 a267' order='S' />
      <bond id = 'b273' atomRefs2='a269 a268' order='D' />
      <bond id = 'b274' atomRefs2='a270 a268' order='S' />
      <bond id = 'b275' atomRefs2='a270 a267' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b276' atomRefs2='a268 a271' order='S' />
      <bond id = 'b277' atomRefs2='a273 a272' order='D' />
      <bond id = 'b278' atomRefs2='a274 a272' order='S' />
      <bond id = 'b279' atomRefs2='a274 a271' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b280' atomRefs2='a272 a275' order='S' />
      <bond id = 'b281' atomRefs2='a277 a276' order='D' />
      <bond id = 'b282' atomRefs2='a278 a276' order='S' />
      <bond id = 'b283' atomRefs2='a278 a275' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b284' atomRefs2='a276 a279' order='S' />
      <bond id = 'b285' atomRefs2='a281 a280' order='D' />
      <bond id = 'b286' atomRefs2='a282 a280' order='S' />
      <bond id = 'b287' atomRefs2='a282 a279' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b288' atomRefs2='a280 a283' order='S' />
      <bond id = 'b289' atomRefs2='a285 a284' order='D' />
      <bond id = 'b290' atomRefs2='a286 a284' order='S' />
      <bond id = 'b291' atomRefs2='a286 a283' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b292' atomRefs2='a284 a287' order='S' />
      <bond id = 'b293' atomRefs2='a289 a288' order='D' />
      <bond id = 'b294' atomRefs2='a290 a288' order='S' />
      <bond id = 'b295' atomRefs2='a290 a287' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b296' atomRefs2='a288 a291' order='S' />
      <bond id = 'b297' atomRefs2='a293 a292' order='D' />
      <bond id = 'b298' atomRefs2='a294 a292' order='S' />
      <bond id = 'b299' atomRefs2='a294 a291' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b300' atomRefs2='a292 a295' order='S' />
      <bond id = 'b301' atomRefs2='a297 a296' order='D' />
      <bond id = 'b302' atomRefs2='a298 a296' order='S' />
      <bond id = 'b303' atomRefs2='a298 a295' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b304' atomRefs2='a296 a299' order='S' />
      <bond id = 'b305' atomRefs2='a301 a300' order='D' />
      <bond id = 'b306' atomRefs2='a302 a300' order='S' />
      <bond id = 'b307' atomRefs2='a302 a299' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b308' atomRefs2='a300 a303' order='S' />
      <bond id = 'b309' atomRefs2='a305 a304' order='D' />
      <bond id = 'b310' atomRefs2='a306 a304' order='S' />
      <bond id = 'b311' atomRefs2='a306 a303' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b312' atomRefs2='a304 a307' order='S' />
      <bond id = 'b313' atomRefs2='a309 a308' order='D' />
      <bond id = 'b314' atomRefs2='a310 a308' order='S' />
      <bond id = 'b315' atomRefs2='a310 a307' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b316' atomRefs2='a308 a311' order='S' />
      <bond id = 'b317' atomRefs2='a313 a312' order='D' />
      <bond id = 'b318' atomRefs2='a314 a312' order='S' />
      <bond id = 'b319' atomRefs2='a314 a311' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b320' atomRefs2='a312 a315' order='S' />
      <bond id = 'b321' atomRefs2='a317 a316' order='D' />
      <bond id = 'b322' atomRefs2='a318 a316' order='S' />
      <bond id = 'b323' atomRefs2='a318 a315' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b324' atomRefs2='a316 a319' order='S' />
      <bond id = 'b325' atomRefs2='a321 a320' order='D' />
      <bond id = 'b326' atomRefs2='a322 a320' order='S' />
      <bond id = 'b327' atomRefs2='a322 a319' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b328' atomRefs2='a320 a323' order='S' />
      <bond id = 'b329' atomRefs2='a325 a324' order='D' />
      <bond id = 'b330' atomRefs2='a326 a324' order='S' />
      <bond id = 'b331' atomRefs2='a326 a323' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b332' atomRefs2='a324 a327' order='S' />
      <bond id = 'b333' atomRefs2='a250 a328' order='S' />
      <bond id = 'b334' atomRefs2='a328 a329' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b335' atomRefs2='a328 a330' order='S' />
      <bond id = 'b336' atomRefs2='a330 a331' order='S' />
      <bond id = 'b337' atomRefs2='a254 a332' order='S' />
      <bond id = 'b338' atomRefs2='a332 a333' order='S' />
      <bond id = 'b339' atomRefs2='a332 a334' order='S' />
      <bond id = 'b340' atomRefs2='a258 a335' order='S' />
      <bond id = 'b341' atomRefs2='a335 a336' order='S' />
      <bond id = 'b342' atomRefs2='a336 a337' order='S' />
      <bond id = 'b343' atomRefs2='a337 a338' order='D' />
      <bond id = 'b344' atomRefs2='a337 a339' order='S' />
      <bond id = 'b345' atomRefs2='a262 a340' order='S' />
      <bond id = 'b346' atomRefs2='a340 a341' order='S' />
      <bond id = 'b347' atomRefs2='a341 a342' order='S' />
      <bond id = 'b348' atomRefs2='a342 a343' order='S' />
      <bond id = 'b349' atomRefs2='a342 a344' order='D' />
      <bond id = 'b350' atomRefs2='a266 a345' order='S' />
      <bond id = 'b351' atomRefs2='a270 a346' order='S' />
      <bond id = 'b352' atomRefs2='a274 a347' order='S' />
      <bond id = 'b353' atomRefs2='a347 a348' order='S'>
        <bondStereo p4:forcedChange='stereo->H' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>H</bondStereo>
      </bond>
      <bond id = 'b354' atomRefs2='a347 a349' order='S' />
      <bond id = 'b355' atomRefs2='a278 a350' order='S' />
      <bond id = 'b356' atomRefs2='a350 a351' order='S' />
      <bond id = 'b357' atomRefs2='a282 a352' order='S' />
      <bond id = 'b358' atomRefs2='a352 a353' order='S'>
        <bondStereo p4:forcedChange='stereo->W' xmlns:p4='http://www.xml-cml.org/dictionary/cmlx/'>W</bondStereo>
      </bond>
      <bond id = 'b359' atomRefs2='a352 a354' order='S' />
      <bond id = 'b360' atomRefs2='a354 a355' order='S' />
      <bond id = 'b361' atomRefs2='a286 a356' order='S' />
      <bond id = 'b362' atomRefs2='a290 a357' order='S' />
      <bond id = 'b363' atomRefs2='a357 a358' order='S' />
      <bond id = 'b364' atomRefs2='a294 a359' order='S' />
      <bond id = 'b365' atomRefs2='a359 a360' order='S' />
      <bond id = 'b366' atomRefs2='a360 a361' order='S' />
      <bond id = 'b367' atomRefs2='a360 a362' order='S' />
      <bond id = 'b368' atomRefs2='a298 a363' order='S' />
      <bond id = 'b369' atomRefs2='a365 a364' order='S' />
      <bond id = 'b370' atomRefs2='a366 a364' order='D' />
      <bond id = 'b371' atomRefs2='a367 a365' order='D' />
      <bond id = 'b372' atomRefs2='a368 a366' order='S' />
      <bond id = 'b373' atomRefs2='a369 a367' order='S' />
      <bond id = 'b374' atomRefs2='a369 a368' order='D' />
      <bond id = 'b375' atomRefs2='a363 a364' order='S' />
      <bond id = 'b376' atomRefs2='a369 a370' order='S' />
      <bond id = 'b377' atomRefs2='a302 a371' order='S' />
      <bond id = 'b378' atomRefs2='a371 a372' order='S' />
      <bond id = 'b379' atomRefs2='a372 a373' order='S' />
      <bond id = 'b380' atomRefs2='a373 a374' order='S' />
      <bond id = 'b381' atomRefs2='a373 a375' order='D' />
      <bond id = 'b382' atomRefs2='a306 a376' order='S' />
      <bond id = 'b383' atomRefs2='a376 a377' order='S' />
      <bond id = 'b384' atomRefs2='a377 a378' order='S' />
      <bond id = 'b385' atomRefs2='a377 a379' order='S' />
      <bond id = 'b386' atomRefs2='a310 a380' order='S' />
      <bond id = 'b387' atomRefs2='a380 a381' order='S' />
      <bond id = 'b388' atomRefs2='a381 a382' order='S' />
      <bond id = 'b389' atomRefs2='a382 a383' order='S' />
      <bond id = 'b390' atomRefs2='a382 a384' order='D' />
      <bond id = 'b391' atomRefs2='a314 a385' order='S' />
      <bond id = 'b392' atomRefs2='a385 a386' order='S' />
      <bond id = 'b393' atomRefs2='a386 a387' order='D' />
      <bond id = 'b394' atomRefs2='a386 a388' order='S' />
      <bond id = 'b395' atomRefs2='a318 a389' order='S' />
      <bond id = 'b396' atomRefs2='a391 a390' order='S' />
      <bond id = 'b397' atomRefs2='a392 a390' order='D' />
      <bond id = 'b398' atomRefs2='a393 a391' order='D' />
      <bond id = 'b399' atomRefs2='a394 a392' order='S' />
      <bond id = 'b400' atomRefs2='a395 a393' order='S' />
      <bond id = 'b401' atomRefs2='a395 a394' order='D' />
      <bond id = 'b402' atomRefs2='a389 a390' order='S' />
      <bond id = 'b403' atomRefs2='a395 a396' order='S' />
      <bond id = 'b404' atomRefs2='a322 a397' order='S' />
      <bond id = 'b405' atomRefs2='a397 a242' order='S' />
      <bond id = 'b406' atomRefs2='a326 a398' order='S' />
      <bond id = 'b407' atomRefs2='a398 a399' order='S' />
      <bond id = 'b408' atomRefs2='a399 a400' order='D' />
      <bond id = 'b409' atomRefs2='a399 a401' order='S' />
      <bond id = 'b410' atomRefs2='a356 a402' order='S' />
      <bond id = 'b411' atomRefs2='a345 a403' order='S' />
      <bond id = 'b412' atomRefs2='a403 a402' order='S' />
      <bond id = 'b413' atomRefs2='a160 a242' order='S' />
      <bond id = 'b414' atomRefs2='a346 a404' order='S' />
      <bond id = 'b415' atomRefs2='a404 a118' order='S' />
    </bondArray>
    <formula concise = '' />
  </molecule >
</cml:cml>";

        public const string TESTOSTERONE = @"<cml
    convention='conventions:molecular'
    conventions='http://www.xml-cml.org/convention/'
    xmlns='http://www.xml-cml.org/schema'
    cmlDict='http://www.xml-cml.org/dictionary/cml/'
    nameDict='http://www.xml-cml.org/dictionary/cml/name/'
    xmlns:c4w='http://www.chem4word.com/cml'>
    <c4w:customXmlPartGuid>ee59cc41d1034ecd942be2c0b7c486e4</c4w:customXmlPartGuid>
    <molecule id='m1'>
        <name dictRef='nameDict:iupac'>(8R,9S,10R,13S,14S,17S)-17-hydroxy-10,13-dimethyl-1,2,6,7,8,9,11,12,14,15,16,17-dodecahydrocyclopenta[a]phenanthren-3-one</name>
        <name dictRef='nameDict:trivial'>testosterone</name>
        <formula concise='C 19 H 28 O 2'/>
        <atomArray>
            <atom id='a1' elementType='C' x2='-19.90999984741211' y2='3.465033833434365'/>
            <atom id='a2' elementType='C' x2='-21.243669188460366' y2='2.6950168928753246'/>
            <atom id='a3' elementType='C' x2='-21.243669188460366' y2='1.1549830117572437'/>
            <atom id='a4' elementType='C' x2='-19.90999984741211' y2='0.38496607119820325'/>
            <atom id='a5' elementType='C' x2='-18.576330506363853' y2='1.1549830117572437'/>
            <atom id='a6' elementType='C' x2='-18.576330506363853' y2='2.6950168928753246'/>
            <atom id='a7' elementType='C' x2='-17.242661165315596' y2='3.465033833434365'/>
            <atom id='a8' elementType='C' x2='-17.242661165315596' y2='0.38496607119820325'/>
            <atom id='a9' elementType='C' x2='-15.90899182426734' y2='1.1549830117572437'/>
            <atom id='a10' elementType='C' x2='-15.90899182426734' y2='2.6950168928753246'/>
            <atom id='a11' elementType='C' x2='-15.908991889174914' y2='5.775084655111486'/>
            <atom id='a12' elementType='C' x2='-17.242661213996275' y2='5.0050676864474655'/>
            <atom id='a13' elementType='C' x2='-14.575322531899763' y2='5.005067742657426'/>
            <atom id='a14' elementType='C' x2='-13.110705319128085' y2='5.480984142934748'/>
            <atom id='a15' elementType='C' x2='-14.575322499445976' y2='3.4650338615393443'/>
            <atom id='a16' elementType='C' x2='-13.110688633553075' y2='2.989260317649962'/>
            <atom id='a17' elementType='C' x2='-12.20554493688105' y2='4.235076970465463'/>
            <atom id='a18' elementType='O' x2='-12.634817271767822' y2='6.945610568471183'/>
            <atom id='a19' elementType='C' x2='-14.524354449478643' y2='6.536632102052604'/>
            <atom id='a20' elementType='H' x2='-14.575322499445976' y2='1.9250338615393443'/>
            <atom id='a21' elementType='H' x2='-15.90899182426734' y2='4.235016892875325'/>
            <atom id='a22' elementType='H' x2='-17.242661165315596' y2='1.925033833434365'/>
            <atom id='a23' elementType='C' x2='-18.576330506363853' y2='4.235016892875325'/>
            <atom id='a24' elementType='O' x2='-22.5773483102884' y2='0.3849830117572426'/>
            <atom id='a25' elementType='H' x2='-11.128469556519141' y2='7.265792642868042'/>
            <atom id='a26' elementType='H' x2='-21.51024453534811' y2='3.8808512704985474'/>
            <atom id='a27' elementType='H' x2='-21.05437947175186' y2='4.9094621071492766'/>
            <atom id='a28' elementType='H' x2='-22.738987570037402' y2='3.4855783510250347'/>
            <atom id='a29' elementType='H' x2='-22.767050161279386' y2='2.096531603074844'/>
            <atom id='a30' elementType='H' x2='-20.3898742569292' y2='-1.1366101162995557'/>
            <atom id='a31' elementType='H' x2='-18.253315942355496' y2='-0.8786519614788898'/>
            <atom id='a32' elementType='H' x2='-16.826017613996882' y2='-1.2026634255247548'/>
            <atom id='a33' elementType='H' x2='-15.495965477684349' y2='-0.438080969967525'/>
            <atom id='a34' elementType='H' x2='-14.47378988204199' y2='0.333585356940667'/>
            <atom id='a35' elementType='H' x2='-16.848399997602154' y2='7.395389353416233'/>
            <atom id='a36' elementType='H' x2='-15.422148019557154' y2='7.610743902337664'/>
            <atom id='a37' elementType='H' x2='-17.739831360294325' y2='6.576129988234964'/>
            <atom id='a38' elementType='H' x2='-18.573768186237118' y2='5.884251962734775'/>
            <atom id='a39' elementType='H' x2='-11.536717411688516' y2='5.792716949656001'/>
            <atom id='a40' elementType='H' x2='-12.849127500876868' y2='1.3745227924742949'/>
            <atom id='a41' elementType='H' x2='-11.732407589844149' y2='2.092647355815956'/>
            <atom id='a42' elementType='H' x2='-10.836424898802628' y2='3.3290354185226096'/>
            <atom id='a43' elementType='H' x2='-10.593644713792333' y2='4.65324020631'/>
            <atom id='a44' elementType='H' x2='-13.360819862054347' y2='6.505448103936256'/>
            <atom id='a45' elementType='H' x2='-14.06369573479471' y2='8.167577295308973'/>
            <atom id='a46' elementType='H' x2='-15.996441851124487' y2='6.793667952050191'/>
            <atom id='a47' elementType='H' x2='-16.961662218192203' y2='4.2309301047132335'/>
            <atom id='a48' elementType='H' x2='-19.66112933385544' y2='5.688406445535843'/>
            <atom id='a49' elementType='H' x2='-19.946589819156426' y2='4.702392740654843'/>
        </atomArray>
        <bondArray>
            <bond atomRefs2='a1 a2' order='S' id='b1' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a2 a3' order='S' id='b2' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a3 a4' order='S' id='b3' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a4 a5' order='D' id='b4' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a1 a6' order='S' id='b5' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a7 a6' order='S' id='b6' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a6 a5' order='S' id='b7' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a5 a8' order='S' id='b8' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a8 a9' order='S' id='b9' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a9 a10' order='S' id='b10' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a10 a15' order='S' id='b11' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a7 a10' order='S' id='b12' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a12 a7' order='S' id='b13' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a11 a12' order='S' id='b14' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a11 a13' order='S' id='b15' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a14 a13' order='S' id='b16' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a15 a16' order='S' id='b17' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a16 a17' order='S' id='b18' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a14 a17' order='S' id='b19' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a13 a15' order='S' id='b20' cyclic='Cyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a14 a18' order='S' id='b21' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'>
                <bondStereo>W</bondStereo>
            </bond>
            <bond atomRefs2='a13 a19' order='S' id='b22' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'>
                <bondStereo>W</bondStereo>
            </bond>
            <bond atomRefs2='a15 a20' order='S' id='b23' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'>
                <bondStereo>H</bondStereo>
            </bond>
            <bond atomRefs2='a10 a21' order='S' id='b24' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'>
                <bondStereo>W</bondStereo>
            </bond>
            <bond atomRefs2='a7 a22' order='S' id='b25' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'>
                <bondStereo>H</bondStereo>
            </bond>
            <bond atomRefs2='a6 a23' order='S' id='b26' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'>
                <bondStereo>W</bondStereo>
            </bond>
            <bond atomRefs2='a3 a24' order='D' id='b27' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a18 a25' order='S' id='b28' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a1 a26' order='S' id='b29' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a1 a27' order='S' id='b30' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a2 a28' order='S' id='b31' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a2 a29' order='S' id='b32' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a4 a30' order='S' id='b33' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a8 a31' order='S' id='b34' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a8 a32' order='S' id='b35' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a9 a33' order='S' id='b36' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a9 a34' order='S' id='b37' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a11 a35' order='S' id='b38' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a11 a36' order='S' id='b39' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a12 a37' order='S' id='b40' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a12 a38' order='S' id='b41' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a14 a39' order='S' id='b42' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'>
                <bondStereo>H</bondStereo>
            </bond>
            <bond atomRefs2='a16 a40' order='S' id='b43' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a16 a41' order='S' id='b44' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a17 a42' order='S' id='b45' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a17 a43' order='S' id='b46' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a19 a44' order='S' id='b47' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a19 a45' order='S' id='b48' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a19 a46' order='S' id='b49' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a23 a47' order='S' id='b50' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a23 a48' order='S' id='b51' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
            <bond atomRefs2='a23 a49' order='S' id='b52' cyclic='Acyclic' p7='http://www.xml-cml.org/dictionary/cmlx/'/>
        </bondArray>
    </molecule>
</cml>";

        public const string PHTHALOCYANINE = @"<cml convention='cmlDict:cmllite' cml='http://www.xml-cml.org/schema' cmlDict='http://www.xml-cml.org/dictionary/cml/' bondDict='http://www.xml-cml.org/dictionary/bond/' pc='http://www.ncbi.nlm.nih.gov'>
<molecule formalCharge='0' id='m1' spinMultiplicity='1'>
<name dictRef='pc:CID'>475701</name>
<atomArray>
<atom id='a1' x2='142.896116420753' y2='0.689661551738648' elementType='Hg'/>
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

        public const string THEMONSTER = @"<?xml version='1.0' encoding='utf-8'?>
<cml convention='conventions:molecular' xmlns='http://www.xml-cml.org/schema' xmlns:conventions='http://www.xml-cml.org/convention/'>
  <molecule id='m14969' formalCharge='0' xmlns:pubchem='http://www.ncbi.nlm.nih.gov'>
    <name dictRef='pubchem:cid'>14969</name>
    <atomArray>
      <atom id='a1' elementType='Cl' x2='10.6602' y2='-0.3451' />
      <atom id='a2' elementType='Cl' x2='7.2808' y2='-0.5349' />
      <atom id='a3' elementType='O' x2='9.7942' y2='5.155' />
      <atom id='a4' elementType='O' x2='8.9282' y2='3.655' />
      <atom id='a5' elementType='O' x2='10.6603' y2='1.655' />
      <atom id='a6' elementType='O' x2='8.9282' y2='7.655' />
      <atom id='a7' elementType='O' x2='10.6602' y2='4.655' />
      <atom id='a8' elementType='O' x2='8.9282' y2='1.655' />
      <atom id='a9' elementType='O' x2='12.3923' y2='3.655' />
      <atom id='a10' elementType='O' x2='13.2583' y2='2.155' />
      <atom id='a11' elementType='O' x2='10.6603' y2='0.655' />
      <atom id='a12' elementType='O' x2='7.1962' y2='0.655' />
      <atom id='a13' elementType='O' x2='8.0609' y2='-3.845' />
      <atom id='a14' elementType='O' x2='10.6603' y2='-1.345' />
      <atom id='a15' elementType='O' x2='14.8315' y2='-0.6379' />
      <atom id='a16' elementType='O' x2='3.1585' y2='-0.5258' />
      <atom id='a17' elementType='O' x2='4.5954' y2='-1.8498' />
      <atom id='a18' elementType='O' x2='13.2583' y2='-3.845' />
      <atom id='a19' elementType='O' x2='2.0' y2='-2.345' />
      <atom id='a20' elementType='O' x2='14.642' y2='-4.2768' />
      <atom id='a21' elementType='O' x2='6.3301' y2='-6.845' />
      <atom id='a22' elementType='O' x2='10.1121' y2='-4.2592' />
      <atom id='a23' elementType='O' x2='5.4641' y2='-7.345' />
      <atom id='a24' elementType='O' x2='5.3574' y2='-4.9436' />
      <atom id='a25' elementType='O' x2='5.486' y2='-3.5353' />
      <atom id='a26' elementType='O' x2='2.0' y2='-7.345' />
      <atom id='a27' elementType='N' x2='7.5622' y2='7.021' />
      <atom id='a28' elementType='N' x2='9.7942' y2='-2.845' />
      <atom id='a29' elementType='N' x2='7.1962' y2='-2.345' />
      <atom id='a30' elementType='N' x2='12.3923' y2='-2.345' />
      <atom id='a31' elementType='N' x2='4.5981' y2='-2.845' />
      <atom id='a32' elementType='N' x2='15.0903' y2='-2.6038' />
      <atom id='a33' elementType='N' x2='2.866' y2='-3.845' />
      <atom id='a34' elementType='N' x2='16.5739' y2='-4.7945' />
      <atom id='a35' elementType='N' x2='11.3368' y2='-5.484' />
      <atom id='a36' elementType='C' x2='8.0622' y2='6.155'>
        <atomParity atomRefs4='a27 a38 a37 a43'>-1.0</atomParity>
      </atom>
      <atom id='a37' elementType='C' x2='8.9282' y2='6.655'>
        <atomParity atomRefs4='a6 a36 a40 a102'>1.0</atomParity>
      </atom>
      <atom id='a38' elementType='C' x2='8.0622' y2='5.155' />
      <atom id='a39' elementType='C' x2='8.9282' y2='4.655'>
        <atomParity atomRefs4='a3 a4 a38 a105'>1.0</atomParity>
      </atom>
      <atom id='a40' elementType='C' x2='9.7942' y2='6.155'>
        <atomParity atomRefs4='a3 a47 a37 a106'>-1.0</atomParity>
      </atom>
      <atom id='a41' elementType='C' x2='9.7942' y2='3.155'>
        <atomParity atomRefs4='a4 a45 a42 a107'>-1.0</atomParity>
      </atom>
      <atom id='a42' elementType='C' x2='10.6602' y2='3.655'>
        <atomParity atomRefs4='a7 a44 a41 a108'>-1.0</atomParity>
      </atom>
      <atom id='a43' elementType='C' x2='7.0622' y2='6.155' />
      <atom id='a44' elementType='C' x2='11.5263' y2='3.155'>
        <atomParity atomRefs4='a9 a42 a46 a112'>1.0</atomParity>
      </atom>
      <atom id='a45' elementType='C' x2='9.7942' y2='2.155'>
        <atomParity atomRefs4='a5 a8 a41 a113'>1.0</atomParity>
      </atom>
      <atom id='a46' elementType='C' x2='11.5263' y2='2.155'>
        <atomParity atomRefs4='a5 a44 a48 a114'>1.0</atomParity>
      </atom>
      <atom id='a47' elementType='C' x2='10.6602' y2='6.655' />
      <atom id='a48' elementType='C' x2='12.3923' y2='1.655' />
      <atom id='a49' elementType='C' x2='8.9282' y2='0.655' />
      <atom id='a50' elementType='C' x2='9.7942' y2='0.155' />
      <atom id='a51' elementType='C' x2='8.0622' y2='0.155' />
      <atom id='a52' elementType='C' x2='9.7942' y2='-0.845' />
      <atom id='a53' elementType='C' x2='8.0622' y2='-0.845' />
      <atom id='a54' elementType='C' x2='8.9282' y2='-1.345' />
      <atom id='a55' elementType='C' x2='8.9282' y2='-2.345'>
        <atomParity atomRefs4='a28 a56 a54 a128'>-1.0</atomParity>
      </atom>
      <atom id='a56' elementType='C' x2='8.0622' y2='-2.845' />
      <atom id='a57' elementType='C' x2='11.5263' y2='0.155' />
      <atom id='a58' elementType='C' x2='6.3301' y2='0.155' />
      <atom id='a59' elementType='C' x2='10.6603' y2='-2.345' />
      <atom id='a60' elementType='C' x2='6.3301' y2='-2.845'>
        <atomParity atomRefs4='a29 a77 a76 a131'>-1.0</atomParity>
      </atom>
      <atom id='a61' elementType='C' x2='11.5263' y2='-2.845'>
        <atomParity atomRefs4='a30 a78 a59 a132'>-1.0</atomParity>
      </atom>
      <atom id='a62' elementType='C' x2='11.5263' y2='-0.845' />
      <atom id='a63' elementType='C' x2='12.3923' y2='0.655' />
      <atom id='a64' elementType='C' x2='6.3301' y2='-0.845' />
      <atom id='a65' elementType='C' x2='5.4641' y2='0.655' />
      <atom id='a66' elementType='C' x2='14.1244' y2='-1.345'>
        <atomParity atomRefs4='a15 a67 a68 a136'>1.0</atomParity>
      </atom>
      <atom id='a67' elementType='C' x2='14.1244' y2='-2.345'>
        <atomParity atomRefs4='a32 a79 a66 a135'>-1.0</atomParity>
      </atom>
      <atom id='a68' elementType='C' x2='13.2583' y2='-0.845' />
      <atom id='a69' elementType='C' x2='3.7321' y2='-1.345'>
        <atomParity atomRefs4='a16 a71 a70 a137'>-1.0</atomParity>
      </atom>
      <atom id='a70' elementType='C' x2='4.5981' y2='-0.845' />
      <atom id='a71' elementType='C' x2='3.7321' y2='-2.345'>
        <atomParity atomRefs4='a31 a69 a81 a138'>1.0</atomParity>
      </atom>
      <atom id='a72' elementType='C' x2='12.3923' y2='-1.345' />
      <atom id='a73' elementType='C' x2='13.2583' y2='0.155' />
      <atom id='a74' elementType='C' x2='5.4641' y2='-1.345' />
      <atom id='a75' elementType='C' x2='4.5981' y2='0.155' />
      <atom id='a76' elementType='C' x2='6.3301' y2='-3.845' />
      <atom id='a77' elementType='C' x2='5.4641' y2='-2.345' />
      <atom id='a78' elementType='C' x2='11.7851' y2='-3.8109' />
      <atom id='a79' elementType='C' x2='13.2583' y2='-2.845' />
      <atom id='a80' elementType='C' x2='16.315' y2='-3.8286'>
        <atomParity atomRefs4='a34 a84 a82 a148'>-1.0</atomParity>
      </atom>
      <atom id='a81' elementType='C' x2='2.866' y2='-2.845' />
      <atom id='a82' elementType='C' x2='17.0221' y2='-3.1215' />
      <atom id='a83' elementType='C' x2='5.4641' y2='-4.345' />
      <atom id='a84' elementType='C' x2='15.3491' y2='-3.5697' />
      <atom id='a85' elementType='C' x2='17.9881' y2='-3.3803' />
      <atom id='a86' elementType='C' x2='7.1962' y2='-4.345' />
      <atom id='a87' elementType='C' x2='5.4641' y2='-5.345' />
      <atom id='a88' elementType='C' x2='3.7321' y2='-4.345'>
        <atomParity atomRefs4='a33 a90 a98 a154'>1.0</atomParity>
      </atom>
      <atom id='a89' elementType='C' x2='4.5981' y2='-5.845' />
      <atom id='a90' elementType='C' x2='3.7321' y2='-5.345' />
      <atom id='a91' elementType='C' x2='11.078' y2='-4.518' />
      <atom id='a92' elementType='C' x2='6.3301' y2='-5.845' />
      <atom id='a93' elementType='C' x2='7.1962' y2='-5.345' />
      <atom id='a94' elementType='C' x2='18.6952' y2='-2.6732' />
      <atom id='a95' elementType='C' x2='18.2469' y2='-4.3462' />
      <atom id='a96' elementType='C' x2='4.5981' y2='-6.845' />
      <atom id='a97' elementType='C' x2='2.866' y2='-5.845' />
      <atom id='a98' elementType='C' x2='4.7175' y2='-4.1751' />
      <atom id='a99' elementType='C' x2='17.5398' y2='-5.0533' />
      <atom id='a100' elementType='C' x2='2.866' y2='-6.845' />
      <atom id='a101' elementType='C' x2='3.7321' y2='-7.345' />
      <atom id='a102' elementType='H' x2='8.3913' y2='6.965' />
      <atom id='a103' elementType='H' x2='7.4516' y2='5.2627' />
      <atom id='a104' elementType='H' x2='7.8501' y2='4.5724' />
      <atom id='a105' elementType='H' x2='8.3913' y2='4.345' />
      <atom id='a106' elementType='H' x2='10.3311' y2='5.845' />
      <atom id='a107' elementType='H' x2='9.7942' y2='3.775' />
      <atom id='a108' elementType='H' x2='11.1972' y2='3.965' />
      <atom id='a109' elementType='H' x2='7.0622' y2='6.775' />
      <atom id='a110' elementType='H' x2='6.4422' y2='6.155' />
      <atom id='a111' elementType='H' x2='7.0622' y2='5.535' />
      <atom id='a112' elementType='H' x2='11.5263' y2='3.775' />
      <atom id='a113' elementType='H' x2='9.2573' y2='2.465' />
      <atom id='a114' elementType='H' x2='11.5263' y2='1.535' />
      <atom id='a115' elementType='H' x2='6.9422' y2='7.021' />
      <atom id='a116' elementType='H' x2='7.8722' y2='7.558' />
      <atom id='a117' elementType='H' x2='10.9702' y2='6.1181' />
      <atom id='a118' elementType='H' x2='11.1972' y2='6.965' />
      <atom id='a119' elementType='H' x2='10.3502' y2='7.1919' />
      <atom id='a120' elementType='H' x2='9.4651' y2='7.965' />
      <atom id='a121' elementType='H' x2='11.9938' y2='1.1801' />
      <atom id='a122' elementType='H' x2='12.7908' y2='1.1801' />
      <atom id='a123' elementType='H' x2='11.1972' y2='4.965' />
      <atom id='a124' elementType='H' x2='12.3923' y2='4.275' />
      <atom id='a125' elementType='H' x2='13.7953' y2='1.845' />
      <atom id='a126' elementType='H' x2='10.3312' y2='-1.155' />
      <atom id='a127' elementType='H' x2='7.5253' y2='-1.155' />
      <atom id='a128' elementType='H' x2='8.9282' y2='-3.195' />
      <atom id='a129' elementType='H' x2='9.7942' y2='-3.465' />
      <atom id='a130' elementType='H' x2='7.1962' y2='-1.725' />
      <atom id='a131' elementType='H' x2='6.9945' y2='-3.3753' />
      <atom id='a132' elementType='H' x2='11.3658' y2='-3.4439' />
      <atom id='a133' elementType='H' x2='12.3923' y2='1.275' />
      <atom id='a134' elementType='H' x2='5.4641' y2='1.275' />
      <atom id='a135' elementType='H' x2='14.5628' y2='-2.7834' />
      <atom id='a136' elementType='H' x2='14.7232' y2='-1.1845' />
      <atom id='a137' elementType='H' x2='3.1332' y2='-1.1845' />
      <atom id='a138' elementType='H' x2='2.9959' y2='-1.92' />
      <atom id='a139' elementType='H' x2='12.3923' y2='-1.965' />
      <atom id='a140' elementType='H' x2='13.7953' y2='0.465' />
      <atom id='a141' elementType='H' x2='5.4641' y2='-1.965' />
      <atom id='a142' elementType='H' x2='4.0611' y2='0.465' />
      <atom id='a143' elementType='H' x2='11.84' y2='-2.0633' />
      <atom id='a144' elementType='H' x2='12.1407' y2='-4.3188' />
      <atom id='a145' elementType='H' x2='12.347' y2='-3.5489' />
      <atom id='a146' elementType='H' x2='4.5981' y2='-3.465' />
      <atom id='a147' elementType='H' x2='15.5287' y2='-2.1654' />
      <atom id='a148' elementType='H' x2='16.9139' y2='-3.989' />
      <atom id='a149' elementType='H' x2='17.2842' y2='-2.5595' />
      <atom id='a150' elementType='H' x2='16.5143' y2='-2.7658' />
      <atom id='a151' elementType='H' x2='4.9272' y2='-4.035' />
      <atom id='a152' elementType='H' x2='17.5497' y2='-3.8187' />
      <atom id='a153' elementType='H' x2='7.7331' y2='-4.035' />
      <atom id='a154' elementType='H' x2='4.2085' y2='-3.9483' />
      <atom id='a155' elementType='H' x2='2.3291' y2='-4.155' />
      <atom id='a156' elementType='H' x2='15.4303' y2='-0.7984' />
      <atom id='a157' elementType='H' x2='3.4205' y2='0.0361' />
      <atom id='a158' elementType='H' x2='7.7331' y2='-5.655' />
      <atom id='a159' elementType='H' x2='16.1355' y2='-5.2329' />
      <atom id='a160' elementType='H' x2='19.1336' y2='-3.1116' />
      <atom id='a161' elementType='H' x2='19.1336' y2='-2.2348' />
      <atom id='a162' elementType='H' x2='18.2568' y2='-2.2348' />
      <atom id='a163' elementType='H' x2='17.648' y2='-4.5067' />
      <atom id='a164' elementType='H' x2='18.4074' y2='-4.9451' />
      <atom id='a165' elementType='H' x2='18.8458' y2='-4.1857' />
      <atom id='a166' elementType='H' x2='2.3291' y2='-5.535' />
      <atom id='a167' elementType='H' x2='17.3793' y2='-5.6522' />
      <atom id='a168' elementType='H' x2='18.1387' y2='-5.2138' />
      <atom id='a169' elementType='H' x2='17.7002' y2='-4.4544' />
      <atom id='a170' elementType='H' x2='10.8984' y2='-5.9224' />
      <atom id='a171' elementType='H' x2='11.9357' y2='-5.6444' />
      <atom id='a172' elementType='H' x2='3.7321' y2='-7.965' />
      <atom id='a173' elementType='H' x2='6.8671' y2='-7.155' />
      <atom id='a174' elementType='H' x2='5.4641' y2='-7.965' />
      <atom id='a175' elementType='H' x2='5.9684' y2='-4.8383' />
      <atom id='a176' elementType='H' x2='2.0' y2='-7.965' />
    </atomArray>
    <bondArray>
      <bond atomRefs2='a1 a62' order='1' id='a1_a62' />
      <bond atomRefs2='a2 a64' order='1' id='a2_a64' />
      <bond atomRefs2='a3 a39' order='1' id='a3_a39' />
      <bond atomRefs2='a3 a40' order='1' id='a3_a40' />
      <bond atomRefs2='a4 a39' order='1' id='a4_a39'>
        <bondStereo>W</bondStereo>
      </bond>
      <bond atomRefs2='a4 a41' order='1' id='a4_a41'>
        <bondStereo>H</bondStereo>
      </bond>
      <bond atomRefs2='a5 a45' order='1' id='a5_a45' />
      <bond atomRefs2='a5 a46' order='1' id='a5_a46' />
      <bond atomRefs2='a6 a37' order='1' id='a6_a37'>
        <bondStereo>H</bondStereo>
      </bond>
      <bond atomRefs2='a6 a120' order='1' id='a6_a120' />
      <bond atomRefs2='a7 a42' order='1' id='a7_a42'>
        <bondStereo>W</bondStereo>
      </bond>
      <bond atomRefs2='a7 a123' order='1' id='a7_a123' />
      <bond atomRefs2='a8 a45' order='1' id='a8_a45'>
        <bondStereo>W</bondStereo>
      </bond>
      <bond atomRefs2='a8 a49' order='1' id='a8_a49' />
      <bond atomRefs2='a9 a44' order='1' id='a9_a44'>
        <bondStereo>H</bondStereo>
      </bond>
      <bond atomRefs2='a9 a124' order='1' id='a9_a124' />
      <bond atomRefs2='a10 a48' order='1' id='a10_a48' />
      <bond atomRefs2='a10 a125' order='1' id='a10_a125' />
      <bond atomRefs2='a11 a50' order='1' id='a11_a50' />
      <bond atomRefs2='a11 a57' order='1' id='a11_a57' />
      <bond atomRefs2='a12 a51' order='1' id='a12_a51' />
      <bond atomRefs2='a12 a58' order='1' id='a12_a58' />
      <bond atomRefs2='a13 a56' order='2' id='a13_a56' />
      <bond atomRefs2='a14 a59' order='2' id='a14_a59' />
      <bond atomRefs2='a15 a66' order='1' id='a15_a66'>
        <bondStereo>W</bondStereo>
      </bond>
      <bond atomRefs2='a15 a156' order='1' id='a15_a156' />
      <bond atomRefs2='a16 a69' order='1' id='a16_a69'>
        <bondStereo>H</bondStereo>
      </bond>
      <bond atomRefs2='a16 a157' order='1' id='a16_a157' />
      <bond atomRefs2='a17 a77' order='2' id='a17_a77' />
      <bond atomRefs2='a18 a79' order='2' id='a18_a79' />
      <bond atomRefs2='a19 a81' order='2' id='a19_a81' />
      <bond atomRefs2='a20 a84' order='2' id='a20_a84' />
      <bond atomRefs2='a21 a92' order='1' id='a21_a92' />
      <bond atomRefs2='a21 a173' order='1' id='a21_a173' />
      <bond atomRefs2='a22 a91' order='2' id='a22_a91' />
      <bond atomRefs2='a23 a96' order='1' id='a23_a96' />
      <bond atomRefs2='a23 a174' order='1' id='a23_a174' />
      <bond atomRefs2='a24 a98' order='1' id='a24_a98' />
      <bond atomRefs2='a24 a175' order='1' id='a24_a175' />
      <bond atomRefs2='a25 a98' order='2' id='a25_a98' />
      <bond atomRefs2='a26 a100' order='1' id='a26_a100' />
      <bond atomRefs2='a26 a176' order='1' id='a26_a176' />
      <bond atomRefs2='a27 a36' order='1' id='a27_a36'>
        <bondStereo>H</bondStereo>
      </bond>
      <bond atomRefs2='a27 a115' order='1' id='a27_a115' />
      <bond atomRefs2='a27 a116' order='1' id='a27_a116' />
      <bond atomRefs2='a28 a55' order='1' id='a28_a55' />
      <bond atomRefs2='a28 a59' order='1' id='a28_a59' />
      <bond atomRefs2='a28 a129' order='1' id='a28_a129' />
      <bond atomRefs2='a29 a56' order='1' id='a29_a56' />
      <bond atomRefs2='a29 a60' order='1' id='a29_a60' />
      <bond atomRefs2='a29 a130' order='1' id='a29_a130' />
      <bond atomRefs2='a30 a61' order='1' id='a30_a61' />
      <bond atomRefs2='a30 a79' order='1' id='a30_a79' />
      <bond atomRefs2='a30 a143' order='1' id='a30_a143' />
      <bond atomRefs2='a31 a71' order='1' id='a31_a71' />
      <bond atomRefs2='a31 a77' order='1' id='a31_a77' />
      <bond atomRefs2='a31 a146' order='1' id='a31_a146' />
      <bond atomRefs2='a32 a67' order='1' id='a32_a67'>
        <bondStereo>W</bondStereo>
      </bond>
      <bond atomRefs2='a32 a84' order='1' id='a32_a84' />
      <bond atomRefs2='a32 a147' order='1' id='a32_a147' />
      <bond atomRefs2='a33 a81' order='1' id='a33_a81' />
      <bond atomRefs2='a33 a88' order='1' id='a33_a88' />
      <bond atomRefs2='a33 a155' order='1' id='a33_a155' />
      <bond atomRefs2='a34 a80' order='1' id='a34_a80'>
        <bondStereo>W</bondStereo>
      </bond>
      <bond atomRefs2='a34 a99' order='1' id='a34_a99' />
      <bond atomRefs2='a34 a159' order='1' id='a34_a159' />
      <bond atomRefs2='a35 a91' order='1' id='a35_a91' />
      <bond atomRefs2='a35 a170' order='1' id='a35_a170' />
      <bond atomRefs2='a35 a171' order='1' id='a35_a171' />
      <bond atomRefs2='a36 a37' order='1' id='a36_a37' />
      <bond atomRefs2='a36 a38' order='1' id='a36_a38' />
      <bond atomRefs2='a36 a43' order='1' id='a36_a43' />
      <bond atomRefs2='a37 a40' order='1' id='a37_a40' />
      <bond atomRefs2='a37 a102' order='1' id='a37_a102' />
      <bond atomRefs2='a38 a39' order='1' id='a38_a39' />
      <bond atomRefs2='a38 a103' order='1' id='a38_a103' />
      <bond atomRefs2='a38 a104' order='1' id='a38_a104' />
      <bond atomRefs2='a39 a105' order='1' id='a39_a105' />
      <bond atomRefs2='a40 a47' order='1' id='a40_a47'>
        <bondStereo>H</bondStereo>
      </bond>
      <bond atomRefs2='a40 a106' order='1' id='a40_a106' />
      <bond atomRefs2='a41 a42' order='1' id='a41_a42' />
      <bond atomRefs2='a41 a45' order='1' id='a41_a45' />
      <bond atomRefs2='a41 a107' order='1' id='a41_a107' />
      <bond atomRefs2='a42 a44' order='1' id='a42_a44' />
      <bond atomRefs2='a42 a108' order='1' id='a42_a108' />
      <bond atomRefs2='a43 a109' order='1' id='a43_a109' />
      <bond atomRefs2='a43 a110' order='1' id='a43_a110' />
      <bond atomRefs2='a43 a111' order='1' id='a43_a111' />
      <bond atomRefs2='a44 a46' order='1' id='a44_a46' />
      <bond atomRefs2='a44 a112' order='1' id='a44_a112' />
      <bond atomRefs2='a45 a113' order='1' id='a45_a113' />
      <bond atomRefs2='a46 a48' order='1' id='a46_a48'>
        <bondStereo>W</bondStereo>
      </bond>
      <bond atomRefs2='a46 a114' order='1' id='a46_a114' />
      <bond atomRefs2='a47 a117' order='1' id='a47_a117' />
      <bond atomRefs2='a47 a118' order='1' id='a47_a118' />
      <bond atomRefs2='a47 a119' order='1' id='a47_a119' />
      <bond atomRefs2='a48 a121' order='1' id='a48_a121' />
      <bond atomRefs2='a48 a122' order='1' id='a48_a122' />
      <bond atomRefs2='a49 a50' order='2' id='a49_a50' />
      <bond atomRefs2='a49 a51' order='1' id='a49_a51' />
      <bond atomRefs2='a50 a52' order='1' id='a50_a52' />
      <bond atomRefs2='a51 a53' order='2' id='a51_a53' />
      <bond atomRefs2='a52 a54' order='2' id='a52_a54' />
      <bond atomRefs2='a52 a126' order='1' id='a52_a126' />
      <bond atomRefs2='a53 a54' order='1' id='a53_a54' />
      <bond atomRefs2='a53 a127' order='1' id='a53_a127' />
      <bond atomRefs2='a54 a55' order='1' id='a54_a55' />
      <bond atomRefs2='a55 a56' order='1' id='a55_a56' />
      <bond atomRefs2='a55 a128' order='1' id='a55_a128'>
        <bondStereo>H</bondStereo>
      </bond>
      <bond atomRefs2='a57 a62' order='2' id='a57_a62' />
      <bond atomRefs2='a57 a63' order='1' id='a57_a63' />
      <bond atomRefs2='a58 a64' order='2' id='a58_a64' />
      <bond atomRefs2='a58 a65' order='1' id='a58_a65' />
      <bond atomRefs2='a59 a61' order='1' id='a59_a61' />
      <bond atomRefs2='a60 a76' order='1' id='a60_a76' />
      <bond atomRefs2='a60 a77' order='1' id='a60_a77' />
      <bond atomRefs2='a60 a131' order='1' id='a60_a131'>
        <bondStereo>W</bondStereo>
      </bond>
      <bond atomRefs2='a61 a78' order='1' id='a61_a78'>
        <bondStereo>W</bondStereo>
      </bond>
      <bond atomRefs2='a61 a132' order='1' id='a61_a132' />
      <bond atomRefs2='a62 a72' order='1' id='a62_a72' />
      <bond atomRefs2='a63 a73' order='2' id='a63_a73' />
      <bond atomRefs2='a63 a133' order='1' id='a63_a133' />
      <bond atomRefs2='a64 a74' order='1' id='a64_a74' />
      <bond atomRefs2='a65 a75' order='2' id='a65_a75' />
      <bond atomRefs2='a65 a134' order='1' id='a65_a134' />
      <bond atomRefs2='a66 a67' order='1' id='a66_a67' />
      <bond atomRefs2='a66 a68' order='1' id='a66_a68' />
      <bond atomRefs2='a66 a136' order='1' id='a66_a136' />
      <bond atomRefs2='a67 a79' order='1' id='a67_a79' />
      <bond atomRefs2='a67 a135' order='1' id='a67_a135' />
      <bond atomRefs2='a68 a72' order='2' id='a68_a72' />
      <bond atomRefs2='a68 a73' order='1' id='a68_a73' />
      <bond atomRefs2='a69 a70' order='1' id='a69_a70' />
      <bond atomRefs2='a69 a71' order='1' id='a69_a71' />
      <bond atomRefs2='a69 a137' order='1' id='a69_a137' />
      <bond atomRefs2='a70 a74' order='2' id='a70_a74' />
      <bond atomRefs2='a70 a75' order='1' id='a70_a75' />
      <bond atomRefs2='a71 a81' order='1' id='a71_a81' />
      <bond atomRefs2='a71 a138' order='1' id='a71_a138'>
        <bondStereo>W</bondStereo>
      </bond>
      <bond atomRefs2='a72 a139' order='1' id='a72_a139' />
      <bond atomRefs2='a73 a140' order='1' id='a73_a140' />
      <bond atomRefs2='a74 a141' order='1' id='a74_a141' />
      <bond atomRefs2='a75 a142' order='1' id='a75_a142' />
      <bond atomRefs2='a76 a83' order='2' id='a76_a83' />
      <bond atomRefs2='a76 a86' order='1' id='a76_a86' />
      <bond atomRefs2='a78 a91' order='1' id='a78_a91' />
      <bond atomRefs2='a78 a144' order='1' id='a78_a144' />
      <bond atomRefs2='a78 a145' order='1' id='a78_a145' />
      <bond atomRefs2='a80 a82' order='1' id='a80_a82' />
      <bond atomRefs2='a80 a84' order='1' id='a80_a84' />
      <bond atomRefs2='a80 a148' order='1' id='a80_a148' />
      <bond atomRefs2='a82 a85' order='1' id='a82_a85' />
      <bond atomRefs2='a82 a149' order='1' id='a82_a149' />
      <bond atomRefs2='a82 a150' order='1' id='a82_a150' />
      <bond atomRefs2='a83 a87' order='1' id='a83_a87' />
      <bond atomRefs2='a83 a151' order='1' id='a83_a151' />
      <bond atomRefs2='a85 a94' order='1' id='a85_a94' />
      <bond atomRefs2='a85 a95' order='1' id='a85_a95' />
      <bond atomRefs2='a85 a152' order='1' id='a85_a152' />
      <bond atomRefs2='a86 a93' order='2' id='a86_a93' />
      <bond atomRefs2='a86 a153' order='1' id='a86_a153' />
      <bond atomRefs2='a87 a89' order='1' id='a87_a89' />
      <bond atomRefs2='a87 a92' order='2' id='a87_a92' />
      <bond atomRefs2='a88 a90' order='1' id='a88_a90' />
      <bond atomRefs2='a88 a98' order='1' id='a88_a98'>
        <bondStereo>H</bondStereo>
      </bond>
      <bond atomRefs2='a88 a154' order='1' id='a88_a154' />
      <bond atomRefs2='a89 a90' order='2' id='a89_a90' />
      <bond atomRefs2='a89 a96' order='1' id='a89_a96' />
      <bond atomRefs2='a90 a97' order='1' id='a90_a97' />
      <bond atomRefs2='a92 a93' order='1' id='a92_a93' />
      <bond atomRefs2='a93 a158' order='1' id='a93_a158' />
      <bond atomRefs2='a94 a160' order='1' id='a94_a160' />
      <bond atomRefs2='a94 a161' order='1' id='a94_a161' />
      <bond atomRefs2='a94 a162' order='1' id='a94_a162' />
      <bond atomRefs2='a95 a163' order='1' id='a95_a163' />
      <bond atomRefs2='a95 a164' order='1' id='a95_a164' />
      <bond atomRefs2='a95 a165' order='1' id='a95_a165' />
      <bond atomRefs2='a96 a101' order='2' id='a96_a101' />
      <bond atomRefs2='a97 a100' order='2' id='a97_a100' />
      <bond atomRefs2='a97 a166' order='1' id='a97_a166' />
      <bond atomRefs2='a99 a167' order='1' id='a99_a167' />
      <bond atomRefs2='a99 a168' order='1' id='a99_a168' />
      <bond atomRefs2='a99 a169' order='1' id='a99_a169' />
      <bond atomRefs2='a100 a101' order='1' id='a100_a101' />
      <bond atomRefs2='a101 a172' order='1' id='a101_a172' />
    </bondArray>
    <formula convention='pubchem:formula' inline='C66H75Cl2N9O24' />
    <formula convention='pubchem:CanonicalSmiles' inline='CC1C(C(CC(O1)OC2C(C(C(OC2OC3=C4C=C5C=C3OC6=C(C=C(C=C6)C(C(C(=O)NC(C(=O)NC5C(=O)NC7C8=CC(=C(C=C8)O)C9=C(C=C(C=C9C(NC(=O)C(C(C1=CC(=C(O4)C=C1)Cl)O)NC7=O)C(=O)O)O)O)CC(=O)N)NC(=O)C(CC(C)C)NC)O)Cl)CO)O)O)(C)N)O' />
    <formula convention='pubchem:IsomericSmiles' inline='C[C@H]1[C@H]([C@@](C[C@@H](O1)O[C@@H]2[C@H]([C@@H]([C@H](O[C@H]2OC3=C4C=C5C=C3OC6=C(C=C(C=C6)[C@H]([C@H](C(=O)N[C@H](C(=O)N[C@H]5C(=O)N[C@@H]7C8=CC(=C(C=C8)O)C9=C(C=C(C=C9[C@H](NC(=O)[C@H]([C@@H](C1=CC(=C(O4)C=C1)Cl)O)NC7=O)C(=O)O)O)O)CC(=O)N)NC(=O)[C@@H](CC(C)C)NC)O)Cl)CO)O)O)(C)N)O' />
  </molecule>
</cml>";

        public const string CHARGESPLUS =
            @"<?xml version='1.0' encoding='utf-8'?>
<cml:cml id='7749b426094446dda781a36168874942' convention='conventions:molecular' xmlns:cml='http://www.xml-cml.org/schema' xmlns:conventions='http://www.xml-cml.org/convention/' xmlns:cmlDict='http://www.xml-cml.org/dictionary/cml/' xmlns:nameDict='http://www.xml-cml.org/dictionary/cml/name/'>
  <cml:molecule id='m0'>
    <cml:atomArray>
      <cml:atom id='a0' elementType='C' x2='297' y2='120.4' />
      <cml:atom id='a1' elementType='C' x2='314.320508075689' y2='130.4' />
      <cml:atom id='a2' elementType='C' x2='314.320508075689' y2='150.4' />
      <cml:atom id='a3' elementType='C' x2='297' y2='160.4' />
      <cml:atom id='a4' elementType='C' x2='279.679491924311' y2='150.4' />
      <cml:atom id='a5' elementType='C' x2='279.679491924311' y2='130.4' />
      <cml:atom id='a6' elementType='N' x2='297' y2='100.4' formalCharge='1' />
      <cml:atom id='a7' elementType='N' x2='331.641016151378' y2='120.4' formalCharge='-1' />
      <cml:atom id='a8' elementType='N' x2='331.641016151378' y2='160.4' formalCharge='-2' />
      <cml:atom id='a9' elementType='N' x2='297' y2='180.4' formalCharge='1' />
      <cml:atom id='a10' elementType='N' x2='262.358983848623' y2='160.4' formalCharge='+1' />
      <cml:atom id='a11' elementType='B' x2='262.358983848622' y2='120.4' formalCharge='-2' />
    </cml:atomArray>
    <cml:bondArray>
      <cml:bond id='b0' atomRefs2='a0 a1' order='S' />
      <cml:bond id='b1' atomRefs2='a1 a2' order='D' />
      <cml:bond id='b2' atomRefs2='a2 a3' order='S' />
      <cml:bond id='b3' atomRefs2='a3 a4' order='D' />
      <cml:bond id='b4' atomRefs2='a4 a5' order='S' />
      <cml:bond id='b5' atomRefs2='a5 a0' order='D' />
      <cml:bond id='b6' atomRefs2='a0 a6' order='S' />
      <cml:bond id='b7' atomRefs2='a1 a7' order='S' />
      <cml:bond id='b8' atomRefs2='a2 a8' order='S' />
      <cml:bond id='b9' atomRefs2='a3 a9' order='S' />
      <cml:bond id='b10' atomRefs2='a4 a10' order='S' />
      <cml:bond id='b11' atomRefs2='a5 a11' order='S' />
    </cml:bondArray>
    <cml:formula concise='C 6 H 18 N 6' />
    <cml:name id='n1' dictRef='chemspider:Inchikey'>OWSZUKMVEBFJMZ-UHFFFAOYSA-T</cml:name>
    <cml:name id='n2' dictRef='chemspider:Synonym'>Not Found</cml:name>
  </cml:molecule>
</cml:cml>";
    }
}