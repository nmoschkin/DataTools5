// ************************************************* ''
// DataTools C# Native Utility Library For Windows - Interop
//
// Module: Printers
//         Windows Printer Api
//
//         Enums are documented in part from the API documentation at MSDN.
//         Other knowledge and references obtained through various sources
//         and all is considered public domain/common knowledge.
//
// Copyright (C) 2011-2020 Nathan Moschkin
// All Rights Reserved
//
// Licensed Under the Microsoft Public License   
// ************************************************* ''


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using DataTools.Text;
using DataTools.MathTools;
using DataTools.Win32;
using DataTools.Desktop.Unified;
using DataTools.Win32.Memory;

namespace DataTools.Hardware.Printers
{
    public static class PrinterModule
    {

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        // define DM_ORIENTATION          0x00000001L
        // #define DM_PAPERSIZE            0x00000002L
        // #define DM_PAPERLENGTH          0x00000004L
        // #define DM_PAPERWIDTH           0x00000008L
        // #define DM_SCALE                0x00000010L
        // #if(WINVER >= 0x0500)
        // #define DM_POSITION             0x00000020L
        // #define DM_NUP                  0x00000040L
        // #endif /* WINVER >= 0x0500 */
        // #if(WINVER >= 0x0501)
        // #define DM_DISPLAYORIENTATION   0x00000080L
        // #endif /* WINVER >= 0x0501 */
        // #define DM_COPIES               0x00000100L
        // #define DM_DEFAULTSOURCE        0x00000200L
        // #define DM_PRINTQUALITY         0x00000400L
        // #define DM_COLOR                0x00000800L
        // #define DM_DUPLEX               0x00001000L
        // #define DM_YRESOLUTION          0x00002000L
        // #define DM_TTOPTION             0x00004000L
        // #define DM_COLLATE              0x00008000L
        // #define DM_FORMNAME             0x00010000L
        // #define DM_LOGPIXELS            0x00020000L
        // #define DM_BITSPERPEL           0x00040000L
        // #define DM_PELSWIDTH            0x00080000L
        // #define DM_PELSHEIGHT           0x00100000L
        // #define DM_DISPLAYFLAGS         0x00200000L
        // #define DM_DISPLAYFREQUENCY     0x00400000L
        // #if(WINVER >= 0x0400)
        // #define DM_ICMMETHOD            0x00800000L
        // #define DM_ICMINTENT            0x01000000L
        // #define DM_MEDIATYPE            0x02000000L
        // #define DM_DITHERTYPE           0x04000000L
        // #define DM_PANNINGWIDTH         0x08000000L
        // #define DM_PANNINGHEIGHT        0x10000000L
        // #endif /* WINVER >= 0x0400 */
        // #if(WINVER >= 0x0501)
        // #define DM_DISPLAYFIXEDOUTPUT   0x20000000L
        // #endif /* WINVER >= 0x0501 */

        // /* orientation selections */
        // #define DMORIENT_PORTRAIT   1
        // #define DMORIENT_LANDSCAPE  2

        // /* paper selections */
        // #define DMPAPER_FIRST                DMPAPER_LETTER
        // #define DMPAPER_LETTER               1  /* Letter 8 1/2 x 11 in               */
        // #define DMPAPER_LETTERSMALL          2  /* Letter Small 8 1/2 x 11 in         */
        // #define DMPAPER_TABLOID              3  /* Tabloid 11 x 17 in                 */
        // #define DMPAPER_LEDGER               4  /* Ledger 17 x 11 in                  */
        // #define DMPAPER_LEGAL                5  /* Legal 8 1/2 x 14 in                */
        // #define DMPAPER_STATEMENT            6  /* Statement 5 1/2 x 8 1/2 in         */
        // #define DMPAPER_EXECUTIVE            7  /* Executive 7 1/4 x 10 1/2 in        */
        // #define DMPAPER_A3                   8  /* A3 297 x 420 mm                    */
        // #define DMPAPER_A4                   9  /* A4 210 x 297 mm                    */
        // #define DMPAPER_A4SMALL             10  /* A4 Small 210 x 297 mm              */
        // #define DMPAPER_A5                  11  /* A5 148 x 210 mm                    */
        // #define DMPAPER_B4                  12  /* B4 (JIS) 250 x 354                 */
        // #define DMPAPER_B5                  13  /* B5 (JIS) 182 x 257 mm              */
        // #define DMPAPER_FOLIO               14  /* Folio 8 1/2 x 13 in                */
        // #define DMPAPER_QUARTO              15  /* Quarto 215 x 275 mm                */
        // #define DMPAPER_10X14               16  /* 10x14 in                           */
        // #define DMPAPER_11X17               17  /* 11x17 in                           */
        // #define DMPAPER_NOTE                18  /* Note 8 1/2 x 11 in                 */
        // #define DMPAPER_ENV_9               19  /* Envelope #9 3 7/8 x 8 7/8          */
        // #define DMPAPER_ENV_10              20  /* Envelope #10 4 1/8 x 9 1/2         */
        // #define DMPAPER_ENV_11              21  /* Envelope #11 4 1/2 x 10 3/8        */
        // #define DMPAPER_ENV_12              22  /* Envelope #12 4 \276 x 11           */
        // #define DMPAPER_ENV_14              23  /* Envelope #14 5 x 11 1/2            */
        // #define DMPAPER_CSHEET              24  /* C size sheet                       */
        // #define DMPAPER_DSHEET              25  /* D size sheet                       */
        // #define DMPAPER_ESHEET              26  /* E size sheet                       */
        // #define DMPAPER_ENV_DL              27  /* Envelope DL 110 x 220mm            */
        // #define DMPAPER_ENV_C5              28  /* Envelope C5 162 x 229 mm           */
        // #define DMPAPER_ENV_C3              29  /* Envelope C3  324 x 458 mm          */
        // #define DMPAPER_ENV_C4              30  /* Envelope C4  229 x 324 mm          */
        // #define DMPAPER_ENV_C6              31  /* Envelope C6  114 x 162 mm          */
        // #define DMPAPER_ENV_C65             32  /* Envelope C65 114 x 229 mm          */
        // #define DMPAPER_ENV_B4              33  /* Envelope B4  250 x 353 mm          */
        // #define DMPAPER_ENV_B5              34  /* Envelope B5  176 x 250 mm          */
        // #define DMPAPER_ENV_B6              35  /* Envelope B6  176 x 125 mm          */
        // #define DMPAPER_ENV_ITALY           36  /* Envelope 110 x 230 mm              */
        // #define DMPAPER_ENV_MONARCH         37  /* Envelope Monarch 3.875 x 7.5 in    */
        // #define DMPAPER_ENV_PERSONAL        38  /* 6 3/4 Envelope 3 5/8 x 6 1/2 in    */
        // #define DMPAPER_FANFOLD_US          39  /* US Std Fanfold 14 7/8 x 11 in      */
        // #define DMPAPER_FANFOLD_STD_GERMAN  40  /* German Std Fanfold 8 1/2 x 12 in   */
        // #define DMPAPER_FANFOLD_LGL_GERMAN  41  /* German Legal Fanfold 8 1/2 x 13 in */
        // #if(WINVER >= 0x0400)
        // #define DMPAPER_ISO_B4              42  /* B4 (ISO) 250 x 353 mm              */
        // #define DMPAPER_JAPANESE_POSTCARD   43  /* Japanese Postcard 100 x 148 mm     */
        // #define DMPAPER_9X11                44  /* 9 x 11 in                          */
        // #define DMPAPER_10X11               45  /* 10 x 11 in                         */
        // #define DMPAPER_15X11               46  /* 15 x 11 in                         */
        // #define DMPAPER_ENV_INVITE          47  /* Envelope Invite 220 x 220 mm       */
        // #define DMPAPER_RESERVED_48         48  /* RESERVED--DO NOT USE               */
        // #define DMPAPER_RESERVED_49         49  /* RESERVED--DO NOT USE               */
        // #define DMPAPER_LETTER_EXTRA        50  /* Letter Extra 9 \275 x 12 in        */
        // #define DMPAPER_LEGAL_EXTRA         51  /* Legal Extra 9 \275 x 15 in         */
        // #define DMPAPER_TABLOID_EXTRA       52  /* Tabloid Extra 11.69 x 18 in        */
        // #define DMPAPER_A4_EXTRA            53  /* A4 Extra 9.27 x 12.69 in           */
        // #define DMPAPER_LETTER_TRANSVERSE   54  /* Letter Transverse 8 \275 x 11 in   */
        // #define DMPAPER_A4_TRANSVERSE       55  /* A4 Transverse 210 x 297 mm         */
        // #define DMPAPER_LETTER_EXTRA_TRANSVERSE 56 /* Letter Extra Transverse 9\275 x 12 in */
        // #define DMPAPER_A_PLUS              57  /* SuperA/SuperA/A4 227 x 356 mm      */
        // #define DMPAPER_B_PLUS              58  /* SuperB/SuperB/A3 305 x 487 mm      */
        // #define DMPAPER_LETTER_PLUS         59  /* Letter Plus 8.5 x 12.69 in         */
        // #define DMPAPER_A4_PLUS             60  /* A4 Plus 210 x 330 mm               */
        // #define DMPAPER_A5_TRANSVERSE       61  /* A5 Transverse 148 x 210 mm         */
        // #define DMPAPER_B5_TRANSVERSE       62  /* B5 (JIS) Transverse 182 x 257 mm   */
        // #define DMPAPER_A3_EXTRA            63  /* A3 Extra 322 x 445 mm              */
        // #define DMPAPER_A5_EXTRA            64  /* A5 Extra 174 x 235 mm              */
        // #define DMPAPER_B5_EXTRA            65  /* B5 (ISO) Extra 201 x 276 mm        */
        // #define DMPAPER_A2                  66  /* A2 420 x 594 mm                    */
        // #define DMPAPER_A3_TRANSVERSE       67  /* A3 Transverse 297 x 420 mm         */
        // #define DMPAPER_A3_EXTRA_TRANSVERSE 68  /* A3 Extra Transverse 322 x 445 mm   */
        // #endif /* WINVER >= 0x0400 */

        // #if(WINVER >= 0x0500)
        // #define DMPAPER_DBL_JAPANESE_POSTCARD 69 /* Japanese Double Postcard 200 x 148 mm */
        // #define DMPAPER_A6                  70  /* A6 105 x 148 mm                 */
        // #define DMPAPER_JENV_KAKU2          71  /* Japanese Envelope Kaku #2       */
        // #define DMPAPER_JENV_KAKU3          72  /* Japanese Envelope Kaku #3       */
        // #define DMPAPER_JENV_CHOU3          73  /* Japanese Envelope Chou #3       */
        // #define DMPAPER_JENV_CHOU4          74  /* Japanese Envelope Chou #4       */
        // #define DMPAPER_LETTER_ROTATED      75  /* Letter Rotated 11 x 8 1/2 11 in */
        // #define DMPAPER_A3_ROTATED          76  /* A3 Rotated 420 x 297 mm         */
        // #define DMPAPER_A4_ROTATED          77  /* A4 Rotated 297 x 210 mm         */
        // #define DMPAPER_A5_ROTATED          78  /* A5 Rotated 210 x 148 mm         */
        // #define DMPAPER_B4_JIS_ROTATED      79  /* B4 (JIS) Rotated 364 x 257 mm   */
        // #define DMPAPER_B5_JIS_ROTATED      80  /* B5 (JIS) Rotated 257 x 182 mm   */
        // #define DMPAPER_JAPANESE_POSTCARD_ROTATED 81 /* Japanese Postcard Rotated 148 x 100 mm */
        // #define DMPAPER_DBL_JAPANESE_POSTCARD_ROTATED 82 /* Double Japanese Postcard Rotated 148 x 200 mm */
        // #define DMPAPER_A6_ROTATED          83  /* A6 Rotated 148 x 105 mm         */
        // #define DMPAPER_JENV_KAKU2_ROTATED  84  /* Japanese Envelope Kaku #2 Rotated */
        // #define DMPAPER_JENV_KAKU3_ROTATED  85  /* Japanese Envelope Kaku #3 Rotated */
        // #define DMPAPER_JENV_CHOU3_ROTATED  86  /* Japanese Envelope Chou #3 Rotated */
        // #define DMPAPER_JENV_CHOU4_ROTATED  87  /* Japanese Envelope Chou #4 Rotated */
        // #define DMPAPER_B6_JIS              88  /* B6 (JIS) 128 x 182 mm           */
        // #define DMPAPER_B6_JIS_ROTATED      89  /* B6 (JIS) Rotated 182 x 128 mm   */
        // #define DMPAPER_12X11               90  /* 12 x 11 in                      */
        // #define DMPAPER_JENV_YOU4           91  /* Japanese Envelope You #4        */
        // #define DMPAPER_JENV_YOU4_ROTATED   92  /* Japanese Envelope You #4 Rotated*/
        // #define DMPAPER_P16K                93  /* PRC 16K 146 x 215 mm            */
        // #define DMPAPER_P32K                94  /* PRC 32K 97 x 151 mm             */
        // #define DMPAPER_P32KBIG             95  /* PRC 32K(Big) 97 x 151 mm        */
        // #define DMPAPER_PENV_1              96  /* PRC Envelope #1 102 x 165 mm    */
        // #define DMPAPER_PENV_2              97  /* PRC Envelope #2 102 x 176 mm    */
        // #define DMPAPER_PENV_3              98  /* PRC Envelope #3 125 x 176 mm    */
        // #define DMPAPER_PENV_4              99  /* PRC Envelope #4 110 x 208 mm    */
        // #define DMPAPER_PENV_5              100 /* PRC Envelope #5 110 x 220 mm    */
        // #define DMPAPER_PENV_6              101 /* PRC Envelope #6 120 x 230 mm    */
        // #define DMPAPER_PENV_7              102 /* PRC Envelope #7 160 x 230 mm    */
        // #define DMPAPER_PENV_8              103 /* PRC Envelope #8 120 x 309 mm    */
        // #define DMPAPER_PENV_9              104 /* PRC Envelope #9 229 x 324 mm    */
        // #define DMPAPER_PENV_10             105 /* PRC Envelope #10 324 x 458 mm   */
        // #define DMPAPER_P16K_ROTATED        106 /* PRC 16K Rotated                 */
        // #define DMPAPER_P32K_ROTATED        107 /* PRC 32K Rotated                 */
        // #define DMPAPER_P32KBIG_ROTATED     108 /* PRC 32K(Big) Rotated            */
        // #define DMPAPER_PENV_1_ROTATED      109 /* PRC Envelope #1 Rotated 165 x 102 mm */
        // #define DMPAPER_PENV_2_ROTATED      110 /* PRC Envelope #2 Rotated 176 x 102 mm */
        // #define DMPAPER_PENV_3_ROTATED      111 /* PRC Envelope #3 Rotated 176 x 125 mm */
        // #define DMPAPER_PENV_4_ROTATED      112 /* PRC Envelope #4 Rotated 208 x 110 mm */
        // #define DMPAPER_PENV_5_ROTATED      113 /* PRC Envelope #5 Rotated 220 x 110 mm */
        // #define DMPAPER_PENV_6_ROTATED      114 /* PRC Envelope #6 Rotated 230 x 120 mm */
        // #define DMPAPER_PENV_7_ROTATED      115 /* PRC Envelope #7 Rotated 230 x 160 mm */
        // #define DMPAPER_PENV_8_ROTATED      116 /* PRC Envelope #8 Rotated 309 x 120 mm */
        // #define DMPAPER_PENV_9_ROTATED      117 /* PRC Envelope #9 Rotated 324 x 229 mm */
        // #define DMPAPER_PENV_10_ROTATED     118 /* PRC Envelope #10 Rotated 458 x 324 mm */
        // #endif /* WINVER >= 0x0500 */

        // #if (WINVER >= 0x0500)
        // #define DMPAPER_LAST                DMPAPER_PENV_10_ROTATED
        // #elif (WINVER >= 0x0400)
        // #define DMPAPER_LAST                DMPAPER_A3_EXTRA_TRANSVERSE
        // #Else
        // #define DMPAPER_LAST                DMPAPER_FANFOLD_LGL_GERMAN
        // #End If

        // #define DMPAPER_USER                256

