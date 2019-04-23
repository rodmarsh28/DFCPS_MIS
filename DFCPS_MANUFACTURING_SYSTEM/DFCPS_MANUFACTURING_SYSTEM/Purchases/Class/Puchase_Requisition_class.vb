﻿Imports System.Data.SqlClient
Public Class Puchase_Requisition_class
    Public command As Integer
    Public requisitionNo As String
    Public DepartmentNo As String
    Public remarks As String
    Public userid As String
    Public itemCode As String
    Public Desc As String
    Public unit As String
    Public onhand As Integer
    Public qty As Integer
    Public searchValue As String
    Public comand As String
    Dim dt As New DataTable


    Public transNo As String
    Public src As String
    Public totAmount As Double
    Public paymentType As String
    Public dueDate As DateTime
    Public paymentDesc As String




    Public Sub insert_update_itemrequisition()
        Try
            Dim cmd As New SqlCommand("insert_update_itemRequisition", conn)
            checkConn()
            With cmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@requisitionNo", SqlDbType.Int).Value = requisitionNo
                .Parameters.AddWithValue("@DepartmentNo", SqlDbType.DateTime2).Value = DepartmentNo
                .Parameters.AddWithValue("@remarks", SqlDbType.VarChar).Value = remarks
                .Parameters.AddWithValue("@userid", SqlDbType.VarChar).Value = MainForm.LBLID.Text
                .Parameters.AddWithValue("@itemCode", SqlDbType.VarChar).Value = itemCode
                .Parameters.AddWithValue("@Description", SqlDbType.VarChar).Value = Desc
                .Parameters.AddWithValue("@unit", SqlDbType.VarChar).Value = unit
                .Parameters.AddWithValue("@onhand", SqlDbType.Int).Value = onhand
                .Parameters.AddWithValue("@qty", SqlDbType.Int).Value = qty
            End With
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Sub insert_RFP()
        Try
            Dim cmd As New SqlCommand("INSERT_RFP", conn)
            checkConn()
            With cmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@TRANSNO", SqlDbType.VarChar).Value = transNo
                .Parameters.AddWithValue("@SRC", SqlDbType.VarChar).Value = Form.ActiveForm.Text
                .Parameters.AddWithValue("@AMOUNT", SqlDbType.Decimal).Value = totAmount
                .Parameters.AddWithValue("@userid", SqlDbType.VarChar).Value = MainForm.LBLID.Text
            End With
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Public Sub insert_payables()
        Try
            Dim cmd As New SqlCommand("insert_update_payables", conn)
            checkConn()
            With cmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@command", SqlDbType.Int).Value = command
                .Parameters.AddWithValue("@refno", SqlDbType.VarChar).Value = transNo
                .Parameters.AddWithValue("@SRC", SqlDbType.VarChar).Value = Form.ActiveForm.Text
                .Parameters.AddWithValue("@paymenttype", SqlDbType.Decimal).Value = paymentType
                .Parameters.AddWithValue("@duedate", SqlDbType.VarChar).Value = dueDate
                .Parameters.AddWithValue("@paymentdesc", SqlDbType.VarChar).Value = paymentDesc
                .Parameters.AddWithValue("@amount", SqlDbType.Decimal).Value = totAmount
                .Parameters.AddWithValue("@status", SqlDbType.VarChar).Value = "waiting for receiving"
            End With
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

End Class
