﻿namespace ZetaResourceEditor.UI.Projects
{
	using System;
	using System.Diagnostics;
	using System.Windows.Forms;
	using DevExpress.XtraBars;
	using DevExpress.XtraEditors;
	using DevExpress.XtraEditors.Controls;
	using Helper.Base;
	using Properties;
	using RuntimeBusinessLogic.Projects;
	using RuntimeUserInterface.Shell;
	using Zeta.EnterpriseLibrary.Tools;
	using Zeta.EnterpriseLibrary.Windows.Persistance;
	using ZetaLongPaths;

	public partial class ProjectSettingsForm : FormBase
	{
		private Project _project;

		public ProjectSettingsForm()
		{
			InitializeComponent();
			createToolTips();
		}

		/// <summary>
		/// Since the strings constantly get lost, create
		/// them in code instead in the designer.
		/// </summary>
		private void createToolTips()
		{
			var superToolTip4 = new DevExpress.Utils.SuperToolTip();
			var toolTipTitleItem4 = new DevExpress.Utils.ToolTipTitleItem();
			var toolTipItem4 = new DevExpress.Utils.ToolTipItem();

			var superToolTip5 = new DevExpress.Utils.SuperToolTip();
			var toolTipTitleItem5 = new DevExpress.Utils.ToolTipTitleItem();
			var toolTipItem5 = new DevExpress.Utils.ToolTipItem();

			var superToolTip6 = new DevExpress.Utils.SuperToolTip();
			var toolTipTitleItem6 = new DevExpress.Utils.ToolTipTitleItem();
			var toolTipItem6 = new DevExpress.Utils.ToolTipItem();

			// --

			toolTipTitleItem4.Text = Resources.SR_toolTipTitleItem4Text;
			toolTipItem4.Text = Resources.SR_toolTipItem4Text;
			superToolTip4.Items.Add(toolTipTitleItem4);
			superToolTip4.Items.Add(toolTipItem4);
			defaultToolTipController1.SetSuperTip(pictureBox8, superToolTip4);

			toolTipTitleItem5.Text = Resources.SR_toolTipTitleItem5Text;
			toolTipItem5.Text = Resources.SR_toolTipItem5Text;
			superToolTip5.Items.Add(toolTipTitleItem5);
			superToolTip5.Items.Add(toolTipItem5);
			defaultToolTipController1.SetSuperTip(pictureBox6, superToolTip5);

			toolTipTitleItem6.Text = Resources.SR_toolTipTitleItem6Text;
			toolTipItem6.Text = Resources.SR_toolTipItem6Text;
			superToolTip6.Items.Add(toolTipTitleItem6);
			superToolTip6.Items.Add(toolTipItem6);
			defaultToolTipController1.SetSuperTip(pictureBox7, superToolTip6);
		}

		internal void Initialize(
			Project project)
		{
			_project = project;
		}

		private class ReadOnlyHelper
		{
			public ReadOnlyFileOverwriteBehaviour Behaviour { get; private set; }

			public ReadOnlyHelper(ReadOnlyFileOverwriteBehaviour behaviour)
			{
				Behaviour = behaviour;
			}

			public override string ToString()
			{
				return StringHelper.GetEnumDescription(Behaviour);
			}
		}

		private void projectSettingsForm_Load(
			object sender,
			EventArgs e)
		{
			WinFormsPersistanceHelper.RestoreState(this);
			CenterToParent();

			foreach (ReadOnlyFileOverwriteBehaviour rob in
				Enum.GetValues(typeof(ReadOnlyFileOverwriteBehaviour)))
			{
				readOnlySaveBehaviourComboBox.Properties.Items.Add(
					new ReadOnlyHelper(rob));
			}

			selectReadOnlyBehaviour(
				readOnlySaveBehaviourComboBox,
				_project.ReadOnlyFileOverwriteBehaviour);

			nameTextBox.Text = ZlpPathHelper.GetFileNameWithoutExtension(
				_project.ProjectConfigurationFilePath.Name);
			locationTextBox.Text = _project.ProjectConfigurationFilePath.DirectoryName;
			neutralLanguageCodeTextEdit.Text = _project.NeutralLanguageCode;
			useSpellCheckingCheckEdit.Checked = _project.UseSpellChecker;
			createBackupsCheckBox.Checked = _project.CreateBackupFiles;
			omitEmptyItemsCheckBox.Checked = _project.OmitEmptyItems;
			hideEmptyRowsCheck.Checked = _project.HideEmptyRows;
			hideTranslatedRowsCheck.Checked = _project.HideTranslatedRows;
			showCommentsColumnInGridCheckEdit.Checked = _project.ShowCommentsColumnInGrid;
			ignoreWindowsFormsDesignerFiles.Checked =
				_project.IgnoreWindowsFormsResourcesWithDesignerFiles;
			hideInternalDesignerRowsCheckEdit.Checked =
				_project.HideInternalDesignerRows;
			shallowCumulationCheckEdit.Checked =
				_project.UseShallowGridDataCumulation;
			hideFileGroupFilesInTreeCheckEdit.Checked =
				_project.HideFileGroupFilesInTree;

			neutralLanguageFileNamePatternTextEdit.Text = _project.NeutralLanguageFileNamePattern;
			nonNeutralLanguageFileNamePatternTextEdit.Text = _project.NonNeutralLanguageFileNamePattern;
			baseNameDotCountSpinEdit.Value = _project.BaseNameDotCount;
			defaultTypesTextEdit.Text = _project.DefaultFileTypesToIgnore;
			persistGridSettingsCheckEdit.Checked = _project.PersistGridSettings;
			colorifyNullCellsCheckEdit.Checked = _project.ColorifyNullCells;
			enableExcelExportSnapshotsCheckEdit.Checked = _project.EnableExcelExportSnapshots;
		}

		private static void selectReadOnlyBehaviour(
			ComboBoxEdit comboBoxEdit,
			ReadOnlyFileOverwriteBehaviour behaviour)
		{
			foreach (ReadOnlyHelper roh in comboBoxEdit.Properties.Items)
			{
				if (roh.Behaviour == behaviour)
				{
					comboBoxEdit.SelectedItem = roh;
					break;
				}
			}
		}

		private void projectSettingsForm_FormClosing(
			object sender,
			FormClosingEventArgs e)
		{
			WinFormsPersistanceHelper.SaveState(this);
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			_project.ReadOnlyFileOverwriteBehaviour =
				((ReadOnlyHelper)readOnlySaveBehaviourComboBox.SelectedItem).Behaviour;

			_project.CreateBackupFiles = createBackupsCheckBox.Checked;
			_project.OmitEmptyItems = omitEmptyItemsCheckBox.Checked;
			_project.NeutralLanguageCode = neutralLanguageCodeTextEdit.Text.Trim();
			_project.UseSpellChecker = useSpellCheckingCheckEdit.Checked;
			_project.HideEmptyRows = hideEmptyRowsCheck.Checked;
			_project.HideTranslatedRows = hideTranslatedRowsCheck.Checked;
			_project.ShowCommentsColumnInGrid = showCommentsColumnInGridCheckEdit.Checked;
			_project.IgnoreWindowsFormsResourcesWithDesignerFiles =
				ignoreWindowsFormsDesignerFiles.Checked;
			_project.HideInternalDesignerRows =
				hideInternalDesignerRowsCheckEdit.Checked;
			_project.UseShallowGridDataCumulation = shallowCumulationCheckEdit.Checked;
			_project.HideFileGroupFilesInTree = hideFileGroupFilesInTreeCheckEdit.Checked;

			_project.NeutralLanguageFileNamePattern = neutralLanguageFileNamePatternTextEdit.Text.Trim();
			_project.NonNeutralLanguageFileNamePattern = nonNeutralLanguageFileNamePatternTextEdit.Text.Trim();
			_project.BaseNameDotCount = (int)baseNameDotCountSpinEdit.Value;
			_project.DefaultFileTypesToIgnore = defaultTypesTextEdit.Text.Trim();
			_project.PersistGridSettings = persistGridSettingsCheckEdit.Checked;
			_project.ColorifyNullCells = colorifyNullCellsCheckEdit.Checked;
			_project.EnableExcelExportSnapshots = enableExcelExportSnapshotsCheckEdit.Checked;

			_project.MarkAsModified();
		}

		private void openButton_ItemClick(object sender, ItemClickEventArgs e)
		{
			if (!string.IsNullOrEmpty(locationTextBox.Text) &&
				ZlpIOHelper.DirectoryExists(locationTextBox.Text))
			{
				var sei =
					new ShellExecuteInformation
					{
						FileName = locationTextBox.Text
					};

				sei.Execute();
			}
		}

		private void hyperLinkEdit1_OpenLink(object sender, OpenLinkEventArgs e)
		{
			Process.Start(
				@"http://extensions.services.openoffice.org/dictionary");
		}

		private void buttonSendProject_Click(object sender, EventArgs e)
		{
			using (var form = new SendProjectWizardForm())
			{
				form.Initialize(_project);
				form.ShowDialog(this);
			}
		}
	}
}