using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreatorPileInMidas
{
    class DocForMidas
    {
        public PileAnalyticalScheme PileAnalyticalScheme { get; }
        public MaterialEnum Material { get; set; }
        public double Side1 { get; }
        public double Side2 { get; }

        public DocForMidas(PileAnalyticalScheme pileAnalyticalScheme, MaterialEnum material, double side1, double side2)
        {
            PileAnalyticalScheme = pileAnalyticalScheme;
            Material = material;
            Side1 = side1;
            Side2 = side2;
        }

        public string WriteDoc()
        {
            string result = WriteCommandForMidasUnit();
            result += WriteCommandForMidasNode();
            result += WriteCommandForMidasElement();
            result += WriteCommandForMidasMaterial(Material);
            result += WriteCommandForMidasCrossSection(Side1, Side2);
            result += WriteCommandForMidasSpring();
            return result;
        }

        private string WriteCommandForMidasUnit()
        {
            string s = @"*UNIT    ; Unit System
                        ; FORCE, LENGTH, HEAT, TEMPER
                        KN   , M, BTU, C";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine(s);

            return stringBuilder.ToString();
        }
        private string WriteCommandForMidasNode()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine(MidasNode.Header);

            foreach (var item in PileAnalyticalScheme.MidasNodes)
            {
                stringBuilder.AppendLine(item.ToString());
            }

            return stringBuilder.ToString();
        }

        private string WriteCommandForMidasElement()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine(MidasBeamElement.Header);

            foreach (var item in PileAnalyticalScheme.MidasBeamElements)
            {
                stringBuilder.AppendLine(item.ToString());
            }

            return stringBuilder.ToString();
        }

        private string WriteCommandForMidasMaterial(MaterialEnum materialEnum)
        {
            string s = @"*MATERIAL    ; Material
                        ; iMAT, TYPE, MNAME, SPHEAT, HEATCO, PLAST, TUNIT, bMASS, DAMPRATIO, [DATA1]           ; STEEL, CONC, USER
                        ; iMAT, TYPE, MNAME, SPHEAT, HEATCO, PLAST, TUNIT, bMASS, DAMPRATIO, [DATA2], [DATA2]  ; SRC
                        ; [DATA1] : 1, STANDARD, CODE/PRODUCT, DB, USEELAST, ELAST
                        ; [DATA1] : 2, ELAST, POISN, THERMAL, DEN, MASS
                        ; [DATA1] : 3, Ex, Ey, Ez, Tx, Ty, Tz, Sxy, Sxz, Syz, Pxy, Pxz, Pyz, DEN, MASS         ; Orthotropic
                        ; [DATA2] : 1, STANDARD, CODE/PRODUCT, DB, USEELAST, ELAST or 2, ELAST, POISN, THERMAL, DEN, MASS";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine(s);

            switch (materialEnum)
            {
                case MaterialEnum.B20:
                    stringBuilder.AppendLine("100, CONC , B20               , 0, 0, , C, NO, 0.05, 1, GOST-SNIP(RC),            , B20           , NO, 2.75323e+009");
                    break;
                case MaterialEnum.B25:
                    stringBuilder.AppendLine("100, CONC , B25               , 0, 0, , C, NO, 0.05, 1, GOST-SNIP(RC),            , B25           , NO, 3.05915e+009");
                    break;
                case MaterialEnum.B30:
                    stringBuilder.AppendLine("100, CONC , B30               , 0, 0, , C, NO, 0.05, 1, GOST-SNIP(RC),            , B30           , NO, 3.31408e+009");
                    break;
                case MaterialEnum.B35:
                    stringBuilder.AppendLine("100, CONC , B35               , 0, 0, , C, NO, 0.05, 1, GOST-SNIP(RC),            , B35           , NO, 3.51802e+006");
                    break;
                case MaterialEnum.B40:
                    stringBuilder.AppendLine("100, CONC , B40               , 0, 0, , C, NO, 0.05, 1, GOST-SNIP(RC),            , B40           , NO, 3.67098e+006");
                    break;
                default:
                    break;
            }

            return stringBuilder.ToString();
        }

        private string WriteCommandForMidasCrossSection(double dimension1, double dimension2)
        {
            string s = @"*SECTION    ; Section
                        ; iSEC, TYPE, SNAME, [OFFSET], bSD, bWE, SHAPE, [DATA1], [DATA2]                    ; 1st line - DB/USER
                        ; iSEC, TYPE, SNAME, [OFFSET], bSD, bWE, SHAPE, BLT, D1, ..., D8, iCEL              ; 1st line - VALUE
                        ;       AREA, ASy, ASz, Ixx, Iyy, Izz                                               ; 2nd line
                        ;       CyP, CyM, CzP, CzM, QyB, QzB, PERI_OUT, PERI_IN, Cy, Cz                     ; 3rd line
                        ;       Y1, Y2, Y3, Y4, Z1, Z2, Z3, Z4, Zyy, Zzz                                    ; 4th line
                        ; iSEC, TYPE, SNAME, [OFFSET], bSD, bWE, SHAPE, ELAST, DEN, POIS, POIC, SF, THERMAL ; 1st line - SRC
                        ;       D1, D2, [SRC]                                                               ; 2nd line
                        ; iSEC, TYPE, SNAME, [OFFSET], bSD, bWE, SHAPE, 1, DB, NAME1, NAME2, D1, D2         ; 1st line - COMBINED
                        ; iSEC, TYPE, SNAME, [OFFSET], bSD, bWE, SHAPE, 2, D11, D12, D13, D14, D15, D21, D22, D23, D24
                        ; iSEC, TYPE, SNAME, [OFFSET2], bSD, bWE, SHAPE, iyVAR, izVAR, STYPE                ; 1st line - TAPERED
                        ;       DB, NAME1, NAME2                                                            ; 2nd line(STYPE=DB)
                        ;       [DIM1], [DIM2]                                                              ; 2nd line(STYPE=USER)
                        ;       D11, D12, D13, D14, D15, D16, D17, D18                                      ; 2nd line(STYPE=VALUE)
                        ;       AREA1, ASy1, ASz1, Ixx1, Iyy1, Izz1                                         ; 3rd line(STYPE=VALUE)
                        ;       CyP1, CyM1, CzP1, CzM1, QyB1, QzB1, PERI_OUT1, PERI_IN1, Cy1, Cz1           ; 4th line(STYPE=VALUE)
                        ;       Y11, Y12, Y13, Y14, Z11, Z12, Z13, Z14, Zyy1, Zyy2                          ; 5th line(STYPE=VALUE)
                        ;       D21, D22, D23, D24, D25, D26, D27, D28                                      ; 6th line(STYPE=VALUE)
                        ;       AREA2, ASy2, ASz2, Ixx2, Iyy2, Izz2                                         ; 7th line(STYPE=VALUE)
                        ;       CyP2, CyM2, CzP2, CzM2, QyB2, QzB2, PERI_OUT2, PERI_IN2, Cy2, Cz2           ; 8th line(STYPE=VALUE)
                        ;       Y21, Y22, Y23, Y24, Z21, Z22, Z23, Z24, Zyy2, Zzz2                          ; 9th line(STYPE=VALUE)
                        ;       OPT1, OPT2, [JOINT]                                                         ; 2nd line(STYPE=PSC)
                        ;       ELAST, DEN, POIS, POIC, THERMAL                                             ; 2nd line(STYPE=PSC-CMPW)
                        ;       bSHEARCHK, [SCHK-I], [SCHK-J], [WT-I], [WT-J], WI, WJ, bSYM, bSIDEHOLE      ; 3rd line(STYPE=PSC)
                        ;       bSHEARCHK, bSYM, bHUNCH, [CMPWEB-I], [CMPWEB-J]                             ; 3rd line(STYPE=PSC-CMPW)
                        ;       bUSERDEFMESHSIZE, MESHSIZE, bUSERINPSTIFF, [STIFF-I], [STIFF-J]             ; 4th line(STYPE=PSC)
                        ;       [SIZE-A]-i                                                                  ; 5th line(STYPE=PSC)
                        ;       [SIZE-B]-i                                                                  ; 6th line(STYPE=PSC)
                        ;       [SIZE-C]-i                                                                  ; 7th line(STYPE=PSC)
                        ;       [SIZE-D]-i                                                                  ; 8th line(STYPE=PSC)
                        ;       [SIZE-A]-j                                                                  ; 9th line(STYPE=PSC)
                        ;       [SIZE-B]-j                                                                  ; 10th line(STYPE=PSC)
                        ;       [SIZE-C]-j                                                                  ; 11th line(STYPE=PSC)
                        ;       [SIZE-D]-j                                                                  ; 12th line(STYPE=PSC)
                        ;       GN, CTC, Bc, Tc, Hh, EsEc, DsDc, Ps, Pc, bMULTI, EsEc-L, EsEc-S             ; 2nd line(STYPE=CMP-B/I)
                        ;       SW_i, Hw_i, tw_i, B_i, Bf1_i, tf1_i, B2_i, Bf2_i, tf2_i                     ; 3rd line(STYPE=CMP-B/I)
                        ;       SW_j, Hw_j, tw_j, B_j, Bf1_j, tf1_j, B2_j, Bf2_j, tf2_j                     ; 4th line(STYPE=CMP-B/I)
                        ;       N1, N2, Hr, Hr2, tr1, tr2                                                   ; 5th line(STYPE=CMP-B)
                        ;       GN, CTC, Bc, Tc, Hh, EgdEsb, DgdDsb, Pgd, Psb, bSYM, SW_i, SW_j             ; 2nd line(STYPE=CMP-CI/CT)
                        ;       OPT1, OPT2, [JOINT]                                                         ; 3rd line(STYPE=CMP-CI/CT)
                        ;       [SIZE-A]-i                                                                  ; 4th line(STYPE=CMP-CI/CT)
                        ;       [SIZE-B]-i                                                                  ; 5th line(STYPE=CMP-CI/CT)
                        ;       [SIZE-C]-i                                                                  ; 6th line(STYPE=CMP-CI/CT)
                        ;       [SIZE-D]-i                                                                  ; 7th line(STYPE=CMP-CI/CT)
                        ;       [SIZE-A]-j                                                                  ; 8th line(STYPE=CMP-CI/CT)
                        ;       [SIZE-B]-j                                                                  ; 9th line(STYPE=CMP-CI/CT)
                        ;       [SIZE-C]-j                                                                  ; 10th line(STYPE=CMP-CI/CT)
                        ;       [SIZE-D]-j                                                                  ; 11th line(STYPE=CMP-CI/CT)
                        ; iSEC, TYPE, SNAME, [OFFSET], bSD, bWE, STYPE1, STYPE2                             ; 1st line - CONSTRUCT
                        ;       SHAPE, ...(same with other type data from shape)                            ; Before (STYPE1)
                        ;       SHAPE, ...(same with other type data from shape)                            ; After  (STYPE2)
                        ; iSEC, TYPE, SNAME, [OFFSET], bSD, bWE, SHAPE                                      ; 1st line - COMPOSITE-B
                        ;       Hw, tw, B1, Bf1, tf1, B2, Bf2, tf2                                          ; 2nd line
                        ;       [SHAPE-NUM], [STIFF-SHAPE], [STIFF-POS] (1~4)                               ; 3rd line
                        ;       SW, GN, CTC, Bc, Tc, Hh, EsEc, DsDc, Ps, Pc, TsTc, bMulti, Elong, Esh       ; 4th line
                        ; iSEC, TYPE, SNAME, [OFFSET], bSD, bWE, SHAPE                                      ; 1st line - COMPOSITE-I
                        ;       Hw, tw, B1, tf1, B2, tf2                                                    ; 2nd line
                        ;       [SHAPE-NUM], [STIFF-SHAPE], [STIFF-POS] (1~2)                               ; 3rd line
                        ;       SW, GN, CTC, Bc, Tc, Hh, EsEc, DsDc, Ps, Pc, TsTc, bMulti, Elong, Esh       ; 4th line
                        ; iSEC, TYPE, SNAME, [OFFSET], bSD, bWE, SHAPE                                      ; 1st line - COMPOSITE-TUB
                        ;       Hw, tw, B1, Bf1, tf1, B2, Bf2, tf2, Bf3, tfp                                ; 2nd line
                        ;       [SHAPE-NUM], [STIFF-SHAPE], [STIFF-POS] (1~3)                               ; 3rd line
                        ;       SW, GN, CTC, Bc, Tc, Hh, EsEc, DsDc, Ps, Pc, TsTc, bMulti, Elong, Esh       ; 4th line
                        ; iSEC, TYPE, SNAME, [OFFSET], bSD, bWE, SHAPE                                      ; 1st line - COMPOSITE-CI/CT
                        ;       OPT1, OPT2, [JOINT]                                                         ; 2nd line
                        ;       [SIZE-A]                                                                    ; 3rd line
                        ;       [SIZE-B]                                                                    ; 4th line
                        ;       [SIZE-C]                                                                    ; 5th line
                        ;       [SIZE-D]                                                                    ; 6th line
                        ;       SW, GN, CTC, Bc, Tc, Hh, EgdEsb, DgdDsb, Pgd, Psb                           ; 7th line
                        ; iSEC, TYPE, SNAME, [OFFSET], bSD, bWE, SHAPE                                      ; 1st line - PSC
                        ;       OPT1, OPT2, [JOINT]                                                         ; 2nd line
                        ;       bSHEARCHK, [SCHK], [WT], WIDTH, bSYM, bSIDEHOLE                             ; 3rd line
                        ;       bUSERDEFMESHSIZE, MESHSIZE, bUSERINPSTIFF, [STIFF]                          ; 4th line
                        ;       bWE, [WARPING POINT]-i, [WARPING POINT]-j                                   ; 5th line
                        ;       [SIZE-A]                                                                    ; 6th line
                        ;       [SIZE-B]                                                                    ; 7th line
                        ;       [SIZE-C]                                                                    ; 8th line
                        ;       [SIZE-D]                                                                    ; 9th line
                        ; [DATA1] : 1, DB, NAME or 2, D1, D2, D3, D4, D5, D6, D7, D8, D9, D10
                        ; [DATA2] : CCSHAPE or iCEL or iN1, iN2
                        ; [SRC]  : 1, DB, NAME1, NAME2 or 2, D1, D2, D3, D4, D5, D6, D7, D8, D9, D10, iN1, iN2
                        ; [DIM1], [DIM2] : D1, D2, D3, D4, D5, D6, D7, D8
                        ; [OFFSET] : OFFSET, iCENT, iREF, iHORZ, HUSER, iVERT, VUSER
                        ; [OFFSET2]: OFFSET, iCENT, iREF, iHORZ, HUSERI, HUSERJ, iVERT, VUSERI, VUSERJ
                        ; [SHAPE-NUM]: SHAPE-NUM, POS, STIFF-NUM1, STIFF-NUM2, STIFF-NUM3, STIFF-NUM4
                        ; [STIFF-SHAPE]: SHAPE-NUM, for(SHAPE-NUM) { NAME, SIZE1~8 }
                        ; [STIFF-POS]: STIFF-NUM, for(STIFF-NUM) { SPACING, iSHAPE, bCALC }
                        ; [JOINT]  :  8(1CELL, 2CELL), 13(3CELL),  9(PSCM),  8(PSCH), 9(PSCT),  2(PSCB), 0(nCELL),  2(nCEL2)
                        ; [SIZE-A] :  6(1CELL, 2CELL), 10(3CELL), 10(PSCM),  6(PSCH), 8(PSCT), 10(PSCB), 5(nCELL), 11(nCEL2)
                        ; [SIZE-B] :  6(1CELL, 2CELL), 12(3CELL),  6(PSCM),  6(PSCH), 8(PSCT),  6(PSCB), 8(nCELL), 18(nCEL2)
                        ; [SIZE-C] : 10(1CELL, 2CELL), 13(3CELL),  9(PSCM), 10(PSCH), 7(PSCT),  8(PSCB), 0(nCELL), 11(nCEL2)
                        ; [SIZE-D] :  8(1CELL, 2CELL), 13(3CELL),  6(PSCM),  7(PSCH), 8(PSCT),  5(PSCB), 0(nCELL), 18(nCEL2)
                        ; [STIFF]  : AREA, ASy, ASz, Ixx, Iyy, Izz
                        ; [SCHK]   : bAUTO_Z1, Z1, bAUTO_Z3, Z3
                        ; [WT]     : bAUTO_TOR, TOR, bAUTO_SHR1, SHR1, bAUTO_SHR2, SHR2, bAUTO_SHR3, SHR3
                        ; [CMPWEB] : EFD, LRF, A, B, H, T
                        ; [WARPING POINT] : nWarpingCheck, X1,X2,X3,X4,X5,X6, Y1,Y2,Y3,Y4,Y5,Y6";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine(s);

            switch (PileAnalyticalScheme.Pile.TypeCrossSection)
            {
                case TypeCrossSectionEnum.Круглое:
                    stringBuilder.AppendLine($"100, DBUSER    , D{dimension1}m             , CC, 0, 0, 0, 0, 0, 0, YES, NO, SR , 2, {dimension1}, 0, 0, 0, 0, 0, 0, 0, 0, 0");
                    break;
                case TypeCrossSectionEnum.Прямоугольное:
                    stringBuilder.AppendLine($"100, DBUSER    , {dimension1}x{dimension2}         , CC, 0, 0, 0, 0, 0, 0, YES, NO, SB , 2, {dimension1}, {dimension2}, 0, 0, 0, 0, 0, 0, 0, 0");
                    break;
                default:
                    break;
            }

            return stringBuilder.ToString();
        }

        private string WriteCommandForMidasSpring()
        {
            string s = @"*SPRING    ; Point Spring Supports
                        ; NODE_LIST, Type, SDx, SDy, SDz, SRx, SRy, SRz, DAMPING, Cx, Cy, Cz, CRx, CRy, CRz, GROUP, [DATA1]  ; LINEAR
                        ; NODE_LIST, Type, Direction, Vx, Vy, Vz, Stiffness, GROUP, [DATA1]                                  ; COMP, TENS
                        ; NODE_LIST, Type, Direction, Vx, Vy, Vz, FUNCTION, GROUP, [DATA1]                                   ; MULTI
                        ; [DATA1] EFFAREA, Kx, Ky, Kz";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(" ");
            stringBuilder.AppendLine(s);

            int i = 2;
            foreach (var item in PileAnalyticalScheme.SpringStiffnesHoriz)
            {
                stringBuilder.AppendLine($"{i}, LINEAR, {item.RX}, {item.RY}, 0, 0, 0, 0, NO, 0, 0, 0, 0, 0, 0, , 0, 0, 0, 0, 0");
                i++;
            }
            stringBuilder.AppendLine($"{i}, LINEAR, 0, 0, {PileAnalyticalScheme.SpringStiffnesVert.Rz}, 0, 0, 0, NO, 0, 0, 0, 0, 0, 0, , 0, 0, 0, 0, 0");

            return stringBuilder.ToString();
        }
    }
}
