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

using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using Tesseract;
using FuncName = System.String;
using StmtLabel = System.String;
using VarName = System.String;
using RetVal = System.String;
using System.Linq;

internal struct RunContext {
    internal AIScreenAutomationApp.Form1 form;
    internal Point screenMatchLoc;
    internal Size matchSize;
    internal Point actionOffsetPt;
    private STMT_CONDITIONS _currCond;
    internal STMT_CONDITIONS currCond {
        get {
            return _currCond;
        } // get
        set {
            form.updateVariableTableRow("currCond", _currCond.ToString(), false, "All,Debug");
            _currCond = value;
        } // set
    } // currCond
    internal string retVal;
    internal Dictionary<string, string> vars;
    internal Dictionary<string, List<string>> arrays;
    internal double maxValue;
    internal string ocrOut;
    internal Dictionary<string, Image> imagesCaptured; // Used for debugging for now.
    internal string clipboard;
    internal Rectangle copyClipboardRect;
} // struct RunContext

namespace AIScreenAutomationApp {
    public partial class Form1 : Form {
        private Point panStartingPoint = Point.Empty;
        private Point panMovingPoint = Point.Empty;
        private bool panning = false;
        internal static Form1 gForm = null;
        internal BackgroundWorker runWorker;
        internal double[] minValues;
        internal double[] maxValues;
        internal Point[] minLocations;
        internal Point[] maxLocations;
        internal Stack<Tuple<FuncName, StmtLabel, RetVal, List<string>>> callStack;
        internal Stack<VarName> argStack;
        internal global::Folder folder;
        internal PROGRAM_STATE programState;
        internal Image uiBoxCalibrateCaptureImgNonZoom;
        internal int selectedFuncIndex = -1;
        internal int selectedProgIndex = -1;
        internal TesseractEngine ocrEngine = null;
        internal RunContext runContext;
        internal int zoomRatio = 100;
        internal Stack<string> indent = new Stack<string>();
        internal Script nullScript = null;
        internal Function nullFunc = null;
        internal Statement nullStmt = null;
        internal Instruction nullInst = null;
        internal Dictionary<string, bool> skipList;
        internal Dictionary<object, EventHandler> callbackMap;
        internal Dictionary<object, bool> callbackMapStatus;
        internal int PB_PADDING;
        internal bool breakOnException = true;
        internal bool stopOnException = true;

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal string funcName() {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        } // funcName
        internal void selectUpdateEnable<T>(T list, bool isEnabled) {
            dtbegin(funcName());
            {
                var obj = list as ToolStripComboBox;
                if (obj != null) {
                    callbackMapStatus[list] = isEnabled;
                    if (isEnabled) {
                        obj.SelectedIndexChanged += callbackMap[list];
                    } else {
                        obj.SelectedIndexChanged -= callbackMap[list];
                    } // if
                } // if
            }
            {
                var obj = list as ComboBox;
                if (obj != null) {
                    callbackMapStatus[list] = isEnabled;
                    if (isEnabled) {
                        obj.SelectedIndexChanged += callbackMap[list];
                    } else {
                        obj.SelectedIndexChanged -= callbackMap[list];
                    } // if
                } // if
            }
            {
                var obj = list as ListBox;
                if (obj != null) {
                    callbackMapStatus[list] = isEnabled;
                    if (isEnabled) {
                        obj.SelectedIndexChanged += callbackMap[list];
                    } else {
                        obj.SelectedIndexChanged -= callbackMap[list];
                    } // if
                } // if
            }
            dtend();
        } // selectUpdateEnable
        internal TextBox getOutputWindow() {
            return uiOutputWindow;
        } // function getOutputWindow
        internal Form1() {
            ocrEngine = new TesseractEngine(@".", "eng", EngineMode.Default);
            gForm = this;
            InitializeComponent();
            skipList = new Dictionary<string, bool>();
            skipList["updateUIStates"] = true;
            skipList["updateTargetImage"] = true;
            skipList["selectUpdateEnable"] = true;
            skipList["uiFuncListChanged"] = true;
            skipList["uiInstListChanged"] = true;
            skipList["uiStmtListChanged"] = true;
            skipList["uiInstParamSelectChanged"] = true;
            skipList["runNextStatement"] = true;
            skipList["uiStmtDelClicked"] = true;
            skipList["uiInstAddClicked"] = true;
            skipList["uiStmtLabelKeyPress"] = true;
            skipList["uiStmtNameUpdateClicked"] = true;
            skipList["uiStmtNameUpdateClicked"] = true;
            skipList["uiStmtNameUpdateClicked"] = true;
            skipList["uiStmtNameUpdateClicked"] = true;
            skipList["uiStmtNameUpdateClicked"] = true;
            skipList["uiILImageClicked"] = true;
            skipList["updateStmtUI"] = true;
            skipList["runInstFinished"] = true;
            skipList["runInst"] = true;
            callbackMapStatus = new Dictionary<object, bool>();
            callbackMapStatus[uiProgList] = true;
            callbackMapStatus[uiFuncList] = true;
            callbackMapStatus[uiStmtList] = true;
            callbackMapStatus[uiInstList] = true;
            callbackMapStatus[uiInstSelect] = true;
            callbackMapStatus[uiBBCondList] = true;
            callbackMapStatus[uiInstParams] = true;
            callbackMapStatus[uiInstParamRight] = true;
            callbackMap = new Dictionary<object, EventHandler>();
            callbackMap[uiProgList] = uiProgListChanged;
            callbackMap[uiFuncList] = uiFuncListChanged;
            callbackMap[uiStmtList] = uiStmtListChanged;
            callbackMap[uiInstList] = uiInstListChanged;
            callbackMap[uiInstSelect] = uiInstListSelectChanged;
            callbackMap[uiBBCondList] = uiBBCondChanged;
            callbackMap[uiInstParams] = uiInstParamSelectChanged;
            callbackMap[uiInstParamRight] = uiInstParamRight_SelectedIndexChanged;
            resetRunContext();
            runWorker = new BackgroundWorker();
            runWorker.DoWork += new DoWorkEventHandler(runInst);
            runWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(runInstFinished);
            uiInstSelect.Items.Add("Assign");
            uiInstSelect.Items.Add("Cmp Args");
            uiInstSelect.Items.Add("Cmp Image");
            uiInstSelect.Items.Add("Cmp Box");
            uiInstSelect.Items.Add("Cmp Box-copy");
            uiInstSelect.Items.Add("Mouse");
            uiInstSelect.Items.Add("Delay");
            uiInstSelect.Items.Add("Key Press");
            uiInstSelect.Items.Add("Stop Program");
            uiInstSelect.Items.Add("Call Func");
            uiInstSelect.Items.Add("Func Return");
            uiInstSelect.Items.Add("Jump to Stmt");
            uiInstSelect.Items.Add("Print");
            uiInstSelect.SelectedIndex = 0;
            programState = PROGRAM_STATE.STOP;
            folder = new Folder();
            updateUIStates();
            uiVariableTableInit();
            uiVariablesTableGroupList.Items.Add("All");
            uiVariablesTableGroupList.Items.Add("Images");
            uiVariablesTableGroupList.Items.Add("Globals");
            uiVariablesTableGroupList.Items.Add("Debug");
            uiVariablesTableGroupList.Items.Add("Locals");
            uiVariablesTableGroupList.Items.Add("Persistents");
            uiVariablesTableGroupList.SelectedIndex = 0;

            void UiBoxCalibrateCaptureImgPanel_MouseWheel(object sender, MouseEventArgs e) {
                void ZoomIn() {
                    zoomRatio += 5;
                } // ZoomIn
                void ZoomOut() {
                    if (zoomRatio > 15) {
                        zoomRatio -= 5;
                    } // if
                } // ZoomOut
                // uiBoxCalibrateZoomLabel
                try {
                    if (e.Delta < 0) {
                        ZoomIn();
                    } else {
                        ZoomOut();
                    } // if
                    Bitmap bitmap = new Bitmap(uiBoxCalibrateCaptureImgNonZoom);
                    Image<Bgr, Byte> image = bitmap.ToImage<Bgr, Byte>().Resize(zoomRatio / 100.0, Inter.Nearest);
                    uiBoxCalibrateCaptureImg.Image = image.ToBitmap();
                    uiBoxCalibrateCaptureImg.Width = image.Width;
                    uiBoxCalibrateCaptureImg.Height= image.Height;
                } catch (Exception ) {
                    zoomRatio = 100;
                } // try
                ((HandledMouseEventArgs)e).Handled = true;
                uiBoxCalibrateZoomLabel.Text = $"Zoom @{zoomRatio}%";
            } // UiBoxCalibrateCaptureImgPanel_MouseWheel
            void UiBoxCalibrateCaptureImg_MouseUp(object sender, MouseEventArgs e) {
                panning = false;
            } // UiBoxCalibrateCaptureImg_MouseUp
            void UiBoxCalibrateCaptureImg_MouseDown(object sender, MouseEventArgs e) {
                panning = true;
                panStartingPoint = new Point(e.Location.X - panMovingPoint.X,
                                          e.Location.Y - panMovingPoint.Y);
            } // UiBoxCalibrateCaptureImg_MouseDown
            void UiBoxCalibrateCaptureImg_MouseMove(object sender, MouseEventArgs e) {
                PictureBox pb = sender as PictureBox;
                try {
                    if (panning) {
                        panMovingPoint = new Point(e.Location.X - panStartingPoint.X,
                                                e.Location.Y - panStartingPoint.Y);
                        uiBoxCalibrateCaptureImg.Invalidate();
                    } else {
                        if (pb.Image != null) {
                            var bitmap = new Bitmap(pb.Image);
                            int x = e.Location.X - panMovingPoint.X;
                            int y = e.Location.Y - panMovingPoint.Y;
                            x = (int)(x / (float)zoomRatio * 100.0);
                            y = (int)(y / (float)zoomRatio * 100.0);
                            if (x >= 0 && y >= 0 && x <bitmap.Width && y < bitmap.Height) {
                                var val = bitmap.GetPixel(x, y);
                                uiBoxCalibratePosValLabel.Text = $"{x},{y}={val}";
                            } else {
                                uiBoxCalibratePosValLabel.Text = $"{x},{y}=?";
                            } // if
                        } // if
                    } // if
                } catch (Exception ex) {
                    uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
                } // try
            } // UiBoxCalibrateCaptureImg_MouseMove
            void UiBoxCalibrateCaptureImg_Paint(object sender, PaintEventArgs e) {
                try {
                    PictureBox pb = sender as PictureBox;
                    if (pb.Image != null) {
                        e.Graphics.Clear(Color.White);
                        e.Graphics.DrawImage(pb.Image, panMovingPoint);
                    } // if
                } catch (Exception ex) {
                    uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
                } // try
            } // UiBoxCalibrateCaptureImg_Paint
            uiBoxCalibrateCaptureImgPanel.MouseWheel += UiBoxCalibrateCaptureImgPanel_MouseWheel;
            uiBoxCalibrateCaptureImg.MouseDown += UiBoxCalibrateCaptureImg_MouseDown;
            uiBoxCalibrateCaptureImg.MouseUp += UiBoxCalibrateCaptureImg_MouseUp;
            uiBoxCalibrateCaptureImg.MouseMove += UiBoxCalibrateCaptureImg_MouseMove;
            uiBoxCalibrateCaptureImg.Paint += UiBoxCalibrateCaptureImg_Paint;
        } // function Form1
        internal void resetRunContext() {
            runContext.vars = new Dictionary<string, string>();
            runContext.arrays = new Dictionary<string, List<string>>();
            runContext.imagesCaptured = new Dictionary<string, Image>();
            runContext.form = this;
            runContext.vars["retVal"] = "<null>";
            runContext.vars["comma"] = ",";
            runContext.vars["at"] = "@";
            runContext.clipboard = "";
            runContext.currCond = STMT_CONDITIONS.NE;
        } // resetRunContext
        private void UiBoxCalibrateCaptureImgPanel_MouseWheel(object sender, MouseEventArgs e) {
            throw new NotImplementedException();
        } // UiBoxCalibrateCaptureImgPanel_MouseWheel
        internal string checkBreak {
            get {
                if (breakOnException) {
                    Debugger.Break();
                } // if
                if (stopOnException) {
                    programState = PROGRAM_STATE.STOP;
                } // if
                return "";
            } // get
        } // checkBreak;
        #region Dynamic Properties
        internal bool progRunning {
            get {
                return programState == PROGRAM_STATE.RUNNING;
            } // get
        } // property progRunning
        internal ImageLibrary imageLib {
            get {
                return folder.imageLibrary;
            } // get
        } // ImageLibrary imageLib 
        internal PictureBox selectedImageVarPB {
            get {
                foreach (PictureBox pb in uiImageLibrary.Controls) {
                    if (pb.BorderStyle == BorderStyle.FixedSingle) {
                        return pb;
                    } // if
                } // foreach
                return null;
            } // get
        } // selectedImageVar
        internal Script currProg {
            get {
                if (folder.progList.Count == 0) return null;
                if (uiProgList.SelectedIndex == -1) return null;
                return folder.progList[uiProgList.SelectedIndex];
            } // get
        } // global::Program currProg
        internal Function currFunc {
            get {
                if (currProg == null) return null;
                if (currProg.funcList.Exists(func => func.funcName == uiFuncList.Text)) {
                    return currProg.funcList.Find(func => func.funcName == uiFuncList.Text);
                } // if
                return null;
            } // get
        } // currFunc
        internal Statement currStmt {
            get {
                if (uiStmtList.SelectedItem != null) {
                    string currStmtLabel = uiStmtList.SelectedItem.ToString();
                    if (currFunc != null) {
                        foreach (Statement stmt in currFunc.stmtList) {
                            if (stmt.text == currStmtLabel) {
                                return stmt;
                            } // if
                        } // foreach
                    } // if
                } // if
                return null;
            } // get
        } // Statement currStmt
        internal Instruction currInst {
            get {
                Instruction retVal = null;
                if (currStmt != null && uiInstList.SelectedIndex != -1) {
                    if (currStmt.instructions.Count == 0) return null;
                    return currStmt.instructions[uiInstList.SelectedIndex];
                } // if
                return retVal;
            } // get
        } // property currInst
        internal Instruction lastInst {
            get {
                if (currStmt.instructions.Count != 0) {
                    return currStmt.instructions[currStmt.instructions.Count-1];
                } // if
                return null;
            } // get
        } // property currInst
        internal Instruction nextInst {
            get {
                if (uiInstList.SelectedIndex != -1 && uiInstList.SelectedIndex != uiInstList.Items.Count-1) {
                    return currStmt.instructions[uiInstList.SelectedIndex+1];
                } // if
                return null;
            } // get
        } // property nextInst
        internal Instruction prevInst {
            get {
                if (uiInstList.SelectedIndex > 0) {
                    return currStmt.instructions[uiInstList.SelectedIndex-1];
                } // if
                return null;
            } // get
        } // property prevInst
        #endregion

