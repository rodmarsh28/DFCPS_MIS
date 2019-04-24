Imports System.Data.SqlClient

Public Class Account_Class
    Public searchValue As String
    Public accountNo As String
    Public header As String
    Public subheader As String
    Public dept As String
    Public accDesc As String

    Public AccName As String
    Public inventorable As Boolean
    Public buyable As Boolean
    Public sellable As Boolean
    Public costAcc As String
    Public incomeAcc As String
    Public assetAcc As String
    Public Sub get_accountInfo()
        Try
            checkConn()
            Dim cmd As New SqlCommand("get_AccountInfo", conn)
            With cmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.AddWithValue("@searchValue", SqlDbType.VarChar).Value = searchValue
            End With
            Dim reader As SqlDataReader = cmd.ExecuteReader
            If reader.Read Then
                accountNo = reader.Item(0)
                header = reader.Item(1)
                subheader = reader.Item(2)
                dept = reader.Item(3)
                accDesc = reader.Item(4)
            Else
                accountNo = ""
                header = ""
                subheader = ""
                dept = ""
                accDesc = ""
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
    End Sub
    Public Sub get_itemAccountInfo()
        Try
            checkConn()
            Dim cmd As New SqlCommand("select buyable,sellable,inventorable,costofsalesAcc,incomeAcc,assetAcc from tblInvtry where itemcode = '" & searchValue & "'", conn)
            Dim dr As SqlDataReader = cmd.ExecuteReader
            If dr.Read Then
                If dr.Item(0) = 1 Then
                    buyable = True
                Else
                    buyable = False
                End If
                If dr.Item(1) = 1 Then
                    sellable = True
                Else
                    sellable = False
                End If
                If dr.Item(2) = 1 Then
                    inventorable = True
                Else
                    inventorable = False
                End If
                costAcc = dr.Item(3)
                incomeAcc = dr.Item(4)
                assetAcc = dr.Item(5)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
    End Sub
    Public Sub getaccountName()
        Try
            checkConn()
            Dim cmd As New SqlCommand("select accountName from tblCOA where accNo = '" & searchValue & "'", conn)
            Dim dr As SqlDataReader = cmd.ExecuteReader
            If dr.Read Then
                accName = dr.Item(0)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
        End Try
    End Sub
End Class
