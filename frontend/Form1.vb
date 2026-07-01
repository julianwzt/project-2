Imports System.IO
Imports System.Net.Http
Imports Newtonsoft.Json

Public Class Form1

    Private listMasterJurusan As New List(Of Jurusan)()
    Private selectedMahasiswaId As Integer = 0
    Private listDataTampil As New List(Of MahasiswaTampil)()

    Private Sub ClearForm()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        DateTimePicker1.Value = DateTime.Now
        TextBox5.Clear()
        ComboBox1.SelectedIndex = -1
        ComboBox2.SelectedIndex = -1
        ComboBox3.SelectedIndex = -1
        selectedMahasiswaId = 0
    End Sub
    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadDropdownData()
        LoadData()
    End Sub

    Private Async Function LoadDropdownData() As Task
        Using client As New HttpClient()
            Try
                Dim response = Await client.GetAsync("http://localhost:8081/api/jurusan")
                If response.IsSuccessStatusCode Then
                    Dim jsonString = Await response.Content.ReadAsStringAsync()
                    listMasterJurusan = JsonConvert.DeserializeObject(Of List(Of Jurusan))(jsonString)

                    ComboBox1.DataSource = listMasterJurusan.Select(Function(x) x.namaJurusan).Distinct().ToList()
                    ComboBox2.DataSource = listMasterJurusan.Select(Function(x) x.fakultas).Distinct().ToList()
                    ComboBox3.DataSource = listMasterJurusan.Select(Function(x) x.jenjang).Distinct().ToList()

                    ComboBox1.SelectedIndex = -1
                    ComboBox2.SelectedIndex = -1
                    ComboBox3.SelectedIndex = -1
                End If
            Catch ex As Exception
            End Try
        End Using
    End Function
    Private Async Sub LoadData()
        Using client As New HttpClient()
            Try
                Dim response = Await client.GetAsync("http://localhost:8081/api/mahasiswa")
                If response.IsSuccessStatusCode Then
                    Dim jsonString As String = Await response.Content.ReadAsStringAsync()
                    Dim data = JsonConvert.DeserializeObject(Of List(Of Mahasiswa))(jsonString)

                    If listMasterJurusan IsNot Nothing AndAlso listMasterJurusan.Count > 0 Then
                        listDataTampil = (From m In data
                                          Join j In listMasterJurusan On m.idJurusan Equals j.idJurusan
                                          Select New MahasiswaTampil With {
                                              .Nama = m.nama,
                                              .NIM = m.nim,
                                              .Jurusan = j.namaJurusan,
                                              .Fakultas = j.fakultas,
                                              .Jenjang = j.jenjang,
                                              .Umur = m.umur,
                                              .TglLahir = m.tglLahir,
                                              .Alamat = m.alamat,
                                              .ID = m.id
                                          }).ToList()

                        DataGridView1.DataSource = listDataTampil
                        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                        DataGridView1.ReadOnly = True
                        DataGridView1.AllowUserToAddRows = False

                        If DataGridView1.Columns.Contains("ID") Then
                            DataGridView1.Columns("ID").Visible = False
                        End If
                    End If
                End If
            Catch ex As Exception
            End Try
        End Using
    End Sub

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
            .umur = Convert.ToInt32(TextBox2.Text),
            .nim = TextBox3.Text,
            .tglLahir = DateTimePicker1.Value.ToString("yyyy-MM-dd"),
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
                    ClearForm()
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
        If listDataTampil IsNot Nothing AndAlso listDataTampil.Count > 0 Then
            Dim keyword As String = TextBox9.Text.ToLower()

            Dim filteredData = listDataTampil.Where(Function(x) x.Nama.ToLower().Contains(keyword) OrElse x.NIM.Contains(keyword)).ToList()

            DataGridView1.DataSource = filteredData
        End If
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

    Private Sub DataGridView1_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = DataGridView1.Rows(e.RowIndex)

            selectedMahasiswaId = Convert.ToInt32(row.Cells("ID").Value)

            TextBox1.Text = row.Cells("Nama").Value?.ToString()
            TextBox3.Text = row.Cells("NIM").Value?.ToString()
            TextBox2.Text = row.Cells("Umur").Value?.ToString()
            TextBox5.Text = row.Cells("Alamat").Value?.ToString()

            ComboBox1.Text = row.Cells("Jurusan").Value?.ToString()
            ComboBox2.Text = row.Cells("Fakultas").Value?.ToString()
            ComboBox3.Text = row.Cells("Jenjang").Value?.ToString()

            Dim tglStr = row.Cells("TglLahir").Value?.ToString()
            Dim tglDate As DateTime
            If DateTime.TryParse(tglStr, tglDate) Then
                DateTimePicker1.Value = tglDate
            Else
                DateTimePicker1.Value = DateTime.Now
            End If
        End If
    End Sub

    Private Async Sub ButtonUpdate_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If selectedMahasiswaId = 0 Then
            MessageBox.Show("Silakan klik/pilih data di tabel terlebih dahulu!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim jurusanTerpilih = listMasterJurusan.FirstOrDefault(Function(x) x.namaJurusan = ComboBox1.Text AndAlso x.fakultas = ComboBox2.Text AndAlso x.jenjang = ComboBox3.Text)

        If jurusanTerpilih Is Nothing Then
            MessageBox.Show("Kombinasi Jurusan, Fakultas, dan Jenjang tidak valid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim mhs As New Mahasiswa With {
            .nama = TextBox1.Text,
            .umur = Convert.ToInt32(TextBox2.Text),
            .nim = TextBox3.Text,
            .tglLahir = DateTimePicker1.Value.ToString("yyyy-MM-dd"),
            .alamat = TextBox5.Text,
            .idJurusan = jurusanTerpilih.idJurusan
        }

        Dim jsonString As String = JsonConvert.SerializeObject(mhs)
        Dim content As New StringContent(jsonString, System.Text.Encoding.UTF8, "application/json")

        Using client As New HttpClient()
            Try
                Dim response = Await client.PutAsync($"http://localhost:8081/api/mahasiswa/{selectedMahasiswaId}", content)
                If response.IsSuccessStatusCode Then
                    MessageBox.Show("Data Mahasiswa berhasil diperbarui!")
                    ClearForm()
                    LoadData()
                Else
                    MessageBox.Show("Gagal update data.")
                End If
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Async Sub ButtonDelete_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If selectedMahasiswaId = 0 Then
            MessageBox.Show("Silakan klik/pilih data di tabel yang ingin dihapus!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim confirm = MessageBox.Show("Apakah Anda yakin ingin menghapus data ini?", "Konfirmasi Hapus", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If confirm = DialogResult.Yes Then
            Using client As New HttpClient()
                Try
                    Dim response = Await client.DeleteAsync($"http://localhost:8081/api/mahasiswa/{selectedMahasiswaId}")
                    If response.IsSuccessStatusCode Then
                        MessageBox.Show("Data berhasil dihapus dari Database!")
                        ClearForm()
                        LoadData()
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub

    Private Async Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("Tidak ada data untuk diekspor!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim sfd As New SaveFileDialog()
        sfd.Filter = "Excel Files (*.xlsx)|*.xlsx"

        If sfd.ShowDialog() = DialogResult.OK Then
            Using client As New HttpClient()
                Dim response = Await client.GetAsync("http://localhost:8081/api/mahasiswa/export")
                If response.IsSuccessStatusCode Then
                    Dim fileBytes = Await response.Content.ReadAsByteArrayAsync()
                    System.IO.File.WriteAllBytes(sfd.FileName, fileBytes)
                    MessageBox.Show("Berhasil download Excel dari Server!")
                End If
            End Using
        End If
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        Dim dob As DateTime = DateTimePicker1.Value
        Dim age As Integer = DateTime.Now.Year - dob.Year

        If DateTime.Now < dob.AddYears(age) Then age -= 1

        TextBox2.Text = Math.Max(0, age).ToString()
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedIndex <> -1 AndAlso listMasterJurusan IsNot Nothing Then
            Dim selectedJurusan = ComboBox1.Text

            Dim fakultasTerkait = listMasterJurusan.Where(Function(x) x.namaJurusan = selectedJurusan).Select(Function(x) x.fakultas).Distinct().ToList()
            ComboBox2.DataSource = fakultasTerkait
            If fakultasTerkait.Count = 1 Then ComboBox2.SelectedIndex = 0

            Dim jenjangTersedia = listMasterJurusan.Where(Function(x) x.namaJurusan = selectedJurusan).Select(Function(x) x.jenjang).Distinct().ToList()
            ComboBox3.DataSource = jenjangTersedia
            ComboBox3.SelectedIndex = -1
        End If
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

Public Class MahasiswaTampil
    Public Property ID As Integer
    Public Property Nama As String
    Public Property NIM As String
    Public Property Jurusan As String
    Public Property Fakultas As String
    Public Property Jenjang As String
    Public Property Umur As Integer
    Public Property TglLahir As String
    Public Property Alamat As String
End Class