package com.technicaltest.backend.controller;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.technicaltest.backend.model.Jurusan;
import com.technicaltest.backend.repository.JurusanRepository;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;

@RestController
@RequestMapping("/api/jurusan")
@Tag(name = "Jurusan", description = "API untuk mengambil daftar pilihan Jurusan")
public class JurusanController {

    @Autowired
    private JurusanRepository repo;

    @Operation(summary = "Dapatkan semua data jurusan")
    @GetMapping
    public List<Jurusan> getAll() {
        return repo.findAll();
    }
}