        // /* bin selections */
        // #define DMBIN_FIRST         DMBIN_UPPER
        // #define DMBIN_UPPER         1
        // #define DMBIN_ONLYONE       1
        // #define DMBIN_LOWER         2
        // #define DMBIN_MIDDLE        3
        // #define DMBIN_MANUAL        4
        // #define DMBIN_ENVELOPE      5
        // #define DMBIN_ENVMANUAL     6
        // #define DMBIN_AUTO          7
        // #define DMBIN_TRACTOR       8
        // #define DMBIN_SMALLFMT      9
        // #define DMBIN_LARGEFMT      10
        // #define DMBIN_LARGECAPACITY 11
        // #define DMBIN_CASSETTE      14
        // #define DMBIN_FORMSOURCE    15
        // #define DMBIN_LAST          DMBIN_FORMSOURCE

        // #define DMBIN_USER          256     /* device specific bins start here */

        // /* print qualities */
        // #define DMRES_DRAFT         (-1)
        // #define DMRES_LOW           (-2)
        // #define DMRES_MEDIUM        (-3)
        // #define DMRES_HIGH          (-4)

        // /* color enable/disable for color printers */
        // #define DMCOLOR_MONOCHROME  1
        // #define DMCOLOR_COLOR       2

        // /* duplex enable */
        // #define DMDUP_SIMPLEX    1
        // #define DMDUP_VERTICAL   2
        // #define DMDUP_HORIZONTAL 3

        // /* TrueType options */
        // #define DMTT_BITMAP     1       /* print TT fonts as graphics */
        // #define DMTT_DOWNLOAD   2       /* download TT fonts as soft fonts */
        // #define DMTT_SUBDEV     3       /* substitute device fonts for TT fonts */
        // #if(WINVER >= 0x0400)
        // #define DMTT_DOWNLOAD_OUTLINE 4 /* download TT fonts as outline soft fonts */
        // #endif /* WINVER >= 0x0400 */

        // /* Collation selections */
        // #define DMCOLLATE_FALSE  0
        // #define DMCOLLATE_TRUE   1

        // #if(WINVER >= 0x0501)
        // /* DEVMODE dmDisplayOrientation specifiations */
        // #define DMDO_DEFAULT    0
        // #define DMDO_90         1
        // #define DMDO_180        2
        // #define DMDO_270        3

        // /* DEVMODE dmDisplayFixedOutput specifiations */
        // #define DMDFO_DEFAULT   0
        // #define DMDFO_STRETCH   1
        // #define DMDFO_CENTER    2
        // #endif /* WINVER >= 0x0501 */

        // /* DEVMODE dmDisplayFlags flags */

        // // #define DM_GRAYSCALE            0x00000001 /* This flag is no longer valid */
        // #define DM_INTERLACED           0x00000002
        // #define DMDISPLAYFLAGS_TEXTMODE 0x00000004

        // /* dmNup , multiple logical page per physical page options */
        // #define DMNUP_SYSTEM        1
        // #define DMNUP_ONEUP         2

        // #if(WINVER >= 0x0400)
        // /* ICM methods */
        // #define DMICMMETHOD_NONE    1   /* ICM disabled */
        // #define DMICMMETHOD_SYSTEM  2   /* ICM handled by system */
        // #define DMICMMETHOD_DRIVER  3   /* ICM handled by driver */
        // #define DMICMMETHOD_DEVICE  4   /* ICM handled by device */

        // #define DMICMMETHOD_USER  256   /* Device-specific methods start here */

        // /* ICM Intents */
        // #define DMICM_SATURATE          1   /* Maximize color saturation */
        // #define DMICM_CONTRAST          2   /* Maximize color contrast */
        // #define DMICM_COLORIMETRIC       3   /* Use specific color metric */
        // #define DMICM_ABS_COLORIMETRIC   4   /* Use specific color metric */

        // #define DMICM_USER        256   /* Device-specific intents start here */

        // /* Media types */

        // #define DMMEDIA_STANDARD      1   /* Standard paper */
        // #define DMMEDIA_TRANSPARENCY  2   /* Transparency */
        // #define DMMEDIA_GLOSSY        3   /* Glossy paper */

        // #define DMMEDIA_USER        256   /* Device-specific media start here */

        // /* Dither types */
        // #define DMDITHER_NONE       1      /* No dithering */
        // #define DMDITHER_COARSE     2      /* Dither with a coarse brush */
        // #define DMDITHER_FINE       3      /* Dither with a fine brush */
        // #define DMDITHER_LINEART    4      /* LineArt dithering */
        // #define DMDITHER_ERRORDIFFUSION 5  /* LineArt dithering */
        // #define DMDITHER_RESERVED6      6      /* LineArt dithering */
        // #define DMDITHER_RESERVED7      7      /* LineArt dithering */
        // #define DMDITHER_RESERVED8      8      /* LineArt dithering */
        // #define DMDITHER_RESERVED9      9      /* LineArt dithering */
        // #define DMDITHER_GRAYSCALE  10     /* Device does grayscaling */

        // #define DMDITHER_USER     256   /* Device-specific dithers start here */
        // #

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        // DevMode extras

        public const int DMRES_DRAFT = -1;
        public const int DMRES_LOW = -2;
        public const int DMRES_MEDIUM = -3;
        public const int DMRES_HIGH = -4;

        //  field selection bits 
        public const int DM_ORIENTATION = 0x1;
        public const int DM_PAPERSIZE = 0x2;
        public const int DM_PAPERLENGTH = 0x4;
        public const int DM_PAPERWIDTH = 0x8;
        public const int DM_SCALE = 0x10;
        // if(WINVER >= 0x0500)
        public const int DM_POSITION = 0x20;
        public const int DM_NUP = 0x40;
        // endif ''  WINVER >= 0x0500 
        // if(WINVER >= 0x0501)
        public const int DM_DISPLAYORIENTATION = 0x80;
        // endif ''  WINVER >= 0x0501 
        public const int DM_COPIES = 0x100;
        public const int DM_DEFAULTSOURCE = 0x200;
        public const int DM_PRINTQUALITY = 0x400;
        public const int DM_COLOR = 0x800;
        public const int DM_DUPLEX = 0x1000;
        public const int DM_YRESOLUTION = 0x2000;
        public const int DM_TTOPTION = 0x4000;
        public const int DM_COLLATE = 0x8000;
        public const int DM_FORMNAME = 0x10000;
        public const int DM_LOGPIXELS = 0x20000;
        public const int DM_BITSPERPEL = 0x40000;
        public const int DM_PELSWIDTH = 0x80000;
        public const int DM_PELSHEIGHT = 0x100000;
        public const int DM_DISPLAYFLAGS = 0x200000;
        public const int DM_DISPLAYFREQUENCY = 0x400000;
        // if(WINVER >= 0x0400)
        public const int DM_ICMMETHOD = 0x800000;
        public const int DM_ICMINTENT = 0x1000000;
        public const int DM_MEDIATYPE = 0x2000000;
        public const int DM_DITHERTYPE = 0x4000000;
        public const int DM_PANNINGWIDTH = 0x8000000;
        public const int DM_PANNINGHEIGHT = 0x10000000;
        // endif ''  WINVER >= 0x0400 
        // if(WINVER >= 0x0501)
        public const int DM_DISPLAYFIXEDOUTPUT = 0x20000000;
        // endif ''  WINVER >= 0x0501 


        //  orientation selections 
        public const int DMORIENT_PORTRAIT = 1;
        public const int DMORIENT_LANDSCAPE = 2; //  paper selections 
        public const int DMPAPER_FIRST = DMPAPER_LETTER;
        public const int DMPAPER_LETTER = 1; //  Letter 8 1/2 x 11 in               
        public const int DMPAPER_LETTERSMALL = 2; //  Letter Small 8 1/2 x 11 in         
        public const int DMPAPER_TABLOID = 3; //  Tabloid 11 x 17 in                 
        public const int DMPAPER_LEDGER = 4; //  Ledger 17 x 11 in                  
        public const int DMPAPER_LEGAL = 5; //  Legal 8 1/2 x 14 in                
        public const int DMPAPER_STATEMENT = 6; //  Statement 5 1/2 x 8 1/2 in         
        public const int DMPAPER_EXECUTIVE = 7; //  Executive 7 1/4 x 10 1/2 in        
        public const int DMPAPER_A3 = 8; //  A3 297 x 420 mm                    
        public const int DMPAPER_A4 = 9; //  A4 210 x 297 mm                    
        public const int DMPAPER_A4SMALL = 10; //  A4 Small 210 x 297 mm              
        public const int DMPAPER_A5 = 11; //  A5 148 x 210 mm                    
        public const int DMPAPER_B4 = 12; //  B4 (JIS) 250 x 354                 
        public const int DMPAPER_B5 = 13; //  B5 (JIS) 182 x 257 mm              
        public const int DMPAPER_FOLIO = 14; //  Folio 8 1/2 x 13 in                
        public const int DMPAPER_QUARTO = 15; //  Quarto 215 x 275 mm                
        public const int DMPAPER_10X14 = 16; //  10x14 in                           
        public const int DMPAPER_11X17 = 17; //  11x17 in                           
        public const int DMPAPER_NOTE = 18; //  Note 8 1/2 x 11 in                 
        public const int DMPAPER_ENV_9 = 19; //  Envelope #9 3 7/8 x 8 7/8          
        public const int DMPAPER_ENV_10 = 20; //  Envelope #10 4 1/8 x 9 1/2         
        public const int DMPAPER_ENV_11 = 21; //  Envelope #11 4 1/2 x 10 3/8        
        public const int DMPAPER_ENV_12 = 22; //  Envelope #12 4 \276 x 11           
        public const int DMPAPER_ENV_14 = 23; //  Envelope #14 5 x 11 1/2            
        public const int DMPAPER_CSHEET = 24; //  C size sheet                       
        public const int DMPAPER_DSHEET = 25; //  D size sheet                       
        public const int DMPAPER_ESHEET = 26; //  E size sheet                       
        public const int DMPAPER_ENV_DL = 27; //  Envelope DL 110 x 220mm            
        public const int DMPAPER_ENV_C5 = 28; //  Envelope C5 162 x 229 mm           
        public const int DMPAPER_ENV_C3 = 29; //  Envelope C3  324 x 458 mm          
        public const int DMPAPER_ENV_C4 = 30; //  Envelope C4  229 x 324 mm          
        public const int DMPAPER_ENV_C6 = 31; //  Envelope C6  114 x 162 mm          
        public const int DMPAPER_ENV_C65 = 32; //  Envelope C65 114 x 229 mm          
        public const int DMPAPER_ENV_B4 = 33; //  Envelope B4  250 x 353 mm          
        public const int DMPAPER_ENV_B5 = 34; //  Envelope B5  176 x 250 mm          
        public const int DMPAPER_ENV_B6 = 35; //  Envelope B6  176 x 125 mm          
        public const int DMPAPER_ENV_ITALY = 36; //  Envelope 110 x 230 mm              
        public const int DMPAPER_ENV_MONARCH = 37; //  Envelope Monarch 3.875 x 7.5 in    
        public const int DMPAPER_ENV_PERSONAL = 38; //  6 3/4 Envelope 3 5/8 x 6 1/2 in    
        public const int DMPAPER_FANFOLD_US = 39; //  US Std Fanfold 14 7/8 x 11 in      
        public const int DMPAPER_FANFOLD_STD_GERMAN = 40; //  German Std Fanfold 8 1/2 x 12 in   
        public const int DMPAPER_FANFOLD_LGL_GERMAN = 41; //  German Legal Fanfold 8 1/2 x 13 in 
        // if(WINVER >= 0x0400)
        public const int DMPAPER_ISO_B4 = 42; //  B4 (ISO) 250 x 353 mm              
        public const int DMPAPER_JAPANESE_POSTCARD = 43; //  Japanese Postcard 100 x 148 mm     
        public const int DMPAPER_9X11 = 44; //  9 x 11 in                          
        public const int DMPAPER_10X11 = 45; //  10 x 11 in                         
        public const int DMPAPER_15X11 = 46; //  15 x 11 in                         
        public const int DMPAPER_ENV_INVITE = 47; //  Envelope Invite 220 x 220 mm       
        public const int DMPAPER_RESERVED_48 = 48; //  RESERVED--DO NOT USE               
        public const int DMPAPER_RESERVED_49 = 49; //  RESERVED--DO NOT USE               
        public const int DMPAPER_LETTER_EXTRA = 50; //  Letter Extra 9 \275 x 12 in        
        public const int DMPAPER_LEGAL_EXTRA = 51; //  Legal Extra 9 \275 x 15 in         
        public const int DMPAPER_TABLOID_EXTRA = 52; //  Tabloid Extra 11.69 x 18 in        
        public const int DMPAPER_A4_EXTRA = 53; //  A4 Extra 9.27 x 12.69 in           
        public const int DMPAPER_LETTER_TRANSVERSE = 54; //  Letter Transverse 8 \275 x 11 in   
        public const int DMPAPER_A4_TRANSVERSE = 55; //  A4 Transverse 210 x 297 mm         
        public const int DMPAPER_LETTER_EXTRA_TRANSVERSE = 56; //  Letter Extra Transverse 9\275 x 12 in 
        public const int DMPAPER_A_PLUS = 57; //  SuperA/SuperA/A4 227 x 356 mm      
        public const int DMPAPER_B_PLUS = 58; //  SuperB/SuperB/A3 305 x 487 mm      
        public const int DMPAPER_LETTER_PLUS = 59; //  Letter Plus 8.5 x 12.69 in         
        public const int DMPAPER_A4_PLUS = 60; //  A4 Plus 210 x 330 mm               
        public const int DMPAPER_A5_TRANSVERSE = 61; //  A5 Transverse 148 x 210 mm         
        public const int DMPAPER_B5_TRANSVERSE = 62; //  B5 (JIS) Transverse 182 x 257 mm   
        public const int DMPAPER_A3_EXTRA = 63; //  A3 Extra 322 x 445 mm              
        public const int DMPAPER_A5_EXTRA = 64; //  A5 Extra 174 x 235 mm              
        public const int DMPAPER_B5_EXTRA = 65; //  B5 (ISO) Extra 201 x 276 mm        
        public const int DMPAPER_A2 = 66; //  A2 420 x 594 mm                    
        public const int DMPAPER_A3_TRANSVERSE = 67; //  A3 Transverse 297 x 420 mm         
        public const int DMPAPER_A3_EXTRA_TRANSVERSE = 68; //  A3 Extra Transverse 322 x 445 mm   
        // endif ''  WINVER >= 0x0400 

