#pragma once

namespace DebugTools {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	/// <summary>
	/// AppMainSelectFrm �̊T�v
	/// </summary>
	public ref class AppMainSelectFrm : public System::Windows::Forms::Form
	{
	public:
		AppMainSelectFrm(void)
		{
			InitializeComponent();
			//
			//TODO: �����ɃR���X�g���N�^�[ �R�[�h��ǉ����܂�
			//
		}

	protected:
		/// <summary>
		/// �g�p���̃��\�[�X�����ׂăN���[���A�b�v���܂��B
		/// </summary>
		~AppMainSelectFrm()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::TextBox^  textBox1;
	protected: 
	private: System::Windows::Forms::Button^  btnSelect;
	private: System::Windows::Forms::Button^  btnOK;
	private: System::Windows::Forms::Button^  button2; 

	private:
		/// <summary>
		/// �K�v�ȃf�U�C�i�[�ϐ��ł��B
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// �f�U�C�i�[ �T�|�[�g�ɕK�v�ȃ��\�b�h�ł��B���̃��\�b�h�̓��e��
		/// �R�[�h �G�f�B�^�[�ŕύX���Ȃ��ł��������B
		/// </summary>
		void InitializeComponent(void)
		{
			this->textBox1 = (gcnew System::Windows::Forms::TextBox());
			this->btnSelect = (gcnew System::Windows::Forms::Button());
			this->btnOK = (gcnew System::Windows::Forms::Button());
			this->button2 = (gcnew System::Windows::Forms::Button()); 
			this->SuspendLayout();
			// 
			// textBox1
			// 
			this->textBox1->Location = System::Drawing::Point(12, 12);
			this->textBox1->Name = L"textBox1";
			this->textBox1->Size = System::Drawing::Size(355, 19);
			this->textBox1->TabIndex = 0;
			// 
			// btnSelect
			// 
			this->btnSelect->Location = System::Drawing::Point(373, 12);
			this->btnSelect->Name = L"btnSelect";
			this->btnSelect->Size = System::Drawing::Size(61, 19);
			this->btnSelect->TabIndex = 1;
			this->btnSelect->Text = L"�I��";
			this->btnSelect->UseVisualStyleBackColor = true;
			this->btnSelect->Click += gcnew System::EventHandler(this, &AppMainSelectFrm::btnSelect_Click);
			// 
			// btnOK
			// 
			this->btnOK->Location = System::Drawing::Point(359, 50);
			this->btnOK->Name = L"btnOK";
			this->btnOK->Size = System::Drawing::Size(75, 23);
			this->btnOK->TabIndex = 2;
			this->btnOK->Text = L"�m��";
			this->btnOK->UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this->button2->Location = System::Drawing::Point(12, 50);
			this->button2->Name = L"button2";
			this->button2->Size = System::Drawing::Size(75, 23);
			this->button2->TabIndex = 3;
			this->button2->Text = L"���";
			this->button2->UseVisualStyleBackColor = true;
			// 
			// AppMainSelectFrm
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 12);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(448, 94);
			this->Controls->Add(this->button2);
			this->Controls->Add(this->btnOK);
			this->Controls->Add(this->btnSelect);
			this->Controls->Add(this->textBox1);
			this->Name = L"AppMainSelectFrm";
			this->Text = L"AppMainSelectFrm";
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion
	private: System::Void btnSelect_Click(System::Object^  sender, System::EventArgs^  e) {
				 FolderBrowserDialog^  folderBrowserDialog1 = gcnew FolderBrowserDialog();
				 folderBrowserDialog1->Show();
			 }
	};
}
