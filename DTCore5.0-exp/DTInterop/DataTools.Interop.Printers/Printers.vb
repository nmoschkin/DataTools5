'' ************************************************* ''
'' DataTools Visual Basic Utility Library - Interop
''
'' Module: Printers
''         Windows Printer Api
''
''         Enums are documented in part from the API documentation at MSDN.
''         Other knowledge and references obtained through various sources
''         and all is considered public domain/common knowledge.
''
'' Copyright (C) 2011-2020 Nathan Moschkin
'' All Rights Reserved
''
'' Licensed Under the Microsoft Public License   
'' ************************************************* ''


Imports CoreCT.Memory
Imports DataTools.Interop.Native
Imports System.IO
Imports System.Reflection

Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Runtime.ConstrainedExecution
Imports CoreCT.Text

Imports DataTools.ExtendedMath.ColorMath
Imports System.Collections.ObjectModel

Namespace Printers

    <HideModuleName>
    Public Module PrinterModule

#Region "Constants"

#Region "Defined as Needed"

        'define DM_ORIENTATION          0x00000001L
        '#define DM_PAPERSIZE            0x00000002L
        '#define DM_PAPERLENGTH          0x00000004L
        '#define DM_PAPERWIDTH           0x00000008L
        '#define DM_SCALE                0x00000010L
        '#if(WINVER >= 0x0500)
        '#define DM_POSITION             0x00000020L
        '#define DM_NUP                  0x00000040L
        '#endif /* WINVER >= 0x0500 */
        '#if(WINVER >= 0x0501)
        '#define DM_DISPLAYORIENTATION   0x00000080L
        '#endif /* WINVER >= 0x0501 */
        '#define DM_COPIES               0x00000100L
        '#define DM_DEFAULTSOURCE        0x00000200L
        '#define DM_PRINTQUALITY         0x00000400L
        '#define DM_COLOR                0x00000800L
        '#define DM_DUPLEX               0x00001000L
        '#define DM_YRESOLUTION          0x00002000L
        '#define DM_TTOPTION             0x00004000L
        '#define DM_COLLATE              0x00008000L
        '#define DM_FORMNAME             0x00010000L
        '#define DM_LOGPIXELS            0x00020000L
        '#define DM_BITSPERPEL           0x00040000L
        '#define DM_PELSWIDTH            0x00080000L
        '#define DM_PELSHEIGHT           0x00100000L
        '#define DM_DISPLAYFLAGS         0x00200000L
        '#define DM_DISPLAYFREQUENCY     0x00400000L
        '#if(WINVER >= 0x0400)
        '#define DM_ICMMETHOD            0x00800000L
        '#define DM_ICMINTENT            0x01000000L
        '#define DM_MEDIATYPE            0x02000000L
        '#define DM_DITHERTYPE           0x04000000L
        '#define DM_PANNINGWIDTH         0x08000000L
        '#define DM_PANNINGHEIGHT        0x10000000L
        '#endif /* WINVER >= 0x0400 */
        '#if(WINVER >= 0x0501)
        '#define DM_DISPLAYFIXEDOUTPUT   0x20000000L
        '#endif /* WINVER >= 0x0501 */

        '/* orientation selections */
        '#define DMORIENT_PORTRAIT   1
        '#define DMORIENT_LANDSCAPE  2

        '/* paper selections */
        '#define DMPAPER_FIRST                DMPAPER_LETTER
        '#define DMPAPER_LETTER               1  /* Letter 8 1/2 x 11 in               */
        '#define DMPAPER_LETTERSMALL          2  /* Letter Small 8 1/2 x 11 in         */
        '#define DMPAPER_TABLOID              3  /* Tabloid 11 x 17 in                 */
        '#define DMPAPER_LEDGER               4  /* Ledger 17 x 11 in                  */
        '#define DMPAPER_LEGAL                5  /* Legal 8 1/2 x 14 in                */
        '#define DMPAPER_STATEMENT            6  /* Statement 5 1/2 x 8 1/2 in         */
        '#define DMPAPER_EXECUTIVE            7  /* Executive 7 1/4 x 10 1/2 in        */
        '#define DMPAPER_A3                   8  /* A3 297 x 420 mm                    */
        '#define DMPAPER_A4                   9  /* A4 210 x 297 mm                    */
        '#define DMPAPER_A4SMALL             10  /* A4 Small 210 x 297 mm              */
        '#define DMPAPER_A5                  11  /* A5 148 x 210 mm                    */
        '#define DMPAPER_B4                  12  /* B4 (JIS) 250 x 354                 */
        '#define DMPAPER_B5                  13  /* B5 (JIS) 182 x 257 mm              */
        '#define DMPAPER_FOLIO               14  /* Folio 8 1/2 x 13 in                */
        '#define DMPAPER_QUARTO              15  /* Quarto 215 x 275 mm                */
        '#define DMPAPER_10X14               16  /* 10x14 in                           */
        '#define DMPAPER_11X17               17  /* 11x17 in                           */
        '#define DMPAPER_NOTE                18  /* Note 8 1/2 x 11 in                 */
        '#define DMPAPER_ENV_9               19  /* Envelope #9 3 7/8 x 8 7/8          */
        '#define DMPAPER_ENV_10              20  /* Envelope #10 4 1/8 x 9 1/2         */
        '#define DMPAPER_ENV_11              21  /* Envelope #11 4 1/2 x 10 3/8        */
        '#define DMPAPER_ENV_12              22  /* Envelope #12 4 \276 x 11           */
        '#define DMPAPER_ENV_14              23  /* Envelope #14 5 x 11 1/2            */
        '#define DMPAPER_CSHEET              24  /* C size sheet                       */
        '#define DMPAPER_DSHEET              25  /* D size sheet                       */
        '#define DMPAPER_ESHEET              26  /* E size sheet                       */
        '#define DMPAPER_ENV_DL              27  /* Envelope DL 110 x 220mm            */
        '#define DMPAPER_ENV_C5              28  /* Envelope C5 162 x 229 mm           */
        '#define DMPAPER_ENV_C3              29  /* Envelope C3  324 x 458 mm          */
        '#define DMPAPER_ENV_C4              30  /* Envelope C4  229 x 324 mm          */
        '#define DMPAPER_ENV_C6              31  /* Envelope C6  114 x 162 mm          */
        '#define DMPAPER_ENV_C65             32  /* Envelope C65 114 x 229 mm          */
        '#define DMPAPER_ENV_B4              33  /* Envelope B4  250 x 353 mm          */
        '#define DMPAPER_ENV_B5              34  /* Envelope B5  176 x 250 mm          */
        '#define DMPAPER_ENV_B6              35  /* Envelope B6  176 x 125 mm          */
        '#define DMPAPER_ENV_ITALY           36  /* Envelope 110 x 230 mm              */
        '#define DMPAPER_ENV_MONARCH         37  /* Envelope Monarch 3.875 x 7.5 in    */
        '#define DMPAPER_ENV_PERSONAL        38  /* 6 3/4 Envelope 3 5/8 x 6 1/2 in    */
        '#define DMPAPER_FANFOLD_US          39  /* US Std Fanfold 14 7/8 x 11 in      */
        '#define DMPAPER_FANFOLD_STD_GERMAN  40  /* German Std Fanfold 8 1/2 x 12 in   */
        '#define DMPAPER_FANFOLD_LGL_GERMAN  41  /* German Legal Fanfold 8 1/2 x 13 in */
        '#if(WINVER >= 0x0400)
        '#define DMPAPER_ISO_B4              42  /* B4 (ISO) 250 x 353 mm              */
        '#define DMPAPER_JAPANESE_POSTCARD   43  /* Japanese Postcard 100 x 148 mm     */
        '#define DMPAPER_9X11                44  /* 9 x 11 in                          */
        '#define DMPAPER_10X11               45  /* 10 x 11 in                         */
        '#define DMPAPER_15X11               46  /* 15 x 11 in                         */
        '#define DMPAPER_ENV_INVITE          47  /* Envelope Invite 220 x 220 mm       */
        '#define DMPAPER_RESERVED_48         48  /* RESERVED--DO NOT USE               */
        '#define DMPAPER_RESERVED_49         49  /* RESERVED--DO NOT USE               */
        '#define DMPAPER_LETTER_EXTRA        50  /* Letter Extra 9 \275 x 12 in        */
        '#define DMPAPER_LEGAL_EXTRA         51  /* Legal Extra 9 \275 x 15 in         */
        '#define DMPAPER_TABLOID_EXTRA       52  /* Tabloid Extra 11.69 x 18 in        */
        '#define DMPAPER_A4_EXTRA            53  /* A4 Extra 9.27 x 12.69 in           */
        '#define DMPAPER_LETTER_TRANSVERSE   54  /* Letter Transverse 8 \275 x 11 in   */
        '#define DMPAPER_A4_TRANSVERSE       55  /* A4 Transverse 210 x 297 mm         */
        '#define DMPAPER_LETTER_EXTRA_TRANSVERSE 56 /* Letter Extra Transverse 9\275 x 12 in */
        '#define DMPAPER_A_PLUS              57  /* SuperA/SuperA/A4 227 x 356 mm      */
        '#define DMPAPER_B_PLUS              58  /* SuperB/SuperB/A3 305 x 487 mm      */
        '#define DMPAPER_LETTER_PLUS         59  /* Letter Plus 8.5 x 12.69 in         */
        '#define DMPAPER_A4_PLUS             60  /* A4 Plus 210 x 330 mm               */
        '#define DMPAPER_A5_TRANSVERSE       61  /* A5 Transverse 148 x 210 mm         */
        '#define DMPAPER_B5_TRANSVERSE       62  /* B5 (JIS) Transverse 182 x 257 mm   */
        '#define DMPAPER_A3_EXTRA            63  /* A3 Extra 322 x 445 mm              */
        '#define DMPAPER_A5_EXTRA            64  /* A5 Extra 174 x 235 mm              */
        '#define DMPAPER_B5_EXTRA            65  /* B5 (ISO) Extra 201 x 276 mm        */
        '#define DMPAPER_A2                  66  /* A2 420 x 594 mm                    */
        '#define DMPAPER_A3_TRANSVERSE       67  /* A3 Transverse 297 x 420 mm         */
        '#define DMPAPER_A3_EXTRA_TRANSVERSE 68  /* A3 Extra Transverse 322 x 445 mm   */
        '#endif /* WINVER >= 0x0400 */

        '#if(WINVER >= 0x0500)
        '#define DMPAPER_DBL_JAPANESE_POSTCARD 69 /* Japanese Double Postcard 200 x 148 mm */
        '#define DMPAPER_A6                  70  /* A6 105 x 148 mm                 */
        '#define DMPAPER_JENV_KAKU2          71  /* Japanese Envelope Kaku #2       */
        '#define DMPAPER_JENV_KAKU3          72  /* Japanese Envelope Kaku #3       */
        '#define DMPAPER_JENV_CHOU3          73  /* Japanese Envelope Chou #3       */
        '#define DMPAPER_JENV_CHOU4          74  /* Japanese Envelope Chou #4       */
        '#define DMPAPER_LETTER_ROTATED      75  /* Letter Rotated 11 x 8 1/2 11 in */
        '#define DMPAPER_A3_ROTATED          76  /* A3 Rotated 420 x 297 mm         */
        '#define DMPAPER_A4_ROTATED          77  /* A4 Rotated 297 x 210 mm         */
        '#define DMPAPER_A5_ROTATED          78  /* A5 Rotated 210 x 148 mm         */
        '#define DMPAPER_B4_JIS_ROTATED      79  /* B4 (JIS) Rotated 364 x 257 mm   */
        '#define DMPAPER_B5_JIS_ROTATED      80  /* B5 (JIS) Rotated 257 x 182 mm   */
        '#define DMPAPER_JAPANESE_POSTCARD_ROTATED 81 /* Japanese Postcard Rotated 148 x 100 mm */
        '#define DMPAPER_DBL_JAPANESE_POSTCARD_ROTATED 82 /* Double Japanese Postcard Rotated 148 x 200 mm */
        '#define DMPAPER_A6_ROTATED          83  /* A6 Rotated 148 x 105 mm         */
        '#define DMPAPER_JENV_KAKU2_ROTATED  84  /* Japanese Envelope Kaku #2 Rotated */
        '#define DMPAPER_JENV_KAKU3_ROTATED  85  /* Japanese Envelope Kaku #3 Rotated */
        '#define DMPAPER_JENV_CHOU3_ROTATED  86  /* Japanese Envelope Chou #3 Rotated */
        '#define DMPAPER_JENV_CHOU4_ROTATED  87  /* Japanese Envelope Chou #4 Rotated */
        '#define DMPAPER_B6_JIS              88  /* B6 (JIS) 128 x 182 mm           */
        '#define DMPAPER_B6_JIS_ROTATED      89  /* B6 (JIS) Rotated 182 x 128 mm   */
        '#define DMPAPER_12X11               90  /* 12 x 11 in                      */
        '#define DMPAPER_JENV_YOU4           91  /* Japanese Envelope You #4        */
        '#define DMPAPER_JENV_YOU4_ROTATED   92  /* Japanese Envelope You #4 Rotated*/
        '#define DMPAPER_P16K                93  /* PRC 16K 146 x 215 mm            */
        '#define DMPAPER_P32K                94  /* PRC 32K 97 x 151 mm             */
        '#define DMPAPER_P32KBIG             95  /* PRC 32K(Big) 97 x 151 mm        */
        '#define DMPAPER_PENV_1              96  /* PRC Envelope #1 102 x 165 mm    */
        '#define DMPAPER_PENV_2              97  /* PRC Envelope #2 102 x 176 mm    */
        '#define DMPAPER_PENV_3              98  /* PRC Envelope #3 125 x 176 mm    */
        '#define DMPAPER_PENV_4              99  /* PRC Envelope #4 110 x 208 mm    */
        '#define DMPAPER_PENV_5              100 /* PRC Envelope #5 110 x 220 mm    */
        '#define DMPAPER_PENV_6              101 /* PRC Envelope #6 120 x 230 mm    */
        '#define DMPAPER_PENV_7              102 /* PRC Envelope #7 160 x 230 mm    */
        '#define DMPAPER_PENV_8              103 /* PRC Envelope #8 120 x 309 mm    */
        '#define DMPAPER_PENV_9              104 /* PRC Envelope #9 229 x 324 mm    */
        '#define DMPAPER_PENV_10             105 /* PRC Envelope #10 324 x 458 mm   */
        '#define DMPAPER_P16K_ROTATED        106 /* PRC 16K Rotated                 */
        '#define DMPAPER_P32K_ROTATED        107 /* PRC 32K Rotated                 */
        '#define DMPAPER_P32KBIG_ROTATED     108 /* PRC 32K(Big) Rotated            */
        '#define DMPAPER_PENV_1_ROTATED      109 /* PRC Envelope #1 Rotated 165 x 102 mm */
        '#define DMPAPER_PENV_2_ROTATED      110 /* PRC Envelope #2 Rotated 176 x 102 mm */
        '#define DMPAPER_PENV_3_ROTATED      111 /* PRC Envelope #3 Rotated 176 x 125 mm */
        '#define DMPAPER_PENV_4_ROTATED      112 /* PRC Envelope #4 Rotated 208 x 110 mm */
        '#define DMPAPER_PENV_5_ROTATED      113 /* PRC Envelope #5 Rotated 220 x 110 mm */
        '#define DMPAPER_PENV_6_ROTATED      114 /* PRC Envelope #6 Rotated 230 x 120 mm */
        '#define DMPAPER_PENV_7_ROTATED      115 /* PRC Envelope #7 Rotated 230 x 160 mm */
        '#define DMPAPER_PENV_8_ROTATED      116 /* PRC Envelope #8 Rotated 309 x 120 mm */
        '#define DMPAPER_PENV_9_ROTATED      117 /* PRC Envelope #9 Rotated 324 x 229 mm */
        '#define DMPAPER_PENV_10_ROTATED     118 /* PRC Envelope #10 Rotated 458 x 324 mm */
        '#endif /* WINVER >= 0x0500 */

        '#if (WINVER >= 0x0500)
        '#define DMPAPER_LAST                DMPAPER_PENV_10_ROTATED
        '#elif (WINVER >= 0x0400)
        '#define DMPAPER_LAST                DMPAPER_A3_EXTRA_TRANSVERSE
        '#Else
        '#define DMPAPER_LAST                DMPAPER_FANFOLD_LGL_GERMAN
        '#End If

        '#define DMPAPER_USER                256

        '/* bin selections */
        '#define DMBIN_FIRST         DMBIN_UPPER
        '#define DMBIN_UPPER         1
        '#define DMBIN_ONLYONE       1
        '#define DMBIN_LOWER         2
        '#define DMBIN_MIDDLE        3
        '#define DMBIN_MANUAL        4
        '#define DMBIN_ENVELOPE      5
        '#define DMBIN_ENVMANUAL     6
        '#define DMBIN_AUTO          7
        '#define DMBIN_TRACTOR       8
        '#define DMBIN_SMALLFMT      9
        '#define DMBIN_LARGEFMT      10
        '#define DMBIN_LARGECAPACITY 11
        '#define DMBIN_CASSETTE      14
        '#define DMBIN_FORMSOURCE    15
        '#define DMBIN_LAST          DMBIN_FORMSOURCE

        '#define DMBIN_USER          256     /* device specific bins start here */

        '/* print qualities */
        '#define DMRES_DRAFT         (-1)
        '#define DMRES_LOW           (-2)
        '#define DMRES_MEDIUM        (-3)
        '#define DMRES_HIGH          (-4)

        '/* color enable/disable for color printers */
        '#define DMCOLOR_MONOCHROME  1
        '#define DMCOLOR_COLOR       2

        '/* duplex enable */
        '#define DMDUP_SIMPLEX    1
        '#define DMDUP_VERTICAL   2
        '#define DMDUP_HORIZONTAL 3

        '/* TrueType options */
        '#define DMTT_BITMAP     1       /* print TT fonts as graphics */
        '#define DMTT_DOWNLOAD   2       /* download TT fonts as soft fonts */
        '#define DMTT_SUBDEV     3       /* substitute device fonts for TT fonts */
        '#if(WINVER >= 0x0400)
        '#define DMTT_DOWNLOAD_OUTLINE 4 /* download TT fonts as outline soft fonts */
        '#endif /* WINVER >= 0x0400 */

        '/* Collation selections */
        '#define DMCOLLATE_FALSE  0
        '#define DMCOLLATE_TRUE   1

        '#if(WINVER >= 0x0501)
        '/* DEVMODE dmDisplayOrientation specifiations */
        '#define DMDO_DEFAULT    0
        '#define DMDO_90         1
        '#define DMDO_180        2
        '#define DMDO_270        3

        '/* DEVMODE dmDisplayFixedOutput specifiations */
        '#define DMDFO_DEFAULT   0
        '#define DMDFO_STRETCH   1
        '#define DMDFO_CENTER    2
        '#endif /* WINVER >= 0x0501 */

        '/* DEVMODE dmDisplayFlags flags */

        '// #define DM_GRAYSCALE            0x00000001 /* This flag is no longer valid */
        '#define DM_INTERLACED           0x00000002
        '#define DMDISPLAYFLAGS_TEXTMODE 0x00000004

        '/* dmNup , multiple logical page per physical page options */
        '#define DMNUP_SYSTEM        1
        '#define DMNUP_ONEUP         2

        '#if(WINVER >= 0x0400)
        '/* ICM methods */
        '#define DMICMMETHOD_NONE    1   /* ICM disabled */
        '#define DMICMMETHOD_SYSTEM  2   /* ICM handled by system */
        '#define DMICMMETHOD_DRIVER  3   /* ICM handled by driver */
        '#define DMICMMETHOD_DEVICE  4   /* ICM handled by device */

        '#define DMICMMETHOD_USER  256   /* Device-specific methods start here */

        '/* ICM Intents */
        '#define DMICM_SATURATE          1   /* Maximize color saturation */
        '#define DMICM_CONTRAST          2   /* Maximize color contrast */
        '#define DMICM_COLORIMETRIC       3   /* Use specific color metric */
        '#define DMICM_ABS_COLORIMETRIC   4   /* Use specific color metric */

        '#define DMICM_USER        256   /* Device-specific intents start here */

        '/* Media types */

        '#define DMMEDIA_STANDARD      1   /* Standard paper */
        '#define DMMEDIA_TRANSPARENCY  2   /* Transparency */
        '#define DMMEDIA_GLOSSY        3   /* Glossy paper */

        '#define DMMEDIA_USER        256   /* Device-specific media start here */

        '/* Dither types */
        '#define DMDITHER_NONE       1      /* No dithering */
        '#define DMDITHER_COARSE     2      /* Dither with a coarse brush */
        '#define DMDITHER_FINE       3      /* Dither with a fine brush */
        '#define DMDITHER_LINEART    4      /* LineArt dithering */
        '#define DMDITHER_ERRORDIFFUSION 5  /* LineArt dithering */
        '#define DMDITHER_RESERVED6      6      /* LineArt dithering */
        '#define DMDITHER_RESERVED7      7      /* LineArt dithering */
        '#define DMDITHER_RESERVED8      8      /* LineArt dithering */
        '#define DMDITHER_RESERVED9      9      /* LineArt dithering */
        '#define DMDITHER_GRAYSCALE  10     /* Device does grayscaling */

        '#define DMDITHER_USER     256   /* Device-specific dithers start here */
        '#