        // if(WINVER >= 0x0500)
        public const int DMPAPER_DBL_JAPANESE_POSTCARD = 69; //  Japanese Double Postcard 200 x 148 mm 
        public const int DMPAPER_A6 = 70; //  A6 105 x 148 mm                 
        public const int DMPAPER_JENV_KAKU2 = 71; //  Japanese Envelope Kaku #2       
        public const int DMPAPER_JENV_KAKU3 = 72; //  Japanese Envelope Kaku #3       
        public const int DMPAPER_JENV_CHOU3 = 73; //  Japanese Envelope Chou #3       
        public const int DMPAPER_JENV_CHOU4 = 74; //  Japanese Envelope Chou #4       
        public const int DMPAPER_LETTER_ROTATED = 75; //  Letter Rotated 11 x 8 1/2 11 in 
        public const int DMPAPER_A3_ROTATED = 76; //  A3 Rotated 420 x 297 mm         
        public const int DMPAPER_A4_ROTATED = 77; //  A4 Rotated 297 x 210 mm         
        public const int DMPAPER_A5_ROTATED = 78; //  A5 Rotated 210 x 148 mm         
        public const int DMPAPER_B4_JIS_ROTATED = 79; //  B4 (JIS) Rotated 364 x 257 mm   
        public const int DMPAPER_B5_JIS_ROTATED = 80; //  B5 (JIS) Rotated 257 x 182 mm   
        public const int DMPAPER_JAPANESE_POSTCARD_ROTATED = 81; //  Japanese Postcard Rotated 148 x 100 mm 
        public const int DMPAPER_DBL_JAPANESE_POSTCARD_ROTATED = 82; //  Double Japanese Postcard Rotated 148 x 200 mm 
        public const int DMPAPER_A6_ROTATED = 83; //  A6 Rotated 148 x 105 mm         
        public const int DMPAPER_JENV_KAKU2_ROTATED = 84; //  Japanese Envelope Kaku #2 Rotated 
        public const int DMPAPER_JENV_KAKU3_ROTATED = 85; //  Japanese Envelope Kaku #3 Rotated 
        public const int DMPAPER_JENV_CHOU3_ROTATED = 86; //  Japanese Envelope Chou #3 Rotated 
        public const int DMPAPER_JENV_CHOU4_ROTATED = 87; //  Japanese Envelope Chou #4 Rotated 
        public const int DMPAPER_B6_JIS = 88; //  B6 (JIS) 128 x 182 mm           
        public const int DMPAPER_B6_JIS_ROTATED = 89; //  B6 (JIS) Rotated 182 x 128 mm   
        public const int DMPAPER_12X11 = 90; //  12 x 11 in                      
        public const int DMPAPER_JENV_YOU4 = 91; //  Japanese Envelope You #4        
        public const int DMPAPER_JENV_YOU4_ROTATED = 92; //  Japanese Envelope You #4 Rotated
        public const int DMPAPER_P16K = 93; //  PRC 16K 146 x 215 mm            
        public const int DMPAPER_P32K = 94; //  PRC 32K 97 x 151 mm             
        public const int DMPAPER_P32KBIG = 95; //  PRC 32K(Big) 97 x 151 mm        
        public const int DMPAPER_PENV_1 = 96; //  PRC Envelope #1 102 x 165 mm    
        public const int DMPAPER_PENV_2 = 97; //  PRC Envelope #2 102 x 176 mm    
        public const int DMPAPER_PENV_3 = 98; //  PRC Envelope #3 125 x 176 mm    
        public const int DMPAPER_PENV_4 = 99; //  PRC Envelope #4 110 x 208 mm    
        public const int DMPAPER_PENV_5 = 100; //  PRC Envelope #5 110 x 220 mm    
        public const int DMPAPER_PENV_6 = 101; //  PRC Envelope #6 120 x 230 mm    
        public const int DMPAPER_PENV_7 = 102; //  PRC Envelope #7 160 x 230 mm    
        public const int DMPAPER_PENV_8 = 103; //  PRC Envelope #8 120 x 309 mm    
        public const int DMPAPER_PENV_9 = 104; //  PRC Envelope #9 229 x 324 mm    
        public const int DMPAPER_PENV_10 = 105; //  PRC Envelope #10 324 x 458 mm   
        public const int DMPAPER_P16K_ROTATED = 106; //  PRC 16K Rotated                 
        public const int DMPAPER_P32K_ROTATED = 107; //  PRC 32K Rotated                 
        public const int DMPAPER_P32KBIG_ROTATED = 108; //  PRC 32K(Big) Rotated            
        public const int DMPAPER_PENV_1_ROTATED = 109; //  PRC Envelope #1 Rotated 165 x 102 mm 
        public const int DMPAPER_PENV_2_ROTATED = 110; //  PRC Envelope #2 Rotated 176 x 102 mm 
        public const int DMPAPER_PENV_3_ROTATED = 111; //  PRC Envelope #3 Rotated 176 x 125 mm 
        public const int DMPAPER_PENV_4_ROTATED = 112; //  PRC Envelope #4 Rotated 208 x 110 mm 
        public const int DMPAPER_PENV_5_ROTATED = 113; //  PRC Envelope #5 Rotated 220 x 110 mm 
        public const int DMPAPER_PENV_6_ROTATED = 114; //  PRC Envelope #6 Rotated 230 x 120 mm 
        public const int DMPAPER_PENV_7_ROTATED = 115; //  PRC Envelope #7 Rotated 230 x 160 mm 
        public const int DMPAPER_PENV_8_ROTATED = 116; //  PRC Envelope #8 Rotated 309 x 120 mm 
        public const int DMPAPER_PENV_9_ROTATED = 117; //  PRC Envelope #9 Rotated 324 x 229 mm 
        public const int DMPAPER_PENV_10_ROTATED = 118; //  PRC Envelope #10 Rotated 458 x 324 mm 
        public const int DMPAPER_LAST = DMPAPER_PENV_10_ROTATED;
        public const int DMPAPER_USER = 256; //  bin selections 
        public const int DMBIN_FIRST = DMBIN_UPPER;
        public const int DMBIN_UPPER = 1;
        public const int DMBIN_ONLYONE = 1;
        public const int DMBIN_LOWER = 2;
        public const int DMBIN_MIDDLE = 3;
        public const int DMBIN_MANUAL = 4;
        public const int DMBIN_ENVELOPE = 5;
        public const int DMBIN_ENVMANUAL = 6;
        public const int DMBIN_AUTO = 7;
        public const int DMBIN_TRACTOR = 8;
        public const int DMBIN_SMALLFMT = 9;
        public const int DMBIN_LARGEFMT = 10;
        public const int DMBIN_LARGECAPACITY = 11;
        public const int DMBIN_CASSETTE = 14;
        public const int DMBIN_FORMSOURCE = 15;
        public const int DMBIN_LAST = DMBIN_FORMSOURCE;
        public const int DMBIN_USER = 256; //  device specific bins start here 

        //  print qualities 
        // define DMRES_DRAFT         (-1)
        // define DMRES_LOW           (-2)
        // define DMRES_MEDIUM        (-3)
        // define DMRES_HIGH          (-4)

        //  color enable/disable for color printers 
        public const int DMCOLOR_MONOCHROME = 1;
        public const int DMCOLOR_COLOR = 2; //  duplex enable 
        public const int DMDUP_SIMPLEX = 1;
        public const int DMDUP_VERTICAL = 2;
        public const int DMDUP_HORIZONTAL = 3; //  TrueType options 
        public const int DMTT_BITMAP = 1; //  print TT fonts as graphics 
        public const int DMTT_DOWNLOAD = 2; //  download TT fonts as soft fonts 
        public const int DMTT_SUBDEV = 3; //  substitute device fonts for TT fonts 
        // if(WINVER >= 0x0400)
        public const int DMTT_DOWNLOAD_OUTLINE = 4; //  download TT fonts as outline soft fonts 
        // endif ''  WINVER >= 0x0400 

        //  Collation selections 
        public const int DMCOLLATE_FALSE = 0;
        public const int DMCOLLATE_TRUE = 1;

        // if(WINVER >= 0x0501)
        //  DEVMODE dmDisplayOrientation specifiations 
        public const int DMDO_DEFAULT = 0;
        public const int DMDO_90 = 1;
        public const int DMDO_180 = 2;
        public const int DMDO_270 = 3; //  DEVMODE dmDisplayFixedOutput specifiations 
        public const int DMDFO_DEFAULT = 0;
        public const int DMDFO_STRETCH = 1;
        public const int DMDFO_CENTER = 2;
        // endif ''  WINVER >= 0x0501 

        //  DEVMODE dmDisplayFlags flags 

        // Public Const DM_GRAYSCALE = 0x00000001 ''  This flag is no longer valid 
        public const int DM_INTERLACED = 0x2;
        public const int DMDISPLAYFLAGS_TEXTMODE = 0x4; //  dmNup , multiple logical page per physical page options 
        public const int DMNUP_SYSTEM = 1;
        public const int DMNUP_ONEUP = 2;

        // if(WINVER >= 0x0400)
        //  ICM methods 
        public const int DMICMMETHOD_NONE = 1; //  ICM disabled 
        public const int DMICMMETHOD_SYSTEM = 2; //  ICM handled by system 
        public const int DMICMMETHOD_DRIVER = 3; //  ICM handled by driver 
        public const int DMICMMETHOD_DEVICE = 4; //  ICM handled by device 
        public const int DMICMMETHOD_USER = 256; //  Device-specific methods start here 

        //  ICM Intents 
        public const int DMICM_SATURATE = 1; //  Maximize color saturation 
        public const int DMICM_CONTRAST = 2; //  Maximize color contrast 
        public const int DMICM_COLORIMETRIC = 3; //  Use specific color metric 
        public const int DMICM_ABS_COLORIMETRIC = 4; //  Use specific color metric 
        public const int DMICM_USER = 256; //  Device-specific intents start here 

        //  Media types 

        public const int DMMEDIA_STANDARD = 1; //  Standard paper 
        public const int DMMEDIA_TRANSPARENCY = 2; //  Transparency 
        public const int DMMEDIA_GLOSSY = 3; //  Glossy paper 
        public const int DMMEDIA_USER = 256; //  Device-specific media start here 

        //  Dither types 
        public const int DMDITHER_NONE = 1; //  No dithering 
        public const int DMDITHER_COARSE = 2; //  Dither with a coarse brush 
        public const int DMDITHER_FINE = 3; //  Dither with a fine brush 
        public const int DMDITHER_LINEART = 4; //  LineArt dithering 
        public const int DMDITHER_ERRORDIFFUSION = 5; //  LineArt dithering 
        public const int DMDITHER_RESERVED6 = 6; //  LineArt dithering 
        public const int DMDITHER_RESERVED7 = 7; //  LineArt dithering 
        public const int DMDITHER_RESERVED8 = 8; //  LineArt dithering 
        public const int DMDITHER_RESERVED9 = 9; //  LineArt dithering 
        public const int DMDITHER_GRAYSCALE = 10; //  Device does grayscaling 
        public const int DMDITHER_USER = 256; //  Device-specific dithers start here 
        // endif ''  WINVER >= 0x0400 



        // DevCaps

        public const int DRIVERVERSION = 0; //  Device driver version
        public const int TECHNOLOGY = 2; //  Device classification
        public const int HORZSIZE = 4; //  Horizontal size in millimeters
        public const int VERTSIZE = 6; //  Vertical size in millimeters
        public const int HORZRES = 8; //  Horizontal width in pixels
        public const int VERTRES = 10; //  Vertical height in pixels
        public const int BITSPIXEL = 12; //  Number of bits per pixel
        public const int PLANES = 14; //  Number of planes
        public const int NUMBRUSHES = 16; //  Number of brushes the device has
        public const int NUMPENS = 18; //  Number of pens the device has
        public const int NUMMARKERS = 20; //  Number of markers the device has
        public const int NUMFONTS = 22; //  Number of fonts the device has
        public const int NUMCOLORS = 24; //  Number of colors the device supports
        public const int PDEVICESIZE = 26; //  Size required for device descriptor
        public const int CURVECAPS = 28; //  Curve capabilities
        public const int LINECAPS = 30; //  Line capabilities
        public const int POLYGONALCAPS = 32; //  Polygonal capabilities
        public const int TEXTCAPS = 34; //  Text capabilities
        public const int CLIPCAPS = 36; //  Clipping capabilities
        public const int RASTERCAPS = 38; //  Bitblt capabilities
        public const int ASPECTX = 40; //  Length of the X leg
        public const int ASPECTY = 42; //  Length of the Y leg
        public const int ASPECTXY = 44; //  Length of the hypotenuse
        public const int LOGPIXELSX = 88; //  Logical pixels/inch in X
        public const int LOGPIXELSY = 90; //  Logical pixels/inch in Y
        public const int SIZEPALETTE = 104; //  Number of entries in physical palette
        public const int NUMRESERVED = 106; //  Number of reserved entries in palette
        public const int COLORRES = 108; //  Actual color resolution

        // Printing related DeviceCaps. These replace the appropriate Escapes

        public const int PHYSICALWIDTH = 110; //  Physical Width in device units
        public const int PHYSICALHEIGHT = 111; //  Physical Height in device units
        public const int PHYSICALOFFSETX = 112; //  Physical Printable Area x margin
        public const int PHYSICALOFFSETY = 113; //  Physical Printable Area y margin
        public const int SCALINGFACTORX = 114; //  Scaling factor x
        public const int SCALINGFACTORY = 115; //  Scaling factor y

        // Display driver specific

        public const int VREFRESH = 116; //  Current vertical refresh rate of the
        //  display device (for displays only) in Hz
        public const int DESKTOPVERTRES = 117; //  Horizontal width of entire desktop in
        //  pixels
        public const int DESKTOPHORZRES = 118; //  Vertical height of entire desktop in
        //  pixels
        public const int BLTALIGNMENT = 119; //  Preferred blt alignment

        //if(WINVER >= 0x0500)
        public const int SHADEBLENDCAPS = 120; //  Shading and blending caps
        public const int COLORMGMTCAPS = 121; //  Color Management caps
        //endif ''  WINVER >= 0x0500

        //ifndef NOGDICAPMASKS

        //  Device Capability Masks:

        //  Device Technologies
        public const int DT_PLOTTER = 0; //  Vector plotter
        public const int DT_RASDISPLAY = 1; //  Raster display
        public const int DT_RASPRINTER = 2; //  Raster printer
        public const int DT_RASCAMERA = 3; //  Raster camera
        public const int DT_CHARSTREAM = 4; //  Character-stream, PLP
        public const int DT_METAFILE = 5; //  Metafile, VDM
        public const int DT_DISPFILE = 6; //  Display-file

        // Device Capabilities

        public const ushort DC_FIELDS = 1;
        public const ushort DC_PAPERS = 2;
        public const ushort DC_PAPERSIZE = 3;
        public const ushort DC_MINEXTENT = 4;
        public const ushort DC_MAXEXTENT = 5;
        public const ushort DC_BINS = 6;
        public const ushort DC_DUPLEX = 7;
        public const ushort DC_SIZE = 8;
        public const ushort DC_EXTRA = 9;
        public const ushort DC_VERSION = 10;
        public const ushort DC_DRIVER = 11;
        public const ushort DC_BINNAMES = 12;
        public const ushort DC_ENUMRESOLUTIONS = 13;
        public const ushort DC_FILEDEPENDENCIES = 14;
        public const ushort DC_TRUETYPE = 15;
        public const ushort DC_PAPERNAMES = 16;
        public const ushort DC_ORIENTATION = 17;
        public const ushort DC_COPIES = 18;
        public const ushort DC_BINADJUST = 19;
        public const ushort DC_EMF_COMPLIANT = 20;
        public const ushort DC_DATATYPE_PRODUCED = 21;
        public const ushort DC_COLLATE = 22;
        public const ushort DC_MANUFACTURER = 23;
        public const ushort DC_MODEL = 24;
        public const ushort DC_PERSONALITY = 25;
        public const ushort DC_PRINTRATE = 26;
        public const ushort DC_PRINTRATEUNIT = 27;
        public const ushort PRINTRATEUNIT_PPM = 1;
        public const ushort PRINTRATEUNIT_CPS = 2;
        public const ushort PRINTRATEUNIT_LPM = 3;
        public const ushort PRINTRATEUNIT_IPM = 4;
        public const ushort DC_PRINTERMEM = 28;
        public const ushort DC_MEDIAREADY = 29;
        public const ushort DC_STAPLE = 30;
        public const ushort DC_PRINTRATEPPM = 31;
        public const ushort DC_COLORDEVICE = 32;
        public const ushort DC_NUP = 33;
        public const ushort DC_MEDIATYPENAMES = 34;
        public const ushort DC_MEDIATYPES = 35;

        // Printer

