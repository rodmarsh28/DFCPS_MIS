Public Class frmCheckVoucher
    Dim nl As Integer = 0
    Dim particulars As String
    Dim partamount As Double
    Dim accEntryId As String
    Dim accno As String
    Dim debit As Double
    Dim credit As Double
    Public Totamount As Double = 0.0
    Public totDebit As Double = 0.0
    Public totCredit As Double = 0.0
    Dim No As String = ""


  
    Sub selectCVNo()
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
                .CommandText = "SELECT checkVoucherNo from tblCheckVoucher order by checkVoucherNo DESC"
            End With
            OleDBDR = OleDBC.ExecuteReader
            If OleDBDR.Read Then
                StrID = Mid(OleDBDR(0), 6, Len(OleDBDR(0)))
                txtCVNo.Text = "CV-" & Format(Val(StrID) + 1, "00000")
            Else
                txtCVNo.Text = "CV-00001"
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
    End Sub


    Sub save()
        If MsgBox("ARE YOU SURE YOU WANT TO CREATE AND SAVE CASH / CHECK VOUCHER ?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "WARNING") = MsgBoxResult.Yes Then
        End If
    End Sub

    Sub saveAccEntry()

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
                .CommandText = "INSERT INTO tblAccountEntry VALUES('" & txtCVNo.Text & _
                    "','" & accno & _
                    "','" & debit & _
                    "','" & credit & "')"

                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
  
    Sub printCheque()

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        For Each row As DataGridViewRow In dgv.SelectedRows
            Totamount = Totamount - dgv.CurrentRow.Cells(1).Value
            dgv.Rows.Remove(row)
            lblTotAmount.Text = CDbl(Totamount).ToString("#,##0.00")
        Next
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        For Each row As DataGridViewRow In dgv.SelectedRows
            totDebit = totDebit - dgv.CurrentRow.Cells(2).Value
            totCredit = totCredit - dgv.CurrentRow.Cells(3).Value
            dgv.Rows.Remove(row)
            lblDebit.Text = CDbl(totDebit).ToString("#,##0.00")
            lblCredit.Text = CDbl(totCredit).ToString("#,##0.00")
        Next
    End Sub

   
    Private Sub txtAmountinWords_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAmountinWords.TextChanged
        txtAmountinWords.Text = UCase(txtAmountinWords.Text)
    End Sub

    Private Sub btnGen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub



    Private Sub lblTotAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblTotAmount.TextChanged
        If lblTotAmount.Text = "0" Then
            txtAmountinWords.Text = ""
        Else
            txtAmountinWords.Text = ConvertNumberToENG(Val(Totamount))
        End If
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If lblTotAmount.Text = lblDebit.Text And lblCredit.Text Then
            save()
        Else
            MsgBox("THE AMOUNT YOU INPUT IS NOT BALANCED, PLEASE CHECK AND TRY AGAIN", MsgBoxStyle.Critical, "ERROR")
        End If
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
   
    End Sub
    Private Sub frmCheckVoucher_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        selectCVNo()
    End Sub

    Private Sub Label16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub
End Class