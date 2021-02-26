using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class MidasBeamElement
    {
        public static string Header = @"*ELEMENT    ; Elements
; iEL, TYPE, iMAT, iPRO, iN1, iN2, ANGLE, iSUB, EXVAL, iOPT(EXVAL2); Frame Element
; iEL, TYPE, iMAT, iPRO, iN1, iN2, ANGLE, iSUB, EXVAL, EXVAL2, bLMT ; Comp/Tens Truss
; iEL, TYPE, iMAT, iPRO, iN1, iN2, iN3, iN4, iSUB, iWID             ; Planar Element
; iEL, TYPE, iMAT, iPRO, iN1, iN2, iN3, iN4, iN5, iN6, iN7, iN8     ; Solid Element
; iEL, TYPE, iMAT, iPRO, iN1, iN2, REF, RPX, RPY, RPZ, iSUB, EXVAL  ; Frame(Ref.Point)";
        public int Number { get; }
        public string Type { get; } = "BEAM";
        public int IDMaterial { get; }
        public int IDCrossSection { get; }
        public int NumberNodeStart { get; }
        public int NumberNodeFinish { get; }
        public double Angle { get; set; } = 0;

        public MidasBeamElement(int number, int iDMaterial, int iDCrossSection, int numberNodeStart, int numberNodeFinish)
        {
            Number = number;            
            IDMaterial = iDMaterial;
            IDCrossSection = iDCrossSection;
            NumberNodeStart = numberNodeStart;
            NumberNodeFinish = numberNodeFinish;
        }

        public override string ToString()
        {
            return $"{Number},{Type },{IDMaterial},{IDCrossSection},{NumberNodeStart},{NumberNodeFinish},{Angle}";
        }
    }
}