        public const int PRINTER_CONTROL_PAUSE = 1;
        public const int PRINTER_CONTROL_RESUME = 2;
        public const int PRINTER_CONTROL_PURGE = 3;
        public const int PRINTER_CONTROL_SET_STATUS = 4;
        public const int PRINTER_STATUS_PAUSED = 0x1;
        public const int PRINTER_STATUS_ERROR = 0x2;
        public const int PRINTER_STATUS_PENDING_DELETION = 0x4;
        public const int PRINTER_STATUS_PAPER_JAM = 0x8;
        public const int PRINTER_STATUS_PAPER_OUT = 0x10;
        public const int PRINTER_STATUS_MANUAL_FEED = 0x20;
        public const int PRINTER_STATUS_PAPER_PROBLEM = 0x40;
        public const int PRINTER_STATUS_OFFLINE = 0x80;
        public const int PRINTER_STATUS_IO_ACTIVE = 0x100;
        public const int PRINTER_STATUS_BUSY = 0x200;
        public const int PRINTER_STATUS_PRINTING = 0x400;
        public const int PRINTER_STATUS_OUTPUT_BIN_FULL = 0x800;
        public const int PRINTER_STATUS_NOT_AVAILABLE = 0x1000;
        public const int PRINTER_STATUS_WAITING = 0x2000;
        public const int PRINTER_STATUS_PROCESSING = 0x4000;
        public const int PRINTER_STATUS_INITIALIZING = 0x8000;
        public const int PRINTER_STATUS_WARMING_UP = 0x10000;
        public const int PRINTER_STATUS_TONER_LOW = 0x20000;
        public const int PRINTER_STATUS_NO_TONER = 0x40000;
        public const int PRINTER_STATUS_PAGE_PUNT = 0x80000;
        public const int PRINTER_STATUS_USER_INTERVENTION = 0x100000;
        public const int PRINTER_STATUS_OUT_OF_MEMORY = 0x200000;
        public const int PRINTER_STATUS_DOOR_OPEN = 0x400000;
        public const int PRINTER_STATUS_SERVER_UNKNOWN = 0x800000;
        public const int PRINTER_STATUS_POWER_SAVE = 0x1000000;
        public const int PRINTER_STATUS_SERVER_OFFLINE = 0x2000000;
        public const int PRINTER_STATUS_DRIVER_UPDATE_NEEDED = 0x4000000;
        public const int PRINTER_ATTRIBUTE_QUEUED = 0x1;
        public const int PRINTER_ATTRIBUTE_DIRECT = 0x2;
        public const int PRINTER_ATTRIBUTE_DEFAULT = 0x4;
        public const int PRINTER_ATTRIBUTE_SHARED = 0x8;
        public const int PRINTER_ATTRIBUTE_NETWORK = 0x10;
        public const int PRINTER_ATTRIBUTE_HIDDEN = 0x20;
        public const int PRINTER_ATTRIBUTE_LOCAL = 0x40;
        public const int PRINTER_ATTRIBUTE_ENABLE_DEVQ = 0x80;
        public const int PRINTER_ATTRIBUTE_KEEPPRINTEDJOBS = 0x100;
        public const int PRINTER_ATTRIBUTE_DO_COMPLETE_FIRST = 0x200;
        public const int PRINTER_ATTRIBUTE_WORK_OFFLINE = 0x400;
        public const int PRINTER_ATTRIBUTE_ENABLE_BIDI = 0x800;
        public const int PRINTER_ATTRIBUTE_RAW_ONLY = 0x1000;
        public const int PRINTER_ATTRIBUTE_PUBLISHED = 0x2000;
        public const int PRINTER_ENUM_DEFAULT = 0x1;
        public const int PRINTER_ENUM_LOCAL = 0x2;
        public const int PRINTER_ENUM_CONNECTIONS = 0x4;
        public const int PRINTER_ENUM_FAVORITE = 0x4;
        public const int PRINTER_ENUM_NAME = 0x8;
        public const int PRINTER_ENUM_REMOTE = 0x10;
        public const int PRINTER_ENUM_SHARED = 0x20;
        public const int PRINTER_ENUM_NETWORK = 0x40;
        public const int PRINTER_ENUM_EXPAND = 0x4000;
        public const int PRINTER_ENUM_CONTAINER = 0x8000;
        public const int PRINTER_ENUM_ICONMASK = 0xFF0000;
        public const int PRINTER_ENUM_ICON1 = 0x10000;
        public const int PRINTER_ENUM_ICON2 = 0x20000;
        public const int PRINTER_ENUM_ICON3 = 0x40000;
        public const int PRINTER_ENUM_ICON4 = 0x80000;
        public const int PRINTER_ENUM_ICON5 = 0x100000;
        public const int PRINTER_ENUM_ICON6 = 0x200000;
        public const int PRINTER_ENUM_ICON7 = 0x400000;
        public const int PRINTER_ENUM_ICON8 = 0x800000;
        public const int PRINTER_ENUM_HIDE = 0x1000000;


        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "OpenPrinterW")]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPWStr)] string pPrinterName, ref IntPtr hPrinter, IntPtr pDefault);
        [DllImport("winspool.drv")]
        public static extern bool ClosePrinter(IntPtr hPrinter);
        [DllImport("winspool.drv", EntryPoint = "GetPrinterW")]
        public static extern bool GetPrinter(IntPtr hPrinter, uint level, IntPtr pPrinter, uint cbBuf, ref uint pcbNeeded);
        [DllImport("winspool.drv", EntryPoint = "GetJobW")]
        public static extern bool GetJob(IntPtr hPrinter, uint JobId, uint Lovel, IntPtr pJob, uint cbBuf, ref uint pcbNeeded);
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, EntryPoint = "DeviceCapabilitiesW")]
        public static extern uint DeviceCapabilities([MarshalAs(UnmanagedType.LPWStr)] string pDevice, [MarshalAs(UnmanagedType.LPWStr)] string pPort, ushort fwCapability, IntPtr pOutput, IntPtr pDevMode);
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, EntryPoint = "EnumPrintersW")]
        public static extern bool EnumPrinters(uint Flags, [MarshalAs(UnmanagedType.LPWStr)] string Name, uint Level, IntPtr pPrinterEnum, uint cbBuf, ref uint pcbNeeded, ref uint pcbReturned);
        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);


        [DllImport("winspool.drv", CharSet = CharSet.Unicode, EntryPoint = "EnumPrinterKeyW")]
        public static extern uint EnumPrinterKey(IntPtr hPrinter, [MarshalAs(UnmanagedType.LPWStr)] string pKeyName, IntPtr pSubkey, uint cbSubKey, ref uint pcbSubkey);

        public static System.Windows.Size GetMaxDPIForPrinter(PrinterObject printer, IntPtr hPrinter)
        {
            uint cb = 0U;
            uint r = 0U;

            var mm = new MemPtr();
            var l = new List<PrinterObject>();

            PrinterObject pn;

            IntPtr cpx;

            EnumPrinters(6U, "", 2U, IntPtr.Zero, 0U, ref cb, ref r);

            if (cb != 0L)
            {
                mm.Alloc(cb);

                EnumPrinters(6U, "", 2U, mm, cb, ref cb, ref r);

                cpx = mm;

                for (int i = 1; i <= r; i++)
                {
                    pn = new PrinterObject(cpx, i == 1);
                   
                    cpx = cpx + 136;

                    l.Add(pn);
                }
            }

            return default;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    /* TODO ERROR: Skipped RegionDirectiveTrivia */

    /// <summary>
    /// Printer attributes flags
    /// </summary>
    [Flags]
    public enum PrinterAttributes : uint
    {

        /// <summary>
        /// Queued printer
        /// </summary>
        Queued = 0x1U,

        /// <summary>
        /// Direct printer
        /// </summary>
        Direct = 0x2U,

        /// <summary>
        /// Is default printer
        /// </summary>
        Default = 0x4U,

        /// <summary>
        /// Is shared printer
        /// </summary>
        Shared = 0x8U,

        /// <summary>
        /// Is network printer
        /// </summary>
        Network = 0x10U,

        /// <summary>
        /// Is hidden
        /// </summary>
        Hidden = 0x20U,

        /// <summary>
        /// Is a local printer
        /// </summary>
        Local = 0x40U,

        /// <summary>
        /// Enable DevQ
        /// </summary>
        EnableDevQ = 0x80U,

        /// <summary>
        /// Keep printed jobs
        /// </summary>
        KeepPrintedJobs = 0x100U,

        /// <summary>
        /// Do complete first
        /// </summary>
        DoCompleteFirst = 0x200U,

        /// <summary>
        /// Work offline
        /// </summary>
        WorkOffline = 0x400U,

        /// <summary>
        /// Enable BIDI
        /// </summary>
        EnableBIDI = 0x800U,

        /// <summary>
        /// Raw mode only
        /// </summary>
        RawOnly = 0x1000U,

        /// <summary>
        /// Published
        /// </summary>
        Published = 0x2000U,
        Reserved1 = 0x4000U,
        Reserved2 = 0x8000U,
        Reserved3 = 0x10000U
    }


    /// <summary>
    /// Printer status flags
    /// </summary>
    [Flags]
    public enum PrinterStatus : uint
    {
        /// <summary>
        /// The printer is busy.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is busy.")]
        Busy = PrinterModule.PRINTER_STATUS_BUSY,

        /// <summary>
        /// The printer door is open.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer door is open.")]
        DoorOpen = PrinterModule.PRINTER_STATUS_DOOR_OPEN,

        /// <summary>
        /// The printer is in an error state.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is in an error state.")]
        Error = PrinterModule.PRINTER_STATUS_ERROR,

        /// <summary>
        /// The printer is initializing.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is initializing.")]
        Initializing = PrinterModule.PRINTER_STATUS_INITIALIZING,

        /// <summary>
        /// The printer is in an active input/output state
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is in an active input/output stat.")]
        IoActive = PrinterModule.PRINTER_STATUS_IO_ACTIVE,

        /// <summary>
        /// The printer is in a manual feed state.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is in a manual feed state.")]
        ManualFeed = PrinterModule.PRINTER_STATUS_MANUAL_FEED,

        /// <summary>
        /// The printer is out of toner.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is out of toner.")]
        NoToner = PrinterModule.PRINTER_STATUS_NO_TONER,

        /// <summary>
        /// The printer is not available for printing.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is not available for printing.")]
        NotAvailable = PrinterModule.PRINTER_STATUS_NOT_AVAILABLE,

        /// <summary>
        /// The printer is offline.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is offline.")]
        Offline = PrinterModule.PRINTER_STATUS_OFFLINE,

        /// <summary>
        /// The printer has run out of memory.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer has run out of memory.")]
        OutOfMemory = PrinterModule.PRINTER_STATUS_OUT_OF_MEMORY,

        /// <summary>
        /// The printer's output bin is full.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer's output bin is full.")]
        OutputBinFull = PrinterModule.PRINTER_STATUS_OUTPUT_BIN_FULL,

        /// <summary>
        /// The printer cannot print the current page.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer cannot print the current page.")]
        PagePunt = PrinterModule.PRINTER_STATUS_PAGE_PUNT,

        /// <summary>
        /// Paper is jammed in the printer
        /// </summary>
        /// <remarks></remarks>
        [Description("Paper is jammed in the printe.")]
        PaperJam = PrinterModule.PRINTER_STATUS_PAPER_JAM,

        /// <summary>
        /// The printer is out of paper.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is out of paper.")]
        PaperOut = PrinterModule.PRINTER_STATUS_PAPER_OUT,

        /// <summary>
        /// The printer has a paper problem.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer has a paper problem.")]
        PaperProblem = PrinterModule.PRINTER_STATUS_PAPER_PROBLEM,

        /// <summary>
        /// The printer is paused.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is paused.")]
        Paused = PrinterModule.PRINTER_STATUS_PAUSED,

        /// <summary>
        /// The printer is being deleted.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is being deleted.")]
        PendingDeletion = PrinterModule.PRINTER_STATUS_PENDING_DELETION,

        /// <summary>
        /// The printer is in power save mode.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is in power save mode.")]
        PowerSave = PrinterModule.PRINTER_STATUS_POWER_SAVE,

        /// <summary>
        /// The printer is printing.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is printing.")]
        Printing = PrinterModule.PRINTER_STATUS_PRINTING,

        /// <summary>
        /// The printer is processing a print job.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is processing a print job.")]
        Processing = PrinterModule.PRINTER_STATUS_PROCESSING,

        /// <summary>
        /// The printer status is unknown.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer status is unknown.")]
        ServerUnknown = PrinterModule.PRINTER_STATUS_SERVER_UNKNOWN,

        /// <summary>
        /// The printer is low on toner.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is low on toner.")]
        TonerLow = PrinterModule.PRINTER_STATUS_TONER_LOW,

        /// <summary>
        /// The printer has an error that requires the user to do something.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer has an error that requires the user to do something.")]
        UserIntervention = PrinterModule.PRINTER_STATUS_USER_INTERVENTION,

        /// <summary>
        /// The printer is waiting.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is waiting.")]
        Waiting = PrinterModule.PRINTER_STATUS_WAITING,

        /// <summary>
        /// The printer is warming up.
        /// </summary>
        /// <remarks></remarks>
        [Description("The printer is warming up.")]
        WarmingUp = PrinterModule.PRINTER_STATUS_WARMING_UP
    }

    /// <summary>
    /// Device mode fields flags
    /// </summary>
    [Flags]
    public enum DeviceModeFields : uint
    {

        /// <summary>
        /// Orientation
        /// </summary>
        Orientation = 0x1U,

        /// <summary>
        /// Paper size
        /// </summary>
        PaperSize = 0x2U,

        /// <summary>
        /// Paper length
        /// </summary>
        PaperLength = 0x4U,

        /// <summary>
        /// Paper width
        /// </summary>
        PaperWidth = 0x8U,

        /// <summary>
        /// Scale
        /// </summary>
        Scale = 0x10U,

        /// <summary>
        /// Position
        /// </summary>
        Position = 0x20U,

        /// <summary>
        /// Nup
        /// </summary>
        Nup = 0x40U,

        /// <summary>
        /// Display orientation
        /// </summary>
        DisplayOrientation = 0x80U,

        /// <summary>
        /// Copies
        /// </summary>
        Copies = 0x100U,

        /// <summary>
        /// Default source
        /// </summary>
        DefaultSource = 0x200U,

        /// <summary>
        /// Print quality
        /// </summary>
        PrintQuality = 0x400U,

        /// <summary>
        /// Color printer
        /// </summary>
        Color = 0x800U,

        /// <summary>
        /// Duplex support
        /// </summary>
        Duplex = 0x1000U,

        /// <summary>
        /// YR resolution
        /// </summary>
        YResolution = 0x2000U,

        /// <summary>
        /// TTOption
        /// </summary>
        TTOption = 0x4000U,

        /// <summary>
        /// Collate
        /// </summary>
        Collate = 0x8000U,

        /// <summary>
        /// Form name
        /// </summary>
        FormName = 0x10000U,

        /// <summary>
        /// Log pixels
        /// </summary>
        LogPixels = 0x20000U,

        /// <summary>
        /// Bits per pixel
        /// </summary>
        BitsPerPel = 0x40000U,

        /// <summary>
        /// Width in pixels
        /// </summary>
        PelsWidth = 0x80000U,

        /// <summary>
        /// Height in pixels
        /// </summary>
        PelsHeight = 0x100000U,

        /// <summary>
        /// Display flags
        /// </summary>
        DisplayFlags = 0x200000U,

        /// <summary>
        /// Display frequency
        /// </summary>
        DisplayFrequency = 0x400000U,

        /// <summary>
        /// ICM Method
        /// </summary>
        ICMMethod = 0x800000U,

        /// <summary>
        /// ICM Intent
        /// </summary>
        ICMIntent = 0x1000000U,

        /// <summary>
        /// Media type
        /// </summary>
        MediaType = 0x2000000U,

        /// <summary>
        /// Dither type
        /// </summary>
        DitherType = 0x4000000U,

        /// <summary>
        /// Panning width
        /// </summary>
        PanningWidth = 0x8000000U,

        /// <summary>
        /// Panning height
        /// </summary>
        PanningHeight = 0x10000000U
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Paper nationalities
    /// </summary>
    public enum PaperNationalities
    {

        /// <summary>
        /// American
        /// </summary>
        American,

        /// <summary>
        /// ISO / International Standard
        /// </summary>
        Iso,

        /// <summary>
        /// Japanese
        /// </summary>
        Japanese,

        /// <summary>
        /// German
        /// </summary>
        German,

        /// <summary>
        /// Chinese
        /// </summary>
        Chinese
    }

    /// <summary>
    /// Represents a collection of all known system paper types
    /// </summary>
    public class SystemPaperTypes
    {

        /// <summary>
        /// This class is not creatable.
        /// </summary>
        /// <remarks></remarks>
        private SystemPaperTypes()
        {
        }

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private static readonly string _SizeDataString = "LETTER	1	US Letter 8 1/2 x 11 in" + "\r\n" + "LETTER_SMALL	2	US Letter Small 8 1/2 x 11 in" + "\r\n" + "TABLOID	3	US Tabloid 11 x 17 in" + "\r\n" + "LEDGER	4	US Ledger 17 x 11 in" + "\r\n" + "LEGAL	5	US Legal 8 1/2 x 14 in" + "\r\n" + "STATEMENT	6	US Statement 5 1/2 x 8 1/2 in" + "\r\n" + "EXECUTIVE	7	US Executive 7 1/4 x 10 1/2 in" + "\r\n" + "A3	8	A3 297 x 420 mm" + "\r\n" + "A4	9	A4 210 x 297 mm" + "\r\n" + "A4_SMALL	10	A4 Small 210 x 297 mm" + "\r\n" + "A5	11	A5 148 x 210 mm" + "\r\n" + "B4	12	B4 (JIS) 257 x 364 mm" + "\r\n" + "B5	13	B5 (JIS) 182 x 257 mm" + "\r\n" + "FOLIO	14	Folio 8 1/2 x 13 in" + "\r\n" + "QUARTO	15	Quarto 215 x 275 mm" + "\r\n" + "10X14	16	10 x 14 in" + "\r\n" + "11X17	17	11 x 17 in" + "\r\n" + "NOTE	18	US Note 8 1/2 x 11 in" + "\r\n" + "ENV_9	19	US Envelope #9 - 3 7/8 x 8 7/8" + "\r\n" + "ENV_10	20	US Envelope #10 - 4 1/8 x 9 1/2" + "\r\n" + "ENV_11	21	US Envelope #11 - 4 1/2 x 10 3/8" + "\r\n" + "ENV_12	22	US Envelope #12 - 4 3/4 x 11 in" + "\r\n" + "ENV_14	23	US Envelope #14 - 5 x 11 1/2" + "\r\n" + "ENV_DL	27	Envelope DL 110 x 220 mm" + "\r\n" + "ENV_C5	28	Envelope C5 - 162 x 229 mm" + "\r\n" + "ENV_C3	29	Envelope C3 - 324 x 458 mm" + "\r\n" + "ENV_C4	30	Envelope C4 - 229 x 324 mm" + "\r\n" + "ENV_C6	31	Envelope C6 - 114 x 162 mm" + "\r\n" + "ENV_C65	32	Envelope C65 - 114 x 229 mm" + "\r\n" + "ENV_B4	33	Envelope B4 - 250 x 353 mm" + "\r\n" + "ENV_B5	34	Envelope B5 - 176 x 250 mm" + "\r\n" + "ENV_B6	35	Envelope B6 - 176 x 125 mm" + "\r\n" + "ENV_ITALY	36	Envelope 110 x 230 mm" + "\r\n" + "ENV_MONARCH	37	US Envelope Monarch 3.875 x 7.5 in" + "\r\n" + "ENV_PERSONAL	38	6 3/4 US Envelope 3 5/8 x 6 1/2 in" + "\r\n" + "FANFOLD_US	39	US Std Fanfold 14 7/8 x 11 in" + "\r\n" + "FANFOLD_STD_GERMAN	40	German Std Fanfold 8 1/2 x 12 in" + "\r\n" + "FANFOLD_LGL_GERMAN	41	German Legal Fanfold 8 1/2 x 13 in" + "\r\n" + "ISO_B4	42	B4 (ISO) 250 x 353 mm" + "\r\n" + "JAPANESE_POSTCARD	43	Japanese Postcard 100 x 148 mm" + "\r\n" + "9X11	44	9 x 11 in" + "\r\n" + "10X11	45	10 x 11 in" + "\r\n" + "15X11	46	15 x 11 in" + "\r\n" + "ENV_INVITE	47	Envelope Invite 220 x 220 mm" + "\r\n" + "LETTER_EXTRA	50	US Letter Extra 9 1/2 x 12 in" + "\r\n" + "LEGAL_EXTRA	51	US Legal Extra 9 1/2 x 15 in" + "\r\n" + "TABLOID_EXTRA	52	US Tabloid Extra 11.69 x 18 in" + "\r\n" + "A4_EXTRA	53	A4 Extra 9.27 x 12.69 in" + "\r\n" + "LETTER_TRANSVERSE	54	Letter Transverse 8 1/2 x 11 in" + "\r\n" + "A4_TRANSVERSE	55	A4 Transverse 210 x 297 mm" + "\r\n" + "LETTER_EXTRA_TRANSVERSE	56	Letter Extra Transverse 9 1/2 x 12 in" + "\r\n" + "A_PLUS	57	SuperA/SuperA/A4 227 x 356 mm" + "\r\n" + "B_PLUS	58	SuperB/SuperB/A3 305 x 487 mm" + "\r\n" + "LETTER_PLUS	59	US Letter Plus 8.5 x 12.69 in" + "\r\n" + "A4_PLUS	60	A4 Plus 210 x 330 mm" + "\r\n" + "A5_TRANSVERSE	61	A5 Transverse 148 x 210 mm" + "\r\n" + "B5_TRANSVERSE	62	B5 (JIS) Transverse 182 x 257 mm" + "\r\n" + "A3_EXTRA	63	A3 Extra 322 x 445 mm" + "\r\n" + "A5_EXTRA	64	A5 Extra 174 x 235 mm" + "\r\n" + "B5_EXTRA	65	B5 (ISO) Extra 201 x 276 mm" + "\r\n" + "A2	66	A2 420 x 594 mm" + "\r\n" + "A3_TRANSVERSE	67	A3 Transverse 297 x 420 mm" + "\r\n" + "A3_EXTRA_TRANSVERSE	68	A3 Extra Transverse 322 x 445 mm" + "\r\n" + "DBL_JAPANESE_POSTCARD	69	Japanese Double Postcard 200 x 148 mm" + "\r\n" + "A6	70	A6 105 x 148 mm" + "\r\n" + "LETTER_ROTATED	75	Letter Rotated 11 x 8 1/2 11 in" + "\r\n" + "A3_ROTATED	76	A3 Rotated 420 x 297 mm" + "\r\n" + "A4_ROTATED	77	A4 Rotated 297 x 210 mm" + "\r\n" + "A5_ROTATED	78	A5 Rotated 210 x 148 mm" + "\r\n" + "B4_JIS_ROTATED	79	B4 (JIS) Rotated 364 x 257 mm" + "\r\n" + "B5_JIS_ROTATED	80	B5 (JIS) Rotated 257 x 182 mm" + "\r\n" + "JAPANESE_POSTCARD_ROTATED	81	Japanese Postcard Rotated 148 x 100 mm" + "\r\n" + "DBL_JAPANESE_POSTCARD_ROTATED	82	Double Japanese Postcard Rotated 148 x 200 mm" + "\r\n" + "A6_ROTATED	83	A6 Rotated 148 x 105 mm" + "\r\n" + "B6_JIS	88	B6 (JIS) 128 x 182 mm" + "\r\n" + "B6_JIS_ROTATED	89	B6 (JIS) Rotated 182 x 128 mm" + "\r\n" + "12X11	90	12 x 11 in" + "\r\n" + "P16K	93	PRC 16K 146 x 215 mm" + "\r\n" + "P32K	94	PRC 32K 97 x 151 mm" + "\r\n" + "P32KBIG	95	PRC 32K(Big) 97 x 151 mm" + "\r\n" + "PENV_1	96	PRC Envelope #1 - 102 x 165 mm" + "\r\n" + "PENV_2	97	PRC Envelope #2 - 102 x 176 mm" + "\r\n" + "PENV_3	98	PRC Envelope #3 - 125 x 176 mm" + "\r\n" + "PENV_4	99	PRC Envelope #4 - 110 x 208 mm" + "\r\n" + "PENV_5	100	PRC Envelope #5 - 110 x 220 mm" + "\r\n" + "PENV_6	101	PRC Envelope #6 - 120 x 230 mm" + "\r\n" + "PENV_7	102	PRC Envelope #7 - 160 x 230 mm" + "\r\n" + "PENV_8	103	PRC Envelope #8 - 120 x 309 mm" + "\r\n" + "PENV_9	104	PRC Envelope #9 - 229 x 324 mm" + "\r\n" + "PENV_10	105	PRC Envelope #10 - 324 x 458 mm" + "\r\n" + "PENV_1_ROTATED	109	PRC Envelope #1 Rotated 165 x 102 mm" + "\r\n" + "PENV_2_ROTATED	110	PRC Envelope #2 Rotated 176 x 102 mm" + "\r\n" + "PENV_3_ROTATED	111	PRC Envelope #3 Rotated 176 x 125 mm" + "\r\n" + "PENV_4_ROTATED	112	PRC Envelope #4 Rotated 208 x 110 mm" + "\r\n" + "PENV_5_ROTATED	113	PRC Envelope #5 Rotated 220 x 110 mm" + "\r\n" + "PENV_6_ROTATED	114	PRC Envelope #6 Rotated 230 x 120 mm" + "\r\n" + "PENV_7_ROTATED	115	PRC Envelope #7 Rotated 230 x 160 mm" + "\r\n" + "PENV_8_ROTATED	116	PRC Envelope #8 Rotated 309 x 120 mm" + "\r\n" + "PENV_9_ROTATED	117	PRC Envelope #9 Rotated 324 x 229 mm" + "\r\n" + "PENV_10_ROTATED	118	PRC Envelope #10 Rotated 458 x 324 mm";

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /// <summary>
        /// Returns the list of supported system paper types.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static ReadOnlyCollection<SystemPaperType> PaperTypes
        {
            get
            {
                return _PaperList;
            }
        }

        private static ReadOnlyCollection<SystemPaperType> _PaperList;

        private static void ParsePapers()
        {
            var objOut = new List<SystemPaperType>();
            var paperList = TextTools.Split(_SizeDataString, "\r\n");
            foreach (var paper in paperList)
            {
                var p = new SystemPaperType();
                var data = TextTools.Split(paper, "\t");
                p.Name = TextTools.CamelCase(data[0]);
                p.Code = int.Parse(data[1]);
                p.Description = data[2];
                p.IsTransverse = data[2].IndexOf("Transverse") != -1;
                p.IsPostcard = data[2].IndexOf("Postcard") != -1;
                p.IsRotated = data[2].IndexOf("Rotated") != -1;
                p.IsEnvelope = data[2].IndexOf("Envelope") != -1;
                if (data[2].IndexOf("German") != -1)
                {
                    p.Nationality = PaperNationalities.German;
                }
                else if (data[2].IndexOf("US ") != -1)
                {
                    p.Nationality = PaperNationalities.American;
                }
                else if (data[2].IndexOf("PRC") != -1)
                {
                    p.Nationality = PaperNationalities.Chinese;
                }
                else if (data[2].IndexOf("Japan") != -1)
                {
                    p.Nationality = PaperNationalities.Japanese;
                }
                else if (data[2].IndexOf("(JIS)") != -1)
                {
                    p.Nationality = PaperNationalities.Japanese;
                }

                bool ismm = false;
                var size = FindSize(data[2], ref ismm);
                if (ismm)
                {
                    p.SizeMillimeters = size;
                }
                else
                {
                    p.SizeInches = size;
                }

                objOut.Add(p);
                p = null;
            }

            _PaperList = new ReadOnlyCollection<SystemPaperType>(objOut);
        }

        //'' <summary>
        //'' Parses a size from any kind of text.
        //'' </summary>
        //'' <param name="text">The text to parse.</param>
        //'' <param name="isMM">Receives a value indicating metric system.</param>
        //'' <param name="scanforDblQuote">Scan for double quotes as inches.</param>
        //'' <param name="acceptComma">Accept a comma as a separator in addition to the 'x'.</param>
        //'' <returns></returns>
        //'' <remarks></remarks>
        private static System.Windows.Size FindSize(string text, ref bool isMM, bool scanforDblQuote = false, bool acceptComma = false)
        {
            char[] ch;
            var sOut = new System.Windows.Size();
            bool pastX = false;
            int i;
            int c;
            int x = 0;
            string t;
            if (scanforDblQuote)
            {
                text = text.Replace("\"", "in").Trim();
            }

            t = text.Substring(text.Length - 2, 2).ToLower();
            isMM = t == "mm";
            if (t == "in" || t == "mm")
            {
                text = text.Substring(0, text.Length - 2).Trim();
            }

            ch = text.ToCharArray();
            i = ch.Count() - 1;

            // not allowed space (for metric)
            bool nas = false;
            for (c = i; c >= 0; c -= 1)
            {
                if (ch[c] == 'x')
                {
                    pastX = true;
                    x = i;
                }
                else if (acceptComma && ch[c] == ',')
                {
                    pastX = true;
                    x = i;
                }
                else if (pastX)
                {
                    if (ch[c] == ' ')
                    {
                        if (isMM & nas)
                            break;
                        continue;
                    }

                    if (ch[c] == '/')
                    {
                        continue;
                    }

                    if (ch[c] == '.')
                    {
                        continue;
                    }

                    if (TextTools.IsNumber(ch[c]) == false)
                    {
                        break;
                    }
                    else
                    {
                        nas = true;
                    }
                }
            }

            text = text.Substring(c + 1).Trim();
            text = text.Replace(",", "x");
            var sizes = TextTools.Split(text, "x");
            double d;
            double e;
            double f;
            int sc = 0;
            foreach (var num in sizes)
            {
                d = 0d;
                var nch = TextTools.Split(num.Trim(), " ");
                if (nch.Count() == 2)
                {
                    var div = TextTools.Split(nch[1], "/");
                    if (div.Count() == 2)
                    {
                        d = double.Parse(div[0]);
                        e = double.Parse(div[1]);
                        d /= e;
                    }
                }

                f = double.Parse(nch[0]) + d;
                if (sc == 0)
                    sOut.Width = f;
                else
                    sOut.Height = f;
                sc = 1;
            }

            return sOut;
        }

        internal static List<SystemPaperType> TypeListFromCodes(IEnumerable<short> list)
        {
            var o = new List<SystemPaperType>();
            foreach (var p in _PaperList)
            {
                foreach (var i in list)
                {
                    if (p.Code == i)
                    {
                        o.Add(p);
                        break;
                    }
                }
            }

            return o;
        }

        static SystemPaperTypes()
        {
            ParsePapers();
        }
    }

    /// <summary>
    /// IPaperType interface
    /// </summary>
    public interface IPaperType
    {

        /// <summary>
        /// Paper type name
        /// </summary>
        /// <returns></returns>
        string Name { get; }

        /// <summary>
        /// True if orientation is landscape
        /// </summary>
        /// <returns></returns>
        bool IsLandscape { get; }

        /// <summary>
        /// If true, size is in metric units (millimeters).
        /// If false, size is in inches.
        /// </summary>
        /// <returns></returns>
        bool IsMetric { get; }

        /// <summary>
        /// Paper size
        /// </summary>
        /// <returns></returns>
        UniSize Size { get; }

        /// <summary>
        /// Compare one paper type to another paper type for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool Equals(IPaperType other);
    }

    /// <summary>
    /// Encapsulates a system paper type
    /// </summary>
    public class SystemPaperType : IEquatable<SystemPaperType>, IPaperType
    {
        private System.Windows.Size _Size;
        private string _Description;
        private bool _IsTransverse;
        private bool _IsRotated;
        private bool _IsPostcard;
        private PaperNationalities _Nationality = PaperNationalities.Iso;

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Equals(IPaperType other)
        {
            if (other.IsLandscape != (IsTransverse | IsEnvelope))
                return false;
            if (other.IsMetric != IsMetric)
                return false;
            if (other.IsMetric)
            {
                if (other.Size.Equals(SizeMillimeters))
                    return true;
            }
            else if (other.Size.Equals(SizeInches))
                return true;
            return false;
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool Equals(SystemPaperType other)
        {
            return _Size.Equals(other._Size);
        }

        /// <summary>
        /// Returns the (apparent) national or international origin of the paper size.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public PaperNationalities Nationality
        {
            get
            {
                return _Nationality;
            }

            internal set
            {
                _Nationality = value;
            }
        }

        /// <summary>
        /// True if this is a postcard layout.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsPostcard
        {
            get
            {
                return _IsPostcard;
            }

            internal set
            {
                _IsPostcard = value;
            }
        }

        /// <summary>
        /// True for transverse/landscape.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsTransverse
        {
            get
            {
                return _IsTransverse;
            }

            internal set
            {
                _IsTransverse = value;
            }
        }

        /// <summary>
        /// Returns true for rotated layout.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsRotated
        {
            get
            {
                return _IsRotated;
            }

            internal set
            {
                _IsRotated = value;
            }
        }

        private bool _IsEnvelope;

        /// <summary>
        /// Returns true if this paper type is an envelope.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsEnvelope
        {
            get
            {
                return _IsEnvelope;
            }

            internal set
            {
                _IsEnvelope = value;
            }
        }

        /// <summary>
        /// Returns true if it is transverse or envelope paper.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsLandscape
        {
            get
            {
                return _IsTransverse | _IsEnvelope;
            }

            internal set
            {
                _IsTransverse = value;
            }
        }

        /// <summary>
        /// Returns a description of the paper.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Description
        {
            get
            {
                return _Description;
            }

            internal set
            {
                _Description = value;
            }
        }

        private string _Name;

        /// <summary>
        /// The name of the paper type.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Name
        {
            get
            {
                return _Name;
            }

            internal set
            {
                _Name = value;
            }
        }

        private int _Code;

        /// <summary>
        /// The Windows paper type code.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Code
        {
            get
            {
                return _Code;
            }

            internal set
            {
                _Code = value;
            }
        }

        /// <summary>
        /// The size of the paper, in inches.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Windows.Size SizeInches
        {
            get
            {
                return _Size;
            }

            internal set
            {
                _Size = value;
            }
        }

        /// <summary>
        /// The size of the paper, in millimeters.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Windows.Size SizeMillimeters
        {
            get
            {
                return InchesToMillimeters(_Size);
            }

            internal set
            {
                _Size = MillimetersToInches(value);
            }
        }

        /// <summary>
        /// Returns the IPaperType IsMetric value.  If this value is true, millimeters are used instead of inches to measure the paper.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsMetric
        {
            get
            {
                return _Nationality != PaperNationalities.American;
            }

            set
            {
                if (value)
                {
                    _Nationality = PaperNationalities.Iso;
                }
                else
                {
                    _Nationality = PaperNationalities.American;
                }
            }
        }

        /// <summary>
        /// Returns the IPaperType size, which is different according
        /// to the IPaperType.IsMetric value.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public UniSize Size
        {
            get
            {
                if (_Nationality == PaperNationalities.American)
                {
                    return SizeInches;
                }
                else
                {
                    return SizeMillimeters;
                }
            }

            internal set
            {
                if (_Nationality == PaperNationalities.American)
                {
                    SizeInches = value;
                }
                else
                {
                    SizeMillimeters = value;
                }
            }
        }

        /// <summary>
        /// Convert inches to millimeters
        /// </summary>
        /// <param name="size">A <see cref="System.Windows.Size"/> structure</param>
        /// <returns></returns>
        public static System.Windows.Size InchesToMillimeters(System.Windows.Size size)
        {
            return new System.Windows.Size(size.Width * 25.4d, size.Height * 25.4d);
        }

        /// <summary>
        /// Convert millimeters to inches
        /// </summary>
        /// <param name="size">A <see cref="System.Windows.Size"/> structure</param>
        /// <returns></returns>
        public static System.Windows.Size MillimetersToInches(System.Windows.Size size)
        {
            return new System.Windows.Size(size.Width / 25.4d, size.Height / 25.4d);
        }


        /// <summary>
        /// Returns the <see cref="Description"/> property.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _Description;
        }

        internal SystemPaperType()
        {
        }


        /// <summary>
        /// Explicit cast to <see cref="String"/>
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static explicit operator string(SystemPaperType operand)
        {
            return operand.Name;
        }

        /// <summary>
        /// Explicit cast to <see cref="SystemPaperType"/>
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static explicit operator SystemPaperType(string operand)
        {
            foreach (var t in SystemPaperTypes.PaperTypes)
            {
                if ((t.Name.ToLower() ?? "") == (operand.ToLower() ?? ""))
                    return t;
            }

            return null;
        }

        /// <summary>
        /// Explicit cast to integer
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>

        public static explicit operator int(SystemPaperType operand)
        {
            return operand.Code;
        }

        /// <summary>
        /// Explicit cast to <see cref="SystemPaperType"/>
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static explicit operator SystemPaperType(int operand)
        {
            foreach (var t in SystemPaperTypes.PaperTypes)
            {
                if (t.Code == operand)
                    return t;
            }

            return null;
        }

        /// <summary>
        /// Explicit cast to unsigned integer
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static explicit operator uint(SystemPaperType operand)
        {
            return (uint)operand.Code;
        }

        /// <summary>
        /// Explicit cast to <see cref="SystemPaperType"/>
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static explicit operator SystemPaperType(uint operand)
        {
            foreach (var t in SystemPaperTypes.PaperTypes)
            {
                if ((long)t.Code == operand)
                    return t;
            }

            return null;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */

    /// <summary>
    /// Information about a job in the print queue
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class JobInfo : SafeHandle
    {
        private MemPtr _ptr;
        private MemPtr _str;

        internal JobInfo(IntPtr ptr) : base(IntPtr.Zero, true)
        {
            _ptr = ptr;
            _str = ptr + 4;
            handle = ptr;
        }

        public override bool IsInvalid
        {
            get
            {
                return _ptr.Handle == IntPtr.Zero;
            }
        }

        protected override bool ReleaseHandle()
        {
            try
            {
                if (_ptr.Handle != IntPtr.Zero)
                    _ptr.Free();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Job Id
        /// </summary>
        /// <returns></returns>
        public uint JobId
        {
            get
            {
                return _ptr.UIntAt(0L);
            }

            set
            {
                _ptr.UIntAt(0L) = value;
            }
        }

        /// <summary>
        /// The name of the printer printing this job
        /// </summary>
        /// <returns></returns>
        public string PrinterName
        {
            get
            {
                return _str.GetStringIndirect(0 * IntPtr.Size);
            }

            set
            {
                _str.SetStringIndirect(0 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// The name of the computer that owns this job
        /// </summary>
        /// <returns></returns>
        public string MachineName
        {
            get
            {
                return _str.GetStringIndirect(1 * IntPtr.Size);
            }

            set
            {
                _str.SetStringIndirect(1 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// The username of the user printing this job
        /// </summary>
        /// <returns></returns>
        public string UserName
        {
            get
            {
                return _str.GetStringIndirect(2 * IntPtr.Size);
            }

            set
            {
                _str.SetStringIndirect(2 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// The name of the document being printed
        /// </summary>
        /// <returns></returns>
        public string Document
        {
            get
            {
                return _str.GetStringIndirect(3 * IntPtr.Size);
            }

            set
            {
                _str.SetStringIndirect(3 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Notification name
        /// </summary>
        /// <returns></returns>
        public string NotifyName
        {
            get
            {
                return _str.GetStringIndirect(4 * IntPtr.Size);
            }

            set
            {
                _str.SetStringIndirect(4 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Data type
        /// </summary>
        /// <returns></returns>
        public string Datatype
        {
            get
            {
                return _str.GetStringIndirect(5 * IntPtr.Size);
            }

            set
            {
                _str.SetStringIndirect(5 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Print processor
        /// </summary>
        /// <returns></returns>
        public string PrintProcessor
        {
            get
            {
                return _str.GetStringIndirect(6 * IntPtr.Size);
            }

            set
            {
                _str.SetStringIndirect(6 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Parameters
        /// </summary>
        /// <returns></returns>
        public string Parameters
        {
            get
            {
                return _str.GetStringIndirect(7 * IntPtr.Size);
            }

            set
            {
                _str.SetStringIndirect(7 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Driver name
        /// </summary>
        /// <returns></returns>
        public string DriverName
        {
            get
            {
                return _str.GetStringIndirect(8 * IntPtr.Size);
            }

            set
            {
                _str.SetStringIndirect(8 * IntPtr.Size, value);
            }
        }

        internal IntPtr DevMode
        {
            get
            {
                return IntPtr.Size == 8 ? (IntPtr)_str.LongAt(9L) : (IntPtr)_str.IntAt(9L);
            }

            set
            {
                if (IntPtr.Size == 4)
                {
                    _str.IntAt(9L) = (int)value;
                }
                else
                {
                    _str.LongAt(9L) = (long)value;
                }
            }
        }

        /// <summary>
        /// Status message
        /// </summary>
        /// <returns></returns>
        public string StatusMessage
        {
            get
            {
                return _str.GetStringIndirect(10 * IntPtr.Size);
            }

            set
            {
                _str.SetStringIndirect(10 * IntPtr.Size, value);
            }
        }

        internal IntPtr SecurityDescriptor
        {
            get
            {
                return IntPtr.Size == 8 ? (IntPtr)_str.LongAt(11L) : (IntPtr)_str.IntAt(11L);
            }

            set
            {
                if (IntPtr.Size == 4)
                {
                    _str.IntAt(11L) = (int)value;
                }
                else
                {
                    _str.LongAt(11L) = (long)value;
                }
            }
        }

        /// <summary>
        /// Status code
        /// </summary>
        /// <returns></returns>
        public uint StatusCode
        {
            get
            {
                int i = 4 + 12 * IntPtr.Size;
                return _ptr.UIntAtAbsolute(i);
            }

            set
            {
                int i = 4 + 12 * IntPtr.Size;
                _ptr.UIntAtAbsolute(i) = value;
            }
        }

        /// <summary>
        /// Print queue priority
        /// </summary>
        /// <returns></returns>
        public uint Priority
        {
            get
            {
                int i = 8 + 12 * IntPtr.Size;
                return _ptr.UIntAtAbsolute(i);
            }

            set
            {
                int i = 8 + 12 * IntPtr.Size;
                _ptr.UIntAtAbsolute(i) = value;
            }
        }

        /// <summary>
        /// Print queue position
        /// </summary>
        /// <returns></returns>
        public uint Position
        {
            get
            {
                int i = 12 + 12 * IntPtr.Size;
                return _ptr.UIntAtAbsolute(i);
            }

            set
            {
                int i = 12 + 12 * IntPtr.Size;
                _ptr.UIntAtAbsolute(i) = value;
            }
        }

        /// <summary>
        /// Job start time
        /// </summary>
        /// <returns></returns>
        public FriendlyUnixTime StartTime
        {
            get
            {
                int i = 16 + 12 * IntPtr.Size;
                return _ptr.UIntAtAbsolute(i);
            }

            set
            {
                int i = 16 + 12 * IntPtr.Size;
                _ptr.UIntAtAbsolute(i) = value;
            }
        }

        /// <summary>
        /// Job run unti time
        /// </summary>
        /// <returns></returns>
        public FriendlyUnixTime UntilTime
        {
            get
            {
                int i = 20 + 12 * IntPtr.Size;
                return _ptr.UIntAtAbsolute(i);
            }

            set
            {
                int i = 20 + 12 * IntPtr.Size;
                _ptr.UIntAtAbsolute(i) = value;
            }
        }

        /// <summary>
        /// Total pages in job
        /// </summary>
        /// <returns></returns>
        public uint TotalPages
        {
            get
            {
                int i = 24 + 12 * IntPtr.Size;
                return _ptr.UIntAtAbsolute(i);
            }

            set
            {
                int i = 24 + 12 * IntPtr.Size;
                _ptr.UIntAtAbsolute(i) = value;
            }
        }

        /// <summary>
        /// The job size
        /// </summary>
        /// <returns></returns>
        public uint Size
        {
            get
            {
                int i = 28 + 12 * IntPtr.Size;
                return _ptr.UIntAtAbsolute(i);
            }

            set
            {
                int i = 28 + 12 * IntPtr.Size;
                _ptr.UIntAtAbsolute(i) = value;
            }
        }

        /// <summary>
        /// The time the job was submitted
        /// </summary>
        /// <returns></returns>
        public DateTime Submitted
        {
            get
            {
                int i = 32 + 12 * IntPtr.Size;
                return (DateTime)(_ptr.ToStructAt<SYSTEMTIME>(i));
            }

            set
            {
                int i = 32 + 12 * IntPtr.Size;
                _ptr.FromStructAt(i, (SYSTEMTIME)value);
            }
        }

        /// <summary>
        /// Elapsed time (in seconds)
        /// </summary>
        /// <returns></returns>
        public uint Time
        {
            get
            {
                int i = 48 + 12 * IntPtr.Size;
                return _ptr.UIntAtAbsolute(i);
            }

            set
            {
                int i = 48 + 12 * IntPtr.Size;
                _ptr.UIntAtAbsolute(i) = value;
            }
        }

        /// <summary>
        /// Page finished printing
        /// </summary>
        /// <returns></returns>
        public uint PagePrinted
        {
            get
            {
                int i = 52 + 12 * IntPtr.Size;
                return _ptr.UIntAtAbsolute(i);
            }

            set
            {
                int i = 52 + 12 * IntPtr.Size;
                _ptr.UIntAtAbsolute(i) = value;
            }
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// A collection of printers available to the machine
    /// </summary>
    public class PrinterObjects : ObservableCollection<PrinterObject>
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct PRINTER_INFO_4
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pPrinterName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pServerName;
            public uint Attributes;
        }

        private PrinterObjects()
        {
        }

        private static PrinterObjects _printers;

        static PrinterObjects()
        {
            _printers = new PrinterObjects();
            Refresh();
        }

        /// <summary>
        /// The collection of printers
        /// </summary>
        /// <returns></returns>
        public static PrinterObjects Printers
        {
            get
            {
                return _printers;
            }
        }

        /// <summary>
        /// Refresh the available printers
        /// </summary>
        /// <returns></returns>
        public static bool Refresh()
        {
            var mm = new MemPtr();
            MemPtr ap;
            uint cb = 0U;
            uint rc = 0U;
            var pif = new PRINTER_INFO_4();
            var sp = new List<string>();
            int ps = Marshal.SizeOf(pif);
            string ts;
            PrinterModule.EnumPrinters(PrinterModule.PRINTER_ENUM_NAME, "", 4U, IntPtr.Zero, 0U, ref cb, ref rc);
            if (cb > 0L)
            {
                mm.Alloc(cb);
                ap = mm;
                PrinterModule.EnumPrinters(PrinterModule.PRINTER_ENUM_NAME, "", 4U, mm, cb, ref cb, ref rc);
                cb = 0U;
                for (int u = 1; u <= rc; u++)
                {
                    ts = ap.GetStringIndirect(cb);
                    sp.Add(ts);
                    ap = ap + ps;
                }

                mm.Free();
            }
            else
            {
                return false;
            }

            PrinterObject po = null;
            _printers.Clear();
            foreach (var s in sp)
            {
                try
                {
                    // MsgBox("Attempting to get highly detailed printer information for '" & s & "'")
                    if ((s.Trim() ?? "") != (s ?? ""))
                    {
                        // MsgBox("Printer name has extra space characters after name! '" & s & "'", MsgBoxStyle.Exclamation)
                    }

                    po = PrinterObject.GetPrinterInfoObject(s);
                }
                catch
                {
                    po = null;
                }

                if (po is object)
                {
                    _printers.Add(po);
                    po = null;
                }
                else
                {
                    // MsgBox("For some reason, the printer that the system just reported would not return a useful Object." & vbCrLf & "Printer: " & s)
                }
            }

            return _printers.Count > 0;
        }
    }

    /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    /* TODO ERROR: Skipped RegionDirectiveTrivia */
    /// <summary>
    /// Encapsulates a printer queue on the system.
    /// </summary>
    /// <remarks></remarks>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PrinterObject : CriticalFinalizerObject, IDisposable, IEquatable<PrinterObject>
    {
        internal MemPtr _ptr;

        // This is a scratch-pad memory pointer for various getting and setting functions
        // so that we don't have to keep allocating and deallocating resources.
        private MemPtr _mm = new MemPtr(16L);
        private DeviceMode _DevMode;

        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        public bool Equals(PrinterObject other)
        {
            return (PrinterName ?? "") == (other.PrinterName ?? "");
        }

        private PrinterObject()
        {
        }

        internal PrinterObject(IntPtr ptr, bool fOwn)
        {
            _fOwn = fOwn;
            _ptr = ptr;
            MemPtr mm = ptr;
            if (IntPtr.Size == 4)
            {
                _DevMode = new DeviceMode((IntPtr)mm.IntAt(7L), false);
            }
            // MsgBox("Got a pointer! x86 mode.")
            else
            {
                _DevMode = new DeviceMode((IntPtr)mm.LongAt(7L), false);
                // MsgBox("Got a pointer! x64 mode.")
                // If printer._DevMode IsNot Nothing Then MsgBox("DevMode retrieval successful, devmode reports device name as '" & printer._DevMode.DeviceName & "'")
            }
        }

        private bool _fOwn = true;

        public void Dispose()
        {
            if (_fOwn)
                _ptr.Free();
            _mm.Free();
            GC.SuppressFinalize(this);
        }

        public PrinterObject(string printerName)
        {
            internalPopulatePrinter(printerName, this);
        }


        ~PrinterObject()
        {
            if (_fOwn)
                _ptr.Free();
            _mm.Free();
        }

        public override string ToString()
        {
            return PrinterName;
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Get the printable area of the page
        /// </summary>
        /// <param name="printer">The printer</param>
        /// <param name="paper">Paper type</param>
        /// <param name="resolution">Resolution</param>
        /// <param name="orientation">Orientation</param>
        /// <returns></returns>
        public static UniRect GetPrintableArea(PrinterObject printer, SystemPaperType paper, UniSize resolution, int orientation = 0)
        {
            var rc = new UniRect();
            DeviceMode dev = (DeviceMode)printer._DevMode.Clone();
            dev.Fields = DeviceModeFields.Orientation | DeviceModeFields.PaperSize | DeviceModeFields.YResolution;
            dev.YResolution = (short)resolution.cy;
            dev.PrintQuality = (short)resolution.cx;
            dev.PaperSize = paper;
            dev.Orientation = (short)orientation;
            var hdc = User32.CreateDC(null, printer.PrinterName, IntPtr.Zero, printer._DevMode._ptr);
            if (hdc != IntPtr.Zero)
            {
                int cx = PrinterModule.GetDeviceCaps(hdc, PrinterModule.PHYSICALWIDTH);
                int cy = PrinterModule.GetDeviceCaps(hdc, PrinterModule.PHYSICALHEIGHT);
                int marginX = PrinterModule.GetDeviceCaps(hdc, PrinterModule.PHYSICALOFFSETX);
                int marginY = PrinterModule.GetDeviceCaps(hdc, PrinterModule.PHYSICALOFFSETY);
                User32.DeleteDC(hdc);
                rc.Left = marginX / resolution.cx;
                rc.Top = marginY / resolution.cy;
                rc.Width = (cx - marginX * 2) / resolution.cx;
                rc.Height = (cy - marginY * 2) / resolution.cy;
            }

            return rc;
        }

        /// <summary>
        /// Get <see cref="PrinterObject"/> by name
        /// </summary>
        /// <param name="name">The name of the printer</param>
        /// <returns></returns>
        public static PrinterObject GetPrinterInfoObject(string name)
        {
            PrinterObject po = null;
            internalGetPrinter(name, ref po);
            return po;
        }

        [DllImport("winspool.drv", EntryPoint = "GetDefaultPrinterW", CharSet = CharSet.Unicode)]
        private static extern bool GetDefaultPrinter(IntPtr pszBuffer, ref uint pcchBuffer);

        /// <summary>
        /// Returns the default printer for the system.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static PrinterObject GetDefaultPrinter()
        {
            return new PrinterObject(DefaultPrinterName);
        }

        /// <summary>
        /// Returns the name of the default printer
        /// </summary>
        /// <returns></returns>
        public static string DefaultPrinterName
        {
            get
            {
                string DefaultPrinterNameRet = default;
                uint l = 0U;
                MemPtr mm = new MemPtr();
                GetDefaultPrinter(IntPtr.Zero, ref l);
                if (l <= 0L)
                    return null;
                l = (uint)(l * 2L + 2L);
                mm.Alloc(l);
                if (GetDefaultPrinter(mm, ref l))
                {
                    DefaultPrinterNameRet = (string)mm;
                }
                else
                {
                    DefaultPrinterNameRet = null;
                }

                mm.Free();
                return DefaultPrinterNameRet;
            }
        }

        private static void internalPopulatePrinter(string name, PrinterObject printer)
        {
            internalGetPrinter(name, ref printer);
        }

        private static void internalGetPrinter(string name, ref PrinterObject printer)
        {

            // MsgBox("We are in internalGetPrinter for " & name)

            var mm = new MemPtr();
            uint cb = 0U;
            var hprinter = IntPtr.Zero;

            if (string.IsNullOrEmpty(name))
            {
                // Interaction.MsgBox("Got null printer name.");
                printer = null;
                return;
            }

            if (!PrinterModule.OpenPrinter(name, ref hprinter, IntPtr.Zero))
            {
                // Interaction.MsgBox("OpenPrinter failed, last Win32 Error: " + NativeErrorMethods.FormatLastError((uint)Marshal.GetLastWin32Error()));
                return;
            }

            // MsgBox("Open Printer for '" & name & "' succeeded...")
            try
            {
                PrinterModule.GetPrinter(hprinter, 2U, IntPtr.Zero, 0U, ref cb);
                mm.Alloc(cb);
                PrinterModule.GetPrinter(hprinter, 2U, mm, cb, ref cb);
                if (printer is null)
                {
                    printer = new PrinterObject();
                    printer._ptr = mm;
                }
                else
                {
                    if (printer._ptr.Handle != IntPtr.Zero)
                    {
                        try
                        {
                            printer._ptr.Free();
                        }
                        catch
                        {
                        }
                    }
                    // we will be holding on to this.
                    printer._ptr = mm;
                }

                if (IntPtr.Size == 4)
                {
                    printer._DevMode = new DeviceMode((IntPtr)mm.IntAt(7L), false);
                }
                // MsgBox("Got a pointer! x86 mode.")
                else
                {
                    printer._DevMode = new DeviceMode((IntPtr)mm.LongAt(7L), false);
                    // MsgBox("Got a pointer! x64 mode.")
                    // If printer._DevMode IsNot Nothing Then MsgBox("DevMode retrieval successful, devmode reports device name as '" & printer._DevMode.DeviceName & "'")
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\sbslog.log", ex.Message + "\r\n" + ex.StackTrace + "\r\n" + "\r\n");
            }

            var sz = PrinterModule.GetMaxDPIForPrinter(printer, hprinter);
            PrinterModule.ClosePrinter(hprinter);
            internalPopulateDevCaps(ref printer);
        }

        private static void internalPopulateDevCaps(ref PrinterObject printer)
        {
            try
            {
                var mm = new MemPtr();

                // Get the supported resolutions.

                // MsgBox("Polling printer for available print quality resolution.")

                uint[] res;
                uint l = PrinterModule.DeviceCapabilities(printer.PrinterName, printer.PortName, PrinterModule.DC_ENUMRESOLUTIONS, IntPtr.Zero, IntPtr.Zero);
                if (printer.PrinterName.Contains("HP LaserJet"))
                {
                    if (l == 0xFFFFFFFFU)
                    {
                        // MsgBox("Attempt to get resolutions failed miserably, let's try it without the port name...")
                        l = PrinterModule.DeviceCapabilities(printer.PrinterName, null, PrinterModule.DC_ENUMRESOLUTIONS, IntPtr.Zero, IntPtr.Zero);
                        if (l == 0xFFFFFFFFU)
                        {
                            // MsgBox("That still failed.  We are going to give the LaserJet practical resolutions, 600x600 and 1200x1200 so that it won't barf")
                            var nRes = new List<System.Windows.Size>();
                            nRes.AddRange(new[] { new System.Windows.Size(600d, 600d), new System.Windows.Size(1200d, 1200d) });
                            printer._Resolutions = nRes;
                        }
                    }

                    if (l > 0L & l != 0xFFFFFFFFU)
                    {
                        // MsgBox("HP LaserJet SAYS it has " & l & " resolutions.")
                        res = null;
                        mm = new MemPtr(l * 8L);
                        l = PrinterModule.DeviceCapabilities(printer.PrinterName, printer.PortName, PrinterModule.DC_ENUMRESOLUTIONS, mm, IntPtr.Zero);

                        // MsgBox("Retrieved printer resolutions, RetVal=" & l)

                        try
                        {
                            // MsgBox("Casting memory into UInteger() array")
                            res = mm.ToArray<uint>();
                        }
                        catch
                        {
                            // Interaction.MsgBox("Getting Resolutions bounced!", MsgBoxStyle.Exclamation);
                        }

                        mm.Free();
                        if (res is object)
                        {
                            string stm = "";
                            foreach (var rn in res)
                            {
                                if (!string.IsNullOrEmpty(stm))
                                    stm += ", ";
                                stm += rn.ToString();
                            }

                            // MsgBox("Resolution raw data for LaserJet: " & stm)

                            var supRes = new List<System.Windows.Size>();

                            // MsgBox("Res count should be divisible by two, is it? Count: " & res.Count)

                            var resLen = res.Length;

                            for (int i = 0; i < resLen; i += 2)
                            {
                                if (res.Length % 2 != 0 && i == res.Length - 1)
                                {
                                    supRes.Add(new System.Windows.Size(res[i], res[i]));
                                }
                                else
                                {
                                    supRes.Add(new System.Windows.Size(res[i], res[i + 1]));
                                }
                            }

                            // MsgBox("Finally, we're going to report that the printer has " & supRes.Count & " resolutions.")
                            printer.Resolutions = supRes;
                        }
                    }
                }
                else if (l > 0L & l != 0xFFFFFFFFU)
                {
                    res = null;
                    mm = new MemPtr(l * 8L);
                    l = PrinterModule.DeviceCapabilities(printer.PrinterName, printer.PortName, PrinterModule.DC_ENUMRESOLUTIONS, mm, IntPtr.Zero);
                    try
                    {
                        res = mm.ToArray<uint>();
                    }
                    catch // (Exception ex)
                    {
                        // Interaction.MsgBox("Getting Resolutions bounced!", MsgBoxStyle.Exclamation);
                    }

                    mm.Free();
                    var supRes = new List<System.Windows.Size>();
                    if (res is object)
                    {
                        var resLen = res.Length;

                        for (int i = 0; i < resLen; i += 2)
                        {
                            if (res.Length % 2 != 0 && i == res.Length - 1)
                            {
                                supRes.Add(new System.Windows.Size(res[i], res[i]));
                            }
                            else
                            {
                                supRes.Add(new System.Windows.Size(res[i], res[i + 1]));
                            }
                        }

                        printer.Resolutions = supRes;
                    }
                    else
                    {
                        var nRes = new List<System.Windows.Size>();
                        nRes.AddRange(new[] { new System.Windows.Size(300d, 300d), new System.Windows.Size(600d, 600d), new System.Windows.Size(1200d, 1200d) });
                        printer._Resolutions = nRes;
                    }
                }

                // MsgBox("Found " & printer.Resolutions.Count & " resolutions.")

                // MsgBox("Getting paper class sizes")
                // Get the supported paper types.
                l = PrinterModule.DeviceCapabilities(printer.PrinterName, printer.PortName, PrinterModule.DC_PAPERS, IntPtr.Zero, IntPtr.Zero);

                // supported paper types are short ints:
                if (l > 0L)
                {
                    mm = new MemPtr(l * 2L);
                    l = PrinterModule.DeviceCapabilities(printer.PrinterName, printer.PortName, PrinterModule.DC_PAPERS, mm, IntPtr.Zero);
                    printer._PaperSizes = SystemPaperTypes.TypeListFromCodes(mm.ToArray<short>());
                    mm.Free();
                }

                // MsgBox("Retrieved " & l & " supported paper class sizes.")

                // MsgBox("Looking for the printer trays.")
                // get the names of the supported paper bins.
                l = PrinterModule.DeviceCapabilities(printer.PrinterName, printer.PortName, PrinterModule.DC_BINNAMES, IntPtr.Zero, IntPtr.Zero);
                if (l > 0L)
                {
                    mm = new MemPtr(l * 24L * 2L);
                    mm.ZeroMemory();

                    PrinterModule.DeviceCapabilities(printer.PrinterName, printer.PortName, PrinterModule.DC_BINNAMES, mm, IntPtr.Zero);
                    printer._Bins.Clear();

                    string srs;
                    int p;

                    for (long i = 0L; i < l; i++)
                    {

                        // some p.o.s. printers make it hard.
                        srs = null;
                        srs = mm.GetString(i * 24L * 2L, 24);
                        if (srs is object)
                        {
                            for (p = 0; p <= 23; p++)
                            {
                                if (srs[p] == '\0')
                                    break;
                            }

                            if (p < 24 && p != 0)
                            {
                                srs = srs.Substring(0, p);
                            }
                            else if (p == 0)
                            {
                                srs = "Unnamed Tray (#" + (i + 1L) + ")";
                            }

                            // MsgBox("Adding printer bin/tray " & srs)
                            printer._Bins.Add(srs);
                        }
                    }

                    mm.Free();
                }

                l = PrinterModule.DeviceCapabilities(printer.PrinterName, printer.PortName, PrinterModule.DC_COLORDEVICE, IntPtr.Zero, IntPtr.Zero);
                printer.IsColorPrinter = l == 0 ? false : true;
                printer._LandscapeRotation = (int)PrinterModule.DeviceCapabilities(printer.PrinterName, printer.PortName, PrinterModule.DC_ORIENTATION, IntPtr.Zero, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\sbslog.log", ex.Message + "\r\n" + ex.StackTrace + "\r\n" + "\r\n");
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Select a printer into this object by printer name.
        /// </summary>
        /// <param name="printerName"></param>
        /// <remarks></remarks>
        public void SelectPrinter(string printerName)
        {
            var argprinter = this;
            internalGetPrinter(printerName, ref argprinter);
        }

        /// <summary>
        /// Returns true if this printer is the Windows system default printer.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsDefault
        {
            get
            {
                return (PrinterName ?? "") == (DefaultPrinterName ?? "");
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        private int _LandscapeRotation;

        /// <summary>
        /// Returns the relationship between portrait and landscape orientations for a device, <br/>
        /// in terms of the number of degrees that portrait orientation is rotated counterclockwise <br/>
        /// to produce landscape orientation.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int LandscapeRotation
        {
            get
            {
                return _LandscapeRotation;
            }

            internal set
            {
                _LandscapeRotation = value;
            }
        }

        private bool _IsColor;

        /// <summary>
        /// Returns true if the printer is capable of color.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsColorPrinter
        {
            get
            {
                return _IsColor;
            }

            internal set
            {
                _IsColor = value;
            }
        }

        /// <summary>
        /// Reports the highest resolution that this printer is capable of printing in.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public System.Windows.Size HighestResolution
        {
            get
            {
                var hsize = new System.Windows.Size();
                foreach (var r in _Resolutions)
                {
                    if (r.Width > hsize.Width & r.Height > hsize.Height)
                    {
                        hsize = r;
                    }
                }

                return hsize;
            }
        }

        private List<System.Windows.Size> _Resolutions;

        /// <summary>
        /// Gets a list of all supported resolutions.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ICollection<System.Windows.Size> Resolutions
        {
            get
            {
                return _Resolutions;
            }

            internal set
            {
                _Resolutions = (List<System.Windows.Size>)value;
            }
        }

        private List<SystemPaperType> _PaperSizes;

        /// <summary>
        /// Gets a list of all paper sizes supported by this printer.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ICollection<SystemPaperType> PaperSizes
        {
            get
            {
                return _PaperSizes;
            }

            internal set
            {
                _PaperSizes = (List<SystemPaperType>)value;
            }
        }

        /// <summary>
        /// Gets a value indicating that this printer supports this particular paper size.
        /// </summary>
        /// <param name="size">The System.Windows.Size structure to compare.</param>
        /// <param name="sizeMetric">True if the given size is in millimeters.</param>
        /// <param name="exactOrientation">True to not compare rotated sizes.</param>
        /// <returns>True if all conditions are met and a size match is found.</returns>
        /// <remarks></remarks>
        public bool SupportsPaperSize(System.Windows.Size size, bool sizeMetric = false, bool exactOrientation = false)
        {

            // two separate for-eaches for time-saving. We don't need to test for sizeMetric for every iteration.
            // we don't need to test exactOrientation every time, either, but that's only referenced once per iteration.

            if (sizeMetric)
            {
                // testing for the millimeters size.
                foreach (var p in _PaperSizes)
                {
                    if (p.SizeMillimeters.Equals(size))
                        return true;
                    if (exactOrientation)
                        continue;
                    {
                        var withBlock = p.SizeMillimeters;
                        if (withBlock.Width == size.Height && withBlock.Height == size.Width)
                            return true;
                    }
                }
            }
            else
            {
                foreach (var p in _PaperSizes)
                {
                    if (p.SizeInches.Equals(size))
                        return true;
                    if (exactOrientation)
                        continue;
                    {
                        var withBlock1 = p.SizeInches;
                        if (withBlock1.Width == size.Height && withBlock1.Height == size.Width)
                            return true;
                    }
                }
            }

            // nothing found!
            return false;
        }

        private List<string> _Bins = new List<string>();

        /// <summary>
        /// Returns a list of all of the available trays this printer serves from.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public ICollection<string> PrinterTrays
        {
            get
            {
                return _Bins;
            }

            internal set
            {
                _Bins = (List<string>)value;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */
        /// <summary>
        /// Gets the server name of this printer.  If this string is null, the printer is served locally.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ServerName
        {
            get
            {
                return _ptr.GetStringIndirect(0 * IntPtr.Size);
            }

            internal set
            {
                _ptr.SetStringIndirect(0 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Gets the name of the printer.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string PrinterName
        {
            get
            {
                return _ptr.GetStringIndirect(1 * IntPtr.Size);
            }

            internal set
            {
                _ptr.SetStringIndirect(1 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Gets the share name of the printer.
        /// If this printer is not shared, this value is null.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string ShareName
        {
            get
            {
                return _ptr.GetStringIndirect(2 * IntPtr.Size);
            }

            internal set
            {
                _ptr.SetStringIndirect(2 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Gets the port name of the printer.  This could be a standard port, or a special port name.
        /// <br/>
        /// If a printer is connected to more than one port, the names of each port must be separated by commas (for example, "LPT1:,LPT2:,LPT3:").
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string PortName
        {
            get
            {
                return _ptr.GetStringIndirect(3 * IntPtr.Size);
            }

            internal set
            {
                _ptr.SetStringIndirect(3 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Returns the name of the printer driver.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string DriverName
        {
            get
            {
                return _ptr.GetStringIndirect(4 * IntPtr.Size);
            }

            internal set
            {
                _ptr.SetStringIndirect(4 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Gets a brief desecription of the printer.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Comment
        {
            get
            {
                return _ptr.GetStringIndirect(5 * IntPtr.Size);
            }

            internal set
            {
                _ptr.SetStringIndirect(5 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Gets a string that specifies the physical location of the printer (for example, "Bldg. 38, Room 1164").
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Location
        {
            get
            {
                return _ptr.GetStringIndirect(6 * IntPtr.Size);
            }

            internal set
            {
                _ptr.SetStringIndirect(6 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Gets the device mode object that reports on and controls further aspect of the printer's current configuration.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public DeviceMode DevMode
        {
            get
            {
                return _DevMode;
            }
        }

        /// <summary>
        /// Gets or sets a string that specifies the name of the file used to create the separator page. This page is used to separate print jobs sent to the printer.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string SepFile
        {
            get
            {
                return _ptr.GetStringIndirect(8 * IntPtr.Size);
            }

            set
            {
                _ptr.SetStringIndirect(8 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Gets or sets a string that specifies the name of the print processor used by the printer.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>You can use the EnumPrintProcessors function to obtain a list of print processors installed on a server.</remarks>
        public string PrintProcessor
        {
            get
            {
                return _ptr.GetStringIndirect(9 * IntPtr.Size);
            }

            set
            {
                _ptr.SetStringIndirect(9 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Gets or sets a string that specifies the data type used to record the print job.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks>You can use the EnumPrintProcessorDatatypes function to obtain a list of data types supported by a specific print processor.</remarks>
        public string Datatype
        {
            get
            {
                return _ptr.GetStringIndirect(10 * IntPtr.Size);
            }

            set
            {
                _ptr.SetStringIndirect(10 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Gets or sets a string that specifies the default print-processor parameters.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Parameters
        {
            get
            {
                return _ptr.GetStringIndirect(11 * IntPtr.Size);
            }

            set
            {
                _ptr.SetStringIndirect(11 * IntPtr.Size, value);
            }
        }

        /// <summary>
        /// Pointer to a SECURITY_DESCRIPTOR structure containing the ACL info.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public IntPtr SecurityDescriptor
        {
            get
            {
                return (IntPtr)_ptr.LongAt(12L);
            }

            internal set
            {
                _ptr.LongAt(12L) = (long)value;
            }
        }

        /// <summary>
        /// gets or sets the printer's attributes.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public PrinterAttributes Attributes
        {
            get
            {
                return (PrinterAttributes)(_ptr.UIntAt(26L));
            }

            set
            {
                _ptr.UIntAt(26L) = (uint)value;
            }
        }

        /// <summary>
        /// A priority value that the spooler uses to route print jobs.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint Priority
        {
            get
            {
                return _ptr.UIntAt(27L);
            }

            set
            {
                _ptr.UIntAt(27L) = value;
            }
        }

        /// <summary>
        /// The default priority value assigned to each print job.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint DefaultPriority
        {
            get
            {
                return _ptr.UIntAt(28L);
            }

            internal set
            {
                _ptr.UIntAt(28L) = value;
            }
        }

        /// <summary>
        /// The earliest time of day the printer will start taking print jobs for the day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public TimeSpan StartTime
        {
            get
            {
                return new TimeSpan(0, _ptr.IntAt(29L), 0);
            }

            internal set
            {
                _ptr.IntAt(29L) = (int)value.TotalMinutes;
            }
        }

        /// <summary>
        /// The time of day that the printer stops taking jobs for the day.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public TimeSpan UntilTime
        {
            get
            {
                return new TimeSpan(0, _ptr.IntAt(30L), 0);
            }

            internal set
            {
                _ptr.IntAt(30L) = (int)value.TotalMinutes;
            }
        }

        private bool _LiveUpdateStatus = false;

        /// <summary>
        /// Gets or sets a value indicating that the status will be updated live, every time it is retrieved.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool LiveUpdateStatus
        {
            get
            {
                return _LiveUpdateStatus;
            }

            set
            {
                _LiveUpdateStatus = value;
            }
        }

        /// <summary>
        /// Gets the current printer status.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public PrinterStatus Status
        {
            get
            {
                if (_LiveUpdateStatus)
                    return UpdateStatus();
                return (PrinterStatus)(_ptr.UIntAt(31L));
            }

            internal set
            {
                _ptr.UIntAt(31L) = (uint)value;
            }
        }

        /// <summary>
        /// Updates the printer's current status.
        /// </summary>
        /// <remarks></remarks>
        public PrinterStatus UpdateStatus()
        {
            uint cb = 0U;
            var hprinter = IntPtr.Zero;
            if (!PrinterModule.OpenPrinter(PrinterName, ref hprinter, IntPtr.Zero))
                return PrinterStatus.Error;
            PrinterModule.GetPrinter(hprinter, 6U, IntPtr.Zero, 0U, ref cb);
            if (cb > 16L)
            {
                _mm.ReAlloc(cb);
            }

            PrinterModule.GetPrinter(hprinter, 6U, _mm, cb, ref cb);
            _ptr.UIntAt(31L) = _mm.UIntAt(0L);
            PrinterModule.ClosePrinter(hprinter);
            return (PrinterStatus)(_ptr.UIntAt(31L));
        }

        /// <summary>
        /// The number of printer jobs in this printer's queue.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint cJobs
        {
            get
            {
                return _ptr.UIntAt(32L);
            }

            internal set
            {
                _ptr.UIntAt(32L) = value;
            }
        }

        /// <summary>
        /// This printer's average pages per minute.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public uint AveragePPM
        {
            get
            {
                return _ptr.UIntAt(33L);
            }

            internal set
            {
                _ptr.UIntAt(33L) = value;
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
        /* TODO ERROR: Skipped RegionDirectiveTrivia */

        /// <summary>
        /// Explicitly cast string to <see cref="PrinterObject"/>
        /// </summary>
        /// <param name="operand"></param>
        /// <returns></returns>
        public static explicit operator PrinterObject(string operand)
        {
            try
            {
                return new PrinterObject(operand);
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException("That printer was not found on the system.", ex);
            }
        }

        /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
    }

    public class DeviceMode : CriticalFinalizerObject, IDisposable, IEquatable<DeviceMode>, ICloneable
    {
        internal MemPtr _ptr;
        private bool _own = true;

        internal DeviceMode(IntPtr ptr, bool fOwn)
        {
            _ptr = ptr;
            _own = fOwn;
        }

        /// <summary>
        /// Device name
        /// </summary>
        /// <returns></returns>
        public string DeviceName
        {
            get
            {
                return _ptr.GetString(0L, 32).Trim('\0');
            }

            set
            {
                if (value.Length > 32)
                    value = value.Substring(0, 32);
                _ptr.SetString(0L, value);
            }
        }

        /// <summary>
        /// Spec version
        /// </summary>
        /// <returns></returns>
        public ushort SpecVersion
        {
            get
            {
                return _ptr.UShortAtAbsolute(64L);
            }

            set
            {
                _ptr.UShortAtAbsolute(64L) = value;
            }
        }

        /// <summary>
        /// Driver version
        /// </summary>
        /// <returns></returns>
        public ushort DriverVersion
        {
            get
            {
                return _ptr.UShortAtAbsolute(66L);
            }

            set
            {
                _ptr.UShortAtAbsolute(66L) = value;
            }
        }

        /// <summary>
        /// Size
        /// </summary>
        /// <returns></returns>
        public ushort Size
        {
            get
            {
                return _ptr.UShortAtAbsolute(68L);
            }

            set
            {
                _ptr.UShortAtAbsolute(68L) = value;
            }
        }

        /// <summary>
        /// Driver Extra
        /// </summary>
        /// <returns></returns>
        public ushort DriverExtra
        {
            get
            {
                return _ptr.UShortAtAbsolute(70L);
            }

            set
            {
                _ptr.UShortAtAbsolute(70L) = value;
            }
        }

        /// <summary>
        /// Device mode fields
        /// </summary>
        /// <returns></returns>
        public DeviceModeFields Fields
        {
            get
            {
                return (DeviceModeFields)(_ptr.UIntAtAbsolute(72L));
            }

            set
            {
                _ptr.UIntAtAbsolute(72L) = (uint)value;
            }
        }

        // union

        // struct

        public short Orientation
        {
            get
            {
                return _ptr.ShortAtAbsolute(76L);
            }

            set
            {
                _ptr.ShortAtAbsolute(76L) = value;
            }
        }

        public SystemPaperType PaperSize
        {
            get
            {
                return (SystemPaperType)_ptr.ShortAtAbsolute(78L);
            }

            set
            {
                _ptr.ShortAtAbsolute(78L) = (short)value.Code;
            }
        }

        public short PaperSizeCode
        {
            get
            {
                return _ptr.ShortAtAbsolute(78L);
            }

            set
            {
                _ptr.ShortAtAbsolute(78L) = value;
            }
        }

        public short PaperLength
        {
            get
            {
                return _ptr.ShortAtAbsolute(80L);
            }

            set
            {
                _ptr.ShortAtAbsolute(80L) = value;
            }
        }

        public short PaperWidth
        {
            get
            {
                return _ptr.ShortAtAbsolute(82L);
            }

            set
            {
                _ptr.ShortAtAbsolute(82L) = value;
            }
        }

        public short Scale
        {
            get
            {
                return _ptr.ShortAtAbsolute(84L);
            }

            set
            {
                _ptr.ShortAtAbsolute(84L) = value;
            }
        }

        public short Copies
        {
            get
            {
                return _ptr.ShortAtAbsolute(86L);
            }

            set
            {
                _ptr.ShortAtAbsolute(86L) = value;
            }
        }

        public short DefaultSource
        {
            get
            {
                return _ptr.ShortAtAbsolute(88L);
            }

            set
            {
                _ptr.ShortAtAbsolute(88L) = value;
            }
        }

        public short PrintQuality
        {
            get
            {
                return _ptr.ShortAtAbsolute(90L);
            }

            set
            {
                _ptr.ShortAtAbsolute(90L) = value;
            }
        }

        // struct

        public System.Drawing.Point Position
        {
            get
            {
                return new System.Drawing.Point(_ptr.IntAtAbsolute(76L), _ptr.IntAtAbsolute(80L));
            }

            set
            {
                _ptr.IntAtAbsolute(76L) = value.X;
                _ptr.IntAtAbsolute(80L) = value.Y;
            }
        }

        public uint DisplayOrientation
        {
            get
            {
                return _ptr.UIntAtAbsolute(84L);
            }

            set
            {
                _ptr.UIntAtAbsolute(84L) = value;
            }
        }

        public uint DisplayFixedOutput
        {
            get
            {
                return _ptr.UIntAtAbsolute(88L);
            }

            set
            {
                _ptr.UIntAtAbsolute(88L) = value;
            }
        }

        // end union

        public short Color
        {
            get
            {
                return _ptr.ShortAtAbsolute(92L);
            }

            set
            {
                _ptr.ShortAtAbsolute(92L) = value;
            }
        }

        public short Duplex
        {
            get
            {
                return _ptr.ShortAtAbsolute(94L);
            }

            set
            {
                _ptr.ShortAtAbsolute(94L) = value;
            }
        }

        public short YResolution
        {
            get
            {
                return _ptr.ShortAtAbsolute(96L);
            }

            set
            {
                _ptr.ShortAtAbsolute(96L) = value;
            }
        }

        public short TTOption
        {
            get
            {
                return _ptr.ShortAtAbsolute(98L);
            }

            set
            {
                _ptr.ShortAtAbsolute(98L) = value;
            }
        }

        public short Collate
        {
            get
            {
                return _ptr.ShortAtAbsolute(100L);
            }

            set
            {
                _ptr.ShortAtAbsolute(100L) = value;
            }
        }

        public string FormName
        {
            get
            {
                return _ptr.GetString(102L, 32).Trim('\0');
            }

            set
            {
                if (value.Length > 32)
                    value = value.Substring(0, 32);
                _ptr.SetString(102L, value);
            }
        }

        public ushort LogPixels
        {
            get
            {
                return _ptr.UShortAtAbsolute(168L);
            }

            set
            {
                _ptr.UShortAtAbsolute(168L) = value;
            }
        }

        public uint BitsPerPel
        {
            get
            {
                return _ptr.UIntAtAbsolute(170L);
            }

            set
            {
                _ptr.UIntAtAbsolute(170L) = value;
            }
        }

        public uint PelsWidth
        {
            get
            {
                return _ptr.UIntAtAbsolute(174L);
            }

            set
            {
                _ptr.UIntAtAbsolute(174L) = value;
            }
        }

        public uint PelsHeight
        {
            get
            {
                return _ptr.UIntAtAbsolute(178L);
            }

            set
            {
                _ptr.UIntAtAbsolute(178L) = value;
            }
        }

        // union

        public uint DisplayFlags
        {
            get
            {
                return _ptr.UIntAtAbsolute(182L);
            }

            set
            {
                _ptr.UIntAtAbsolute(182L) = value;
            }
        }

        public uint Nup
        {
            get
            {
                return _ptr.UIntAtAbsolute(182L);
            }

            set
            {
                _ptr.UIntAtAbsolute(182L) = value;
            }
        }

        // end union

        public uint DisplayFrequency
        {
            get
            {
                return _ptr.UIntAtAbsolute(186L);
            }

            set
            {
                _ptr.UIntAtAbsolute(186L) = value;
            }
        }

        public uint ICMMethod
        {
            get
            {
                return _ptr.UIntAtAbsolute(190L);
            }

            set
            {
                _ptr.UIntAtAbsolute(190L) = value;
            }
        }

        public uint ICMIntent
        {
            get
            {
                return _ptr.UIntAtAbsolute(194L);
            }

            set
            {
                _ptr.UIntAtAbsolute(194L) = value;
            }
        }

        public uint MediaType
        {
            get
            {
                return _ptr.UIntAtAbsolute(198L);
            }

            set
            {
                _ptr.UIntAtAbsolute(198L) = value;
            }
        }

        public uint DitherType
        {
            get
            {
                return _ptr.UIntAtAbsolute(202L);
            }

            set
            {
                _ptr.UIntAtAbsolute(202L) = value;
            }
        }

        public uint Reserved1
        {
            get
            {
                return _ptr.UIntAtAbsolute(206L);
            }

            set
            {
                _ptr.UIntAtAbsolute(206L) = value;
            }
        }

        public uint Reserved2
        {
            get
            {
                return _ptr.UIntAtAbsolute(210L);
            }

            set
            {
                _ptr.UIntAtAbsolute(210L) = value;
            }
        }

        public uint PanningWidth
        {
            get
            {
                return _ptr.UIntAtAbsolute(214L);
            }

            set
            {
                _ptr.UIntAtAbsolute(214L) = value;
            }
        }

        public uint PanningHeight
        {
            get
            {
                return _ptr.UIntAtAbsolute(218L);
            }

            set
            {
                _ptr.UIntAtAbsolute(218L) = value;
            }
        }

        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                if (_own)
                    _ptr.Free();
            }

            disposedValue = true;
        }

        ~DeviceMode()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Equals(DeviceMode other)
        {
            var pi = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            object o1;
            object o2;
            foreach (var pe in pi)
            {
                o1 = pe.GetValue(this);
                o2 = pe.GetValue(other);
                if (o1.Equals(o2) == false)
                    return false;
            }

            return true;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

}