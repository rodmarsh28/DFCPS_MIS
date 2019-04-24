Imports System.Data.SqlClient

Public Class frmPurchasedReceiving
    Public cardID As String
    Public invoiceNo As String
    Private WithEvents p_event As New public_event_class
    Public totAmount As Double
    Dim row1 As Integer
    Dim col As Integer
    Dim clearingAccount As String
    Dim allitemsreceived As Integer

    Private Sub VariableChanged(ByVal NewValue As Integer) Handles p_event.VariableChanged
    End Sub

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
                .CommandText = "SELECT purchaseReceivingNo from tblPurchaseReceived order by purchaseReceivingNo DESC"
            End With
            OleDBDR = OleDBC.ExecuteReader
            If OleDBDR.Read Then
                StrID = OleDBDR.Item(0).Substring(OleDBDR.Item(0).Length - 5)
                transNo.Text = "REC-" & Format(Val(StrID) + 1, "00000")
            Else
                transNo.Text = "REC-00001"
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
        End Try
    End Sub

    Private Sub frmPurchasedReceiving_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
            Me.Dispose()
    End Sub

    Private Sub frmPurchasedReceiving_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtQty.Focus()
        End If

    End Sub

    Private Sub frmPurchasedReceiving_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        generateNo()
        totAmount = 0.0
        lblTotal.Text = "Php " & Format(CDbl(totAmount), "N")

    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If txtQty.Text = "" Then
            txtQty.Text = "1"
        End If
        InventoryList.mode = "Receiving"
        InventoryList.ShowDialog()
        If InventoryList.clickedItem = True Then
            Dim r As Integer = dgv.Rows.Count
            dgv.Rows.Add()
            dgv.Item(0, r).Value = InventoryList.dgv.CurrentRow.Cells(0).Value
            dgv.Item(1, r).Value = InventoryList.dgv.CurrentRow.Cells(1).Value
            dgv.Item(2, r).Value = InventoryList.dgv.CurrentRow.Cells(2).Value
            dgv.Item(3, r).Value = InventoryList.dgv.CurrentRow.Cells(3).Value
            dgv.Item(4, r).Value = txtQty.Text
            InventoryList.clickedItem = False
            dgv.Item(5, r).Value = Format(CDbl(InventoryList.dgv.CurrentRow.Cells(3).Value) * CDbl(txtQty.Text), "N")
            dgv.Item(6, r).Value = InventoryList.dgv.CurrentRow.Cells(7).Value
            lblTotal.Text = "Php " & Format(totAmount, "N")
        End If
    End Sub
    Sub RECEIVEDITEMS()
        Try
            Dim command As Integer
            col = 0
            row1 = dgv.RowCount
            While col < row1
                If BTNSAVE.Text = "&POST" Then
                    command = 0
                Else
                    command = 1
                End If
                Dim cmd As New SqlCommand("insert_Purchase_Receiving", conn)
                conn.Close()
                ConnectDatabase()
                With cmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.AddWithValue("@COMMAND", SqlDbType.VarChar).Value = command
                    .Parameters.AddWithValue("@AIR", SqlDbType.Int).Value = allitemsreceived
                    .Parameters.AddWithValue("@TRANSNO", SqlDbType.VarChar).Value = transNo.Text
                    .Parameters.AddWithValue("@REFNO", SqlDbType.VarChar).Value = txtRefNo.Text
                    .Parameters.AddWithValue("@INVOICENO", SqlDbType.VarChar).Value = txtInvoice.Text
                    .Parameters.AddWithValue("@CARDID", SqlDbType.Date).Value = cardID
                    .Parameters.AddWithValue("@ITEMNO", SqlDbType.VarChar).Value = dgv.Item(0, col).Value
                    .Parameters.AddWithValue("@QTY", SqlDbType.VarChar).Value = dgv.Item(4, col).Value
                    .Parameters.AddWithValue("@UPRICE", SqlDbType.Decimal).Value = dgv.Item(3, col).Value
                    .Parameters.AddWithValue("@AMOUNT", SqlDbType.VarChar).Value = dgv.Item(5, col).Value
                    .Parameters.AddWithValue("@USERID", SqlDbType.VarChar).Value = MainForm.LBLID.Text
                    .Parameters.AddWithValue("@STATUS", SqlDbType.VarChar).Value = ""
                End With
                cmd.ExecuteNonQuery()
                account_entry()
                ADD_INVENTORY()
                col = col + 1
            End While
            'If lblClearingAcc.Text <> "" Then
            '    Dim ac As New accEntry_class
            '    ac.refno = txtRefNo.Text
            '    ac.account = txtClearingAcc.Text
            '    ac.memo = txtMemo.Text
            '    ac.debit = 0
            '    ac.credit = totAmount
            '    ac.insert_Acc_entry_class()
            'End If
            add_payables()
            Dim req As New Puchase_Requisition_class
            MsgBox(lblFormMode.Text & " POSTED !", MsgBoxStyle.Information, "SUCCESS")
            Me.Close()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Sub account_entry()
        Dim ac As New accEntry_class
        ac.refno = txtRefNo.Text
        ac.account = dgv.Item(6, col).Value
        ac.memo = txtMemo.Text
        ac.debit = dgv.Item(5, col).Value
        ac.credit = 0
        ac.insert_Acc_entry_class()
    End Sub
    Sub add_payables()
        Dim payable As New payable_class
        payable.command = 0
        payable.transNo = transNo.Text
        payable.src = Form.ActiveForm.Text
        payable.payment = cmbPayment.Text
        payable.dueDate = Format(DTPDUEDATE.Value, "yyyy/MM/dd")
        payable.totAmount = totAmount
        payable.status = "Item Received"
        payable.insert_update_payables()
    End Sub
    Sub ADD_INVENTORY()
        Dim inventoryClass As New inventory_class
        inventoryClass.refNo = txtRefNo.Text
        inventoryClass.itemNo = dgv.Item(0, col).Value
        inventoryClass.unitCost = dgv.Item(3, col).Value
        inventoryClass.qty = dgv.Item(4, col).Value
        inventoryClass.insert_invItem_transaction()
    End Sub
    Sub generateAccountEntry()
        Dim ac As New Account_Class
        ac.searchValue = dgv.CurrentRow.Cells(0).Value
        ac.get_itemAccountInfo()
        For Each row As DataGridViewRow In dgvAccEntry.Rows
            Dim hasrows As Integer = 0
            If row.Cells(0).Value = ac.assetAcc Then
                row.Cells(4).Value = CDbl(dgv.CurrentRow.Cells(5).Value)
                hasrows = +1
            End If
            If hasrows < 1 Then
                Dim r As Integer = dgvAccEntry.Rows.Count
                dgvAccEntry.Rows.Add()
                dgv.Item(0, r).Value = ac.assetAcc
                dgv.Item(2, r).Value = CDbl(dgv.CurrentRow.Cells(5).Value)
                dgv.Item(3, r).Value = "0.00"
            End If
        Next
    End Sub
    Private Sub lblTotal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblTotal.TextChanged
    End Sub

    Private Sub txtQty_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnSearch.PerformClick()
        End If

    End Sub
    Sub sumofAmount()
        totAmount = 0
        For i As Integer = 0 To dgv.Rows.Count() - 1 Step +1
            totAmount = totAmount + dgv.Rows(i).Cells(5).Value
        Next
        lblTotal.Text = "Php " & Format(totAmount, "N")
    End Sub

    Private Sub dgv_CellMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles dgv.CellMouseDoubleClick
        Try
            Dim sC As Integer = dgv.CurrentCell.ColumnIndex
            If sC <= 0 Then
                frmAccountList.ShowDialog()
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub dgv_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgv.CellValueChanged
        Try
            dgv.CurrentRow.Cells(5).Value = Format(CDbl(dgv.CurrentRow.Cells(3).Value) * CDbl(dgv.CurrentRow.Cells(4).Value), "N")
            sumofAmount()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If MsgBox("Are you sure ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            dgv.Rows.Clear()
            lblTotal.Text = totAmount
        End If

    End Sub

    Private Sub BTNSAVE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTNSAVE.Click
        If txtName.Text <> "" Or invoiceNo <> "" Then
            If MsgBox("Are you sure ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                If MsgBox("Did you received all your ordered items ?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    allitemsreceived = 1
                Else
                    allitemsreceived = 0
                End If
                RECEIVEDITEMS()
            End If
        Else
            MsgBox("Please fillup all information ", MsgBoxStyle.Critical, "Error")
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            frmGetRequisitionItemList.Close()
            frmGetRequisitionItemList.viewList_purchaseOrder()
            frmGetRequisitionItemList.Show()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
        list_for_selected_accounts.get_clearing_accounts()
        list_for_selected_accounts.ShowDialog()
    End Sub
End Class