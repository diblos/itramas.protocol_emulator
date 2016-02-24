<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.Group1 = New System.Windows.Forms.GroupBox
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.btnTest = New System.Windows.Forms.Button
        Me.btnCOM = New System.Windows.Forms.Button
        Me.GroupSerial = New System.Windows.Forms.GroupBox
        Me.ComboBaud = New System.Windows.Forms.ComboBox
        Me.ComboCOM = New System.Windows.Forms.ComboBox
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.btnTest2 = New System.Windows.Forms.Button
        Me.btnCOM2 = New System.Windows.Forms.Button
        Me.GroupSerial2 = New System.Windows.Forms.GroupBox
        Me.ComboBaud2 = New System.Windows.Forms.ComboBox
        Me.ComboCOM2 = New System.Windows.Forms.ComboBox
        Me.GroupVerbose = New System.Windows.Forms.GroupBox
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.Group1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupSerial.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupSerial2.SuspendLayout()
        Me.GroupVerbose.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 401)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(934, 22)
        Me.StatusStrip1.TabIndex = 8
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'Group1
        '
        Me.Group1.Controls.Add(Me.TabControl1)
        Me.Group1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Group1.Location = New System.Drawing.Point(0, 0)
        Me.Group1.Name = "Group1"
        Me.Group1.Size = New System.Drawing.Size(934, 207)
        Me.Group1.TabIndex = 7
        Me.Group1.TabStop = False
        Me.Group1.Text = "Device Settings"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TabControl1.Location = New System.Drawing.Point(3, 16)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(928, 177)
        Me.TabControl1.TabIndex = 7
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.GroupSerial)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(920, 151)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Taxi Meter"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnTest)
        Me.GroupBox2.Controls.Add(Me.btnCOM)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox2.Location = New System.Drawing.Point(3, 79)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(914, 69)
        Me.GroupBox2.TabIndex = 11
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Taxi Meter Simulation"
        '
        'btnTest
        '
        Me.btnTest.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnTest.Enabled = False
        Me.btnTest.Location = New System.Drawing.Point(3, 43)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(908, 23)
        Me.btnTest.TabIndex = 12
        Me.btnTest.Text = "Test"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'btnCOM
        '
        Me.btnCOM.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnCOM.Location = New System.Drawing.Point(3, 16)
        Me.btnCOM.Name = "btnCOM"
        Me.btnCOM.Size = New System.Drawing.Size(908, 23)
        Me.btnCOM.TabIndex = 11
        Me.btnCOM.Text = "Start"
        Me.btnCOM.UseVisualStyleBackColor = True
        '
        'GroupSerial
        '
        Me.GroupSerial.Controls.Add(Me.ComboBaud)
        Me.GroupSerial.Controls.Add(Me.ComboCOM)
        Me.GroupSerial.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupSerial.Location = New System.Drawing.Point(3, 3)
        Me.GroupSerial.Name = "GroupSerial"
        Me.GroupSerial.Size = New System.Drawing.Size(914, 70)
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
        Me.ComboBaud.Size = New System.Drawing.Size(908, 21)
        Me.ComboBaud.TabIndex = 5
        '
        'ComboCOM
        '
        Me.ComboCOM.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboCOM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboCOM.FormattingEnabled = True
        Me.ComboCOM.Location = New System.Drawing.Point(3, 16)
        Me.ComboCOM.Name = "ComboCOM"
        Me.ComboCOM.Size = New System.Drawing.Size(908, 21)
        Me.ComboCOM.TabIndex = 4
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBox3)
        Me.TabPage2.Controls.Add(Me.GroupSerial2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(920, 151)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Cashless Terminal "
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnTest2)
        Me.GroupBox3.Controls.Add(Me.btnCOM2)
        Me.GroupBox3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupBox3.Location = New System.Drawing.Point(3, 79)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(914, 69)
        Me.GroupBox3.TabIndex = 12
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Cashless Terminal Simulation"
        '
        'btnTest2
        '
        Me.btnTest2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.btnTest2.Enabled = False
        Me.btnTest2.Location = New System.Drawing.Point(3, 43)
        Me.btnTest2.Name = "btnTest2"
        Me.btnTest2.Size = New System.Drawing.Size(908, 23)
        Me.btnTest2.TabIndex = 12
        Me.btnTest2.Text = "Test"
        Me.btnTest2.UseVisualStyleBackColor = True
        '
        'btnCOM2
        '
        Me.btnCOM2.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnCOM2.Location = New System.Drawing.Point(3, 16)
        Me.btnCOM2.Name = "btnCOM2"
        Me.btnCOM2.Size = New System.Drawing.Size(908, 23)
        Me.btnCOM2.TabIndex = 11
        Me.btnCOM2.Text = "Start"
        Me.btnCOM2.UseVisualStyleBackColor = True
        '
        'GroupSerial2
        '
        Me.GroupSerial2.Controls.Add(Me.ComboBaud2)
        Me.GroupSerial2.Controls.Add(Me.ComboCOM2)
        Me.GroupSerial2.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupSerial2.Location = New System.Drawing.Point(3, 3)
        Me.GroupSerial2.Name = "GroupSerial2"
        Me.GroupSerial2.Size = New System.Drawing.Size(914, 70)
        Me.GroupSerial2.TabIndex = 3
        Me.GroupSerial2.TabStop = False
        Me.GroupSerial2.Text = "Serial Connection"
        '
        'ComboBaud2
        '
        Me.ComboBaud2.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboBaud2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBaud2.FormattingEnabled = True
        Me.ComboBaud2.Location = New System.Drawing.Point(3, 37)
        Me.ComboBaud2.Name = "ComboBaud2"
        Me.ComboBaud2.Size = New System.Drawing.Size(908, 21)
        Me.ComboBaud2.TabIndex = 5
        '
        'ComboCOM2
        '
        Me.ComboCOM2.Dock = System.Windows.Forms.DockStyle.Top
        Me.ComboCOM2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboCOM2.FormattingEnabled = True
        Me.ComboCOM2.Location = New System.Drawing.Point(3, 16)
        Me.ComboCOM2.Name = "ComboCOM2"
        Me.ComboCOM2.Size = New System.Drawing.Size(908, 21)
        Me.ComboCOM2.TabIndex = 4
        '
        'GroupVerbose
        '
        Me.GroupVerbose.Controls.Add(Me.ListBox1)
        Me.GroupVerbose.Location = New System.Drawing.Point(3, 255)
        Me.GroupVerbose.Name = "GroupVerbose"
        Me.GroupVerbose.Size = New System.Drawing.Size(468, 143)
        Me.GroupVerbose.TabIndex = 6
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
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(934, 423)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.Group1)
        Me.Controls.Add(Me.GroupVerbose)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.Group1.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupSerial.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupSerial2.ResumeLayout(False)
        Me.GroupVerbose.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents Group1 As System.Windows.Forms.GroupBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents GroupSerial As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBaud As System.Windows.Forms.ComboBox
    Friend WithEvents ComboCOM As System.Windows.Forms.ComboBox
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents GroupVerbose As System.Windows.Forms.GroupBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents GroupSerial2 As System.Windows.Forms.GroupBox
    Friend WithEvents ComboBaud2 As System.Windows.Forms.ComboBox
    Friend WithEvents ComboCOM2 As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnCOM As System.Windows.Forms.Button
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnTest2 As System.Windows.Forms.Button
    Friend WithEvents btnCOM2 As System.Windows.Forms.Button

End Class
