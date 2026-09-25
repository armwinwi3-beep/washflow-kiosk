Imports System.Drawing.Drawing2D

Public Class Form1
    Private ReadOnly Navy = Color.FromArgb(14, 32, 54)
    Private ReadOnly Blue = Color.FromArgb(35, 126, 232)
    Private ReadOnly Pale = Color.FromArgb(239, 246, 251)
    Private ReadOnly Ink = Color.FromArgb(26, 42, 58)
    Private ReadOnly Muted = Color.FromArgb(102, 119, 136)
    Private ReadOnly Green = Color.FromArgb(27, 164, 112)
    Private selectedMachine = "W03", selectedProgram = "ซักมาตรฐาน"
    Private selectedPrice = 50, queueNumber = 18
    Private summaryMachine, summaryProgram, summaryPrice, systemStatus As Label
    Private payButton As Button

    Public Sub New()
        InitializeComponent()
        DoubleBuffered = True : BackColor = Pale
        WindowState = FormWindowState.Maximized : FormBorderStyle = FormBorderStyle.None
        BuildUi()
    End Sub

    Private Sub BuildUi()
        Dim root = New TableLayoutPanel With {.Dock = DockStyle.Fill, .RowCount = 2, .BackColor = Pale}
        root.RowStyles.Add(New RowStyle(SizeType.Absolute, 88)) : root.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        root.Controls.Add(BuildHeader(), 0, 0) : root.Controls.Add(BuildContent(), 0, 1) : Controls.Add(root)
    End Sub

    Private Function BuildHeader() As Control
        Dim h = New Panel With {.Dock = DockStyle.Fill, .BackColor = Navy, .Padding = New Padding(30, 14, 30, 12)}
        h.Controls.Add(New Label With {.Text = "WASH" & vbLf & "FLOW", .ForeColor = Color.White, .Font = New Font("Segoe UI", 18, FontStyle.Bold), .AutoSize = True, .Location = New Point(30, 15)})
        h.Controls.Add(New Label With {.Text = "ซักง่าย  จ่ายไว  ไม่ต้องรอหน้าเครื่อง", .ForeColor = Color.FromArgb(203, 220, 235), .Font = New Font("Leelawadee UI", 15), .AutoSize = True, .Location = New Point(165, 30)})
        systemStatus = New Label With {.Text = "●  ระบบพร้อมให้บริการ", .ForeColor = Color.FromArgb(109, 229, 180), .Font = New Font("Leelawadee UI", 13, FontStyle.Bold), .AutoSize = True}
        Dim close = New Button With {.Text = "×", .ForeColor = Color.White, .BackColor = Navy, .FlatStyle = FlatStyle.Flat, .Font = New Font("Segoe UI", 20), .Size = New Size(48, 48), .Cursor = Cursors.Hand}
        close.FlatAppearance.BorderSize = 0 : AddHandler close.Click, Sub() Me.Close()
        h.Controls.Add(systemStatus) : h.Controls.Add(close)
        AddHandler h.Resize, Sub()
                                 close.Location = New Point(h.Width - 70, 18)
                                 systemStatus.Location = New Point(close.Left - systemStatus.Width - 28, 31)
                             End Sub
        Return h
    End Function

    Private Function BuildContent() As Control
        Dim t = New TableLayoutPanel With {.Dock = DockStyle.Fill, .ColumnCount = 2, .Padding = New Padding(38, 30, 38, 30)}
        t.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 67)) : t.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33))
        t.Controls.Add(BuildMachines(), 0, 0) : t.Controls.Add(BuildCheckout(), 1, 0) : Return t
    End Function

    Private Function BuildMachines() As Control
        Dim a = New TableLayoutPanel With {.Dock = DockStyle.Fill, .RowCount = 4, .Padding = New Padding(0, 0, 30, 0)}
        a.RowStyles.Add(New RowStyle(SizeType.Absolute, 64))
        a.RowStyles.Add(New RowStyle(SizeType.Absolute, 320))
        a.RowStyles.Add(New RowStyle(SizeType.Absolute, 150))
        a.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        a.Controls.Add(New Label With {.Text = "เลือกเครื่องซักผ้า", .Font = New Font("Leelawadee UI", 23, FontStyle.Bold), .ForeColor = Ink, .Dock = DockStyle.Fill}, 0, 0)
        Dim flow = New FlowLayoutPanel With {.Dock = DockStyle.Fill, .AutoScroll = True, .WrapContents = True, .Padding = New Padding(0, 0, 0, 8)}
        flow.Controls.Add(Machine("W01", "พร้อมใช้งาน", "10 กก.", True))
        flow.Controls.Add(Machine("W02", "พร้อมใช้งาน", "14 กก.", True))
        flow.Controls.Add(Machine("W03", "พร้อมใช้งาน", "10 กก.", True))
        flow.Controls.Add(Machine("W04", "พร้อมใช้งาน", "18 กก.", True))
        flow.Controls.Add(Machine("W05", "พร้อมใช้งาน", "14 กก.", True))
        flow.Controls.Add(Machine("W06", "พร้อมใช้งาน", "10 กก.", True))
        a.Controls.Add(flow, 0, 1) : a.Controls.Add(BuildPrograms(), 0, 2) : Return a
    End Function

    Private Function Machine(code As String, state As String, capacity As String, available As Boolean) As Panel
        Dim p As New RoundPanel With {.Size = New Size(248, 142), .BackColor = If(code = selectedMachine, Color.FromArgb(224, 240, 255), Color.White), .Margin = New Padding(0, 0, 16, 16), .Cursor = If(available, Cursors.Hand, Cursors.Default), .Tag = code}
        Dim l1 = New Label With {.Text = code, .Font = New Font("Segoe UI", 20, FontStyle.Bold), .ForeColor = Ink, .Location = New Point(18, 15), .AutoSize = True}
        Dim l2 = New Label With {.Text = capacity, .Font = New Font("Leelawadee UI", 11), .ForeColor = Muted, .Location = New Point(19, 54), .AutoSize = True}
        Dim l3 = New Label With {.Text = If(available, "●  ", "") & state, .Font = New Font("Leelawadee UI", 12, FontStyle.Bold), .ForeColor = If(available, Green, If(state.Contains("ชำระ"), Color.FromArgb(226, 142, 41), Muted)), .Location = New Point(18, 96), .AutoSize = True}
        p.Controls.AddRange({l1, l2, l3})
        If available Then
            Dim choose = Sub()
                             selectedMachine = code
                             For Each c As Control In p.Parent.Controls
                                 If TypeOf c Is Panel Then c.BackColor = Color.White
                             Next
                             p.BackColor = Color.FromArgb(224, 240, 255)
                             If payButton IsNot Nothing Then payButton.Enabled = True
                             UpdateSummary()
                         End Sub
            AddHandler p.Click, Sub() choose()
            For Each c As Control In p.Controls
                AddHandler c.Click, Sub() choose()
            Next
        Else
            p.BackColor = Color.FromArgb(247, 249, 251)
        End If
        Return p
    End Function

    Private Function BuildPrograms() As Control
        Dim box = New TableLayoutPanel With {.Dock = DockStyle.Fill, .RowCount = 2, .Padding = New Padding(0, 8, 0, 0)}
        box.RowStyles.Add(New RowStyle(SizeType.Absolute, 50)) : box.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        box.Controls.Add(New Label With {.Text = "เลือกโปรแกรม", .Font = New Font("Leelawadee UI", 18, FontStyle.Bold), .ForeColor = Ink, .Dock = DockStyle.Fill}, 0, 0)
        Dim row = New FlowLayoutPanel With {.Dock = DockStyle.Fill, .WrapContents = False}
        row.Controls.Add(Program("ซักด่วน", "25 นาที", 40)) : row.Controls.Add(Program("ซักมาตรฐาน", "35 นาที", 50)) : row.Controls.Add(Program("ซักหนัก", "45 นาที", 65))
        box.Controls.Add(row, 0, 1) : Return box
    End Function

    Private Function Program(name As String, duration As String, price As Integer) As Button
        Dim b = New Button With {.Text = name & vbLf & duration & "  •  ฿" & price, .Font = New Font("Leelawadee UI", 12, FontStyle.Bold), .Size = New Size(190, 78), .Margin = New Padding(0, 0, 12, 0), .FlatStyle = FlatStyle.Flat, .Cursor = Cursors.Hand, .Tag = New Object() {name, price}}
        StyleProgram(b, name = selectedProgram)
        AddHandler b.Click, Sub()
                                For Each c As Control In b.Parent.Controls
                                    If TypeOf c Is Button Then StyleProgram(DirectCast(c, Button), False)
                                Next
                                StyleProgram(b, True) : selectedProgram = name : selectedPrice = price : UpdateSummary()
                            End Sub
        Return b
    End Function

    Private Sub StyleProgram(b As Button, active As Boolean)
        b.BackColor = If(active, Blue, Color.White) : b.ForeColor = If(active, Color.White, Ink)
        b.FlatAppearance.BorderSize = 2 : b.FlatAppearance.BorderColor = If(active, Blue, Color.FromArgb(211, 222, 232))
    End Sub

    Private Function BuildCheckout() As Control
        Dim card As New RoundPanel With {.Dock = DockStyle.Fill, .BackColor = Color.White, .Padding = New Padding(34, 30, 34, 24)}
        Dim t = New TableLayoutPanel With {.Dock = DockStyle.Fill, .RowCount = 8}
        For Each h In {64, 46, 46, 68, 105, 74, 78} : t.RowStyles.Add(New RowStyle(SizeType.Absolute, h)) : Next
        t.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
        t.Controls.Add(New Label With {.Text = "สรุปรายการ", .Font = New Font("Leelawadee UI", 21, FontStyle.Bold), .ForeColor = Ink, .Dock = DockStyle.Fill}, 0, 0)
        summaryMachine = Summary("เครื่อง        " & selectedMachine) : summaryProgram = Summary("โปรแกรม     " & selectedProgram)
        summaryPrice = New Label With {.Text = "฿" & selectedPrice, .Font = New Font("Segoe UI", 30, FontStyle.Bold), .ForeColor = Blue, .TextAlign = ContentAlignment.MiddleRight, .Dock = DockStyle.Fill}
        t.Controls.Add(summaryMachine, 0, 1) : t.Controls.Add(summaryProgram, 0, 2) : t.Controls.Add(summaryPrice, 0, 3)
        Dim q = New Panel With {.Dock = DockStyle.Fill, .BackColor = Color.FromArgb(232, 246, 255), .Margin = New Padding(0, 8, 0, 8)}
        q.Controls.Add(New Label With {.Text = "คิวชำระเงินแบบครบวงจร", .Font = New Font("Leelawadee UI", 13, FontStyle.Bold), .ForeColor = Blue, .AutoSize = True, .Location = New Point(16, 13)})
        q.Controls.Add(New Label With {.Text = "รับหมายเลขคิว • จ่ายเงิน • ติดตามสถานะ", .Font = New Font("Leelawadee UI", 10), .ForeColor = Muted, .AutoSize = True, .Location = New Point(16, 47)})
        t.Controls.Add(q, 0, 4)
        t.Controls.Add(New Label With {.Text = "ชำระด้วย  QR PromptPay   •   เงินสด", .Font = New Font("Leelawadee UI", 12, FontStyle.Bold), .ForeColor = Ink, .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill}, 0, 5)
        payButton = New Button With {.Text = "รับคิวและชำระเงิน  →", .Dock = DockStyle.Fill, .BackColor = Blue, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Font = New Font("Leelawadee UI", 16, FontStyle.Bold), .Cursor = Cursors.Hand, .Margin = New Padding(0, 10, 0, 0)}
        payButton.FlatAppearance.BorderSize = 0 : AddHandler payButton.Click, AddressOf StartPayment : t.Controls.Add(payButton, 0, 6)
        t.Controls.Add(New Label With {.Text = "ต้องการความช่วยเหลือ?  กดปุ่มเรียกพนักงาน", .Font = New Font("Leelawadee UI", 11), .ForeColor = Muted, .TextAlign = ContentAlignment.BottomCenter, .Dock = DockStyle.Fill}, 0, 7)
        card.Controls.Add(t) : Return card
    End Function

    Private Function Summary(text As String) As Label
        Return New Label With {.Text = text, .Font = New Font("Leelawadee UI", 13), .ForeColor = Ink, .Dock = DockStyle.Fill, .TextAlign = ContentAlignment.MiddleLeft}
    End Function

    Private Sub UpdateSummary()
        If summaryMachine Is Nothing Then Return
        summaryMachine.Text = "เครื่อง        " & selectedMachine : summaryProgram.Text = "โปรแกรม     " & selectedProgram : summaryPrice.Text = "฿" & selectedPrice
    End Sub

    Private Sub StartPayment(sender As Object, e As EventArgs)
        queueNumber += 1 : payButton.Enabled = False : payButton.Text = "กำลังสร้างคิวชำระเงิน..." : systemStatus.Text = "●  กำลังดำเนินรายการ"
        Dim timer = New Timer With {.Interval = 700}
        AddHandler timer.Tick, Sub()
                                   timer.Stop() : timer.Dispose() : ShowPayment()
                                   If selectedMachine <> "" Then
                                       payButton.Enabled = True : payButton.Text = "รับคิวและชำระเงิน  →" : systemStatus.Text = "●  ระบบพร้อมให้บริการ"
                                   End If
                               End Sub
        timer.Start()
    End Sub

    Private Sub ShowPayment()
        Dim d = New Form With {.Text = "ชำระเงิน", .Size = New Size(560, 560), .StartPosition = FormStartPosition.CenterParent, .FormBorderStyle = FormBorderStyle.FixedDialog, .MaximizeBox = False, .MinimizeBox = False, .BackColor = Color.White}
        Dim title = New Label With {.Text = "คิวชำระเงิน A" & queueNumber.ToString("000"), .Font = New Font("Leelawadee UI", 24, FontStyle.Bold), .ForeColor = Ink, .Dock = DockStyle.Top, .Height = 70, .TextAlign = ContentAlignment.MiddleCenter}
        Dim info = New Label With {.Text = selectedMachine & "  •  " & selectedProgram & vbLf & "ยอดชำระ ฿" & selectedPrice, .Font = New Font("Leelawadee UI", 16), .ForeColor = Ink, .Dock = DockStyle.Top, .Height = 76, .TextAlign = ContentAlignment.MiddleCenter}
        Dim prompt = New Label With {.Text = "เลือกช่องทางชำระเงิน", .Font = New Font("Leelawadee UI", 14, FontStyle.Bold), .ForeColor = Ink, .Dock = DockStyle.Top, .Height = 58, .TextAlign = ContentAlignment.MiddleCenter}
        Dim methods = New FlowLayoutPanel With {.Dock = DockStyle.Top, .Height = 190, .FlowDirection = FlowDirection.LeftToRight, .WrapContents = False, .Padding = New Padding(48, 12, 20, 12)}
        methods.Controls.Add(PaymentButton("QR PromptPay", "สแกนผ่านแอปธนาคาร", PaymentMethod.Qr, d))
        methods.Controls.Add(PaymentButton("เงินสด", "กรอกจำนวนเงินที่รับ", PaymentMethod.Cash, d))
        Dim note = New Label With {.Text = "เมื่อระบบได้รับการยืนยันจากช่องทางชำระเงิน" & vbLf & "เครื่องซักผ้าจะเริ่มทำงานโดยอัตโนมัติ", .Font = New Font("Leelawadee UI", 12), .ForeColor = Muted, .Dock = DockStyle.Top, .Height = 72, .TextAlign = ContentAlignment.MiddleCenter}
        Dim cancel = New Button With {.Text = "ยกเลิกรายการ", .BackColor = Color.White, .ForeColor = Muted, .FlatStyle = FlatStyle.Flat, .Font = New Font("Leelawadee UI", 12, FontStyle.Bold), .Dock = DockStyle.Bottom, .Height = 54, .Cursor = Cursors.Hand}
        cancel.FlatAppearance.BorderColor = Color.FromArgb(211, 222, 232)
        AddHandler cancel.Click, Sub() d.Close()
        d.Controls.AddRange({cancel, note, methods, prompt, info, title}) : d.ShowDialog(Me)
    End Sub

    Private Function PaymentButton(title As String, detail As String, method As PaymentMethod, owner As Form) As Button
        Dim b = New Button With {.Text = title & vbLf & detail, .Font = New Font("Leelawadee UI", 11, FontStyle.Bold), .ForeColor = Ink, .BackColor = Color.White, .FlatStyle = FlatStyle.Flat, .Size = New Size(220, 132), .Margin = New Padding(0, 0, 12, 0), .Cursor = Cursors.Hand}
        b.FlatAppearance.BorderColor = Color.FromArgb(192, 210, 224)
        AddHandler b.Click, Sub() BeginRealPayment(method, owner)
        Return b
    End Function

    Private Sub BeginRealPayment(method As PaymentMethod, owner As Form)
        If method = PaymentMethod.Qr Then
            owner.Hide()
            ShowPromptPay(owner)
            owner.Close()
            Return
        End If

        If method = PaymentMethod.Cash Then
            owner.Hide()
            ShowCashPayment()
            owner.Close()
            Return
        End If

        Dim endpoint = Environment.GetEnvironmentVariable("WASHFLOW_PAYMENT_ENDPOINT")
        If String.IsNullOrWhiteSpace(endpoint) Then
            MessageBox.Show("ยังไม่ได้ตั้งค่าระบบรับชำระเงินจริง" & vbLf & "กรุณาตั้งค่า WASHFLOW_PAYMENT_ENDPOINT ก่อนเปิดให้บริการ", "ไม่สามารถรับชำระเงิน", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' จุดเชื่อมต่อ production: ส่งเลขคิว เครื่อง โปรแกรม ยอดเงิน และช่องทาง
        ' ไปยัง payment service จากนั้นเริ่มเครื่องเฉพาะเมื่อได้รับสถานะ PAID เท่านั้น
        MessageBox.Show("ส่งคิว A" & queueNumber.ToString("000") & " ไปยังระบบชำระเงินแล้ว" & vbLf & "กำลังรอการยืนยันจากผู้ให้บริการ", "รอชำระเงิน", MessageBoxButtons.OK, MessageBoxIcon.Information)
        owner.Close()
    End Sub

    Private Sub ShowCashPayment()
        Dim d = New Form With {.Text = "รับชำระเงินสด", .Size = New Size(520, 470), .StartPosition = FormStartPosition.CenterParent, .FormBorderStyle = FormBorderStyle.FixedDialog, .MaximizeBox = False, .MinimizeBox = False, .BackColor = Color.White}
        Dim title = New Label With {.Text = "ชำระเงินสด", .Font = New Font("Leelawadee UI", 23, FontStyle.Bold), .ForeColor = Ink, .Dock = DockStyle.Top, .Height = 70, .TextAlign = ContentAlignment.MiddleCenter}
        Dim total = New Label With {.Text = "ยอดที่ต้องชำระ  ฿" & selectedPrice, .Font = New Font("Leelawadee UI", 16, FontStyle.Bold), .ForeColor = Blue, .Dock = DockStyle.Top, .Height = 54, .TextAlign = ContentAlignment.MiddleCenter}
        Dim prompt = New Label With {.Text = "กรอกจำนวนเงินที่ได้รับ", .Font = New Font("Leelawadee UI", 12), .ForeColor = Muted, .Dock = DockStyle.Top, .Height = 44, .TextAlign = ContentAlignment.BottomCenter}
        Dim amount = New NumericUpDown With {.Minimum = 0D, .Maximum = 100000D, .DecimalPlaces = 0, .Increment = 10D, .Value = selectedPrice, .Font = New Font("Segoe UI", 24, FontStyle.Bold), .TextAlign = HorizontalAlignment.Center, .Width = 300, .Height = 62, .Location = New Point(103, 180), .ThousandsSeparator = True}
        Dim change = New Label With {.Text = "เงินทอน  ฿0", .Font = New Font("Leelawadee UI", 15, FontStyle.Bold), .ForeColor = Green, .Size = New Size(470, 48), .Location = New Point(18, 252), .TextAlign = ContentAlignment.MiddleCenter}
        Dim errorText = New Label With {.Text = "", .Font = New Font("Leelawadee UI", 11), .ForeColor = Color.FromArgb(210, 67, 67), .Size = New Size(470, 32), .Location = New Point(18, 298), .TextAlign = ContentAlignment.MiddleCenter}
        Dim actions = New Panel With {.Dock = DockStyle.Bottom, .Height = 82, .Padding = New Padding(18, 10, 18, 10)}
        Dim cancel = New Button With {.Text = "ย้อนกลับ", .Dock = DockStyle.Left, .Width = 170, .BackColor = Color.White, .ForeColor = Muted, .FlatStyle = FlatStyle.Flat, .Font = New Font("Leelawadee UI", 12, FontStyle.Bold), .Cursor = Cursors.Hand}
        Dim confirm = New Button With {.Text = "ยืนยันรับเงิน", .Dock = DockStyle.Right, .Width = 260, .BackColor = Green, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Font = New Font("Leelawadee UI", 14, FontStyle.Bold), .Cursor = Cursors.Hand}
        cancel.FlatAppearance.BorderColor = Color.FromArgb(211, 222, 232) : confirm.FlatAppearance.BorderSize = 0
        AddHandler amount.ValueChanged, Sub()
                                            Dim received = CInt(amount.Value)
                                            change.Text = "เงินทอน  ฿" & Math.Max(0, received - selectedPrice)
                                            confirm.Enabled = received >= selectedPrice
                                            errorText.Text = If(received < selectedPrice, "จำนวนเงินยังขาด ฿" & (selectedPrice - received), "")
                                        End Sub
        AddHandler cancel.Click, Sub() d.Close()
        AddHandler confirm.Click, Sub()
                                        If amount.Value < selectedPrice Then Return
                                        MessageBox.Show("รับเงิน ฿" & CInt(amount.Value) & " เรียบร้อย" & vbLf & "เงินทอน ฿" & (CInt(amount.Value) - selectedPrice), "ชำระเงินสำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                        MarkMachineRunning()
                                        d.Close()
                                    End Sub
        actions.Controls.AddRange({cancel, confirm})
        d.Controls.AddRange({actions, errorText, change, amount, prompt, total, title})
        d.ShowDialog(Me)
    End Sub

    Private Sub ShowPromptPay(paymentWindow As Form)
        Dim imagePath = IO.Path.Combine(Application.StartupPath, "Assets", "promptpay.png")
        If Not IO.File.Exists(imagePath) Then
            MessageBox.Show("ไม่พบไฟล์ QR PromptPay กรุณาติดต่อผู้ดูแลระบบ", "ไม่สามารถชำระเงิน", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim d = New Form With {.Text = "PromptPay", .Size = New Size(650, 820), .StartPosition = FormStartPosition.CenterParent, .FormBorderStyle = FormBorderStyle.FixedDialog, .MaximizeBox = False, .MinimizeBox = False, .BackColor = Color.White}
        Dim title = New Label With {.Text = "คิว A" & queueNumber.ToString("000") & "  •  ยอดชำระ ฿" & selectedPrice, .Font = New Font("Leelawadee UI", 20, FontStyle.Bold), .ForeColor = Ink, .Dock = DockStyle.Top, .Height = 64, .TextAlign = ContentAlignment.MiddleCenter}
        Dim picture = New PictureBox With {.Image = Image.FromFile(imagePath), .SizeMode = PictureBoxSizeMode.Zoom, .Dock = DockStyle.Fill, .BackColor = Color.White, .Margin = New Padding(18)}
        Dim actions = New Panel With {.Dock = DockStyle.Bottom, .Height = 84, .Padding = New Padding(18, 10, 18, 10), .BackColor = Color.White}
        Dim paid = New Button With {.Text = "แจ้งชำระเงินแล้ว", .Dock = DockStyle.Right, .Width = 280, .BackColor = Green, .ForeColor = Color.White, .FlatStyle = FlatStyle.Flat, .Font = New Font("Leelawadee UI", 14, FontStyle.Bold), .Cursor = Cursors.Hand}
        Dim back = New Button With {.Text = "ย้อนกลับ", .Dock = DockStyle.Left, .Width = 180, .BackColor = Color.White, .ForeColor = Muted, .FlatStyle = FlatStyle.Flat, .Font = New Font("Leelawadee UI", 12, FontStyle.Bold), .Cursor = Cursors.Hand}
        paid.FlatAppearance.BorderSize = 0 : back.FlatAppearance.BorderColor = Color.FromArgb(211, 222, 232)
        AddHandler back.Click, Sub() d.Close()
        AddHandler paid.Click, Sub()
                                   MessageBox.Show("รับแจ้งการชำระเงินคิว A" & queueNumber.ToString("000") & " แล้ว" & vbLf & "ระบบจะเริ่มเครื่อง " & selectedMachine & " หลังตรวจสอบยอดสำเร็จ", "กำลังตรวจสอบยอด", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                   MarkMachineRunning()
                                   d.Close()
                               End Sub
        actions.Controls.AddRange({back, paid})
        d.Controls.Add(picture) : d.Controls.Add(actions) : d.Controls.Add(title)
        d.ShowDialog(Me)
        picture.Image.Dispose()
    End Sub

    Private Sub MarkMachineRunning()
        Dim minutes As Integer
        Select Case selectedProgram
            Case "ซักด่วน" : minutes = 25
            Case "ซักหนัก" : minutes = 45
            Case Else : minutes = 35
        End Select

        Dim machineArea = FindMachinePanel(Me, selectedMachine)
        If machineArea IsNot Nothing Then
            machineArea.BackColor = Color.FromArgb(247, 249, 251)
            machineArea.Cursor = Cursors.Default
            If machineArea.Controls.Count >= 3 Then
                Dim state = TryCast(machineArea.Controls(2), Label)
                If state IsNot Nothing Then
                    state.Text = "กำลังซัก • เหลือ " & minutes & " นาที"
                    state.ForeColor = Blue
                End If
            End If
            machineArea.Enabled = False
        End If

        systemStatus.Text = "●  " & selectedMachine & " กำลังซัก • " & minutes & " นาที"
        systemStatus.ForeColor = Color.FromArgb(109, 229, 180)
        payButton.Enabled = False
        payButton.Text = "เลือกเครื่องว่างเพื่อทำรายการใหม่"
        selectedMachine = ""
        summaryMachine.Text = "เครื่อง        —"
    End Sub

    Private Function FindMachinePanel(parent As Control, machineCode As String) As Panel
        For Each child As Control In parent.Controls
            Dim panel = TryCast(child, Panel)
            If panel IsNot Nothing AndAlso String.Equals(TryCast(panel.Tag, String), machineCode, StringComparison.Ordinal) Then Return panel
            Dim nested = FindMachinePanel(child, machineCode)
            If nested IsNot Nothing Then Return nested
        Next
        Return Nothing
    End Function
End Class

Public Enum PaymentMethod
    Qr
    Cash
End Enum

Public Class RoundPanel
    Inherits Panel
    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e) : If Width < 20 OrElse Height < 20 Then Return
        Using p As New GraphicsPath()
            p.AddArc(0, 0, 18, 18, 180, 90) : p.AddArc(Width - 18, 0, 18, 18, 270, 90)
            p.AddArc(Width - 18, Height - 18, 18, 18, 0, 90) : p.AddArc(0, Height - 18, 18, 18, 90, 90) : p.CloseFigure()
            Region = New Region(p)
        End Using
    End Sub
End Class
