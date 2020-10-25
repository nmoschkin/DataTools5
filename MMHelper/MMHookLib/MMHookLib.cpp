
// Credit goes to Chris Wilson for pretty much all of this.
// I fixed it to compile for modern times.
// - Nathan Moschkin (2020)

#include "pch.h"
#include <windows.h>
#include "MMHookLib.h"

HHOOK hookCbt = NULL;
HHOOK hookShell = NULL;
HHOOK hookKeyboard = NULL;
HHOOK hookMouse = NULL;
HHOOK hookKeyboardLL = NULL;
HHOOK hookMouseLL = NULL;
HHOOK hookCallWndProc = NULL;
HHOOK hookGetMsg = NULL;

#ifdef _M_AMD64
	
#define LIB_NAME L"MMHookLib64.dll"
#define HOOK_PREFIX L"NNM_HOOK_64_"

#else

#define LIB_NAME L"MMHookLib.dll"

#define HOOK_PREFIX L"NNM_HOOK_"

#endif

//
// Store the application instance of this module to pass to
// hook initialization. This is set in DLLMain().
//

#define DllExport extern "C"  __declspec( dllexport )

HINSTANCE g_appInstance = NULL;
static RECT CBT_SIZE_RECT;

typedef void (CALLBACK* HookProc)(int code, WPARAM w, LPARAM l);

static LRESULT CALLBACK CbtHookCallback(int code, WPARAM wparam, LPARAM lparam);
static LRESULT CALLBACK ShellHookCallback(int code, WPARAM wparam, LPARAM lparam);
static LRESULT CALLBACK KeyboardHookCallback(int code, WPARAM wparam, LPARAM lparam);
static LRESULT CALLBACK MouseHookCallback(int code, WPARAM wparam, LPARAM lparam);
static LRESULT CALLBACK KeyboardLLHookCallback(int code, WPARAM wparam, LPARAM lparam);
static LRESULT CALLBACK MouseLLHookCallback(int code, WPARAM wparam, LPARAM lparam);
static LRESULT CALLBACK CallWndProcHookCallback(int code, WPARAM wparam, LPARAM lparam);
static LRESULT CALLBACK GetMsgHookCallback(int code, WPARAM wparam, LPARAM lparam);

