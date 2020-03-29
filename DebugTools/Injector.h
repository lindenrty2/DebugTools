

#pragma once
#include < stdio.h >
#include < stdlib.h >
#include < vcclr.h >
#include "TCHAR.h"
#include "Windows.h"
#include "Tlhelp32.h"

namespace DebugTools {

System::Void Attach(System::Diagnostics::Process^ process);

void Launch( System::IntPtr windowHandle, System::String^ launcherName, System::String^ assemblyName, System::String^ className, System::String^ methodName);

BOOL UnInjectDll(const TCHAR* ptszDllFile,DWORD dwProcessId) ;

}