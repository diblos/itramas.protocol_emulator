<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form3
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.GroupVerbose = New System.Windows.Forms.GroupBox
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.Group1 = New System.Windows.Forms.GroupBox
        Me.btnCOM = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.GroupSerial = New System.Windows.Forms.GroupBox
        Me.ComboBaud = New System.Windows.Forms.ComboBox
        Me.ComboCOM = New System.Windows.Forms.ComboBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.GroupTCP = New System.Windows.Forms.GroupBox
        Me.LabelTCPPort = New System.Windows.Forms.Label
        Me.LabelTCPIP = New System.Windows.Forms.Label
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.txtIP = New System.Windows.Forms.TextBox
        Me.TabPage3 = New System.Windows.Forms.TabPage
        Me.GroupTCP2 = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtPort_RSM = New System.Windows.Forms.TextBox
        Me.txtIP_RSM = New System.Windows.Forms.TextBox
        Me.TabFTP = New System.Windows.Forms.TabPage
        Me.GroupFTP = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtPWD = New System.Windows.Forms.TextBox
        Me.txtUID = New System.Windows.Forms.TextBox
        Me.Group2 = New System.Windows.Forms.GroupBox
        Me.TabControl2 = New System.Windows.Forms.TabControl
        Me.TabPage4 = New System.Windows.Forms.TabPage
        Me.Button1 = New System.Windows.Forms.Button
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.TabPage5 = New System.Windows.Forms.TabPage
        Me.gbTaxi = New System.Windows.Forms.GroupBox
        Me.btnUpdateTaxi = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtPlate_DC = New System.Windows.Forms.TextBox
        Me.txtTrackerID_DC = New System.Windows.Forms.TextBox
        Me.gbDriver = New System.Windows.Forms.GroupBox
        Me.btnLoginDrv = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtDrvPwd = New System.Windows.Forms.TextBox
        Me.txtDriver = New System.Windows.Forms.TextBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.cbSOS_DC = New System.Windows.Forms.CheckBox
        Me.TabPage6 = New System.Windows.Forms.TabPage
        Me.btnContextUpdate_RSM = New System.Windows.Forms.Button
        Me.cbSOS_RSM = New System.Windows.Forms.CheckBox
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.btnValidatetaxi = New System.Windows.Forms.Button
        Me.GroupVerbose.SuspendLayout()
        Me.Group1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupSerial.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupTCP.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.GroupTCP2.SuspendLayout()
        Me.TabFTP.SuspendLayout()
        Me.GroupFTP.SuspendLayout()
        Me.Group2.SuspendLayout()
        Me.TabControl2.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage5.SuspendLayout()
        Me.gbTaxi.SuspendLayout()
        Me.gbDriver.SuspendLayout()
        Me.TabPage6.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupVerbose
        '
        Me.GroupVerbose.Controls.Add(Me.ListBox1)
        Me.GroupVerbose.Location = New System.Drawing.Point(12, 248)
        Me.GroupVerbose.Name = "GroupVerbose"
        Me.GroupVerbose.Size = New System.Drawing.Size(468, 143)
        Me.GroupVerbose.TabIndex = 2
        Me.GroupVerbose.TabStop = False
        Me.GroupVerbose.Text = "Verbose"
        '
        'ListBox1
        '
        Me.ListBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(3, 16)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(462, 121)
        Me.ListBox1.TabIndex = 2
        Me.ListBox1.TabStop = False
        '
        'Group1
        '
        Me.Group1.Controls.Add(Me.btnCOM)
        Me.Group1.Controls.Add(Me.TabControl1)
        Me.Group1.Location = New System.Drawing.Point(12, 12)
        Me.Group1.Name = "Group1"
        Me.Group1.Size = New System.Drawing.Size(200, 217)
        Me.Group1.TabIndex = 3
        Me.Group1.TabStop = False
        Me.Group1.Text = "Connection Settings"
        '
        'btnCOM
        '
        Me.btnCOM.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnCOM.Location = New System.Drawing.Point(3, 191)
        Me.btnCOM.Name = "btnCOM"
        Me.btnCOM.Size = New System.Drawing.Size(194, 23)
        Me.btnCOM.TabIndex = 8
        Me.btnCOM.Text = "Start"
        Me.btnCOM.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabFTP)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TabControl1.Location = New System.Drawing.Point(3, 16)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(194, 177)
        Me.TabControl1.TabIndex = 7
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupSerial)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(186, 151)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Roof Top Signage"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupSerial
        '
        Me.GroupSerial.Controls.Add(Me.ComboBaud)
        Me.GroupSerial.Controls.Add(Me.ComboCOM)
        Me.GroupSerial.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupSerial.Location = New System.Drawing.Point(3, 3)
        Me.GroupSerial.Name = "GroupSerial"
        Me.GroupSerial.Size = New System.Drawing.Size(180, 145)
        Me.GroupSerial.TabIndex = 2
        Me.GroupSerial.TabStop = False
        Me.GroupSerial.Text = "Serial Connection"
        '
        'ComboBaud
        '
        Me.ComboBaud.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBaud.FormattingEnabled = True
        Me.ComboBaud.Location = New System.Drawing.Point(3, 37)
        Me.ComboBaud.Name = "ComboBaud"
        Me.ComboBaud.Size = New System.Drawing.Size(174, 21)
        Me.ComboBaud.TabIndex = 5
        '
        'ComboCOM
        '
        Me.ComboCOM.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboCOM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboCOM.FormattingEnabled = True
        Me.ComboCOM.Location = New System.Drawing.Point(3, 16)
        Me.ComboCOM.Name = "ComboCOM"
        Me.ComboCOM.Size = New System.Drawing.Size(174, 21)
        Me.ComboCOM.TabIndex = 4
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupTCP)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(186, 151)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Driver Console"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupTCP
        '
        Me.GroupTCP.Controls.Add(Me.LabelTCPPort)
        Me.GroupTCP.Controls.Add(Me.LabelTCPIP)
        Me.GroupTCP.Controls.Add(Me.txtPort)
        Me.GroupTCP.Controls.Add(Me.txtIP)
        Me.GroupTCP.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupTCP.Location = New System.Drawing.Point(3, 3)
        Me.GroupTCP.Name = "GroupTCP"
        Me.GroupTCP.Size = New System.Drawing.Size(180, 145)
        Me.GroupTCP.TabIndex = 1
        Me.GroupTCP.TabStop = False
        Me.GroupTCP.Text = "TCP Connention"
        '
        'LabelTCPPort
        '
        Me.LabelTCPPort.AutoSize = True
        Me.LabelTCPPort.Location = New System.Drawing.Point(7, 48)
        Me.LabelTCPPort.Name = "LabelTCPPort"
        Me.LabelTCPPort.Size = New System.Drawing.Size(26, 13)
        Me.LabelTCPPort.TabIndex = 4
        Me.LabelTCPPort.Text = "Port"
        '
        'LabelTCPIP
        '
        Me.LabelTCPIP.AutoSize = True
        Me.LabelTCPIP.Location = New System.Drawing.Point(7, 25)
        Me.LabelTCPIP.Name = "LabelTCPIP"
        Me.LabelTCPIP.Size = New System.Drawing.Size(17, 13)
        Me.LabelTCPIP.TabIndex = 3
        Me.LabelTCPIP.Text = "IP"
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(88, 45)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(100, 20)
        Me.txtPort.TabIndex = 1
        '
        'txtIP
        '
        Me.txtIP.Location = New System.Drawing.Point(88, 19)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(100, 20)
        Me.txtIP.TabIndex = 0
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.GroupTCP2)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(186, 151)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Rear Seat Monitor"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'GroupTCP2
        '
        Me.GroupTCP2.Controls.Add(Me.Label1)
        Me.GroupTCP2.Controls.Add(Me.Label2)
        Me.GroupTCP2.Controls.Add(Me.txtPort_RSM)
        Me.GroupTCP2.Controls.Add(Me.txtIP_RSM)
        Me.GroupTCP2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupTCP2.Location = New System.Drawing.Point(3, 3)
        Me.GroupTCP2.Name = "GroupTCP2"
        Me.GroupTCP2.Size = New System.Drawing.Size(180, 145)
        Me.GroupTCP2.TabIndex = 2
        Me.GroupTCP2.TabStop = False
        Me.GroupTCP2.Text = "TCP Connention"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(26, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Port"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(7, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(17, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "IP"
        '
        'txtPort_RSM
        '
        Me.txtPort_RSM.Location = New System.Drawing.Point(88, 45)
        Me.txtPort_RSM.Name = "txtPort_RSM"
        Me.txtPort_RSM.Size = New System.Drawing.Size(100, 20)
        Me.txtPort_RSM.TabIndex = 1
        '
        'txtIP_RSM
        '
        Me.txtIP_RSM.Location = New System.Drawing.Point(88, 19)
        Me.txtIP_RSM.Name = "txtIP_RSM"
        Me.txtIP_RSM.Size = New System.Drawing.Size(100, 20)
        Me.txtIP_RSM.TabIndex = 0
        '
        'TabFTP
        '
        Me.TabFTP.Controls.Add(Me.GroupFTP)
        Me.TabFTP.Location = New System.Drawing.Point(4, 22)
        Me.TabFTP.Name = "TabFTP"
        Me.TabFTP.Padding = New System.Windows.Forms.Padding(3)
        Me.TabFTP.Size = New System.Drawing.Size(186, 151)
        Me.TabFTP.TabIndex = 3
        Me.TabFTP.Text = "FTP"
        Me.TabFTP.UseVisualStyleBackColor = True
        '
        'GroupFTP
        '
        Me.GroupFTP.Controls.Add(Me.Label3)
        Me.GroupFTP.Controls.Add(Me.Label4)
        Me.GroupFTP.Controls.Add(Me.txtPWD)
        Me.GroupFTP.Controls.Add(Me.txtUID)
        Me.GroupFTP.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupFTP.Location = New System.Drawing.Point(3, 3)
        Me.GroupFTP.Name = "GroupFTP"
        Me.GroupFTP.Size = New System.Drawing.Size(180, 145)
        Me.GroupFTP.TabIndex = 3
        Me.GroupFTP.TabStop = False
        Me.GroupFTP.Text = "Account Settings"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(53, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Password"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(55, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Username"
        '
        'txtPWD
        '
        Me.txtPWD.Location = New System.Drawing.Point(88, 45)
        Me.txtPWD.Name = "txtPWD"
        Me.txtPWD.Size = New System.Drawing.Size(100, 20)
        Me.txtPWD.TabIndex = 1
        '
        'txtUID
        '
        Me.txtUID.Location = New System.Drawing.Point(88, 19)
        Me.txtUID.Name = "txtUID"
        Me.txtUID.Size = New System.Drawing.Size(100, 20)
        Me.txtUID.TabIndex = 0
        '
        'Group2
        '
        Me.Group2.Controls.Add(Me.TabControl2)
        Me.Group2.Location = New System.Drawing.Point(243, 12)
        Me.Group2.Name = "Group2"
        Me.Group2.Size = New System.Drawing.Size(246, 214)
        Me.Group2.TabIndex = 4
        Me.Group2.TabStop = False
        Me.Group2.Text = "Controls"
        '
        'TabControl2
        '
        Me.TabControl2.Controls.Add(Me.TabPage4)
        Me.TabControl2.Controls.Add(Me.TabPage5)
        Me.TabControl2.Controls.Add(Me.TabPage6)
        Me.TabControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl2.Enabled = False
        Me.TabControl2.Location = New System.Drawing.Point(3, 16)
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(240, 195)
        Me.TabControl2.TabIndex = 4
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.Button1)
        Me.TabPage4.Controls.Add(Me.DataGridView1)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(232, 169)
        Me.TabPage4.TabIndex = 0
        Me.TabPage4.Text = "Roof Top Signage"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Button1.Location = New System.Drawing.Point(3, 143)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(226, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Test"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Top
        Me.DataGridView1.Location = New System.Drawing.Point(3, 3)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(226, 37)
        Me.DataGridView1.TabIndex = 3
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.gbTaxi)
        Me.TabPage5.Controls.Add(Me.gbDriver)
        Me.TabPage5.Controls.Add(Me.Button2)
        Me.TabPage5.Controls.Add(Me.cbSOS_DC)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(232, 169)
        Me.TabPage5.TabIndex = 1
        Me.TabPage5.Text = "Driver Console"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'gbTaxi
        '
        Me.gbTaxi.Controls.Add(Me.btnValidatetaxi)
        Me.gbTaxi.Controls.Add(Me.btnUpdateTaxi)
        Me.gbTaxi.Controls.Add(Me.Label7)
        Me.gbTaxi.Controls.Add(Me.Label8)
        Me.gbTaxi.Controls.Add(Me.txtPlate_DC)
        Me.gbTaxi.Controls.Add(Me.txtTrackerID_DC)
        Me.gbTaxi.Dock = System.Windows.Forms.DockStyle.Right
        Me.gbTaxi.Location = New System.Drawing.Point(113, 3)
        Me.gbTaxi.Name = "gbTaxi"
        Me.gbTaxi.Size = New System.Drawing.Size(116, 123)
        Me.gbTaxi.TabIndex = 5
        Me.gbTaxi.TabStop = False
        Me.gbTaxi.Text = "Update Taxi profile"
        '
        'btnUpdateTaxi
        '
        Me.btnUpdateTaxi.Location = New System.Drawing.Point(10, 73)
        Me.btnUpdateTaxi.Name = "btnUpdateTaxi"
        Me.btnUpdateTaxi.Size = New System.Drawing.Size(51, 23)
        Me.btnUpdateTaxi.TabIndex = 5
        Me.btnUpdateTaxi.Text = "Update"
        Me.btnUpdateTaxi.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(7, 48)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(48, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Plate No"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(7, 25)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(55, 13)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "TrackerID"
        '
        'txtPlate_DC
        '
        Me.txtPlate_DC.Location = New System.Drawing.Point(88, 45)
        Me.txtPlate_DC.MaxLength = 10
        Me.txtPlate_DC.Name = "txtPlate_DC"
        Me.txtPlate_DC.Size = New System.Drawing.Size(100, 20)
        Me.txtPlate_DC.TabIndex = 1
        '
        'txtTrackerID_DC
        '
        Me.txtTrackerID_DC.Location = New System.Drawing.Point(88, 19)
        Me.txtTrackerID_DC.MaxLength = 50
        Me.txtTrackerID_DC.Name = "txtTrackerID_DC"
        Me.txtTrackerID_DC.Size = New System.Drawing.Size(100, 20)
        Me.txtTrackerID_DC.TabIndex = 0
        '
        'gbDriver
        '
        Me.gbDriver.Controls.Add(Me.btnLoginDrv)
        Me.gbDriver.Controls.Add(Me.Label5)
        Me.gbDriver.Controls.Add(Me.Label6)
        Me.gbDriver.Controls.Add(Me.txtDrvPwd)
        Me.gbDriver.Controls.Add(Me.txtDriver)
        Me.gbDriver.Dock = System.Windows.Forms.DockStyle.Left
        Me.gbDriver.Location = New System.Drawing.Point(3, 3)
        Me.gbDriver.Name = "gbDriver"
        Me.gbDriver.Size = New System.Drawing.Size(196, 123)
        Me.gbDriver.TabIndex = 4
        Me.gbDriver.TabStop = False
        Me.gbDriver.Text = "Driver Login"
        '
        'btnLoginDrv
        '
        Me.btnLoginDrv.Location = New System.Drawing.Point(10, 73)
        Me.btnLoginDrv.Name = "btnLoginDrv"
        Me.btnLoginDrv.Size = New System.Drawing.Size(50, 23)
        Me.btnLoginDrv.TabIndex = 5
        Me.btnLoginDrv.Text = "Login"
        Me.btnLoginDrv.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 48)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(53, 13)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Password"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(7, 25)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(64, 13)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Driver name"
        '
        'txtDrvPwd
        '
        Me.txtDrvPwd.Location = New System.Drawing.Point(88, 45)
        Me.txtDrvPwd.MaxLength = 10
        Me.txtDrvPwd.Name = "txtDrvPwd"
        Me.txtDrvPwd.Size = New System.Drawing.Size(100, 20)
        Me.txtDrvPwd.TabIndex = 1
        '
        'txtDriver
        '
        Me.txtDriver.Location = New System.Drawing.Point(88, 19)
        Me.txtDriver.MaxLength = 50
        Me.txtDriver.Name = "txtDriver"
        Me.txtDriver.Size = New System.Drawing.Size(100, 20)
        Me.txtDriver.TabIndex = 0
        '
        'Button2
        '
        Me.Button2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Button2.Location = New System.Drawing.Point(3, 126)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(226, 23)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        Me.Button2.Visible = False
        '
        'cbSOS_DC
        '
        Me.cbSOS_DC.AutoSize = True
        Me.cbSOS_DC.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.cbSOS_DC.Location = New System.Drawing.Point(3, 149)
        Me.cbSOS_DC.Name = "cbSOS_DC"
        Me.cbSOS_DC.Size = New System.Drawing.Size(226, 17)
        Me.cbSOS_DC.TabIndex = 0
        Me.cbSOS_DC.Text = "SOS"
        Me.cbSOS_DC.UseVisualStyleBackColor = True
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.btnContextUpdate_RSM)
        Me.TabPage6.Controls.Add(Me.cbSOS_RSM)
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage6.Size = New System.Drawing.Size(232, 169)
        Me.TabPage6.TabIndex = 2
        Me.TabPage6.Text = "Rear Seat Monitor"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'btnContextUpdate_RSM
        '
        Me.btnContextUpdate_RSM.Location = New System.Drawing.Point(3, 126)
        Me.btnContextUpdate_RSM.Name = "btnContextUpdate_RSM"
        Me.btnContextUpdate_RSM.Size = New System.Drawing.Size(226, 23)
        Me.btnContextUpdate_RSM.TabIndex = 1
        Me.btnContextUpdate_RSM.Text = "Request Context Update"
        Me.btnContextUpdate_RSM.UseVisualStyleBackColor = True
        '
        'cbSOS_RSM
        '
        Me.cbSOS_RSM.AutoSize = True
        Me.cbSOS_RSM.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.cbSOS_RSM.Location = New System.Drawing.Point(3, 149)
        Me.cbSOS_RSM.Name = "cbSOS_RSM"
        Me.cbSOS_RSM.Size = New System.Drawing.Size(226, 17)
        Me.cbSOS_RSM.TabIndex = 0
        Me.cbSOS_RSM.Text = "SOS"
        Me.cbSOS_RSM.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 381)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(536, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'btnValidatetaxi
        '
        Me.btnValidatetaxi.Location = New System.Drawing.Point(67, 73)
        Me.btnValidatetaxi.Name = "btnValidatetaxi"
        Me.btnValidatetaxi.Size = New System.Drawing.Size(56, 23)
        Me.btnValidatetaxi.TabIndex = 6
        Me.btnValidatetaxi.Text = "Validate"
        Me.btnValidatetaxi.UseVisualStyleBackColor = True
        '
        'Form3
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(536, 403)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Group2)
        Me.Controls.Add(Me.Group1)
        Me.Controls.Add(Me.GroupVerbose)
        Me.Name = "Form3"
        Me.Text = "Form3"
        Me.GroupVerbose.ResumeLayout(False)
        Me.Group1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupSerial.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.GroupTCP.ResumeLayout(False)
        Me.GroupTCP.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.GroupTCP2.ResumeLayout(False)
        Me.GroupTCP2.PerformLayout()
        Me.TabFTP.ResumeLayout(False)
        Me.GroupFTP.ResumeLayout(False)
        Me.GroupFTP.PerformLayout()
        Me.Group2.ResumeLayout(False)
        Me.TabControl2.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.gbTaxi.ResumeLayout(False)
        Me.gbTaxi.PerformLayout()
        Me.gbDriver.ResumeLayout(False)
        Me.gbDriver.PerformLayout()
        Me.TabPage6.ResumeLayout(False)
        Me.TabPage6.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupVerbose As System.Windows.Forms.GroupBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Group1 As System.Windows.Forms.GroupBox
    Friend WithEvents Group2 As System.Windows.Forms.GroupBox
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents GroupSerial As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBaud As System.Windows.Forms.ComboBox
    Friend WithEvents ComboCOM As System.Windows.Forms.ComboBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents GroupTCP As System.Windows.Forms.GroupBox
    Friend WithEvents LabelTCPPort As System.Windows.Forms.Label
    Friend WithEvents LabelTCPIP As System.Windows.Forms.Label
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents txtIP As System.Windows.Forms.TextBox
    Friend WithEvents btnCOM As System.Windows.Forms.Button
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents GroupTCP2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtPort_RSM As System.Windows.Forms.TextBox
    Friend WithEvents txtIP_RSM As System.Windows.Forms.TextBox
    Friend WithEvents TabControl2 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage6 As System.Windows.Forms.TabPage
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents cbSOS_DC As System.Windows.Forms.CheckBox
    Friend WithEvents cbSOS_RSM As System.Windows.Forms.CheckBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents btnContextUpdate_RSM As System.Windows.Forms.Button
    Friend WithEvents TabFTP As System.Windows.Forms.TabPage
    Friend WithEvents GroupFTP As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtPWD As System.Windows.Forms.TextBox
    Friend WithEvents txtUID As System.Windows.Forms.TextBox
    Friend WithEvents gbDriver As System.Windows.Forms.GroupBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents txtDrvPwd As System.Windows.Forms.TextBox
    Friend WithEvents txtDriver As System.Windows.Forms.TextBox
    Friend WithEvents btnLoginDrv As System.Windows.Forms.Button
    Friend WithEvents gbTaxi As System.Windows.Forms.GroupBox
    Friend WithEvents btnUpdateTaxi As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtPlate_DC As System.Windows.Forms.TextBox
    Friend WithEvents txtTrackerID_DC As System.Windows.Forms.TextBox
    Friend WithEvents btnValidatetaxi As System.Windows.Forms.Button

End Class
