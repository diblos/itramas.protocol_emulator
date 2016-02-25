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
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabTM = New System.Windows.Forms.TabPage
        Me.btnCOM_TM = New System.Windows.Forms.Button
        Me.GroupSerial_TM = New System.Windows.Forms.GroupBox
        Me.ComboBaud_TM = New System.Windows.Forms.ComboBox
        Me.ComboCOM_TM = New System.Windows.Forms.ComboBox
        Me.TabCT = New System.Windows.Forms.TabPage
        Me.btnCOM_CT = New System.Windows.Forms.Button
        Me.GroupBox_CT = New System.Windows.Forms.GroupBox
        Me.ComboBaud_CT = New System.Windows.Forms.ComboBox
        Me.ComboCOM_CT = New System.Windows.Forms.ComboBox
        Me.TabRTS = New System.Windows.Forms.TabPage
        Me.btnCOM_RTS = New System.Windows.Forms.Button
        Me.GroupBox_RTS = New System.Windows.Forms.GroupBox
        Me.ComboBaud_RTS = New System.Windows.Forms.ComboBox
        Me.ComboCOM_RTS = New System.Windows.Forms.ComboBox
        Me.Group2 = New System.Windows.Forms.GroupBox
        Me.Label_Stat_RTS = New System.Windows.Forms.Label
        Me.Label_Stat_CT = New System.Windows.Forms.Label
        Me.Label_Stat_TM = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.GroupVerbose.SuspendLayout()
        Me.Group1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabTM.SuspendLayout()
        Me.GroupSerial_TM.SuspendLayout()
        Me.TabCT.SuspendLayout()
        Me.GroupBox_CT.SuspendLayout()
        Me.TabRTS.SuspendLayout()
        Me.GroupBox_RTS.SuspendLayout()
        Me.Group2.SuspendLayout()
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
        Me.Group1.Controls.Add(Me.TabControl1)
        Me.Group1.Location = New System.Drawing.Point(12, 12)
        Me.Group1.Name = "Group1"
        Me.Group1.Size = New System.Drawing.Size(241, 217)
        Me.Group1.TabIndex = 3
        Me.Group1.TabStop = False
        Me.Group1.Text = "Connection Settings"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabTM)
        Me.TabControl1.Controls.Add(Me.TabCT)
        Me.TabControl1.Controls.Add(Me.TabRTS)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TabControl1.Location = New System.Drawing.Point(3, 16)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(235, 177)
        Me.TabControl1.TabIndex = 7
        '
        'TabTM
        '
        Me.TabTM.Controls.Add(Me.btnCOM_TM)
        Me.TabTM.Controls.Add(Me.GroupSerial_TM)
        Me.TabTM.Location = New System.Drawing.Point(4, 22)
        Me.TabTM.Name = "TabTM"
        Me.TabTM.Padding = New System.Windows.Forms.Padding(3)
        Me.TabTM.Size = New System.Drawing.Size(227, 151)
        Me.TabTM.TabIndex = 0
        Me.TabTM.Text = "Taxi Meter"
        Me.TabTM.UseVisualStyleBackColor = True
        '
        'btnCOM_TM
        '
        Me.btnCOM_TM.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnCOM_TM.Location = New System.Drawing.Point(3, 125)
        Me.btnCOM_TM.Name = "btnCOM_TM"
        Me.btnCOM_TM.Size = New System.Drawing.Size(221, 23)
        Me.btnCOM_TM.TabIndex = 10
        Me.btnCOM_TM.Text = "Start"
        Me.btnCOM_TM.UseVisualStyleBackColor = True
        '
        'GroupSerial_TM
        '
        Me.GroupSerial_TM.Controls.Add(Me.ComboBaud_TM)
        Me.GroupSerial_TM.Controls.Add(Me.ComboCOM_TM)
        Me.GroupSerial_TM.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupSerial_TM.Location = New System.Drawing.Point(3, 3)
        Me.GroupSerial_TM.Name = "GroupSerial_TM"
        Me.GroupSerial_TM.Size = New System.Drawing.Size(221, 120)
        Me.GroupSerial_TM.TabIndex = 2
        Me.GroupSerial_TM.TabStop = False
        Me.GroupSerial_TM.Text = "Serial Connection"
        '
        'ComboBaud_TM
        '
        Me.ComboBaud_TM.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBaud_TM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBaud_TM.FormattingEnabled = True
        Me.ComboBaud_TM.Location = New System.Drawing.Point(3, 37)
        Me.ComboBaud_TM.Name = "ComboBaud_TM"
        Me.ComboBaud_TM.Size = New System.Drawing.Size(215, 21)
        Me.ComboBaud_TM.TabIndex = 5
        '
        'ComboCOM_TM
        '
        Me.ComboCOM_TM.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboCOM_TM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboCOM_TM.FormattingEnabled = True
        Me.ComboCOM_TM.Location = New System.Drawing.Point(3, 16)
        Me.ComboCOM_TM.Name = "ComboCOM_TM"
        Me.ComboCOM_TM.Size = New System.Drawing.Size(215, 21)
        Me.ComboCOM_TM.TabIndex = 4
        '
        'TabCT
        '
        Me.TabCT.Controls.Add(Me.btnCOM_CT)
        Me.TabCT.Controls.Add(Me.GroupBox_CT)
        Me.TabCT.Location = New System.Drawing.Point(4, 22)
        Me.TabCT.Name = "TabCT"
        Me.TabCT.Padding = New System.Windows.Forms.Padding(3)
        Me.TabCT.Size = New System.Drawing.Size(227, 151)
        Me.TabCT.TabIndex = 3
        Me.TabCT.Text = "Cashless Terminal"
        Me.TabCT.UseVisualStyleBackColor = True
        '
        'btnCOM_CT
        '
        Me.btnCOM_CT.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnCOM_CT.Location = New System.Drawing.Point(3, 125)
        Me.btnCOM_CT.Name = "btnCOM_CT"
        Me.btnCOM_CT.Size = New System.Drawing.Size(221, 23)
        Me.btnCOM_CT.TabIndex = 11
        Me.btnCOM_CT.Text = "Start"
        Me.btnCOM_CT.UseVisualStyleBackColor = True
        '
        'GroupBox_CT
        '
        Me.GroupBox_CT.Controls.Add(Me.ComboBaud_CT)
        Me.GroupBox_CT.Controls.Add(Me.ComboCOM_CT)
        Me.GroupBox_CT.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox_CT.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox_CT.Name = "GroupBox_CT"
        Me.GroupBox_CT.Size = New System.Drawing.Size(221, 120)
        Me.GroupBox_CT.TabIndex = 3
        Me.GroupBox_CT.TabStop = False
        Me.GroupBox_CT.Text = "Serial Connection"
        '
        'ComboBaud_CT
        '
        Me.ComboBaud_CT.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBaud_CT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBaud_CT.FormattingEnabled = True
        Me.ComboBaud_CT.Location = New System.Drawing.Point(3, 37)
        Me.ComboBaud_CT.Name = "ComboBaud_CT"
        Me.ComboBaud_CT.Size = New System.Drawing.Size(215, 21)
        Me.ComboBaud_CT.TabIndex = 5
        '
        'ComboCOM_CT
        '
        Me.ComboCOM_CT.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboCOM_CT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboCOM_CT.FormattingEnabled = True
        Me.ComboCOM_CT.Location = New System.Drawing.Point(3, 16)
        Me.ComboCOM_CT.Name = "ComboCOM_CT"
        Me.ComboCOM_CT.Size = New System.Drawing.Size(215, 21)
        Me.ComboCOM_CT.TabIndex = 4
        '
        'TabRTS
        '
        Me.TabRTS.Controls.Add(Me.btnCOM_RTS)
        Me.TabRTS.Controls.Add(Me.GroupBox_RTS)
        Me.TabRTS.Location = New System.Drawing.Point(4, 22)
        Me.TabRTS.Name = "TabRTS"
        Me.TabRTS.Padding = New System.Windows.Forms.Padding(3)
        Me.TabRTS.Size = New System.Drawing.Size(227, 151)
        Me.TabRTS.TabIndex = 4
        Me.TabRTS.Text = "Roof Top Signage"
        Me.TabRTS.UseVisualStyleBackColor = True
        '
        'btnCOM_RTS
        '
        Me.btnCOM_RTS.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnCOM_RTS.Location = New System.Drawing.Point(3, 125)
        Me.btnCOM_RTS.Name = "btnCOM_RTS"
        Me.btnCOM_RTS.Size = New System.Drawing.Size(221, 23)
        Me.btnCOM_RTS.TabIndex = 11
        Me.btnCOM_RTS.Text = "Start"
        Me.btnCOM_RTS.UseVisualStyleBackColor = True
        '
        'GroupBox_RTS
        '
        Me.GroupBox_RTS.Controls.Add(Me.ComboBaud_RTS)
        Me.GroupBox_RTS.Controls.Add(Me.ComboCOM_RTS)
        Me.GroupBox_RTS.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupBox_RTS.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox_RTS.Name = "GroupBox_RTS"
        Me.GroupBox_RTS.Size = New System.Drawing.Size(221, 120)
        Me.GroupBox_RTS.TabIndex = 3
        Me.GroupBox_RTS.TabStop = False
        Me.GroupBox_RTS.Text = "Serial Connection"
        '
        'ComboBaud_RTS
        '
        Me.ComboBaud_RTS.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBaud_RTS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBaud_RTS.FormattingEnabled = True
        Me.ComboBaud_RTS.Location = New System.Drawing.Point(3, 37)
        Me.ComboBaud_RTS.Name = "ComboBaud_RTS"
        Me.ComboBaud_RTS.Size = New System.Drawing.Size(215, 21)
        Me.ComboBaud_RTS.TabIndex = 5
        '
        'ComboCOM_RTS
        '
        Me.ComboCOM_RTS.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboCOM_RTS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboCOM_RTS.FormattingEnabled = True
        Me.ComboCOM_RTS.Location = New System.Drawing.Point(3, 16)
        Me.ComboCOM_RTS.Name = "ComboCOM_RTS"
        Me.ComboCOM_RTS.Size = New System.Drawing.Size(215, 21)
        Me.ComboCOM_RTS.TabIndex = 4
        '
        'Group2
        '
        Me.Group2.Controls.Add(Me.Label_Stat_RTS)
        Me.Group2.Controls.Add(Me.Label_Stat_CT)
        Me.Group2.Controls.Add(Me.Label_Stat_TM)
        Me.Group2.Controls.Add(Me.Label3)
        Me.Group2.Controls.Add(Me.Label2)
        Me.Group2.Controls.Add(Me.Label1)
        Me.Group2.Location = New System.Drawing.Point(278, 12)
        Me.Group2.Name = "Group2"
        Me.Group2.Size = New System.Drawing.Size(246, 214)
        Me.Group2.TabIndex = 4
        Me.Group2.TabStop = False
        Me.Group2.Text = "Controls"
        '
        'Label_Stat_RTS
        '
        Me.Label_Stat_RTS.AutoSize = True
        Me.Label_Stat_RTS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label_Stat_RTS.Location = New System.Drawing.Point(142, 72)
        Me.Label_Stat_RTS.Name = "Label_Stat_RTS"
        Me.Label_Stat_RTS.Size = New System.Drawing.Size(2, 15)
        Me.Label_Stat_RTS.TabIndex = 8
        '
        'Label_Stat_CT
        '
        Me.Label_Stat_CT.AutoSize = True
        Me.Label_Stat_CT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label_Stat_CT.Location = New System.Drawing.Point(142, 50)
        Me.Label_Stat_CT.Name = "Label_Stat_CT"
        Me.Label_Stat_CT.Size = New System.Drawing.Size(2, 15)
        Me.Label_Stat_CT.TabIndex = 7
        '
        'Label_Stat_TM
        '
        Me.Label_Stat_TM.AutoSize = True
        Me.Label_Stat_TM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label_Stat_TM.Location = New System.Drawing.Point(142, 28)
        Me.Label_Stat_TM.Name = "Label_Stat_TM"
        Me.Label_Stat_TM.Size = New System.Drawing.Size(2, 15)
        Me.Label_Stat_TM.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(26, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Rooftop Signage"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(26, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(92, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Cashless Terminal"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(26, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(57, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Taxi Meter"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 381)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(536, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
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
        Me.TabTM.ResumeLayout(False)
        Me.GroupSerial_TM.ResumeLayout(False)
        Me.TabCT.ResumeLayout(False)
        Me.GroupBox_CT.ResumeLayout(False)
        Me.TabRTS.ResumeLayout(False)
        Me.GroupBox_RTS.ResumeLayout(False)
        Me.Group2.ResumeLayout(False)
        Me.Group2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupVerbose As System.Windows.Forms.GroupBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents Group1 As System.Windows.Forms.GroupBox
    Friend WithEvents Group2 As System.Windows.Forms.GroupBox
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabTM As System.Windows.Forms.TabPage
    Friend WithEvents GroupSerial_TM As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBaud_TM As System.Windows.Forms.ComboBox
    Friend WithEvents ComboCOM_TM As System.Windows.Forms.ComboBox
    Friend WithEvents TabCT As System.Windows.Forms.TabPage
    Friend WithEvents TabRTS As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox_CT As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBaud_CT As System.Windows.Forms.ComboBox
    Friend WithEvents ComboCOM_CT As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox_RTS As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBaud_RTS As System.Windows.Forms.ComboBox
    Friend WithEvents ComboCOM_RTS As System.Windows.Forms.ComboBox
    Friend WithEvents btnCOM_TM As System.Windows.Forms.Button
    Friend WithEvents btnCOM_CT As System.Windows.Forms.Button
    Friend WithEvents btnCOM_RTS As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label_Stat_RTS As System.Windows.Forms.Label
    Friend WithEvents Label_Stat_CT As System.Windows.Forms.Label
    Friend WithEvents Label_Stat_TM As System.Windows.Forms.Label

End Class
