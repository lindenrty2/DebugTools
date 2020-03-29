// DebugTools.cpp : メイン プロジェクト ファイルです。

#include "stdafx.h"
#include "TargetSelectFrm.h"
#include "Injector.h"

using namespace DebugTools;

[STAThreadAttribute]
int main(array<System::String ^> ^args)
{
 
	// コントロールが作成される前に、Windows XP ビジュアル効果を有効にします
	Application::EnableVisualStyles();
	Application::SetCompatibleTextRenderingDefault(false); 

	if(args->Length > 0){
		if (args[0] = "-p"){
			System::Int32 processId = Int32::Parse(args[1]);
			System::Diagnostics::Process^ process = System::Diagnostics::Process::GetProcessById(processId);
			DebugTools::Attach(process);
			return 0;
		}
		else
		{
			System::IO::Directory::SetCurrentDirectory(System::IO::Path::GetDirectoryName(args[0]));
			System::String ^ parameter ;
			for (int i =1;i<args->Length;i++ ){
				parameter += args[i] + " ";
			}
			//System::Windows::Forms::MessageBox::Show(parameter);
			Process^ process = System::Diagnostics::Process::Start(args[0],parameter);
			while(true){
				if(process->MainWindowHandle.ToInt32() != 0 || process->HasExited ) break;
				System::Threading::Thread::Sleep(100);
			}
			Launch(process->MainWindowHandle,
				System::Reflection::Assembly::GetCallingAssembly()->Location,
				System::IO::Path::Combine(System::IO::Path::GetDirectoryName(System::Reflection::Assembly::GetCallingAssembly()->Location),"DebugModule.dll"),
						"DebugModule.Core.MainModule","AttachedMain"); 
		
			return 0;
		}
	}

	// メイン ウィンドウを作成して、実行します
	Application::Run(gcnew TargetSelectFrm());
	return 0;
}