#End Region

        '' DevMode extras

        Public Const DMRES_DRAFT = (-1)
        Public Const DMRES_LOW = (-2)
        Public Const DMRES_MEDIUM = (-3)
        Public Const DMRES_HIGH = (-4)

        ''  field selection bits 
        Public Const DM_ORIENTATION = &H1
        Public Const DM_PAPERSIZE = &H2
        Public Const DM_PAPERLENGTH = &H4
        Public Const DM_PAPERWIDTH = &H8
        Public Const DM_SCALE = &H10
        '' if(WINVER >= 0x0500)
        Public Const DM_POSITION = &H20
        Public Const DM_NUP = &H40
        '' endif ''  WINVER >= 0x0500 
        '' if(WINVER >= 0x0501)
        Public Const DM_DISPLAYORIENTATION = &H80
        '' endif ''  WINVER >= 0x0501 
        Public Const DM_COPIES = &H100
        Public Const DM_DEFAULTSOURCE = &H200
        Public Const DM_PRINTQUALITY = &H400
        Public Const DM_COLOR = &H800
        Public Const DM_DUPLEX = &H1000
        Public Const DM_YRESOLUTION = &H2000
        Public Const DM_TTOPTION = &H4000
        Public Const DM_COLLATE = &H8000
        Public Const DM_FORMNAME = &H10000
        Public Const DM_LOGPIXELS = &H20000
        Public Const DM_BITSPERPEL = &H40000
        Public Const DM_PELSWIDTH = &H80000
        Public Const DM_PELSHEIGHT = &H100000
        Public Const DM_DISPLAYFLAGS = &H200000
        Public Const DM_DISPLAYFREQUENCY = &H400000
        '' if(WINVER >= 0x0400)
        Public Const DM_ICMMETHOD = &H800000
        Public Const DM_ICMINTENT = &H1000000
        Public Const DM_MEDIATYPE = &H2000000
        Public Const DM_DITHERTYPE = &H4000000
        Public Const DM_PANNINGWIDTH = &H8000000
        Public Const DM_PANNINGHEIGHT = &H10000000
        '' endif ''  WINVER >= 0x0400 
        '' if(WINVER >= 0x0501)
        Public Const DM_DISPLAYFIXEDOUTPUT = &H20000000
        '' endif ''  WINVER >= 0x0501 


        ''  orientation selections 
        Public Const DMORIENT_PORTRAIT = 1
        Public Const DMORIENT_LANDSCAPE = 2 ''  paper selections 
        Public Const DMPAPER_FIRST = DMPAPER_LETTER
        Public Const DMPAPER_LETTER = 1 ''  Letter 8 1/2 x 11 in               
        Public Const DMPAPER_LETTERSMALL = 2 ''  Letter Small 8 1/2 x 11 in         
        Public Const DMPAPER_TABLOID = 3 ''  Tabloid 11 x 17 in                 
        Public Const DMPAPER_LEDGER = 4 ''  Ledger 17 x 11 in                  
        Public Const DMPAPER_LEGAL = 5 ''  Legal 8 1/2 x 14 in                
        Public Const DMPAPER_STATEMENT = 6 ''  Statement 5 1/2 x 8 1/2 in         
        Public Const DMPAPER_EXECUTIVE = 7 ''  Executive 7 1/4 x 10 1/2 in        
        Public Const DMPAPER_A3 = 8 ''  A3 297 x 420 mm                    
        Public Const DMPAPER_A4 = 9 ''  A4 210 x 297 mm                    
        Public Const DMPAPER_A4SMALL = 10 ''  A4 Small 210 x 297 mm              
        Public Const DMPAPER_A5 = 11 ''  A5 148 x 210 mm                    
        Public Const DMPAPER_B4 = 12 ''  B4 (JIS) 250 x 354                 
        Public Const DMPAPER_B5 = 13 ''  B5 (JIS) 182 x 257 mm              
        Public Const DMPAPER_FOLIO = 14 ''  Folio 8 1/2 x 13 in                
        Public Const DMPAPER_QUARTO = 15 ''  Quarto 215 x 275 mm                
        Public Const DMPAPER_10X14 = 16 ''  10x14 in                           
        Public Const DMPAPER_11X17 = 17 ''  11x17 in                           
        Public Const DMPAPER_NOTE = 18 ''  Note 8 1/2 x 11 in                 
        Public Const DMPAPER_ENV_9 = 19 ''  Envelope #9 3 7/8 x 8 7/8          
        Public Const DMPAPER_ENV_10 = 20 ''  Envelope #10 4 1/8 x 9 1/2         
        Public Const DMPAPER_ENV_11 = 21 ''  Envelope #11 4 1/2 x 10 3/8        
        Public Const DMPAPER_ENV_12 = 22 ''  Envelope #12 4 \276 x 11           
        Public Const DMPAPER_ENV_14 = 23 ''  Envelope #14 5 x 11 1/2            
        Public Const DMPAPER_CSHEET = 24 ''  C size sheet                       
        Public Const DMPAPER_DSHEET = 25 ''  D size sheet                       
        Public Const DMPAPER_ESHEET = 26 ''  E size sheet                       
        Public Const DMPAPER_ENV_DL = 27 ''  Envelope DL 110 x 220mm            
        Public Const DMPAPER_ENV_C5 = 28 ''  Envelope C5 162 x 229 mm           
        Public Const DMPAPER_ENV_C3 = 29 ''  Envelope C3  324 x 458 mm          
        Public Const DMPAPER_ENV_C4 = 30 ''  Envelope C4  229 x 324 mm          
        Public Const DMPAPER_ENV_C6 = 31 ''  Envelope C6  114 x 162 mm          
        Public Const DMPAPER_ENV_C65 = 32 ''  Envelope C65 114 x 229 mm          
        Public Const DMPAPER_ENV_B4 = 33 ''  Envelope B4  250 x 353 mm          
        Public Const DMPAPER_ENV_B5 = 34 ''  Envelope B5  176 x 250 mm          
        Public Const DMPAPER_ENV_B6 = 35 ''  Envelope B6  176 x 125 mm          
        Public Const DMPAPER_ENV_ITALY = 36 ''  Envelope 110 x 230 mm              
        Public Const DMPAPER_ENV_MONARCH = 37 ''  Envelope Monarch 3.875 x 7.5 in    
        Public Const DMPAPER_ENV_PERSONAL = 38 ''  6 3/4 Envelope 3 5/8 x 6 1/2 in    
        Public Const DMPAPER_FANFOLD_US = 39 ''  US Std Fanfold 14 7/8 x 11 in      
        Public Const DMPAPER_FANFOLD_STD_GERMAN = 40 ''  German Std Fanfold 8 1/2 x 12 in   
        Public Const DMPAPER_FANFOLD_LGL_GERMAN = 41 ''  German Legal Fanfold 8 1/2 x 13 in 
        '' if(WINVER >= 0x0400)
        Public Const DMPAPER_ISO_B4 = 42 ''  B4 (ISO) 250 x 353 mm              
        Public Const DMPAPER_JAPANESE_POSTCARD = 43 ''  Japanese Postcard 100 x 148 mm     
        Public Const DMPAPER_9X11 = 44 ''  9 x 11 in                          
        Public Const DMPAPER_10X11 = 45 ''  10 x 11 in                         
        Public Const DMPAPER_15X11 = 46 ''  15 x 11 in                         
        Public Const DMPAPER_ENV_INVITE = 47 ''  Envelope Invite 220 x 220 mm       
        Public Const DMPAPER_RESERVED_48 = 48 ''  RESERVED--DO NOT USE               
        Public Const DMPAPER_RESERVED_49 = 49 ''  RESERVED--DO NOT USE               
        Public Const DMPAPER_LETTER_EXTRA = 50 ''  Letter Extra 9 \275 x 12 in        
        Public Const DMPAPER_LEGAL_EXTRA = 51 ''  Legal Extra 9 \275 x 15 in         
        Public Const DMPAPER_TABLOID_EXTRA = 52 ''  Tabloid Extra 11.69 x 18 in        
        Public Const DMPAPER_A4_EXTRA = 53 ''  A4 Extra 9.27 x 12.69 in           
        Public Const DMPAPER_LETTER_TRANSVERSE = 54 ''  Letter Transverse 8 \275 x 11 in   
        Public Const DMPAPER_A4_TRANSVERSE = 55 ''  A4 Transverse 210 x 297 mm         
        Public Const DMPAPER_LETTER_EXTRA_TRANSVERSE = 56 ''  Letter Extra Transverse 9\275 x 12 in 
        Public Const DMPAPER_A_PLUS = 57 ''  SuperA/SuperA/A4 227 x 356 mm      
        Public Const DMPAPER_B_PLUS = 58 ''  SuperB/SuperB/A3 305 x 487 mm      
        Public Const DMPAPER_LETTER_PLUS = 59 ''  Letter Plus 8.5 x 12.69 in         
        Public Const DMPAPER_A4_PLUS = 60 ''  A4 Plus 210 x 330 mm               
        Public Const DMPAPER_A5_TRANSVERSE = 61 ''  A5 Transverse 148 x 210 mm         
        Public Const DMPAPER_B5_TRANSVERSE = 62 ''  B5 (JIS) Transverse 182 x 257 mm   
        Public Const DMPAPER_A3_EXTRA = 63 ''  A3 Extra 322 x 445 mm              
        Public Const DMPAPER_A5_EXTRA = 64 ''  A5 Extra 174 x 235 mm              
        Public Const DMPAPER_B5_EXTRA = 65 ''  B5 (ISO) Extra 201 x 276 mm        
        Public Const DMPAPER_A2 = 66 ''  A2 420 x 594 mm                    
        Public Const DMPAPER_A3_TRANSVERSE = 67 ''  A3 Transverse 297 x 420 mm         
        Public Const DMPAPER_A3_EXTRA_TRANSVERSE = 68 ''  A3 Extra Transverse 322 x 445 mm   
        '' endif ''  WINVER >= 0x0400 

        '' if(WINVER >= 0x0500)
        Public Const DMPAPER_DBL_JAPANESE_POSTCARD = 69 ''  Japanese Double Postcard 200 x 148 mm 
        Public Const DMPAPER_A6 = 70 ''  A6 105 x 148 mm                 
        Public Const DMPAPER_JENV_KAKU2 = 71 ''  Japanese Envelope Kaku #2       
        Public Const DMPAPER_JENV_KAKU3 = 72 ''  Japanese Envelope Kaku #3       
        Public Const DMPAPER_JENV_CHOU3 = 73 ''  Japanese Envelope Chou #3       
        Public Const DMPAPER_JENV_CHOU4 = 74 ''  Japanese Envelope Chou #4       
        Public Const DMPAPER_LETTER_ROTATED = 75 ''  Letter Rotated 11 x 8 1/2 11 in 
        Public Const DMPAPER_A3_ROTATED = 76 ''  A3 Rotated 420 x 297 mm         
        Public Const DMPAPER_A4_ROTATED = 77 ''  A4 Rotated 297 x 210 mm         
        Public Const DMPAPER_A5_ROTATED = 78 ''  A5 Rotated 210 x 148 mm         
        Public Const DMPAPER_B4_JIS_ROTATED = 79 ''  B4 (JIS) Rotated 364 x 257 mm   
        Public Const DMPAPER_B5_JIS_ROTATED = 80 ''  B5 (JIS) Rotated 257 x 182 mm   
        Public Const DMPAPER_JAPANESE_POSTCARD_ROTATED = 81 ''  Japanese Postcard Rotated 148 x 100 mm 
        Public Const DMPAPER_DBL_JAPANESE_POSTCARD_ROTATED = 82 ''  Double Japanese Postcard Rotated 148 x 200 mm 
        Public Const DMPAPER_A6_ROTATED = 83 ''  A6 Rotated 148 x 105 mm         
        Public Const DMPAPER_JENV_KAKU2_ROTATED = 84 ''  Japanese Envelope Kaku #2 Rotated 
        Public Const DMPAPER_JENV_KAKU3_ROTATED = 85 ''  Japanese Envelope Kaku #3 Rotated 
        Public Const DMPAPER_JENV_CHOU3_ROTATED = 86 ''  Japanese Envelope Chou #3 Rotated 
        Public Const DMPAPER_JENV_CHOU4_ROTATED = 87 ''  Japanese Envelope Chou #4 Rotated 
        Public Const DMPAPER_B6_JIS = 88 ''  B6 (JIS) 128 x 182 mm           
        Public Const DMPAPER_B6_JIS_ROTATED = 89 ''  B6 (JIS) Rotated 182 x 128 mm   
        Public Const DMPAPER_12X11 = 90 ''  12 x 11 in                      
        Public Const DMPAPER_JENV_YOU4 = 91 ''  Japanese Envelope You #4        
        Public Const DMPAPER_JENV_YOU4_ROTATED = 92 ''  Japanese Envelope You #4 Rotated
        Public Const DMPAPER_P16K = 93 ''  PRC 16K 146 x 215 mm            
        Public Const DMPAPER_P32K = 94 ''  PRC 32K 97 x 151 mm             
        Public Const DMPAPER_P32KBIG = 95 ''  PRC 32K(Big) 97 x 151 mm        
        Public Const DMPAPER_PENV_1 = 96 ''  PRC Envelope #1 102 x 165 mm    
        Public Const DMPAPER_PENV_2 = 97 ''  PRC Envelope #2 102 x 176 mm    
        Public Const DMPAPER_PENV_3 = 98 ''  PRC Envelope #3 125 x 176 mm    
        Public Const DMPAPER_PENV_4 = 99 ''  PRC Envelope #4 110 x 208 mm    
        Public Const DMPAPER_PENV_5 = 100 ''  PRC Envelope #5 110 x 220 mm    
        Public Const DMPAPER_PENV_6 = 101 ''  PRC Envelope #6 120 x 230 mm    
        Public Const DMPAPER_PENV_7 = 102 ''  PRC Envelope #7 160 x 230 mm    
        Public Const DMPAPER_PENV_8 = 103 ''  PRC Envelope #8 120 x 309 mm    
        Public Const DMPAPER_PENV_9 = 104 ''  PRC Envelope #9 229 x 324 mm    
        Public Const DMPAPER_PENV_10 = 105 ''  PRC Envelope #10 324 x 458 mm   
        Public Const DMPAPER_P16K_ROTATED = 106 ''  PRC 16K Rotated                 
        Public Const DMPAPER_P32K_ROTATED = 107 ''  PRC 32K Rotated                 
        Public Const DMPAPER_P32KBIG_ROTATED = 108 ''  PRC 32K(Big) Rotated            
        Public Const DMPAPER_PENV_1_ROTATED = 109 ''  PRC Envelope #1 Rotated 165 x 102 mm 
        Public Const DMPAPER_PENV_2_ROTATED = 110 ''  PRC Envelope #2 Rotated 176 x 102 mm 
        Public Const DMPAPER_PENV_3_ROTATED = 111 ''  PRC Envelope #3 Rotated 176 x 125 mm 
        Public Const DMPAPER_PENV_4_ROTATED = 112 ''  PRC Envelope #4 Rotated 208 x 110 mm 
        Public Const DMPAPER_PENV_5_ROTATED = 113 ''  PRC Envelope #5 Rotated 220 x 110 mm 
        Public Const DMPAPER_PENV_6_ROTATED = 114 ''  PRC Envelope #6 Rotated 230 x 120 mm 
        Public Const DMPAPER_PENV_7_ROTATED = 115 ''  PRC Envelope #7 Rotated 230 x 160 mm 
        Public Const DMPAPER_PENV_8_ROTATED = 116 ''  PRC Envelope #8 Rotated 309 x 120 mm 
        Public Const DMPAPER_PENV_9_ROTATED = 117 ''  PRC Envelope #9 Rotated 324 x 229 mm 
        Public Const DMPAPER_PENV_10_ROTATED = 118 ''  PRC Envelope #10 Rotated 458 x 324 mm 
        Public Const DMPAPER_LAST = DMPAPER_PENV_10_ROTATED

        Public Const DMPAPER_USER = 256 ''  bin selections 
        Public Const DMBIN_FIRST = DMBIN_UPPER
        Public Const DMBIN_UPPER = 1
        Public Const DMBIN_ONLYONE = 1
        Public Const DMBIN_LOWER = 2
        Public Const DMBIN_MIDDLE = 3
        Public Const DMBIN_MANUAL = 4
        Public Const DMBIN_ENVELOPE = 5
        Public Const DMBIN_ENVMANUAL = 6
        Public Const DMBIN_AUTO = 7
        Public Const DMBIN_TRACTOR = 8
        Public Const DMBIN_SMALLFMT = 9
        Public Const DMBIN_LARGEFMT = 10
        Public Const DMBIN_LARGECAPACITY = 11
        Public Const DMBIN_CASSETTE = 14
        Public Const DMBIN_FORMSOURCE = 15
        Public Const DMBIN_LAST = DMBIN_FORMSOURCE

        Public Const DMBIN_USER = 256 ''  device specific bins start here 

        ''  print qualities 
        '' define DMRES_DRAFT         (-1)
        '' define DMRES_LOW           (-2)
        '' define DMRES_MEDIUM        (-3)
        '' define DMRES_HIGH          (-4)

        ''  color enable/disable for color printers 
        Public Const DMCOLOR_MONOCHROME = 1
        Public Const DMCOLOR_COLOR = 2 ''  duplex enable 
        Public Const DMDUP_SIMPLEX = 1
        Public Const DMDUP_VERTICAL = 2
        Public Const DMDUP_HORIZONTAL = 3 ''  TrueType options 
        Public Const DMTT_BITMAP = 1 ''  print TT fonts as graphics 
        Public Const DMTT_DOWNLOAD = 2 ''  download TT fonts as soft fonts 
        Public Const DMTT_SUBDEV = 3 ''  substitute device fonts for TT fonts 
        '' if(WINVER >= 0x0400)
        Public Const DMTT_DOWNLOAD_OUTLINE = 4 ''  download TT fonts as outline soft fonts 
        '' endif ''  WINVER >= 0x0400 

        ''  Collation selections 
        Public Const DMCOLLATE_FALSE = 0
        Public Const DMCOLLATE_TRUE = 1

        '' if(WINVER >= 0x0501)
        ''  DEVMODE dmDisplayOrientation specifiations 
        Public Const DMDO_DEFAULT = 0
        Public Const DMDO_90 = 1
        Public Const DMDO_180 = 2
        Public Const DMDO_270 = 3 ''  DEVMODE dmDisplayFixedOutput specifiations 
        Public Const DMDFO_DEFAULT = 0
        Public Const DMDFO_STRETCH = 1
        Public Const DMDFO_CENTER = 2
        '' endif ''  WINVER >= 0x0501 

        ''  DEVMODE dmDisplayFlags flags 

        '' Public Const DM_GRAYSCALE = 0x00000001 ''  This flag is no longer valid 
        Public Const DM_INTERLACED = &H2
        Public Const DMDISPLAYFLAGS_TEXTMODE = &H4 ''  dmNup , multiple logical page per physical page options 
        Public Const DMNUP_SYSTEM = 1
        Public Const DMNUP_ONEUP = 2

        '' if(WINVER >= 0x0400)
        ''  ICM methods 
        Public Const DMICMMETHOD_NONE = 1 ''  ICM disabled 
        Public Const DMICMMETHOD_SYSTEM = 2 ''  ICM handled by system 
        Public Const DMICMMETHOD_DRIVER = 3 ''  ICM handled by driver 
        Public Const DMICMMETHOD_DEVICE = 4 ''  ICM handled by device 

        Public Const DMICMMETHOD_USER = 256 ''  Device-specific methods start here 

        ''  ICM Intents 
        Public Const DMICM_SATURATE = 1 ''  Maximize color saturation 
        Public Const DMICM_CONTRAST = 2 ''  Maximize color contrast 
        Public Const DMICM_COLORIMETRIC = 3 ''  Use specific color metric 
        Public Const DMICM_ABS_COLORIMETRIC = 4 ''  Use specific color metric 

        Public Const DMICM_USER = 256 ''  Device-specific intents start here 

        ''  Media types 

        Public Const DMMEDIA_STANDARD = 1 ''  Standard paper 
        Public Const DMMEDIA_TRANSPARENCY = 2 ''  Transparency 
        Public Const DMMEDIA_GLOSSY = 3 ''  Glossy paper 

        Public Const DMMEDIA_USER = 256 ''  Device-specific media start here 

        ''  Dither types 
        Public Const DMDITHER_NONE = 1 ''  No dithering 
        Public Const DMDITHER_COARSE = 2 ''  Dither with a coarse brush 
        Public Const DMDITHER_FINE = 3 ''  Dither with a fine brush 
        Public Const DMDITHER_LINEART = 4 ''  LineArt dithering 
        Public Const DMDITHER_ERRORDIFFUSION = 5 ''  LineArt dithering 
        Public Const DMDITHER_RESERVED6 = 6 ''  LineArt dithering 
        Public Const DMDITHER_RESERVED7 = 7 ''  LineArt dithering 
        Public Const DMDITHER_RESERVED8 = 8 ''  LineArt dithering 
        Public Const DMDITHER_RESERVED9 = 9 ''  LineArt dithering 
        Public Const DMDITHER_GRAYSCALE = 10 ''  Device does grayscaling 

        Public Const DMDITHER_USER = 256 ''  Device-specific dithers start here 
        '' endif ''  WINVER >= 0x0400 



        '' DevCaps

        Public Const DRIVERVERSION = 0 ''  Device driver version
        Public Const TECHNOLOGY = 2 ''  Device classification
        Public Const HORZSIZE = 4 ''  Horizontal size in millimeters
        Public Const VERTSIZE = 6 ''  Vertical size in millimeters
        Public Const HORZRES = 8 ''  Horizontal width in pixels
        Public Const VERTRES = 10 ''  Vertical height in pixels
        Public Const BITSPIXEL = 12 ''  Number of bits per pixel
        Public Const PLANES = 14 ''  Number of planes
        Public Const NUMBRUSHES = 16 ''  Number of brushes the device has
        Public Const NUMPENS = 18 ''  Number of pens the device has
        Public Const NUMMARKERS = 20 ''  Number of markers the device has
        Public Const NUMFONTS = 22 ''  Number of fonts the device has
        Public Const NUMCOLORS = 24 ''  Number of colors the device supports
        Public Const PDEVICESIZE = 26 ''  Size required for device descriptor
        Public Const CURVECAPS = 28 ''  Curve capabilities
        Public Const LINECAPS = 30 ''  Line capabilities
        Public Const POLYGONALCAPS = 32 ''  Polygonal capabilities
        Public Const TEXTCAPS = 34 ''  Text capabilities
        Public Const CLIPCAPS = 36 ''  Clipping capabilities
        Public Const RASTERCAPS = 38 ''  Bitblt capabilities
        Public Const ASPECTX = 40 ''  Length of the X leg
        Public Const ASPECTY = 42 ''  Length of the Y leg
        Public Const ASPECTXY = 44 ''  Length of the hypotenuse

        Public Const LOGPIXELSX = 88 ''  Logical pixels/inch in X
        Public Const LOGPIXELSY = 90 ''  Logical pixels/inch in Y

        Public Const SIZEPALETTE = 104 ''  Number of entries in physical palette
        Public Const NUMRESERVED = 106 ''  Number of reserved entries in palette
        Public Const COLORRES = 108 ''  Actual color resolution

        '' Printing related DeviceCaps. These replace the appropriate Escapes

        Public Const PHYSICALWIDTH = 110 ''  Physical Width in device units
        Public Const PHYSICALHEIGHT = 111 ''  Physical Height in device units
        Public Const PHYSICALOFFSETX = 112 ''  Physical Printable Area x margin
        Public Const PHYSICALOFFSETY = 113 ''  Physical Printable Area y margin
        Public Const SCALINGFACTORX = 114 ''  Scaling factor x
        Public Const SCALINGFACTORY = 115 ''  Scaling factor y

        '' Display driver specific

        Public Const VREFRESH = 116 ''  Current vertical refresh rate of the
        ''  display device (for displays only) in Hz
        Public Const DESKTOPVERTRES = 117 ''  Horizontal width of entire desktop in
        ''  pixels
        Public Const DESKTOPHORZRES = 118 ''  Vertical height of entire desktop in
        ''  pixels
        Public Const BLTALIGNMENT = 119 ''  Preferred blt alignment

        ''if(WINVER >= 0x0500)
        Public Const SHADEBLENDCAPS = 120 ''  Shading and blending caps
        Public Const COLORMGMTCAPS = 121 ''  Color Management caps
        ''endif ''  WINVER >= 0x0500

        ''ifndef NOGDICAPMASKS

        ''  Device Capability Masks:

        ''  Device Technologies
        Public Const DT_PLOTTER = 0 ''  Vector plotter
        Public Const DT_RASDISPLAY = 1 ''  Raster display
        Public Const DT_RASPRINTER = 2 ''  Raster printer
        Public Const DT_RASCAMERA = 3 ''  Raster camera
        Public Const DT_CHARSTREAM = 4 ''  Character-stream, PLP
        Public Const DT_METAFILE = 5 ''  Metafile, VDM
        Public Const DT_DISPFILE = 6 ''  Display-file

        '' Device Capabilities

        Public Const DC_FIELDS As UShort = 1
        Public Const DC_PAPERS As UShort = 2
        Public Const DC_PAPERSIZE As UShort = 3
        Public Const DC_MINEXTENT As UShort = 4
        Public Const DC_MAXEXTENT As UShort = 5
        Public Const DC_BINS As UShort = 6
        Public Const DC_DUPLEX As UShort = 7
        Public Const DC_SIZE As UShort = 8
        Public Const DC_EXTRA As UShort = 9
        Public Const DC_VERSION As UShort = 10
        Public Const DC_DRIVER As UShort = 11
        Public Const DC_BINNAMES As UShort = 12
        Public Const DC_ENUMRESOLUTIONS As UShort = 13
        Public Const DC_FILEDEPENDENCIES As UShort = 14
        Public Const DC_TRUETYPE As UShort = 15
        Public Const DC_PAPERNAMES As UShort = 16
        Public Const DC_ORIENTATION As UShort = 17
        Public Const DC_COPIES As UShort = 18
        Public Const DC_BINADJUST As UShort = 19
        Public Const DC_EMF_COMPLIANT As UShort = 20
        Public Const DC_DATATYPE_PRODUCED As UShort = 21
        Public Const DC_COLLATE As UShort = 22
        Public Const DC_MANUFACTURER As UShort = 23
        Public Const DC_MODEL As UShort = 24
        Public Const DC_PERSONALITY As UShort = 25
        Public Const DC_PRINTRATE As UShort = 26
        Public Const DC_PRINTRATEUNIT As UShort = 27
        Public Const PRINTRATEUNIT_PPM As UShort = 1
        Public Const PRINTRATEUNIT_CPS As UShort = 2
        Public Const PRINTRATEUNIT_LPM As UShort = 3
        Public Const PRINTRATEUNIT_IPM As UShort = 4
        Public Const DC_PRINTERMEM As UShort = 28
        Public Const DC_MEDIAREADY As UShort = 29
        Public Const DC_STAPLE As UShort = 30
        Public Const DC_PRINTRATEPPM As UShort = 31
        Public Const DC_COLORDEVICE As UShort = 32
        Public Const DC_NUP As UShort = 33
        Public Const DC_MEDIATYPENAMES As UShort = 34
        Public Const DC_MEDIATYPES As UShort = 35

        '' Printer

        Public Const PRINTER_CONTROL_PAUSE = 1
        Public Const PRINTER_CONTROL_RESUME = 2
        Public Const PRINTER_CONTROL_PURGE = 3
        Public Const PRINTER_CONTROL_SET_STATUS = 4

        Public Const PRINTER_STATUS_PAUSED = &H1
        Public Const PRINTER_STATUS_ERROR = &H2
        Public Const PRINTER_STATUS_PENDING_DELETION = &H4
        Public Const PRINTER_STATUS_PAPER_JAM = &H8
        Public Const PRINTER_STATUS_PAPER_OUT = &H10
        Public Const PRINTER_STATUS_MANUAL_FEED = &H20
        Public Const PRINTER_STATUS_PAPER_PROBLEM = &H40
        Public Const PRINTER_STATUS_OFFLINE = &H80
        Public Const PRINTER_STATUS_IO_ACTIVE = &H100
        Public Const PRINTER_STATUS_BUSY = &H200
        Public Const PRINTER_STATUS_PRINTING = &H400
        Public Const PRINTER_STATUS_OUTPUT_BIN_FULL = &H800
        Public Const PRINTER_STATUS_NOT_AVAILABLE = &H1000
        Public Const PRINTER_STATUS_WAITING = &H2000
        Public Const PRINTER_STATUS_PROCESSING = &H4000
        Public Const PRINTER_STATUS_INITIALIZING = &H8000
        Public Const PRINTER_STATUS_WARMING_UP = &H10000
        Public Const PRINTER_STATUS_TONER_LOW = &H20000
        Public Const PRINTER_STATUS_NO_TONER = &H40000
        Public Const PRINTER_STATUS_PAGE_PUNT = &H80000
        Public Const PRINTER_STATUS_USER_INTERVENTION = &H100000
        Public Const PRINTER_STATUS_OUT_OF_MEMORY = &H200000
        Public Const PRINTER_STATUS_DOOR_OPEN = &H400000
        Public Const PRINTER_STATUS_SERVER_UNKNOWN = &H800000
        Public Const PRINTER_STATUS_POWER_SAVE = &H1000000
        Public Const PRINTER_STATUS_SERVER_OFFLINE = &H2000000
        Public Const PRINTER_STATUS_DRIVER_UPDATE_NEEDED = &H4000000

        Public Const PRINTER_ATTRIBUTE_QUEUED = &H1
        Public Const PRINTER_ATTRIBUTE_DIRECT = &H2
        Public Const PRINTER_ATTRIBUTE_DEFAULT = &H4
        Public Const PRINTER_ATTRIBUTE_SHARED = &H8
        Public Const PRINTER_ATTRIBUTE_NETWORK = &H10
        Public Const PRINTER_ATTRIBUTE_HIDDEN = &H20
        Public Const PRINTER_ATTRIBUTE_LOCAL = &H40

        Public Const PRINTER_ATTRIBUTE_ENABLE_DEVQ = &H80
        Public Const PRINTER_ATTRIBUTE_KEEPPRINTEDJOBS = &H100
        Public Const PRINTER_ATTRIBUTE_DO_COMPLETE_FIRST = &H200

        Public Const PRINTER_ATTRIBUTE_WORK_OFFLINE = &H400
        Public Const PRINTER_ATTRIBUTE_ENABLE_BIDI = &H800
        Public Const PRINTER_ATTRIBUTE_RAW_ONLY = &H1000
        Public Const PRINTER_ATTRIBUTE_PUBLISHED = &H2000

        Public Const PRINTER_ENUM_DEFAULT = &H1
        Public Const PRINTER_ENUM_LOCAL = &H2
        Public Const PRINTER_ENUM_CONNECTIONS = &H4
        Public Const PRINTER_ENUM_FAVORITE = &H4
        Public Const PRINTER_ENUM_NAME = &H8
        Public Const PRINTER_ENUM_REMOTE = &H10
        Public Const PRINTER_ENUM_SHARED = &H20
        Public Const PRINTER_ENUM_NETWORK = &H40

        Public Const PRINTER_ENUM_EXPAND = &H4000
        Public Const PRINTER_ENUM_CONTAINER = &H8000

        Public Const PRINTER_ENUM_ICONMASK = &HFF0000
        Public Const PRINTER_ENUM_ICON1 = &H10000
        Public Const PRINTER_ENUM_ICON2 = &H20000
        Public Const PRINTER_ENUM_ICON3 = &H40000
        Public Const PRINTER_ENUM_ICON4 = &H80000
        Public Const PRINTER_ENUM_ICON5 = &H100000
        Public Const PRINTER_ENUM_ICON6 = &H200000
        Public Const PRINTER_ENUM_ICON7 = &H400000
        Public Const PRINTER_ENUM_ICON8 = &H800000
        Public Const PRINTER_ENUM_HIDE = &H1000000


