Imports System.Net.Http
Imports Newtonsoft.Json

Public Class Form1

    Private listMasterJurusan As New List(Of Jurusan)()
    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadData()
        LoadJurusan()
    End Sub

    Private Async Sub LoadData()
        Using client As New HttpClient()
            Try
                Dim response = Await client.GetAsync("http://localhost:8081/api/mahasiswa")
                If response.IsSuccessStatusCode Then
                    Dim jsonString As String = Await response.Content.ReadAsStringAsync()
                    Dim data = JsonConvert.DeserializeObject(Of List(Of Mahasiswa))(jsonString)

                    DataGridView1.DataSource = data

                    If DataGridView1.Columns.Contains("id") Then
                        DataGridView1.Columns("id").Visible = False
                    End If
                End If
            Catch ex As Exception
            End Try
        End Using
    End Sub

    Private Async Function LoadJurusan() As Task
        Using client As New HttpClient()
            Try
                Dim response = Await client.GetAsync("http://localhost:8081/api/jurusan")
                If response.IsSuccessStatusCode Then
                    Dim jsonString = Await response.Content.ReadAsStringAsync()
                    listMasterJurusan = JsonConvert.DeserializeObject(Of List(Of Jurusan))(jsonString)

                    Dim daftarJurusan = listMasterJurusan.Select(Function(x) x.namaJurusan).Distinct().ToList()
                    Dim daftarFakultas = listMasterJurusan.Select(Function(x) x.fakultas).Distinct().ToList()
                    Dim daftarJenjang = listMasterJurusan.Select(Function(x) x.jenjang).Distinct().ToList()

                    ComboBox1.DataSource = daftarJurusan
                    ComboBox2.DataSource = daftarFakultas
                    ComboBox3.DataSource = daftarJenjang

                    ComboBox1.SelectedIndex = -1
                    ComboBox2.SelectedIndex = -1
                    ComboBox3.SelectedIndex = -1
                End If
            Catch ex As Exception
                MessageBox.Show("Gagal memuat data master jurusan: " & ex.Message)
            End Try
        End Using
    End Function

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click

    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label14_Click(sender As Object, e As EventArgs) Handles Label14.Click

    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click

    End Sub

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If String.IsNullOrWhiteSpace(TextBox1.Text) OrElse ComboBox1.SelectedIndex = -1 OrElse ComboBox2.SelectedIndex = -1 OrElse ComboBox3.SelectedIndex = -1 Then
            MessageBox.Show("Mohon lengkapi semua data dan pilihan!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim jurusanTerpilih = listMasterJurusan.FirstOrDefault(Function(x) x.namaJurusan = ComboBox1.Text AndAlso x.fakultas = ComboBox2.Text AndAlso x.jenjang = ComboBox3.Text)

        If jurusanTerpilih Is Nothing Then
            MessageBox.Show("Kombinasi Jurusan, Fakultas, dan Jenjang tersebut tidak valid/tidak ada di data master jurusan!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim mhs As New Mahasiswa With {
            .nama = TextBox1.Text,
            .umur = If(IsNumeric(TextBox2.Text), Convert.ToInt32(TextBox2.Text), 0),
            .nim = TextBox3.Text,
            .tglLahir = TextBox4.Text,
            .alamat = TextBox5.Text,
            .idJurusan = jurusanTerpilih.idJurusan
        }

        Dim jsonString As String = JsonConvert.SerializeObject(mhs)
        Dim content As New StringContent(jsonString, System.Text.Encoding.UTF8, "application/json")

        Using client As New HttpClient()
            Try
                Dim response = Await client.PostAsync("http://localhost:8081/api/mahasiswa", content)
                If response.IsSuccessStatusCode Then
                    MessageBox.Show("Data Mahasiswa berhasil disimpan!")
                    TextBox1.Clear()
                    TextBox2.Clear()
                    TextBox3.Clear()
                    TextBox4.Clear()
                    TextBox5.Clear()
                    ComboBox1.SelectedIndex = -1
                    ComboBox2.SelectedIndex = -1
                    ComboBox3.SelectedIndex = -1
                    LoadData()
                Else
                    MessageBox.Show("Gagal menyimpan. Kode: " & response.StatusCode)
                End If
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub TextBox9_TextChanged(sender As Object, e As EventArgs) Handles TextBox9.TextChanged

    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click

    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click

    End Sub

    Private Sub Label17_Click(sender As Object, e As EventArgs) Handles Label17.Click

    End Sub
End Class

Public Class Mahasiswa
    Public Property id As Integer?
    Public Property nama As String
    Public Property umur As Integer
    Public Property nim As String
    Public Property tglLahir As String
    Public Property alamat As String
    Public Property idJurusan As Integer
End Class

Public Class Jurusan
    Public Property idJurusan As Integer
    Public Property namaJurusan As String
    Public Property fakultas As String
    Public Property jenjang As String
End Class