        #region Debugging related functions
        internal T dtend<T>(T expr, [CallerLineNumber] int lineNum = -1, [CallerFilePath] string fileName = "") {
            debugOut($"}} // {indent.Pop()}", lineNum, fileName);
            return expr;
        } // dtend<T>
        internal void dtend([CallerLineNumber] int lineNum = -1, [CallerFilePath] string fileName = "") {
            try {
                if (!skipList.ContainsKey(indent.Peek())) {
                    debugOut($"}} // {indent.Peek()}", lineNum, fileName);
                } // if
                indent.Pop();
            } catch (Exception e) {
                debugOut($"Exception", lineNum, fileName);
            } // try
        } // dtend
        internal void dtbegin(string text, [CallerLineNumber] int lineNum = -1, [CallerFilePath] string fileName = "") {
            try {
                if (!skipList.ContainsKey(text)) {
                    debugOut($"{text}() {{", lineNum, fileName);
                } // if
                indent.Push(text);
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // dtbegin
        internal void debugOut(string text, int lineNum=-1, string fileName="") {
            try {
                uiDebugWindow.AppendText(text + Environment.NewLine);
            } catch (Exception e) { 
            } // try
            string pad = indent.Count != 0 ? new string(' ', indent.Count*4) : "";
            string loc = $"{fileName}({lineNum,-4}):";
            Console.WriteLine($"{loc}{pad}{text}");
        } // debugOut
        internal void stdOut(string text) {
            try {
                uiOutputWindow.AppendText(text);
            } catch (Exception e) { 
            } // try
            Console.WriteLine(text);
        } // debugOut
        internal void putImage(string name, Bitmap image) {
            try {
                ImageVar imImage = null;
                List<ImageVar> ivs = folder.imageLibrary.findImage(name);
                if (ivs.Count != 0) {
                    imImage = ivs[0];
                } // if
                if (imImage != null) {
                    imImage.image = image;
                    imImage.pb.Image = image;
                } else {
                    var pb = new PictureBox();
                    pb.Click += new System.EventHandler(this.uiILImageClicked);
                    pb.Image = image;
                    pb.Padding = new Padding(PB_PADDING);
                    pb.BorderStyle = BorderStyle.None;
                    var iv = new ImageVar(image, folder.imageLibrary, pb);
                    iv.imageName = name;
                    folder.imageLibrary.imageVars.Add(iv);
                    folder.imageLibrary.pb2ImageVar[pb] = iv;
                    uiImageLibrary.Controls.Add(pb);
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // putImage
        #endregion

        #region Refresh / Update control states
        internal bool IsStmtLabelUnique(string name) {
            dtbegin(funcName());
            int index = uiStmtList.FindStringExact(name);
            return dtend(index == -1);
        } // function IsStmtLabelUnique
        internal void updateUIStates() {
            dtbegin(funcName());
            try {
                uiNewFuncButton.Enabled = currProg != null;
                uiDelFuncButton.Enabled = currProg != null;
                uiFunctionRun.Enabled = currProg != null;
                uiFuncList.Enabled = currProg != null;
                uiProgList.Enabled = uiProgList.Items.Count != 0;
                uiDelProgButton.Enabled = uiProgList.Items.Count != 0;
                if (programState == PROGRAM_STATE.RUNNING || programState == PROGRAM_STATE.STEPINST) {
                    uiImageLibrary.Visible = false;
                } else {
                    uiImageLibrary.Visible = true;
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function updateGroupBoxVisibility
        internal void updateTargetImage(ref PictureBox image, Bitmap inBitmap, string offset) {
            dtbegin(funcName());
            try {
                Bitmap bitmap = new Bitmap(inBitmap);
                using (var graphics = Graphics.FromImage(bitmap)) {
                    Pen pen = new Pen(Color.Red, 1);
                    int x = bitmap.Width / 2;
                    int y = bitmap.Height / 2;
                    if (offset != null && offset != "") {
                        var parts = offset.Split(',');
                        x = int.Parse(parts[0]);
                        y = int.Parse(parts[1]);
                    } // if
                    graphics.DrawLine(pen, x, 0, x, bitmap.Height);
                    graphics.DrawLine(pen, 0, y, bitmap.Width, y);
                } // using graphics
                image.Image = (Image)bitmap;
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function updateTargetImage
        internal void showStmts() {
            dtbegin(funcName());
            try {
                if (currFunc != null) {
                    uiStmtList.Items.Clear();
                    foreach (Statement stmt in currFunc.stmtList) {
                        uiStmtList.Items.Add(stmt.text);
                    } // foreach 
                    uiStmtList.SelectedIndex = uiStmtList.Items.Count == 0 ? -1 : 0;
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function showStmts
        internal void showInsts() {
            dtbegin(funcName());
            try { 
                if (currStmt != null) {
                    uiInstList.Items.Clear();
                    foreach (Instruction inst in currStmt.instructions) {
                        uiInstList.Items.Add(inst.text);
                    } // foreach 
                    uiInstList.SelectedIndex = uiInstList.Items.Count == 0 ? -1 : 0;
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function showInsts
        internal void updateStmtUI(string name, int uiInstIndex = -1) {
            dtbegin(funcName());
            try { 
                if (currProg == null || currFunc == null) { }
                else {
                    selectUpdateEnable(uiInstList, false); 
                    foreach (Statement stmt in currFunc.stmtList) {
                        if (stmt.stmtLabel == name || stmt.text == name) {
                            uiInstList.Items.Clear();
                            foreach (dynamic inst in stmt.instructions) {
                                uiInstList.Items.Add(inst.text);
                            } // foreach
                            if (!progRunning && uiInstList.SelectedIndex != -1) {
                                uiBBCondList.Text = currInst.instCondition.ToString();
                            } // if
                            break;
                        } // if
                    } // foreach
                    selectUpdateEnable(uiInstList, true); 
                    if (uiInstIndex != -1) {
                        uiInstList.SelectedIndex = uiInstIndex;
                    } else {
                        uiInstList.SelectedIndex = uiInstList.Items.Count - 1;
                    } // if
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function updateStmtUI
        #endregion

        #region Execution related codes
        internal void uiProgRunClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            try { 
                if (uiProgRun.Enabled != false) {
                    uiProgRun.Enabled = false;
                    resetRunContext();
                    programState = PROGRAM_STATE.RUNNING;
                    uiFunctionRun.Enabled = !uiFunctionRun.Enabled;
                    uiProgRun.Enabled = !uiProgRun.Enabled;
                    callStack = new Stack<Tuple<FuncName, StmtLabel, RetVal, List<string>>>();
                    foreach (Function func in currProg.funcList) {
                        if (func.funcName == "Main") {
                            uiFuncList.SelectedItem = "Main";
                            //Cursor.Hide();
                            debugOut("Run func: " + currFunc.funcName);
                            if (uiStmtList.Items.Count != 0) {
                                uiStmtList.SelectedIndex = 0;
                                uiInstList.SelectedIndex = uiInstList.Items.Count == 0 ? -1 : 0;
                                debugOut("Run stmt: " + currStmt.stmtLabel);
                                runWorker.RunWorkerAsync(currInst);
                            } else {
                                debugOut("Function is empty");
                            } // if
                            dtend();
                            return;
                        } // if
                    } // foreach
                    uiStatusLabel.Text = "Could not find function 'Main'.";
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function uiProgRunClicked
        internal void updateVariableTableRow(string name, string value, bool isArray, string groups) {
            try {
                foreach (DataGridViewRow row in uiVariablesTable.Rows) {
                    if ((string)row.Cells["Name"].Value == name) {
                        uiVariablesTable.Rows.Remove(row);
                        break;
                    } // if
                } // foreach
                if (uiVariablesTableGroupList.SelectedItem != null && new List<string>(groups.Split(',')).Contains(uiVariablesTableGroupList.SelectedItem.ToString())) {
                    if (isArray == false) {
                        if (runContext.vars.ContainsKey(name)) {
                            uiVariablesTable.Rows.Add(new string[] { name, runContext.vars[name] });
                        } else if (value != "null") {
                            uiVariablesTable.Rows.Add(new string[] { name, value });
                        } // if
                    } else {
                        if (runContext.arrays.ContainsKey(name.Substring(1))) {
                            uiVariablesTable.Rows.Add(new string[] { name, string.Join(",", runContext.arrays[name.Substring(1)]) });
                        } else if (value != "null") {
                            uiVariablesTable.Rows.Add(new string[] { name, value });
                        } // if
                    } // if
                } // if
                uiVariablesTable.Invalidate();
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}";
            } // try
        } // updateVariableTableRow
        internal void uiInstRunButtonClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            try { 
                if (runWorker.IsBusy || programState == PROGRAM_STATE.RUNNING || currProg == null || currFunc == null || currStmt == null || currInst == null) {
                    debugOut("runWorker.IsBusy || programState == PROGRAM_STATE.RUNNING || currProg == null || currFunc == null || currStmt == null || currInst == null");
                    dtend();
                    return;
                } // if
                programState = PROGRAM_STATE.RUNINST;
                uiFunctionRun.Enabled = !uiFunctionRun.Enabled;
                uiProgRun.Enabled = !uiProgRun.Enabled;
                callStack = new Stack<Tuple<FuncName, StmtLabel, RetVal, List<string>>>();
                argStack = new Stack<VarName>();
                uiImageLibrary.Visible = false;
                runWorker.RunWorkerAsync(currInst);
                uiImageLibrary.Visible = true;
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // uiInstRunButtonClicked
        internal void uiFuncRunClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            try { 
                if (programState == PROGRAM_STATE.RUNNING || currProg == null || currFunc == null) {
                    debugOut("programState == PROGRAM_STATE.RUNNING || currProg == null || currFunc == null");
                    dtend();
                    return;
                } // if
                if (uiStmtList.Items.Count == 0) {
                    debugOut("uiStmtList.Items.Count == 0");
                    dtend();
                    return;
                } // if
                resetRunContext();
                programState = PROGRAM_STATE.RUNNING;
                uiFunctionRun.Enabled = !uiFunctionRun.Enabled;
                uiProgRun.Enabled = !uiProgRun.Enabled;
                callStack = new Stack<Tuple<FuncName, StmtLabel, RetVal, List<string>>>();
                argStack = new Stack<VarName>();
                uiStmtList.SelectedIndex = 0;
                uiInstList.SelectedIndex = uiInstList.Items.Count == 0 ? -1 : 0;
                //Cursor.Hide();
                uiStatusLabel.Text = "Program running...";
                debugOut("Run stmt: " + currStmt.text);
                runWorker.RunWorkerAsync(currInst);
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function uiFunctionRunClick
        internal void processCmpImage(string imageName, bool isCopyClipboard, ImageVar _iv = null) {
            dtbegin(funcName());
            Bitmap screenBitmap = null;
            Graphics screenGraphics = null;
            List<ImageVar> ivs = null;
            var fullScreenRect = new Rectangle(0, 0, 1920, 1080);
            try { 
                if (_iv == null) {
                    imageName = substituteVars(imageName);
                    debugOut($"Subsituted image name is {imageName}");
                    ivs = folder.imageLibrary.findImage(imageName);
                } else {
                    ivs = new List<ImageVar>();
                    ivs.Add(_iv);
                } // if
                if (ivs.Count == 0) {
                    runContext.currCond = STMT_CONDITIONS.EQ;
                } else { // check for match if at least one of the images match
                    runContext.currCond = STMT_CONDITIONS.NE;
                    ImageVar iv0 = ivs[0];
                    screenBitmap = new Bitmap(1920, 1080, iv0.image.PixelFormat);
                    screenGraphics = Graphics.FromImage(screenBitmap);
                    screenGraphics.CopyFromScreen(0, 0, 0, 0, new Size(1920, 1080), CopyPixelOperation.SourceCopy);
                    foreach (ImageVar iv in ivs) {
                        if (iv.OCRRectBR != iv.OCRRectTL && iv.isOCRRectBox && isCopyClipboard) {
                            if (processCmpBox(iv, false)) {
                                runContext.currCond = STMT_CONDITIONS.EQ;
                                break;
                            } // if
                        } // if
                        if (iv.OCRRectBR != iv.OCRRectTL) {
                            if (processCmpBox(iv, iv.isOCRRectBox)) {
                                runContext.currCond = STMT_CONDITIONS.EQ;
                                break;
                            } // if
                        } // if
                        if (searchImage(iv)) {
                            runContext.currCond = STMT_CONDITIONS.EQ;
                            break;
                        } // if
                    } // foreach
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            string doOCR(Image<Bgr, Byte> searchImg, ImageVar iv) {
                try {
                    if (iv.OCRParams == null) iv.OCRParams = "1,-1,-1,-1,-1";
                    string[] parameters = iv.OCRParams.Split(',');
                    var ocrImage = searchImg.Clone().Resize(float.Parse(parameters[0]), Inter.Cubic);
                    var ocrImageGray = ocrImage.Convert<Gray, byte>();
                    if (float.Parse(parameters[1]) != -1 && float.Parse(parameters[2]) != -1) {
                        ocrImageGray = ocrImageGray.SmoothBilateral(int.Parse(parameters[1]), int.Parse(parameters[2]), int.Parse(parameters[2]));
                    } // if
                    if (int.Parse(parameters[4]) != -1) {
                        if (int.Parse(parameters[3]) == 1) {
                            ocrImageGray = ocrImageGray.ThresholdBinaryInv(new Gray(int.Parse(parameters[4])), new Gray(255));
                        } else {
                            ocrImageGray = ocrImageGray.ThresholdBinary(new Gray(int.Parse(parameters[4])), new Gray(255));
                        } // if
                    } // if
                    Pix pix = PixConverter.ToPix(ocrImageGray.AsBitmap<Gray, Byte>());
                    Page page = ocrEngine.Process(pix, PageSegMode.SingleLine);
                    string retVal = page.GetText().Replace('\n', ' ');
                    page.Dispose();
                    pix.Dispose();
                    debugOut($"{page.GetMeanConfidence()}@ " + retVal);
                    return retVal;
                } catch (Exception e) {
                    debugOut($"OCR exception: {e.ToString()}");
                } // try
                return "";
            } // doOCR
            bool doSearchXYWH(Image<Bgr, Byte> searchImg, ImageVar iv, Rectangle srcRect, out Point matchLoc) {
                bool retVal = false;
                matchLoc = Point.Empty;
                try { 
                    iv.matchThreshold = iv.matchThreshold.Replace(',', '.'); // TODO: fix culture.
                    float matchTreshold = float.Parse(iv.matchThreshold, CultureInfo.InvariantCulture);
                    runContext.vars["debug-matchTreshold"] = iv.matchThreshold;
                    srcRect.X = Math.Min(1920, Math.Max(0, srcRect.X));
                    srcRect.Y = Math.Min(1080, Math.Max(0, srcRect.Y));
                    srcRect.Width = Math.Min(1920-srcRect.X, Math.Max(0, srcRect.Width));
                    srcRect.Height = Math.Min(1080-srcRect.Y, Math.Max(0, srcRect.Height));
                    Bitmap screenPart = screenBitmap.Clone(srcRect, iv.image.PixelFormat);
                    Image<Bgr, Byte> screenImg = screenPart.ToImage<Bgr, byte>();
                    if (srcRect.X != 1980) runContext.imagesCaptured["debug-search-image"] = screenPart;
                    Image<Gray, float> matchImg = screenImg.MatchTemplate(searchImg, TemplateMatchingType.CcoeffNormed);
                    Image<Gray, float> matchesImg = matchImg.Convert((value) => { return value >= matchTreshold ? 1.0f : 0.0f;  });
                    runContext.imagesCaptured["debug-matchImg"] = matchImg.Convert<Gray,Byte>().ToBitmap();
                    runContext.imagesCaptured["debug-matchesImg"] = matchesImg.Convert<Gray,Byte>().ToBitmap();
                    matchesImg.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
                    for (int c = 1; c < maxValues.Length; c++) { // select the left most match
                        if (maxLocations[c].X < maxLocations[0].X) {
                            maxLocations[0].X = maxLocations[c].X;
                            maxLocations[0].Y = maxLocations[c].Y;
                            maxValues[0] = maxValues[c];
                        } // if
                    } // for
                    runContext.maxValue = maxValues[0];
                    if (maxValues[0] >= matchTreshold) {
                        debugOut($"runLoop match val: {maxValues[0]}");
                        retVal = true;
                        matchLoc = new Point(maxLocations[0].X + srcRect.X, maxLocations[0].Y + srcRect.Y);
                        runContext.actionOffsetPt.X = iv.actionOffsetPt.X;
                        runContext.actionOffsetPt.Y = iv.actionOffsetPt.Y;
                    } // if
                } catch (Exception ex) {
                    uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
                } // try
                return retVal;
            } // function doSearchXYWH
            bool searchImage(ImageVar iv) {
                dtbegin(funcName());
                bool retVal = false;
                try {
                    Bitmap searchBitmap = new Bitmap(iv.image);
                    if (iv.searchRectBR != iv.searchRectTL) {
                        int top = iv.searchRectTL.Y;
                        int left = iv.searchRectTL.X;
                        int bottom = iv.searchRectBR.Y;
                        int right = iv.searchRectBR.X;
                        if (left > right) (left, right) = (right, left);
                        if (top > bottom) (top, bottom) = (bottom, top);
                        var rect = new Rectangle(left, top, Math.Abs(left - right), Math.Abs(top - bottom));
                        rect.Width = Math.Min(rect.Width, iv.image.Width - rect.Left);
                        rect.Height= Math.Min(rect.Height, iv.image.Height - rect.Top);
                        searchBitmap = searchBitmap.Clone(rect, iv.image.PixelFormat);
                    } // if
                    Image<Bgr, Byte> searchImg = searchBitmap.ToImage<Bgr, byte>();
                    runContext.imagesCaptured["debug-processCmpImage"] = searchBitmap;
                    retVal = doSearchXYWH(searchImg, iv, fullScreenRect, out runContext.screenMatchLoc);
                    debugOut($"Compare image match: {runContext.currCond}");
                } catch (Exception ex) {
                    uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
                } // try
                dtend();
                return retVal;
            } // searchImage
            bool processCmpBox(ImageVar iv, bool isCopy = false) {
                dtbegin(funcName());
                bool imgMatched = false;
                try {
                    Bitmap searchBitmap = new Bitmap(iv.image);
                    Bitmap searchBitmapLeft = new Bitmap(iv.image);
                    Bitmap searchBitmapRight = new Bitmap(iv.image);
                    Bitmap searchBitmapOCRBox = new Bitmap(iv.image);
                    if (iv.OCRRectBR == iv.OCRRectTL) {
                        debugOut($"Compare image match: {runContext.currCond}");
                        dtend();
                        return false;
                    } // if
                    if (iv.OCRRectTL.X > iv.OCRRectBR.X) (iv.OCRRectTL.X, iv.OCRRectBR.X) = (iv.OCRRectBR.X, iv.OCRRectTL.X);
                    if (iv.OCRRectTL.Y > iv.OCRRectBR.Y) (iv.OCRRectTL.Y, iv.OCRRectBR.Y) = (iv.OCRRectBR.Y, iv.OCRRectTL.Y);
                    int ocrBoxTop = iv.OCRRectTL.Y;
                    int ocrBoxLeft = iv.OCRRectTL.X;
                    int ocrBoxBottom = iv.OCRRectBR.Y;
                    int ocrBoxRight = iv.OCRRectBR.X;
                    var rectLeftOfOCRBox = new Rectangle(0, 0, ocrBoxLeft, iv.image.Height);
                    var rectRightOfOCRBox = new Rectangle(ocrBoxRight, 0, iv.image.Width - ocrBoxRight, iv.image.Height);
                    rectLeftOfOCRBox.Width = Math.Min(rectLeftOfOCRBox.Width, iv.image.Width);
                    rectLeftOfOCRBox.Height= Math.Min(rectLeftOfOCRBox.Height, iv.image.Height);
                    rectRightOfOCRBox.Width = Math.Min(rectRightOfOCRBox.Width, iv.image.Width);
                    rectRightOfOCRBox.Height= Math.Min(rectRightOfOCRBox.Height, iv.image.Height);
                    searchBitmapLeft = searchBitmap.Clone(rectLeftOfOCRBox, iv.image.PixelFormat);
                    searchBitmapRight = searchBitmap.Clone(rectRightOfOCRBox, iv.image.PixelFormat);
                    runContext.imagesCaptured["debug-processCmpBox-left"] = searchBitmapLeft;
                    runContext.imagesCaptured["debug-processCmpBox-right"] = searchBitmapRight;
                    Image<Bgr, Byte> searchImg = searchBitmapLeft.ToImage<Bgr, byte>();
                    var screenLeftMatchPt = Point.Empty;
                    imgMatched = doSearchXYWH(searchImg, iv, fullScreenRect, out screenLeftMatchPt);
                    if (!isCopy) {
                        updateVariableTableRow("ocrOut", "??", false, "Debug,All");
                    } // if
                    if (imgMatched) { // search right image
                        Image<Bgr, Byte> searchImgRight = searchBitmapRight.ToImage<Bgr, byte>();
                        Rectangle srcSearchRect = new Rectangle(
                            /* x */ screenLeftMatchPt.X + searchBitmapLeft.Width,
                            /* y */ screenLeftMatchPt.Y,
                            /* w */ 1920-screenLeftMatchPt.X - searchBitmapLeft.Width,
                            /* h */ iv.image.Height);
                        var screenRightMatchPt = Point.Empty;
                        imgMatched = doSearchXYWH(searchImgRight, iv, srcSearchRect, out screenRightMatchPt);
                        runContext.matchSize.Width = -1;
                        runContext.matchSize.Height = -1;
                        if (imgMatched) { // create Box image and do OCR
                            var rectOCRBox = new Rectangle();
                            rectOCRBox.X = screenLeftMatchPt.X + searchBitmapLeft.Width;
                            rectOCRBox.Y = screenLeftMatchPt.Y + ocrBoxTop;
                            rectOCRBox.Width = screenRightMatchPt.X-(screenLeftMatchPt.X + searchBitmapLeft.Width);
                            rectOCRBox.Height = Math.Abs(iv.OCRRectTL.Y - iv.OCRRectBR.Y);
                            debugOut($"OCRBox: {rectOCRBox.X},{rectOCRBox.Y},{rectOCRBox.Width},{rectOCRBox.Height}");
                            runContext.matchSize.Width = rectOCRBox.Width;
                            runContext.matchSize.Height = rectOCRBox.Height;
                            if (!isCopy) {
                                searchBitmapOCRBox = screenBitmap.Clone(rectOCRBox, iv.image.PixelFormat);
                                Image<Bgr, Byte> searchBitmapOCRBoxImage = searchBitmapOCRBox.ToImage<Bgr, byte>();
                                runContext.vars["ocrOut"] = doOCR(searchBitmapOCRBoxImage, iv);
                                runContext.imagesCaptured["debug-processCmpBox-ocr"] = searchBitmapOCRBox;
                                updateVariableTableRow("ocrOut", runContext.vars["ocrOut"], false, "Debug,All");
                                debugOut($"OCR result = {runContext.vars["ocrOut"]}");
                            } else {
                                runContext.copyClipboardRect = rectOCRBox;
                            } // if
                        } // if
                    } // if
                    debugOut($"Compare box match: {runContext.currCond}");
                } catch (Exception ex) {
                    uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
                } // try
                dtend();
                return imgMatched;
            } // processCmpBox
            dtend();
        } // function processCmpImage
        internal string substituteVars(string expression)
        { // search for {var-name} and replace it with its value
            dtbegin(funcName());
            try { 
                debugOut($"expression={expression}");
                { // replace escape sequences
                    expression = Regex.Replace(expression, "{{", "Ö"); // TODO: fix this
                    expression = Regex.Replace(expression, "}}", "Ğ"); // TODO: fix this
                } // block
                var reString = "{.*?}";
                Match m = Regex.Match(expression, reString);
                while (m.Success) {
                    var variableName = m.Value.Substring(1, m.Value.Length - 2);
                    var colonIdx = variableName.IndexOf(':');
                    var insertIdx = variableName.IndexOf('<');
                    var removeIdx = variableName.IndexOf('>');
                    var numMatches = colonIdx != -1 ? 1 : 0;
                    numMatches = insertIdx != -1 ? numMatches + 1 : numMatches;
                    numMatches = removeIdx != -1 ? numMatches + 1 : numMatches;
                    if (insertIdx != -1) {
                        uiStatusLabel.Text = "Error: < cannot be used in variables.";
                        debugOut(uiStatusLabel.Text);
                        programState = PROGRAM_STATE.STOP;
                        break;
                    } // if
                    if (numMatches > 1) {
                        uiStatusLabel.Text = "Error: Only  one of :,<,> can be used.";
                        debugOut(uiStatusLabel.Text);
                        programState = PROGRAM_STATE.STOP;
                        break;
                    } // if
                    int paramIdx = -1;
                    if (variableName.StartsWith("p") && int.TryParse(variableName.Substring(1), out paramIdx)) {
                        List<string> currParams = null;
                        if (callStack.Count != 0) currParams = callStack.Peek().Item4;
                        if (currParams != null && paramIdx >=0 && paramIdx < currParams.Count) {
                            expression = Regex.Replace(expression, "{p" + paramIdx + "}", currParams[paramIdx]);
                            m = Regex.Match(expression, reString);
                        } else {
                            if (runContext.vars.ContainsKey($"p{paramIdx}")) {
                                expression = Regex.Replace(expression, "{p" + paramIdx + "}", runContext.vars[$"p{paramIdx}"]);
                                m = Regex.Match(expression, reString);
                            }  else { 
                                if (currParams == null) {
                                    uiStatusLabel.Text = $"Error: no params in stack using pn=<null> as value.";
                                } else {
                                    uiStatusLabel.Text = $"Error: parameter index {paramIdx} is out of range, using <null> value.";
                                } // if
                                debugOut(uiStatusLabel.Text);
                                expression = Regex.Replace(expression, "{p" + paramIdx + "}", "<null>");
                                m = Regex.Match(expression, reString);
                            } // if
                        } // if
                    } else if (variableName == "comma") {
                        debugOut($"comma matched");
                        expression = Regex.Replace(expression, "{comma}", ",");
                        m = Regex.Match(expression, reString);
                    } else if (variableName == "at") {
                        debugOut($"at@ matched");
                        expression = Regex.Replace(expression, "{at}", "@");
                        m = Regex.Match(expression, reString);
                    } else if (variableName == "clipboard") {
                        debugOut($"clipboard matched");
                        var clipboard = "<null>";
                        if (runContext.clipboard != null) clipboard = runContext.clipboard;
                        expression = Regex.Replace(expression, "{clipboard}", clipboard);
                        m = Regex.Match(expression, reString);
                    } else if (variableName == "clipboard-text") {
                        debugOut($"clipboard-text matched");
                        var clipboard = "";
                        try {
                            if (Clipboard.ContainsText()) clipboard = Clipboard.GetText();
                        } catch (Exception) { }
                        expression = Regex.Replace(expression, "{clipboard-text}", clipboard);
                        m = Regex.Match(expression, reString);
                    } else if (runContext.vars.ContainsKey(variableName)) {
                        debugOut($"a variable is matched = {variableName}");
                        string value = runContext.vars[variableName];
                        var regex = new Regex("{" + variableName + "}");
                        expression = regex.Replace(expression, value, 1);
                        m = Regex.Match(expression, reString);
                    } else if (variableName[0] == '@' && numMatches == 0 && runContext.arrays.ContainsKey(variableName.Substring(1))) {
                        var arrName = variableName.Substring(1);
                        debugOut($"an array is matched = {arrName}");
                        var value = "";
                        foreach (var elem in runContext.arrays[variableName.Substring(1)]) {
                            value += elem + ",";
                        } // foreach
                        value = value.Remove(value.Length - 1, 1);
                        var regex = new Regex("{" + variableName + "}");
                        expression = regex.Replace(expression, value, 1);
                        m = Regex.Match(expression, reString);
                    } else if (variableName[0] == '@' && numMatches == 1) {
                        var operatorIdx = colonIdx * insertIdx * removeIdx;
                        // @arr[<>:]idx|front|back = case
                        var arrName = variableName.Substring(1, operatorIdx - 1);
                        var indexVar = variableName.Substring(operatorIdx + 1, variableName.Length - operatorIdx - 1);
                        debugOut($"an array is matched with operator {arrName}");
                        if (!runContext.arrays.ContainsKey(arrName)) {
                            uiStatusLabel.Text = $"Error: array '{arrName}' not found @currInst?";
                            debugOut(uiStatusLabel.Text);
                            programState = PROGRAM_STATE.STOP;
                            break;
                        } // if
                        if (variableName.EndsWith("len")) {
                            debugOut($"array length is matched");
                            var length = $"{runContext.arrays[arrName].Count}";
                            expression = Regex.Replace(expression, "{.*?:len}", length);
                            m = Regex.Match(expression, reString);
                            continue;
                        } else if (variableName.EndsWith("back")) {
                            debugOut($"array back is matched");
                            indexVar = $"{runContext.arrays[arrName].Count - 1}";
                        } else if (variableName.EndsWith("front")) {
                            debugOut($"array front is matched");
                            indexVar = $"{0}";
                        } // if
                        var elemIdx = int.Parse(substituteVars(indexVar));
                        if (elemIdx < 0 || elemIdx >= runContext.arrays[arrName].Count) {
                            uiStatusLabel.Text = $"Error: array index out of range @currInst?";
                            debugOut(uiStatusLabel.Text);
                            programState = PROGRAM_STATE.STOP;
                            break;
                        } // if
                        var value = runContext.arrays[arrName][elemIdx];
                        if (removeIdx != -1) { // remove the element at elemIdx
                            debugOut($"array removing element at {elemIdx}");
                            runContext.arrays[arrName].RemoveAt(elemIdx);
                        } // if
                        expression = Regex.Replace(expression, "{.*?}", value);
                        m = Regex.Match(expression, reString);
                    } else {
                        debugOut($"Variable {variableName} not found value is <null>.");
                        expression = Regex.Replace(expression, "{.*?}", "<null>");
                        m = Regex.Match(expression, reString);
                    } // if
                } // while
                { // replace back escape sequences
                    expression = Regex.Replace(expression, "Ö", "{"); // TODO: fix this
                    expression = Regex.Replace(expression, "Ğ", "}"); // TODO: fix this
                } // block
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
            return expression;
        } // substituteVars
        internal void runInst(object sender, DoWorkEventArgs f) {
            dtbegin(funcName());
            try { 
                Instruction currInst = (Instruction)f.Argument;
                debugOut($"runInst begin@ inst: {currInst?.text}");
                runContext.imagesCaptured.Clear();
                if (currInst != null && currInst.isCmpImgInst) {
                    if (!currInst.isCommentedOut) {
                        processCmpImage(currInst.instParamLeft, currInst?.instructionType == InstructionType.SAT_CMPBOXCOPY);
                    } // if
                } // if
                debugOut($"runInst end");
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function runInst
        public void runInstFinished(object sender, RunWorkerCompletedEventArgs e) {
            dtbegin(funcName());
            try { 
                //    debugOut("Compare image match: " + currCond + " @ x=" + currStmt.cmpInstruction.screenLoc.X.ToString() +
                //                ", y=" + currStmt.cmpInstruction.screenLoc.Y.ToString());
                updateVariableTableRow("screenMatchLoc", $"{runContext.screenMatchLoc.X}, {runContext.screenMatchLoc.Y}", false, "Debug");
                updateVariableTableRow("screenMatchSize", $"{runContext.matchSize.Width}, {runContext.matchSize.Height}", false, "Debug");
                foreach (var item in runContext.imagesCaptured) {
                    putImage(item.Key, new Bitmap (item.Value));
                } // foreach
                bool shouldStop = programState == PROGRAM_STATE.STOP;
                if (currInst != null && !currInst.isCommentedOut && currInst.isCmpImgInst && programState == PROGRAM_STATE.RUNINST) { 
                    shouldStop = true;
                } // if
                if (shouldStop) {
                    if (uiInstList.SelectedIndex != uiInstList.Items.Count - 1) { 
                        uiInstList.SelectedIndex++;
                    } // if
                    debugOut("Program being stopped." + Environment.NewLine);
                    uiStatusLabel.Text = "Program stopped.";
                    uiProgRun.Enabled = true;
                    uiFunctionRun.Enabled = true;
                    uiProgRun.Enabled = true;
                    dtend();
                    return;
                } // if
                if (currFunc != null && currStmt != null) {
                    while (currInst != null) {
                        bool condOk = runContext.currCond == STMT_CONDITIONS.LT && currInst.instCondition == STMT_CONDITIONS.LE;
                        condOk |= runContext.currCond == STMT_CONDITIONS.EQ && currInst.instCondition == STMT_CONDITIONS.LE;
                        condOk |= runContext.currCond == STMT_CONDITIONS.EQ && currInst.instCondition == STMT_CONDITIONS.GE;
                        condOk |= runContext.currCond == STMT_CONDITIONS.GT && currInst.instCondition == STMT_CONDITIONS.GE;
                        condOk |= runContext.currCond == STMT_CONDITIONS.GT && currInst.instCondition == STMT_CONDITIONS.NE;
                        condOk |= runContext.currCond == STMT_CONDITIONS.LT && currInst.instCondition == STMT_CONDITIONS.NE;
                        if (!currInst.isCommentedOut) { 
                            if (condOk || currInst.instCondition == runContext.currCond || currInst.instCondition == STMT_CONDITIONS.AL) {
                                debugOut("---- Inst: " + currInst.ToString());
                                if (!currInst.ExecInstContinue(currStmt)) {
                                    // runFinished();
                                    dtend();
                                    return;
                                } // if
                                if (currInst.isCallOrJumpInst()) {
                                    dtend();
                                    return;
                                } // if
                            } // if
                        } // if
                        if (uiInstList.SelectedIndex == uiInstList.Items.Count - 1) break;
                        uiInstList.SelectedIndex++;
                        if (programState == PROGRAM_STATE.STEPINST) {
                            dtend();
                            return;
                        } // if
                        if (programState == PROGRAM_STATE.RUNINST) {
                            programState = PROGRAM_STATE.STOP;
                            if (callStack.Count != 0) {
                                var caller = callStack.Pop();
                                runContext.vars["retVal"] = caller.Item3;
                                uiFuncList.Text = caller.Item1;
                                updateStmtUI(caller.Item1);
                                uiStmtList.Text = caller.Item2;
                            } // if
                            uiProgRun.Enabled = true;
                            uiFunctionRun.Enabled = true;
                            uiProgRun.Enabled = true;
                            if (runContext.vars["retVal"] != null) {
                                updateVariableTableRow($"retVal", runContext.vars["retVal"], false, "All,Debug");
                            } // if
                            uiStatusLabel.Text = "Program stopped.";
                            return;
                        } // if
                    } // while
                    if (uiStmtList.SelectedIndex != uiStmtList.Items.Count - 1)
                    { // fallthrough case, run next statement
                        runNextStatement();
                    } else {
                        runFinished();
                    } // if
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function runInstFinished
        internal void runNextStatement() {
            dtbegin(funcName());
            try {
                if (uiStmtList.SelectedIndex != uiStmtList.Items.Count - 1) {
                    uiStmtList.SelectedIndex = uiStmtList.SelectedIndex + 1;
                    if (currStmt.isCommentedOut) {
                        runNextStatement();
                        dtend();
                        return;
                    } // if
                    uiInstList.SelectedIndex = uiInstList.Items.Count == 0 ? -1 : 0;
                    debugOut("-- stmt:" + currStmt.stmtLabel);
                    runWorker.RunWorkerAsync(currInst);
                } else {
                    runFinished();
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function runNextStatement
        internal void runFinished() {
            dtbegin(funcName());
            try { 
                if (callStack.Count != 0) { // return to caller
                    var caller = callStack.Pop();
                    runContext.vars["retVal"] = caller.Item3;
                    uiFuncList.Text = caller.Item1;
                    updateStmtUI(caller.Item1);
                    uiStmtList.Text = caller.Item2;
                    if (runContext.vars.ContainsKey("retVal")) {
                        updateVariableTableRow($"retVal", runContext.vars["retVal"], false, "All,Debug");
                    } // if
                    debugOut("Continuing at the caller: f@" + currFunc.funcName +", s@"+ currStmt.stmtLabel);
                    runNextStatement();
                    dtend();
                    return;
                } // if
                uiStatusLabel.Text = "Run Finished!";
                uiProgRun.Enabled = true;
                programState = PROGRAM_STATE.STOP;
                uiFunctionRun.Enabled = !uiFunctionRun.Enabled;
                uiProgRun.Enabled = !uiProgRun.Enabled;
                // Cursor.Show();
                // TODO: continue looping until there is a match on the screen.
                updateUIStates();
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function runFinished
        #endregion

        #region Button actions
        internal void uiProgDelClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            var index = uiProgList.SelectedIndex;
            if (uiProgList.SelectedIndex != -1) {
                string currName = uiProgList.SelectedItem.ToString();
                folder.progList.RemoveAt(folder.progList.FindIndex(item => item.progName == currName));
                uiProgList.Items.RemoveAt(uiProgList.SelectedIndex);
                if (uiProgList.Items.Count > 0) {
                    uiProgList.SelectedIndex = 0;
                } else {
                    uiProgList.Text = "";
                    uiFuncList.Text = "";
                } // if
            } // if
            updateUIStates();
            dtend();
        } // function uiProgDelClicked
        internal void uiProgNewClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            int count = 1;
            while (uiProgList.FindStringExact("New App " + count) != -1) {
                count++;
            } // while
            var prog = new global::Script("New App " + count);
            uiProgList.Items.Add(prog.progName);
            folder.progList.Add(prog);
            uiFuncList.Items.Clear();
            uiFuncList.Text = "";
            foreach (Function func in prog.funcList) {
                uiFuncList.Items.Add(func.funcName);
            } // foreach
            uiFuncList.SelectedIndex = uiFuncList.Items.Count - 1;
            uiProgList.SelectedIndex = uiProgList.Items.Count - 1;
            updateUIStates();
            dtend();
        } // function uiProgNewClicked
        internal void uiProgStopClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            programState = PROGRAM_STATE.STOP;
            uiFunctionRun.Enabled = !uiFunctionRun.Enabled;
            uiProgRun.Enabled = !uiProgRun.Enabled;
            dtend();
        } // function uiProgStopClicked
        internal void uiFuncNewClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            if (currProg != null) { 
                int count = 1;
                while (uiFuncList.FindStringExact("New Function " + count) != -1) {
                    count++;
                } // while
                var func = new Function();
                func.funcName = "New Function " + count;
                uiFuncList.Items.Add(func.funcName);
                currProg.funcList.Add(func);
                uiFuncList.SelectedIndex = uiFuncList.Items.Count - 1;
                updateUIStates();
            } // if
            dtend();
        } // function uiFuncNewClicked
        internal void uiFuncDelClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            var index = uiFuncList.SelectedIndex;
            if (uiFuncList.SelectedIndex != -1) {
                string currName = uiFuncList.SelectedItem.ToString();
                currProg.funcList.RemoveAt(currProg.funcList.FindIndex(func => func.funcName == currName));
                uiFuncList.Items.RemoveAt(uiFuncList.SelectedIndex);
                if (uiFuncList.Items.Count > 0) {
                    uiFuncList.SelectedIndex = Math.Min(index, uiFuncList.Items.Count - 1);
                } else {
                    uiFuncNewClicked(null, null);
                } // if
            } // if
            updateUIStates();
            dtend();
        } // function uiFuncDelClicked
        internal void uiFuncDuplicateClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            if (currFunc != null) {
                Function func = (Function)currFunc.Clone();
                int count = 1;
                while (uiFuncList.FindStringExact($"{func.funcName}-clone-{count}") != -1) {
                    count++;
                } // while
                func.funcName = $"{func.funcName}-clone-{count}";
                currProg.funcList.Add(func);
                uiFuncList.Items.Add(func.funcName);
                uiFuncList.SelectedItem = func.funcName;
            } // if
            dtend();
        } // function uiFuncDuplicateClicked
        internal void uiStmtAddClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            if (currFunc == null) { 
                dtend();
                return;
            } // if
            int statementCount = 1;
            while (!IsStmtLabelUnique("New Statement " + statementCount)) {
                statementCount++;
            } // while
            string name = "New Statement " + statementCount;
            selectUpdateEnable(uiInstList, false);
            selectUpdateEnable(uiStmtList, false);
            uiStmtList.Items.Add(name);
            uiStmtList.SelectedIndex = uiStmtList.Items.Count-1;
            Statement statement = new Statement(name);
            statement.stmtLabel = name;
            currFunc.stmtList.Add(statement);
            uiInstList.Items.Clear();
            selectUpdateEnable(uiStmtList, true);
            selectUpdateEnable(uiInstList, true);
            updateUIStates();
            dtend();
        } // function uiStmtAddClicked
        internal void uiStmtDelClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            var index = uiStmtList.SelectedIndex;
            if (uiStmtList.SelectedIndex != -1) {
                string currStamentLabel = uiStmtList.SelectedItem.ToString();
                currFunc.stmtList.RemoveAt(currFunc.stmtList.FindIndex(statement => statement.text == currStamentLabel));
                uiStmtList.Items.RemoveAt(uiStmtList.SelectedIndex);
                if (uiStmtList.Items.Count > 0) {
                    uiStmtList.SelectedIndex = Math.Min(index, uiStmtList.Items.Count - 1);
                } // if
                updateUIStates();
            } // if
            dtend();
        } // function uiStmtDelClicked
        internal void uiStmtMoveDownClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            if (uiStmtList.SelectedIndex > -1 && uiStmtList.SelectedIndex <= uiStmtList.Items.Count - 2) {
                var index = uiStmtList.SelectedIndex;
                var temp = uiStmtList.Items[index];
                uiStmtList.Items.RemoveAt(index);
                uiStmtList.Items.Insert(index + 1, temp);
                uiStmtList.SelectedIndex = index + 1;
                var temp2 = currFunc.stmtList[index];
                currFunc.stmtList[index] = currFunc.stmtList[index + 1];
                currFunc.stmtList[index + 1] = temp2;
            } // if
            dtend();
        } // function uiStmtMoveDownClicked
        internal void uiStmtMoveUpClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            if (uiStmtList.SelectedIndex > 0) {
                var index = uiStmtList.SelectedIndex;
                var temp = uiStmtList.Items[index];
                uiStmtList.Items.RemoveAt(index);
                uiStmtList.Items.Insert(index - 1, temp);
                uiStmtList.SelectedIndex = index - 1;
                var temp2 = currFunc.stmtList[index];
                currFunc.stmtList[index] = currFunc.stmtList[index - 1];
                currFunc.stmtList[index - 1] = temp2;
            } // if
            dtend();
        } // function uiStmtMoveUpClicked
        internal void uiStmtDuplicateClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            Statement newStmt = (Statement)currStmt.Clone();
            int count = 1;
            string currName = newStmt.stmtLabel;
            while (!IsStmtLabelUnique($"{currName}-clone-{count}")) count++;
            string cloneName = $"{currName}-clone-{count}";
            newStmt.stmtLabel = cloneName;
            currFunc.stmtList.Insert(uiStmtList.SelectedIndex + 1, newStmt);
            uiStmtList.Items.Insert(uiStmtList.SelectedIndex + 1, cloneName);
            uiStmtList.SelectedIndex = uiStmtList.SelectedIndex + 1;
            dtend();
        } // function uiStmtDuplicateClicked
        internal void uiInstListItemClicked(object sender, MouseEventArgs e) {
            dtbegin(funcName());
            int index = uiInstList.IndexFromPoint(e.Location);
            if (uiInstList.SelectedIndex == index && e.Button == MouseButtons.Right) {
                uiInstList.SelectedIndex = -1;
            } // if
            dtend();
        } // function uiInstListItemClicked
        internal void uiInstMoveUpClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            if (currInst.isCallInst()) {
                dtend();
                return; 
            } // if
            if (currInst.isJumpInst()) {
                if (prevInst != null && prevInst.isCallInst()) {
                    dtend();
                    return; 
                } // if
            } else if (prevInst != null && prevInst.isCallOrJumpInst()) {
                dtend();
                return; 
            } // if
            if (uiInstList.SelectedIndex > 0) {
                var index = uiInstList.SelectedIndex;
                var temp2 = currStmt.instructions[index];
                currStmt.instructions[index] = currStmt.instructions[index - 1];
                currStmt.instructions[index - 1] = temp2;
                uiInstList.Items.Clear();
                System.Object[] ItemObject = new System.Object[currStmt.instructions.Count];
                for (int c = 0; c < currStmt.instructions.Count; c ++) {
                    ItemObject[c] = currStmt.instructions[c].text;
                } // for
                uiInstList.Items.AddRange(ItemObject);
                uiInstList.SelectedIndex = index - 1;
            } // if
            dtend();
        } // function uiInstMoveUpClicked
        internal void uiInstMoveDownClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            if (currInst.isCallInst()) {
                dtend();
                return;
            } else if (currInst.isJumpInst()) {
                if (nextInst != null && nextInst.isCallInst()) {
                    dtend();
                    return; 
                }
            } else if (nextInst != null && nextInst.isCallOrJumpInst()) {
                dtend();
                return;
            } // if
            if (uiInstList.SelectedIndex > -1 && uiInstList.SelectedIndex <= uiInstList.Items.Count - 2) {
                var index = uiInstList.SelectedIndex;
                var temp2 = currStmt.instructions[index];
                currStmt.instructions[index] = currStmt.instructions[index + 1];
                currStmt.instructions[index + 1] = temp2;
                uiInstList.Items.Clear();
                System.Object[] ItemObject = new System.Object[currStmt.instructions.Count];
                for (int c = 0; c < currStmt.instructions.Count; c++) {
                    ItemObject[c] = currStmt.instructions[c].text;
                } // for
                uiInstList.Items.AddRange(ItemObject);
                uiInstList.SelectedIndex = index + 1;
            } // if
            dtend();
        } // function uiInstMoveDownClicked
        internal void uiInstAddClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            Instruction inst = null;
            bool insertAtEnd = true;
            STMT_CONDITIONS instCond = STMT_CONDITIONS.AL;
            if (uiBBCondList.Text != "") {
                instCond = (STMT_CONDITIONS)Enum.Parse(typeof(STMT_CONDITIONS), uiBBCondList.Text); 
            } else {
                uiBBCondList.Text = "AL";
            } // if
            if (currStmt.instructions.Count != 0) {
                if (lastInst.isCallInst()) {
                    insertAtEnd = false;
                } // if
                if (lastInst.isJumpInst()) {
                    if (uiInstSelect.Text != "Call Func" && uiInstSelect.Text != "Jump to Stmt")
                        insertAtEnd = false;
                } // if
            } // if
            if (uiInstSelect.Text == "Mouse") {
                switch (uiInstParams.Text) {
                    case "Left Click": inst = new Instruction(InstructionType.SAT_LCLICK, uiInstParams.Text, instCond); break;
                    case "Right Click": inst = new Instruction(InstructionType.SAT_RCLICK, uiInstParams.Text, instCond); break;
                    case "Left Down": inst = new Instruction(InstructionType.SAT_LDOWN, uiInstParams.Text, instCond); break;
                    case "Left Up": inst = new Instruction(InstructionType.SAT_LUP, uiInstParams.Text, instCond); break;
                    case "Mouse Move": inst = new Instruction(InstructionType.SAT_MMOVE, uiInstParams.Text, instCond); break;
                    case "Left Double": inst = new Instruction(InstructionType.SAT_DCLICK, uiInstParams.Text, instCond); break;
                    default:
                        dtend();
                        return;
                }
            } else {
                switch (uiInstSelect.Text) {
                    case "Delay": inst = new Instruction(InstructionType.SAT_DELAY, uiInstParams.Text, instCond); break;
                    case "Cmp Image": inst = new Instruction(InstructionType.SAT_CMPIMG, uiInstParams.Text, instCond); break;
                    case "Cmp Box": inst = new Instruction(InstructionType.SAT_CMPBOX, uiInstParams.Text, instCond); break;
                    case "Cmp Box-copy": inst = new Instruction(InstructionType.SAT_CMPBOXCOPY, uiInstParams.Text, instCond); break;
                    case "Cmp Args": inst = new Instruction(InstructionType.SAT_CMPARGS, uiInstParams.Text, instCond, uiInstParamRight.Text); break;
                    case "Jump to Stmt": inst = new Instruction(InstructionType.SAT_JSTMT, uiInstParams.Text, instCond); break;
                    case "Call Func": inst = new Instruction(InstructionType.SAT_CALLF, uiInstParams.Text, instCond, uiInstParamRight.Text); break;
                    case "Key Press": inst = new Instruction(InstructionType.SAT_KEYPRESS, uiInstParams.Text, instCond); break;
                    case "Func Return": inst = new Instruction(InstructionType.SAT_FRETURN, uiInstParams.Text, instCond); break;
                    case "Stop Program": inst = new Instruction(InstructionType.SAT_STOP_PROGRAM, uiInstParams.Text, instCond); break;
                    case "Assign": inst = new Instruction(InstructionType.SAT_ASSIGN, uiInstParams.Text, instCond, uiInstParamRight.Text); break;
                    case "Print": inst = new Instruction(InstructionType.SAT_PRINTLN, uiInstParams.Text, instCond); break;
                    default:
                        dtend();
                        return;
                } // switch
            } // if
            int selectedIdx = uiInstList.SelectedIndex + 1;
            currStmt.instructions.Insert(selectedIdx, inst);
            uiInstList.Items.Insert(selectedIdx, inst.text);
            uiInstList.SelectedIndex ++;
            updateUIStates();
            dtend();
        } // function uiInstAddClicked
        internal void uiInstDelClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            if (uiInstList.SelectedIndex != -1) {
                int index = uiInstList.SelectedIndex;
                currStmt.instructions.RemoveAt(uiInstList.SelectedIndex);
                uiInstList.Items.RemoveAt(uiInstList.SelectedIndex);
                if (index < uiInstList.Items.Count) {
                    uiInstList.SelectedIndex = index;
                } else {
                    uiInstList.SelectedIndex = index-1;
                } // if
            } // if
            updateUIStates();
            dtend();
        } // function uiInstDelClicked
        #endregion

        #region Other functions
        internal void uiTestButton1_Click(object sender, EventArgs e) {
            dtbegin(funcName());
            uiTestTextBox.Text += "Button 1 Clicked\r\n";
            dtend();
        } // function uiTestButton1_Click
        internal void uiTestButton2_Click(object sender, EventArgs e) {
            dtbegin(funcName());
            uiTestTextBox.Text += "Button 2 Clicked\r\n";
            dtend();
        } // function uiTestButton2_Click
        #endregion

        #region TextBox actions
        internal void uiProgListNameChanged(object sender, EventArgs e) {
            dtbegin(funcName());
            if (selectedProgIndex != -1 && uiProgList.Items.Count != 0) {
                uiProgList.Items[selectedProgIndex] = uiProgList.Text;
                folder.progList[selectedProgIndex].progName = uiProgList.Text;
            } // if
            dtend();
        } // function uiProgListNameChanged
        internal void uiFuncListNameChanged(object sender, EventArgs e) {
            dtbegin(funcName());
            if (currProg != null) {
                if (selectedFuncIndex != -1) {
                    var currFuncName = currProg.funcList[selectedFuncIndex].funcName;
                    foreach (var func in currProg.funcList)
                        foreach (var stmt in func.stmtList)
                            foreach (var inst in stmt.instructions)
                                if (inst.isCallInst() && inst.instParamLeft == currFuncName) {
                                    inst.instParamLeft = uiFuncList.Text;
                                } // if
                    uiFuncList.Items[selectedFuncIndex] = uiFuncList.Text;
                    currProg.funcList[selectedFuncIndex].funcName = uiFuncList.Text;
                    updateStmtUI(currStmt?.stmtLabel);
                } // if
            } // if
            dtend();
        } // function uiFuncListNameChanged
        internal void uiStmtNameUpdateClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            if (currStmt != null) {
                if (uiStmtLabel.Text.Length >= 1) {
                    if (IsStmtLabelUnique(uiStmtLabel.Text)) {
                        string currStmtLabel = uiStmtList.SelectedItem.ToString();
                        // update jump targets
                        foreach (var stmt in currFunc.stmtList) {
                            foreach (var inst in stmt.instructions) {
                                if (inst.isJumpInst() && inst.instParamLeft == currStmtLabel) {
                                    inst.instParamLeft = uiStmtLabel.Text;
                                } // if
                            } // foreach
                        } // foreach
                        int index = uiStmtList.FindStringExact(currStmtLabel);
                        uiStmtList.Items.RemoveAt(index);
                        if (currFunc.stmtList[index].isCommentedOut) 
                        uiStmtList.Items.Insert(index, "// " + uiStmtLabel.Text);
                        else uiStmtList.Items.Insert(index, uiStmtLabel.Text);
                        uiStmtList.SelectedIndex = index;
                        currFunc.stmtList[index].stmtLabel = uiStmtLabel.Text;
                        updateStmtUI(currStmt.stmtLabel);
                    } else {
                        uiStatusLabel.Text = "Statement label not unique.";
                    } // if
                } // if
            } // if
            dtend();
        } // function uiStmtNameUpdateClicked
        #endregion

        #region List item selection change events
        internal void uiProgListChanged(object sender, EventArgs e) {
            dtbegin(funcName());
            if (callbackMapStatus[uiProgList])
            {
                selectUpdateEnable(uiProgList, false);
                selectedProgIndex = -1;
                if (uiProgList.SelectedItem != null)
                {
                    selectedProgIndex = uiProgList.SelectedIndex;
                    uiFuncList.Items.Clear();
                    folder.progList[uiProgList.SelectedIndex].funcList.Sort(delegate (Function f0, Function f1) {
                        return f0.funcName.CompareTo(f1.funcName);
                    });
                    foreach (Function func in folder.progList[uiProgList.SelectedIndex].funcList)
                    {
                        uiFuncList.Items.Add(func.funcName);
                    } // foreach
                    uiFuncList.SelectedIndex = uiFuncList.Items.Count - 1;
                    uiFuncListChanged(null, null);
                } // if
                selectUpdateEnable(uiProgList, true);
            } // if
            dtend();
        } // function uiProgListChanged
        internal void uiFuncListChanged(object sender, EventArgs e) {
            dtbegin(funcName());
            if (callbackMapStatus[uiFuncList]) {
                selectUpdateEnable(uiFuncList, false);
                selectedFuncIndex = -1;
                if (uiFuncList.SelectedItem != null){
                    selectedFuncIndex = uiFuncList.SelectedIndex;
                    uiInstList.Items.Clear();
                    if (currFunc != null){
                        uiStmtList.Items.Clear();
                        foreach (Statement stmt in currFunc.stmtList) {
                            uiStmtList.Items.Add(stmt.text);
                        } // foreach
                        if (uiStmtList.Items.Count != 0) {
                            uiStmtList.SelectedIndex = 0;
                        } // if
                        updateUIStates();
                    } // if
                } // if
                updateUIStates();
                selectUpdateEnable(uiFuncList, true);
            } // if
            dtend();
        } // function uiFuncListChanged
        internal void uiStmtListChanged(object sender, EventArgs e) {
            dtbegin(funcName());
            if (callbackMapStatus[uiStmtList]) {
                selectUpdateEnable(uiInstList, false);
                if (uiStmtList.SelectedItem != null) {
                    string currStatementLabel = uiStmtList.SelectedItem.ToString();
                    uiStmtLabel.Text = currStatementLabel;
                    updateStmtUI(currStatementLabel);
                } // if
                updateUIStates();
                selectUpdateEnable(uiInstList, true);
            } // if
            dtend();
        } // function uiStmtListChanged
        internal void uiBBCondChanged(object sender, EventArgs e) {
            dtbegin(funcName());
            if (callbackMapStatus[uiBBCondList]) {
                if (currStmt != null && uiBBCondList.SelectedItem != null && currInst != null) {
                    currInst.instCondition = (STMT_CONDITIONS)Enum.Parse(typeof(STMT_CONDITIONS), uiBBCondList.SelectedItem.ToString());
                    int index = uiInstList.SelectedIndex;
                    selectUpdateEnable(uiInstList, false);
                    uiInstList.Items.RemoveAt(index);
                    uiInstList.Items.Insert(index, currStmt.instructions[index].text);
                    uiInstList.SelectedIndex = index;
                    selectUpdateEnable(uiInstList, true);
                } // if
            } // if
            dtend();
        } // function uiBBCondChanged
        internal void uiInstListChanged(object sender, EventArgs e)
        {
            dtbegin(funcName());
            if (!progRunning && callbackMapStatus[uiInstList]) {
                if (uiInstList.SelectedIndex != -1) {
                    selectUpdateEnable(uiInstParams, false);
                    selectUpdateEnable(uiInstParamRight, false);
                    selectUpdateEnable(uiBBCondList, false);
                    uiBBCondList.Text = currInst.instCondition.ToString();
                    //                uiInstParamRight.SelectedIndexChanged -= uiInstParamSelectChanged;
                    uiInstParams.Text = currInst.instParamLeft;
                    uiInstParamRight.Text = currInst.instParamRight;
                    uiInstParamRight.Enabled = currInst.instName == "Cmp Args" || 
                        currInst.instName == "Call Func" ||
                        currInst.instName == "Assign";
                    if (uiInstSelect.Text == "Mouse") {
                        uiInstParams.Text = currInst.instName;
                    } // if
                    selectUpdateEnable(uiInstParams, true);
                    selectUpdateEnable(uiInstParamRight, true);
                    selectUpdateEnable(uiBBCondList, true);
                    if (currInst.instructionType == InstructionType.SAT_CMPIMG ||
                        currInst.instructionType == InstructionType.SAT_CMPBOX ||
                        currInst.instructionType == InstructionType.SAT_CMPBOXCOPY)
                    syncImageListItem(currInst.instParamLeft);
                    selectUpdateEnable(uiInstSelect, false);
                    uiInstSelect.Text = currInst.selectName;
                    selectUpdateEnable(uiInstSelect, true);
                    uiInstList.Focus();
                } else {
                    uiInstParams.SelectedIndexChanged -= uiInstParamSelectChanged;
                    uiBBCondList.SelectedIndexChanged -= uiBBCondChanged;
                    uiBBCondList.Text = "";
                    uiInstParams.Text = "";
                    uiBBCondList.SelectedIndexChanged += uiBBCondChanged;
                    uiInstParams.SelectedIndexChanged += uiInstParamSelectChanged;
                } // if 
            } // if
            dtend();
        } // function uiInstListChanged
        internal void uiInstParamListShow(object sender, EventArgs e) {
            dtbegin(funcName());
            if (callbackMapStatus[uiInstParams])
            {
                selectUpdateEnable(uiInstParams, false);
                uiInstParams.Items.Clear();
                //            uiInstParams.Text = "";
                if (uiInstSelect.SelectedItem == "Jump to Stmt") {
                    uiInstParams.Items.Add("<next>");
                    uiInstParams.Items.Add("<end>");
                    foreach (Statement stmt in currFunc.stmtList) {
                        uiInstParams.Items.Add(stmt.stmtLabel);
                    } // foreach
                } else if (uiInstSelect.SelectedItem == "Mouse") {
                    uiInstParams.Items.Add("Left Click");
                    uiInstParams.Items.Add("Right Click");
                    uiInstParams.Items.Add("Left Down");
                    uiInstParams.Items.Add("Left Up");
                    uiInstParams.Items.Add("Left Double");
                    uiInstParams.Items.Add("Mouse Move");
                    uiInstParams.Text = "Left Click";
                } else if (uiInstSelect.SelectedItem == "Delay") {
                    uiInstParams.Items.Add("100");
                    uiInstParams.Items.Add("500");
                    uiInstParams.Items.Add("1000");
                    uiInstParams.Items.Add("2000");
                    uiInstParams.Items.Add("5000");
                } else if (uiInstSelect.SelectedItem == "Print") {
                    uiInstParams.Items.Add("{clipboard}");
                    uiInstParams.Items.Add("{retVal}");
                    uiInstParams.Items.Add("{ocrOut}");
                    uiInstParams.Items.Add("{p0}");
                    uiInstParams.Items.Add("{p1}");
                } else if (uiInstSelect.SelectedItem == "Cmp Image") {
                    foreach (ImageVar iv in folder.imageLibrary.imageVars) {
                        if (iv.pb.Visible) {
                            List<string> names = iv.getImageNames;
                            foreach (var ivName in names) {
                                if (uiInstParams.FindStringExact(ivName) == -1) {
                                    uiInstParams.Items.Add(ivName);
                                } // if
                            } // foreach
                        } // if
                    } // foreach
                } else if (uiInstSelect.SelectedItem == "Cmp Box" || 
                    uiInstSelect.SelectedItem == "Cmp Box-copy") {
                    foreach (ImageVar iv in folder.imageLibrary.imageVars) {
                        if (iv.isOCRRectBox) {
                            List<string> names = iv.getImageNames;
                            foreach (var ivName in names)
                            {
                                if (uiInstParams.FindStringExact(ivName) == -1)
                                {
                                    uiInstParams.Items.Add(ivName);
                                } // if
                            } // foreach
                        } // if
                    } // foreach
                } else if (uiInstSelect.SelectedItem == "Call Func") {
                    foreach (Function func in currProg.funcList) {
                        uiInstParams.Items.Add(func.funcName);
                    } // foreach
                } // if
                selectUpdateEnable(uiInstParams, true);
            } // if
            dtend();
        } // function uiInstParamListShow 
        internal void uiInstListSelectChanged(object sender, EventArgs e) {
            dtbegin(funcName());
            if (callbackMapStatus[uiInstSelect])
            {
                selectUpdateEnable(uiInstSelect, false);
                selectUpdateEnable(uiInstParams, false);
                uiInstParamListShow(null, null);
                uiInstParams.SelectedIndex = uiInstParams.Items.Count != 0 ? 0 : -1;
                uiInstParamRight.Enabled = uiInstSelect.Text == "Cmp Args" ||
                    uiInstSelect.Text == "Call Func" ||
                    uiInstSelect.Text == "Assign";
                if (currInst != null) {
                    currInst.instructionType = Instruction.getInstType(uiInstSelect.Text);
                } // if
                if (currStmt != null) {
                    updateStmtUI(currStmt.stmtLabel, uiInstList.SelectedIndex);
                } // if
                selectUpdateEnable(uiInstParams, true);
                selectUpdateEnable(uiInstSelect, true);
            } // if
            dtend();
        } // function uiInstListSelectChanged
        private void uiInstParamSelectChanged(object sender, EventArgs e)
        {
            dtbegin(funcName());
            if (callbackMapStatus[uiInstParams])
            {
                selectUpdateEnable(uiInstParams, false);
                if (uiInstList.SelectedIndex != -1)
                {
                    selectUpdateEnable(uiInstList, false);
                    if (uiInstSelect.Text == "Mouse")
                    {
                        currInst.instructionType = Instruction.getInstType(uiInstParams.Text);
                    }
                    else
                    {
                        currInst.instParamLeft = uiInstParams.Text;
                    } // if
                    updateStmtUI(currStmt.stmtLabel, uiInstList.SelectedIndex);
                    selectUpdateEnable(uiInstList, true);
                } // if
                selectUpdateEnable(uiInstParams, true);
            } // if
            dtend();
        } // function uiInstParamSelectChanged
        #endregion

        #region Variables window functions
        private void uiVariablesTableGroupListSelectChanged(object sender, EventArgs e)
        {
            uiVariableTableInit();
        } // uiVariablesTableGroupListSelectChanged

        #endregion

        #region Load/Save functions
        internal void uiSaveClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            TextWriter file = null;
            uiStatusLabel.Text = "Saving to file...";
            int count = 1;
            var baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            try {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(global::Folder));
                while (File.Exists($"{baseFolder}\\MyMacroFile~{count}.xml")) count ++;
                file = new StreamWriter($"{baseFolder}\\MyMacroFile~{count}.xml");
                xmlSerializer.Serialize(file, folder);
                uiStatusLabel.Text = "Saving success..." + $"{baseFolder}\\MyMacroFile~{count}.xml";
            } catch (Exception) {
                uiStatusLabel.Text = "Saving file error...";
            } finally {
                file?.Close();
                File.Copy($"{baseFolder}\\MyMacroFile~{count}.xml", $"{baseFolder}\\MyMacroFile.xml", true);
                File.Copy($"{baseFolder}\\MyMacroFile~{count}.xml", $"{baseFolder}\\MyMacroFile.xml", true);
            } // finally
            dtend();
        } // function uiSaveClicked
        internal void uiLoadClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            uiStatusLabel.Text = "Loading...";
            uiProgList.Items.Clear();
            uiStmtList.Items.Clear();
            uiFuncList.Items.Clear();
            uiInstList.Items.Clear();
            uiProgList.Text = "";
            uiFuncList.Text = "";
            uiImageLibrary.Controls.Clear();
            var baseFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            FileStream fs = null;
            try {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(global::Folder));
                fs = new FileStream($"{baseFolder}\\MyMacroFile.xml", FileMode.Open);
                folder = (global::Folder)xmlSerializer.Deserialize(fs);
            } catch {
                uiStatusLabel.Text = "Loading file error...";
                fs?.Close();
                updateUIStates();
                dtend();
                return;
            } finally {
                fs?.Close();
            } // finally
            try {
                folder.progList?.Sort(delegate(Script p0, Script p1) {
                    return p0.progName.CompareTo(p1.progName);
                });
                foreach (var prog in folder.progList) {
                    uiProgList.Items.Add(prog.progName);
                } // foreach
                if (folder.progList.Count != 0) {
                    folder.progList[0]?.funcList?.Sort(delegate (Function f0, Function f1) {
                        return f0.funcName.CompareTo(f1.funcName);
                    });
                    foreach (var func in folder.progList[0].funcList) {
                        uiFuncList.Items.Add(func.funcName);
                    } // foreach
                } // if
                if (currFunc != null) {
                    showStmts();
                } // if
                void mouseUp(object _sender, MouseEventArgs _e) {
                    if (_e.Button == MouseButtons.Left && uiILSetOCRRect.Checked) {
                        ImageVar iv = folder.imageLibrary.pb2ImageVar[(PictureBox)_sender];
                        (iv.OCRRectBR.X, iv.OCRRectBR.Y) = (_e.X, _e.Y);
                        if (iv.OCRRectTL.X > iv.OCRRectBR.X) (iv.OCRRectTL.X, iv.OCRRectBR.X) = (iv.OCRRectBR.X, iv.OCRRectTL.X);
                        if (iv.OCRRectTL.Y > iv.OCRRectBR.Y) (iv.OCRRectTL.Y, iv.OCRRectBR.Y) = (iv.OCRRectBR.Y, iv.OCRRectTL.Y);
                    } // if
                } // mouseUp
                void mouseDown(object _sender, MouseEventArgs _e) {
                    if (uiILSetSearchRect.Checked) {
                        ImageVar iv = folder.imageLibrary.pb2ImageVar[(PictureBox)_sender];
                        (iv.searchRectTL.X, iv.searchRectTL.Y) = (_e.X, _e.Y);
                        (iv.searchRectBR.X, iv.searchRectBR.Y) = (_e.X, _e.Y);
                    } // if
                    if (uiILSetOCRRect.Checked) {
                        ImageVar iv = folder.imageLibrary.pb2ImageVar[(PictureBox)_sender];
                        (iv.OCRRectTL.X, iv.OCRRectTL.Y) = (_e.X, _e.Y);
                        (iv.OCRRectBR.X, iv.OCRRectBR.Y) = (_e.X, _e.Y);
                    } // if
                } // mouseDown
                void mouseMove(object _sender, MouseEventArgs _e) {
                    if (_e.Button == MouseButtons.Left && uiILSetSearchRect.Checked) {
                        ImageVar iv = folder.imageLibrary.pb2ImageVar[(PictureBox)_sender];
                        (iv.searchRectBR.X, iv.searchRectBR.Y) = (_e.X, _e.Y);
                        ((PictureBox)_sender).Invalidate();
                    } // if
                    if (_e.Button == MouseButtons.Left && uiILSetOCRRect.Checked) {
                        ImageVar iv = folder.imageLibrary.pb2ImageVar[(PictureBox)_sender];
                        (iv.OCRRectBR.X, iv.OCRRectBR.Y) = (_e.X, _e.Y);
                        ((PictureBox)_sender).Invalidate();
                    } // if
                } // mouseMove
                void paintSearchRect(object _sender, PaintEventArgs pe) {
                    ImageVar iv = folder.imageLibrary.pb2ImageVar[(PictureBox)_sender];
                    {
                        int top = iv.searchRectTL.Y;
                        int left = iv.searchRectTL.X;
                        int bottom = iv.searchRectBR.Y;
                        int right = iv.searchRectBR.X;
                        if (left > right) (left, right) = (right, left);
                        if (top > bottom) (top, bottom) = (bottom, top);
                        pe.Graphics.DrawRectangle(new Pen(Brushes.Aqua), new Rectangle(left, top,
                            Math.Abs(left - right), Math.Abs(top - bottom)));
                    }
                    {
                        int top = iv.OCRRectTL.Y;
                        int left = iv.OCRRectTL.X;
                        int bottom = iv.OCRRectBR.Y;
                        int right = iv.OCRRectBR.X;
                        if (left > right) (left, right) = (right, left);
                        if (top > bottom) (top, bottom) = (bottom, top);
                        pe.Graphics.DrawRectangle(new Pen(Brushes.BlueViolet), new Rectangle(left, top,
                            Math.Abs(left - right), Math.Abs(top - bottom)));
                    }
                } // paintSearchRect
                uiVariableTableInit();
                foreach (ImageVar iv in folder.imageLibrary.imageVars) {
                    var pb = new PictureBox();
                    pb.Padding = new Padding(PB_PADDING);
                    pb.MouseUp += new MouseEventHandler(mouseUp);
                    pb.MouseDown += new MouseEventHandler(mouseDown);
                    pb.MouseMove += new MouseEventHandler(mouseMove);
                    pb.Paint += new PaintEventHandler(paintSearchRect);
                    pb.Click += new System.EventHandler(this.uiILImageClicked);
                    pb.Image = iv.image;
                    uiImageLibrary.Controls.Add(pb);
                    iv.pb = pb;
                    folder.imageLibrary.pb2ImageVar[pb] = iv;
                    iv.drawHotSpotOnPB();
                } // foreach
                uiFuncList.SelectedIndex = uiFuncList.Items.Count - 1;
                uiProgList.SelectedIndex = uiProgList.Items.Count - 1;
                showStmts();
                showInsts();
                updateUIStates();
                uiStatusLabel.Text = "Loading successful...";
            } catch (Exception exception) {
                uiStatusLabel.Text = $"Loading parsing error... {exception.Message}";
                uiStmtList.Items.Clear();
                uiFuncList.Items.Clear();
                uiProgList.Items.Clear();
                uiInstList.Items.Clear();
                folder = new Folder();
            } // catch
            dtend();
        } // function uiLoadClicked

        private bool uiVariableTableHasName(string name) {
            foreach(DataGridViewRow row in uiVariablesTable.Rows) {
                if ((string)row.Cells["Name"].Value == name) return true;
            } // foreach
            return false;
        } // uiVariableTableHasName
        private void uiVariableTableInit() {
            try { 
                uiVariablesTable.Rows.Clear();
                if (runContext.vars.ContainsKey("retVal")) { 
                    updateVariableTableRow("retVal", runContext.vars["retVal"], false, "All,Debug");
                } // if
                updateVariableTableRow("clipboard", runContext.clipboard, false, "All,Debug");
                updateVariableTableRow("ocrOut", "??", false, "All,Debug");
                updateVariableTableRow("currCond", runContext.currCond.ToString(), false, "All,Debug");
                foreach(var var in runContext.vars) {
                    updateVariableTableRow(var.Key, var.Value, false, "All,Debug,Locals,Globals,Persistents");
                } // foreach
                foreach(var array in runContext.arrays) {
                    updateVariableTableRow(array.Key, string.Join(",", array.Value), true, "All,Debug,Locals,Globals,Persistents");
                } // foreach
                foreach (ImageVar iv in folder.imageLibrary.imageVars) {
                    foreach (string name in iv.imageName.Split(',')) {
                        if (!uiVariableTableHasName(name)) {
                            updateVariableTableRow(name, "<image>", false, "All,Images");
                        } // if
                    } // if
                } // foreach
                foreach (var im in runContext.imagesCaptured) {
                    updateVariableTableRow(im.Key, "<image>", false, "All,Images,Debug");
                } // foreach
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // uiVariableTableInit
        #endregion

        #region Image library related functions
        internal void uiILToggleOriginalSizeClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            try {
                foreach (var iv in folder.imageLibrary.imageVars) {
                    if (uiILToggleOriginalSize.Checked) {
                        iv.pb.Width = iv.image.Width;
                        iv.pb.Height = iv.image.Height;
                    } else {
                        iv.pb.Width = Math.Min(150, iv.image.Width);
                        iv.pb.Height = Math.Min(100, iv.image.Height);
                    } // if
                } // foreach
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try            dtend();
        } // function uiILToggleOriginalSizeClicked
        internal void uiILPasteClipboardImageClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            try { 
                Image clipboardImage = Clipboard.GetImage();
                if (clipboardImage != null) {
                    PictureBox pb = new PictureBox();
                    pb.Padding = new Padding(PB_PADDING);
                    pb.BorderStyle = BorderStyle.None;
                    pb.Image = clipboardImage;
                    pb.Click += new System.EventHandler(this.uiILImageClicked);
                    uiImageLibrary.Controls.Add(pb);
                    ImageVar iv = new ImageVar(clipboardImage, folder.imageLibrary, pb);
                    folder.imageLibrary.imageVars.Add(iv);
                    folder.imageLibrary.pb2ImageVar[pb] = iv;
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function uiILPasteClipboardImageClicked
        internal void uiILImageDelClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            try {
                PictureBox pb = selectedImageVarPB;
                if (pb != null) {
                    ImageVar iv = folder.imageLibrary.pb2ImageVar[pb];
                    folder.imageLibrary.pb2ImageVar.Remove(pb);
                    uiImageLibrary.Controls.Remove(pb);
                    folder.imageLibrary.imageVars.Remove(iv);
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function uiILImageDelClicked
        internal void uiILImageClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            try {
                MouseEventArgs me = (MouseEventArgs)e;
                PictureBox libImagePB = (PictureBox)sender;
                foreach (PictureBox pb in uiImageLibrary.Controls) {
                    pb.BorderStyle = BorderStyle.None;
                } // foreach
                libImagePB.BorderStyle = BorderStyle.FixedSingle;
                ImageVar iv = folder.imageLibrary.pb2ImageVar[libImagePB];
                uiILImageName.Text = iv.imageName;
                if (me?.Button == MouseButtons.Right) { 
                    Clipboard.SetImage(iv.image);
                } // if
                iv.matchThreshold = iv.matchThreshold.Replace(',', '.');
                uiILOCRParams.Text = $"{iv.matchThreshold},{iv.OCRParams}";
                if (iv.searchRectBR != iv.searchRectTL) {
                    uiILSetSearchRect.Text = "[🔍]";
                } else {
                    uiILSetSearchRect.Text = "🔍";
                } // if
                if (iv.OCRRectBR != iv.OCRRectTL) {
                    uiILSetOCRRect.Text = "[🗚]";
                } else {
                    uiILSetOCRRect.Text = "🗚";
                } // if
                if (iv.isOCRRectBox) {
                    uiILOCRBoxButton.Text = "[❏]";
                } else {
                    uiILOCRBoxButton.Text = "❏";
                } // if
                libImagePB.Width = iv.image.Width;
                libImagePB.Height = iv.image.Height;
                if (iv.actionOffset.Contains(",")) {
                    uiILSetHotSpot.Text = "[🎯]";
                } else { 
                    uiILSetHotSpot.Text = "🎯";
                } // if
                if (uiILSetHotSpot.Checked && me != null && me.Button == MouseButtons.Left)
                { // mark the hotspot
                    iv.actionOffset = me.Location.X + "," + me.Location.Y;
                    uiILSetHotSpot.Text = "[🎯]";
                    iv.drawHotSpotOnPB();
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function uiILImageClicked
        internal void uiILReplaceImageClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            try {
                Image clipboardImage = Clipboard.GetImage();
                if (clipboardImage != null) {
                    PictureBox pb = selectedImageVarPB;
                    pb.Image = clipboardImage;
                    var iv = folder.imageLibrary.pb2ImageVar[pb];
                    iv.image = pb.Image;
                    iv.searchRectBR = iv.searchRectTL;
                    iv.actionOffset = "";
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function uiILReplaceImageClicked
        private void uiILImageNameChanged(object sender, KeyPressEventArgs e) {
            dtbegin(funcName());
            try {
                if (selectedImageVarPB != null) {
                    folder.imageLibrary.pb2ImageVar[selectedImageVarPB].imageName = uiILImageName.Text;
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function uiILImageNameChanged
        private void uiILShowOCROutClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            if (selectedImageVarPB == null) {
                uiStatusLabel.Text = "Please select an image in the library.";
                dtend();
                return; 
            } // if
            string[] parameters = Regex.Replace(uiILOCRParams.Text, "^.*?,", "").Split(',');
            if (parameters.Length != 5) {
                uiStatusLabel.Text = "You need to provide 5 parameters.";
                dtend();
                return;
            } // if
            selectedImageVarPB.Image = folder.imageLibrary.pb2ImageVar[selectedImageVarPB].image;
            try {
                var iv = folder.imageLibrary.pb2ImageVar[selectedImageVarPB];
                Bitmap searchBitmap = new Bitmap(iv.image);
                if (iv.searchRectBR != iv.searchRectTL) {
                    int top = iv.searchRectTL.Y;
                    int left = iv.searchRectTL.X;
                    int bottom = iv.searchRectBR.Y;
                    int right = iv.searchRectBR.X;
                    if (left > right) (left, right) = (right, left);
                    if (top > bottom) (top, bottom) = (bottom, top);
                    var rect = new Rectangle(left, top, Math.Abs(left - right), Math.Abs(top - bottom));
                    searchBitmap = searchBitmap.Clone(rect, iv.image.PixelFormat);
                } // if
                Image<Bgr, Byte> searchImg = searchBitmap.ToImage<Bgr, byte>();
                var ocrImage = searchImg.Clone().Resize(float.Parse(parameters[0]), Inter.Cubic);
                var ocrImageGray = ocrImage.Convert<Gray, byte>();
                if (float.Parse(parameters[1]) != -1 && float.Parse(parameters[2]) != -1) {
                    ocrImageGray = ocrImageGray.SmoothBilateral(int.Parse(parameters[1]), int.Parse(parameters[2]), int.Parse(parameters[2]));
                } // if
                if (int.Parse(parameters[4]) != -1) { 
                    if (int.Parse(parameters[3]) == 1) {
                        ocrImageGray = ocrImageGray.ThresholdBinaryInv(new Gray(int.Parse(parameters[4])),
                        new Gray(255));
                    } else {
                        ocrImageGray = ocrImageGray.ThresholdBinary(new Gray(int.Parse(parameters[4])),
                        new Gray(255));
                    } // if
                } // if
                selectedImageVarPB.Image = ocrImageGray.ToBitmap();
                Pix pix = PixConverter.ToPix(ocrImageGray.AsBitmap<Gray, Byte>());
                Page page = ocrEngine.Process(pix, PageSegMode.SingleLine);
                string textOut = page.GetText().Replace('\n', ' ');
                uiStatusLabel.Text = $"OCR output is {textOut}";
                debugOut(page.GetText());
                page.Dispose();
                pix.Dispose();
            } catch (Exception ex) {
                uiStatusLabel.Text = "Error in processing: " + ex.ToString();
                debugOut(ex.ToString());
            } // catch
            dtend();
        } // function uiILShowOCROutClicked
        #endregion

        #region Key press events
        internal void uiProgListKeyPress(object sender, KeyPressEventArgs e) {
            dtbegin(funcName());
            try { 
                if (e.KeyChar == (int)Keys.Enter) {
                    uiProgListNameChanged(null, null);
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function uiProgListKeyPress
        internal void uiFuncListKeyPress(object sender, KeyPressEventArgs e) {
            dtbegin(funcName());
            try { 
                if (e.KeyChar == (int)Keys.Enter) {
                    uiFuncListNameChanged(null, null);
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function uiFuncListKeyPress
        internal void uiStmtLabelKeyPress(object sender, KeyPressEventArgs e) {
            dtbegin(funcName());
            try { 
                if (e.KeyChar == (int)Keys.Enter) {
                    uiStmtNameUpdateClicked(null, null);
                    uiStmtList.Focus();
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function uiStmtLabelKeyPress
        internal void uiInstParamsKeyPress(object sender, KeyPressEventArgs e) {
            dtbegin(funcName());
            try {
                if (e.KeyChar == (int)Keys.Enter) {
                    if (uiInstList.SelectedIndex != -1) { 
                        currInst.instParamLeft = uiInstParams.Text;
                        currInst.instParamRight = uiInstParamRight.Text;
                        updateStmtUI(currStmt.stmtLabel, uiInstList.SelectedIndex);
                        uiInstList.Focus();
                    } // if
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // function uiInstParamsKeyPress
        #endregion
        private void uiInstParamRight_SelectedIndexChanged(object sender, EventArgs e){

        } // uiInstParamRight_SelectedIndexChanged

        private void uiILCheckMatchResult(object sender, EventArgs e) {
            dtbegin(funcName());
            try {
                if (selectedImageVarPB != null) {
                    selectedImageVarPB.Image = folder.imageLibrary.pb2ImageVar[selectedImageVarPB].image;
                    uiImageLibrary.Visible = false;
                    ImageVar iv = folder.imageLibrary.pb2ImageVar[selectedImageVarPB];
                    processCmpImage(null, iv.isOCRRectBox, iv);
                    uiImageLibrary.Visible = true;
                    uiILOCRParams.Text = $"{runContext.maxValue}";
                } else { 
                    uiStatusLabel.Text = "Please select an image in the library.";
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // uiILCheckMatchResult

        private void uiInstParamsRightKeyPress(object sender, KeyPressEventArgs e)
        {
            dtbegin(funcName());
            if (e.KeyChar == (int)Keys.Enter)
            {
                if (uiInstList.SelectedIndex != -1)
                {
                    currInst.instParamLeft = uiInstParams.Text;
                    currInst.instParamRight = uiInstParamRight.Text;
                    updateStmtUI(currStmt.stmtLabel, uiInstList.SelectedIndex);
                    uiInstList.Focus();
                } // if
            } // if
            dtend();
        } // uiInstParamsRightKeyPress

        private void uiILSetThresholdClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            try {
                if (selectedImageVarPB == null) {
                    uiStatusLabel.Text = "Please select an image in the library.";
                    dtend();
                    return;
                } // if
                string matchTreshold = uiILOCRParams.Text.Split(',')[0];
                string ocrParams = Regex.Replace(uiILOCRParams.Text, "^.*?,", "");
                folder.imageLibrary.pb2ImageVar[selectedImageVarPB].matchThreshold = matchTreshold;
                folder.imageLibrary.pb2ImageVar[selectedImageVarPB].OCRParams = ocrParams;
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // uiILSetThresholdClicked
        private void uiILOCRBoxButtonClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            try { 
                if (selectedImageVarPB == null) {
                    uiStatusLabel.Text = "Please select an image in the library.";
                } else {
                    folder.imageLibrary.pb2ImageVar[selectedImageVarPB].isOCRRectBox = !folder.imageLibrary.pb2ImageVar[selectedImageVarPB].isOCRRectBox;
                    if (folder.imageLibrary.pb2ImageVar[selectedImageVarPB].isOCRRectBox) {
                        uiILOCRBoxButton.Text = "[❏]";
                    } else {
                        uiILOCRBoxButton.Text = "❏";
                    } // if
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // uiILOCRBoxButtonClicked
        private void uiInstCommentToggleClicked(object sender, EventArgs e) {
            try {
                currInst.isCommentedOut = !currInst.isCommentedOut;
                int instIndex = uiInstList.SelectedIndex + 1;
                if (instIndex == uiInstList.Items.Count) instIndex--;
                updateStmtUI(currStmt.stmtLabel, instIndex);
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // uiInstCommentToggleClicked
        private void uiStmtCommentToggleClicked(object sender, EventArgs e) {
            try {
                var idx = uiStmtList.SelectedIndex+1;
                currStmt.isCommentedOut = !currStmt.isCommentedOut;
                uiFuncListChanged(null, null);
                if (idx == uiStmtList.Items.Count) idx--;
                uiStmtList.SelectedIndex = idx;
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // uiStmtCommentToggleClicked
        private void uiVariablesTableDelButtonClicked(object sender, EventArgs e) {
            try {
                uiVariableTableInit();
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // uiVariablesTableDelButtonClicked
        private void uiInstStepNextButtonClicked(object sender, EventArgs e) {
            dtbegin(funcName());
            try {
                if (!runWorker.IsBusy) { 
                    if (programState == PROGRAM_STATE.STEPINST) {
                        runWorker.RunWorkerAsync(currInst);
                    } else {
                        programState = PROGRAM_STATE.STEPINST;
                        callStack = new Stack<Tuple<FuncName, StmtLabel, RetVal, List<string>>>();
                        argStack = new Stack<VarName>();
                        if (uiStmtList.Items.Count != 0) {
                            uiStmtList.SelectedIndex = 0;
                            uiInstList.SelectedIndex = uiInstList.Items.Count == 0 ? -1 : 0;
                            uiStatusLabel.Text = "Program running...";
                            debugOut("Run stmt: " + currStmt.text);
                            runWorker.RunWorkerAsync(currInst);
                        } // if
                    } // if
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
            dtend();
        } // uiInstStepNextButtonClicked

        private void uiVariablesTable_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            try {
                foreach (DataGridViewRow row in uiVariablesTable.SelectedRows) {
                    if ((string)row.Cells["Value"].Value == "<image>") {
                        var imageName = (string)row.Cells["Name"].Value;
                        if (uiBoxCalibrateTab.TabPages[uiBoxCalibrateTab.SelectedIndex].Text != "Box Calibrate") {
                            syncImageListItem(imageName);
                        } // if
                        if (runContext.imagesCaptured.ContainsKey(imageName)) {
                            uiBoxCalibrateCaptureImgNonZoom = (Image)runContext.imagesCaptured[imageName].Clone();
                            uiBoxCalibrateCaptureImg.Image = uiBoxCalibrateCaptureImgNonZoom;
                            uiBoxCalibrateCaptureImg.Width = uiBoxCalibrateCaptureImgNonZoom.Width;
                            uiBoxCalibrateCaptureImg.Height = uiBoxCalibrateCaptureImgNonZoom.Height;
                            panMovingPoint = Point.Empty;
                            panStartingPoint = Point.Empty;
                            panning = false;
                        } // if
                    } // if
                } // foreach
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // uiVariablesTable_RowHeaderMouseClick
        void syncImageListItem(string imageNames) {
            try {
                uiBoxCalibrateTab.SelectedTab = uiImageLibraryTab;
                foreach (var imageName in imageNames.Split(',')) {
                    List<ImageVar> ivs = folder.imageLibrary.findImage(imageName);
                    foreach (ImageVar iv in ivs) {
                        uiImageLibrary.Controls.SetChildIndex(iv.pb, 0);
                    } // foreach
                    if (ivs.Count != 0) {
                        uiImageLibrary.ScrollControlIntoView(ivs[0].pb);
                        uiILImageClicked(ivs[0].pb, null);
                    } // if
                } // foreach
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // syncImageListItem

        private void uiBoxCalibrateImgSelect_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                var ivs = folder.imageLibrary.findImage(uiBoxCalibrateImgSelect.Text);
                uiBoxCalibrateMatchImg.Image = ivs[0].image;
                uiBoxCalibrateMatchImg.Width = ivs[0].image.Width;
                uiBoxCalibrateMatchImg.Height = ivs[0].image.Height;
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // uiBoxCalibrateImgSelect_SelectedIndexChanged

        private void uiBoxCalibrateImgSelect_DropDown(object sender, EventArgs e) {
            try {
                uiBoxCalibrateImgSelect.Items.Clear();
                foreach (ImageVar iv in folder.imageLibrary.imageVars) {
                    if (iv.isOCRRectBox) {
                        List<string> names = iv.getImageNames;
                        foreach (var ivName in names) {
                            if (uiBoxCalibrateImgSelect.FindStringExact(ivName) == -1) {
                                uiBoxCalibrateImgSelect.Items.Add(ivName);
                            } // if
                        } // foreach
                    } // if
                } // foreach
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // uiBoxCalibrateImgSelect_DropDown

        private void uiBoxCalibratePasteCaptureImg_Click(object sender, EventArgs e) {
            try {
                Image clipboardImage = Clipboard.GetImage();
                if (clipboardImage != null) {
                    runContext.imagesCaptured["debug-clipboard"] = clipboardImage;

                    uiBoxCalibrateCaptureImgNonZoom = (Image)clipboardImage.Clone();
                    uiBoxCalibrateCaptureImg.Image = uiBoxCalibrateCaptureImgNonZoom;
                    uiBoxCalibrateCaptureImg.Width = uiBoxCalibrateCaptureImgNonZoom.Width;
                    uiBoxCalibrateCaptureImg.Height = uiBoxCalibrateCaptureImgNonZoom.Height;
                    panMovingPoint = Point.Empty;
                    panStartingPoint = Point.Empty;
                    panning = false;
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // uiBoxCalibratePasteCaptureImg_Click
        private void uiBoxCalibrateMatchButton_Click(object sender, EventArgs e) {
            try {
                var ivs = folder.imageLibrary.findImage(uiBoxCalibrateImgSelect.Text);
                if (ivs.Count != 0) {
                    uiBoxCalibrateTab.Visible = false;
                    processCmpImage(null, false, ivs[0]);
                    uiBoxCalibrateTab.Visible = true;
                } // if
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // uiBoxCalibrateMatchButton_Click

        private void uiBoxCalibrateExportButton_Click(object sender, EventArgs e) {
            try {
                string imageExportTxt = "";
                var bitmap = new Bitmap(uiBoxCalibrateCaptureImg.Image);
                for (var r = 0; r < bitmap.Height; r++) { 
                    for (var c = 0; c < bitmap.Width; c ++) {
                        var pixData = bitmap.GetPixel(c, r);
                        imageExportTxt += pixData.ToString() + "\t";
                    } // for
                    imageExportTxt += "\n\r";
                } // for
                Clipboard.SetText(imageExportTxt);
                uiStatusLabel.Text = "Image data export to clipboard is finished. " + DateTime.Now;
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // uiBoxCalibrateExportButton_Click

        private void uiILFilterName_TextChanged(object sender, KeyPressEventArgs e) {
            try {
                var text = uiILImageName.Text;
                uiILImageName.Text = "Please wait...";
                uiILImageName.Enabled = false;
                if (e.KeyChar == (int)Keys.Enter) {
                    foreach (var img in folder.imageLibrary.imageVars) {
                        img.pb.Visible = img.imageName.Contains(uiILFilterName.Text);
                    } // foreach
                } // if
                uiILImageName.Text = text;
                uiILImageName.Enabled = true;
            } catch (Exception ex) {
                uiStatusLabel.Text = $"Internal error: {ex.Message}{checkBreak}";
            } // try
        } // uiILFilterName_TextChanged
    } // class Form1
} // namespace AIScreenAutomationApp

enum PROGRAM_STATE { STOP, RUNNING, PAUSE, STEPINST, RUNINST } // enum PROGRAM_STATE 

public enum InstructionType {SAT_DELAY, SAT_KEYPRESS, SAT_LCLICK,
    SAT_LDOWN, SAT_LUP, SAT_RCLICK, SAT_DCLICK, SAT_MMOVE, SAT_PRINTLN,
    SAT_CMPBOX, SAT_CMPBOXCOPY, SAT_CMPIMG, SAT_CMPARGS, SAT_FRETURN, SAT_ASSIGN,
    SAT_JSTMT, SAT_CALLF, SAT_STOP_PROGRAM, SAT_PUSHARG, // push string arguments before call
    SAT_INVALID
} // enum InstructionType
public class Instruction : ICloneable {
    public STMT_CONDITIONS instCondition = STMT_CONDITIONS.AL;
    public string instParamLeft; // left handside parameter
    public string instParamRight; // right handside parameter.
    public bool isCommentedOut;
    public Point screenLoc; // recognition location when inst is cmpimg
    public InstructionType instructionType;
    public string text {
        get {
            if (isCommentedOut) {
                return "// " + instCondition + " @ " + instName + paramText;
            } // if
            return instCondition + " @ " + instName + paramText;
        } // get
    } // string text
    static public InstructionType getInstType(string instName) {
        switch (instName) {
            case "Call Func": return InstructionType.SAT_CALLF;
            case "Func Return": return InstructionType.SAT_FRETURN;
            case "Assign": return InstructionType.SAT_ASSIGN;
            case "Cmp Image": return InstructionType.SAT_CMPIMG;
            case "Cmp Box-copy": return InstructionType.SAT_CMPBOXCOPY;
            case "Cmp Box": return InstructionType.SAT_CMPBOX;
            case "Cmp Args": return InstructionType.SAT_CMPARGS;
            case "Delay": return InstructionType.SAT_DELAY;
            case "Key Press": return InstructionType.SAT_KEYPRESS;
            case "Stop Program": return InstructionType.SAT_STOP_PROGRAM;
            case "Left Double": return InstructionType.SAT_DCLICK;
            case "Right Click": return InstructionType.SAT_RCLICK;
            case "Left Click": return InstructionType.SAT_LCLICK;
            case "Mouse": return InstructionType.SAT_LCLICK;
            case "Left Up": return InstructionType.SAT_LUP;
            case "Left Down": return InstructionType.SAT_LDOWN;
            case "Mouse Move": return InstructionType.SAT_MMOVE;
            case "Jump to Stmt": return InstructionType.SAT_JSTMT;
            case "Print": return InstructionType.SAT_PRINTLN;
        } // switch
        return InstructionType.SAT_INVALID;
    } // function getInstType
    public string selectName {
        get {
            switch (instructionType) {
                case InstructionType.SAT_CMPARGS: return "Cmp Args";
                case InstructionType.SAT_CMPIMG: return "Cmp Image";
                case InstructionType.SAT_CMPBOX: return "Cmp Box";
                case InstructionType.SAT_CMPBOXCOPY: return "Cmp Box-copy";
                case InstructionType.SAT_DELAY: return "Delay";
                case InstructionType.SAT_ASSIGN: return "Assign";
                case InstructionType.SAT_FRETURN: return "Func Return";
                case InstructionType.SAT_KEYPRESS: return "Key Press";
                case InstructionType.SAT_STOP_PROGRAM: return "Stop Program";
                case InstructionType.SAT_DCLICK:
                case InstructionType.SAT_RCLICK:
                case InstructionType.SAT_LUP:
                case InstructionType.SAT_LCLICK:
                case InstructionType.SAT_MMOVE:
                case InstructionType.SAT_LDOWN: return "Mouse";
                case InstructionType.SAT_CALLF: return "Call Func";
                case InstructionType.SAT_JSTMT: return "Jump to Stmt";
                case InstructionType.SAT_PRINTLN: return "Print";
            } // switch
            return "Invalid";
        } // get
    } // selectName
    public string instName {
        get {
            switch (instructionType) {
                case InstructionType.SAT_CMPARGS: return "Cmp Args";
                case InstructionType.SAT_CMPIMG: return "Cmp Image";
                case InstructionType.SAT_CMPBOX: return "Cmp Box";
                case InstructionType.SAT_CMPBOXCOPY: return "Cmp Box-copy";
                case InstructionType.SAT_DELAY: return "Delay";
                case InstructionType.SAT_KEYPRESS: return "Key Press";
                case InstructionType.SAT_STOP_PROGRAM: return "Stop Program";
                case InstructionType.SAT_LDOWN: return "Left Down";
                case InstructionType.SAT_LUP: return "Left Up";
                case InstructionType.SAT_LCLICK: return "Left Click";
                case InstructionType.SAT_CALLF: return "Call Func";
                case InstructionType.SAT_FRETURN: return "Func Return";
                case InstructionType.SAT_ASSIGN: return "Assign";
                case InstructionType.SAT_JSTMT: return "Jump to Stmt";
                case InstructionType.SAT_DCLICK: return "Left Double";
                case InstructionType.SAT_RCLICK: return "Right Click";
                case InstructionType.SAT_MMOVE: return "Mouse Move";
                case InstructionType.SAT_PRINTLN: return "Print";
            } // switch
            return "Invalid";
        } // get
    } // string instName
    public string paramText {
        get {
            switch (instructionType) {
                case InstructionType.SAT_KEYPRESS:
                case InstructionType.SAT_DELAY:
                case InstructionType.SAT_CMPIMG:
                case InstructionType.SAT_CMPBOX:
                case InstructionType.SAT_CMPBOXCOPY:
                case InstructionType.SAT_PUSHARG:
                case InstructionType.SAT_FRETURN:
                case InstructionType.SAT_JSTMT:
                case InstructionType.SAT_PRINTLN:
                    return "(" + instParamLeft + ")";
                case InstructionType.SAT_CMPARGS:
                    return $"({instParamLeft}, {instParamRight})";
                case InstructionType.SAT_ASSIGN:
                    return $"({instParamLeft}={instParamRight})";
                case InstructionType.SAT_CALLF:
                    return $" {instParamLeft}({instParamRight})";
            } // switch
            return "";
        } // get
    } // string paramText
    private global::AIScreenAutomationApp.Form1 form {
        get {
            return global::AIScreenAutomationApp.Form1.gForm;
        } // get
    } // global::AIScreenAutomationApp.Form1 form
    [DllImport("user32.dll")]
    public static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);
    public const int MOUSEEVENTF_MOVE = 0x01;
    public const int MOUSEEVENTF_LEFTDOWN = 0x02;
    public const int MOUSEEVENTF_LEFTUP = 0x04;
    public const int MOUSEEVENTF_RIGHTDOWN = 0x08;
    public const int MOUSEEVENTF_RIGHTUP = 0x10;
    public const int MOUSEEVENTF_ABSOLUTE = 0x8000;

    public bool isBuiltIn(string funcName) {
        switch(funcName) {
            case "split":
            case "replace":
            case "min":
            case "max":
                return true;
        } // switch
        return false;
    } // isBuiltIn
    public bool isCallInst()
    {
        if (instructionType == InstructionType.SAT_CALLF) return !isBuiltIn(instParamLeft);
        return false;
    } // function isCallInst
    public bool isCmpImgInst{
        get {
            return instructionType == InstructionType.SAT_CMPIMG || instructionType == InstructionType.SAT_CMPBOX || instructionType == InstructionType.SAT_CMPBOXCOPY;
        } // get
    } // isCmpImgInst
    public bool isCallOrJumpInst()
    {
        return isCallInst() || instructionType == InstructionType.SAT_JSTMT;
    } // function isCallInst
    public bool isJumpInst()
    {
        return instructionType == InstructionType.SAT_JSTMT;
    } // function isJumpInst
    public Instruction() {
        instructionType = InstructionType.SAT_INVALID;
        instParamLeft = "";
        instParamRight = "";
        instCondition = STMT_CONDITIONS.AL;
    } // Instruction
    public bool ExecInstContinue(Statement statement) {
        form.debugOut("ExecInstContinue: " + text);
        uint isAbsolute = 0;
        Point getHotSpot() {
            Point _pt = new Point(0, 0);
            isAbsolute = MOUSEEVENTF_ABSOLUTE;
            _pt = new Point((Size)form.runContext.screenMatchLoc);
            _pt.X += form.runContext.actionOffsetPt.X;
            _pt.Y += form.runContext.actionOffsetPt.Y;
            _pt.X = _pt.X * 65535 / 1920;
            _pt.Y = _pt.Y * 65535 / 1080;
            return _pt;
        } // function getHotSpot
        
        void mouseRandomCircle() {
            var rand = new Random();
            int size = rand.Next() % 20+5;
            int dx = 0;
            int dy = 0;
            while (size-- > 0) {
                int cx = rand.Next() % 20 - 10;
                int cy = rand.Next() % 20 - 10;
                dx += cx;
                dy += cy;
                mouse_event(MOUSEEVENTF_MOVE, (int)cx, (int)cy, 0, 0);
                Thread.Sleep(10);
            } // while
            mouse_event(MOUSEEVENTF_MOVE, -(int)dx, -(int)dy, 0, 0);
        } // function mouseRandomCircle
        Point pt;
        switch (instructionType) {
            case InstructionType.SAT_CMPBOXCOPY:
                form.runWorker.RunWorkerAsync(form.currInst);
                Point ptTL = new Point(form.runContext.copyClipboardRect.X, form.runContext.copyClipboardRect.Y);
                Point ptBR = new Point(form.runContext.copyClipboardRect.Right, form.runContext.copyClipboardRect.Bottom);
                Instruction.mouse_event(Instruction.MOUSEEVENTF_MOVE | Instruction.MOUSEEVENTF_ABSOLUTE, ptTL.X * 65535 / 1920, ptTL.Y * 65535 / 1080, 0, 0);
                Instruction.mouse_event(Instruction.MOUSEEVENTF_LEFTDOWN, ptTL.X * 65535 / 1920, ptTL.Y * 65535 / 1080, 0, 0);
                Instruction.mouse_event(Instruction.MOUSEEVENTF_MOVE | Instruction.MOUSEEVENTF_ABSOLUTE, ptBR.X * 65535 / 1920, ptBR.Y * 65535 / 1080, 0, 0);
                Instruction.mouse_event(Instruction.MOUSEEVENTF_LEFTUP, ptBR.X * 65535 / 1920, ptBR.Y * 65535 / 1080, 0, 0);
                Thread.Sleep(300);
                SendKeys.Send("^c");
                Thread.Sleep(300);
                try {
                    if (Clipboard.ContainsText()) {
                        form.runContext.clipboard = Clipboard.GetText(TextDataFormat.Text);
                        form.updateVariableTableRow("clipboard", form.runContext.clipboard, false, "All,Debug");
                    } // if
                } catch (Exception e) {
                    form.debugOut($"Clipboard access error: {e.Message}");
                } // try
                if (form.uiInstList.SelectedIndex != form.uiInstList.Items.Count - 1) {
                    form.uiInstList.SelectedIndex++;
                } // if
                return false;            
            case InstructionType.SAT_CMPBOX:
                form.runWorker.RunWorkerAsync(form.currInst);
                if (form.uiInstList.SelectedIndex != form.uiInstList.Items.Count - 1) {
                    form.uiInstList.SelectedIndex++;
                } // if
                return false;
            case InstructionType.SAT_CMPIMG:
                form.runWorker.RunWorkerAsync(form.currInst);
                if (form.uiInstList.SelectedIndex != form.uiInstList.Items.Count - 1) {
                    form.uiInstList.SelectedIndex++;
                } // if
                return false;
            case InstructionType.SAT_CMPARGS:
                form.runContext.currCond = STMT_CONDITIONS.INVALID;
                int leftInt, rightInt;
                bool isInt = true;
                string leftParam = form.substituteVars(instParamLeft);
                string rightParam = form.substituteVars(instParamRight);
                isInt &= int.TryParse(leftParam, out leftInt);
                isInt &= int.TryParse(rightParam, out rightInt);
                form.debugOut($"isInt={isInt}, left={leftInt}, right={rightInt}");
                if (!isInt) { // try decimal
                    float leftFloat, rightFloat;
                    bool isFloat = true;
                    isFloat &= float.TryParse(leftParam, System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out leftFloat);
                    isFloat &= float.TryParse(rightParam, System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out rightFloat);
                    form.debugOut($"isFloat={isFloat}, left={leftFloat}, right={rightFloat}");
                    if (!isFloat) { // do string cmp
                        form.debugOut($"STR case left={leftParam}, right={rightParam}");
                        if (leftParam == rightParam) { form.runContext.currCond = STMT_CONDITIONS.EQ; goto cmp_end; }
                        int cmpStrOut = String.CompareOrdinal(leftParam, rightParam);
                        if (cmpStrOut < 0) { form.runContext.currCond = STMT_CONDITIONS.LT; goto cmp_end; }
                        if (cmpStrOut > 0) { form.runContext.currCond = STMT_CONDITIONS.GT; goto cmp_end; }
                        form.runContext.currCond = STMT_CONDITIONS.NE;
                    } else {
                        if (leftFloat == rightFloat) { form.runContext.currCond = STMT_CONDITIONS.EQ; goto cmp_end; }
                        if (leftFloat < rightFloat) { form.runContext.currCond = STMT_CONDITIONS.LT; goto cmp_end; }
                        if (leftFloat > rightFloat) { form.runContext.currCond = STMT_CONDITIONS.GT; goto cmp_end; }
                        form.runContext.currCond = STMT_CONDITIONS.NE;
                    } // if
                } else {
                    if (leftInt == rightInt) { form.runContext.currCond = STMT_CONDITIONS.EQ; goto cmp_end; }
                    if (leftInt < rightInt) { form.runContext.currCond = STMT_CONDITIONS.LT; goto cmp_end; }
                    if (leftInt > rightInt) { form.runContext.currCond = STMT_CONDITIONS.GT; goto cmp_end; }
                    form.runContext.currCond = STMT_CONDITIONS.NE;
                } // if
                cmp_end:
                form.debugOut($"CMP result={form.runContext.currCond}");
                break;
            case InstructionType.SAT_PUSHARG:
                form.argStack.Push(instParamLeft);
                break;
            case InstructionType.SAT_DELAY:
                try {
                    Thread.Sleep(int.Parse(instParamLeft));
                } catch {
                    // TODO: check parsing of errors
                } // try
                break;
            case InstructionType.SAT_LCLICK:
//                var orgPt = Cursor.Position;
                pt = getHotSpot();
                mouse_event(MOUSEEVENTF_MOVE | isAbsolute, (int)pt.X, (int)pt.Y, 0, 0);
                mouseRandomCircle();
                Thread.Sleep(20);
                mouse_event(MOUSEEVENTF_LEFTDOWN, (int)pt.X, (int)pt.Y, 0, 0);
                Thread.Sleep(20);
                mouse_event(MOUSEEVENTF_LEFTUP, (int)pt.X, (int)pt.Y, 0, 0);
//                mouse_event(MOUSEEVENTF_MOVE, (int)orgPt.X, (int)orgPt.Y, 0, 0);
                break;
            case InstructionType.SAT_LDOWN:
                pt = getHotSpot();
                mouse_event(MOUSEEVENTF_MOVE | isAbsolute, (int)pt.X, (int)pt.Y, 0, 0);
                mouseRandomCircle();
                Thread.Sleep(20);
                mouse_event(MOUSEEVENTF_LEFTDOWN, (int)pt.X, (int)pt.Y, 0, 0);
                break;
            case InstructionType.SAT_LUP:
                pt = getHotSpot();
                mouse_event(MOUSEEVENTF_MOVE | isAbsolute, (int)pt.X, (int)pt.Y, 0, 0);
                mouseRandomCircle();
                Thread.Sleep(20);
                mouse_event(MOUSEEVENTF_LEFTUP, (int)pt.X, (int)pt.Y, 0, 0);
                break;
            case InstructionType.SAT_DCLICK:
                pt = getHotSpot();
                mouse_event(MOUSEEVENTF_MOVE | isAbsolute, (int)pt.X, (int)pt.Y, 0, 0);
                mouseRandomCircle();
                Thread.Sleep(20);
                mouse_event(MOUSEEVENTF_LEFTDOWN, (int)pt.X, (int)pt.Y, 0, 0);
                Thread.Sleep(20);
                mouse_event(MOUSEEVENTF_LEFTUP, (int)pt.X, (int)pt.Y, 0, 0);
                Thread.Sleep(50);
                mouse_event(MOUSEEVENTF_LEFTDOWN, (int)pt.X, (int)pt.Y, 0, 0);
                Thread.Sleep(20);
                mouse_event(MOUSEEVENTF_LEFTUP, (int)pt.X, (int)pt.Y, 0, 0);
                break;
            case InstructionType.SAT_RCLICK:
                pt = getHotSpot();
                mouse_event(MOUSEEVENTF_MOVE | isAbsolute, (int)pt.X, (int)pt.Y, 0, 0);
                mouseRandomCircle();
                Thread.Sleep(20);
                mouse_event(MOUSEEVENTF_RIGHTDOWN, (int)pt.X, (int)pt.Y, 0, 0);
                Thread.Sleep(20);
                mouse_event(MOUSEEVENTF_RIGHTUP, (int)pt.X, (int)pt.Y, 0, 0);
                break;
            case InstructionType.SAT_JSTMT:
                if (instParamLeft == "<end>") {
                    form.runFinished();
                    return false;
                } // if
                if (instParamLeft == "<next>") {
                    form.runNextStatement();
                    return false;
                } // if
                string jmpToStmt = form.substituteVars(instParamLeft);
                int jumpToStmtIndex = form.currFunc.stmtList.FindIndex(step => step.stmtLabel == jmpToStmt);
                if (jumpToStmtIndex != -1) {
                    form.debugOut("-- Jump to stmt:" + jmpToStmt);
                    form.uiStmtList.SelectedIndex = jumpToStmtIndex;
                    form.uiInstList.SelectedIndex = form.uiInstList.Items.Count == 0 ? -1 : 0;
                    form.debugOut("-- stmt:" + form.currStmt.stmtLabel);
                    if (form.currStmt.isCommentedOut) {
                        form.debugOut($"Error: jumping to commented out statement.{form.currStmt.stmtLabel}");
                        form.debugOut("Stopping program");
                        form.programState = PROGRAM_STATE.STOP;
                        form.uiFunctionRun.Enabled = !form.uiFunctionRun.Enabled;
                        form.uiProgRun.Enabled = !form.uiProgRun.Enabled;
                        break;
                    } // if
                    form.runWorker.RunWorkerAsync(form.currInst);
                    return false;
                } else {
                    form.debugOut($"Internal Error: statement not found: {instParamLeft}/{jmpToStmt} @statement=" +
                            form.currStmt.stmtLabel);
                    form.debugOut("Stopping program");
                    form.programState = PROGRAM_STATE.STOP;
                    form.uiFunctionRun.Enabled = !form.uiFunctionRun.Enabled;
                    form.uiProgRun.Enabled = !form.uiProgRun.Enabled;
                } // if
                break;
            case InstructionType.SAT_CALLF:
                var funcIndex = form.currProg.funcList.FindIndex(func => func.funcName == instParamLeft);
                if (funcIndex != -1) {
                    form.debugOut("Stack storing return address: f@" + form.currFunc.funcName +
                        ", s@" + form.currStmt.stmtLabel);
                    List<string> parameters = new List<string>();
                    parameters.InsertRange(0, instParamRight.Split(','));
                    for (int c = 0;c < parameters.Count;c ++) {
                        parameters[c] = form.substituteVars(parameters[c]);
                    } // for
                    if (!form.runContext.vars.ContainsKey("retVal")) {
                        form.runContext.vars["retVal"] = "<null>";
                    } // if
                    form.callStack.Push(new Tuple<FuncName, StmtLabel, RetVal, List<string>>(form.currFunc.funcName, form.currStmt.stmtLabel, form.runContext.vars["retVal"], parameters));
                    form.updateVariableTableRow($"retVal", "<?>", false, "All,Debug");
                    form.uiFuncList.SelectedIndex = funcIndex;
                    form.uiStmtList.SelectedIndex = form.uiStmtList.Items.Count == 0 ? -1 : 0;
                    form.uiInstList.SelectedIndex = form.uiInstList.Items.Count == 0 ? -1 : 0;
                    form.debugOut("-- Jump to func:" + form.currFunc.funcName);
                    var firstStmt = form.currProg.funcList[funcIndex].stmtList[0];
                    form.runWorker.RunWorkerAsync(form.currInst);
                    return false;
                } else {
                    switch (instParamLeft) { // check built-in functions
                        case "split": { // splits the string in to array using separator
                                string[] args = instParamRight.Split(',');
                                args = args.Select(item => form.substituteVars(item)).ToArray();
                                if (args[1].Length != 1) {
                                    form.debugOut($"Error: split separator must be a single char: {args[1]}");
                                } else {
                                    form.runContext.arrays["retVal"] = new List<string>(args[0].Split(args[1][0]));
                                    form.updateVariableTableRow($"@retVal", null, true, "All,Debug,Local");
                                } // if
                                return true;
                            } // case split
                        case "replace": { // replaces first string with 
                                string[] args = instParamRight.Split(',');
                                args = args.Select(item => form.substituteVars(item)).ToArray();
                                if (args.Length != 3) {
                                    form.debugOut($"Error: replace must be have three arguments.");
                                } else {
                                    form.runContext.vars["retVal"] = args[0].Replace(args[1], args[2]);
                                    form.updateVariableTableRow($"retVal", form.runContext.vars["retVal"], false, "All,Debug,Local");
                                } // if
                                return true;
                            } // case split
                        case "max": case "min": {
                                string[] args = instParamRight.Split(',');
                                args = args.Select(item => form.substituteVars(item)).ToArray();
                                try {
                                    int[] intArgs = args.Select(item => int.Parse(item, NumberStyles.Integer)).ToArray();
                                    var val = instParamLeft == "min" ? intArgs.Min() : intArgs.Max();
                                    form.runContext.vars["retVal"] = $"{val}";
                                } catch (Exception) {
                                    try {
                                        float[] intArgs = args.Select(item => float.Parse(item, NumberStyles.Float)).ToArray();
                                        var val = instParamLeft == "min" ? intArgs.Min() : intArgs.Max();
                                        form.runContext.vars["retVal"] = $"{val}";
                                    } catch (Exception) {
                                        var val = instParamLeft == "min" ? args.Min() : args.Max();
                                        form.runContext.vars["retVal"] = $"{val}";
                                    } // try
                                } // try
                                return true;
                            } // case min, max
                    } // switch
                    form.debugOut("Internal Error: function not found: " +
                        instParamLeft + " @statement=" + 
                        form.currStmt.stmtLabel);
                    form.debugOut("Stopping program");
                    form.programState = PROGRAM_STATE.STOP;
                    form.uiFunctionRun.Enabled = !form.uiFunctionRun.Enabled;
                    form.uiProgRun.Enabled = !form.uiProgRun.Enabled;
                } // if
                break;
            case InstructionType.SAT_MMOVE:
                pt = getHotSpot();
                mouse_event(MOUSEEVENTF_MOVE | isAbsolute, (int)pt.X, (int)pt.Y, 0, 0);
                mouseRandomCircle();
                break;
            case InstructionType.SAT_KEYPRESS:
                try {
                    SendKeys.Send(form.substituteVars(instParamLeft));
                } catch (Exception e) {
                    form.debugOut($"Exception: {e.Message}");
                } // try
                break;
            case InstructionType.SAT_FRETURN:
                if (form.callStack.Count != 0)
                { // if there is a caller in the stack, push the return value
                    var tuples = form.callStack.Pop();
                    var returnExpr = form.substituteVars(instParamLeft);
                    form.callStack.Push(new Tuple<FuncName, StmtLabel, RetVal, List<string>>(tuples.Item1, tuples.Item2, returnExpr, tuples.Item4
                        ));
                } // if
                form.runFinished();
                return false;
            case InstructionType.SAT_ASSIGN:
                if (instParamLeft[0] == '@') { // LHS array case
                    var colonIdx = instParamLeft.IndexOf(':');
                    var insertIdx = instParamLeft.IndexOf('<');
                    var removeIdx = instParamLeft.IndexOf('>');
                    var numMatches = colonIdx != -1 ? 1 : 0;
                    numMatches = insertIdx != -1 ? numMatches+1 : numMatches;
                    numMatches = removeIdx != -1 ? numMatches+1 : numMatches;
                    if (numMatches > 1)
                    {
                        form.uiStatusLabel.Text = "Error: Only  one of :,<,> can be used.";
                        form.programState = PROGRAM_STATE.STOP;
                        form.uiFunctionRun.Enabled = !form.uiFunctionRun.Enabled;
                        form.uiProgRun.Enabled = !form.uiProgRun.Enabled;
                        break;
                    } // if
                    var operatorIdx = colonIdx * insertIdx * removeIdx;
                    if (numMatches == 0) { // array create or destroy case
                        var arrName = instParamLeft.Substring(1);
                        if (instParamRight == "null")
                        { // remove array
                            form.runContext.arrays.Remove(arrName);
                            form.updateVariableTableRow($"@{arrName}", "null", true, "All,Debug,Locals,Globals,Persistents");
                        } else { // @arr = d,s,g,d,d,f
                            form.runContext.arrays[arrName] = new List<string>();
                            var items = instParamRight.Split(',');
                            foreach(var item in items) {
                                form.runContext.arrays[arrName].Add(form.substituteVars(item));
                            } // if
                            form.updateVariableTableRow($"@{arrName}", string.Join(", ", form.runContext.arrays[arrName]), true, "All,Debug,Locals,Globals,Persistents");
                        } // if
                    } else { // @arr:idx|front|back = case
                        var arrName = instParamLeft.Substring(1, operatorIdx - 1);
                        var indexVar = instParamLeft.Substring(operatorIdx + 1, instParamLeft.Length - operatorIdx - 1);
                        if (instParamLeft.EndsWith("back")) {
                            indexVar = $"{form.runContext.arrays[arrName].Count - 1}";
                        } else if (instParamLeft.EndsWith("front")) {
                            indexVar = $"{0}";
                        } // if
                        if (instParamRight == "null" && removeIdx == -1)
                        { // remove array element
                            form.uiStatusLabel.Text = "Error: RHS must be null when > operator is used.";
                            form.programState = PROGRAM_STATE.STOP;
                            form.uiFunctionRun.Enabled = !form.uiFunctionRun.Enabled;
                            form.uiProgRun.Enabled = !form.uiProgRun.Enabled;
                        } else {
                            var elemIdx = int.Parse(form.substituteVars(indexVar));
                            if (removeIdx == -1) {
                                form.runContext.arrays[arrName].Insert(elemIdx, form.substituteVars(instParamRight));
                                if (colonIdx != -1)
                                { // remove the old element
                                    if (form.runContext.arrays[arrName].Count > elemIdx + 1)
                                    {
                                        form.runContext.arrays[arrName].RemoveAt(elemIdx + 1);
                                    } // if
                                } else if (insertIdx == -1) {
                                    // LIE: just insert the new element
                                } // if
                            } else { // RHS is irrelevant
                                if (instParamRight != "null") {
                                    form.uiStatusLabel.Text = "Warning: RHS must be null @" + text;
                                } // if
                                form.runContext.arrays[arrName].RemoveAt(elemIdx);
                            } // if
                        } // if
                    } // if
                } else { // LHS variable case
                    if (instParamRight == "null") {
                        form.updateVariableTableRow(instParamLeft, "null", false, "All");
                        form.runContext.vars.Remove(instParamLeft);
                    } else {
                        form.runContext.vars[instParamLeft] = form.substituteVars(instParamRight);
                        form.updateVariableTableRow(instParamLeft, form.runContext.vars[instParamLeft], false, "All,Debug,Locals,Globals,Persistents");
                    } // if
                } // if
                break;
            case InstructionType.SAT_STOP_PROGRAM:
                form.debugOut("Stopping program");
                form.programState = PROGRAM_STATE.STOP;
                form.uiFunctionRun.Enabled = !form.uiFunctionRun.Enabled;
                form.uiProgRun.Enabled = !form.uiProgRun.Enabled;
                return false;
            case InstructionType.SAT_PRINTLN:
                var msg = form.substituteVars(instParamLeft);
                form.stdOut(msg);
                form.stdOut(Environment.NewLine);
                form.debugOut(msg);
                form.debugOut(Environment.NewLine);
                break;
        } // switch
        return true;
    } // function ExecInstContinue

    public object Clone() {
        Instruction retVal = new Instruction();
        retVal.instructionType = instructionType;
        retVal.instParamLeft = instParamLeft;
        retVal.instParamRight = instParamRight;
        retVal.instCondition = instCondition;
        retVal.screenLoc = screenLoc;
        retVal.isCommentedOut = isCommentedOut;
        return retVal;
    } // function Clone

    public Instruction(InstructionType _actionType, string _params, STMT_CONDITIONS _instCond, string _rightParam = "") {
        instructionType = _actionType;
        instParamLeft = _params;
        instCondition = _instCond;
        instParamRight = _rightParam;
        isCommentedOut = false;
    } // function Instruction

    public override string ToString() { return text; }
} // class Instruction

public enum STMT_CONDITIONS {AL,EQ,NE,GE,GT,LE,LT,INVALID} // enum STMT_JUMP_CONDITIONS

public class Statement : ICloneable {
    public Statement() {
        isCommentedOut = false;
        stmtLabel = "New Statement 1";
        instructions = new List<Instruction>();
    } // function Statement
    public Statement (string _label) : this() {
        stmtLabel = _label;
    } // function Statement
    public string text {
        get {
            return isCommentedOut ? $"// {stmtLabel}" : stmtLabel;
        } // get
    } // prop text
    public string stmtLabel;
    public bool isCommentedOut;
    public List<Instruction> instructions;

    public object Clone() {
        Statement retVal = new Statement();
        retVal.stmtLabel = stmtLabel;
        foreach(Instruction inst in instructions) {
            retVal.instructions.Add((Instruction)inst.Clone());
        } // foreach
        return retVal;
    } // function Clone
} // class Statement

public class Function : ICloneable {
    public Function () {
        stmtList = new List<Statement>();
        funcName = "New Function 1";
    } // function Function
    public string funcName;
    public List<Statement> stmtList;
    public object Clone() {
        Function retVal = new Function();
        retVal.funcName = funcName + "-Clone";
        foreach (Statement stmt in stmtList) {
            retVal.stmtList.Add((Statement)stmt.Clone());
        } // foreach
        return retVal;
    } // function Clone
} // class Function
public class ImageVar {
    private global::AIScreenAutomationApp.Form1 form {
        get {
            return global::AIScreenAutomationApp.Form1.gForm;
        } // get
    } // global::AIScreenAutomationApp.Form1 form
    public ImageVar() {
        searchRectTL = Point.Empty;
        searchRectBR = Point.Empty;
        isOCRRectBox = false;
        matchThreshold = "0.93";
    } // function ImageVar
    public ImageVar(Image _image, ImageLibrary il, PictureBox _pb) {
        try {
            image = _image;
            pb = _pb;
            isOCRRectBox = false;
            matchThreshold = "0.93";
            int x = image.Width / 2;
            int y = image.Height / 2;
            actionOffset = x.ToString() + ","+ y.ToString();
            bool isNamePresent(string name, ImageLibrary _il) {
                foreach (ImageVar iv in _il.imageVars) {
                    foreach (var ivName in iv.getImageNames) {
                        if (ivName == name) {
                            return true;
                        } // if
                    } // foreach
                } // foreach
                return false;
            } // function isNamePresent
            int count = 1;
            while (isNamePresent("Image_" + count, il)) {
                count++;
            } // while
            imageName = "Image_" + count;
        } catch (Exception ex) {
            form.uiStatusLabel.Text = $"Internal error: {ex.Message}{form.checkBreak}";
        } // try
    } // function ImageVar
    public void drawHotSpotOnPB() {
        try {
            pb.Image = image;
            form.updateTargetImage(ref pb, new Bitmap(pb.Image), actionOffset);
        } catch (Exception ex) {
            form.uiStatusLabel.Text = $"Internal error: {ex.Message}{form.checkBreak}";
        } // try
    } // function drawHotSpotOnPB
    public string imageName; // Comma separated list.
    [XmlIgnore]
    public PictureBox pb;
    [XmlIgnore]
    public Point actionOffsetPt {
        get {
            if (actionOffset == null || actionOffset == "") return new Point(0, 0);
            return new Point(int.Parse(actionOffset.Split(',')[0]), int.Parse(actionOffset.Split(',')[1]));
        } // get
    } // actionOffsetPt
    public string actionOffset; // "x,y" comma separated numbers
    public Point searchRectTL;
    public Point searchRectBR;
    public Point OCRRectTL;
    public Point OCRRectBR;
    public string matchThreshold;
    public bool isOCRRectBox;
    public string OCRParams;
    [XmlIgnore]
    public Image image;
    [XmlElement("Image")]
    public byte[] imageAsBase64 {
        get {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(image, typeof(byte[]));
        } // get
        set {
            if (value.Length > 0) {
                using (var ms = new MemoryStream(value)) {
                    image = Image.FromStream(ms);
                } // using
            } // if
        } // set
    } // imageAsBase64
    [XmlIgnore]
    public List<string> getImageNames {
        get {
            List<string> retVal = new List<string>();
            retVal.AddRange(imageName.Split(','));
            return retVal;
        } // get
    } // getImageNames
} // class ImageVar
public class ImageLibrary {
    private global::AIScreenAutomationApp.Form1 form {
        get {
            return global::AIScreenAutomationApp.Form1.gForm;
        } // get
    } // global::AIScreenAutomationApp.Form1 form
    public ImageLibrary() {
        imageVars = new List<ImageVar>();
        pb2ImageVar = new Dictionary<PictureBox, ImageVar>();
    } // function ImageLibrary
    public List<ImageVar> imageVars;
    public List<ImageVar> findImage(string imageName) {
        List<ImageVar> retVal = new List<ImageVar>();
        try { 
            string regex = Regex.Replace(imageName, "{.*?}", "(.*?)");
            foreach(ImageVar iv in imageVars) {
                foreach (var ivName in iv.getImageNames) {
                    if (Regex.IsMatch(ivName, $"^{regex}$")) {
                        retVal.Add(iv);
                    } // if
                } // foreach
            } // foreach
        } catch (Exception ex) {
            form.uiStatusLabel.Text = $"Internal error: {ex.Message}{form.checkBreak}";
        } // try
        return retVal;
    } // function findImage
    [XmlIgnore]
    public Dictionary<PictureBox, ImageVar> pb2ImageVar;
} // class ImageLibrary
public class Script {
    private global::AIScreenAutomationApp.Form1 form {
        get {
            return global::AIScreenAutomationApp.Form1.gForm;
        } // get
    } // global::AIScreenAutomationApp.Form1 form
    public Script() {
        progName = "New App 1";
        funcList = new List<Function>();
    } // function Program
    public Script(string name) : this() {
        progName = name;
    } // function Program
    public string progName;
    public List<Function> funcList;
//    public ImageLibrary imageLibrary;
} // class Program
public class Folder {
    private global::AIScreenAutomationApp.Form1 form {
        get {
            return global::AIScreenAutomationApp.Form1.gForm;
        } // get
    } // global::AIScreenAutomationApp.Form1 form
    public Folder() {
        progList = new List<Script>();
        imageLibrary = new ImageLibrary();
    } // function Folder
    public List<Script> progList;
    public ImageLibrary imageLibrary;
} // class Folder

