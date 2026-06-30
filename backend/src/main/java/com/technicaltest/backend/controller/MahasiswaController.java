package com.technicaltest.backend.controller;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.technicaltest.backend.model.Mahasiswa;
import com.technicaltest.backend.repository.MahasiswaRepository;

import io.swagger.v3.oas.annotations.Operation;

@RestController
@RequestMapping("/api/mahasiswa")
public class MahasiswaController {

    @Autowired
    private MahasiswaRepository repo;

    @GetMapping
    @Operation(summary = "Dapatkan daftar mahasiswa", description = "Mengambil semua data mahasiswa dari PostgreSQL")
    public List<Mahasiswa> getAll() {
        return repo.findAll();
    }

    @PostMapping
    @Operation(summary = "Tambah mahasiswa baru")
    public Mahasiswa create(@RequestBody Mahasiswa m) {
        return repo.save(m);
    }
}