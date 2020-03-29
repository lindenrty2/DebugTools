#pragma once
#include "injector.h"


namespace DebugTools {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	using namespace System::Diagnostics;

	public ref class TargetSelectFrm : public System::Windows::Forms::Form
	{
	public:
		TargetSelectFrm(void)
		{
			InitializeComponent();
		}

	protected:
		~TargetSelectFrm()
		{
			if (components)
			{
				delete components;
			}
		}

	private: System::Windows::Forms::Button^  btnRef;
	private: System::Windows::Forms::Button^  btnAttach;
	private: System::Windows::Forms::TextBox^  txtFilter;


	private: System::Windows::Forms::ListView^  lvProcess;
	private: System::Windows::Forms::ColumnHeader^  id;
	private: System::Windows::Forms::ColumnHeader^  title;
	private: System::Windows::Forms::Label^  label1;
	private: System::Windows::Forms::ColumnHeader^  mainWindowTitle;







	protected: 



	private:
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code

		void InitializeComponent(void)
		{
			this->btnRef = (gcnew System::Windows::Forms::Button());
			this->btnAttach = (gcnew System::Windows::Forms::Button());
			this->txtFilter = (gcnew System::Windows::Forms::TextBox());
			this->lvProcess = (gcnew System::Windows::Forms::ListView());
			this->id = (gcnew System::Windows::Forms::ColumnHeader());
			this->title = (gcnew System::Windows::Forms::ColumnHeader());
			this->mainWindowTitle = (gcnew System::Windows::Forms::ColumnHeader());
			this->label1 = (gcnew System::Windows::Forms::Label());
			this->SuspendLayout();
			// 
			// btnRef
			// 
			this->btnRef->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Right));
			this->btnRef->Location = System::Drawing::Point(656, 15);
			this->btnRef->Name = L"btnRef";
			this->btnRef->Size = System::Drawing::Size(103, 32);
			this->btnRef->TabIndex = 1;
			this->btnRef->Text = L"刷新";
			this->btnRef->UseVisualStyleBackColor = true;
			this->btnRef->Click += gcnew System::EventHandler(this, &TargetSelectFrm::btnRef_Click);
			// 
			// btnAttach
			// 
			this->btnAttach->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Right));
			this->btnAttach->Location = System::Drawing::Point(656, 53);
			this->btnAttach->Name = L"btnAttach";
			this->btnAttach->Size = System::Drawing::Size(103, 31);
			this->btnAttach->TabIndex = 2;
			this->btnAttach->Text = L"附加到进程";
			this->btnAttach->UseVisualStyleBackColor = true;
			this->btnAttach->Click += gcnew System::EventHandler(this, &TargetSelectFrm::btnAttach_Click);
			// 
			// txtFilter
			// 
			this->txtFilter->Anchor = static_cast<System::Windows::Forms::AnchorStyles>(((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Left)
				| System::Windows::Forms::AnchorStyles::Right));
			this->txtFilter->Location = System::Drawing::Point(12, 16);
			this->txtFilter->Name = L"txtFilter";
			this->txtFilter->Size = System::Drawing::Size(635, 21);
			this->txtFilter->TabIndex = 4;
			this->txtFilter->TextChanged += gcnew System::EventHandler(this, &TargetSelectFrm::txtFilter_TextChanged);
			// 
			// lvProcess
			// 
			this->lvProcess->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((((System::Windows::Forms::AnchorStyles::Top | System::Windows::Forms::AnchorStyles::Bottom)
				| System::Windows::Forms::AnchorStyles::Left)
				| System::Windows::Forms::AnchorStyles::Right));
			this->lvProcess->Columns->AddRange(gcnew cli::array< System::Windows::Forms::ColumnHeader^  >(3) {
				this->id, this->title,
					this->mainWindowTitle
			});
			this->lvProcess->FullRowSelect = true;
			this->lvProcess->Location = System::Drawing::Point(12, 41);
			this->lvProcess->Name = L"lvProcess";
			this->lvProcess->Size = System::Drawing::Size(635, 292);
			this->lvProcess->TabIndex = 5;
			this->lvProcess->UseCompatibleStateImageBehavior = false;
			this->lvProcess->View = System::Windows::Forms::View::Details;
			// 
			// id
			// 
			this->id->Text = L"ID";
			// 
			// title
			// 
			this->title->Text = L"标题";
			this->title->Width = 265;
			// 
			// mainWindowTitle
			// 
			this->mainWindowTitle->Text = L"AAAA";
			this->mainWindowTitle->Width = 305;
			// 
			// label1
			// 
			this->label1->Anchor = static_cast<System::Windows::Forms::AnchorStyles>((System::Windows::Forms::AnchorStyles::Bottom | System::Windows::Forms::AnchorStyles::Right));
			this->label1->AutoSize = true;
			this->label1->Font = (gcnew System::Drawing::Font(L"MS UI Gothic", 18, System::Drawing::FontStyle::Regular, System::Drawing::GraphicsUnit::Point,
				static_cast<System::Byte>(128)));
			this->label1->ForeColor = System::Drawing::SystemColors::HotTrack;
			this->label1->Location = System::Drawing::Point(675, 309);
			this->label1->Name = L"label1";
			this->label1->Size = System::Drawing::Size(39, 24);
			this->label1->TabIndex = 6;
			this->label1->Text = L"3.0";
			// 
			// TargetSelectFrm
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 12);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(769, 345);
			this->Controls->Add(this->label1);
			this->Controls->Add(this->lvProcess);
			this->Controls->Add(this->txtFilter);
			this->Controls->Add(this->btnAttach);
			this->Controls->Add(this->btnRef);
			this->Name = L"TargetSelectFrm";
			this->Text = L"DebugTools- 3.0";
			this->Load += gcnew System::EventHandler(this, &TargetSelectFrm::Form1_Load);
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion

	private : 
		array<Process^> ^processes;
	    String ^filter;

		System::Void Form1_Load(System::Object^  sender, System::EventArgs^  e) {
			Ref();
		}

		System::Void btnRef_Click(System::Object^  sender, System::EventArgs^  e) {
			Ref();
		}

		System::Void Ref() { 
			processes = Process::GetProcesses(); 
			ReDrawList();
		}

		System::Void ReDrawList() {
				lvProcess->Items->Clear(); 
				for(int i=0;i<processes->Length;i++){
					Process^ process = processes[i];
					if(!String::IsNullOrWhiteSpace(filter) && process->ProcessName->ToUpper()->IndexOf(filter->ToUpper()) == -1){
						continue;
					}
					if(!CanAttachProcess(process)){
						continue;
					}
					ListViewItem^ viewItem = gcnew ListViewItem();
					viewItem->Text = process->Id.ToString();
					viewItem->SubItems->Add(process->ProcessName);
					viewItem->SubItems->Add(process->MainWindowTitle);
					this->lvProcess->Items->Add(viewItem);
				}

			 }

		bool CanAttachProcess(Process^ process)
        {
			try{
				bool result = false;
				if (System::Environment::Is64BitOperatingSystem )
				{
					result= true;
				}
				for each (ProcessModule^ module in process->Modules)
				{
					System::Console::WriteLine(module->ModuleName);
					if (String::Compare(module->ModuleName, "InjectModule.dll", true) == 0)
						return false;
					if (String::Compare(module->ModuleName, "mscoree.dll", true) == 0)
						result = true;
				}
				return result;
			}
			catch(System::Exception^ ex)
			{
				System::Console::WriteLine(ex->ToString());
				return false;
			}
        } 

		System::Void btnAttach_Click(System::Object^  sender, System::EventArgs^  e) {
			if(lvProcess->SelectedItems->Count == 0) return;
			String^ processid = lvProcess->SelectedItems[0]->Text;
			Process^ process = Process::GetProcessById(int::Parse( processid));
			Attach(process);
			this->Close();
		} 

		System::Void btnDettach_Click(System::Object^  sender, System::EventArgs^  e) {
	/*		 String^ processid = lvProcess->SelectedItems[0]->Text;
			 UnInjectDll(L"InjectModule.dll",(DWORD)int::Parse( processid));
			 UnInjectDll(L"DebugModule.dll",(DWORD)int::Parse( processid));*/
		 }

		System::Void txtFilter_TextChanged(System::Object^  sender, System::EventArgs^  e) {
			 filter = txtFilter->Text;
			 ReDrawList();
		 }
		 

};

}

