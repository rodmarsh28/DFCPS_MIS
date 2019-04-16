Imports System.Data.SqlClient

Public Class frmPurchaseInvoice
    Public totAmount As Double
    Public formMode As String
    Public CardID As String
    Public RefNo As String
    Public totalBalance As Double
    Dim checkNo As String
    Dim amountPaid As String
    Dim PAYNO As String
    Public MEMO As String


    Sub generateNo()
        Dim StrID As String
        Try
            If conn.State = ConnectionState.Open Then
                OleDBC.Dispose()
                conn.Close()
            End If
            If conn.State <> ConnectionState.Open Then
                ConnectDatabase()
            End If
            With OleDBC
                .Connection = conn
                If lblFormMode.Text = "PURCHASE REQUISITION" Then
                    .CommandText = "SELECT purchaseRequisitionNo from tblPurchaseRequisition order by purchaseRequisitionNo DESC"
                ElseIf lblFormMode.Text = "PURCHASE ORDER" Then
                    .CommandText = "SELECT purchaseOrderNo from tblPurchaseOrder order by purchaseOrderNo DESC"
                ElseIf lblFormMode.Text = "PURCHASE INVOICE" Then
                    .CommandText = "SELECT purchaseInvoiceNo from tblPurchaseInvoice order by purchaseInvoiceNo DESC"
                End If
            End With
            OleDBDR = OleDBC.ExecuteReader
            If OleDBDR.Read Then
                StrID = OleDBDR.Item(0)
                txtSalesNo.Text = Format(Val(StrID) + 1, "00000")

            Else
                txtSalesNo.Text = "00001"
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
    End Sub
    'Sub clear()
    '    txtColor.Text = ""
    '    txtUnitPrice.Text = ""
    '    txtQty.Text = ""
    'End Sub
    Sub disposeform()
        txtSalesNo.Text = ""
        RefNo = ""
        CardID = ""
        txtName.Text = ""
        totAmount = 0.0
        lblTotal.Text = "Total" & vbCrLf & "Php " & Format(totAmount, "N")
        dgv.Rows.Clear()
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub


    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub


    'Sub printAcknowledgementReceipt()
    '    If MsgBox("You Want to Print Acknowledgement Receipt ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "WARNING") = MsgBoxResult.Yes Then
    '        Try
    '            If conn.State = ConnectionState.Open Then
    '                OleDBC.Dispose()
    '                conn.Close()
    '            End If
    '            If conn.State <> ConnectionState.Open Then
    '                ConnectDatabase()
    '            End If
    '            With OleDBC
    '                .Connection = conn
    '                .CommandText = "SELECT dbo.tblSales.SALESNO, " & _
    '                "dbo.tblPaid.PAYDATE, " & _
    '                "dbo.tblSales.CUSTOMERNAME, " & _
    '                "dbo.tblSales.TOTALAMOUNT, " & _
    '                "dbo.tblSales.RECEIVEDBY, " & _
    '                "dbo.tblSalesTR.WIDTH, " & _
    '                "dbo.tblSalesTR.LENGTH, " & _
    '                "dbo.tblSalesTR.COLOR, " & _
    '                "dbo.tblSalesTR.SEWNTYPE, " & _
    '                "dbo.tblSalesTR.PRINTED, " & _
    '                "dbo.tblSalesTR.QTY, " & _
    '                "dbo.tblSalesTR.UPRICE, " & _
    '                "dbo.tblSalesTR.AMOUNT, " & _
    '                "dbo.tblPaid.PAYNO, " & _
    '                "dbo.tblPaid.PAYTYPE, " & _
    '                "dbo.tblPaid.CHEQUENO, " & _
    '                "dbo.tblPaid.AMOUNTPAID " & _
    '                "FROM dbo.tblSales " & _
    '                "INNER JOIN dbo.tblSalesTR ON dbo.tblSales.SALESNO = dbo.tblSalesTR.SALESNO " & _
    '                "INNER JOIN dbo.tblPaid ON dbo.tblSales.SALESNO = dbo.tblPaid.SALESNO " & _
    '                "WHERE tblPaid.payno = '" & PAYNO & "'"

    '            End With
    '            OleDBDR = OleDBC.ExecuteReader
    '            Dim dt As New DataTable
    '            With dt
    '                .Columns.Add("SALESNO")
    '                .Columns.Add("PAYNO")
    '                .Columns.Add("DATE")
    '                .Columns.Add("CUSTOMER")
    '                .Columns.Add("AMOUNTINWORDS")
    '                .Columns.Add("TOTAMOUNT")
    '                .Columns.Add("PAYTYPE")
    '                .Columns.Add("RECEIVEDBY")
    '                .Columns.Add("DESCRIPTION")
    '                .Columns.Add("UNIT")
    '                .Columns.Add("QTY")
    '                .Columns.Add("UNITPRICE")
    '                .Columns.Add("AMOUNT")
    '            End With
    '            If OleDBDR.HasRows Then
    '                While OleDBDR.Read
    '                    Dim PAYTYPE As String
    '                    If OleDBDR.Item(14) = "CASH (PARTIAL)" Or OleDBDR.Item(14) = "CHEQUE (PARTIAL)" Then
    '                        PAYTYPE = "partial "
    '                    Else
    '                        PAYTYPE = ""
    '                    End If
    '                    dt.Rows.Add(OleDBDR.Item(0),
    '                    OleDBDR.Item(13),
    '                     Format(OleDBDR.Item(1), "MMM dd, yyyy"),
    '                     OleDBDR.Item(2),
    '                     UCase(ConvertNumberToENG(Val(OleDBDR.Item(16)))),
    '                     Format(OleDBDR.Item(16), "N"),
    '                    PAYTYPE,
    '                     OleDBDR.Item(4),
    '                    OleDBDR.Item(5) & "x" & Format(OleDBDR.Item(6), "N0") & " - " & OleDBDR.Item(7) & " (" & OleDBDR.Item(8) & " " & OleDBDR.Item(9) & ")",
    '                      "PCS",
    '                     Format(OleDBDR.Item(10), "N"),
    '                     Format(OleDBDR.Item(11), "N"),
    '                     Format(OleDBDR.Item(12), "N"))
    '                End While
    '            End If
    '            Dim rptDoc As CrystalDecisions.CrystalReports.Engine.ReportDocument
    '            rptDoc = New ACKprint
    '            rptDoc.SetDataSource(dt)
    '            frmReportViewerNoDateRange.CrystalReportViewer1.ReportSource = rptDoc
    '            frmReportViewerNoDateRange.ShowDialog()
    '        Catch ex As Exception
    '            MsgBox(ex.Message)
    '        End Try
    '    End If
    'End Sub
    Sub RECORDPURCHASES()
        Try
            Dim row1 As Integer
            Dim col As Integer
            col = 0
            row1 = dgv.RowCount
            While col < row1
                Dim cmd As New SqlCommand("insert_Purchases", conn)
                conn.Close()
                ConnectDatabase()
                With cmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.AddWithValue("@processCount", SqlDbType.Int).Value = col
                    .Parameters.AddWithValue("@purchaseMode", SqlDbType.VarChar).Value = lblFormMode.Text
                    .Parameters.AddWithValue("@purchaseno", SqlDbType.VarChar).Value = txtSalesNo.Text
                    .Parameters.AddWithValue("@dateofTR", SqlDbType.Date).Value = Format(Now, "MM/dd/yyyy hh:mm")
                    .Parameters.AddWithValue("@refNo", SqlDbType.VarChar).Value = txtRefNo.Text
                    .Parameters.AddWithValue("@cardID", SqlDbType.VarChar).Value = CardID
                    .Parameters.AddWithValue("@totalAmount", SqlDbType.Decimal).Value = CDbl(totAmount)
                    .Parameters.AddWithValue("@userid", SqlDbType.VarChar).Value = MainForm.LBLID.Text
                    .Parameters.AddWithValue("@itemNo", SqlDbType.VarChar).Value = dgv.Item(0, col).Value
                    .Parameters.AddWithValue("@qty", SqlDbType.Int).Value = CInt(dgv.Item(4, col).Value)
                    .Parameters.AddWithValue("@uprice", SqlDbType.Decimal).Value = CDbl(dgv.Item(3, col).Value)
                    .Parameters.AddWithValue("@amount", SqlDbType.Decimal).Value = CDbl(dgv.Item(5, col).Value)
                End With
                cmd.ExecuteNonQuery()
                col = col + 1
            End While
            If chkPaynow.Checked = True Then
                Dim req As New Puchase_Requisition_class
                req.transNo = txtSalesNo.Text
                req.src = Form.ActiveForm.Text
                req.totAmount = totAmount
                req.insert_RFP()
            End If
            MsgBox(lblFormMode.Text & " POSTED !", MsgBoxStyle.Information, "SUCCESS")
            disposeform()
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If txtName.Text <> "" Then
            If lblFormMode.Text = "PURCHASE REQUISITION" Then
                If MsgBox("Are you sure you want to post?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "WARNING") = MsgBoxResult.Yes Then
                    RECORDPURCHASES()
                End If
            ElseIf lblFormMode.Text = "PURCHASE ORDER" Then
                If MsgBox("Are you sure you want to post?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "WARNING") = MsgBoxResult.Yes Then
                    RECORDPURCHASES()
                End If
            ElseIf lblFormMode.Text = "PURCHASE INVOICE" Then
                MEMO = InputBox("to proceed", "Input a transaction memo here ", "sample")
                RECORDPURCHASES()
            End If
        Else
            MsgBox("Please Enter Supplier Name ", MsgBoxStyle.Critical, "Error")
        End If
    End Sub

    Private Sub frmSales_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        disposeform()
    End Sub

    Private Sub frmSales_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtQty.Focus()
        ElseIf e.KeyCode = Keys.D AndAlso e.Modifiers = Keys.Control Then
            If MsgBox("ARE YOU SURE TO DELETE THIS ITEM?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "WARNING") = MsgBoxResult.Yes Then
                For Each row As DataGridViewRow In dgv.SelectedRows
                    totAmount = totAmount - dgv.CurrentRow.Cells(8).Value
                    dgv.Rows.Remove(row)
                Next
                lblTotal.Text = "Php " & Format(totAmount, "N")
            End If

            'ElseIf e.KeyCode = Keys.F5 Then
            '    If MsgBox("ARE YOU SURE ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "WARNING") = MsgBoxResult.Yes Then
            '        'If lblFormMode.Text = "PURCHASE REQUISITION" Then
            '        '    lblFormMode.Text = "PURCHASE ORDER"
            '        If lblFormMode.Text = "PURCHASE ORDER" Then
            '            lblFormMode.Text = "PURCHASE INVOICE"
            '        ElseIf lblFormMode.Text = "PURCHASE INVOICE" Then
            '            lblFormMode.Text = "PURCHASE REQUISITION"
            '        End If
            '        generateNo()
            '    End If
        End If
    End Sub

    Private Sub frmSales_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        generateNo()
        totAmount = 0
        lblTotal.Text = "Php " & Format(totAmount, "N")
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If txtQty.Text = "" Then
            txtQty.Text = "1"
        End If
        InventoryList.mode = "Purchases"
        InventoryList.ShowDialog()
        If InventoryList.clickedItem = True Then
            Dim r As Integer = dgv.Rows.Count
            dgv.Rows.Add()
            dgv.Item(0, r).Value = InventoryList.dgv.CurrentRow.Cells(0).Value
            dgv.Item(1, r).Value = InventoryList.dgv.CurrentRow.Cells(1).Value
            dgv.Item(2, r).Value = InventoryList.dgv.CurrentRow.Cells(2).Value
            If lblFormMode.Text = "PURCHASE REQUISITION" Then
                dgv.Item(3, r).Value = "0.00"
                dgv.Item(5, r).Value = "0.00"
            Else
                dgv.Item(3, r).Value = InventoryList.dgv.CurrentRow.Cells(3).Value
                dgv.Item(5, r).Value = CDbl(InventoryList.dgv.CurrentRow.Cells(3).Value) * CDbl(txtQty.Text)
            End If
            dgv.Item(4, r).Value = txtQty.Text
            lblTotal.Text = "Php " & Format(totAmount, "N")
            InventoryList.clickedItem = False
        End If

    End Sub


    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnSearch.PerformClick()
        ElseIf e.KeyCode = Keys.B AndAlso e.Modifiers = Keys.Control Then

        End If
    End Sub

    Private Sub btnSearchCustomer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchCustomer.Click
        frmCardListForSelection.formMode = "Purchase Invoice"
        frmCardListForSelection.ShowDialog()
    End Sub

    Private Sub lblFormMode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblFormMode.Click

    End Sub

    Private Sub lblFormMode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblFormMode.TextChanged
        If lblFormMode.Text = "PURCHASE REQUISITION" Then
            Me.BackColor = System.Drawing.SystemColors.Info
        ElseIf lblFormMode.Text = "PURCHASE ORDER" Then
            Me.BackColor = System.Drawing.SystemColors.GradientInactiveCaption
        ElseIf lblFormMode.Text = "PURCHASE INVOICE" Then
            Me.BackColor = Color.White
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            frmGetRequisitionItemList.Close()
            frmGetRequisitionItemList.Show()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub dgv_CellMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgv.CellMouseClick
        Try
            Dim VALUE As Double = InputBox("Input Value")
            dgv.CurrentCell.Value = VALUE
        Catch ex As Exception
        End Try
    End Sub

    Private Sub dgv_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv.CellValueChanged
        Try
            dgv.CurrentRow.Cells(5).Value = CDbl(dgv.CurrentRow.Cells(3).Value) * CDbl(dgv.CurrentRow.Cells(4).Value)
        Catch ex As Exception
        End Try
    End Sub
End Class