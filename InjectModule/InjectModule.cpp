
#include "stdafx.h"

#include "InjectModule.h"
#include < vcclr.h >

#pragma unmanaged

BOOL WINAPI DllMain( HINSTANCE hinstDLL, DWORD fdwReason, LPVOID lpvReserved ) 
{
	return true;
}

#pragma managed


typedef int (WINAPI *OriCreateProcess)(   
                       LPCTSTR lpApplicationName,   
                       LPTSTR lpCommandLine,   
                       LPSECURITY_ATTRIBUTES lpProcessAttributes,   
                       LPSECURITY_ATTRIBUTES lpThreadAttributes,   
                       BOOL bInheritHandles,   
                       DWORD dwCreationFlags,   
                       LPVOID lpEnvironment,   
                       LPCTSTR lpCurrentDirectory,   
                       LPSTARTUPINFO lpStartupInfo,   
                       LPPROCESS_INFORMATION lpProcessInformation   
                       ) ;

char szOldMyCreateProcess[5] = {0}; 
char szJmpMyCreateProcess[5] = {(char)0xe9}; 
OriCreateProcess _createProcess = NULL; 
   
wchar_t* _launcherPath;

//Hook进程启动函数,用于在附加DebugTools的进程启动新进程时自动附加本工具
BOOL WINAPI MyCreateProcessW(   
                       LPCTSTR lpApplicationName,   
                       LPTSTR lpCommandLine,   
                       LPSECURITY_ATTRIBUTES lpProcessAttributes,   
                       LPSECURITY_ATTRIBUTES lpThreadAttributes,   
                       BOOL bInheritHandles,   
                       DWORD dwCreationFlags,   
                       LPVOID lpEnvironment,   
                       LPCTSTR lpCurrentDirectory,   
                       LPSTARTUPINFO lpStartupInfo,   
                       LPPROCESS_INFORMATION lpProcessInformation   
                       )   
{   
	TCHAR message[200];
	_stprintf_s(message,L"是否对%s适用DebugTools?",lpApplicationName);
	
	
    int iret = MessageBox(NULL,message,L"??",MB_OKCANCEL);   
	WriteProcessMemory((void*)-1, _createProcess, szOldMyCreateProcess, 5, NULL); 

    if(iret == IDOK)   
	{
		int size = wcslen(_launcherPath) + wcslen(lpCommandLine) + 2 ;
		wchar_t* target = new wchar_t[size];
		target[0] = '\0';
		wcscat_s( target,size,_launcherPath);
		wcscat_s( target,size,L" ");
		wcscat_s( target,size,lpCommandLine);

		::CreateProcessW(_launcherPath, target ,lpProcessAttributes,lpThreadAttributes,
			bInheritHandles,dwCreationFlags,lpEnvironment,lpCurrentDirectory,lpStartupInfo,lpProcessInformation) ;   
	} 
	else
	{
		::CreateProcessW(lpApplicationName, lpCommandLine ,lpProcessAttributes,lpThreadAttributes,
			bInheritHandles,dwCreationFlags,lpEnvironment,lpCurrentDirectory,lpStartupInfo,lpProcessInformation) ;  
	} 

	WriteProcessMemory((void*)-1, _createProcess, szJmpMyCreateProcess, 5, NULL); 
    return TRUE;   
   
} 

int HookCreateProcessW() 
{ 
	DWORD dwJmpAddr = 0; 
	HMODULE hModule = LoadLibrary(L"Kernel32.dll"); 
	_createProcess = (OriCreateProcess)GetProcAddress(hModule, "CreateProcessW"); 
	dwJmpAddr = (DWORD)MyCreateProcessW - (DWORD)_createProcess - 5; 
	memcpy(szJmpMyCreateProcess + 1, &dwJmpAddr, 4); 
	FreeLibrary(hModule); 
	ReadProcessMemory((void*)-1, _createProcess, szOldMyCreateProcess, 5, NULL); 
	WriteProcessMemory((void*)-1, _createProcess, szJmpMyCreateProcess, 5, NULL); 
	 
	return 0; 
}

static unsigned int WM_INJECTOR_MAROOON = ::RegisterWindowMessage(L"Injector_Marooon");

__declspec( dllexport )  
int __stdcall MessageHookProc(int nCode, WPARAM wparam, LPARAM lparam) {
	if (nCode == HC_ACTION)
	{
		CWPSTRUCT* msg = (CWPSTRUCT*)lparam; 
		if (msg != NULL && msg->message == WM_INJECTOR_MAROOON)
		{ 
			//HookCreateProcessW();
			wchar_t* acmRemote = (wchar_t*)msg->wParam; 
			System::String^ acmLocal = gcnew System::String(acmRemote);
			 
			//acmSplit[0]:the assembly's location 
			//acmSplit[1]:className; 
			//acmSplit[2]:methodName 
			cli::array<System::String^>^ acmSplit = acmLocal->Split('$'); 
			pin_ptr<const wchar_t> wch = PtrToStringChars(acmSplit[0]);
			int size = wcslen(wch);
			_launcherPath = new wchar_t[size];
			wmemcpy_s(_launcherPath,size,wch,size);
			_launcherPath[size] = '\0';
			System::Reflection::Assembly^ assembly = System::Reflection::Assembly::LoadFrom(acmSplit[1]);
			if (assembly != nullptr)
			{ 
				if(System::String::IsNullOrEmpty(acmSplit[2])){
					return CallNextHookEx(NULL, nCode, wparam, lparam);
				}
				System::Type^ type = assembly->GetType(acmSplit[2]); 
				if (type != nullptr)
				{ 
					System::Reflection::MethodInfo^ methodInfo = 
						type->GetMethod(acmSplit[3], System::Reflection::BindingFlags::Static | System::Reflection::BindingFlags::Public);
					if (methodInfo != nullptr)
					{ 
						methodInfo->Invoke(nullptr, nullptr);
					}
				}
			}
			
		}
	}
	//return CallNextHookEx(_messageHookHandle, nCode, wparam, lparam);
	return CallNextHookEx(NULL, nCode, wparam, lparam);
}