#End Region

#Region "Declares"

        <DllImport("winspool.drv", CharSet:=CharSet.Unicode, SetLastError:=True, EntryPoint:="OpenPrinterW")>
        Function OpenPrinter(<MarshalAs(UnmanagedType.LPWStr)> pPrinterName As String, ByRef hPrinter As IntPtr, pDefault As IntPtr) As Boolean
        End Function

        Declare Function ClosePrinter Lib "winspool.drv" (hPrinter As IntPtr) As Boolean

        Declare Function GetPrinter Lib "winspool.drv" Alias "GetPrinterW" _
            (hPrinter As IntPtr, level As UInteger, pPrinter As IntPtr, cbBuf As UInteger, ByRef pcbNeeded As UInteger) As Boolean

        Declare Function GetJob Lib "winspool.drv" Alias "GetJobW" _
            (hPrinter As IntPtr, JobId As UInteger, Lovel As UInteger, pJob As IntPtr, cbBuf As UInteger, ByRef pcbNeeded As UInteger) As Boolean

        <DllImport("winspool.drv", CharSet:=CharSet.Unicode, EntryPoint:="DeviceCapabilitiesW")>
        Function DeviceCapabilities _
            (<MarshalAs(UnmanagedType.LPWStr)>
             pDevice As String,
             <MarshalAs(UnmanagedType.LPWStr)>
             pPort As String,
             fwCapability As UShort,
             pOutput As IntPtr,
             pDevMode As IntPtr) As UInteger
        End Function

        <DllImport("winspool.drv", CharSet:=CharSet.Unicode, EntryPoint:="EnumPrintersW")>
        Function EnumPrinters(Flags As UInteger,
                              <MarshalAs(UnmanagedType.LPWStr)> Name As String,
                              Level As UInteger,
                              pPrinterEnum As IntPtr,
                              cbBuf As UInteger,
                              ByRef pcbNeeded As UInteger,
                              ByRef pcbReturned As UInteger) As Boolean
        End Function

        <DllImport("gdi32.dll")>
        Function GetDeviceCaps(hdc As IntPtr, nIndex As Integer) As Integer
        End Function


        '        DWORD EnumPrinterKey(
        '  _In_   HANDLE hPrinter,
        '  _In_   LPCTSTR pKeyName,
        '  _Out_  LPTSTR pSubkey,
        '  _In_   DWORD cbSubkey,
        '  _Out_  LPDWORD pcbSubkey
        ');


        <DllImport("winspool.drv", CharSet:=CharSet.Unicode, EntryPoint:="EnumPrinterKeyW")>
        Function EnumPrinterKey(hPrinter As IntPtr,
                                <MarshalAs(UnmanagedType.LPWStr)> pKeyName As String,
                                pSubkey As IntPtr,
                                cbSubKey As UInteger,
                                ByRef pcbSubkey As UInteger) As UInteger
        End Function



