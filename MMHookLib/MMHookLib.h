// The following ifdef block is the standard way of creating macros which make exporting
// from a DLL simpler. All files within this DLL are compiled with the MMHOOKLIB_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see
// MMHOOKLIB_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef MMHOOKLIB_EXPORTS
#define MMHOOKLIB_API __declspec(dllexport)
#else
#define MMHOOKLIB_API __declspec(dllimport)
#endif

// This class is exported from the dll
class MMHOOKLIB_API CMMHookLib {
public:
	CMMHookLib(void);
	// TODO: add your methods here.
};

extern MMHOOKLIB_API int nMMHookLib;

MMHOOKLIB_API int fnMMHookLib(void);
