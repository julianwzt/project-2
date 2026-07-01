package com.technicaltest.backend.controller;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.util.List;
import java.util.Map;
import java.util.function.Function;
import java.util.stream.Collectors;

import org.apache.poi.ss.usermodel.Row;
import org.apache.poi.ss.usermodel.Sheet;
import org.apache.poi.ss.usermodel.Workbook;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.technicaltest.backend.model.Jurusan;
import com.technicaltest.backend.model.Mahasiswa;
import com.technicaltest.backend.repository.JurusanRepository;
import com.technicaltest.backend.repository.MahasiswaRepository;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.tags.Tag;

@RestController
@RequestMapping("/api/mahasiswa")
@CrossOrigin(origins = "*")
@Tag(name = "Mahasiswa", description = "API untuk mengelola data mahasiswa")
public class MahasiswaController {

    @Autowired
    private MahasiswaRepository repo;

    @Autowired
    private JurusanRepository jurusanRepo;

    @Operation(summary = "Dapatkan semua data mahasiswa")
    @ApiResponse(responseCode = "200", description = "Berhasil mengambil data mahasiswa")
    @ApiResponse(responseCode = "404", description = "Data mahasiswa tidak ditemukan")
    @GetMapping
    public List<Mahasiswa> getAll() {
        return repo.findAll();
    }

    @Operation(summary = "Mengunduh Data Excel", description = "Mengekspor seluruh data mahasiswa menjadi file .xlsx")
    @ApiResponse(responseCode = "200", description = "Berhasil mengunduh data mahasiswa dalam format Excel")
    @GetMapping("/export")
    public ResponseEntity<byte[]> exportMahasiswaExcel() throws IOException {
        List<Mahasiswa> list = repo.findAll();
        Map<Long, Jurusan> jurusanMap = jurusanRepo.findAll().stream()
                .collect(Collectors.toMap(Jurusan::getIdJurusan, Function.identity()));

        try (Workbook workbook = new XSSFWorkbook(); ByteArrayOutputStream bos = new ByteArrayOutputStream()) {
            Sheet sheet = workbook.createSheet("Data Mahasiswa");
            Row header = sheet.createRow(0);
            header.createCell(0).setCellValue("Nama");
            header.createCell(1).setCellValue("NIM");
            header.createCell(2).setCellValue("Umur");
            header.createCell(3).setCellValue("Tanggal Lahir");
            header.createCell(4).setCellValue("Alamat");
            header.createCell(5).setCellValue("Jurusan");
            header.createCell(6).setCellValue("Fakultas");
            header.createCell(7).setCellValue("Jenjang");

            for (int i = 0; i < list.size(); i++) {
                Mahasiswa mhs = list.get(i);
                Row row = sheet.createRow(i + 1);
                Jurusan jurusan = jurusanMap.get(mhs.getIdJurusan());
                String namaJurusan = jurusan != null ? jurusan.getNamaJurusan() : "";
                String fakultas = jurusan != null ? jurusan.getFakultas() : "";
                String jenjang = jurusan != null ? jurusan.getJenjang() : "";

                row.createCell(0).setCellValue(mhs.getNama() != null ? mhs.getNama() : "");
                row.createCell(1).setCellValue(mhs.getNim() != null ? mhs.getNim() : "");
                row.createCell(2).setCellValue(mhs.getUmur() != null ? mhs.getUmur() : 0);
                row.createCell(3).setCellValue(mhs.getTglLahir() != null ? mhs.getTglLahir() : "");
                row.createCell(4).setCellValue(mhs.getAlamat() != null ? mhs.getAlamat() : "");
                row.createCell(5).setCellValue(namaJurusan);
                row.createCell(6).setCellValue(fakultas);
                row.createCell(7).setCellValue(jenjang);
            }

            workbook.write(bos);
            byte[] excelContent = bos.toByteArray();

            HttpHeaders headers = new HttpHeaders();
            headers.setContentType(MediaType.APPLICATION_OCTET_STREAM);
            headers.setContentDispositionFormData("attachment", "Data_Mahasiswa.xlsx");

            return new ResponseEntity<>(excelContent, headers, HttpStatus.OK);
        }
    }

    @Operation(summary = "Save data mahasiswa baru")
    @ApiResponse(responseCode = "200", description = "Berhasil menyimpan data mahasiswa baru")
    @ApiResponse(responseCode = "400", description = "Permintaan tidak valid")
    @PostMapping
    public Mahasiswa create(@RequestBody Mahasiswa mhs) {
        return repo.save(mhs);
    }

    @Operation(summary = "Perbarui data mahasiswa berdasarkan ID")
    @ApiResponse(responseCode = "200", description = "Berhasil memperbarui data mahasiswa")
    @ApiResponse(responseCode = "404", description = "Data mahasiswa tidak ditemukan")
    @PutMapping("/{id}")
    public ResponseEntity<Mahasiswa> update(@PathVariable Long id, @RequestBody Mahasiswa mhsDetails) {
        return repo.findById(id).map(mhs -> {
            mhs.setNama(mhsDetails.getNama());
            mhs.setUmur(mhsDetails.getUmur());
            mhs.setNim(mhsDetails.getNim());
            mhs.setTglLahir(mhsDetails.getTglLahir());
            mhs.setAlamat(mhsDetails.getAlamat());
            mhs.setIdJurusan(mhsDetails.getIdJurusan());
            return ResponseEntity.ok(repo.save(mhs));
        }).orElse(ResponseEntity.notFound().build());
    }

    @Operation(summary = "Hapus data mahasiswa berdasarkan ID")
    @ApiResponse(responseCode = "200", description = "Berhasil menghapus data mahasiswa")
    @ApiResponse(responseCode = "404", description = "Data mahasiswa tidak ditemukan")
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> delete(@PathVariable Long id) {
        if (repo.existsById(id)) {
            repo.deleteById(id);
            return ResponseEntity.ok().build();
        }
        return ResponseEntity.notFound().build();
    }
}