#End Region

#Region "Convoluted Printer Capabilities Deducing Functions"

        Public Function GetMaxDPIForPrinter(printer As PrinterObject, hPrinter As IntPtr) As System.Windows.Size

            Dim cb As UInteger = 0
            Dim r As UInteger = 0

            Dim mm As New MemPtr
            Dim l As New List(Of PrinterObject)
            Dim pn As PrinterObject
            Dim cpx As IntPtr = IntPtr.Zero
            Dim minfo As Integer
            EnumPrinters(6, "", 2, IntPtr.Zero, 0, cb, r)

            If cb <> 0 Then
                mm.Alloc(cb)
                EnumPrinters(6, "", 2, mm, cb, cb, r)
                minfo = 136
                cpx = mm

                For i As Integer = 1 To CInt(r)
                    pn = New PrinterObject(cpx, i = 1)
                    cpx += 136

                    l.Add(pn)
                Next

            End If

        End Function

#End Region

    End Module

#Region "Enums"


    ''' <summary>
    ''' Printer attributes flags
    ''' </summary>
    <Flags>
    Public Enum PrinterAttributes As UInteger

        ''' <summary>
        ''' Queued printer
        ''' </summary>
        Queued = &H1

        ''' <summary>
        ''' Direct printer
        ''' </summary>
        Direct = &H2

        ''' <summary>
        ''' Is default printer
        ''' </summary>
        [Default] = &H4

        ''' <summary>
        ''' Is shared printer
        ''' </summary>
        [Shared] = &H8

        ''' <summary>
        ''' Is network printer
        ''' </summary>
        Network = &H10

        ''' <summary>
        ''' Is hidden
        ''' </summary>
        Hidden = &H20

        ''' <summary>
        ''' Is a local printer
        ''' </summary>
        Local = &H40

        ''' <summary>
        ''' Enable DevQ
        ''' </summary>
        EnableDevQ = &H80

        ''' <summary>
        ''' Keep printed jobs
        ''' </summary>
        KeepPrintedJobs = &H100

        ''' <summary>
        ''' Do complete first
        ''' </summary>
        DoCompleteFirst = &H200

        ''' <summary>
        ''' Work offline
        ''' </summary>
        WorkOffline = &H400

        ''' <summary>
        ''' Enable BIDI
        ''' </summary>
        EnableBIDI = &H800

        ''' <summary>
        ''' Raw mode only
        ''' </summary>
        RawOnly = &H1000

        ''' <summary>
        ''' Published
        ''' </summary>
        Published = &H2000

        Reserved1 = &H4000
        Reserved2 = &H8000
        Reserved3 = &H10000

    End Enum


    ''' <summary>
    ''' Printer status flags
    ''' </summary>
    <Flags>
    Public Enum PrinterStatus As UInteger
        ''' <summary>
        '''  The printer is busy.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is busy.")>
        Busy = PRINTER_STATUS_BUSY

        ''' <summary>
        '''  The printer door is open.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer door is open.")>
        DoorOpen = PRINTER_STATUS_DOOR_OPEN

        ''' <summary>
        '''  The printer is in an error state.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is in an error state.")>
        [Error] = PRINTER_STATUS_ERROR

        ''' <summary>
        '''  The printer is initializing.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is initializing.")>
        Initializing = PRINTER_STATUS_INITIALIZING

        ''' <summary>
        '''  The printer is in an active input/output state
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is in an active input/output stat.")>
        IoActive = PRINTER_STATUS_IO_ACTIVE

        ''' <summary>
        '''  The printer is in a manual feed state.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is in a manual feed state.")>
        ManualFeed = PRINTER_STATUS_MANUAL_FEED

        ''' <summary>
        '''  The printer is out of toner.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is out of toner.")>
        NoToner = PRINTER_STATUS_NO_TONER

        ''' <summary>
        '''  The printer is not available for printing.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is not available for printing.")>
        NotAvailable = PRINTER_STATUS_NOT_AVAILABLE

        ''' <summary>
        '''  The printer is offline.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is offline.")>
        Offline = PRINTER_STATUS_OFFLINE

        ''' <summary>
        '''  The printer has run out of memory.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer has run out of memory.")>
        OutOfMemory = PRINTER_STATUS_OUT_OF_MEMORY

        ''' <summary>
        '''  The printer's output bin is full.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer's output bin is full.")>
        OutputBinFull = PRINTER_STATUS_OUTPUT_BIN_FULL

        ''' <summary>
        '''  The printer cannot print the current page.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer cannot print the current page.")>
        PagePunt = PRINTER_STATUS_PAGE_PUNT

        ''' <summary>
        '''  Paper is jammed in the printer
        ''' </summary>
        ''' <remarks></remarks>
        <Description("Paper is jammed in the printe.")>
        PaperJam = PRINTER_STATUS_PAPER_JAM

        ''' <summary>
        '''  The printer is out of paper.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is out of paper.")>
        PaperOut = PRINTER_STATUS_PAPER_OUT

        ''' <summary>
        '''  The printer has a paper problem.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer has a paper problem.")>
        PaperProblem = PRINTER_STATUS_PAPER_PROBLEM

        ''' <summary>
        '''  The printer is paused.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is paused.")>
        Paused = PRINTER_STATUS_PAUSED

        ''' <summary>
        '''  The printer is being deleted.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is being deleted.")>
        PendingDeletion = PRINTER_STATUS_PENDING_DELETION

        ''' <summary>
        '''  The printer is in power save mode.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is in power save mode.")>
        PowerSave = PRINTER_STATUS_POWER_SAVE

        ''' <summary>
        '''  The printer is printing.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is printing.")>
        Printing = PRINTER_STATUS_PRINTING

        ''' <summary>
        '''  The printer is processing a print job.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is processing a print job.")>
        Processing = PRINTER_STATUS_PROCESSING

        ''' <summary>
        '''  The printer status is unknown.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer status is unknown.")>
        ServerUnknown = PRINTER_STATUS_SERVER_UNKNOWN

        ''' <summary>
        '''  The printer is low on toner.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is low on toner.")>
        TonerLow = PRINTER_STATUS_TONER_LOW

        ''' <summary>
        '''  The printer has an error that requires the user to do something.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer has an error that requires the user to do something.")>
        UserIntervention = PRINTER_STATUS_USER_INTERVENTION

        ''' <summary>
        '''  The printer is waiting.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is waiting.")>
        Waiting = PRINTER_STATUS_WAITING

        ''' <summary>
        '''  The printer is warming up.
        ''' </summary>
        ''' <remarks></remarks>
        <Description("The printer is warming up.")>
        WarmingUp = PRINTER_STATUS_WARMING_UP

    End Enum

    ''' <summary>
    ''' Device mode fields flags
    ''' </summary>
    <Flags>
    Public Enum DeviceModeFields As UInteger

        ''' <summary>
        ''' Orientation
        ''' </summary>
        Orientation = &H1

        ''' <summary>
        ''' Paper size
        ''' </summary>
        PaperSize = &H2

        ''' <summary>
        ''' Paper length
        ''' </summary>
        PaperLength = &H4

        ''' <summary>
        ''' Paper width
        ''' </summary>
        PaperWidth = &H8

        ''' <summary>
        ''' Scale
        ''' </summary>
        Scale = &H10

        ''' <summary>
        ''' Position
        ''' </summary>
        Position = &H20

        ''' <summary>
        ''' Nup
        ''' </summary>
        Nup = &H40

        ''' <summary>
        ''' Display orientation
        ''' </summary>
        DisplayOrientation = &H80

        ''' <summary>
        ''' Copies
        ''' </summary>
        Copies = &H100

        ''' <summary>
        ''' Default source
        ''' </summary>
        DefaultSource = &H200

        ''' <summary>
        ''' Print quality
        ''' </summary>
        PrintQuality = &H400

        ''' <summary>
        ''' Color printer
        ''' </summary>
        Color = &H800

        ''' <summary>
        ''' Duplex support
        ''' </summary>
        Duplex = &H1000

        ''' <summary>
        ''' YR resolution
        ''' </summary>
        YResolution = &H2000

        ''' <summary>
        ''' TTOption
        ''' </summary>
        TTOption = &H4000

        ''' <summary>
        ''' Collate
        ''' </summary>
        Collate = &H8000

        ''' <summary>
        ''' Form name
        ''' </summary>
        FormName = &H10000

        ''' <summary>
        ''' Log pixels
        ''' </summary>
        LogPixels = &H20000

        ''' <summary>
        ''' Bits per pixel
        ''' </summary>
        BitsPerPel = &H40000

        ''' <summary>
        ''' Width in pixels
        ''' </summary>
        PelsWidth = &H80000

        ''' <summary>
        ''' Height in pixels
        ''' </summary>
        PelsHeight = &H100000

        ''' <summary>
        ''' Display flags
        ''' </summary>
        DisplayFlags = &H200000

        ''' <summary>
        ''' Display frequency
        ''' </summary>
        DisplayFrequency = &H400000

        ''' <summary>
        ''' ICM Method
        ''' </summary>
        ICMMethod = &H800000UI

        ''' <summary>
        ''' ICM Intent
        ''' </summary>
        ICMIntent = &H1000000UI

        ''' <summary>
        ''' Media type
        ''' </summary>
        MediaType = &H2000000UI

        ''' <summary>
        ''' Dither type
        ''' </summary>
        DitherType = &H4000000UI

        ''' <summary>
        ''' Panning width
        ''' </summary>
        PanningWidth = &H8000000UI

        ''' <summary>
        ''' Panning height
        ''' </summary>
        PanningHeight = &H10000000UI

    End Enum

#End Region

