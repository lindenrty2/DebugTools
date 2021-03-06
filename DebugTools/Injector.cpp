#include "stdafx.h"
#include "Injector.h"


namespace DebugTools {
	//defines a new window message that is guaranteed to be unique throughout the system. 
	//The message value can be used when sending or posting messages.
	unsigned int WM_MAROOON = ::RegisterWindowMessage(L"Injector_Marooon");

	static HHOOK _messageHookHandle;

	System::Void Attach(System::Diagnostics::Process^ process) {
		System::IntPtr handle = process->MainWindowHandle;
		Launch(handle,
			System::Reflection::Assembly::GetEntryAssembly()->Location,
			System::IO::Path::Combine( System::Environment::CurrentDirectory,"DebugModule.dll"),
			"DebugModule.Core.MainModule","AttachedMain"); 

	}

	//-----------------------------------------------------------------------------
	//Spying Process functions follow
	//-----------------------------------------------------------------------------
	void Launch(System::IntPtr windowHandle, System::String^ launcherName, System::String^ assembly, System::String^ className, System::String^ methodName) {

		System::String^ assemblyClassAndMethod = launcherName + "$" + assembly + "$" + className + "$" + methodName;
		//convert String to local wchar_t* or char*
		pin_ptr<const wchar_t> acmLocal = PtrToStringChars(assemblyClassAndMethod);

		//Maps the specified executable module into the address space of the calling process.
		HINSTANCE hinstDLL = ::LoadLibrary((LPCTSTR) (L"InjectModule.dll")); 

		if (hinstDLL)
		{
			DWORD processID = 0;
			//get the process id and thread id
			DWORD threadID = ::GetWindowThreadProcessId((HWND)windowHandle.ToPointer(), &processID);

			if (processID)
			{
				//get the target process object (handle) 
				HANDLE hProcess = ::OpenProcess(PROCESS_ALL_ACCESS, FALSE, processID);
				if (hProcess)
				{
					int buffLen = (assemblyClassAndMethod->Length + 1) * sizeof(wchar_t);
					//Allocates physical storage in memory or in the paging file on disk for the specified reserved memory pages. 
					//The function initializes the memory to zero. 
					//The return value is the base address of the allocated region of pages.
					void* acmRemote = ::VirtualAllocEx(hProcess, NULL, buffLen, MEM_COMMIT, PAGE_READWRITE);

					if (acmRemote)
					{
						//copies the data(the assemblyClassAndMethod string) 
						//from the specified buffer in the current process 
						//to the address range of the target process
						::WriteProcessMemory(hProcess, acmRemote, acmLocal, buffLen, NULL);
					
						//Retrieves the address of MessageHookProc method from the hintsDLL 
						HOOKPROC procAddress = (HOOKPROC)GetProcAddress(hinstDLL, "MessageHookProc");

						//install a hook procedure to the target thread(before the system sends the messages to the destination window procedure)
						_messageHookHandle = ::SetWindowsHookEx(WH_CALLWNDPROC, procAddress, hinstDLL, threadID);

						if (_messageHookHandle)
						{
							//send our custom message to the target window of the target process
							::SendMessage((HWND)windowHandle.ToPointer(), WM_MAROOON, (WPARAM)acmRemote, 0);
							//removes the hook procedure installed in a hook chain by the SetWindowsHookEx function. 
							::UnhookWindowsHookEx(_messageHookHandle);
						}

						//removes a hook procedure installed in a hook chain by the SetWindowsHookEx function. 
						::VirtualFreeEx(hProcess, acmRemote, buffLen, MEM_RELEASE);
					}

					::CloseHandle(hProcess);
				}
			}
			//Decrements the reference count of the loaded DLL
			::FreeLibrary(hinstDLL);
		}
	}

BOOL UnInjectDll(const TCHAR* ptszDllFile, DWORD dwProcessId)  
{  
    // 参数无效  
    if (NULL == ptszDllFile || 0 == ::_tcslen(ptszDllFile))  
    {  
        return false;  
    }  
    HANDLE hModuleSnap = INVALID_HANDLE_VALUE;  
    HANDLE hProcess = NULL;  
    HANDLE hThread = NULL;  
    // 获取模块快照  
    hModuleSnap = ::CreateToolhelp32Snapshot(TH32CS_SNAPMODULE, dwProcessId);  
    if (INVALID_HANDLE_VALUE == hModuleSnap)  
    {  
        return false;  
    }  
    MODULEENTRY32 me32;  
    memset(&me32, 0, sizeof(MODULEENTRY32));  
    me32.dwSize = sizeof(MODULEENTRY32);  
    // 开始遍历  
    if(FALSE == ::Module32First(hModuleSnap, &me32))  
    {  
        ::CloseHandle(hModuleSnap);  
        return false;  
    }  
    // 遍历查找指定模块  
    bool isFound = false;  
    do  
    {  
        //isFound = (0 == ::_tcsicmp(me32.szModule, ptszDllFile) || 0 == ::_tcsicmp(me32.szExePath, ptszDllFile));  
		isFound = (0 == ::_tcsicmp(me32.szModule, ptszDllFile) || 0 == ::_tcsicmp(me32.szExePath, ptszDllFile));  
        if (isFound) // 找到指定模块  
        {  
            break;  
        }  
    } while (TRUE == ::Module32Next(hModuleSnap, &me32));  
    ::CloseHandle(hModuleSnap);  
    if (false == isFound)  
    {  
        return false;  
    }  
    // 获取目标进程句柄  
    hProcess = ::OpenProcess(PROCESS_CREATE_THREAD | PROCESS_VM_OPERATION, FALSE, dwProcessId);  
    if (NULL == hProcess)  
    {  
        return false;  
    }  
    // 从 Kernel32.dll 中获取 FreeLibrary 函数地址  
    LPTHREAD_START_ROUTINE lpThreadFun = (PTHREAD_START_ROUTINE)::GetProcAddress(::GetModuleHandle(_T("Kernel32")), "FreeLibrary");  
    if (NULL == lpThreadFun)  
    {  
        ::CloseHandle(hProcess);  
        return false;  
    }  
    // 创建远程线程调用 FreeLibrary  
    hThread = ::CreateRemoteThread(hProcess, NULL, 0, lpThreadFun, me32.modBaseAddr /* 模块地址 */, 0, NULL);  
    if (NULL == hThread)  
    {  
        ::CloseHandle(hProcess);  
        return false;  
    }  
    // 等待远程线程结束  
    ::WaitForSingleObject(hThread, INFINITE);  
    // 清理  
    ::CloseHandle(hThread);  
    ::CloseHandle(hProcess);  
    return true;  
}  

}
