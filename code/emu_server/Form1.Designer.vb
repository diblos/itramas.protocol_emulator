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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.GroupVerbose = New System.Windows.Forms.GroupBox
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.TabControl2 = New System.Windows.Forms.TabControl
        Me.TabClient = New System.Windows.Forms.TabPage
        Me.Button1 = New System.Windows.Forms.Button
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.TabServer = New System.Windows.Forms.TabPage
        Me.gbTaxi = New System.Windows.Forms.GroupBox
        Me.btnValidatetaxi = New System.Windows.Forms.Button
        Me.btnUpdateTaxi = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtPort = New System.Windows.Forms.TextBox
        Me.txtIPAddress = New System.Windows.Forms.TextBox
        Me.GroupVerbose.SuspendLayout()
        Me.TabControl2.SuspendLayout()
        Me.TabClient.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabServer.SuspendLayout()
        Me.gbTaxi.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 251)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(516, 22)
        Me.StatusStrip1.TabIndex = 7
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'GroupVerbose
        '
        Me.GroupVerbose.Controls.Add(Me.ListBox1)
        Me.GroupVerbose.Location = New System.Drawing.Point(0, 65)
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
        'TabControl2
        '
        Me.TabControl2.Controls.Add(Me.TabClient)
        Me.TabControl2.Controls.Add(Me.TabServer)
        Me.TabControl2.Location = New System.Drawing.Point(8, 8)
        Me.TabControl2.Name = "TabControl2"
        Me.TabControl2.SelectedIndex = 0
        Me.TabControl2.Size = New System.Drawing.Size(240, 195)
        Me.TabControl2.TabIndex = 8
        '
        'TabClient
        '
        Me.TabClient.Controls.Add(Me.Button1)
        Me.TabClient.Controls.Add(Me.DataGridView1)
        Me.TabClient.Location = New System.Drawing.Point(4, 22)
        Me.TabClient.Name = "TabClient"
        Me.TabClient.Padding = New System.Windows.Forms.Padding(3)
        Me.TabClient.Size = New System.Drawing.Size(232, 169)
        Me.TabClient.TabIndex = 0
        Me.TabClient.Text = "Clients Status"
        Me.TabClient.UseVisualStyleBackColor = True
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
        Me.DataGridView1.MultiSelect = False
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(226, 37)
        Me.DataGridView1.TabIndex = 3
        '
        'TabServer
        '
        Me.TabServer.Controls.Add(Me.gbTaxi)
        Me.TabServer.Location = New System.Drawing.Point(4, 22)
        Me.TabServer.Name = "TabServer"
        Me.TabServer.Padding = New System.Windows.Forms.Padding(3)
        Me.TabServer.Size = New System.Drawing.Size(232, 169)
        Me.TabServer.TabIndex = 1
        Me.TabServer.Text = "Server Control"
        Me.TabServer.UseVisualStyleBackColor = True
        '
        'gbTaxi
        '
        Me.gbTaxi.Controls.Add(Me.btnValidatetaxi)
        Me.gbTaxi.Controls.Add(Me.btnUpdateTaxi)
        Me.gbTaxi.Controls.Add(Me.Label7)
        Me.gbTaxi.Controls.Add(Me.Label8)
        Me.gbTaxi.Controls.Add(Me.txtPort)
        Me.gbTaxi.Controls.Add(Me.txtIPAddress)
        Me.gbTaxi.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbTaxi.Location = New System.Drawing.Point(3, 3)
        Me.gbTaxi.Name = "gbTaxi"
        Me.gbTaxi.Size = New System.Drawing.Size(226, 163)
        Me.gbTaxi.TabIndex = 5
        Me.gbTaxi.TabStop = False
        Me.gbTaxi.Text = "Server Status"
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
        Me.Label7.Size = New System.Drawing.Size(26, 13)
        Me.Label7.TabIndex = 4
        Me.Label7.Text = "Port"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(7, 25)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(17, 13)
        Me.Label8.TabIndex = 3
        Me.Label8.Text = "IP"
        '
        'txtPort
        '
        Me.txtPort.Location = New System.Drawing.Point(88, 45)
        Me.txtPort.MaxLength = 10
        Me.txtPort.Name = "txtPort"
        Me.txtPort.ReadOnly = True
        Me.txtPort.Size = New System.Drawing.Size(100, 20)
        Me.txtPort.TabIndex = 1
        '
        'txtIPAddress
        '
        Me.txtIPAddress.Location = New System.Drawing.Point(88, 19)
        Me.txtIPAddress.MaxLength = 50
        Me.txtIPAddress.Name = "txtIPAddress"
        Me.txtIPAddress.ReadOnly = True
        Me.txtIPAddress.Size = New System.Drawing.Size(100, 20)
        Me.txtIPAddress.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(516, 273)
        Me.Controls.Add(Me.TabControl2)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupVerbose)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.GroupVerbose.ResumeLayout(False)
        Me.TabControl2.ResumeLayout(False)
        Me.TabClient.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabServer.ResumeLayout(False)
        Me.gbTaxi.ResumeLayout(False)
        Me.gbTaxi.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents GroupVerbose As System.Windows.Forms.GroupBox
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents TabControl2 As System.Windows.Forms.TabControl
    Friend WithEvents TabClient As System.Windows.Forms.TabPage
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents TabServer As System.Windows.Forms.TabPage
    Friend WithEvents gbTaxi As System.Windows.Forms.GroupBox
    Friend WithEvents btnValidatetaxi As System.Windows.Forms.Button
    Friend WithEvents btnUpdateTaxi As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtPort As System.Windows.Forms.TextBox
    Friend WithEvents txtIPAddress As System.Windows.Forms.TextBox

End Class