DllExport bool InitializeCbtHook(int threadID, HWND destination)
{
	if (g_appInstance == NULL)
	{
		g_appInstance = GetModuleHandle(LIB_NAME);
	}

	if (GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_CBT") != NULL)
	{
		SendNotifyMessage((HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_CBT"), RegisterWindowMessage(HOOK_PREFIX L"CBT_REPLACED"), 0, 0);
	}

	SetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_CBT", destination);

	hookCbt = SetWindowsHookEx(WH_CBT, (HOOKPROC)CbtHookCallback, g_appInstance, threadID);
	return hookCbt != NULL;
}

DllExport void UninitializeCbtHook()
{
	if (hookCbt != NULL)
		UnhookWindowsHookEx(hookCbt);
	hookCbt = NULL;
}

static LRESULT CALLBACK CbtHookCallback(int code, WPARAM wparam, LPARAM lparam)
{
	if (code >= 0)
	{
		UINT msg = 0;

		if (code == HCBT_ACTIVATE)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HCBT_ACTIVATE");
		else if (code == HCBT_CREATEWND)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HCBT_CREATEWND");
		else if (code == HCBT_DESTROYWND)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HCBT_DESTROYWND");
		else if (code == HCBT_MINMAX)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HCBT_MINMAX");
		else if (code == HCBT_MOVESIZE) {
			CBT_SIZE_RECT = *((RECT*)lparam);
			lparam = (LPARAM)&CBT_SIZE_RECT;
			msg = RegisterWindowMessage(HOOK_PREFIX L"HCBT_MOVESIZE");
		}
		else if (code == HCBT_SETFOCUS)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HCBT_SETFOCUS");
		else if (code == HCBT_SYSCOMMAND)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HCBT_SYSCOMMAND");

		HWND dstWnd = (HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_CBT");

		if (msg != 0)
			SendNotifyMessage(dstWnd, msg, wparam, lparam);
	}

	return CallNextHookEx(hookCbt, code, wparam, lparam);
}

DllExport bool InitializeShellHook(int threadID, HWND destination)
{
	if (g_appInstance == NULL)
	{
		g_appInstance = GetModuleHandle(LIB_NAME);
	}

	if (GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_SHELL") != NULL)
	{
		SendNotifyMessage((HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_SHELL"), RegisterWindowMessage(HOOK_PREFIX L"SHELL_REPLACED"), 0, 0);
	}

	SetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_SHELL", destination);

	hookShell = SetWindowsHookEx(WH_SHELL, (HOOKPROC)ShellHookCallback, g_appInstance, threadID);
	return hookShell != NULL;
}

DllExport void UninitializeShellHook()
{
	if (hookShell != NULL)
		UnhookWindowsHookEx(hookShell);
	hookShell = NULL;
}

static LRESULT CALLBACK ShellHookCallback(int code, WPARAM wparam, LPARAM lparam)
{
	if (code >= 0)
	{
		UINT msg = 0;

		if (code == HSHELL_ACTIVATESHELLWINDOW)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HSHELL_ACTIVATESHELLWINDOW");
		else if (code == HSHELL_GETMINRECT)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HSHELL_GETMINRECT");
		else if (code == HSHELL_LANGUAGE)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HSHELL_LANGUAGE");
		else if (code == HSHELL_REDRAW)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HSHELL_REDRAW");
		else if (code == HSHELL_TASKMAN)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HSHELL_TASKMAN");
		else if (code == HSHELL_WINDOWACTIVATED)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HSHELL_WINDOWACTIVATED");
		else if (code == HSHELL_WINDOWCREATED)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HSHELL_WINDOWCREATED");
		else if (code == HSHELL_WINDOWDESTROYED)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HSHELL_WINDOWDESTROYED");
		else if (code == HSHELL_WINDOWREPLACED)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HSHELL_WINDOWREPLACED");
		else if (code == HSHELL_WINDOWREPLACING)
			msg = RegisterWindowMessage(HOOK_PREFIX L"HSHELL_WINDOWREPLACING");

		HWND dstWnd = (HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_SHELL");

		if (msg != 0)
			SendNotifyMessage(dstWnd, msg, wparam, lparam);
	}

	return CallNextHookEx(hookShell, code, wparam, lparam);
}

DllExport bool InitializeKeyboardHook(int threadID, HWND destination)
{
	if (g_appInstance == NULL)
	{
		g_appInstance = GetModuleHandle(LIB_NAME);
	}

	if (GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_KEYBOARD") != NULL)
	{
		SendNotifyMessage((HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_KEYBOARD"), RegisterWindowMessage(HOOK_PREFIX L"KEYBOARD_REPLACED"), 0, 0);
	}

	SetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_KEYBOARD", destination);

	hookKeyboard = SetWindowsHookEx(WH_KEYBOARD, (HOOKPROC)KeyboardHookCallback, g_appInstance, threadID);
	return hookKeyboard != NULL;
}

DllExport void UninitializeKeyboardHook()
{
	if (hookKeyboard != NULL)
		UnhookWindowsHookEx(hookKeyboard);
	hookKeyboard = NULL;
}

static LRESULT CALLBACK KeyboardHookCallback(int code, WPARAM wparam, LPARAM lparam)
{
	if (code >= 0)
	{
		UINT msg = 0;

		msg = RegisterWindowMessage(HOOK_PREFIX L"KEYBOARD");

		HWND dstWnd = (HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_KEYBOARD");

		if (msg != 0)
			SendNotifyMessage(dstWnd, msg, wparam, lparam);
	}

	return CallNextHookEx(hookKeyboard, code, wparam, lparam);
}

DllExport bool InitializeMouseHook(int threadID, HWND destination)
{
	if (g_appInstance == NULL)
	{
		g_appInstance = GetModuleHandle(LIB_NAME);
	}

	if (GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_MOUSE") != NULL)
	{
		SendNotifyMessage((HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_MOUSE"), RegisterWindowMessage(HOOK_PREFIX L"MOUSE_REPLACED"), 0, 0);
	}

	SetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_MOUSE", destination);

	hookMouse = SetWindowsHookEx(WH_MOUSE, (HOOKPROC)MouseHookCallback, g_appInstance, threadID);
	return hookMouse != NULL;
}

DllExport void UninitializeMouseHook()
{
	if (hookMouse != NULL)
		UnhookWindowsHookEx(hookMouse);
	hookMouse = NULL;
}

static LRESULT CALLBACK MouseHookCallback(int code, WPARAM wparam, LPARAM lparam)
{
	if (code >= 0)
	{
		UINT msg = 0;

		msg = RegisterWindowMessage(HOOK_PREFIX L"MOUSE");

		HWND dstWnd = (HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_MOUSE");

		if (msg != 0)
			SendNotifyMessage(dstWnd, msg, wparam, lparam);
	}

	return CallNextHookEx(hookMouse, code, wparam, lparam);
}

DllExport bool InitializeKeyboardLLHook(int threadID, HWND destination)
{
	if (g_appInstance == NULL)
	{
		g_appInstance = GetModuleHandle(LIB_NAME);
	}

	if (GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_KEYBOARDLL") != NULL)
	{
		SendNotifyMessage((HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_KEYBOARDLL"), RegisterWindowMessage(HOOK_PREFIX L"KEYBOARDLL_REPLACED"), 0, 0);
	}

	SetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_KEYBOARDLL", destination);

	hookKeyboardLL = SetWindowsHookEx(WH_KEYBOARD_LL, (HOOKPROC)KeyboardLLHookCallback, g_appInstance, threadID);
	return hookKeyboardLL != NULL;
}

DllExport void UninitializeKeyboardLLHook()
{
	if (hookKeyboardLL != NULL)
		UnhookWindowsHookEx(hookKeyboardLL);
	hookKeyboardLL = NULL;
}

static LRESULT CALLBACK KeyboardLLHookCallback(int code, WPARAM wparam, LPARAM lparam)
{
	if (code >= 0)
	{
		UINT msg = 0;

		msg = RegisterWindowMessage(HOOK_PREFIX L"KEYBOARDLL");

		HWND dstWnd = (HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_KEYBOARDLL");

		if (msg != 0)
			SendNotifyMessage(dstWnd, msg, wparam, lparam);
	}

	return CallNextHookEx(hookKeyboardLL, code, wparam, lparam);
}

DllExport bool InitializeMouseLLHook(int threadID, HWND destination)
{
	if (g_appInstance == NULL)
	{
		g_appInstance = GetModuleHandle(LIB_NAME);
	}

	if (GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_MOUSELL") != NULL)
	{
		SendNotifyMessage((HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_MOUSELL"), RegisterWindowMessage(HOOK_PREFIX L"MOUSELL_REPLACED"), 0, 0);
	}

	SetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_MOUSELL", destination);

	hookMouseLL = SetWindowsHookEx(WH_MOUSE_LL, (HOOKPROC)MouseLLHookCallback, g_appInstance, threadID);
	return hookMouseLL != NULL;
}

DllExport void UninitializeMouseLLHook()
{
	if (hookMouseLL != NULL)
		UnhookWindowsHookEx(hookMouseLL);
	hookMouseLL = NULL;
}

static LRESULT CALLBACK MouseLLHookCallback(int code, WPARAM wparam, LPARAM lparam)
{
	if (code >= 0)
	{
		UINT msg = 0;

		msg = RegisterWindowMessage(HOOK_PREFIX L"MOUSELL");

		HWND dstWnd = (HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_MOUSELL");

		if (msg != 0)
			SendNotifyMessage(dstWnd, msg, wparam, lparam);
	}

	return CallNextHookEx(hookMouseLL, code, wparam, lparam);
}

DllExport bool InitializeCallWndProcHook(int threadID, HWND destination)
{
	if (g_appInstance == NULL)
	{
		g_appInstance = GetModuleHandle(LIB_NAME);
	}

	if (GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_CALLWNDPROC") != NULL)
	{
		SendNotifyMessage((HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_CALLWNDPROC"), RegisterWindowMessage(HOOK_PREFIX L"CALLWNDPROC_REPLACED"), 0, 0);
	}

	SetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_CALLWNDPROC", destination);

	hookCallWndProc = SetWindowsHookEx(WH_CALLWNDPROC, (HOOKPROC)CallWndProcHookCallback, g_appInstance, threadID);
	return hookCallWndProc != NULL;
}

DllExport void UninitializeCallWndProcHook()
{
	if (hookCallWndProc != NULL)
		UnhookWindowsHookEx(hookCallWndProc);
	hookCallWndProc = NULL;
}

static LRESULT CALLBACK CallWndProcHookCallback(int code, WPARAM wparam, LPARAM lparam)
{
	if (code >= 0)
	{
		UINT msg = 0;
		UINT msg2 = 0;

		msg = RegisterWindowMessage(HOOK_PREFIX L"CALLWNDPROC");
		msg2 = RegisterWindowMessage(HOOK_PREFIX L"CALLWNDPROC_PARAMS");

		HWND dstWnd = (HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_CALLWNDPROC");

		CWPSTRUCT* pCwpStruct = (CWPSTRUCT*)lparam;

		if (msg != 0 && pCwpStruct->message != msg && pCwpStruct->message != msg2)
		{
			SendNotifyMessage(dstWnd, msg, (WPARAM)pCwpStruct->hwnd, pCwpStruct->message);
			SendNotifyMessage(dstWnd, msg2, pCwpStruct->wParam, pCwpStruct->lParam);
		}
	}

	return CallNextHookEx(hookCallWndProc, code, wparam, lparam);
}

DllExport bool InitializeGetMsgHook(int threadID, HWND destination)
{
	if (g_appInstance == NULL)
	{
		g_appInstance = GetModuleHandle(LIB_NAME);
	}

	if (GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_GETMSG") != NULL)
	{
		SendNotifyMessage((HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_GETMSG"), RegisterWindowMessage(HOOK_PREFIX L"GETMSG_REPLACED"), 0, 0);
	}

	SetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_GETMSG", destination);

	hookGetMsg = SetWindowsHookEx(WH_GETMESSAGE, (HOOKPROC)GetMsgHookCallback, g_appInstance, threadID);
	return hookGetMsg != NULL;
}

DllExport void UninitializeGetMsgHook()
{
	if (hookGetMsg != NULL)
		UnhookWindowsHookEx(hookGetMsg);
	hookGetMsg = NULL;
}

static LRESULT CALLBACK GetMsgHookCallback(int code, WPARAM wparam, LPARAM lparam)
{
	if (code >= 0)
	{
		UINT msg = 0;
		UINT msg2 = 0;

		msg = RegisterWindowMessage(HOOK_PREFIX L"GETMSG");
		msg2 = RegisterWindowMessage(HOOK_PREFIX L"GETMSG_PARAMS");

		HWND dstWnd = (HWND)GetProp(GetDesktopWindow(), HOOK_PREFIX L"HWND_GETMSG");

		MSG* pMsg = (MSG*)lparam;

		if (msg != 0 && pMsg->message != msg && pMsg->message != msg2)
		{
			SendNotifyMessage(dstWnd, msg, (WPARAM)pMsg->hwnd, pMsg->message);
			SendNotifyMessage(dstWnd, msg2, pMsg->wParam, pMsg->lParam);
		}
	}

	return CallNextHookEx(hookGetMsg, code, wparam, lparam);
}