#Region "System Paper Types"

    ''' <summary>
    ''' Paper nationalities
    ''' </summary>
    Public Enum PaperNationalities

        ''' <summary>
        ''' American
        ''' </summary>
        American

        ''' <summary>
        ''' ISO / International Standard
        ''' </summary>
        Iso

        ''' <summary>
        ''' Japanese
        ''' </summary>
        Japanese

        ''' <summary>
        ''' German
        ''' </summary>
        German

        ''' <summary>
        ''' Chinese
        ''' </summary>
        Chinese
    End Enum

    ''' <summary>
    ''' Represents a collection of all known system paper types
    ''' </summary>
    Public Class SystemPaperTypes

        ''' <summary>
        ''' This class is not creatable.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()

        End Sub

#Region "Paper Size Data"

        Private Shared ReadOnly _SizeDataString As String =
            "LETTER	1	US Letter 8 1/2 x 11 in" & vbCrLf &
            "LETTER_SMALL	2	US Letter Small 8 1/2 x 11 in" & vbCrLf &
            "TABLOID	3	US Tabloid 11 x 17 in" & vbCrLf &
            "LEDGER	4	US Ledger 17 x 11 in" & vbCrLf &
            "LEGAL	5	US Legal 8 1/2 x 14 in" & vbCrLf &
            "STATEMENT	6	US Statement 5 1/2 x 8 1/2 in" & vbCrLf &
            "EXECUTIVE	7	US Executive 7 1/4 x 10 1/2 in" & vbCrLf &
            "A3	8	A3 297 x 420 mm" & vbCrLf &
            "A4	9	A4 210 x 297 mm" & vbCrLf &
            "A4_SMALL	10	A4 Small 210 x 297 mm" & vbCrLf &
            "A5	11	A5 148 x 210 mm" & vbCrLf &
            "B4	12	B4 (JIS) 257 x 364 mm" & vbCrLf &
            "B5	13	B5 (JIS) 182 x 257 mm" & vbCrLf &
            "FOLIO	14	Folio 8 1/2 x 13 in" & vbCrLf &
            "QUARTO	15	Quarto 215 x 275 mm" & vbCrLf &
            "10X14	16	10 x 14 in" & vbCrLf &
            "11X17	17	11 x 17 in" & vbCrLf &
            "NOTE	18	US Note 8 1/2 x 11 in" & vbCrLf &
            "ENV_9	19	US Envelope #9 - 3 7/8 x 8 7/8" & vbCrLf &
            "ENV_10	20	US Envelope #10 - 4 1/8 x 9 1/2" & vbCrLf &
            "ENV_11	21	US Envelope #11 - 4 1/2 x 10 3/8" & vbCrLf &
            "ENV_12	22	US Envelope #12 - 4 3/4 x 11 in" & vbCrLf &
            "ENV_14	23	US Envelope #14 - 5 x 11 1/2" & vbCrLf &
            "ENV_DL	27	Envelope DL 110 x 220 mm" & vbCrLf &
            "ENV_C5	28	Envelope C5 - 162 x 229 mm" & vbCrLf &
            "ENV_C3	29	Envelope C3 - 324 x 458 mm" & vbCrLf &
            "ENV_C4	30	Envelope C4 - 229 x 324 mm" & vbCrLf &
            "ENV_C6	31	Envelope C6 - 114 x 162 mm" & vbCrLf &
            "ENV_C65	32	Envelope C65 - 114 x 229 mm" & vbCrLf &
            "ENV_B4	33	Envelope B4 - 250 x 353 mm" & vbCrLf &
            "ENV_B5	34	Envelope B5 - 176 x 250 mm" & vbCrLf &
            "ENV_B6	35	Envelope B6 - 176 x 125 mm" & vbCrLf &
            "ENV_ITALY	36	Envelope 110 x 230 mm" & vbCrLf &
            "ENV_MONARCH	37	US Envelope Monarch 3.875 x 7.5 in" & vbCrLf &
            "ENV_PERSONAL	38	6 3/4 US Envelope 3 5/8 x 6 1/2 in" & vbCrLf &
            "FANFOLD_US	39	US Std Fanfold 14 7/8 x 11 in" & vbCrLf &
            "FANFOLD_STD_GERMAN	40	German Std Fanfold 8 1/2 x 12 in" & vbCrLf &
            "FANFOLD_LGL_GERMAN	41	German Legal Fanfold 8 1/2 x 13 in" & vbCrLf &
            "ISO_B4	42	B4 (ISO) 250 x 353 mm" & vbCrLf &
            "JAPANESE_POSTCARD	43	Japanese Postcard 100 x 148 mm" & vbCrLf &
            "9X11	44	9 x 11 in" & vbCrLf &
            "10X11	45	10 x 11 in" & vbCrLf &
            "15X11	46	15 x 11 in" & vbCrLf &
            "ENV_INVITE	47	Envelope Invite 220 x 220 mm" & vbCrLf &
            "LETTER_EXTRA	50	US Letter Extra 9 1/2 x 12 in" & vbCrLf &
            "LEGAL_EXTRA	51	US Legal Extra 9 1/2 x 15 in" & vbCrLf &
            "TABLOID_EXTRA	52	US Tabloid Extra 11.69 x 18 in" & vbCrLf &
            "A4_EXTRA	53	A4 Extra 9.27 x 12.69 in" & vbCrLf &
            "LETTER_TRANSVERSE	54	Letter Transverse 8 1/2 x 11 in" & vbCrLf &
            "A4_TRANSVERSE	55	A4 Transverse 210 x 297 mm" & vbCrLf &
            "LETTER_EXTRA_TRANSVERSE	56	Letter Extra Transverse 9 1/2 x 12 in" & vbCrLf &
            "A_PLUS	57	SuperA/SuperA/A4 227 x 356 mm" & vbCrLf &
            "B_PLUS	58	SuperB/SuperB/A3 305 x 487 mm" & vbCrLf &
            "LETTER_PLUS	59	US Letter Plus 8.5 x 12.69 in" & vbCrLf &
            "A4_PLUS	60	A4 Plus 210 x 330 mm" & vbCrLf &
            "A5_TRANSVERSE	61	A5 Transverse 148 x 210 mm" & vbCrLf &
            "B5_TRANSVERSE	62	B5 (JIS) Transverse 182 x 257 mm" & vbCrLf &
            "A3_EXTRA	63	A3 Extra 322 x 445 mm" & vbCrLf &
            "A5_EXTRA	64	A5 Extra 174 x 235 mm" & vbCrLf &
            "B5_EXTRA	65	B5 (ISO) Extra 201 x 276 mm" & vbCrLf &
            "A2	66	A2 420 x 594 mm" & vbCrLf &
            "A3_TRANSVERSE	67	A3 Transverse 297 x 420 mm" & vbCrLf &
            "A3_EXTRA_TRANSVERSE	68	A3 Extra Transverse 322 x 445 mm" & vbCrLf &
            "DBL_JAPANESE_POSTCARD	69	Japanese Double Postcard 200 x 148 mm" & vbCrLf &
            "A6	70	A6 105 x 148 mm" & vbCrLf &
            "LETTER_ROTATED	75	Letter Rotated 11 x 8 1/2 11 in" & vbCrLf &
            "A3_ROTATED	76	A3 Rotated 420 x 297 mm" & vbCrLf &
            "A4_ROTATED	77	A4 Rotated 297 x 210 mm" & vbCrLf &
            "A5_ROTATED	78	A5 Rotated 210 x 148 mm" & vbCrLf &
            "B4_JIS_ROTATED	79	B4 (JIS) Rotated 364 x 257 mm" & vbCrLf &
            "B5_JIS_ROTATED	80	B5 (JIS) Rotated 257 x 182 mm" & vbCrLf &
            "JAPANESE_POSTCARD_ROTATED	81	Japanese Postcard Rotated 148 x 100 mm" & vbCrLf &
            "DBL_JAPANESE_POSTCARD_ROTATED	82	Double Japanese Postcard Rotated 148 x 200 mm" & vbCrLf &
            "A6_ROTATED	83	A6 Rotated 148 x 105 mm" & vbCrLf &
            "B6_JIS	88	B6 (JIS) 128 x 182 mm" & vbCrLf &
            "B6_JIS_ROTATED	89	B6 (JIS) Rotated 182 x 128 mm" & vbCrLf &
            "12X11	90	12 x 11 in" & vbCrLf &
            "P16K	93	PRC 16K 146 x 215 mm" & vbCrLf &
            "P32K	94	PRC 32K 97 x 151 mm" & vbCrLf &
            "P32KBIG	95	PRC 32K(Big) 97 x 151 mm" & vbCrLf &
            "PENV_1	96	PRC Envelope #1 - 102 x 165 mm" & vbCrLf &
            "PENV_2	97	PRC Envelope #2 - 102 x 176 mm" & vbCrLf &
            "PENV_3	98	PRC Envelope #3 - 125 x 176 mm" & vbCrLf &
            "PENV_4	99	PRC Envelope #4 - 110 x 208 mm" & vbCrLf &
            "PENV_5	100	PRC Envelope #5 - 110 x 220 mm" & vbCrLf &
            "PENV_6	101	PRC Envelope #6 - 120 x 230 mm" & vbCrLf &
            "PENV_7	102	PRC Envelope #7 - 160 x 230 mm" & vbCrLf &
            "PENV_8	103	PRC Envelope #8 - 120 x 309 mm" & vbCrLf &
            "PENV_9	104	PRC Envelope #9 - 229 x 324 mm" & vbCrLf &
            "PENV_10	105	PRC Envelope #10 - 324 x 458 mm" & vbCrLf &
            "PENV_1_ROTATED	109	PRC Envelope #1 Rotated 165 x 102 mm" & vbCrLf &
            "PENV_2_ROTATED	110	PRC Envelope #2 Rotated 176 x 102 mm" & vbCrLf &
            "PENV_3_ROTATED	111	PRC Envelope #3 Rotated 176 x 125 mm" & vbCrLf &
            "PENV_4_ROTATED	112	PRC Envelope #4 Rotated 208 x 110 mm" & vbCrLf &
            "PENV_5_ROTATED	113	PRC Envelope #5 Rotated 220 x 110 mm" & vbCrLf &
            "PENV_6_ROTATED	114	PRC Envelope #6 Rotated 230 x 120 mm" & vbCrLf &
            "PENV_7_ROTATED	115	PRC Envelope #7 Rotated 230 x 160 mm" & vbCrLf &
            "PENV_8_ROTATED	116	PRC Envelope #8 Rotated 309 x 120 mm" & vbCrLf &
            "PENV_9_ROTATED	117	PRC Envelope #9 Rotated 324 x 229 mm" & vbCrLf &
            "PENV_10_ROTATED	118	PRC Envelope #10 Rotated 458 x 324 mm"

#End Region

        ''' <summary>
        ''' Returns the list of supported system paper types.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property PaperTypes As ObjectModel.ReadOnlyCollection(Of SystemPaperType)
            Get
                Return _PaperList
            End Get
        End Property

        Private Shared _PaperList As ObjectModel.ReadOnlyCollection(Of SystemPaperType)

        Private Shared Sub ParsePapers()

            Dim objOut As New List(Of SystemPaperType)

            Dim paperList() As String = TextTools.Split(_SizeDataString, vbCrLf)

            For Each paper In paperList
                Dim p As New SystemPaperType

                Dim data() As String = TextTools.Split(paper, vbTab)

                p.Name = TextTools.CamelCase(data(0))
                p.Code = CInt(data(1))
                p.Description = data(2)
                p.IsTransverse = data(2).IndexOf("Transverse") <> -1
                p.IsPostcard = data(2).IndexOf("Postcard") <> -1
                p.IsRotated = data(2).IndexOf("Rotated") <> -1
                p.IsEnvelope = data(2).IndexOf("Envelope") <> -1

                If data(2).IndexOf("German") <> -1 Then
                    p.Nationality = PaperNationalities.German
                ElseIf data(2).IndexOf("US ") <> -1 Then
                    p.Nationality = PaperNationalities.American
                ElseIf data(2).IndexOf("PRC") <> -1 Then
                    p.Nationality = PaperNationalities.Chinese
                ElseIf data(2).IndexOf("Japan") <> -1 Then
                    p.Nationality = PaperNationalities.Japanese
                ElseIf data(2).IndexOf("(JIS)") <> -1 Then
                    p.Nationality = PaperNationalities.Japanese
                End If

                Dim ismm As Boolean = False

                Dim size As System.Windows.Size = FindSize(data(2), ismm)

                If ismm Then
                    p.SizeMillimeters = size
                Else
                    p.SizeInches = size
                End If
                objOut.Add(p)
                p = Nothing
            Next

            _PaperList = New ObjectModel.ReadOnlyCollection(Of SystemPaperType)(objOut)
        End Sub

        '''' <summary>
        '''' Parses a size from any kind of text.
        '''' </summary>
        '''' <param name="text">The text to parse.</param>
        '''' <param name="isMM">Receives a value indicating metric system.</param>
        '''' <param name="scanforDblQuote">Scan for double quotes as inches.</param>
        '''' <param name="acceptComma">Accept a comma as a separator in addition to the 'x'.</param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        Private Shared Function FindSize(text As String, ByRef isMM As Boolean, Optional scanforDblQuote As Boolean = False, Optional acceptComma As Boolean = False) As System.Windows.Size

            Dim ch() As Char
            Dim sOut As New System.Windows.Size

            Dim pastX As Boolean = False
            Dim i As Integer,
                c As Integer
            Dim x As Integer = 0

            Dim t As String

            If scanforDblQuote Then
                text = text.Replace("""", "in").Trim
            End If

            t = text.Substring(text.Length - 2, 2).ToLower

            isMM = (t = "mm")

            If t = "in" OrElse t = "mm" Then
                text = text.Substring(0, text.Length - 2).Trim
            End If

            ch = text.ToCharArray
            i = ch.Count - 1

            '' not allowed space (for metric)
            Dim nas As Boolean = False

            For c = i To 0 Step -1
                If ch(c) = "x"c Then
                    pastX = True
                    x = i
                ElseIf acceptComma AndAlso ch(c) = ","c Then
                    pastX = True
                    x = i
                ElseIf pastX Then
                    If ch(c) = " "c Then
                        If isMM And nas Then Exit For
                        Continue For
                    End If
                    If ch(c) = "/"c Then
                        Continue For
                    End If

                    If ch(c) = "."c Then
                        Continue For
                    End If

                    If TextTools.IsNumber(ch(c)) = False Then
                        Exit For
                    Else
                        nas = True
                    End If
                End If
            Next

            text = text.Substring(c + 1).Trim
            text = text.Replace(",", "x")

            Dim sizes() As String = TextTools.Split(text, "x")

            Dim d As Double,
                e As Double,
                f As Double

            Dim sc As Integer = 0

            For Each num In sizes
                d = 0
                Dim nch() As String = TextTools.Split(num.Trim, " ")

                If nch.Count = 2 Then
                    Dim div() As String = TextTools.Split(nch(1), "/")

                    If div.Count = 2 Then
                        d = CDbl(div(0))
                        e = CDbl(div(1))

                        d /= e
                    End If

                End If

                f = CDbl(nch(0)) + d
                If sc = 0 Then sOut.Width = f Else sOut.Height = f
                sc = 1
            Next

            Return sOut

        End Function

        Friend Shared Function TypeListFromCodes(list As IEnumerable(Of Short)) As List(Of SystemPaperType)
            Dim o As New List(Of SystemPaperType)
            For Each p In _PaperList

                For Each i In list

                    If p.Code = i Then
                        o.Add(p)
                        Exit For
                    End If
                Next
            Next

            Return o
        End Function

        Shared Sub New()
            ParsePapers()
        End Sub

    End Class

    ''' <summary>
    ''' IPaperType interface
    ''' </summary>
    Public Interface IPaperType

        ''' <summary>
        ''' Paper type name
        ''' </summary>
        ''' <returns></returns>
        Property Name As String

        ''' <summary>
        ''' True if orientation is landscape
        ''' </summary>
        ''' <returns></returns>
        Property IsLandscape As Boolean

        ''' <summary>
        ''' If true, size is in metric units (millimeters).
        ''' If false, size is in inches.
        ''' </summary>
        ''' <returns></returns>
        Property IsMetric As Boolean

        ''' <summary>
        ''' Paper size
        ''' </summary>
        ''' <returns></returns>
        Property Size As UniSize

        ''' <summary>
        ''' Compare one paper type to another paper type for equality
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        Function Equals(other As IPaperType) As Boolean

    End Interface

    ''' <summary>
    ''' Encapsulates a system paper type
    ''' </summary>
    Public Class SystemPaperType
        Implements IEquatable(Of SystemPaperType), IPaperType

        Private _Size As System.Windows.Size

        Private _Description As String

        Private _IsTransverse As Boolean

        Private _IsRotated As Boolean

        Private _IsPostcard As Boolean

        Private _Nationality As PaperNationalities = PaperNationalities.Iso

        ''' <summary>
        ''' Equals
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Equals(other As IPaperType) As Boolean Implements IPaperType.Equals
            With other
                If .IsLandscape <> (IsTransverse Or IsEnvelope) Then Return False
                If .IsMetric <> IsMetric Then Return False

                If .IsMetric Then
                    If .Size.Equals(SizeMillimeters) Then Return True
                Else
                    If .Size.Equals(SizeInches) Then Return True

                End If
            End With

            Return False
        End Function

        ''' <summary>
        ''' Equals
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Equals(obj As Object) As Boolean
            Try
                If obj.GetType.IsPrimitive Then
                    Return _Code = CType(obj, Integer)
                ElseIf TypeOf obj Is SystemPaperType Then
                    Return Equals(CType(obj, SystemPaperType))
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Equals
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Equals(other As SystemPaperType) As Boolean Implements IEquatable(Of DataTools.Interop.Printers.SystemPaperType).Equals
            Return _Size.Equals(other._Size)
        End Function

        ''' <summary>
        ''' Returns the (apparent) national or international origin of the paper size.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nationality As PaperNationalities
            Get
                Return _Nationality
            End Get
            Friend Set(value As PaperNationalities)
                _Nationality = value
            End Set
        End Property

        ''' <summary>
        ''' True if this is a postcard layout.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsPostcard As Boolean
            Get
                Return _IsPostcard
            End Get
            Friend Set(value As Boolean)
                _IsPostcard = value
            End Set
        End Property

        ''' <summary>
        ''' True for transverse/landscape.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsTransverse As Boolean
            Get
                Return _IsTransverse
            End Get
            Friend Set(value As Boolean)
                _IsTransverse = value
            End Set
        End Property

        ''' <summary>
        ''' Returns true for rotated layout.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsRotated As Boolean
            Get
                Return _IsRotated
            End Get
            Friend Set(value As Boolean)
                _IsRotated = value
            End Set
        End Property

        Private _IsEnvelope As Boolean

        ''' <summary>
        ''' Returns true if this paper type is an envelope.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsEnvelope As Boolean
            Get
                Return _IsEnvelope
            End Get
            Friend Set(value As Boolean)
                _IsEnvelope = value
            End Set
        End Property

        ''' <summary>
        ''' Returns true if it is transverse or envelope paper.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsLandscape As Boolean Implements IPaperType.IsLandscape
            Get
                Return _IsTransverse Or _IsEnvelope
            End Get
            Friend Set(value As Boolean)
                _IsTransverse = value
            End Set
        End Property

        ''' <summary>
        ''' Returns a description of the paper.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Description As String
            Get
                Return _Description
            End Get
            Friend Set(value As String)
                _Description = value
            End Set
        End Property

        Private _Name As String

        ''' <summary>
        ''' The name of the paper type.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Name As String Implements IPaperType.Name
            Get
                Return _Name
            End Get
            Friend Set(value As String)
                _Name = value
            End Set
        End Property

        Private _Code As Integer

        ''' <summary>
        ''' The Windows paper type code.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Code As Integer
            Get
                Return _Code
            End Get
            Friend Set(value As Integer)
                _Code = value
            End Set
        End Property

        ''' <summary>
        ''' The size of the paper, in inches.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SizeInches As System.Windows.Size
            Get
                Return _Size
            End Get
            Friend Set(value As System.Windows.Size)
                _Size = value
            End Set
        End Property

        ''' <summary>
        ''' The size of the paper, in millimeters.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SizeMillimeters As System.Windows.Size
            Get
                Return InchesToMillimeters(_Size)
            End Get
            Friend Set(value As System.Windows.Size)
                _Size = MillimetersToInches(value)
            End Set
        End Property

        ''' <summary>
        ''' Returns the IPaperType IsMetric value.  If this value is true, millimeters are used instead of inches to measure the paper.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsMetric As Boolean Implements IPaperType.IsMetric
            Get
                Return (_Nationality <> PaperNationalities.American)
            End Get
            Set(value As Boolean)
                If value Then
                    _Nationality = PaperNationalities.Iso
                Else
                    _Nationality = PaperNationalities.American
                End If
            End Set
        End Property

        ''' <summary>
        ''' Returns the IPaperType size, which is different according
        ''' to the IPaperType.IsMetric value.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Size As UniSize Implements IPaperType.Size
            Get
                If _Nationality = PaperNationalities.American Then
                    Return SizeInches
                Else
                    Return SizeMillimeters
                End If
            End Get
            Friend Set(value As UniSize)
                If _Nationality = PaperNationalities.American Then
                    SizeInches = value
                Else
                    SizeMillimeters = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Convert inches to millimeters
        ''' </summary>
        ''' <param name="size">A <see cref="System.Windows.Size"/> structure</param>
        ''' <returns></returns>
        Public Shared Function InchesToMillimeters(size As System.Windows.Size) As System.Windows.Size
            Return New System.Windows.Size(size.Width * 25.4, size.Height * 25.4)
        End Function

        ''' <summary>
        ''' Convert millimeters to inches
        ''' </summary>
        ''' <param name="size">A <see cref="System.Windows.Size"/> structure</param>
        ''' <returns></returns>
        Public Shared Function MillimetersToInches(size As System.Windows.Size) As System.Windows.Size
            Return New System.Windows.Size(size.Width / 25.4, size.Height / 25.4)
        End Function


        ''' <summary>
        ''' Returns the <see cref="Description"/> property.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return _Description
        End Function

        Friend Sub New()

        End Sub


        ''' <summary>
        ''' Explicit cast to <see cref="String"/>
        ''' </summary>
        ''' <param name="operand"></param>
        ''' <returns></returns>
        Public Shared Narrowing Operator CType(operand As SystemPaperType) As String
            Return operand.Name
        End Operator

        ''' <summary>
        ''' Explicit cast to <see cref="SystemPaperType"/>
        ''' </summary>
        ''' <param name="operand"></param>
        ''' <returns></returns>
        Public Shared Narrowing Operator CType(operand As String) As SystemPaperType
            For Each t In SystemPaperTypes.PaperTypes
                If t.Name.ToLower = operand.ToLower Then Return t
            Next
            Return Nothing
        End Operator

        ''' <summary>
        ''' Explicit cast to integer
        ''' </summary>
        ''' <param name="operand"></param>
        ''' <returns></returns>

        Public Shared Narrowing Operator CType(operand As SystemPaperType) As Integer
            Return operand.Code
        End Operator

        ''' <summary>
        ''' Explicit cast to <see cref="SystemPaperType"/>
        ''' </summary>
        ''' <param name="operand"></param>
        ''' <returns></returns>
        Public Shared Narrowing Operator CType(operand As Integer) As SystemPaperType
            For Each t In SystemPaperTypes.PaperTypes
                If t.Code = operand Then Return t
            Next
            Return Nothing
        End Operator

        ''' <summary>
        ''' Explicit cast to unsigned integer
        ''' </summary>
        ''' <param name="operand"></param>
        ''' <returns></returns>
        Public Shared Narrowing Operator CType(operand As SystemPaperType) As UInteger
            Return CUInt(operand.Code)
        End Operator

        ''' <summary>
        ''' Explicit cast to <see cref="SystemPaperType"/>
        ''' </summary>
        ''' <param name="operand"></param>
        ''' <returns></returns>
        Public Shared Narrowing Operator CType(operand As UInteger) As SystemPaperType
            For Each t In SystemPaperTypes.PaperTypes
                If t.Code = operand Then Return t
            Next
            Return Nothing
        End Operator

    End Class

#End Region

#Region "JobInfo Class"


    ''' <summary>
    ''' Information about a job in the print queue
    ''' </summary>
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Public Class JobInfo

        Inherits SafeHandle

        Private _ptr As MemPtr
        Private _str As MemPtr

        Friend Sub New(ptr As IntPtr)
            MyBase.New(IntPtr.Zero, True)
            _ptr = ptr
            _str = ptr + 4
            handle = ptr
        End Sub

        Public Overrides ReadOnly Property IsInvalid As Boolean
            Get
                Return (_ptr.Handle = IntPtr.Zero)
            End Get
        End Property

        Protected Overrides Function ReleaseHandle() As Boolean
            Try
                If _ptr.Handle <> IntPtr.Zero Then _ptr.Free()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Job Id
        ''' </summary>
        ''' <returns></returns>
        Public Property JobId As UInteger
            Get
                Return _ptr.UIntAt(0)
            End Get
            Set(value As UInteger)
                _ptr.UIntAt(0) = value
            End Set
        End Property

        ''' <summary>
        ''' The name of the printer printing this job
        ''' </summary>
        ''' <returns></returns>
        Public Property PrinterName As String
            Get
                Return _str.GetStringIndirect(0 * IntPtr.Size)
            End Get
            Set(value As String)
                _str.SetStringIndirect(0 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' The name of the computer that owns this job
        ''' </summary>
        ''' <returns></returns>
        Public Property MachineName As String
            Get
                Return _str.GetStringIndirect(1 * IntPtr.Size)
            End Get
            Set(value As String)
                _str.SetStringIndirect(1 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' The username of the user printing this job
        ''' </summary>
        ''' <returns></returns>
        Public Property UserName As String
            Get
                Return _str.GetStringIndirect(2 * IntPtr.Size)
            End Get
            Set(value As String)
                _str.SetStringIndirect(2 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' The name of the document being printed
        ''' </summary>
        ''' <returns></returns>
        Public Property Document As String
            Get
                Return _str.GetStringIndirect(3 * IntPtr.Size)
            End Get
            Set(value As String)
                _str.SetStringIndirect(3 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Notification name
        ''' </summary>
        ''' <returns></returns>
        Public Property NotifyName As String
            Get
                Return _str.GetStringIndirect(4 * IntPtr.Size)
            End Get
            Set(value As String)
                _str.SetStringIndirect(4 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Data type
        ''' </summary>
        ''' <returns></returns>
        Public Property Datatype As String
            Get
                Return _str.GetStringIndirect(5 * IntPtr.Size)
            End Get
            Set(value As String)
                _str.SetStringIndirect(5 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Print processor
        ''' </summary>
        ''' <returns></returns>
        Public Property PrintProcessor As String
            Get
                Return _str.GetStringIndirect(6 * IntPtr.Size)
            End Get
            Set(value As String)
                _str.SetStringIndirect(6 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Parameters
        ''' </summary>
        ''' <returns></returns>
        Public Property Parameters As String
            Get
                Return _str.GetStringIndirect(7 * IntPtr.Size)
            End Get
            Set(value As String)
                _str.SetStringIndirect(7 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Driver name
        ''' </summary>
        ''' <returns></returns>
        Public Property DriverName As String
            Get
                Return _str.GetStringIndirect(8 * IntPtr.Size)
            End Get
            Set(value As String)
                _str.SetStringIndirect(8 * IntPtr.Size, value)
            End Set
        End Property

        Friend Property DevMode As IntPtr
            Get
                Return If(IntPtr.Size = 8, CType(_str.LongAt(9), IntPtr), CType(_str.IntAt(9), IntPtr))
            End Get
            Set(value As IntPtr)
                If IntPtr.Size = 4 Then
                    _str.IntAt(9) = CInt(value)
                Else
                    _str.LongAt(9) = CLng(value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Status message
        ''' </summary>
        ''' <returns></returns>
        Public Property StatusMessage As String
            Get
                Return _str.GetStringIndirect(10 * IntPtr.Size)
            End Get
            Set(value As String)
                _str.SetStringIndirect(10 * IntPtr.Size, value)
            End Set
        End Property

        Friend Property SecurityDescriptor As IntPtr
            Get
                Return If(IntPtr.Size = 8, CType(_str.LongAt(11), IntPtr), CType(_str.IntAt(11), IntPtr))
            End Get
            Set(value As IntPtr)
                If IntPtr.Size = 4 Then
                    _str.IntAt(11) = CInt(value)
                Else
                    _str.LongAt(11) = CLng(value)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Status code
        ''' </summary>
        ''' <returns></returns>
        Public Property StatusCode As UInteger
            Get
                Dim i As Integer = 4 + (12 * IntPtr.Size)
                Return _ptr.UIntAtAbsolute(i)
            End Get
            Set(value As UInteger)
                Dim i As Integer = 4 + (12 * IntPtr.Size)
                _ptr.UIntAtAbsolute(i) = value
            End Set
        End Property

        ''' <summary>
        ''' Print queue priority
        ''' </summary>
        ''' <returns></returns>
        Public Property Priority As UInteger
            Get
                Dim i As Integer = 8 + (12 * IntPtr.Size)
                Return _ptr.UIntAtAbsolute(i)
            End Get
            Set(value As UInteger)
                Dim i As Integer = 8 + (12 * IntPtr.Size)
                _ptr.UIntAtAbsolute(i) = value
            End Set
        End Property

        ''' <summary>
        ''' Print queue position
        ''' </summary>
        ''' <returns></returns>
        Public Property Position As UInteger
            Get
                Dim i As Integer = 12 + (12 * IntPtr.Size)
                Return _ptr.UIntAtAbsolute(i)
            End Get
            Set(value As UInteger)
                Dim i As Integer = 12 + (12 * IntPtr.Size)
                _ptr.UIntAtAbsolute(i) = value
            End Set
        End Property

        ''' <summary>
        ''' Job start time
        ''' </summary>
        ''' <returns></returns>
        Public Property StartTime As FriendlyUnixTime
            Get
                Dim i As Integer = 16 + (12 * IntPtr.Size)
                Return _ptr.UIntAtAbsolute(i)
            End Get
            Set(value As FriendlyUnixTime)
                Dim i As Integer = 16 + (12 * IntPtr.Size)
                _ptr.UIntAtAbsolute(i) = value
            End Set
        End Property

        ''' <summary>
        ''' Job run unti time
        ''' </summary>
        ''' <returns></returns>
        Public Property UntilTime As FriendlyUnixTime
            Get
                Dim i As Integer = 20 + (12 * IntPtr.Size)
                Return _ptr.UIntAtAbsolute(i)
            End Get
            Set(value As FriendlyUnixTime)
                Dim i As Integer = 20 + (12 * IntPtr.Size)
                _ptr.UIntAtAbsolute(i) = value
            End Set
        End Property

        ''' <summary>
        ''' Total pages in job
        ''' </summary>
        ''' <returns></returns>
        Public Property TotalPages As UInteger
            Get
                Dim i As Integer = 24 + (12 * IntPtr.Size)
                Return _ptr.UIntAtAbsolute(i)
            End Get
            Set(value As UInteger)
                Dim i As Integer = 24 + (12 * IntPtr.Size)
                _ptr.UIntAtAbsolute(i) = value
            End Set
        End Property

        ''' <summary>
        ''' The job size
        ''' </summary>
        ''' <returns></returns>
        Public Property Size As UInteger
            Get
                Dim i As Integer = 28 + (12 * IntPtr.Size)
                Return _ptr.UIntAtAbsolute(i)
            End Get
            Set(value As UInteger)
                Dim i As Integer = 28 + (12 * IntPtr.Size)
                _ptr.UIntAtAbsolute(i) = value
            End Set
        End Property

        ''' <summary>
        ''' The time the job was submitted
        ''' </summary>
        ''' <returns></returns>
        Public Property Submitted As Date
            Get
                Dim i As Integer = 32 + (12 * IntPtr.Size)
                Return CDate(_ptr.ToStructAt(Of SYSTEMTIME)(i))
            End Get
            Set(value As Date)
                Dim i As Integer = 32 + (12 * IntPtr.Size)
                _ptr.FromStructAt(Of SYSTEMTIME)(i, CType(value, SYSTEMTIME))
            End Set
        End Property

        ''' <summary>
        ''' Elapsed time (in seconds)
        ''' </summary>
        ''' <returns></returns>
        Public Property Time As UInteger
            Get
                Dim i As Integer = 48 + (12 * IntPtr.Size)
                Return _ptr.UIntAtAbsolute(i)
            End Get
            Set(value As UInteger)
                Dim i As Integer = 48 + (12 * IntPtr.Size)
                _ptr.UIntAtAbsolute(i) = value
            End Set
        End Property

        ''' <summary>
        ''' Page finished printing
        ''' </summary>
        ''' <returns></returns>
        Public Property PagePrinted As UInteger
            Get
                Dim i As Integer = 52 + (12 * IntPtr.Size)
                Return _ptr.UIntAtAbsolute(i)
            End Get
            Set(value As UInteger)
                Dim i As Integer = 52 + (12 * IntPtr.Size)
                _ptr.UIntAtAbsolute(i) = value
            End Set
        End Property

    End Class

#End Region

#Region "Printer Object Collection"

    ''' <summary>
    ''' A collection of printers available to the machine
    ''' </summary>
    Public Class PrinterObjects
        Inherits ObservableCollection(Of PrinterObject)

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Friend Structure PRINTER_INFO_4
            <MarshalAs(UnmanagedType.LPWStr)>
            Public pPrinterName As String

            <MarshalAs(UnmanagedType.LPWStr)>
            Public pServerName As String

            Public Attributes As UInteger
        End Structure

        Private Sub New()
        End Sub

        Private Shared _printers As PrinterObjects

        Shared Sub New()
            _printers = New PrinterObjects
            Refresh()
        End Sub

        ''' <summary>
        ''' The collection of printers
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property Printers As PrinterObjects
            Get
                Return _printers
            End Get
        End Property

        ''' <summary>
        ''' Refresh the available printers
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function Refresh() As Boolean

            Dim mm As New MemPtr
            Dim ap As MemPtr
            Dim cb As UInteger = 0
            Dim rc As UInteger = 0
            Dim pif As New PRINTER_INFO_4
            Dim sp As New List(Of String)
            Dim ps As Integer = Marshal.SizeOf(pif)
            Dim ts As String

            EnumPrinters(PRINTER_ENUM_NAME, "", 4, IntPtr.Zero, 0, cb, rc)

            If cb > 0 Then
                mm.Alloc(cb)
                ap = mm
                EnumPrinters(PRINTER_ENUM_NAME, "", 4, mm, cb, cb, rc)

                cb = 0

                For u As Integer = 1 To CInt(rc)
                    ts = ap.GetStringIndirect(cb)
                    sp.Add(ts)
                    ap += ps
                Next

                mm.Free()
            Else
                Return False
            End If

            Dim po As PrinterObject = Nothing

            _printers.Clear()

            For Each s In sp
                Try
                    'MsgBox("Attempting to get highly detailed printer information for '" & s & "'")
                    If (s.Trim <> s) Then
                        ' MsgBox("Printer name has extra space characters after name! '" & s & "'", MsgBoxStyle.Exclamation)
                    End If
                    po = PrinterObject.GetPrinterInfoObject(s)
                Catch ex As Exception
                    po = Nothing
                End Try

                If po IsNot Nothing Then
                    _printers.Add(po)
                    po = Nothing
                Else
                    'MsgBox("For some reason, the printer that the system just reported would not return a useful Object." & vbCrLf & "Printer: " & s)
                End If
            Next

            Return (_printers.Count > 0)
        End Function

    End Class

#End Region

#Region "PrinterObject Class"

    ''' <summary>
    ''' Encapsulates a printer queue on the system.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PrinterObject
        Inherits CriticalFinalizerObject
        Implements IDisposable, IEquatable(Of PrinterObject)

        Friend _ptr As MemPtr

        '' This is a scratch-pad memory pointer for various getting and setting functions
        '' so that we don't have to keep allocating and deallocating resources.
        Private _mm As New MemPtr(16)

        Private _DevMode As DeviceMode

#Region "Object Stuff (New, Finalize, Dispose, Equals, ToString, etc...)"

        Public Overloads Function Equals(other As PrinterObject) As Boolean Implements IEquatable(Of DataTools.Interop.Printers.PrinterObject).Equals
            Return Me.PrinterName = other.PrinterName
        End Function

        Private Sub New()
        End Sub

        Friend Sub New(ptr As IntPtr, fOwn As Boolean)
            _fOwn = fOwn
            _ptr = ptr

            Dim mm As MemPtr = ptr

            If IntPtr.Size = 4 Then
                _DevMode = New DeviceMode(CType(mm.IntAt(7), IntPtr), False)
                '   MsgBox("Got a pointer! x86 mode.")
            Else
                _DevMode = New DeviceMode(CType(mm.LongAt(7), IntPtr), False)
                '    MsgBox("Got a pointer! x64 mode.")
                'If printer._DevMode IsNot Nothing Then MsgBox("DevMode retrieval successful, devmode reports device name as '" & printer._DevMode.DeviceName & "'")
            End If

        End Sub

        Private _fOwn As Boolean = True

        Public Sub Dispose() Implements IDisposable.Dispose
            If _fOwn Then _ptr.Free()
            _mm.Free()
            GC.SuppressFinalize(Me)
        End Sub

        Public Sub New(printerName As String)
            internalGetPrinter(printerName, Me)
        End Sub

        Protected Overrides Sub Finalize()
            If _fOwn Then _ptr.Free()
            _mm.Free()
        End Sub

        Public Overrides Function ToString() As String
            Return PrinterName
        End Function

#End Region

#Region "Shared Methods"

        ''' <summary>
        ''' Get the printable area of the page
        ''' </summary>
        ''' <param name="printer">The printer</param>
        ''' <param name="paper">Paper type</param>
        ''' <param name="resolution">Resolution</param>
        ''' <param name="orientation">Orientation</param>
        ''' <returns></returns>
        Public Shared Function GetPrintableArea(printer As PrinterObject, paper As SystemPaperType, resolution As UniSize, Optional orientation As Integer = 0) As UniRect

            Dim rc As New UniRect
            Dim dev As DeviceMode = CType(printer._DevMode.Clone, DeviceMode)

            dev.Fields = DeviceModeFields.Orientation Or DeviceModeFields.PaperSize Or DeviceModeFields.YResolution

            dev.YResolution = CShort(resolution.cy)
            dev.PrintQuality = CShort(resolution.cx)

            dev.PaperSize = paper
            dev.Orientation = CShort(orientation)

            Dim hdc = CreateDC(Nothing, printer.PrinterName, IntPtr.Zero, printer._DevMode._ptr)

            If hdc <> IntPtr.Zero Then
                Dim cx As Integer = GetDeviceCaps(hdc, PHYSICALWIDTH)
                Dim cy As Integer = GetDeviceCaps(hdc, PHYSICALHEIGHT)

                Dim marginX = GetDeviceCaps(hdc, PHYSICALOFFSETX)
                Dim marginY = GetDeviceCaps(hdc, PHYSICALOFFSETY)

                DeleteDC(hdc)

                rc.Left = marginX / resolution.cx
                rc.Top = marginY / resolution.cy

                rc.Width = (cx - (marginX * 2)) / resolution.cx
                rc.Height = (cy - (marginY * 2)) / resolution.cy

            End If

            Return rc
        End Function

        ''' <summary>
        ''' Get <see cref="PrinterObject"/> by name
        ''' </summary>
        ''' <param name="name">The name of the printer</param>
        ''' <returns></returns>
        Public Shared Function GetPrinterInfoObject(name As String) As PrinterObject
            GetPrinterInfoObject = Nothing
            internalGetPrinter(name, GetPrinterInfoObject)
        End Function

        <DllImport("winspool.drv", EntryPoint:="GetDefaultPrinterW", CharSet:=CharSet.Unicode)>
        Private Shared Function GetDefaultPrinter(pszBuffer As IntPtr, ByRef pcchBuffer As UInteger) As Boolean
        End Function

        ''' <summary>
        ''' Returns the default printer for the system.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDefaultPrinter() As PrinterObject
            Return New PrinterObject(DefaultPrinterName)
        End Function

        ''' <summary>
        ''' Returns the name of the default printer
        ''' </summary>
        ''' <returns></returns>
        Public Shared ReadOnly Property DefaultPrinterName As String
            Get
                Dim l As UInteger = 0
                Dim mm As MemPtr

                GetDefaultPrinter(IntPtr.Zero, l)

                If l <= 0 Then Return Nothing

                l = CUInt((l * 2) + 2)
                mm.Alloc(l)

                If GetDefaultPrinter(mm, l) Then
                    DefaultPrinterName = CStr(mm)
                Else
                    DefaultPrinterName = Nothing
                End If

                mm.Free()

            End Get
        End Property

        Private Shared Sub internalGetPrinter(name As String, ByRef printer As PrinterObject)

            'MsgBox("We are in internalGetPrinter for " & name)

            Dim mm As New MemPtr
            Dim cb As UInteger = 0
            Dim hprinter As IntPtr = IntPtr.Zero

            If String.IsNullOrEmpty(name) Then
                MsgBox("Got null printer name.")
                printer = Nothing
                Return
            End If

            If Not OpenPrinter(name, hprinter, IntPtr.Zero) Then

                MsgBox("OpenPrinter failed, last Win32 Error: " & FormatLastError(CUInt(Marshal.GetLastWin32Error)))
                Return

            End If

            'MsgBox("Open Printer for '" & name & "' succeeded...")
            Try

                GetPrinter(hprinter, 2, IntPtr.Zero, 0, cb)

                mm.Alloc(cb)
                GetPrinter(hprinter, 2, mm, cb, cb)

                If printer Is Nothing Then
                    printer = New PrinterObject()
                    printer._ptr = mm
                Else
                    If printer._ptr.Handle <> IntPtr.Zero Then
                        Try
                            printer._ptr.Free()
                        Catch ex As Exception

                        End Try
                    End If
                    '' we will be holding on to this.
                    printer._ptr = mm
                End If

                If IntPtr.Size = 4 Then
                    printer._DevMode = New DeviceMode(CType(mm.IntAt(7), IntPtr), False)
                    '   MsgBox("Got a pointer! x86 mode.")
                Else
                    printer._DevMode = New DeviceMode(CType(mm.LongAt(7), IntPtr), False)
                    '    MsgBox("Got a pointer! x64 mode.")
                    'If printer._DevMode IsNot Nothing Then MsgBox("DevMode retrieval successful, devmode reports device name as '" & printer._DevMode.DeviceName & "'")
                End If

            Catch ex As Exception
                File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\sbslog.log", ex.Message & vbCrLf & ex.StackTrace & vbCrLf & vbCrLf)
            End Try

            Dim sz = GetMaxDPIForPrinter(printer, hprinter)

            ClosePrinter(hprinter)

            internalPopulateDevCaps(printer)
        End Sub

        Private Shared Sub internalPopulateDevCaps(ByRef printer As PrinterObject)
            Try
                Dim mm = New MemPtr

                '' Get the supported resolutions.

                '               MsgBox("Polling printer for available print quality resolution.")

                Dim res() As UInteger
                Dim l As UInteger = DeviceCapabilities(printer.PrinterName, printer.PortName, DC_ENUMRESOLUTIONS, IntPtr.Zero, IntPtr.Zero)

                If printer.PrinterName.Contains("HP LaserJet") Then

                    If l = &HFFFFFFFFUI Then
                        'MsgBox("Attempt to get resolutions failed miserably, let's try it without the port name...")
                        l = DeviceCapabilities(printer.PrinterName, Nothing, DC_ENUMRESOLUTIONS, IntPtr.Zero, IntPtr.Zero)

                        If l = &HFFFFFFFFUI Then
                            'MsgBox("That still failed.  We are going to give the LaserJet practical resolutions, 600x600 and 1200x1200 so that it won't barf")
                            Dim nRes As New List(Of System.Windows.Size)
                            nRes.AddRange({New System.Windows.Size(600, 600), New System.Windows.Size(1200, 1200)})
                            printer._Resolutions = nRes
                        End If
                    End If

                    If (l > 0) And (l <> &HFFFFFFFFUI) Then
                        'MsgBox("HP LaserJet SAYS it has " & l & " resolutions.")
                        res = Nothing
                        mm = New MemPtr(l * 8)

                        l = DeviceCapabilities(printer.PrinterName, printer.PortName, DC_ENUMRESOLUTIONS, mm, IntPtr.Zero)

                        'MsgBox("Retrieved printer resolutions, RetVal=" & l)

                        Try
                            '    MsgBox("Casting memory into UInteger() array")
                            res = mm.ToArray(Of UInteger)()
                        Catch ex As Exception
                            MsgBox("Getting Resolutions bounced!", MsgBoxStyle.Exclamation)
                        End Try

                        mm.Free()

                        If res IsNot Nothing Then
                            Dim stm As String = ""
                            For Each rn In res
                                If stm <> "" Then stm &= ", "
                                stm &= rn
                            Next

                            'MsgBox("Resolution raw data for LaserJet: " & stm)

                            Dim supRes As New List(Of System.Windows.Size)

                            'MsgBox("Res count should be divisible by two, is it? Count: " & res.Count)

                            For i = 0 To res.Length - 1 Step 2
                                If ((res.Length Mod 2) <> 0) AndAlso (i = (res.Length - 1)) Then
                                    supRes.Add(New System.Windows.Size(res(i), res(i)))
                                Else
                                    supRes.Add(New System.Windows.Size(res(i), res(i + 1)))
                                End If
                            Next

                            'MsgBox("Finally, we're going to report that the printer has " & supRes.Count & " resolutions.")
                            printer.Resolutions = supRes
                        End If

                    End If

                Else
                    If (l > 0) And (l <> &HFFFFFFFFUI) Then
                        res = Nothing
                        mm = New MemPtr(l * 8)

                        l = DeviceCapabilities(printer.PrinterName, printer.PortName, DC_ENUMRESOLUTIONS, mm, IntPtr.Zero)

                        Try
                            res = mm.ToArray(Of UInteger)
                        Catch ex As Exception
                            MsgBox("Getting Resolutions bounced!", MsgBoxStyle.Exclamation)
                        End Try

                        mm.Free()

                        Dim supRes As New List(Of System.Windows.Size)

                        If res IsNot Nothing Then
                            For i = 0 To res.Length - 1 Step 2

                                If ((res.Length Mod 2) <> 0) AndAlso (i = (res.Length - 1)) Then
                                    supRes.Add(New System.Windows.Size(res(i), res(i)))
                                Else
                                    supRes.Add(New System.Windows.Size(res(i), res(i + 1)))
                                End If
                            Next

                            printer.Resolutions = supRes
                        Else
                            Dim nRes As New List(Of System.Windows.Size)
                            nRes.AddRange({New System.Windows.Size(300, 300), New System.Windows.Size(600, 600), New System.Windows.Size(1200, 1200)})
                            printer._Resolutions = nRes
                        End If

                    End If

                End If

                'MsgBox("Found " & printer.Resolutions.Count & " resolutions.")

                'MsgBox("Getting paper class sizes")
                '' Get the supported paper types.
                l = DeviceCapabilities(printer.PrinterName, printer.PortName, DC_PAPERS, IntPtr.Zero, IntPtr.Zero)

                '' supported paper types are short ints:
                If (l > 0) Then
                    mm = New MemPtr(l * 2)

                    l = DeviceCapabilities(printer.PrinterName, printer.PortName, DC_PAPERS, mm, IntPtr.Zero)
                    printer._PaperSizes = SystemPaperTypes.TypeListFromCodes(mm.ToArray(Of Short))

                    mm.Free()
                End If

                'MsgBox("Retrieved " & l & " supported paper class sizes.")

                'MsgBox("Looking for the printer trays.")
                '' get the names of the supported paper bins.
                l = DeviceCapabilities(printer.PrinterName, printer.PortName, DC_BINNAMES, IntPtr.Zero, IntPtr.Zero)

                If l > 0 Then
                    mm = New MemPtr(l * 24 * 2)
                    mm.ZeroMemory()

                    DeviceCapabilities(printer.PrinterName, printer.PortName, DC_BINNAMES, mm, IntPtr.Zero)
                    printer._Bins.Clear()

                    Dim srs As String,
                        p As Integer

                    For i = 0 To l - 1

                        '' some p.o.s. printers make it hard.
                        srs = Nothing
                        srs = mm.GetString((i * 24 * 2), 24)

                        If srs IsNot Nothing Then
                            For p = 0 To 23
                                If srs.Chars(p) = ChrW(0) Then Exit For
                            Next

                            If p < 24 AndAlso (p <> 0) Then
                                srs = srs.Substring(0, p)
                            ElseIf p = 0 Then
                                srs = "Unnamed Tray (#" & (i + 1) & ")"
                            End If

                            '           MsgBox("Adding printer bin/tray " & srs)
                            printer._Bins.Add(srs)
                        End If
                    Next

                    mm.Free()
                End If

                l = DeviceCapabilities(printer.PrinterName, printer.PortName, DC_COLORDEVICE, IntPtr.Zero, IntPtr.Zero)
                printer.IsColorPrinter = CBool(l)

                printer._LandscapeRotation = CInt(DeviceCapabilities(printer.PrinterName, printer.PortName, DC_ORIENTATION, IntPtr.Zero, IntPtr.Zero))

            Catch ex As Exception

                File.AppendAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\sbslog.log", ex.Message & vbCrLf & ex.StackTrace & vbCrLf & vbCrLf)

            End Try

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Select a printer into this object by printer name.
        ''' </summary>
        ''' <param name="printerName"></param>
        ''' <remarks></remarks>
        Public Sub SelectPrinter(printerName As String)
            internalGetPrinter(printerName, Me)
        End Sub

        ''' <summary>
        ''' Returns true if this printer is the Windows system default printer.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsDefault As Boolean
            Get
                Return PrinterName = DefaultPrinterName
            End Get
        End Property

#End Region

#Region "DeviceCapabilities Properties"

        Private _LandscapeRotation As Integer

        ''' <summary>
        ''' Returns the relationship between portrait and landscape orientations for a device, <br/>
        ''' in terms of the number of degrees that portrait orientation is rotated counterclockwise <br/>
        ''' to produce landscape orientation.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LandscapeRotation As Integer
            Get
                Return _LandscapeRotation
            End Get
            Friend Set(value As Integer)
                _LandscapeRotation = value
            End Set
        End Property

        Private _IsColor As Boolean

        ''' <summary>
        ''' Returns true if the printer is capable of color.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsColorPrinter As Boolean
            Get
                Return _IsColor
            End Get
            Friend Set(value As Boolean)
                _IsColor = value
            End Set
        End Property

        ''' <summary>
        ''' Reports the highest resolution that this printer is capable of printing in.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property HighestResolution As System.Windows.Size
            Get
                Dim hsize As New System.Windows.Size

                For Each r In _Resolutions

                    If r.Width > hsize.Width And r.Height > hsize.Height Then
                        hsize = r
                    End If

                Next

                Return hsize
            End Get
        End Property

        Private _Resolutions As List(Of System.Windows.Size)

        ''' <summary>
        ''' Gets a list of all supported resolutions.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Resolutions As ICollection(Of System.Windows.Size)
            Get
                Return _Resolutions
            End Get
            Friend Set(value As ICollection(Of System.Windows.Size))
                _Resolutions = CType(value, List(Of Windows.Size))
            End Set
        End Property

        Private _PaperSizes As List(Of SystemPaperType)

        ''' <summary>
        ''' Gets a list of all paper sizes supported by this printer.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PaperSizes As ICollection(Of SystemPaperType)
            Get
                Return _PaperSizes
            End Get
            Friend Set(value As ICollection(Of SystemPaperType))
                _PaperSizes = CType(value, List(Of SystemPaperType))
            End Set
        End Property

        ''' <summary>
        ''' Gets a value indicating that this printer supports this particular paper size.
        ''' </summary>
        ''' <param name="size">The System.Windows.Size structure to compare.</param>
        ''' <param name="sizeMetric">True if the given size is in millimeters.</param>
        ''' <param name="exactOrientation">True to not compare rotated sizes.</param>
        ''' <returns>True if all conditions are met and a size match is found.</returns>
        ''' <remarks></remarks>
        Public Function SupportsPaperSize(size As System.Windows.Size, Optional sizeMetric As Boolean = False, Optional exactOrientation As Boolean = False) As Boolean

            '' two separate for-eaches for time-saving. We don't need to test for sizeMetric for every iteration.
            '' we don't need to test exactOrientation every time, either, but that's only referenced once per iteration.

            If sizeMetric Then
                '' testing for the millimeters size.
                For Each p In _PaperSizes
                    If p.SizeMillimeters.Equals(size) Then Return True

                    If exactOrientation Then Continue For

                    With p.SizeMillimeters
                        If .Width = size.Height AndAlso .Height = size.Width Then Return True
                    End With
                Next
            Else
                For Each p In _PaperSizes
                    If p.SizeInches.Equals(size) Then Return True

                    If exactOrientation Then Continue For

                    With p.SizeInches
                        If .Width = size.Height AndAlso .Height = size.Width Then Return True
                    End With
                Next
            End If

            '' nothing found!
            Return False
        End Function

        Private _Bins As New List(Of String)

        ''' <summary>
        ''' Returns a list of all of the available trays this printer serves from.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PrinterTrays As ICollection(Of String)
            Get
                Return _Bins
            End Get
            Friend Set(value As ICollection(Of String))
                _Bins = CType(value, List(Of String))
            End Set
        End Property

#End Region

#Region "Properties based on PRINTER_INFO_2"

        ''' <summary>
        ''' Gets the server name of this printer.  If this string is null, the printer is served locally.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ServerName As String
            Get
                Return _ptr.GetStringIndirect(0 * IntPtr.Size)
            End Get
            Friend Set(value As String)
                _ptr.SetStringIndirect(0 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets the name of the printer.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PrinterName As String
            Get
                Return _ptr.GetStringIndirect(1 * IntPtr.Size)
            End Get
            Friend Set(value As String)
                _ptr.SetStringIndirect(1 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets the share name of the printer.
        ''' If this printer is not shared, this value is null.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ShareName As String
            Get
                Return _ptr.GetStringIndirect(2 * IntPtr.Size)
            End Get
            Friend Set(value As String)
                _ptr.SetStringIndirect(2 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets the port name of the printer.  This could be a standard port, or a special port name.
        ''' <br/>
        ''' If a printer is connected to more than one port, the names of each port must be separated by commas (for example, "LPT1:,LPT2:,LPT3:").
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PortName As String
            Get
                Return _ptr.GetStringIndirect(3 * IntPtr.Size)
            End Get
            Friend Set(value As String)
                _ptr.SetStringIndirect(3 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Returns the name of the printer driver.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DriverName As String
            Get
                Return _ptr.GetStringIndirect(4 * IntPtr.Size)
            End Get
            Friend Set(value As String)
                _ptr.SetStringIndirect(4 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets a brief desecription of the printer.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Comment As String
            Get
                Return _ptr.GetStringIndirect(5 * IntPtr.Size)
            End Get
            Friend Set(value As String)
                _ptr.SetStringIndirect(5 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets a string that specifies the physical location of the printer (for example, "Bldg. 38, Room 1164").
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Location As String
            Get
                Return _ptr.GetStringIndirect(6 * IntPtr.Size)
            End Get
            Friend Set(value As String)
                _ptr.SetStringIndirect(6 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets the device mode object that reports on and controls further aspect of the printer's current configuration.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DevMode As DeviceMode
            Get
                Return _DevMode
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets a string that specifies the name of the file used to create the separator page. This page is used to separate print jobs sent to the printer.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SepFile As String
            Get
                Return _ptr.GetStringIndirect(8 * IntPtr.Size)
            End Get
            Set(value As String)
                _ptr.SetStringIndirect(8 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a string that specifies the name of the print processor used by the printer.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>You can use the EnumPrintProcessors function to obtain a list of print processors installed on a server.</remarks>
        Public Property PrintProcessor As String
            Get
                Return _ptr.GetStringIndirect(9 * IntPtr.Size)
            End Get
            Set(value As String)
                _ptr.SetStringIndirect(9 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a string that specifies the data type used to record the print job.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>You can use the EnumPrintProcessorDatatypes function to obtain a list of data types supported by a specific print processor.</remarks>
        Public Property Datatype As String
            Get
                Return _ptr.GetStringIndirect(10 * IntPtr.Size)
            End Get
            Set(value As String)
                _ptr.SetStringIndirect(10 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets a string that specifies the default print-processor parameters.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Parameters As String
            Get
                Return _ptr.GetStringIndirect(11 * IntPtr.Size)
            End Get
            Set(value As String)
                _ptr.SetStringIndirect(11 * IntPtr.Size, value)
            End Set
        End Property

        ''' <summary>
        ''' Pointer to a SECURITY_DESCRIPTOR structure containing the ACL info.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SecurityDescriptor As IntPtr
            Get
                Return CType(_ptr.LongAt(12), IntPtr)
            End Get
            Friend Set(value As IntPtr)
                _ptr.LongAt(12) = CLng(value)
            End Set
        End Property

        ''' <summary>
        ''' gets or sets the printer's attributes.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Attributes As PrinterAttributes
            Get
                Return CType(_ptr.UIntAt(26), PrinterAttributes)
            End Get
            Set(value As PrinterAttributes)
                _ptr.UIntAt(26) = value
            End Set
        End Property

        ''' <summary>
        ''' A priority value that the spooler uses to route print jobs.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Priority As UInteger
            Get
                Return _ptr.UIntAt(27)
            End Get
            Set(value As UInteger)
                _ptr.UIntAt(27) = value
            End Set
        End Property

        ''' <summary>
        ''' The default priority value assigned to each print job.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DefaultPriority As UInteger
            Get
                Return _ptr.UIntAt(28)
            End Get
            Friend Set(value As UInteger)
                _ptr.UIntAt(28) = value
            End Set
        End Property

        ''' <summary>
        ''' The earliest time of day the printer will start taking print jobs for the day.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StartTime As TimeSpan
            Get
                Return New TimeSpan(0, _ptr.IntAt(29), 0)
            End Get
            Friend Set(value As TimeSpan)
                _ptr.IntAt(29) = CInt(value.TotalMinutes)
            End Set
        End Property

        ''' <summary>
        ''' The time of day that the printer stops taking jobs for the day.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UntilTime As TimeSpan
            Get
                Return New TimeSpan(0, _ptr.IntAt(30), 0)
            End Get
            Friend Set(value As TimeSpan)
                _ptr.IntAt(30) = CInt(value.TotalMinutes)
            End Set
        End Property

        Private _LiveUpdateStatus As Boolean = False

        ''' <summary>
        ''' Gets or sets a value indicating that the status will be updated live, every time it is retrieved.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LiveUpdateStatus As Boolean
            Get
                Return _LiveUpdateStatus
            End Get
            Set(value As Boolean)
                _LiveUpdateStatus = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the current printer status.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Status As PrinterStatus
            Get
                If _LiveUpdateStatus Then Return UpdateStatus()
                Return CType(_ptr.UIntAt(31), PrinterStatus)
            End Get
            Friend Set(value As PrinterStatus)
                _ptr.UIntAt(31) = value
            End Set
        End Property

        ''' <summary>
        ''' Updates the printer's current status.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function UpdateStatus() As PrinterStatus
            Dim cb As UInteger = 0
            Dim hprinter As IntPtr = IntPtr.Zero

            If Not OpenPrinter(PrinterName, hprinter, IntPtr.Zero) Then Return PrinterStatus.Error

            GetPrinter(hprinter, 6, IntPtr.Zero, 0, cb)

            If cb > 16 Then
                _mm.ReAlloc(cb)
            End If

            GetPrinter(hprinter, 6, _mm, cb, cb)
            _ptr.UIntAt(31) = _mm.UIntAt(0)

            ClosePrinter(hprinter)

            Return CType(_ptr.UIntAt(31), PrinterStatus)

        End Function

        ''' <summary>
        ''' The number of printer jobs in this printer's queue.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property cJobs As UInteger
            Get
                Return _ptr.UIntAt(32)
            End Get
            Friend Set(value As UInteger)
                _ptr.UIntAt(32) = value
            End Set
        End Property

        ''' <summary>
        ''' This printer's average pages per minute.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AveragePPM As UInteger
            Get
                Return _ptr.UIntAt(33)
            End Get
            Friend Set(value As UInteger)
                _ptr.UIntAt(33) = value
            End Set
        End Property

#End Region

#Region "Operators"


        ''' <summary>
        ''' Explicitly cast string to <see cref="PrinterObject"/>
        ''' </summary>
        ''' <param name="operand"></param>
        ''' <returns></returns>
        Public Shared Narrowing Operator CType(operand As String) As PrinterObject
            Try
                Return New PrinterObject(operand)
            Catch ex As Exception
                Throw New KeyNotFoundException("That printer was not found on the system.", ex)
            End Try
        End Operator

#End Region

    End Class

#End Region

#Region "DeviceMode: DEVMODE Structure Wrapper Class"

    Public Class DeviceMode
        Inherits CriticalFinalizerObject
        Implements IDisposable, IEquatable(Of DeviceMode), ICloneable

        Friend _ptr As MemPtr
        Private _own As Boolean = True

        Friend Sub New(ptr As IntPtr, fOwn As Boolean)
            _ptr = ptr
            _own = fOwn
        End Sub

        ''' <summary>
        ''' Device name
        ''' </summary>
        ''' <returns></returns>
        Public Property DeviceName As String
            Get
                Return _ptr.GetString(0, 32).Trim(ChrW(0))
            End Get
            Set(value As String)
                If (value.Length > 32) Then value = value.Substring(0, 32)
                _ptr.SetString(0, value)
            End Set
        End Property

        ''' <summary>
        ''' Spec version
        ''' </summary>
        ''' <returns></returns>
        Public Property SpecVersion As UShort
            Get
                Return _ptr.UShortAtAbsolute(64)
            End Get
            Set(value As UShort)
                _ptr.UShortAtAbsolute(64) = value
            End Set
        End Property

        ''' <summary>
        ''' Driver version
        ''' </summary>
        ''' <returns></returns>
        Public Property DriverVersion As UShort
            Get
                Return _ptr.UShortAtAbsolute(66)
            End Get
            Set(value As UShort)
                _ptr.UShortAtAbsolute(66) = value
            End Set
        End Property

        ''' <summary>
        ''' Size
        ''' </summary>
        ''' <returns></returns>
        Public Property Size As UShort
            Get
                Return _ptr.UShortAtAbsolute(68)
            End Get
            Set(value As UShort)
                _ptr.UShortAtAbsolute(68) = value
            End Set
        End Property

        ''' <summary>
        ''' Driver Extra
        ''' </summary>
        ''' <returns></returns>
        Public Property DriverExtra As UShort
            Get
                Return _ptr.UShortAtAbsolute(70)
            End Get
            Set(value As UShort)
                _ptr.UShortAtAbsolute(70) = value
            End Set
        End Property

        ''' <summary>
        ''' Device mode fields
        ''' </summary>
        ''' <returns></returns>
        Public Property Fields As DeviceModeFields
            Get
                Return CType(_ptr.UIntAtAbsolute(72), DeviceModeFields)
            End Get
            Set(value As DeviceModeFields)
                _ptr.UIntAtAbsolute(72) = value
            End Set
        End Property

        'union

        'struct

        Public Property Orientation As Short
            Get
                Return _ptr.ShortAtAbsolute(76)
            End Get
            Set(value As Short)
                _ptr.ShortAtAbsolute(76) = value
            End Set
        End Property

        Public Property PaperSize As SystemPaperType
            Get
                Return CType(_ptr.ShortAtAbsolute(78), SystemPaperType)
            End Get
            Set(value As SystemPaperType)
                _ptr.ShortAtAbsolute(78) = CShort(value.Code)
            End Set
        End Property

        Public Property PaperSizeCode As Short
            Get
                Return _ptr.ShortAtAbsolute(78)
            End Get
            Set(value As Short)
                _ptr.ShortAtAbsolute(78) = value
            End Set
        End Property

        Public Property PaperLength As Short
            Get
                Return _ptr.ShortAtAbsolute(80)
            End Get
            Set(value As Short)
                _ptr.ShortAtAbsolute(80) = value
            End Set
        End Property

        Public Property PaperWidth As Short
            Get
                Return _ptr.ShortAtAbsolute(82)
            End Get
            Set(value As Short)
                _ptr.ShortAtAbsolute(82) = value
            End Set
        End Property

        Public Property Scale As Short
            Get
                Return _ptr.ShortAtAbsolute(84)
            End Get
            Set(value As Short)
                _ptr.ShortAtAbsolute(84) = value
            End Set
        End Property

        Public Property Copies As Short
            Get
                Return _ptr.ShortAtAbsolute(86)
            End Get
            Set(value As Short)
                _ptr.ShortAtAbsolute(86) = value
            End Set
        End Property

        Public Property DefaultSource As Short
            Get
                Return _ptr.ShortAtAbsolute(88)
            End Get
            Set(value As Short)
                _ptr.ShortAtAbsolute(88) = value
            End Set
        End Property

        Public Property PrintQuality As Short
            Get
                Return _ptr.ShortAtAbsolute(90)
            End Get
            Set(value As Short)
                _ptr.ShortAtAbsolute(90) = value
            End Set
        End Property

        'struct

        Public Property Position As System.Drawing.Point
            Get
                Return New System.Drawing.Point(_ptr.IntAtAbsolute(76), _ptr.IntAtAbsolute(80))
            End Get
            Set(value As System.Drawing.Point)
                _ptr.IntAtAbsolute(76) = value.X
                _ptr.IntAtAbsolute(80) = value.Y
            End Set
        End Property

        Public Property DisplayOrientation As UInteger
            Get
                Return _ptr.UIntAtAbsolute(84)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(84) = value
            End Set
        End Property

        Public Property DisplayFixedOutput As UInteger
            Get
                Return _ptr.UIntAtAbsolute(88)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(88) = value
            End Set
        End Property

        ' end union

        Public Property Color As Short
            Get
                Return _ptr.ShortAtAbsolute(92)
            End Get
            Set(value As Short)
                _ptr.ShortAtAbsolute(92) = value
            End Set
        End Property

        Public Property Duplex As Short
            Get
                Return _ptr.ShortAtAbsolute(94)
            End Get
            Set(value As Short)
                _ptr.ShortAtAbsolute(94) = value
            End Set
        End Property

        Public Property YResolution As Short
            Get
                Return _ptr.ShortAtAbsolute(96)
            End Get
            Set(value As Short)
                _ptr.ShortAtAbsolute(96) = value
            End Set
        End Property

        Public Property TTOption As Short
            Get
                Return _ptr.ShortAtAbsolute(98)
            End Get
            Set(value As Short)
                _ptr.ShortAtAbsolute(98) = value
            End Set
        End Property

        Public Property Collate As Short
            Get
                Return _ptr.ShortAtAbsolute(100)
            End Get
            Set(value As Short)
                _ptr.ShortAtAbsolute(100) = value
            End Set
        End Property

        Public Property FormName As String
            Get
                Return _ptr.GetString(102, 32).Trim(ChrW(0))
            End Get
            Set(value As String)
                If (value.Length > 32) Then value = value.Substring(0, 32)
                _ptr.SetString(102, value)
            End Set
        End Property

        Public Property LogPixels As UShort
            Get
                Return _ptr.UShortAtAbsolute(168)
            End Get
            Set(value As UShort)
                _ptr.UShortAtAbsolute(168) = value
            End Set
        End Property

        Public Property BitsPerPel As UInteger
            Get
                Return _ptr.UIntAtAbsolute(170)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(170) = value
            End Set
        End Property

        Public Property PelsWidth As UInteger
            Get
                Return _ptr.UIntAtAbsolute(174)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(174) = value
            End Set
        End Property

        Public Property PelsHeight As UInteger
            Get
                Return _ptr.UIntAtAbsolute(178)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(178) = value
            End Set
        End Property

        'union

        Public Property DisplayFlags As UInteger
            Get
                Return _ptr.UIntAtAbsolute(182)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(182) = value
            End Set
        End Property

        Public Property Nup As UInteger
            Get
                Return _ptr.UIntAtAbsolute(182)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(182) = value
            End Set
        End Property

        'end union

        Public Property DisplayFrequency As UInteger
            Get
                Return _ptr.UIntAtAbsolute(186)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(186) = value
            End Set
        End Property

        Public Property ICMMethod As UInteger
            Get
                Return _ptr.UIntAtAbsolute(190)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(190) = value
            End Set
        End Property

        Public Property ICMIntent As UInteger
            Get
                Return _ptr.UIntAtAbsolute(194)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(194) = value
            End Set
        End Property

        Public Property MediaType As UInteger
            Get
                Return _ptr.UIntAtAbsolute(198)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(198) = value
            End Set
        End Property

        Public Property DitherType As UInteger
            Get
                Return _ptr.UIntAtAbsolute(202)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(202) = value
            End Set
        End Property

        Public Property Reserved1 As UInteger
            Get
                Return _ptr.UIntAtAbsolute(206)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(206) = value
            End Set
        End Property

        Public Property Reserved2 As UInteger
            Get
                Return _ptr.UIntAtAbsolute(210)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(210) = value
            End Set
        End Property

        Public Property PanningWidth As UInteger
            Get
                Return _ptr.UIntAtAbsolute(214)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(214) = value
            End Set
        End Property

        Public Property PanningHeight As UInteger
            Get
                Return _ptr.UIntAtAbsolute(218)
            End Get
            Set(value As UInteger)
                _ptr.UIntAtAbsolute(218) = value
            End Set
        End Property

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                End If

                If _own Then _ptr.Free()

            End If
            Me.disposedValue = True
        End Sub

        Protected Overrides Sub Finalize()
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(False)
            MyBase.Finalize()
        End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

        Public Overrides Function Equals(obj As Object) As Boolean
            If Not TypeOf obj Is DeviceMode Then Return False
            Return Equals(CType(obj, DeviceMode))
        End Function

        Public Overloads Function Equals(other As DeviceMode) As Boolean Implements IEquatable(Of DeviceMode).Equals
            Dim pi() As PropertyInfo = Me.GetType.GetProperties(BindingFlags.Public Or BindingFlags.Instance)

            Dim o1 As Object,
                o2 As Object

            For Each pe In pi

                o1 = pe.GetValue(Me)
                o2 = pe.GetValue(other)

                If o1.Equals(o2) = False Then Return False
            Next

            Return True

        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me.MemberwiseClone
        End Function
    End Class

#End Region

End Namespace