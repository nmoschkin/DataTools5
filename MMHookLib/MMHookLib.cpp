// MMHookLib.cpp : Defines the exported functions for the DLL.
//

#include "pch.h"
#include "framework.h"
#include "MMHookLib.h"


// This is an example of an exported variable
MMHOOKLIB_API int nMMHookLib=0;

// This is an example of an exported function.
MMHOOKLIB_API int fnMMHookLib(void)
{
    return 0;
}

// This is the constructor of a class that has been exported.
CMMHookLib::CMMHookLib()
{
    return;
}
