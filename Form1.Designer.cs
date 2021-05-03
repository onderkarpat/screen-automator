/**
 * Copyright (c) 2021, O. Karpat All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, 
 * are permitted provided that the following conditions are met:
 * 
 * - Redistributions of source code must retain the above copyright notice, this list 
 *   of conditions and the following disclaimer. 
 * - Redistributions in binary form must reproduce the above copyright notice, this 
 *   list of conditions and the following disclaimer in the documentation and/or other 
 *   materials provided with the distribution. 
 * - Neither the name of the O. Karpat nor the names of its contributors may be used to 
 *   endorse or promote products derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
 * IN NO EVENT SHALL O. Karpat BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR 
 * BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
 * ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

namespace AIScreenAutomationApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.uiStmtList = new System.Windows.Forms.ListBox();
            this.uiInstList = new System.Windows.Forms.ListBox();
            this.uiStatementsGroupBox = new System.Windows.Forms.GroupBox();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.uiStmtMoveUpButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.uiStmtLabel = new System.Windows.Forms.ToolStripTextBox();
            this.uiStmtCommentToggle = new System.Windows.Forms.ToolStripButton();
            this.uiBasicBlockGroupBox = new System.Windows.Forms.GroupBox();
            this.uiInstParamRight = new System.Windows.Forms.ComboBox();
            this.uiInstParams = new System.Windows.Forms.ComboBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.uiInstMoveUpButton = new System.Windows.Forms.ToolStripButton();
            this.uiInstMoveDownButton = new System.Windows.Forms.ToolStripButton();
            this.uiInstAdd = new System.Windows.Forms.ToolStripButton();
            this.uiStepActionDelete = new System.Windows.Forms.ToolStripButton();
            this.uiInstDuplicateButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.uiBBCondList = new System.Windows.Forms.ToolStripComboBox();
            this.uiInstCommentToggle = new System.Windows.Forms.ToolStripButton();
            this.uiInstSelect = new System.Windows.Forms.ComboBox();
            this.uiTestTextBox = new System.Windows.Forms.TextBox();
            this.uiTestButton1 = new System.Windows.Forms.Button();
            this.uiTestButton2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.uiProgRun = new System.Windows.Forms.ToolStripButton();
            this.uiStopProgramButton = new System.Windows.Forms.ToolStripButton();
            this.uiSaveButton = new System.Windows.Forms.ToolStripButton();
            this.uiLoadButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.uiNewProgButton = new System.Windows.Forms.ToolStripButton();
            this.uiProgList = new System.Windows.Forms.ToolStripComboBox();
            this.uiDelProgButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.uiNewFuncButton = new System.Windows.Forms.ToolStripButton();
            this.uiFuncList = new System.Windows.Forms.ToolStripComboBox();
            this.uiDelFuncButton = new System.Windows.Forms.ToolStripButton();
            this.uiFunctionRun = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.uiStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.uiOutputWindow = new System.Windows.Forms.TextBox();
            this.uiImageLibraryTab = new System.Windows.Forms.TabPage();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.uiILPasteClipboardImage = new System.Windows.Forms.ToolStripButton();
            this.uiILReplaceImage = new System.Windows.Forms.ToolStripButton();
            this.uiILDelImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.uiILImageName = new System.Windows.Forms.ToolStripTextBox();
            this.uiILSetHotSpot = new System.Windows.Forms.ToolStripButton();
            this.uiILToggleOriginalSize = new System.Windows.Forms.ToolStripButton();
            this.uiILSetSearchRect = new System.Windows.Forms.ToolStripButton();
            this.uiILOCRBoxButton = new System.Windows.Forms.ToolStripButton();
            this.uiILSetOCRRect = new System.Windows.Forms.ToolStripButton();
            this.uiILShowOCROut = new System.Windows.Forms.ToolStripButton();
            this.uiILOCRParams = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.uiILSetThreshold = new System.Windows.Forms.ToolStripButton();
            this.uiImageLibrary = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.uiDebugWindow = new System.Windows.Forms.TextBox();
            this.uiStatementsGroupBox.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.uiBasicBlockGroupBox.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.uiImageLibraryTab.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiStmtList
            // 
            this.uiStmtList.FormattingEnabled = true;
            this.uiStmtList.Location = new System.Drawing.Point(6, 47);
            this.uiStmtList.Name = "uiStmtList";
            this.uiStmtList.Size = new System.Drawing.Size(276, 134);
            this.uiStmtList.TabIndex = 0;
            this.uiStmtList.SelectedIndexChanged += new System.EventHandler(this.uiStmtListChanged);
            // 
            // uiInstList
            // 
            this.uiInstList.FormattingEnabled = true;
            this.uiInstList.Location = new System.Drawing.Point(6, 77);
            this.uiInstList.Name = "uiInstList";
            this.uiInstList.Size = new System.Drawing.Size(331, 108);
            this.uiInstList.TabIndex = 7;
            this.uiInstList.SelectedIndexChanged += new System.EventHandler(this.uiInstListChanged);
            this.uiInstList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.uiInstListItemClicked);
            // 
            // uiStatementsGroupBox
            // 
            this.uiStatementsGroupBox.Controls.Add(this.toolStrip3);
            this.uiStatementsGroupBox.Controls.Add(this.uiStmtList);
            this.uiStatementsGroupBox.Location = new System.Drawing.Point(12, 28);
            this.uiStatementsGroupBox.Name = "uiStatementsGroupBox";
            this.uiStatementsGroupBox.Size = new System.Drawing.Size(288, 196);
            this.uiStatementsGroupBox.TabIndex = 14;
            this.uiStatementsGroupBox.TabStop = false;
            this.uiStatementsGroupBox.Text = "Statements";
            // 
            // toolStrip3
            // 
            this.toolStrip3.BackColor = System.Drawing.Color.PapayaWhip;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uiStmtMoveUpButton,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripButton6,
            this.uiStmtLabel,
            this.uiStmtCommentToggle});
            this.toolStrip3.Location = new System.Drawing.Point(3, 16);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(282, 25);
            this.toolStrip3.TabIndex = 34;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // uiStmtMoveUpButton
            // 
            this.uiStmtMoveUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiStmtMoveUpButton.Image = ((System.Drawing.Image)(resources.GetObject("uiStmtMoveUpButton.Image")));
            this.uiStmtMoveUpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiStmtMoveUpButton.Name = "uiStmtMoveUpButton";
            this.uiStmtMoveUpButton.Size = new System.Drawing.Size(23, 22);
            this.uiStmtMoveUpButton.Text = "▲";
            this.uiStmtMoveUpButton.Click += new System.EventHandler(this.uiStmtMoveUpClicked);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "▼";
            this.toolStripButton3.Click += new System.EventHandler(this.uiStmtMoveDownClicked);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "➕";
            this.toolStripButton4.Click += new System.EventHandler(this.uiStmtAddClicked);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "🗑";
            this.toolStripButton5.Click += new System.EventHandler(this.uiStmtDelClicked);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(27, 22);
            this.toolStripButton6.Text = "++";
            this.toolStripButton6.Click += new System.EventHandler(this.uiStmtDuplicateClicked);
            // 
            // uiStmtLabel
            // 
            this.uiStmtLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uiStmtLabel.Name = "uiStmtLabel";
            this.uiStmtLabel.Size = new System.Drawing.Size(100, 25);
            this.uiStmtLabel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.uiStmtLabelKeyPress);
            // 
            // uiStmtCommentToggle
            // 
            this.uiStmtCommentToggle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiStmtCommentToggle.Image = ((System.Drawing.Image)(resources.GetObject("uiStmtCommentToggle.Image")));
            this.uiStmtCommentToggle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiStmtCommentToggle.Name = "uiStmtCommentToggle";
            this.uiStmtCommentToggle.Size = new System.Drawing.Size(23, 22);
            this.uiStmtCommentToggle.Text = "//";
            this.uiStmtCommentToggle.ToolTipText = "Comment out the statement";
            this.uiStmtCommentToggle.Click += new System.EventHandler(this.uiStmtCommentToggleClicked);
            // 
            // uiBasicBlockGroupBox
            // 
            this.uiBasicBlockGroupBox.Controls.Add(this.uiInstParamRight);
            this.uiBasicBlockGroupBox.Controls.Add(this.uiInstParams);
            this.uiBasicBlockGroupBox.Controls.Add(this.toolStrip2);
            this.uiBasicBlockGroupBox.Controls.Add(this.uiInstSelect);
            this.uiBasicBlockGroupBox.Controls.Add(this.uiInstList);
            this.uiBasicBlockGroupBox.Location = new System.Drawing.Point(306, 28);
            this.uiBasicBlockGroupBox.Name = "uiBasicBlockGroupBox";
            this.uiBasicBlockGroupBox.Size = new System.Drawing.Size(343, 196);
            this.uiBasicBlockGroupBox.TabIndex = 16;
            this.uiBasicBlockGroupBox.TabStop = false;
            this.uiBasicBlockGroupBox.Text = "Instructions";
            // 
            // uiInstParamRight
            // 
            this.uiInstParamRight.FormattingEnabled = true;
            this.uiInstParamRight.Location = new System.Drawing.Point(222, 50);
            this.uiInstParamRight.Name = "uiInstParamRight";
            this.uiInstParamRight.Size = new System.Drawing.Size(115, 21);
            this.uiInstParamRight.TabIndex = 24;
            this.uiInstParamRight.SelectedIndexChanged += new System.EventHandler(this.uiInstParamRight_SelectedIndexChanged);
            this.uiInstParamRight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.uiInstParamsRightKeyPress);
            // 
            // uiInstParams
            // 
            this.uiInstParams.FormattingEnabled = true;
            this.uiInstParams.Location = new System.Drawing.Point(102, 50);
            this.uiInstParams.Name = "uiInstParams";
            this.uiInstParams.Size = new System.Drawing.Size(114, 21);
            this.uiInstParams.Sorted = true;
            this.uiInstParams.TabIndex = 23;
            this.uiInstParams.DropDown += new System.EventHandler(this.uiInstParamListShow);
            this.uiInstParams.SelectedIndexChanged += new System.EventHandler(this.uiInstParamSelectChanged);
            this.uiInstParams.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.uiInstParamsKeyPress);
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.PapayaWhip;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uiInstMoveUpButton,
            this.uiInstMoveDownButton,
            this.uiInstAdd,
            this.uiStepActionDelete,
            this.uiInstDuplicateButton,
            this.toolStripLabel1,
            this.uiBBCondList,
            this.uiInstCommentToggle});
            this.toolStrip2.Location = new System.Drawing.Point(3, 16);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(337, 25);
            this.toolStrip2.TabIndex = 22;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // uiInstMoveUpButton
            // 
            this.uiInstMoveUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiInstMoveUpButton.Image = ((System.Drawing.Image)(resources.GetObject("uiInstMoveUpButton.Image")));
            this.uiInstMoveUpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiInstMoveUpButton.Name = "uiInstMoveUpButton";
            this.uiInstMoveUpButton.Size = new System.Drawing.Size(23, 22);
            this.uiInstMoveUpButton.Text = "▲";
            this.uiInstMoveUpButton.Click += new System.EventHandler(this.uiInstMoveUpClicked);
            // 
            // uiInstMoveDownButton
            // 
            this.uiInstMoveDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiInstMoveDownButton.Image = ((System.Drawing.Image)(resources.GetObject("uiInstMoveDownButton.Image")));
            this.uiInstMoveDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiInstMoveDownButton.Name = "uiInstMoveDownButton";
            this.uiInstMoveDownButton.Size = new System.Drawing.Size(23, 22);
            this.uiInstMoveDownButton.Text = "▼";
            this.uiInstMoveDownButton.Click += new System.EventHandler(this.uiInstMoveDownClicked);
            // 
            // uiInstAdd
            // 
            this.uiInstAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiInstAdd.Image = ((System.Drawing.Image)(resources.GetObject("uiInstAdd.Image")));
            this.uiInstAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiInstAdd.Name = "uiInstAdd";
            this.uiInstAdd.Size = new System.Drawing.Size(23, 22);
            this.uiInstAdd.Text = "➕";
            this.uiInstAdd.ToolTipText = "Add instruction";
            this.uiInstAdd.Click += new System.EventHandler(this.uiInstAddClicked);
            // 
            // uiStepActionDelete
            // 
            this.uiStepActionDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiStepActionDelete.Image = ((System.Drawing.Image)(resources.GetObject("uiStepActionDelete.Image")));
            this.uiStepActionDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiStepActionDelete.Name = "uiStepActionDelete";
            this.uiStepActionDelete.Size = new System.Drawing.Size(23, 22);
            this.uiStepActionDelete.Text = "🗑";
            this.uiStepActionDelete.ToolTipText = "Delete instruction";
            this.uiStepActionDelete.Click += new System.EventHandler(this.uiInstDelClicked);
            // 
            // uiInstDuplicateButton
            // 
            this.uiInstDuplicateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiInstDuplicateButton.Enabled = false;
            this.uiInstDuplicateButton.Image = ((System.Drawing.Image)(resources.GetObject("uiInstDuplicateButton.Image")));
            this.uiInstDuplicateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiInstDuplicateButton.Name = "uiInstDuplicateButton";
            this.uiInstDuplicateButton.Size = new System.Drawing.Size(27, 22);
            this.uiInstDuplicateButton.Text = "++";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(39, 22);
            this.toolStripLabel1.Text = "Cond:";
            // 
            // uiBBCondList
            // 
            this.uiBBCondList.AutoSize = false;
            this.uiBBCondList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiBBCondList.DropDownWidth = 60;
            this.uiBBCondList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uiBBCondList.Items.AddRange(new object[] {
            "AL",
            "EQ",
            "NE",
            "GT",
            "GE",
            "LT",
            "LE"});
            this.uiBBCondList.Name = "uiBBCondList";
            this.uiBBCondList.Size = new System.Drawing.Size(75, 23);
            this.uiBBCondList.SelectedIndexChanged += new System.EventHandler(this.uiBBCondChanged);
            // 
            // uiInstCommentToggle
            // 
            this.uiInstCommentToggle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiInstCommentToggle.Image = ((System.Drawing.Image)(resources.GetObject("uiInstCommentToggle.Image")));
            this.uiInstCommentToggle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiInstCommentToggle.Name = "uiInstCommentToggle";
            this.uiInstCommentToggle.Size = new System.Drawing.Size(23, 22);
            this.uiInstCommentToggle.Text = "//";
            this.uiInstCommentToggle.ToolTipText = "Comment out the instruction";
            this.uiInstCommentToggle.Click += new System.EventHandler(this.uiInstCommentToggleClicked);
            // 
            // uiInstSelect
            // 
            this.uiInstSelect.AllowDrop = true;
            this.uiInstSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiInstSelect.FormattingEnabled = true;
            this.uiInstSelect.Location = new System.Drawing.Point(6, 50);
            this.uiInstSelect.Name = "uiInstSelect";
            this.uiInstSelect.Size = new System.Drawing.Size(90, 21);
            this.uiInstSelect.TabIndex = 17;
            this.uiInstSelect.SelectedIndexChanged += new System.EventHandler(this.uiInstListSelectChanged);
            // 
            // uiTestTextBox
            // 
            this.uiTestTextBox.Location = new System.Drawing.Point(3, 3);
            this.uiTestTextBox.Multiline = true;
            this.uiTestTextBox.Name = "uiTestTextBox";
            this.uiTestTextBox.Size = new System.Drawing.Size(424, 149);
            this.uiTestTextBox.TabIndex = 19;
            // 
            // uiTestButton1
            // 
            this.uiTestButton1.Location = new System.Drawing.Point(621, 17);
            this.uiTestButton1.Name = "uiTestButton1";
            this.uiTestButton1.Size = new System.Drawing.Size(75, 23);
            this.uiTestButton1.TabIndex = 21;
            this.uiTestButton1.Text = "Button 1";
            this.uiTestButton1.UseVisualStyleBackColor = true;
            this.uiTestButton1.Click += new System.EventHandler(this.uiTestButton1_Click);
            // 
            // uiTestButton2
            // 
            this.uiTestButton2.Location = new System.Drawing.Point(621, 45);
            this.uiTestButton2.Name = "uiTestButton2";
            this.uiTestButton2.Size = new System.Drawing.Size(75, 23);
            this.uiTestButton2.TabIndex = 22;
            this.uiTestButton2.Text = "Button 2";
            this.uiTestButton2.UseVisualStyleBackColor = true;
            this.uiTestButton2.Click += new System.EventHandler(this.uiTestButton2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(477, 21);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(128, 20);
            this.textBox1.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(433, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "Field 1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(433, 53);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Field 2";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(477, 47);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(128, 20);
            this.textBox2.TabIndex = 25;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.PapayaWhip;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uiProgRun,
            this.uiStopProgramButton,
            this.uiSaveButton,
            this.uiLoadButton,
            this.toolStripSeparator3,
            this.uiNewProgButton,
            this.uiProgList,
            this.uiDelProgButton,
            this.toolStripSeparator1,
            this.uiNewFuncButton,
            this.uiFuncList,
            this.uiDelFuncButton,
            this.uiFunctionRun,
            this.toolStripButton1,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(749, 25);
            this.toolStrip1.TabIndex = 38;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // uiProgRun
            // 
            this.uiProgRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiProgRun.Image = ((System.Drawing.Image)(resources.GetObject("uiProgRun.Image")));
            this.uiProgRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiProgRun.Name = "uiProgRun";
            this.uiProgRun.Size = new System.Drawing.Size(23, 22);
            this.uiProgRun.Text = "▶️";
            this.uiProgRun.Click += new System.EventHandler(this.uiProgRunClicked);
            // 
            // uiStopProgramButton
            // 
            this.uiStopProgramButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiStopProgramButton.Image = ((System.Drawing.Image)(resources.GetObject("uiStopProgramButton.Image")));
            this.uiStopProgramButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiStopProgramButton.Name = "uiStopProgramButton";
            this.uiStopProgramButton.Size = new System.Drawing.Size(23, 22);
            this.uiStopProgramButton.Text = "❌";
            this.uiStopProgramButton.ToolTipText = "Stop Program";
            this.uiStopProgramButton.Click += new System.EventHandler(this.uiProgStopClicked);
            // 
            // uiSaveButton
            // 
            this.uiSaveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiSaveButton.Image = ((System.Drawing.Image)(resources.GetObject("uiSaveButton.Image")));
            this.uiSaveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiSaveButton.Name = "uiSaveButton";
            this.uiSaveButton.RightToLeftAutoMirrorImage = true;
            this.uiSaveButton.Size = new System.Drawing.Size(23, 22);
            this.uiSaveButton.Text = "🖫";
            this.uiSaveButton.Click += new System.EventHandler(this.uiSaveClicked);
            // 
            // uiLoadButton
            // 
            this.uiLoadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiLoadButton.Image = ((System.Drawing.Image)(resources.GetObject("uiLoadButton.Image")));
            this.uiLoadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiLoadButton.Name = "uiLoadButton";
            this.uiLoadButton.Size = new System.Drawing.Size(23, 22);
            this.uiLoadButton.Text = "📂";
            this.uiLoadButton.Click += new System.EventHandler(this.uiLoadClicked);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // uiNewProgButton
            // 
            this.uiNewProgButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiNewProgButton.Image = ((System.Drawing.Image)(resources.GetObject("uiNewProgButton.Image")));
            this.uiNewProgButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiNewProgButton.Name = "uiNewProgButton";
            this.uiNewProgButton.Size = new System.Drawing.Size(84, 22);
            this.uiNewProgButton.Text = "New Program";
            this.uiNewProgButton.Click += new System.EventHandler(this.uiProgNewClicked);
            // 
            // uiProgList
            // 
            this.uiProgList.Name = "uiProgList";
            this.uiProgList.Size = new System.Drawing.Size(121, 25);
            this.uiProgList.SelectedIndexChanged += new System.EventHandler(this.uiProgListChanged);
            this.uiProgList.Leave += new System.EventHandler(this.uiProgListNameChanged);
            this.uiProgList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.uiProgListKeyPress);
            // 
            // uiDelProgButton
            // 
            this.uiDelProgButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiDelProgButton.Image = ((System.Drawing.Image)(resources.GetObject("uiDelProgButton.Image")));
            this.uiDelProgButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiDelProgButton.Name = "uiDelProgButton";
            this.uiDelProgButton.Size = new System.Drawing.Size(23, 22);
            this.uiDelProgButton.Text = "🗑";
            this.uiDelProgButton.Click += new System.EventHandler(this.uiProgDelClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // uiNewFuncButton
            // 
            this.uiNewFuncButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiNewFuncButton.Image = ((System.Drawing.Image)(resources.GetObject("uiNewFuncButton.Image")));
            this.uiNewFuncButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiNewFuncButton.Name = "uiNewFuncButton";
            this.uiNewFuncButton.Size = new System.Drawing.Size(85, 22);
            this.uiNewFuncButton.Text = "New Function";
            this.uiNewFuncButton.Click += new System.EventHandler(this.uiFuncNewClicked);
            // 
            // uiFuncList
            // 
            this.uiFuncList.BackColor = System.Drawing.Color.Moccasin;
            this.uiFuncList.Name = "uiFuncList";
            this.uiFuncList.Size = new System.Drawing.Size(171, 25);
            this.uiFuncList.SelectedIndexChanged += new System.EventHandler(this.uiFuncListChanged);
            this.uiFuncList.Leave += new System.EventHandler(this.uiFuncListNameChanged);
            this.uiFuncList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.uiFuncListKeyPress);
            // 
            // uiDelFuncButton
            // 
            this.uiDelFuncButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiDelFuncButton.Image = ((System.Drawing.Image)(resources.GetObject("uiDelFuncButton.Image")));
            this.uiDelFuncButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiDelFuncButton.Name = "uiDelFuncButton";
            this.uiDelFuncButton.Size = new System.Drawing.Size(23, 22);
            this.uiDelFuncButton.Text = "🗑";
            this.uiDelFuncButton.ToolTipText = "Delete function";
            this.uiDelFuncButton.Click += new System.EventHandler(this.uiFuncDelClicked);
            // 
            // uiFunctionRun
            // 
            this.uiFunctionRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiFunctionRun.Image = ((System.Drawing.Image)(resources.GetObject("uiFunctionRun.Image")));
            this.uiFunctionRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiFunctionRun.Name = "uiFunctionRun";
            this.uiFunctionRun.Size = new System.Drawing.Size(23, 22);
            this.uiFunctionRun.Text = "▶️";
            this.uiFunctionRun.ToolTipText = "Start run on this function";
            this.uiFunctionRun.Click += new System.EventHandler(this.uiFuncRunClicked);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(27, 22);
            this.toolStripButton1.Text = "++";
            this.toolStripButton1.ToolTipText = "Duplicate function";
            this.toolStripButton1.Click += new System.EventHandler(this.uiFuncDuplicateClicked);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uiStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 420);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(749, 22);
            this.statusStrip1.TabIndex = 39;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // uiStatusLabel
            // 
            this.uiStatusLabel.Name = "uiStatusLabel";
            this.uiStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.uiImageLibraryTab);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 230);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(728, 187);
            this.tabControl1.TabIndex = 40;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.uiOutputWindow);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(720, 161);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Output Window";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // uiOutputWindow
            // 
            this.uiOutputWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiOutputWindow.Location = new System.Drawing.Point(4, 7);
            this.uiOutputWindow.Multiline = true;
            this.uiOutputWindow.Name = "uiOutputWindow";
            this.uiOutputWindow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.uiOutputWindow.Size = new System.Drawing.Size(710, 142);
            this.uiOutputWindow.TabIndex = 0;
            // 
            // uiImageLibraryTab
            // 
            this.uiImageLibraryTab.Controls.Add(this.toolStrip4);
            this.uiImageLibraryTab.Controls.Add(this.uiImageLibrary);
            this.uiImageLibraryTab.Location = new System.Drawing.Point(4, 22);
            this.uiImageLibraryTab.Name = "uiImageLibraryTab";
            this.uiImageLibraryTab.Padding = new System.Windows.Forms.Padding(3);
            this.uiImageLibraryTab.Size = new System.Drawing.Size(720, 161);
            this.uiImageLibraryTab.TabIndex = 1;
            this.uiImageLibraryTab.Text = "Image Library";
            this.uiImageLibraryTab.UseVisualStyleBackColor = true;
            // 
            // toolStrip4
            // 
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uiILPasteClipboardImage,
            this.uiILReplaceImage,
            this.uiILDelImage,
            this.toolStripLabel2,
            this.uiILImageName,
            this.uiILSetHotSpot,
            this.uiILToggleOriginalSize,
            this.uiILSetSearchRect,
            this.uiILOCRBoxButton,
            this.uiILSetOCRRect,
            this.uiILShowOCROut,
            this.uiILOCRParams,
            this.toolStripButton2,
            this.uiILSetThreshold});
            this.toolStrip4.Location = new System.Drawing.Point(3, 3);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.Size = new System.Drawing.Size(714, 25);
            this.toolStrip4.TabIndex = 1;
            this.toolStrip4.Text = "toolStrip4";
            // 
            // uiILPasteClipboardImage
            // 
            this.uiILPasteClipboardImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiILPasteClipboardImage.Image = ((System.Drawing.Image)(resources.GetObject("uiILPasteClipboardImage.Image")));
            this.uiILPasteClipboardImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiILPasteClipboardImage.Name = "uiILPasteClipboardImage";
            this.uiILPasteClipboardImage.Size = new System.Drawing.Size(23, 22);
            this.uiILPasteClipboardImage.Text = "➕";
            this.uiILPasteClipboardImage.Click += new System.EventHandler(this.uiILPasteClipboardImageClicked);
            // 
            // uiILReplaceImage
            // 
            this.uiILReplaceImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiILReplaceImage.Image = ((System.Drawing.Image)(resources.GetObject("uiILReplaceImage.Image")));
            this.uiILReplaceImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiILReplaceImage.Name = "uiILReplaceImage";
            this.uiILReplaceImage.Size = new System.Drawing.Size(23, 22);
            this.uiILReplaceImage.Text = "↶";
            this.uiILReplaceImage.Click += new System.EventHandler(this.uiILReplaceImageClicked);
            // 
            // uiILDelImage
            // 
            this.uiILDelImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiILDelImage.Image = ((System.Drawing.Image)(resources.GetObject("uiILDelImage.Image")));
            this.uiILDelImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiILDelImage.Name = "uiILDelImage";
            this.uiILDelImage.Size = new System.Drawing.Size(23, 22);
            this.uiILDelImage.Text = "🗑";
            this.uiILDelImage.Click += new System.EventHandler(this.uiILImageDelClicked);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(42, 22);
            this.toolStripLabel2.Text = "Name:";
            // 
            // uiILImageName
            // 
            this.uiILImageName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uiILImageName.Name = "uiILImageName";
            this.uiILImageName.Size = new System.Drawing.Size(100, 25);
            this.uiILImageName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.uiILImageNameChanged);
            // 
            // uiILSetHotSpot
            // 
            this.uiILSetHotSpot.CheckOnClick = true;
            this.uiILSetHotSpot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiILSetHotSpot.Image = ((System.Drawing.Image)(resources.GetObject("uiILSetHotSpot.Image")));
            this.uiILSetHotSpot.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiILSetHotSpot.Name = "uiILSetHotSpot";
            this.uiILSetHotSpot.Size = new System.Drawing.Size(23, 22);
            this.uiILSetHotSpot.Text = "🎯";
            this.uiILSetHotSpot.ToolTipText = "Set Hot Spot";
            // 
            // uiILToggleOriginalSize
            // 
            this.uiILToggleOriginalSize.CheckOnClick = true;
            this.uiILToggleOriginalSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiILToggleOriginalSize.Image = ((System.Drawing.Image)(resources.GetObject("uiILToggleOriginalSize.Image")));
            this.uiILToggleOriginalSize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiILToggleOriginalSize.Name = "uiILToggleOriginalSize";
            this.uiILToggleOriginalSize.Size = new System.Drawing.Size(23, 22);
            this.uiILToggleOriginalSize.Text = "⛶";
            this.uiILToggleOriginalSize.ToolTipText = "Toggle original size";
            this.uiILToggleOriginalSize.Click += new System.EventHandler(this.uiILToggleOriginalSizeClicked);
            // 
            // uiILSetSearchRect
            // 
            this.uiILSetSearchRect.CheckOnClick = true;
            this.uiILSetSearchRect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiILSetSearchRect.Image = ((System.Drawing.Image)(resources.GetObject("uiILSetSearchRect.Image")));
            this.uiILSetSearchRect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiILSetSearchRect.Name = "uiILSetSearchRect";
            this.uiILSetSearchRect.Size = new System.Drawing.Size(23, 22);
            this.uiILSetSearchRect.Text = "🔍";
            this.uiILSetSearchRect.ToolTipText = "Set search rectangle";
            // 
            // uiILOCRBoxButton
            // 
            this.uiILOCRBoxButton.CheckOnClick = true;
            this.uiILOCRBoxButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiILOCRBoxButton.Image = ((System.Drawing.Image)(resources.GetObject("uiILOCRBoxButton.Image")));
            this.uiILOCRBoxButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiILOCRBoxButton.Name = "uiILOCRBoxButton";
            this.uiILOCRBoxButton.Size = new System.Drawing.Size(23, 22);
            this.uiILOCRBoxButton.Text = "❏";
            this.uiILOCRBoxButton.ToolTipText = "Set OCR box";
            this.uiILOCRBoxButton.Click += new System.EventHandler(this.uiILOCRBoxButtonClicked);
            // 
            // uiILSetOCRRect
            // 
            this.uiILSetOCRRect.CheckOnClick = true;
            this.uiILSetOCRRect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiILSetOCRRect.Image = ((System.Drawing.Image)(resources.GetObject("uiILSetOCRRect.Image")));
            this.uiILSetOCRRect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiILSetOCRRect.Name = "uiILSetOCRRect";
            this.uiILSetOCRRect.Size = new System.Drawing.Size(23, 22);
            this.uiILSetOCRRect.Text = "🗚";
            // 
            // uiILShowOCROut
            // 
            this.uiILShowOCROut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiILShowOCROut.Image = ((System.Drawing.Image)(resources.GetObject("uiILShowOCROut.Image")));
            this.uiILShowOCROut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiILShowOCROut.Name = "uiILShowOCROut";
            this.uiILShowOCROut.Size = new System.Drawing.Size(81, 22);
            this.uiILShowOCROut.Text = "showOCRout";
            this.uiILShowOCROut.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.uiILShowOCROut.Click += new System.EventHandler(this.uiILShowOCROutClicked);
            // 
            // uiILOCRParams
            // 
            this.uiILOCRParams.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uiILOCRParams.Name = "uiILOCRParams";
            this.uiILOCRParams.Size = new System.Drawing.Size(100, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "¿";
            this.toolStripButton2.ToolTipText = "Check match result";
            this.toolStripButton2.Click += new System.EventHandler(this.uiILCheckMatchResult);
            // 
            // uiILSetThreshold
            // 
            this.uiILSetThreshold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.uiILSetThreshold.Image = ((System.Drawing.Image)(resources.GetObject("uiILSetThreshold.Image")));
            this.uiILSetThreshold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uiILSetThreshold.Name = "uiILSetThreshold";
            this.uiILSetThreshold.Size = new System.Drawing.Size(23, 22);
            this.uiILSetThreshold.Text = "📈";
            this.uiILSetThreshold.Click += new System.EventHandler(this.uiILSetThresholdClicked);
            // 
            // uiImageLibrary
            // 
            this.uiImageLibrary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiImageLibrary.AutoScroll = true;
            this.uiImageLibrary.Location = new System.Drawing.Point(7, 37);
            this.uiImageLibrary.Name = "uiImageLibrary";
            this.uiImageLibrary.Size = new System.Drawing.Size(731, 112);
            this.uiImageLibrary.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBox1);
            this.tabPage3.Controls.Add(this.uiTestTextBox);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.uiTestButton2);
            this.tabPage3.Controls.Add(this.textBox2);
            this.tabPage3.Controls.Add(this.uiTestButton1);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(720, 161);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Test Area";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.uiDebugWindow);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(720, 161);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Debug Window";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // uiDebugWindow
            // 
            this.uiDebugWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiDebugWindow.Location = new System.Drawing.Point(5, 6);
            this.uiDebugWindow.Multiline = true;
            this.uiDebugWindow.Name = "uiDebugWindow";
            this.uiDebugWindow.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.uiDebugWindow.Size = new System.Drawing.Size(738, 142);
            this.uiDebugWindow.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PapayaWhip;
            this.ClientSize = new System.Drawing.Size(749, 442);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.uiBasicBlockGroupBox);
            this.Controls.Add(this.uiStatementsGroupBox);
            this.Name = "Form1";
            this.Text = "Screen Automator";
            this.uiStatementsGroupBox.ResumeLayout(false);
            this.uiStatementsGroupBox.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.uiBasicBlockGroupBox.ResumeLayout(false);
            this.uiBasicBlockGroupBox.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.uiImageLibraryTab.ResumeLayout(false);
            this.uiImageLibraryTab.PerformLayout();
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox uiStatementsGroupBox;
        private System.Windows.Forms.GroupBox uiBasicBlockGroupBox;
        private System.Windows.Forms.ComboBox uiInstSelect;
        private System.Windows.Forms.TextBox uiTestTextBox;
        private System.Windows.Forms.Button uiTestButton1;
        private System.Windows.Forms.Button uiTestButton2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        internal System.Windows.Forms.ToolStripButton uiProgRun;
        private System.Windows.Forms.ToolStripButton uiSaveButton;
        private System.Windows.Forms.ToolStripButton uiLoadButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        internal System.Windows.Forms.ToolStripStatusLabel uiStatusLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton uiNewFuncButton;
        private System.Windows.Forms.ToolStripButton uiDelFuncButton;
        internal System.Windows.Forms.ToolStripButton uiFunctionRun;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage uiImageLibraryTab;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox uiOutputWindow;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.FlowLayoutPanel uiImageLibrary;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton uiInstMoveUpButton;
        private System.Windows.Forms.ToolStripButton uiInstMoveDownButton;
        private System.Windows.Forms.ToolStripButton uiInstAdd;
        private System.Windows.Forms.ToolStripButton uiStepActionDelete;
        private System.Windows.Forms.ToolStripButton uiInstDuplicateButton;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox uiBBCondList;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton uiStmtMoveUpButton;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton uiStopProgramButton;
        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripButton uiILDelImage;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox uiILImageName;
        private System.Windows.Forms.ToolStripButton uiILSetHotSpot;
        private System.Windows.Forms.ToolStripButton uiILPasteClipboardImage;
        private System.Windows.Forms.ToolStripComboBox uiProgList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton uiNewProgButton;
        private System.Windows.Forms.ToolStripButton uiDelProgButton;
        private System.Windows.Forms.ToolStripTextBox uiStmtLabel;
        private System.Windows.Forms.ComboBox uiInstParams;
        internal System.Windows.Forms.TextBox uiDebugWindow;
        internal System.Windows.Forms.ToolStripComboBox uiFuncList;
        internal System.Windows.Forms.ListBox uiStmtList;
        internal System.Windows.Forms.ListBox uiInstList;
        private System.Windows.Forms.ToolStripButton uiILToggleOriginalSize;
        private System.Windows.Forms.ToolStripButton uiILSetSearchRect;
        private System.Windows.Forms.ToolStripButton uiILReplaceImage;
        private System.Windows.Forms.ToolStripButton uiILSetOCRRect;
        private System.Windows.Forms.ToolStripButton uiILShowOCROut;
        private System.Windows.Forms.ToolStripTextBox uiILOCRParams;
        private System.Windows.Forms.ComboBox uiInstParamRight;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton uiILSetThreshold;
        private System.Windows.Forms.ToolStripButton uiILOCRBoxButton;
        private System.Windows.Forms.ToolStripButton uiStmtCommentToggle;
        private System.Windows.Forms.ToolStripButton uiInstCommentToggle;
    }
}

